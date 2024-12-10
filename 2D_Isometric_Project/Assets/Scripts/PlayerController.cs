using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Rigidbody2D body;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public float moveSpeed = 1.0f;

    Vector2 currentDirection;
    Vector2 lastMoveDirection;

    MovementKeys currentKeyPressed = MovementKeys.None;

    void Update()
    {
        ProcessInputs();
        HandleSpriteFlipX();
        AnimateSprite();
    }

    void FixedUpdate()
    {
        body.velocity = currentDirection * moveSpeed;
    }

    void ProcessInputs()
    {
        // Initialize movement vector
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
    }

    void HandleKeyReleases()
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

    void AssignKeyPressed()
    {
        if (currentKeyPressed == MovementKeys.None)
        {
            if (Input.GetKey(KeyCode.W))
            {
                currentKeyPressed |= MovementKeys.W; // Lock to W
            }
            else if (Input.GetKey(KeyCode.S))
            {
                currentKeyPressed |= MovementKeys.S; // Lock to S
            }
            else if (Input.GetKey(KeyCode.A))
            {
                currentKeyPressed |= MovementKeys.A; // Lock to A
            }
            else if (Input.GetKey(KeyCode.D))
            {
                currentKeyPressed |= MovementKeys.D; // Lock to D
            }
        }
    }

    void PerformMovement()
    {
        if ((currentKeyPressed & MovementKeys.W) != 0 && Input.GetKey(KeyCode.W))
        {
            currentDirection = new Vector2(0.1f, 0.05f); // Move Northeast
        }
        else if ((currentKeyPressed & MovementKeys.S) != 0 && Input.GetKey(KeyCode.S))
        {
            currentDirection = new Vector2(-0.1f, -0.05f); // Move Southwest
        }
        else if ((currentKeyPressed & MovementKeys.A) != 0 && Input.GetKey(KeyCode.A))
        {
            currentDirection = new Vector2(-0.1f, 0.05f); // Move Northwest
        }
        else if ((currentKeyPressed & MovementKeys.D) != 0 && Input.GetKey(KeyCode.D))
        {
            currentDirection = new Vector2(0.1f, -0.05f); // Move Southeast
        }
    }


    void HandleSpriteFlipX()
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

    void AnimateSprite()
    {
        animator.SetFloat("MoveX", Mathf.Abs(currentDirection.x));
        animator.SetFloat("MoveY", currentDirection.y);
        animator.SetFloat("MoveMagnitude", currentDirection.magnitude);
        animator.SetFloat("LastMoveX", Mathf.Abs(lastMoveDirection.x));
        animator.SetFloat("LastMoveY", lastMoveDirection.y);
    }
}
