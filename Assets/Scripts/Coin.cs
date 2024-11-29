using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip coinCollectedSound;
    bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isCollected)
        {
            isCollected = true;
            Destroy(gameObject);
            FindObjectOfType<GameSession>().CoinPickup();
            AudioSource.PlayClipAtPoint(coinCollectedSound, gameObject.transform.position, 1.0f);
        }
    }
}
