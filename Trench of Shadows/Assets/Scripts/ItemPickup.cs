using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Assign the InventoryItemData asset (for example, a Healing Potion) via the Inspector.
    public InventoryItemData itemData;
    // The amount of the item to pick up (default is 1)
    public int amount = 1;

    // This trigger is activated when another Collider2D enters the pickup's trigger area.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Colliding with: {collision.gameObject.name}, Tag: {collision.tag}");

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger!");
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
            if (inventoryManager != null)
            {
                inventoryManager.AddItem(itemData, amount);
            }
            else
            {
                Debug.LogError("No InventoryManager found in the scene!");
            }
            Destroy(gameObject);
        }
    }
}
