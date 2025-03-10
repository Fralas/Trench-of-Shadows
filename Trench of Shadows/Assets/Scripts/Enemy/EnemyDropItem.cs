using UnityEngine;

public class EnemyDropItem : MonoBehaviour
{
    [SerializeField] private Item itemToDrop; // The item to drop
    [SerializeField] private int dropAmount = 1; // Amount of the item to drop
    [SerializeField] private InventoryManager playerInventory; // Reference to the player's inventory

    private void Start()
    {
        // Check if playerInventory is assigned.
        if (playerInventory == null)
        {
            Debug.LogError("Player Inventory is not assigned in the Inspector!");
        }
    }

    // Call this method when the enemy dies
    public void DropItem()
    {
        if (itemToDrop == null || playerInventory == null) return;

        for (int i = 0; i < dropAmount; i++)
        {
            AddItemToInventory(playerInventory);
        }

        Debug.Log(dropAmount + " " + itemToDrop.itemID + " added to the player's inventory.");
        Destroy(gameObject); // Destroy the enemy object after dropping the item
    }

    private void AddItemToInventory(InventoryManager inventory)
    {
        // Create a copy of the item to drop.
        Item itemCopy = itemToDrop.Clone();
        itemCopy.itemAmt = 1;

        // Search for an existing slot with the same item.
        foreach (Transform child in inventory.inventoryGrid.transform)
        {
            UISlotHandler slot = child.GetComponent<UISlotHandler>();
            if (slot.item != null && slot.item.itemID == itemCopy.itemID)
            {
                // If found, stack the item in that slot.
                inventory.StackInInventory(slot, itemCopy);
                Debug.Log(itemToDrop.itemID + " stacked in existing slot.");
                return; // Exit after stacking
            }
        }

        UISlotHandler emptySlot = FindEmptySlot(inventory);
        if (emptySlot != null)
        {
            inventory.PlaceInInventory(emptySlot, itemCopy);
        }
        else
        {
            Debug.Log("Player's inventory is full!");
        }
    }

    private UISlotHandler FindEmptySlot(InventoryManager inventory)
    {
        foreach (Transform child in inventory.inventoryGrid.transform)
        {
            UISlotHandler slot = child.GetComponent<UISlotHandler>();
            if (slot.item == null)
            {
                return slot;
            }
        }
        return null;
    }

    // You can call this method when the enemy's health reaches 0 or under
    public void OnEnemyDeath()
    {
        DropItem(); // Drop the item upon death
    }
}
