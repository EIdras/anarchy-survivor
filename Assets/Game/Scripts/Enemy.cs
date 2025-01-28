using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // --- Enumération des états de l’ennemi ---
    private enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Dead
    }

    // --- Propriétés de l’ennemi ---
    [Header("Paramètres de base")]
    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private float damageToPlayer = 10f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float damageColorDuration = 0.2f;
    [SerializeField] private float expDropChance = 0.1f;
    [SerializeField] private float healthDropChance = 0.1f;
    [SerializeField] private int expAmount;
    
    [Header("Distances")]
    [SerializeField] private float detectionRange = 10f;  // Distance à partir de laquelle on commence à "Chase"
    [SerializeField] private float attackRange = 1.5f;    // Distance à partir de laquelle on passe en "Attack"

    [Header("Couleurs / Visuels")]
    public Color damageColor = Color.red;
    private List<Color> originalColor_head;
    private List<Color> originalColor_body;

    [Header("Références externes")]
    public Animator animator;
    private SoundManager soundManager;
    private Transform player;

    // --- Internes ---
    private float lastAttackTime = 0f;
    private EnemyState currentState = EnemyState.Chase;

    // Rendu
    private SkinnedMeshRenderer bodyRenderer;
    private SkinnedMeshRenderer headRenderer;
    private GameObject body;
    private GameObject head;

    private void Awake()
    {
        soundManager = SoundManager.Instance;
        if (soundManager == null)
        {
            Debug.LogWarning("SoundManager est introuvable ou non initialisé.");
        }
    }

    // Méthode d'initialisation appelée depuis l'extérieur
    public void Initialize(int health, float speed, float damage, float dropChance, int expAmount, Transform player)
    {
        this.health = health;
        this.speed = speed;
        this.damageToPlayer = damage;
        this.expDropChance = dropChance;
        this.expAmount = expAmount;
        this.player = player;

        currentState = EnemyState.Chase;
        RestoreOriginalColors();
        StopAllCoroutines();

        gameObject.SetActive(true);
    }

    private void Start()
    {
        // Si vous le souhaitez, vous pouvez forcer l’état à Idle ou un autre état ici.
        currentState = EnemyState.Chase;
    }

    private void RestoreOriginalColors()
    {
        if (originalColor_body == null || originalColor_head == null)
            return;

        for (int i = 0; i < bodyRenderer.materials.Length; i++)
        {
            bodyRenderer.materials[i].color = originalColor_body[i];
        }
        for (int i = 0; i < headRenderer.materials.Length; i++)
        {
            headRenderer.materials[i].color = originalColor_head[i];
        }
    }

    private void Update()
    {
        // On met à jour la FSM à chaque frame
        UpdateFSM();
        UpdateAnimator();
    }

    private void UpdateFSM()
    {
        // En fonction de l'état actuel, on appelle la méthode correspondante
        switch (currentState)
        {
            case EnemyState.Idle:
                IdleState();
                break;
            case EnemyState.Chase:
                ChaseState();
                break;
            case EnemyState.Attack:
                AttackState();
                break;
            case EnemyState.Dead:
                break;
        }
    }

    private void UpdateAnimator()
    {
        if (animator != null)
        {
            // On peut lier la vitesse ou d'autres paramètres à l’animator
            animator.SetFloat("Speed", (currentState == EnemyState.Chase) ? speed : 0f);
        }
    }

    // --- Implémentation des comportements par état ---
    private void IdleState()
    {
        if (player == null) return;

        // Vérifie la distance du joueur
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            // Passe en état Chase
            SwitchState(EnemyState.Chase);
        }
    }

    private void ChaseState()
    {
        if (player == null)
        {
            SwitchState(EnemyState.Idle);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            // Passe en état Attack
            SwitchState(EnemyState.Attack);
        }
        else
        {
            // Continue de se déplacer vers le joueur
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            transform.LookAt(player);
        }
    }

    private void AttackState()
    {
        if (player == null)
        {
            Debug.LogWarning("Player introuvable, retour à l'état Idle.");
            SwitchState(EnemyState.Idle);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Si le joueur s'éloigne, on repasse en Chase
        if (distanceToPlayer > attackRange)
        {
            SwitchState(EnemyState.Chase);
            return;
        }

        // Sinon, on attaque régulièrement
        if (Time.time >= lastAttackTime + attackSpeed)
        {
            StartCoroutine(PerformAttack());
        }
    }

    // --- Attaque au joueur ---
    private System.Collections.IEnumerator PerformAttack()
    {
        PlayerManager.Instance.TakeDamage(damageToPlayer);
        lastAttackTime = Time.time;
        Debug.Log($"Enemy hit the player, dealt {damageToPlayer} damage.");
        yield return new WaitForSeconds(attackSpeed);
        // Une fois le cooldown fini, l'ennemi peut réattaquer si toujours en AttackState
    }

    // --- Prendre des dégâts ---
    public void TakeDamage(float damage)
    {
        health -= damage;
        
        if (health <= 0 && currentState != EnemyState.Dead)
        {
            SwitchState(EnemyState.Dead);
            HandleDeath();
        }
        else
        {
            soundManager.PlayEnemyHitSound();
            StartCoroutine(ShowDamageEffect());
        }
    }

    private System.Collections.IEnumerator ShowDamageEffect()
    {
        PlayHitParticles();

        // Change la couleur de tous les matériaux des SkinnedMeshRenderer
        foreach (Material mat in bodyRenderer.materials)
        {
            mat.color = damageColor;
        }
        foreach (Material mat in headRenderer.materials)
        {
            mat.color = damageColor;
        }

        yield return new WaitForSeconds(damageColorDuration);

        // Restaure la couleur originale
        for (int i = 0; i < bodyRenderer.materials.Length; i++)
        {
            bodyRenderer.materials[i].color = originalColor_body[i];
        }
        for (int i = 0; i < headRenderer.materials.Length; i++)
        {
            headRenderer.materials[i].color = originalColor_head[i];
        }
    }

    // --- Gestion de la mort ---
    private void HandleDeath()
    {
        soundManager.PlayEnemyDeathSound();
        PlayDeathParticles();
        gameObject.SetActive(false); // Ajoute l’ennemi au pool pour réutilisation, ou le détruit

        // Calcul de la chance de drop
        float randomChance = UnityEngine.Random.value;
        if (randomChance <= expDropChance)
        {
            // Spawn d’un cube rare avec la valeur d’XP
            ExperienceCubeSpawner.Instance.SpawnRareExperienceCube(transform.position, expAmount);
        } else if (UnityEngine.Random.value <= healthDropChance)
        {
            // Spawn d’un pack de soin
            HealthPackSpawner.Instance.SpawnPack(transform.position);
        }
    }

    // --- Particules ---
    private void PlayHitParticles()
    {
        ParticleUtils.PlayBloodParticles(transform.position, 2, 8);
    }
    private void PlayDeathParticles()
    {
        ParticleUtils.PlayBloodParticles(transform.position, 20, 40);
    }

    // --- Changement d’état ---
    private void SwitchState(EnemyState newState)
    {
        // On pourrait avoir un OnExitState / OnEnterState si besoin
        currentState = newState;
    }

    // --- Gestion des triggers (Optionnel, si on préfère attaquer via collisions) ---
    // Ici, on peut simplement faire des transitions vers Attack si on veut un comportement
    // identique à votre code d’origine avec OnTriggerEnter / OnTriggerStay.

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Si vous préférez, vous pouvez forcer l’état Attack ici :
            if (currentState != EnemyState.Attack && currentState != EnemyState.Dead)
            {
                SwitchState(EnemyState.Attack);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Même logique, forcer Attack si l’ennemi n’est pas mort
            if (currentState != EnemyState.Attack && currentState != EnemyState.Dead)
            {
                SwitchState(EnemyState.Attack);
            }
        }
    }

    // Gizmos pour visualiser la zone de l’ennemi (optionnel)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
