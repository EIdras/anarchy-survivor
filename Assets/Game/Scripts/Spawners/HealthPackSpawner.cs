using System.Collections.Generic;
using UnityEngine;

public class HealthPackSpawner : MonoBehaviour
{
    public static HealthPackSpawner Instance { get; private set; }
    
    public GameObject healthPackPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void SpawnPack(Vector2 position2D)
    {
        GameObject pack = Instantiate(healthPackPrefab, position2D, Quaternion.identity);
    }

    public void DestroyPack(GameObject pack)
    {
        if (pack != null)
        {
            pack.SetActive(false);
            Destroy(pack);
        }
    }
}
