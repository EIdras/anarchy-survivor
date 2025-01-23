using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUpdater : MonoBehaviour
{
    public Image healthBar;
    public TMP_Text levelText;

    private PlayerManager playerManager;

    private void Awake()
    {
        playerManager = PlayerManager.Instance;
    }

    private void OnEnable()
    {
        if (playerManager != null)
        {
            playerManager.OnHealthChanged += UpdateHealthBar;
            playerManager.OnLevelUp += UpdateLevelText;
        }
        else
        {
            Debug.LogWarning("PlayerManager.Instance vaut null, l'event OnHealthChanged n'a pas été assigné.");
        }
    }

    private void OnDisable()
    {
        if (playerManager != null)
        {
            playerManager.OnHealthChanged -= UpdateHealthBar;
            playerManager.OnLevelUp -= UpdateLevelText;
        }
    }

    private void UpdateHealthBar(float health)
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = health / 100f;
        }
        else
        {
            Debug.LogWarning("healthBar n'est pas assignée dans l'inspecteur.");
        }
    }
    
    private void UpdateLevelText(int level)
    {
        if (levelText != null)
        {
            levelText.text = level.ToString();
        }
        else
        {
            Debug.LogWarning("levelText n'est pas assignée dans l'inspecteur.");
        }
    }
}