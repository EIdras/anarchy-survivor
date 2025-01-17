using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenusManager : MonoBehaviour
{
    public List<MenuData> menusData;
    public EventSystem eventSystem;

    [Serializable]
    public struct MenuData
    {
        public GameObject menu;
        public GameObject firstSelected;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ChangeMenu(int index)
    {
        menusData.ForEach(menuData => menuData.menu.SetActive(false));
        EventSystem.current.SetSelectedGameObject(menusData[index].firstSelected);
        menusData[index].menu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
