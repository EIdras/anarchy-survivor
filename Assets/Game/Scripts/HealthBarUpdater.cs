using UnityEngine;
using UnityEngine.UI;

public class HealthBarUpdater : MonoBehaviour
{
    public Image healthBar;

    private void OnEnable()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnHealthChanged += UpdateHealthBar;
        }
        else
        {
            Debug.LogWarning("PlayerManager.Instance vaut null, l'event OnHealthChanged n'a pas été assigné.");
        }
    }

    private void OnDisable()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void UpdateHealthBar(float health)
    {
        Debug.Log("Updating health bar with value: " + health);
        if (healthBar != null)
        {
            healthBar.fillAmount = health / 100f;
        }
        else
        {
            Debug.LogWarning("healthBar n'est pas assignée dans l'inspecteur.");
        }
    }
}