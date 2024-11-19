using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;

    Rigidbody2D myRb;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    SpriteRenderer spriteRenderer;

    float playerGravityScaleAtStart;
    bool isAlive = true;

    [SerializeField] float playerSpeed = 3.0f;
    [SerializeField] float playerJumpForce = 5.0f;
    [SerializeField] float playerClimbSpeed = 5.0f;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerGravityScaleAtStart = myRb.gravityScale;
    }

    void Update()
    {
        if (!isAlive) return;
        Run();
        Climb();
        ChangeDirection();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) return;
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) return;
        if (value.isPressed && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platform"))) myRb.velocity = new Vector2(0f, playerJumpForce);
    }    

    void Run()
    {
        //Running logical
        Vector2 playerVelocity = new Vector2(moveInput.x * playerSpeed, myRb.velocity.y);
        myRb.velocity = playerVelocity;

        //Running animation
        bool playerHasHorizontalSpeed = Mathf.Abs(myRb.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void Climb()
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myRb.velocity = new Vector2(myRb.velocity.x, moveInput.y * playerClimbSpeed);
            myRb.gravityScale = 0f;
            if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platform")))
            {
                myAnimator.SetBool("isClimbing", true);
                if (moveInput == Vector2.zero) myAnimator.speed = 0;
                else myAnimator.speed = 1;
            }
            else myAnimator.SetBool("isClimbing", false);
        }
        else
        {
            myRb.gravityScale = playerGravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
        }
    }

    void ChangeDirection()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRb.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed) transform.localScale = new Vector2(Mathf.Sign(myRb.velocity.x), 1f);
    }

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazard")))
        {
            isAlive = false;
            myBodyCollider.enabled = false;
            myAnimator.SetTrigger("Dying");
            Invoke("RemovePlayer", 0.5f);
        }
    }

    void RemovePlayer()
    {
        spriteRenderer.color = new Color(0f, 0f, 0f, 0f);
    }
}
