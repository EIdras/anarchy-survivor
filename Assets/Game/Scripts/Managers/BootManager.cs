using UnityEngine;
using UnityEngine.SceneManagement;

public class BootManager : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    void Start()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

}
