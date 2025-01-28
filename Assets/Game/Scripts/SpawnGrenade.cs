using UnityEngine;
using UnityEngine.UI;

public class SpawnGrenade : MonoBehaviour
{
    public Transform playerPos;

    public GameObject grenadePrefab;

    [Header("Paramètres de l'image")]
    public Image radialImage; 

    [Header("Durée du remplissage (en secondes)")]
    public float duration = 5f;

    private float timer = 0f;
    private bool isFilling = true;

    private void Start()
    {
        if (radialImage == null)
        {
            Debug.LogError("Veuillez attribuer une image de type UI (Image) au script.");
            return;
        }

        if (radialImage.type != Image.Type.Filled)
        {
            Debug.LogError("L'image doit être configurée en mode Filled pour que le script fonctionne.");
            return;
        }

        radialImage.fillAmount = 0f; //image à vide
        Debug.Log("Image remise à zero.");
    }

    private void Update()
    {
        if (isFilling)
        {
            // Augmente le timer et met à jour l'image
            timer += Time.deltaTime;
            float fillAmount = Mathf.Clamp01(timer / duration);
            radialImage.fillAmount = fillAmount;

            // Arrêter une fois l'image complètement remplie
            if (fillAmount >= 1f)
            {
                isFilling = false;
                Debug.Log("Remplissage terminé.");
            }
        }
    }

    // Méthode publique pour démarrer le remplissage
    public void StartFilling()
    {
        if (!isFilling)
        {
            timer = 0f;
            radialImage.fillAmount = 0f;
            isFilling = true;
        }
    }

    // Optionnel : Méthode publique pour réinitialiser l'image
    public void ResetFill()
    {
        timer = 0f;
        radialImage.fillAmount = 0f;
        isFilling = false;
        StartFilling();
    }

    public void ThrowGrenade()
    {
        if (isFilling)
        {
            Debug.Log("Impossible de lancer la grenade tant que le remplissage n'est pas terminé.");
            return;
        }

        // Créer une grenade et la placer à la position du joueur
        GameObject grenade = Instantiate(grenadePrefab, playerPos.position, playerPos.rotation);
        Debug.Log("Grenade lancée !");
        ResetFill();
    }
}