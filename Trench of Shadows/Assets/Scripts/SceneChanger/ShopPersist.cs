using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC_Persist : MonoBehaviour
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
        if (SceneManager.GetActiveScene().name == "Blacksmith_Shop" && gameObject.name == "Blacksmith" ||
            SceneManager.GetActiveScene().name == "Carpenter_Shop" && gameObject.name == "RealEstate" ||
            SceneManager.GetActiveScene().name == "Grocery_Shop" && gameObject.name == "Merchant")
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
