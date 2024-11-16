using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.0f;

    Rigidbody2D myRb;
    BoxCollider2D myFeetCollider;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        myRb.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        FlipSlimeFacing();
    }

    void FlipSlimeFacing()
    {
        transform.localScale = new Vector2(-Mathf.Sign(myRb.velocity.x), 1f);
    }
}
