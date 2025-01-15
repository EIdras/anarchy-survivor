using System.Collections.Generic;
using UnityEngine;

public class PlayerOrbital : MonoBehaviour
{
    private List<GameObject> orbitals = new List<GameObject>();
    private const int BaseObjectCount = 3;
    private int objectCount;
    private float rotationSpeed;
    private GameObject orbitalCenter; // Centre de rotation pour les lames

    public void Activate(GameObject bladePrefab, int level, float speed)
    {
        int count = BaseObjectCount + (level - 1);
        Debug.Log("Orbital activation : " + count + " objects, speed " + speed);
        objectCount = count;
        rotationSpeed = speed;

        // Si orbitalCenter n'existe pas, le créer
        if (orbitalCenter == null)
        {
            orbitalCenter = new GameObject("OrbitalCenter");
        }

        // Positionner le centre orbital à la position du joueur
        orbitalCenter.transform.position = transform.position;

        // Supprime les anciennes lames si elles existent
        foreach (var orbital in orbitals)
        {
            Destroy(orbital);
        }
        orbitals.Clear();

        // Crée de nouvelles lames autour du centre orbital
        for (int i = 0; i < objectCount; i++)
        {
            float angle = i * Mathf.PI * 2 / objectCount;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 2;
            GameObject blade = Instantiate(bladePrefab, orbitalCenter.transform.position + position, Quaternion.identity, orbitalCenter.transform);
            blade.transform.LookAt(transform.position); // Pointe la lame vers le joueur
            blade.AddComponent<OrbitalBlade>().Initialize(this); // Ajoute le script OrbitalBlade
            orbitals.Add(blade);
        }
    }

    private void Update()
    {
        if (orbitalCenter == null) return;

        // Mettre à jour la position du centre orbital pour qu'il suive le joueur
        orbitalCenter.transform.position = transform.position;

        // Faire tourner les lames autour du centre orbital
        for (int i = 0; i < orbitals.Count; i++)
        {
            float angle = i * Mathf.PI * 2 / objectCount + Time.time * rotationSpeed * Mathf.Deg2Rad;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 2;
            orbitals[i].transform.position = orbitalCenter.transform.position + position;
            orbitals[i].transform.LookAt(orbitals[i].transform.position + position); // Orienter la lame vers l'extérieur
        }
    }
}
