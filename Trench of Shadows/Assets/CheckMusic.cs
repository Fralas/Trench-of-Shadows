using UnityEngine;
using UnityEngine.SceneManagement;


public class CheckMusic : MonoBehaviour
{

    private static CheckMusic instance;
    void Start()
    {
        CheckScene();
        SceneManager.sceneLoaded += OnSceneLoaded; 
    

        GameObject existingMusic = GameObject.Find("Music");
        
        if (existingMusic == null)
        {
            gameObject.SetActive(true);
        }
        else if (existingMusic != gameObject)
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
        if (SceneManager.GetActiveScene().name == "Cave")
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    
}
