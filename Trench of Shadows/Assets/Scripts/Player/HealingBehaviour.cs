using UnityEngine;

public class HealingBehaviour : MonoBehaviour
{
    public InventoryManager playerInventory;
    public string[] healingItemIDs; // Array of item IDs that can heal the player
    public int healAmount = 10; // Amount of HP restored per use

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PlayerHasHealingItem())
        {
            HealPlayer();
        }
    }

    private bool PlayerHasHealingItem()
    {
        if (playerInventory != null)
        {
            Item heldItem = playerInventory.GetHeldSlotItem();
            if (heldItem != null)
            {
                foreach (string id in healingItemIDs)
                {
                    if (heldItem.itemID == id)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HealPlayer()
    {
        PlayerDatas.Instance.Heal(healAmount);
        Debug.Log($"Player healed by {healAmount} HP!");

        // Remove one instance of the healing item from inventory
        Item heldItem = playerInventory.GetHeldSlotItem();
        if (heldItem != null)
        {
            playerInventory.RemoveHeldItem();
            Debug.Log("Used one healing item.");
        }
    }
}
