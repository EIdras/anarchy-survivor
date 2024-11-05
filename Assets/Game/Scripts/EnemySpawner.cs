using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemySpawner;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public Transform player;
    public float activeRadius = 30f;       // Rayon de la zone active autour du joueur
    public float preloadRadius = 50f;      // Rayon de la zone de préchargement autour du joueur
    public int maxEnemys = 100;   // Nombre maximum d'ennemi d'expérience simultanés
    public int pvEnemys = 1;

    [System.Serializable]
    public struct EnemyTypes
    {
        public int experienceValue;
        public GameObject enemySkin;
        public float spawnChance; // Chance d'apparition relative (0-1)
    }

    public List<EnemyTypes> enemyTypes; // Liste des types d'ennemi

    private List<GameObject> enemyPool = new List<GameObject>();
    private List<GameObject> activeEnemy = new List<GameObject>();

    private void Start()
    {
        // Initialisation du pool de enemy
        for (int i = 0; i < maxEnemys; i++)
        {
            GameObject enemy = Instantiate(EnemyPrefab);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
    }

    private void Update()
    {
        ManageSpawnZones();
    }

    private void ManageSpawnZones()
    {
        // Décharge les enemy trop éloignés
        for (int i = activeEnemy.Count - 1; i >= 0; i--)
        {
            GameObject enemy = activeEnemy[i];

            // Vérifie si l'enemy a été détruit
            if (enemy == null)
            {
                activeEnemy.RemoveAt(i);
                continue;
            }

            Vector2 enemyPosition2D = new Vector2(enemy.transform.position.x, enemy.transform.position.z);
            Vector2 playerPosition2D = new Vector2(player.position.x, player.position.z);

            if (Vector2.Distance(playerPosition2D, enemyPosition2D) > preloadRadius)
            {
                DeactivateEnemy(enemy);
                activeEnemy.RemoveAt(i);
            }
        }

        // Charge de nouveaux enemy dans la zone de préchargement si nécessaire
        while (activeEnemy.Count < maxEnemys)
        {
            Vector2 spawnPosition2D = GetRandomPositionOutsideActiveRadius();
            if (IsPositionValid(spawnPosition2D))
            {
                ActivateEnemy(spawnPosition2D);
            }
        }
    }


    private Vector2 GetRandomPositionOutsideActiveRadius()
    {
        // Choisit une position aléatoire dans la zone de préchargement en 2D, en dehors de la zone active
        Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(activeRadius, preloadRadius);
        Vector2 playerPosition2D = new Vector2(player.position.x, player.position.z);
        return playerPosition2D + randomOffset;
    }

    private bool IsPositionValid(Vector2 position)
    {
        // Vérifie que la position est suffisamment éloignée des autres cubes actifs
        foreach (GameObject enemy in activeEnemy)
        {
            Vector2 enemyPosition2D = new Vector2(enemy.transform.position.x, enemy.transform.position.z);
            if (Vector2.Distance(position, enemyPosition2D) < 2.0f) // Distance minimale entre ennemi
            {
                return false;
            }
        }
        return true;
    }

    private void ActivateEnemy(Vector2 position2D)
    {
        if (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool[0];
            enemyPool.RemoveAt(0);

            // Configure le type d'ennemi
            EnemyTypes enemyType = GetRandomEnemyType();

            enemy = Instantiate(enemyType.enemySkin);

            // Place l'ennemi sur le plan XZ
            enemy.transform.position = new Vector3(position2D.x, 0, position2D.y);
            // Configure le skin le l'ennemi


            enemy.SetActive(true);
            activeEnemy.Add(enemy);
        }
        else
        {
            Debug.LogWarning("Pas d'ennemi disponible dans le pool. Impossible d'activer un nouveau ennemi.");
        }
    }

    private void DeactivateEnemy(GameObject enemy)
    {
        if (enemy != null)
        {
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
    }


    private EnemyTypes GetRandomEnemyType()
    {
        if (enemyTypes == null || enemyTypes.Count == 0)
        {
            Debug.LogError("La liste des types d'ennemi est vide. Veuillez configurer des types d'ennemi dans l'inspecteur.");
            throw new System.Exception("Pas de types d'ennemi configurés dans DynamicSpawner.");
        }

        float totalChance = 0f;
        foreach (var enemyType in enemyTypes)
        {
            totalChance += enemyType.spawnChance;
        }

        if (totalChance <= 0)
        {
            Debug.LogError("La chance totale d'apparition des types d'ennemi est nulle ou négative. Veuillez configurer des chances d'apparition valides dans l'inspecteur.");
            throw new System.Exception("Chances d'apparition nulles ou négatives dans DynamicSpawner.");
        }

        float randomValue = Random.value * totalChance;
        float cumulativeChance = 0f;

        foreach (var enemyType in enemyTypes)
        {
            cumulativeChance += enemyType.spawnChance;
            if (randomValue <= cumulativeChance)
            {
                return enemyType;
            }
        }

        // En dernier recours, retourne le premier type comme solution de secours
        Debug.LogWarning("Aucun type d'ennemi n'a été sélectionné. Retourne le premier type par défaut.");
        return enemyTypes[0];
    }


    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.position;
            Vector3 playerPosition2D = new Vector3(playerPosition.x, 0, playerPosition.z);

            // Couleur de la zone active (vert)
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(playerPosition2D, activeRadius);

            // Couleur de la zone de préchargement (bleu)
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(playerPosition2D, preloadRadius);
        }
    }
}
