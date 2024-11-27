using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;

    Rigidbody2D myRb;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    SpriteRenderer sprite;

    float playerGravityScaleAtStart;
    float playerJumpForceAtStart;
    bool isAlive = true;
    bool isInWater = false;
    int jumpCount = 1;

    [SerializeField] float playerSpeed = 3.0f;
    [SerializeField] float playerJumpForce = 5.0f;
    [SerializeField] float playerJumpForceInWater = 1.0f;
    [SerializeField] float playerClimbSpeed = 5.0f;
    [SerializeField] GameObject bullet;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        playerGravityScaleAtStart = myRb.gravityScale;
        playerJumpForceAtStart = playerJumpForce;
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
        if (value.isPressed && jumpCount > 0)
        {
            myRb.velocity = new Vector2(0f, playerJumpForce);
            jumpCount--;
        }
    }
    
    void OnFire(InputValue value)
    {
        if (!isAlive) return;
        if (value.isPressed)
        {
            GameObject gun = transform.Find("Gun").gameObject;
            Instantiate(bullet, gun.transform.position, Quaternion.identity);
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
        else if (!isInWater)
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
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            Debug.Log("enterwater");
            isInWater = true;
            jumpCount = 999;
            myRb.gravityScale = 0.5f;
            playerJumpForce = playerJumpForceInWater;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            Debug.Log("exitwater");
            jumpCount = 0;
            isInWater = false;
            playerJumpForce = playerJumpForceAtStart;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platform")) && jumpCount == 0) { jumpCount = 1; Debug.Log("resetjumpcount"); }
    }
}
