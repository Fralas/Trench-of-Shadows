using UnityEngine;
using UnityEngine.SceneManagement;

public class LightPersist : MonoBehaviour
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
        if (SceneManager.GetActiveScene().name == "Game" && gameObject.name == "Lamps")
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
