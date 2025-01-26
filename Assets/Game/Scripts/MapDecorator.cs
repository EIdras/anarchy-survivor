using UnityEngine;
using System.Collections.Generic;

public class MapDecorator : MonoBehaviour
{
    public GameObject[] prefabs; // Liste des préfabs à placer
    public GameObject plane; // Surface de la map
    public int density = 50;
    public bool randomRotation = true;
    public float minimumDistance = 1.0f;

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
            Debug.LogWarning("Aucun préfab");
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

            while (!placed && attempts < density * 10) // Permet d'éviter des boucle infinie
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

                    // Ajouter un MeshCollider car le préfab n'en a pas
                    AddMeshCollider(instance);

                    placed = true;
                }
                attempts++;
            }
        }
    }

    // Calculer la distance entre la position proposée et les positions déjà utilisées
    bool IsPositionValid(Vector3 position)
    {
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
