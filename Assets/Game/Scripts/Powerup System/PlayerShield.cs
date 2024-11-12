using System.Collections;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [Header("Shield Settings")]
    public GameObject shieldPrefab;       // Référence au prefab de la sphère de bouclier
    public float regenerationTime = 5f;   // Temps de régénération par couche

    private GameObject shieldInstance;
    private int maxLayers;                // Nombre de couches maximal, basé sur le niveau

    [SerializeField, Tooltip("Nombre de couches actuellement disponibles")] 
    private int currentLayers;            // Couches actuelles disponibles, visible dans l'éditeur

    private bool isRegenerating = false;
    
    private static float transparency = 0.2f;

    // Couleurs pour chaque couche, du plus faible au plus fort (bleu, vert, jaune, orange, rouge avec de la transparence)
    private Color[] layerColors = { new Color(0, 0, 1, transparency), new Color(0, 1, 0, transparency), new Color(1, 1, 0, transparency), new Color(1, 0.5f, 0, transparency), new Color(1, 0, 0, transparency) };

    public void InitializeShield(int level)
    {
        Debug.Log("Initializing shield with level " + level);
        maxLayers = Mathf.Clamp(level, 1, layerColors.Length);  // Nombre maximal de couches basé sur le niveau et limité aux couleurs disponibles
        currentLayers = maxLayers;

        // Instancie le bouclier s'il n'existe pas encore
        if (shieldInstance == null)
        {
            // Crée le bouclier en tant qu'enfant du joueur, en y=1 pour le visuel
            shieldInstance = Instantiate(shieldPrefab, transform.position + Vector3.up, Quaternion.identity);
            shieldInstance.transform.SetParent(transform); // Ne pas le parenté pour éviter de suivre la rotation
        }

        UpdateShieldVisual();
        shieldInstance.SetActive(true);  // Toujours actif
    }

    private void UpdateShieldVisual()
    {
        // Change la couleur de la sphère de bouclier en fonction du nombre de couches disponibles
        int colorIndex = Mathf.Clamp(currentLayers - 1, 0, layerColors.Length - 1);
        Renderer renderer = shieldInstance.GetComponent<Renderer>();
        renderer.material.color = layerColors[colorIndex];
    }

    public void TakeHit()
    {
        if (currentLayers > 0)
        {
            currentLayers--; // Réduit les couches disponibles
            UpdateShieldVisual();

            // Si toutes les couches sont détruites, commence la régénération
            if (currentLayers == 0 && !isRegenerating)
            {
                StartCoroutine(RegenerateShield());
            }
        }
    }

    private IEnumerator RegenerateShield()
    {
        isRegenerating = true;
        while (currentLayers < maxLayers)
        {
            yield return new WaitForSeconds(regenerationTime); // Attendre avant de régénérer chaque couche
            currentLayers++;
            UpdateShieldVisual();
        }
        isRegenerating = false;
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyProjectile"))
        {
            Destroy(other.gameObject);
            TakeHit();
        }
    }
    */
}
