using System.Collections.Generic;
using UnityEngine;

public class DynamicSpawner : MonoBehaviour
{
    public GameObject experienceCubePrefab;
    public Transform player;
    public float activeRadius = 30f;       // Rayon de la zone active autour du joueur
    public float preloadRadius = 50f;      // Rayon de la zone de préchargement autour du joueur
    public int maxExperienceCubes = 100;   // Nombre maximum de cubes d'expérience simultanés

    [System.Serializable]
    public struct CubeType
    {
        public int experienceValue;
        public Color color;
        public float spawnChance; // Chance d'apparition relative (0-1)
    }

    public List<CubeType> cubeTypes; // Liste des types de cubes

    private List<GameObject> experienceCubesPool = new List<GameObject>();
    private List<GameObject> activeCubes = new List<GameObject>();

    private void Start()
    {
        // Initialisation du pool de cubes d'expérience
        for (int i = 0; i < maxExperienceCubes; i++)
        {
            GameObject cube = Instantiate(experienceCubePrefab);
            cube.SetActive(false);
            experienceCubesPool.Add(cube);
        }
    }

    private void Update()
    {
        ManageSpawnZones();
    }

    private void ManageSpawnZones()
    {
        // Décharge les cubes d'expérience trop éloignés
        for (int i = activeCubes.Count - 1; i >= 0; i--)
        {
            GameObject cube = activeCubes[i];

            // Vérifie si le cube a été détruit
            if (cube == null)
            {
                activeCubes.RemoveAt(i);
                continue;
            }

            Vector2 cubePosition2D = new Vector2(cube.transform.position.x, cube.transform.position.z);
            Vector2 playerPosition2D = new Vector2(player.position.x, player.position.z);

            if (Vector2.Distance(playerPosition2D, cubePosition2D) > preloadRadius)
            {
                DeactivateCube(cube);
                activeCubes.RemoveAt(i);
            }
        }

        // Charge de nouveaux cubes dans la zone de préchargement si nécessaire
        while (activeCubes.Count < maxExperienceCubes)
        {
            Vector2 spawnPosition2D = GetRandomPositionOutsideActiveRadius();
            if (IsPositionValid(spawnPosition2D))
            {
                ActivateCube(spawnPosition2D);
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
        foreach (GameObject cube in activeCubes)
        {
            Vector2 cubePosition2D = new Vector2(cube.transform.position.x, cube.transform.position.z);
            if (Vector2.Distance(position, cubePosition2D) < 2.0f) // Distance minimale entre cubes
            {
                return false;
            }
        }
        return true;
    }

    private void ActivateCube(Vector2 position2D)
    {
        if (experienceCubesPool.Count > 0)
        {
            GameObject cube = experienceCubesPool[0];
            experienceCubesPool.RemoveAt(0);

            // Configure le type de cube
            CubeType cubeType = GetRandomCubeType();
            cube.GetComponent<Renderer>().material.color = cubeType.color;
            cube.GetComponent<ExperienceCube>().SetExperienceValue(cubeType.experienceValue);

            // Place le cube sur le plan XZ
            cube.transform.position = new Vector3(position2D.x, 0, position2D.y);
            cube.SetActive(true);
            activeCubes.Add(cube);
        }
        else
        {
            Debug.LogWarning("Pas de cube disponible dans le pool. Impossible d'activer un nouveau cube.");
        }
    }

    private void DeactivateCube(GameObject cube)
    {
        if (cube != null)
        {
            cube.SetActive(false);
            experienceCubesPool.Add(cube);
        }
    }


    private CubeType GetRandomCubeType()
    {
        if (cubeTypes == null || cubeTypes.Count == 0)
        {
            Debug.LogError("La liste des types de cubes est vide. Veuillez configurer des types de cubes dans l'inspecteur.");
            throw new System.Exception("Pas de types de cubes configurés dans DynamicSpawner.");
        }

        float totalChance = 0f;
        foreach (var cubeType in cubeTypes)
        {
            totalChance += cubeType.spawnChance;
        }

        if (totalChance <= 0)
        {
            Debug.LogError("La chance totale d'apparition des types de cubes est nulle ou négative. Veuillez configurer des chances d'apparition valides dans l'inspecteur.");
            throw new System.Exception("Chances d'apparition nulles ou négatives dans DynamicSpawner.");
        }

        float randomValue = Random.value * totalChance;
        float cumulativeChance = 0f;

        foreach (var cubeType in cubeTypes)
        {
            cumulativeChance += cubeType.spawnChance;
            if (randomValue <= cumulativeChance)
            {
                return cubeType;
            }
        }

        // En dernier recours, retourne le premier type comme solution de secours
        Debug.LogWarning("Aucun type de cube n'a été sélectionné. Retourne le premier type par défaut.");
        return cubeTypes[0];
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
