using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLifes = 3;
    int score = 0;

    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
    }

    public void ProcessPlayerDeath()
    {
        if (playerLifes > 1) Invoke("TakeLife", 0.5f);
        else Invoke("ResetGameSession", 0.5f);
    }

    void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    void TakeLife()
    {
        playerLifes--;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CoinPickup()
    {
        score++;
    }

    public int GetScore()
    {
        return score;
    }
}
