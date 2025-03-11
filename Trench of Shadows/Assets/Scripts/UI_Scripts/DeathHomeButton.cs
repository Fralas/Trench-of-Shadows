using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathHomeButton : MonoBehaviour
{
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private PlayerDatas player;
    [SerializeField] private PlayerRespawn playerRespawn; // Reference to PlayerRespawn
    [SerializeField] private InventoryManager inventoryManager; // Reference to InventoryManager

    private Transform startingPoint;
    public static bool hasRespawned = false; // Global variable to signal respawn

    private void Start()
    {
        LevelChanger levelChanger = FindObjectOfType<LevelChanger>();
        if (levelChanger != null)
        {
            startingPoint = GameObject.Find("StartingPoint")?.transform;
        }
    }

    /// <summary>
    /// Respawn nella stessa scena
    /// </summary>
    public void RespawnInGame()
    {
        Debug.Log("🔄 Attempting to respawn...");

        // Heal the player
        if (player != null)
        {
            player.HealFull();
            player.ConsumeHunger(-player.MaxHunger); // Reset della fame
        }

        // Reset player position to the starting point
        if (startingPoint != null && player != null)
        {
            player.transform.position = startingPoint.position;
            Debug.Log($"➡️ Player returned to {startingPoint.position}");
        }

        // Hide the death screen
        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
            Time.timeScale = 1f; // Unpause the game
        }

        // Signal that the player has respawned
        hasRespawned = true;

        // Trigger respawn logic from PlayerRespawn
        playerRespawn.RespawnPlayer(); // Manually call the respawn method
        
        // Now we delay the scene reload by a small amount of time to ensure everything is cleared
        StartCoroutine(ReloadSceneAfterDelay(1f));
    }

    /// <summary>
    /// Torna alla schermata home
    /// </summary>
    public void LoadHomeScene()
    {
        Debug.Log("🏠 Returning to Home Screen...");
        
        // Ripristina il tempo di gioco
        Time.timeScale = 1f;

        // Carica la scena della home (sostituisci "HomeScene" con il nome corretto della tua scena home)
        SceneManager.LoadScene("Home");
    }

    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        if (!gameObject.activeSelf)
        {
            yield break; // Exit if the GameObject is inactive
        }

        yield return new WaitForSeconds(delay);

        // Reload the scene to reset everything
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
