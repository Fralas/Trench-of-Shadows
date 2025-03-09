using UnityEngine;
using UnityEngine.UI;

public class MerchBehaviour : MonoBehaviour
{
    [Header("Merchant UI Settings")]
    public GameObject merchantUI;  // Reference to the merchant's UI
    public GameObject interactionPrompt; // UI prompt when near merchant
    
    private Transform player;
    public float interactionRange = 2f; // Distance required to interact
    private InventoryManager inventoryManager; // Reference to the InventoryManager

    // UI elements for sell slots
    public GameObject sellSlot1;
    public GameObject sellSlot2;
    public GameObject sellSlot3;

    // Inventory items the merchant sells
    private string[] itemsForSale = { "Wheat", "Wood", "Stone"}; // Example items, adjust as needed

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) Debug.LogError("Player not found!");

        inventoryManager = GameObject.Find("Manager")?.GetComponent<InventoryManager>();
        if (inventoryManager == null) Debug.LogError("InventoryManager not found!");

        AssignSellSlot(sellSlot1, 0);
        AssignSellSlot(sellSlot2, 1);
        AssignSellSlot(sellSlot3, 2);
        
        if (merchantUI == null) Debug.LogError("Merchant UI is not assigned!");
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < interactionRange)
        {
            if (interactionPrompt != null && !interactionPrompt.activeSelf)
                interactionPrompt.SetActive(true);

            if (Input.GetKeyDown(KeyCode.I))
                ToggleMerchantUI();
        }
        else
        {
            if (interactionPrompt != null && interactionPrompt.activeSelf)
                interactionPrompt.SetActive(false);
            
            if (merchantUI != null && merchantUI.activeSelf)
                merchantUI.SetActive(false);
        }
    }

    private void ToggleMerchantUI()
    {
        if (merchantUI != null)
        {
            merchantUI.SetActive(!merchantUI.activeSelf);
        }
    }

    private void AssignSellSlot(GameObject sellSlot, int itemIndex)
    {
        if (sellSlot == null)
        {
            Debug.LogError($"SellSlot{itemIndex + 1} is not assigned!");
            return;
        }
        sellSlot.GetComponent<Button>().onClick.AddListener(() => HandleTransaction(itemIndex));
    }

    private void HandleTransaction(int itemIndex)
    {
        string itemToSell = itemsForSale[itemIndex];
        int itemCount = inventoryManager.GetItemCount(itemToSell);

        if (itemCount >= 1)
        {
            inventoryManager.RemoveItem(itemToSell, 1);
            inventoryManager.AddItemToInventory(new Item { itemID = "Money", itemAmt = 2 });
            Debug.Log($"Transaction successful! You received 2 money for selling {itemToSell}.");
        }
        else
        {
            Debug.Log("Not enough items for the transaction.");
        }
    }
}
