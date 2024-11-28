using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip coinCollectedSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(gameObject);
            FindObjectOfType<GameSession>().CoinPickup();
            AudioSource.PlayClipAtPoint(coinCollectedSound, gameObject.transform.position, 1.0f);
        }
    }
}
