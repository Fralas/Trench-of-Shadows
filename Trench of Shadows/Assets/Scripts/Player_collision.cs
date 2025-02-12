using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public int goToSceneIndex;  // Indice della scena da caricare
    public Vector2 spawnPosition; // Coordinate di spawn nella nuova scena

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Scene changer entered");

        if (other.CompareTag("Player"))  // 🔹 Usa CompareTag per prestazioni migliori
        {
            Debug.Log("Player entered door. Saving spawn position: " + spawnPosition);
            if (PlayerDatas.instance != null)
            {
                PlayerDatas.instance.SetSpawnPosition(spawnPosition);
            }

            Debug.Log("Switching to scene: " + goToSceneIndex);
            SceneManager.LoadScene(goToSceneIndex, LoadSceneMode.Single); // 🔹 Corretto
        }
    }
}