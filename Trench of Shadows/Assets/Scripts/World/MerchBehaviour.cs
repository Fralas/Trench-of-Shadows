using UnityEngine;

public class MerchBehaviour : MonoBehaviour
{
    [Header("Merchant UI Settings")]
    public GameObject merchantUI;  // Reference to the merchant's UI
    private Transform player;
    public float interactionRange = 2f; // Distance required to interact

    private InventoryManager inventoryManager; // Reference to the InventoryManager

    // UI elements for sell slots
    public GameObject sellSlot1;
    public GameObject sellSlot2;
    public GameObject sellSlot3;

    // Inventory items the merchant sells
    private string[] itemsForSale = { "Wheat", "Wood", "Stone" }; // Example items, adjust as needed

    private void Start()
    {
        // Ensure the player object is found
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found!");
        }

        // Get the player's InventoryManager instance
        inventoryManager = GameObject.Find("Manager").GetComponent<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager not found!");
        }

        // Ensure sell slots are assigned
        if (sellSlot1 == null) Debug.LogError("SellSlot1 is not assigned!");
        if (sellSlot2 == null) Debug.LogError("SellSlot2 is not assigned!");
        if (sellSlot3 == null) Debug.LogError("SellSlot3 is not assigned!");

        // Set up UI interactions for sell slots
        if (sellSlot1 != null)
            sellSlot1.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => HandleTransaction(sellSlot1, 0));
        if (sellSlot2 != null)
            sellSlot2.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => HandleTransaction(sellSlot2, 1));
        if (sellSlot3 != null)
            sellSlot3.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => HandleTransaction(sellSlot3, 2));

        // Check if merchantUI is assigned
        if (merchantUI == null)
        {
            Debug.LogError("Merchant UI is not assigned!");
        }
    }

    private void Update()
    {
        if (player == null) return;

        // Check if the player is close enough to interact with the merchant
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < interactionRange)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ToggleMerchantUI();
            }
        }
        else
        {
            if (merchantUI != null && merchantUI.activeSelf)
            {
                merchantUI.SetActive(false);
            }
        }
    }

    private void ToggleMerchantUI()
    {
        if (merchantUI != null)
        {
            merchantUI.SetActive(!merchantUI.activeSelf); // Toggle visibility
        }
    }

    private void HandleTransaction(GameObject sellSlot, int itemIndex)
    {
        string itemToSell = itemsForSale[itemIndex];

        // Check if the player has enough of the item in their inventory using InventoryManager
        int itemCount = inventoryManager.GetItemCount(itemToSell);
        if (itemCount >= 1)  // Assuming 1 item is required for the transaction
        {
            // Remove 1 item from player's inventory and give 2 money items
            inventoryManager.RemoveItem(itemToSell, 1);
            inventoryManager.AddItemToInventory(new Item { itemID = "Money", itemAmt = 2 }); // You should have a predefined "Money" item

            Debug.Log($"Transaction successful! You received 2 money for selling {itemToSell}.");
        }
        else
        {
            Debug.Log("Not enough items for the transaction.");
        }
    }
}
