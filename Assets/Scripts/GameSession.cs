using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLifes = 3;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    int score = 0;

    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        livesText.text = "Lives: " + playerLifes.ToString();
        scoreText.text = "Score: " + score.ToString();
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
        livesText.text = "Lives: " + playerLifes.ToString();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CoinPickup()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();
    }

    public int GetScore()
    {
        return score;
    }
}
