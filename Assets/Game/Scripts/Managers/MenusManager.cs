using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenusManager : MonoBehaviour
{
    public List<MenuData> menusData;
    public TMP_Text highScoreText;
    public EventSystem eventSystem;

    [Serializable]
    public struct MenuData
    {
        public GameObject menu;
        public GameObject firstSelected;
    }

    private void Awake()
    {
        if (PlayerPrefs.HasKey(HighScoreService.BestTimeKey))
        {
            float bestSurvivalTime = PlayerPrefs.GetFloat(HighScoreService.BestTimeKey);
            Debug.Log("Existing best time: " + bestSurvivalTime);
            if (bestSurvivalTime > 0)
            {
                Debug.Log("Displaying best time");
                int minutes = Mathf.FloorToInt(bestSurvivalTime / 60);
                int seconds = Mathf.FloorToInt(bestSurvivalTime % 60);
                string formattedTime = string.Format("{0}min {1}s", minutes, seconds);
                highScoreText.text = "Best time: " + formattedTime;
            }
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(HighScoreService.BestTimeKey))
        {
            float bestSurvivalTime = PlayerPrefs.GetFloat(HighScoreService.BestTimeKey);
            Debug.Log("Existing best time: " + bestSurvivalTime);
            if (bestSurvivalTime > 0)
            {
                Debug.Log("Displaying best time");
                int minutes = Mathf.FloorToInt(bestSurvivalTime / 60);
                int seconds = Mathf.FloorToInt(bestSurvivalTime % 60);
                string formattedTime = string.Format("{0}min {1}s", minutes, seconds);
                highScoreText.text = "Best time: " + formattedTime;
            }
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ChangeMenu(int index)
    {
        menusData.ForEach(menuData => menuData.menu.SetActive(false));
        eventSystem.SetSelectedGameObject(menusData[index].firstSelected);
        menusData[index].menu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
