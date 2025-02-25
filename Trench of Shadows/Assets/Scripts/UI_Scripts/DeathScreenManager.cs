using UnityEngine;
public class DeathScreenManager : MonoBehaviour
{
    [SerializeField]
    private GameObject deathScreenUI; // The death screen UI panel

    [SerializeField]
    private PlayerDatas player; // Reference to PlayerDatas

    [SerializeField] private InventoryManager inventoryManager; // Reference to InventoryManager
    [SerializeField] private PlayerRespawn playerRespawn; // Reference to PlayerRespawn script

    private void Start()
    {
        deathScreenUI.SetActive(false);

        player.Died.AddListener(ShowDeathScreen);
    }

    private void ShowDeathScreen()
    {
        deathScreenUI.SetActive(true);
        Time.timeScale = 0f; // Freeze the game when the player dies
    }
}
