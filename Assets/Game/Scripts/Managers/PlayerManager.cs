using System;
using System.Collections;
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
    public Weapon weapon;
    public float passiveRegen = 0.0f; // régénération passive
    public PlayerShield shield;
    
    private bool isDead = false;
    
    [SerializeField] private Animator animator;
    private SoundManager soundManager;

    public event Action<float> OnHealthChanged;
    public event Action OnPlayerDeathAnim;
    public event Action OnPlayerDeath;
    public event Action<int, int> OnExperienceChanged;
    public event Action<int> OnLevelUp;
    public event Action OnTogglePause; 

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            soundManager = SoundManager.Instance;
            if (soundManager == null) {
                Debug.LogWarning("SoundManager is not initialized.");
            }
        }
    }

    private void Start()
    {
        StartCoroutine(RegenerateHealth());
    }
    
    private IEnumerator RegenerateHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            Heal(passiveRegen);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;
        if (shield.isShieldTanking())
        {
            soundManager.PlayPlayerShieldBrokenSound();
            shield.TakeHit();
            return;
        }
        soundManager.PlayPlayerHitSound();
        ParticleUtils.PlayBloodParticles(transform.position, 2, 8);
        health -= damage;
        health = Mathf.Clamp(health, 0, 100);
        OnHealthChanged?.Invoke(health);
        if (health <= 0)
        {
            isDead = true;
            StopCoroutine(RegenerateHealth());
            StartCoroutine(HandleDeath());
        }
    }

    public void Heal(float amount)
    {
        if (isDead)
            return;
        health += amount;
        health = Mathf.Clamp(health, 0, 100);
        OnHealthChanged?.Invoke(health);
    }

    public void AddExperience(int amount)
    {
        if (isDead)
            return;
        experience += amount;
        int experienceRequiredToLevelUp = level * 10;
        OnExperienceChanged?.Invoke(experience, experienceRequiredToLevelUp);

        if (experience >= level * 10) // Ex: chaque niveau requiert 100 points * le niveau actuel
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        if (isDead)
            return;
        level++;
        experience = 0; // Réinitialise l'expérience après le niveau
        OnLevelUp?.Invoke(level);
    }

    private IEnumerator HandleDeath()
    {
        // Gestion de la mort du joueur
        OnPlayerDeathAnim?.Invoke();
        soundManager.PlayPlayerDeathSound();
        yield return StartCoroutine(PlayDeathAnimation());
        OnPlayerDeath?.Invoke();
        Debug.Log("Player has died");
    }

    private IEnumerator PlayDeathAnimation()
    {
        animator.SetLayerWeight(1, 0f);
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(1.6f);
    }
    
    public void TogglePause()
    {
        OnTogglePause?.Invoke();
    }

    private void OnDrawGizmos()
    {
        // afficher la portée de collecte
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, collectionRadius);
    }
}