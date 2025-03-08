using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldLightActivation : MonoBehaviour
{
    void Start()
    {
        CheckScene();
        SceneManager.sceneLoaded += OnSceneLoaded; 
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; 
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckScene();
    }

    void CheckScene()
    {
        if (SceneManager.GetActiveScene().name == "HomeLv1" ||
            SceneManager.GetActiveScene().name == "HomeLv2")
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
