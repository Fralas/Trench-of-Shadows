using UnityEngine;
using UnityEngine.SceneManagement;

public class HousePersistence : MonoBehaviour
{
    [SerializeField] GameObject house;
    public static bool comprato;

    private static HousePersistence instance; // Singleton

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene l'oggetto tra le scene
        }
        else
        {
            Destroy(gameObject); // Evita duplicati
        }
    }

    void Update()
    {
        Debug.Log("stato " + comprato);

        if (SceneManager.GetActiveScene().name != "Game")
        {
            house.SetActive(false);
            Debug.Log("diverso");
        }
        else
        {
            if (house.activeSelf)
            {
                Debug.Log("attivatoooo");
                comprato = true;
            }
            if (comprato)
            {
                Debug.Log("riattivo");
                house.SetActive(true);
            }
        }
    }
}
