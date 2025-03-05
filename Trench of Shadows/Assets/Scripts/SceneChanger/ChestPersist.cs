using UnityEngine;
using UnityEngine.SceneManagement;

public class ChestPersist : MonoBehaviour
{
    private string sceneOfOrigin;

    void Awake()
    {
        sceneOfOrigin = SceneManager.GetActiveScene().name;
        DontDestroyOnLoad(gameObject); 
        //Debug.Log("AAAAA SCENA CHEST: " + sceneOfOrigin);
    }

    public string GetSceneOfOrigin()
    {
        return sceneOfOrigin;
    }

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
        if (SceneManager.GetActiveScene().name == sceneOfOrigin)
        {
            gameObject.SetActive(true);
            Debug.Log("chest attiva in scena: " + SceneManager.GetActiveScene().name);
        }
        else
        {
            gameObject.SetActive(false);
            Debug.Log("chest disattivata in scena: " + SceneManager.GetActiveScene().name);
        }
    }
}
