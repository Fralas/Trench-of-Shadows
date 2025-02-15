using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    [Header("Chest UI Settings")]
    public GameObject chestInventoryUI;  // Reference to chest's inventory UI
    private Transform player;
    private float timeSinceLastPrint = 0f;
    public float printDelay = 1f;  // Delay for showing messages
    public float interactionRange = 2f; // Distance required to interact

    private Vector2 chestInitialPosition; // Store the initial position of the chest UI

    private GameObject playerInventoryBackground; // Reference to the player inventory background
    private GameObject chestInventoryBackground; // Reference to the chest inventory background

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Reference to the player inventory background
        playerInventoryBackground = GameObject.Find("InventoryBackground");

        // Reference to the chest inventory background
        chestInventoryBackground = GameObject.Find("ChestInventoryBackground");
    }

    private void Update()
    {
        if (player == null) return;

        // Calculate distance to the player
        float distance = Vector2.Distance(transform.position, player.position);

        // Check if the player is within interaction range
        if (distance < interactionRange)
        {
            timeSinceLastPrint += Time.deltaTime;

            // Show the interaction message with a delay
            if (timeSinceLastPrint >= printDelay)
            {
                Debug.Log("Press 'I' to open the chest.");
                timeSinceLastPrint = 0f;  // Reset the timer
            }

            // Trigger the chest inventory toggle when 'I' is pressed
            if (Input.GetKeyDown(KeyCode.I))
            {
                OpenChestInventory();  // Open the chest inventory if near the chest
            }
        }
        else
        {
            // If the player is not near a chest, just ensure the chest inventory is closed
            if (chestInventoryUI != null && chestInventoryUI.activeSelf)
            {
                chestInventoryUI.SetActive(false);
            }
        }
    }

    private void OpenChestInventory()
    {
        // Only open the chest inventory if it's not already active
        if (chestInventoryUI != null && !chestInventoryUI.activeSelf)
        {
            chestInventoryUI.SetActive(true);
            Debug.Log("Chest inventory opened.");

            // Ensure both player and chest inventories are active before resizing
            if (playerInventoryBackground != null && !playerInventoryBackground.activeSelf)
            {
                playerInventoryBackground.SetActive(true);
                Debug.Log("Player inventory activated.");
            }
            if (chestInventoryBackground != null && !chestInventoryBackground.activeSelf)
            {
                chestInventoryBackground.SetActive(true);
                Debug.Log("Chest inventory activated.");
            }

            // Resize both player and chest inventories
            RectTransform playerInvRect = playerInventoryBackground?.GetComponent<RectTransform>();
            RectTransform chestInvRect = chestInventoryBackground?.GetComponent<RectTransform>();  // Using chestInventoryBackground's RectTransform

            if (playerInvRect != null && chestInvRect != null)
            {
                // Resize both inventories
                ResizeInventory(playerInvRect);
                ResizeInventory(chestInvRect);

                // Optionally move the chest inventory relative to the player inventory
                if (chestInitialPosition == Vector2.zero)
                {
                    float additionalOffset = 40f; // Adjust this value to control the distance
                    chestInitialPosition = new Vector2(
                        playerInvRect.anchoredPosition.x + playerInvRect.rect.width + 20 + additionalOffset, // Added offset
                        playerInvRect.anchoredPosition.y
                    );
                    chestInvRect.anchoredPosition = chestInitialPosition;
                    Debug.Log($"Chest UI moved to: {chestInvRect.anchoredPosition}");
                }
            }
            else
            {
                Debug.LogWarning("Failed to find Player Inventory or Chest Inventory RectTransforms.");
            }
        }
    }

    private void ResizeInventory(RectTransform inventoryRect)
    {
        if (inventoryRect != null)
        {
            // Resize to a smaller UI size (adjust values as needed)
            inventoryRect.sizeDelta = new Vector2(80, 85); // Example size for both inventories
            Debug.Log($"Resized inventory to: {inventoryRect.sizeDelta} on {inventoryRect.gameObject.name}");
        }
        else
        {
            Debug.LogError("Inventory RectTransform is null!");
        }
    }
}
