using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private float timeOfStart;
    private bool gameOver = false;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject pauseMenu;
    
    [SerializeField] private AudioSource playerEffectsSource;
    [SerializeField] private AudioSource effectsSource;
    [SerializeField] private AudioSource musicSource;
    
    private HighScoreService highScoreService;
    private SoundManager soundManager;
    private TimeManager timeManager;

    private void Awake()
    {
        highScoreService = HighScoreService.instance;
        soundManager = SoundManager.Instance;
        timeManager = TimeManager.Instance;
        
        if (highScoreService == null)
        {
            Debug.LogError("HighScoreService instance is not initialized.");
            return;
        }
        
        if (soundManager == null)
        {
            Debug.LogError("SoundManager instance is not initialized.");
            return;
        }
        
        soundManager.InitializeAudioSources(playerEffectsSource, effectsSource, musicSource);
    }

    void Start()
    {
        soundManager.PlayGameMusic();
        
        PlayerManager playerInstance = PlayerManager.Instance;
        playerInstance.OnPlayerDeath += GameLost;
        playerInstance.OnLevelUp += LevelUp;
        playerInstance.OnTogglePause += TogglePause;
        timeOfStart = Time.time;
    }

    private void GameLost()
    {
        float gameTime = Time.time - timeOfStart;
        timeManager.StopTime();
        PlayerManager.Instance.OnPlayerDeath -= GameLost;
        gameOver = true;
        highScoreService.SendGameDuration(gameTime);
        Debug.Log("GAME OVER ! Tu as survécu " + gameTime.ToString("F2") + " secondes.");
        
        gameOverCanvas.SetActive(true);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(gameOverCanvas.GetComponentInChildren<UnityEngine.UI.Button>().gameObject);
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
        timeManager.StopTime();
        PowerupPanelManager.Instance.ShowPowerupOptions();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        timeManager.ResumeTime();
    }
    
    public void LoadMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        timeManager.ResumeTime();
    }
    
    public void TogglePause()
    {
        if (Time.timeScale.Equals(0f))
        {
            timeManager.ResumeTime();
            pauseMenu.SetActive(false);
        }
        else
        {
            timeManager.StopTime();
            pauseMenu.SetActive(true);
            pauseMenu.GetComponentInChildren<MenusManager>().ChangeMenu(0);
        }
    }
    
    public SoundManager GetSoundManager()
    {
        return soundManager;
    }
}
