using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnRadius = 50f;      // Rayon de spawn des ennemis
    public float despawnRadius = 60f;    // Rayon de despawn des ennemis 
    public int baseMaxEnemies = 10;      // Limite de base des ennemis
    public float baseSpawnInterval = 2f; // Intervalle de spawn de base
    public float healthOverTimeMultiplier = 0.1f; // Multiplicateur de vie des ennemis au fil du temps

    [System.Serializable]
    public struct EnemyType
    {
        public int health;
        public float speed;
        public Color color;
        public float spawnChance;
        public float baseDamage;
        public float expSpawnChance;
        public int expAmount;
    }

    public List<EnemyType> enemyTypes;
    private float spawnTimer;
    private List<GameObject> enemyPool = new List<GameObject>();
    private List<GameObject> activeEnemies = new List<GameObject>();

    private int currentMaxEnemies;
    private float currentSpawnInterval;
    private float timeSurvived;
    
    private TimeManager timeManager;
    private void Start()
    {
        timeManager = TimeManager.Instance;
        currentMaxEnemies = baseMaxEnemies;
        currentSpawnInterval = baseSpawnInterval;

        // Initialisation du pool d'ennemis
        for (int i = 0; i < baseMaxEnemies * 10; i++) // Crée un pool plus grand pour anticiper l'augmentation de la limite
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }

        spawnTimer = GetSpawnInterval();
    }

    private void Update()
    {
        ManageSpawn();

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = GetSpawnInterval();
        }
    }

    private void FixedUpdate()
    {
        timeSurvived = timeManager.GetGameTime();
        UpdateDifficulty();
    }

    private void UpdateDifficulty()
    {
        // Augmente le nombre maximum d'ennemis progressivement
        int newMaxEnemies = baseMaxEnemies + Mathf.FloorToInt(timeSurvived * 0.5f);
        if (newMaxEnemies != currentMaxEnemies)
        {
            currentMaxEnemies = newMaxEnemies;
        }

        // Diminue l'intervalle de spawn progressivement
        float newSpawnInterval = Mathf.Max(baseSpawnInterval - (timeSurvived * 0.02f), 0.1f);
        if (Mathf.Abs(newSpawnInterval - currentSpawnInterval) > 0.01f)
        {
            currentSpawnInterval = newSpawnInterval;
        }
    }

    // Récupère la fréquence de spawn des ennemis en fonction du niveau du joueur
    private float GetSpawnInterval()
    {
        return currentSpawnInterval;
    }

    private int GetMaxEnemies()
    {
        return currentMaxEnemies;
    }

    // Gère le spawn et le despawn des ennemis
    private void ManageSpawn()
    {
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            GameObject enemy = activeEnemies[i];
            if (enemy == null || Vector3.Distance(enemy.transform.position, player.position) > despawnRadius)
            {
                DeactivateEnemy(enemy);
                activeEnemies.RemoveAt(i);
            }
        }
    }

    // Spawne un ennemi à une position aléatoire
    private void SpawnEnemy()
    {
        if (activeEnemies.Count >= GetMaxEnemies())
        {
            return;
        }

        Vector2 spawnPosition2D = GetRandomSpawnPosition();
        if (IsPositionValid(spawnPosition2D))
        {
            ActivateEnemy(spawnPosition2D);
        }
    }

    // Récupère une position de spawn aléatoire autour du joueur
    private Vector2 GetRandomSpawnPosition()
    {
        Vector2 randomOffset = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 playerPosition = player.position;
        Vector2 playerPosition2D = new Vector2(playerPosition.x, playerPosition.z);
        return playerPosition2D + randomOffset;
    }

    // Vérifie si la position de spawn est valide
    private bool IsPositionValid(Vector2 position)
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy == null) continue;
            Vector3 enemyPosition = enemy.transform.position;
            Vector2 enemyPosition2D = new Vector2(enemyPosition.x, enemyPosition.z);
            if (Vector2.Distance(position, enemyPosition2D) < 2.0f)
            {
                return false;
            }
        }
        return true;
    }

    // Active un ennemi à une position donnée
    private void ActivateEnemy(Vector2 position2D)
    {
        if (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool[0];
            enemyPool.RemoveAt(0);

            EnemyType enemyType = GetRandomEnemyTypeBasedOnTime();

            // Configure l'ennemi
            GameObject body = enemy.transform.GetChild(0).gameObject;
            GameObject head = enemy.transform.GetChild(1).gameObject;
            SkinnedMeshRenderer bodyRenderer = body.GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer headRenderer = head.GetComponent<SkinnedMeshRenderer>();
            bodyRenderer.materials[2].color = enemyType.color;
            headRenderer.materials[2].color = enemyType.color;

            enemy.GetComponent<Enemy>().Initialize(
                enemyType.health + Mathf.FloorToInt(Time.timeSinceLevelLoad * healthOverTimeMultiplier), // Augmente la vie au fil du temps
                enemyType.speed,
                enemyType.baseDamage,
                enemyType.expSpawnChance,
                enemyType.expAmount,
                player
            );

            enemy.transform.position = new Vector3(position2D.x, 0, position2D.y);
            enemy.SetActive(true);
            activeEnemies.Add(enemy);
        }
        else
        {
            // Si aucun ennemi n'est disponible dans le pool, en créer un nouveau
            Debug.LogWarning("Pas d'ennemi disponible dans le pool. Création d'un nouvel ennemi.");
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
    }

    // Désactive un ennemi
    private void DeactivateEnemy(GameObject enemy)
    {
        if (enemy != null)
        {
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
    }

    private EnemyType GetRandomEnemyTypeBasedOnTime()
    {
        float totalChance = 0f;
        List<EnemyType> weightedTypes = new List<EnemyType>();

        foreach (var enemyType in enemyTypes)
        {
            float adjustedChance = enemyType.spawnChance * (1 + timeSurvived * 0.01f); // Augmente les chances des types d'ennemis plus puissants
            weightedTypes.Add(new EnemyType
            {
                health = enemyType.health,
                speed = enemyType.speed,
                color = enemyType.color,
                spawnChance = adjustedChance,
                baseDamage = enemyType.baseDamage,
                expSpawnChance = enemyType.expSpawnChance,
                expAmount = enemyType.expAmount
            });
            totalChance += adjustedChance;
        }

        float randomValue = Random.value * totalChance;
        float cumulativeChance = 0f;

        foreach (var enemyType in weightedTypes)
        {
            cumulativeChance += enemyType.spawnChance;
            if (randomValue <= cumulativeChance)
            {
                return enemyType;
            }
        }

        return weightedTypes[0];
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = new Color(1f, .3f, 0f, 0.5f);
            Gizmos.DrawWireSphere(player.position, spawnRadius);

            Gizmos.color = new Color(0.5f, 0f, 0f, 0.3f);
            Gizmos.DrawWireSphere(player.position, despawnRadius);
        }
    }
}
