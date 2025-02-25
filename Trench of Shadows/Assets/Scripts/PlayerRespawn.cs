using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private GameObject respawnPoint; // Reference to the GameObject you want to use as respawn
    public InventoryManager inventoryManager; // Reference to the InventoryManager to call ClearPlayerInventory

    public void RespawnPlayer() // Public method to manually respawn the player
    {
        if (respawnPoint != null)
        {
            StartCoroutine(RespawnCoroutine());
            inventoryManager.ClearPlayerInventory();
        }
        else
        {
            Debug.LogWarning("Respawn point not set!");
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(1f); // Optional delay before respawn

        // Use the position of the respawnPoint GameObject for the respawn
        transform.position = respawnPoint.transform.position;

        // Reset health and hunger
        PlayerDatas.Instance.HealFull();
        PlayerDatas.Instance.AdjustHunger(PlayerDatas.Instance.MaxHunger);
    }
}
