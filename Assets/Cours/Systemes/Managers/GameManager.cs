using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private float timeOfStart;
    private bool gameOver = false;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;

    void Start()
    {
        PlayerManager playerInstance = PlayerManager.Instance;
        playerInstance.OnPlayerDeath += GameLost;
        playerInstance.OnLevelUp += LevelUp;
        timeOfStart = Time.time;
    }

    private void GameLost()
    {
        float gameTime = Time.time - timeOfStart;
        
        PlayerManager.Instance.OnPlayerDeath -= GameLost;
        gameOver = true;
        Debug.Log("GAME OVER ! Tu as survécu " + gameTime.ToString("F2") + " secondes.");
        RestartGame();
    }


    private void Update()
    {
        if (gameOver)
            return;
        DisplayCurrentTime();
    }
    
    private void DisplayCurrentTime()
    {
        float gameTime = Time.time - timeOfStart;
        int minutes = (int) gameTime / 60;
        int seconds = (int) gameTime % 60;
        timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
    
    private void LevelUp(int level)
    {
        Debug.Log("LEVEL UP ! Now level " + level + ".");
        PowerupPanelManager.Instance.ShowPowerupOptions();
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
