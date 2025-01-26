using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    private float timeOfStart;
    private bool gameOver = false;
    
    private PlayerController playerController;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        playerController = PlayerController.Instance;
        timeOfStart = Time.time;
    }

    private void FixedUpdate()
    {
        if (gameOver)
            return;
        DisplayCurrentTime();
    }

    public void StopTime()
    {
        Time.timeScale = 0;
        playerController.EnableControl(false);
    }
    
    public void ResumeTime()
    {
        Time.timeScale = 1;
        playerController.EnableControl(true);
    }
    
    private void DisplayCurrentTime()
    {
        float gameTime = GetGameTime();
        int minutes = (int) gameTime / 60;
        int seconds = (int) gameTime % 60;
        timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
    
    public float GetGameTime()
    {
        return Time.time - timeOfStart;
    }

    public void GameEnd()
    {
        StopTime();
        gameOver = true;
    }
}
