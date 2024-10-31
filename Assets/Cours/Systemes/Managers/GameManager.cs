using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private float timeOfStart;
    private bool gameOver = false;

    void Start()
    {
        HealthManager.Instance.OnHealthChanged += CheckHealth;
        timeOfStart = Time.time;
    }
    
    private void CheckHealth(float newHealth)
    {
        if (newHealth <= 0)
        {
            GameLost();
        }
    }

    private void GameWon()
    {
        Debug.Log("GAME WON");
        HighScoreService.instance.SendGameDuration(Time.time - timeOfStart);
        RestartGame();
    }

    private void GameLost()
    {
        HealthManager.Instance.OnHealthChanged -= CheckHealth;
        gameOver = true;
        Debug.Log("GAME OVER");
        RestartGame();
    }


    private void Update()
    {
        if (gameOver)
            return;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
