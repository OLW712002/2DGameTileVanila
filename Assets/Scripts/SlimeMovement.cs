using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] float boostSpeed = 2.0f;
    [SerializeField] float distanceCheck = 10.0f;

    Rigidbody2D myRb;
    BoxCollider2D myFeetCollider;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        CheckPlayer();
    }

    void CheckPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)myRb.transform.position, (Vector2)myRb.velocity.normalized, distanceCheck, LayerMask.GetMask("Player"));
        Debug.DrawLine((Vector2)myRb.transform.position, (Vector2)myRb.transform.position + myRb.velocity.normalized * distanceCheck, Color.red);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            myRb.velocity = new Vector2(boostSpeed, 0f);
            Debug.Log("DetectPlayer");
        }
        else myRb.velocity = new Vector2(moveSpeed, 0f);
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
