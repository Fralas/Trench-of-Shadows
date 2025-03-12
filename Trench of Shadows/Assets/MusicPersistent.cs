using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPersistent : MonoBehaviour
{

    private static MusicPersistent instance;

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
