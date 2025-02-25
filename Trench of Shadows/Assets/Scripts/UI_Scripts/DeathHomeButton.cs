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

    public void LoadHomeScene()
    {
        Debug.Log("🔄 Attempting to respawn...");

        // Heal the player
        if (player != null)
        {
            player.HealFull();
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
            Time.timeScale = 1f; // Freeze the game when the player dies
        }

        // Signal that the player has respawned
        hasRespawned = true;

        // Trigger respawn logic from PlayerRespawn
        playerRespawn.RespawnPlayer(); // Manually call the respawn method
        
        // Now we delay the scene reload by a small amount of time to ensure everything is cleared
        StartCoroutine(ReloadSceneAfterDelay(1f));
    }

    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        // Make sure this GameObject is active before starting the coroutine
        if (!gameObject.activeSelf)
        {
            yield break; // Exit if the GameObject is inactive
        }

        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Reload the scene to reset everything
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
