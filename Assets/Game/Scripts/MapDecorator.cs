using UnityEngine;
using System.Collections.Generic;

public class MapDecorator : MonoBehaviour
{
    public GameObject[] prefabs; // Liste des prefabs à placer
    public GameObject plane; // Surface de la map
    public int density = 50; // Nombre d'objets à placer
    public bool randomRotation = true;
    public float minimumDistance = 1.0f; // Distance minimale entre les objets
    public Transform playerSpawnPoint; // Point de spawn du joueur
    public float safeZoneRadius = 5.0f; // Rayon de la zone de sécurité autour du spawn

    private List<Vector3> placedPositions = new List<Vector3>();

    void Start()
    {
        if (plane == null)
        {
            Debug.LogError("Ajouter un plane au script!");
            return;
        }
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogWarning("Aucun prefab");
            return;
        }
        if (playerSpawnPoint == null)
        {
            Debug.LogError("Ajouter le point de spawn du joueur au script!");
            return;
        }

        DecoratePlane();
    }

    void DecoratePlane()
    {
        // Récupérer la taille du plane pour placer dans l'ensemble de la surface
        MeshRenderer planeRenderer = plane.GetComponent<MeshRenderer>();
        if (planeRenderer == null)
        {
            Debug.LogError("Le Plane n'a pas de MeshRenderer !");
            return;
        }

        Vector3 planeSize = planeRenderer.bounds.size;

        int attempts = 0;

        for (int i = 0; i < density; i++)
        {
            bool placed = false;

            while (!placed && attempts < density * 10) // Permet d'éviter une boucle infinie
            {
                float randomX = Random.Range(-planeSize.x / 2, planeSize.x / 2);
                float randomZ = Random.Range(-planeSize.z / 2, planeSize.z / 2);
                Vector3 position = new Vector3(randomX, 0, randomZ) + plane.transform.position;

                if (IsPositionValid(position))
                {
                    placedPositions.Add(position);

                    GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

                    // Rotation aléatoire + rotation de -90 degrés pour que les objets soient alignés avec le sol
                    Quaternion rotation = Quaternion.Euler(-90, randomRotation ? Random.Range(0, 360) : 0, 0);

                    GameObject instance = Instantiate(prefab, position, rotation, transform);

                    // Ajouter un MeshCollider si le prefab n'en a pas
                    AddMeshCollider(instance);

                    placed = true;
                }
                attempts++;
            }
        }
    }

    // Vérifie si la position proposée est valide
    bool IsPositionValid(Vector3 position)
    {
        // Vérifie si la position est trop proche du spawn du joueur
        if (playerSpawnPoint != null && Vector3.Distance(position, playerSpawnPoint.position) < safeZoneRadius)
        {
            return false;
        }

        // Vérifie la distance par rapport aux objets déjà placés
        foreach (Vector3 placedPosition in placedPositions)
        {
            if (Vector3.Distance(position, placedPosition) < minimumDistance)
            {
                return false;
            }
        }
        return true;
    }

    void AddMeshCollider(GameObject instance)
    {
        MeshCollider collider = instance.GetComponent<MeshCollider>();
        if (collider == null)
        {
            MeshFilter meshFilter = instance.GetComponent<MeshFilter>();
            if (meshFilter != null && meshFilter.sharedMesh != null)
            {
                collider = instance.AddComponent<MeshCollider>();
                collider.sharedMesh = meshFilter.sharedMesh;
                collider.convex = false;
            }
            else
            {
                Debug.LogWarning($"Impossible d'ajouter un MeshCollider à {instance.name} car il n'a pas de MeshFilter ou de Mesh.");
            }
        }
    }
}
