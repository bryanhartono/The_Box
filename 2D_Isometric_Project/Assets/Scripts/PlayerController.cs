using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D body;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public float walkSpeed;
    
    Vector2 currentDirection;
    Vector2 lastMoveDirection;
    float idleTime;

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
        HandleSpriteFlipX();
        AnimateSprite();
    }

    void FixedUpdate() 
    {
        body.velocity = currentDirection * walkSpeed;    
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical") * 0.5f;

        if ((moveX == 0 && moveY == 0) && (currentDirection.x != 0 || currentDirection.y != 0))
        {
            lastMoveDirection = currentDirection;
        }

        currentDirection = new Vector2(moveX, moveY).normalized;
    }

    void HandleSpriteFlipX()
    {
        if (!spriteRenderer.flipX && currentDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (spriteRenderer.flipX && currentDirection.x > 0)
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
