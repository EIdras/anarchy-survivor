using UnityEngine;

public class HighScoreService : MonoBehaviour
{
    public static HighScoreService instance;
    public static string BestTimeKey = "BestSurvivalTime";

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    void Start(){
        if(!PlayerPrefs.HasKey(BestTimeKey)){
            Debug.Log("PlayerPrefs: Best time key not found. Creating it.");
            PlayerPrefs.SetFloat(BestTimeKey, 0);
        }
    }

    void OnApplicationQuit(){
        PlayerPrefs.Save();
    }

    public void SendGameDuration(float gameDuration)
    {
        float existingBestTime = PlayerPrefs.GetFloat(BestTimeKey, 0);
        
        if (gameDuration > existingBestTime)
        {
            Debug.Log("PlayerPrefs: New best time = " + gameDuration);
            PlayerPrefs.SetFloat(BestTimeKey, gameDuration);
        }
    }
}
