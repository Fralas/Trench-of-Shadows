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

    private MerchBehaviour merchantBehaviour; // Reference to the merchant's behavior

/*
    void Awake()
    {
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager reference is not assigned in " + gameObject.name);
        }
        if (merchantBehaviour == null)
        {
            Debug.LogError("MerchantBehaviour reference is not assigned in " + gameObject.name);
        }
        UpdateSlotUI();
    }*/

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
        if (item == null) return;

        if (inventoryManager.GetItemCount(item.itemID) >= 1) // Check if the player has the item to sell
        {
            // Perform the transaction: remove 1 item from the inventory and give money
            inventoryManager.RemoveItem(item.itemID, 1);
            inventoryManager.AddItemToInventory(new Item { itemID = "Money", itemAmt = 2 });

            Debug.Log($"Sold 1 {item.itemID} and received 2 money.");
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

