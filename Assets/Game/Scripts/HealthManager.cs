using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    public Image healthBar;
    public float healthAmount = 100f;

    public event Action<float> OnHealthChanged;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            TakeDamage(10);
        if(Input.GetKeyDown(KeyCode.H))
            Heal(20);
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;
        OnHealthChanged?.Invoke(healthAmount);
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }
    
    private void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        
        healthBar.fillAmount = healthAmount / 100f;
        
    }
    
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
}
