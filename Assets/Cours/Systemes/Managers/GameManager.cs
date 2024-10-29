using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int scoreToWin = 3;
    [SerializeField] private float timeToLoose = 30.0f;
    private float timeOfStart;
    private bool gameOver = false;

    void Start()
    {
        ScoreManager.instance.OnScoreChanged += CheckScore;
        timeOfStart = Time.time;
    }

    private void CheckScore(int newScore)
    {
        if (newScore >= scoreToWin)
        {
            GameWon();
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
        ScoreManager.instance.OnScoreChanged -= CheckScore;
        gameOver = true;
        Debug.Log("GAME OVER");
        RestartGame();
    }


    private void Update()
    {
        if (gameOver)
            return;

        if (Time.time - timeOfStart > timeToLoose)
        {
            GameLost();
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
