using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public event Action<int> OnScoreChanged;

    private int score = 0;
    public int Score => score;

    public void AddScore(int add)
    {
        score += add;
        OnScoreChanged?.Invoke(score);
        Debug.Log("Current Score: " + score);
    }




    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

}
