using UnityEngine;
using UnityEngine.UI;

public class SpawnGrenade : MonoBehaviour
{
    public Transform playerPos;

    public GameObject grenadePrefab;

    [Header("Param�tres de l'image")]
    public Image radialImage; 

    [Header("Dur�e du remplissage (en secondes)")]
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
            Debug.LogError("L'image doit �tre configur�e en mode Filled pour que le script fonctionne.");
            return;
        }

        radialImage.fillAmount = 0f; //image � vide
        Debug.Log("Image remise � zero.");
    }

    private void Update()
    {
        if (isFilling)
        {
            // Augmente le timer et met � jour l'image
            timer += Time.deltaTime;
            float fillAmount = Mathf.Clamp01(timer / duration);
            radialImage.fillAmount = fillAmount;

            // Arr�ter une fois l'image compl�tement remplie
            if (fillAmount >= 1f)
            {
                isFilling = false;
                Debug.Log("Remplissage termin�.");
            }
        }
    }

    // M�thode publique pour d�marrer le remplissage
    public void StartFilling()
    {
        if (!isFilling)
        {
            timer = 0f;
            radialImage.fillAmount = 0f;
            isFilling = true;
        }
    }

    // Optionnel : M�thode publique pour r�initialiser l'image
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
            Debug.Log("Impossible de lancer la grenade tant que le remplissage n'est pas termin�.");
            return;
        }

        // Cr�er une grenade et la placer � la position du joueur
        GameObject grenade = Instantiate(grenadePrefab, playerPos.position, playerPos.rotation);
        Debug.Log("Grenade lanc�e !");
        ResetFill();
    }
}