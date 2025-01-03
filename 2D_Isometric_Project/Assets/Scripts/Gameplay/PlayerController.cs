using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Flags]
public enum MovementKeys : byte
{
    None = 0,
    W = 1 << 0, // 0001
    A = 1 << 1, // 0010
    S = 1 << 2, // 0100
    D = 1 << 3  // 1000
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private LayerMask invalidLayers;  
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float rayLength; 

    [Header("Box related")]
    [SerializeField] private Transform interactableTrigger;
    [SerializeField] private Transform pickableBoxTransform;
    [SerializeField] private GameObject movableBoxParent;
    [SerializeField] private GameObject previewBoxPrefab;

    private Vector3Int currentTilePosition;
    private Vector3 currentGhostBoxPosition;
    private Vector3 currentGroundPosition;
    private Vector2 currentDirection;
    private Vector2 lastMoveDirection;

    private MovementKeys currentKeyPressed = MovementKeys.None;
    private PickableBox currentPickableBox = null;
    private PickableBox interactableBox = null; // Box currently in range
    private GameObject previewBoxInstance = null; // Reference to the instantiated ghost box
    
    private void Awake() 
    {
        lastMoveDirection = new Vector2(0.1f, -0.05f);
        currentGhostBoxPosition = Vector3.negativeInfinity;
    }

    private void Update()
    {
        ProcessInputs();
        HandleSpriteFlipX();
        AnimateSprite();

        if (currentPickableBox != null) // If player is holding a box
        {
            UpdateCurrentTilePosition();
            UpdateGhostBox();
        }
        else
        {
            DestroyGhostBox();
        }
    }

    private void FixedUpdate()
    {
        body.velocity = currentDirection * moveSpeed;
    }

    private void ProcessInputs()
    {
        if (!GameManager.Instance.IsGamePaused)
        {
            ProcessPlayerMovement();
            ProcessPlayerActions();
        }
        else
        {
            currentDirection = Vector2.zero;
        }
    }

    #region Player Movement
    private void ProcessPlayerMovement()
    {
        currentDirection = Vector2.zero;

        // Handle key releases to reset the lock
        HandleKeyReleases();

        // Assign the first key pressed
        AssignKeyPressed();

        // Perform movement based on the locked key
        PerformMovement();

        // Store the last move direction when moving
        if (currentDirection != Vector2.zero)
        {
            lastMoveDirection = currentDirection;
        }

        interactableTrigger.localPosition = new Vector3(lastMoveDirection.x, lastMoveDirection.y, 0) * 0.5f;
    }

    private void HandleKeyReleases()
    {
        if (Input.GetKeyUp(KeyCode.W) && (currentKeyPressed & MovementKeys.W) != 0)
        {
            currentKeyPressed = MovementKeys.None;
        }
        else if (Input.GetKeyUp(KeyCode.S) && (currentKeyPressed & MovementKeys.S) != 0)
        {
            currentKeyPressed = MovementKeys.None;
        }
        else if (Input.GetKeyUp(KeyCode.A) && (currentKeyPressed & MovementKeys.A) != 0)
        {
            currentKeyPressed = MovementKeys.None;
        }
        else if (Input.GetKeyUp(KeyCode.D) && (currentKeyPressed & MovementKeys.D) != 0)
        {
            currentKeyPressed = MovementKeys.None;
        }
    }

    private void AssignKeyPressed()
    {
        if (currentKeyPressed == MovementKeys.None)
        {
            if (Input.GetKey(KeyCode.W))
            {
                currentKeyPressed |= MovementKeys.W;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                currentKeyPressed |= MovementKeys.S;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                currentKeyPressed |= MovementKeys.A;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                currentKeyPressed |= MovementKeys.D;
            }
        }
    }

    private void PerformMovement()
    {
        if ((currentKeyPressed & MovementKeys.W) != 0 && Input.GetKey(KeyCode.W))
        {
            currentDirection = new Vector2(0.1f, 0.05f);
        }
        else if ((currentKeyPressed & MovementKeys.S) != 0 && Input.GetKey(KeyCode.S))
        {
            currentDirection = new Vector2(-0.1f, -0.05f);
        }
        else if ((currentKeyPressed & MovementKeys.A) != 0 && Input.GetKey(KeyCode.A))
        {
            currentDirection = new Vector2(-0.1f, 0.05f);
        }
        else if ((currentKeyPressed & MovementKeys.D) != 0 && Input.GetKey(KeyCode.D))
        {
            currentDirection = new Vector2(0.1f, -0.05f);
        }
    }

    private void HandleSpriteFlipX()
    {
        if (currentDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (currentDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void AnimateSprite()
    {
        animator.SetFloat("MoveX", Mathf.Abs(currentDirection.x));
        animator.SetFloat("MoveY", currentDirection.y);
        animator.SetFloat("MoveMagnitude", currentDirection.magnitude);
        animator.SetFloat("LastMoveX", Mathf.Abs(lastMoveDirection.x));
        animator.SetFloat("LastMoveY", lastMoveDirection.y);
    }
    #endregion

    #region Player Actions
    private void ProcessPlayerActions()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentPickableBox == null) // Pick up a box
            {
                if (interactableBox != null)
                {
                    PickUpBox(interactableBox);
                }
            }
            else // Drop the box
            {
                DropBox();
            }
        }
    }

    private void PickUpBox(PickableBox box)
    {
        SoundManager.Instance.PlaySFX(SFXType.PickBox);

        currentPickableBox = box;
        currentPickableBox.transform.SetParent(pickableBoxTransform);
        currentPickableBox.transform.localPosition = Vector3.zero;
        currentPickableBox.transform.localScale *= 0.5f;
        currentPickableBox.HighlightBox(false);
        currentPickableBox.GetComponent<SpriteRenderer>().sortingOrder += 1;
        currentPickableBox.GetComponent<Collider2D>().enabled = false;
    }

    private void DropBox()
    {
        // Place the box at the ghost box position
        if (IsPositionValid(currentGhostBoxPosition))
        {
            SoundManager.Instance.PlaySFX(SFXType.DropBox);

            // Place the box
            currentPickableBox.transform.SetParent(movableBoxParent.transform);
            currentPickableBox.transform.localPosition = currentGhostBoxPosition;
            currentPickableBox.transform.localScale = Vector3.one;
            currentPickableBox.GetComponent<SpriteRenderer>().sortingOrder = 0;
            currentPickableBox.GetComponent<Collider2D>().enabled = true;
            currentPickableBox = null;

            // Reset preview box position
            currentGhostBoxPosition = Vector3.negativeInfinity;
        }
    }

    private bool IsPositionValid(Vector3 position)
    {
        Vector2 targetPosition = new Vector2(interactableTrigger.position.x, interactableTrigger.position.y + 0.08f);

        // Perform the raycast from the player's position in the current direction
        RaycastHit2D hit = Physics2D.Raycast(targetPosition, lastMoveDirection.normalized, rayLength, invalidLayers);

        Debug.DrawRay(targetPosition, lastMoveDirection.normalized * rayLength, Color.red, 0.1f);

        if (hit.collider != null)
        {
            return false;
        }

        return true; // Valid position
    }

    private void UpdateGhostBox()
    {
        // Calculate the position for the ghost box
        Vector3 valueToAdd = Vector3.zero;

        if (lastMoveDirection == new Vector2(0.1f, 0.05f))
        {
            valueToAdd = new Vector3(0.08f, 0.04f, 0);
        }
        else if (lastMoveDirection == new Vector2(-0.1f, 0.05f))
        {
            valueToAdd = new Vector3(-0.08f, 0.04f, 0);
        }
        else if (lastMoveDirection == new Vector2(0.1f, -0.05f))
        {
            valueToAdd = new Vector3(0.08f, -0.04f, 0);
        }
        else if (lastMoveDirection == new Vector2(-0.1f, -0.05f))
        {
            valueToAdd = new Vector3(-0.08f, -0.04f, 0);
        }

        Vector3 targetPosition = currentGroundPosition + valueToAdd;

        if (!IsPositionValid(targetPosition))
        {
            DestroyGhostBox();
            return;
        }

        if (previewBoxInstance == null)
        {
            // Instantiate the ghost box prefab when the player starts carrying a box
            previewBoxInstance = Instantiate(previewBoxPrefab);
        }

        previewBoxInstance.transform.position = targetPosition;
        currentGhostBoxPosition = targetPosition;
    }

    private void DestroyGhostBox()
    {
        if (previewBoxInstance != null)
        {
            Destroy(previewBoxInstance.gameObject); // Clean up the ghost box when no longer needed
            previewBoxInstance = null;
        }
    }

    private void UpdateCurrentTilePosition()
    {
        // Convert the player's world position to a grid position
        currentTilePosition = groundTilemap.WorldToCell(transform.position);

        // Get the center position of the tile in world space
        currentGroundPosition = groundTilemap.GetCellCenterWorld(currentTilePosition);
        currentGroundPosition.y += 0.08f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PickableBox") && currentPickableBox == null)
        {
            if (interactableBox != null)
            {
                interactableBox.HighlightBox(false);
                interactableBox = null;
            }
            
            interactableBox = other.GetComponent<PickableBox>();
            interactableBox.HighlightBox(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PickableBox") && interactableBox != null && currentPickableBox == null)
        {
            interactableBox.HighlightBox(false);
            interactableBox = null;
        }
    }
    #endregion
}