using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    // Reference to the player's inventory UI
    public GameObject playerInventoryUI;
    public RectTransform playerInventoryRect;

    // List to hold references to all chest inventory UIs
    public GameObject[] chestInventories;
    public RectTransform[] chestInventoryRects;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize player inventory and chest inventories
        PositionPlayerInventoryAtBottom();
        PositionChestInventoriesAtTop();

        // Make sure all chests' inventories are hidden by default
        foreach (var chest in chestInventories)
        {
            chest.SetActive(false);
        }
    }

    // Call this method when interacting with a chest
    public void ToggleChestInventory(int chestIndex)
    {
        // Ensure we stay within bounds
        if (chestIndex < 0 || chestIndex >= chestInventories.Length)
        {
            Debug.LogWarning("Invalid chest index!");
            return;
        }

        // Toggle the chest inventory visibility
        bool isChestActive = chestInventories[chestIndex].activeSelf;

        // Close all chests before opening the selected one
        CloseAllChestInventories();

        if (!isChestActive)
        {
            chestInventories[chestIndex].SetActive(true); // Open the selected chest
            PositionChestInventoryAtTop(chestIndex);      // Position it at the top
        }
    }

    // Close all chest inventories
    private void CloseAllChestInventories()
    {
        foreach (var chest in chestInventories)
        {
            chest.SetActive(false);  // Hide all chests' inventories
        }
    }

    // Position the player inventory at the bottom of the screen
    private void PositionPlayerInventoryAtBottom()
    {
        playerInventoryRect.anchorMin = new Vector2(0, 0);     // Bottom-left corner
        playerInventoryRect.anchorMax = new Vector2(1, 0);     // Stretch across the full width
        playerInventoryRect.anchoredPosition = Vector2.zero;   // Position at the bottom of the screen
    }

    // Position all chest inventories at the top of the screen
    private void PositionChestInventoriesAtTop()
    {
        for (int i = 0; i < chestInventoryRects.Length; i++)
        {
            chestInventoryRects[i].anchorMin = new Vector2(0, 1);  // Top-left corner
            chestInventoryRects[i].anchorMax = new Vector2(1, 1);  // Stretch across the full width
            chestInventoryRects[i].anchoredPosition = new Vector2(0, 0); // Position at the top
        }
    }

    // Position a specific chest inventory at the top
    private void PositionChestInventoryAtTop(int chestIndex)
    {
        if (chestIndex >= 0 && chestIndex < chestInventoryRects.Length)
        {
            chestInventoryRects[chestIndex].anchorMin = new Vector2(0, 1);  // Top-left corner
            chestInventoryRects[chestIndex].anchorMax = new Vector2(1, 1);  // Stretch across the full width
            chestInventoryRects[chestIndex].anchoredPosition = new Vector2(0, 0); // Position at the top
        }
    }
}
