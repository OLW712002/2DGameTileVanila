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

    float playerGravityScaleAtStart;

    [SerializeField] float playerSpeed = 3.0f;
    [SerializeField] float playerJumpForce = 5.0f;
    [SerializeField] float playerClimbSpeed = 5.0f;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        playerGravityScaleAtStart = myRb.gravityScale;
    }

    void Update()
    {
        Run();
        Climb();
        ChangeDirection();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
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
}
