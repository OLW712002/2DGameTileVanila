using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;

    Rigidbody2D myRb;
    Animator myAnimator;
    CapsuleCollider2D myCollider;

    [SerializeField] float playerSpeed = 3.0f;
    [SerializeField] float playerJumpForce = 5.0f;
    [SerializeField] float playerClimbSpeed = 5.0f;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        Run();
        Climb();
        AvoidWallStuck();
        ChangeDirection();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && myCollider.IsTouchingLayers(LayerMask.GetMask("Platform"))) myRb.velocity = new Vector2(0f, playerJumpForce);
    }    

    void OnClimb(InputValue value)
    {
        if (value.isPressed && myCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myRb.velocity = new Vector2(0f, playerClimbSpeed);
            Debug.Log("climb");
        }
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
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) myRb.velocity = new Vector2(myRb.velocity.x, moveInput.y * playerClimbSpeed);
    }

    void ChangeDirection()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRb.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed) transform.localScale = new Vector2(Mathf.Sign(myRb.velocity.x), 1f);
    }

    void AvoidWallStuck()
    {
        float forceAmount = 50.0f;
        float rayCheckWallDistance = myCollider.size.x / 2 + Mathf.Epsilon;
        Vector2 rayDirection = new Vector2(myRb.velocity.x, 0f).normalized;
        LayerMask layerMask = LayerMask.GetMask("Platform");
        Vector2 rayLocation = new Vector2(transform.position.x, transform.position.y - myCollider.size.y / 2);
        if (Physics2D.Raycast(rayLocation, rayDirection, rayCheckWallDistance, layerMask)) myRb.AddForce(-rayDirection * forceAmount);
    }
}
