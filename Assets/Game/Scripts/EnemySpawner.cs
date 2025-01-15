using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnRadius = 50f;      // Rayon de spawn des ennemis
    public float despawnRadius = 60f;    // Rayon de despawn des ennemis 
    public int maxEnemies = 50;

    [System.Serializable]
    public struct EnemyType
    {
        public int health;
        public float speed;
        public Color color;
        public float spawnChance;
        public float baseDamage;
    }

    public List<EnemyType> enemyTypes;
    public int playerLevel = 1;  // Niveau du joueur
    public float baseSpawnInterval = 2f;
    private float spawnTimer;

    private List<GameObject> enemyPool = new List<GameObject>();
    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        // Initialisation du pool d'ennemis
        for (int i = 0; i < maxEnemies; i++)
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
        UpdateEnemyBehavior();

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = GetSpawnInterval();
        }
    }

    // Récupère la fréquence de spawn des ennemis en fonction du niveau du joueur
    private float GetSpawnInterval()
    {
        return Mathf.Max(baseSpawnInterval / (1 + playerLevel * 0.1f), 0.2f);
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
        if (activeEnemies.Count >= maxEnemies)
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
        Vector2 playerPosition2D = new Vector2(player.position.x, player.position.z);
        return playerPosition2D + randomOffset;
    }

    // Vérifie si la position de spawn est valide
    private bool IsPositionValid(Vector2 position)
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy == null) continue;
            Vector2 enemyPosition2D = new Vector2(enemy.transform.position.x, enemy.transform.position.z);
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

            EnemyType enemyType = GetRandomEnemyTypeBasedOnLevel();

            // Détermine les dégâts de l'ennemi en fonction de son niveau
            float damage = enemyType.baseDamage * (1 + playerLevel * 0.2f);

            // Configure l'ennemi
            GameObject body = enemy.transform.GetChild(0).gameObject;
            GameObject head = enemy.transform.GetChild(1).gameObject;
            SkinnedMeshRenderer bodyRenderer = body.GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer headRenderer = head.GetComponent<SkinnedMeshRenderer>();
            bodyRenderer.materials[2].color = enemyType.color;
            headRenderer.materials[2].color = enemyType.color;

            enemy.GetComponent<Enemy>().Initialize(enemyType.health, enemyType.speed, damage, player);

            enemy.transform.position = new Vector3(position2D.x, 0, position2D.y);
            enemy.SetActive(true);
            activeEnemies.Add(enemy);
        }
        else
        {
            Debug.LogWarning("Pas d'ennemi disponible dans le pool. Impossible d'activer un nouvel ennemi.");
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

    // Récupère un type d'ennemi aléatoire en fonction du niveau du joueur
    private EnemyType GetRandomEnemyTypeBasedOnLevel()
    {
        float totalChance = 0f;
        List<EnemyType> weightedTypes = new List<EnemyType>();

        foreach (var enemyType in enemyTypes)
        {
            float adjustedChance = enemyType.spawnChance * (1 + playerLevel * 0.05f);
            weightedTypes.Add(new EnemyType
            {
                health = enemyType.health,
                speed = enemyType.speed,
                color = enemyType.color,
                spawnChance = adjustedChance,
                baseDamage = enemyType.baseDamage
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

    // Met à jour le comportement des ennemis actifs
    private void UpdateEnemyBehavior()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().MoveTowardsPlayer();
            }
        }
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
