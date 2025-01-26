using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootManager : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private Image loadingScreen;
    
    private float fadeProgress = 0f;
    [SerializeField] private float fadeSpeed = 0.2f;
    private void Update()
    {
        if (fadeProgress < 1)
        {
            fadeProgress += Time.deltaTime * fadeSpeed;
            float alpha = Mathf.Pow(fadeProgress, 2); // Utilisation d'une interpolation quadratique
            loadingScreen.color = new Color(loadingScreen.color.r, loadingScreen.color.g, loadingScreen.color.b, alpha);
        }
        else
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

}
