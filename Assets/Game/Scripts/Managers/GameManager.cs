using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
        playerInstance.OnPlayerDeathAnim += DisableControls;
        playerInstance.OnPlayerDeath += GameLost;
        playerInstance.OnLevelUp += LevelUp;
        playerInstance.OnTogglePause += TogglePause;
    }

    private void GameLost()
    {
        timeManager.GameEnd();
        float gameTime = timeManager.GetGameTime();
        PlayerManager.Instance.OnPlayerDeath -= GameLost;
        highScoreService.SendGameDuration(gameTime);
        Debug.Log("GAME OVER ! Tu as survécu " + gameTime.ToString("F2") + " secondes.");
        
        gameOverCanvas.SetActive(true);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(gameOverCanvas.GetComponentInChildren<UnityEngine.UI.Button>().gameObject);
    }
    
    private void DisableControls()
    {
        timeManager.EnableControl(false);
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
