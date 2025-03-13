using UnityEngine;
using UnityEngine.UI;

public class MerchBehaviour : MonoBehaviour
{
    [Header("Merchant UI Settings")]
    public GameObject merchantUI;
    public GameObject interactionPrompt;
    
    private Transform player;
    public float interactionRange = 2f;
    private InventoryManager inventoryManager;

    // UI elements for sell slots
    public GameObject[] sellSlots = new GameObject[8];

    // Inventory items the merchant sells
    private string[] itemsForSale = { "Stone", "Wheat", "Wood", "RawFish", "RawMeat", "Skull", "Gold", "Copper"};
    
    // Money received per item sold
    public int[] moneyPerSale = { 2, 3, 5, 10, 15, 4, 6, 7 }; // Adjustable money values per slot

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) Debug.LogError("Player not found!");

        inventoryManager = GameObject.Find("Manager")?.GetComponent<InventoryManager>();
        if (inventoryManager == null) Debug.LogError("InventoryManager not found!");

        for (int i = 0; i < sellSlots.Length; i++)
        {
            AssignSellSlot(sellSlots[i], i);
        }
        
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
            Debug.LogError($"SellSlot {itemIndex + 1} is not assigned!");
            return;
        }
        sellSlot.GetComponent<Button>().onClick.AddListener(() => HandleTransaction(itemIndex));
    }

    private void HandleTransaction(int itemIndex)
    {
        if (itemIndex >= itemsForSale.Length || itemIndex >= moneyPerSale.Length) return;

        string itemToSell = itemsForSale[itemIndex];
        int itemCount = inventoryManager.GetItemCount(itemToSell);

        if (itemCount >= 1)
        {
            inventoryManager.RemoveItem(itemToSell, 1);
            inventoryManager.AddItemToInventory(new Item { itemID = "Money", itemAmt = moneyPerSale[itemIndex] });
            Debug.Log($"Transaction successful! You received {moneyPerSale[itemIndex]} money for selling {itemToSell}.");
        }
        else
        {
            Debug.Log("Not enough items for the transaction.");
        }
    }
}