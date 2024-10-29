using UnityEngine;

public class HighScoreService : MonoBehaviour
{
    public static HighScoreService instance;

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


    public void SendGameDuration(float gameDuration)
    {
        float existingBestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);
        
        if (gameDuration < existingBestTime)
        {
            PlayerPrefs.SetFloat("BestTime", existingBestTime);
        }
    }
}
