using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SellSlotHandler : MonoBehaviour, IPointerClickHandler
{
    public Item item; // Item to be sold
    public Image slotImg;
    public TextMeshProUGUI itemCount;
    public InventoryManager inventoryManager; // Reference to InventoryManager
    public Item moneyItem; // Selectable money item from inspector

    private MerchBehaviour merchantBehaviour; // Reference to the merchant's behavior
    private int slotIndex; // Index of this slot in the merchant UI

    void Awake()
    {
        // Cerca InventoryManager nella scena se non è assegnato
        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
            if (inventoryManager == null)
            {
                Debug.LogError("rcavacca InventoryManager non trovato nella scena!");
            }
        }

        // Cerca MerchBehaviour nella scena se non è assegnato
        if (merchantBehaviour == null)
        {
            merchantBehaviour = FindObjectOfType<MerchBehaviour>();
            if (merchantBehaviour == null)
            {
                Debug.LogError("rcavacca MerchantBehaviour non trovato nella scena!");
            }
        }

        UpdateSlotUI();
    }

    // Update the UI for this sell slot
    private void UpdateSlotUI()
    {
        if (item != null)
        {
            slotImg.sprite = item.itemImg;
            itemCount.text = item.itemAmt.ToString();
            slotImg.gameObject.SetActive(true);
        }
        else
        {
            itemCount.text = string.Empty;
            slotImg.gameObject.SetActive(false);
        }
    }

    // On click, try to sell the item to the merchant
    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null || merchantBehaviour == null || moneyItem == null) return;

        int moneyReward = 2; // Default value
        
        // Find the correct money reward if slot index is valid
        for (int i = 0; i < merchantBehaviour.sellSlots.Length; i++)
        {
            if (merchantBehaviour.sellSlots[i] == gameObject)
            {
                slotIndex = i;
                if (slotIndex < merchantBehaviour.moneyPerSale.Length)
                {
                    moneyReward = merchantBehaviour.moneyPerSale[slotIndex];
                }
                break;
            }
        }

        if (inventoryManager.GetItemCount(item.itemID) >= 1) // Check if the player has the item to sell
        {
            // Perform the transaction: remove 1 item from the inventory and give money
            inventoryManager.RemoveItem(item.itemID, 1);
            inventoryManager.AddItemToInventory(new Item { itemID = moneyItem.itemID, itemAmt = moneyReward, itemImg = moneyItem.itemImg });

            Debug.Log($"Sold 1 {item.itemID} and received {moneyReward} {moneyItem.itemID}.");
        }
        else
        {
            Debug.Log("Not enough items to sell.");
        }
    }

    // Assign an item to this sell slot
    public void AssignItem(Item newItem)
    {
        item = newItem;
        UpdateSlotUI();
    }

    // Clear the item from this sell slot
    public void ClearSlot()
    {
        item = null;
        UpdateSlotUI();
    }
}
