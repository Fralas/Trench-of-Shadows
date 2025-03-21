using UnityEngine;
using UnityEngine.SceneManagement;

public class UIActivation : MonoBehaviour
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
        if (SceneManager.GetActiveScene().name == "Game" ||
            SceneManager.GetActiveScene().name == "Cave" ||
            SceneManager.GetActiveScene().name == "HomeLv1" ||
            SceneManager.GetActiveScene().name == "HomeLv2" ||
            SceneManager.GetActiveScene().name == "HomeLv3" ||
            SceneManager.GetActiveScene().name == "HomeLv4" ||
            SceneManager.GetActiveScene().name == "Blacksmith_Shop" ||
            SceneManager.GetActiveScene().name == "Carpenter_Shop" ||
            SceneManager.GetActiveScene().name == "Grocery_Shop" ||
            SceneManager.GetActiveScene().name == "game_test_fra")
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
