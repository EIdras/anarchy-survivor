using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Player Stats")]
    public float health = 100f;
    public int experience = 0;
    public int level = 1;
    public float collectionRadius = 3.0f; // portée de collecte
    public float moveSpeed = 5.0f; // vitesse de déplacement

    public event Action<float> OnHealthChanged;
    public event Action<int> OnExperienceChanged;
    public event Action<int> OnLevelUp;

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

    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, 100);
        OnHealthChanged?.Invoke(health);
        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, 100);
        OnHealthChanged?.Invoke(health);
    }

    public void AddExperience(int amount)
    {
        experience += amount;
        OnExperienceChanged?.Invoke(experience);

        if (experience >= level * 100) // Ex: chaque niveau requiert 100 points * le niveau actuel
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        experience = 0; // Réinitialise l'expérience après le niveau
        OnLevelUp?.Invoke(level);
    }

    private void Die()
    {
        // Gestion de la mort du joueur
        Debug.Log("Player has died");
    }
}