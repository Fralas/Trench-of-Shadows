using UnityEngine;
using UnityEngine.SceneManagement;

public class HousePersist : MonoBehaviour
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
        if (SceneManager.GetActiveScene().name == "Game" && gameObject.name == "House_lev1" ||
            SceneManager.GetActiveScene().name == "Game" && gameObject.name == "House_lev2" ||
            SceneManager.GetActiveScene().name == "Game" && gameObject.name == "House_lev3" ||
            SceneManager.GetActiveScene().name == "Game" && gameObject.name == "House_lev4")
        {
            gameObject.SetActive(true);
            Debug.Log("AO ATTIVATO ER BAMBOCCIO: " + gameObject.name);
        }
        else
        {
            gameObject.SetActive(false);
            Debug.Log("AO DISATTIVATO ER BAMBOCCIO: " + gameObject.name);
        }
    }
}
