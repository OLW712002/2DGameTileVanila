using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPortal : MonoBehaviour
{
    [SerializeField] float delayTime = 1.0f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(NextLevel());
        }
    }

    IEnumerator NextLevel()
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex + "," + SceneManager.sceneCountInBuildSettings);
        yield return new WaitForSecondsRealtime(delayTime);
        if (SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0);
            Debug.Log("Congratulation, your score: " + FindObjectOfType<GameSession>().GetScore());
        }
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
