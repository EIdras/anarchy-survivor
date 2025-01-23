using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    
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
}
