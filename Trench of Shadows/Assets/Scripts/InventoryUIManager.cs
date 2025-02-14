using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    // References to the player and chest inventory UIs
    public GameObject playerInventoryUI;
    public GameObject chestInventoryUI;

    // RectTransforms for adjusting inventory positions
    public RectTransform playerInventoryRect;
    public RectTransform chestInventoryRect;

    // Start is called before the first frame update
    void Start()
    {
        // Position inventories and ensure they are initially set to the correct state
        PositionPlayerInventoryAtBottom();
        PositionChestInventoryAtTop();
    }

    // Method to open both inventories at the same time
    public void OpenInventories()
    {
        // Make both inventories visible
        playerInventoryUI.SetActive(true);
        chestInventoryUI.SetActive(true);

        // Adjust positions of inventories to prevent overlap
        PositionPlayerInventoryAtBottom();
        PositionChestInventoryAtTop();
    }

    // Method to close both inventories at the same time
    public void CloseInventories()
    {
        // Hide both inventories
        playerInventoryUI.SetActive(false);
        chestInventoryUI.SetActive(false);
    }

    // Position the player inventory at the bottom of the screen
    private void PositionPlayerInventoryAtBottom()
    {
        playerInventoryRect.anchorMin = new Vector2(0, 0);     // Bottom-left corner
        playerInventoryRect.anchorMax = new Vector2(1, 0);     // Stretch across the full width
        playerInventoryRect.anchoredPosition = Vector2.zero;   // Position at the bottom of the screen
    }

    // Position the chest inventory at the top of the screen
    private void PositionChestInventoryAtTop()
    {
        chestInventoryRect.anchorMin = new Vector2(0, 1);      // Top-left corner
        chestInventoryRect.anchorMax = new Vector2(1, 1);      // Stretch across the full width
        chestInventoryRect.anchoredPosition = new Vector2(0, 0); // Position at the top of the screen
    }
}
