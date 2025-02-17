using UnityEngine;

public class CraftingInteraction : MonoBehaviour
{
    [Header("Crafting Table UI Settings")]
    public GameObject craftInventoryUI;  // Reference to crafting table's inventory UI
    private Transform player;
    private float timeSinceLastPrint = 0f;
    public float printDelay = 1f;  // Delay for showing messages
    public float interactionRange = 2f; // Distance required to interact

    private Vector2 craftInitialPosition; // Store the initial position of the crafting UI

    private GameObject playerInventoryBackground; // Reference to the player inventory background
    private GameObject craftingInventoryBackground; // Reference to the crafting table inventory background
    private GameObject recipeBackground; // Reference to the recipe background

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Reference to the player inventory background
        playerInventoryBackground = GameObject.Find("InventoryBackground");

        // Reference to the crafting inventory background
        craftingInventoryBackground = GameObject.Find("CraftingBackground");

        // Reference to the recipe inventory background
        recipeBackground = GameObject.Find("RecipeBackground");

        // Debug log for any missing references
        if (craftInventoryUI == null)
        {
            Debug.LogError("Craft Inventory UI is not assigned!");
        }
        if (playerInventoryBackground == null)
        {
            Debug.LogError("Player Inventory Background is not found!");
        }
        if (craftingInventoryBackground == null)
        {
            Debug.LogError("Crafting Background is not found!");
        }
        if (recipeBackground == null)
        {
            Debug.LogError("Recipe Background is not found!");
        }
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
                Debug.Log("Press 'I' to open the Crafting Table.");
                timeSinceLastPrint = 0f;  // Reset the timer
            }

            // Trigger the crafting table UI toggle when 'I' is pressed
            if (Input.GetKeyDown(KeyCode.I))
            {
                OpenCraftInventory();  // Open the crafting inventory if near the crafting table
            }
        }
        else
        {
            // If the player is not near the crafting table, just ensure the crafting inventory is closed
            if (craftInventoryUI != null && craftInventoryUI.activeSelf)
            {
                craftInventoryUI.SetActive(false);
            }
        }
    }

    private void OpenCraftInventory()
    {
        // Only open the crafting inventory if it's not already active
        if (craftInventoryUI != null && !craftInventoryUI.activeSelf)
        {
            craftInventoryUI.SetActive(true);
            Debug.Log("Crafting inventory opened.");

            // Ensure both player and crafting inventories are active before resizing
            if (playerInventoryBackground != null && !playerInventoryBackground.activeSelf)
            {
                playerInventoryBackground.SetActive(true);
                Debug.Log("Player inventory activated.");
            }
            if (craftingInventoryBackground != null && !craftingInventoryBackground.activeSelf)
            {
                craftingInventoryBackground.SetActive(true);
                Debug.Log("Crafting inventory activated.");
            }
            if (recipeBackground != null && !recipeBackground.activeSelf)
            {
                recipeBackground.SetActive(true);
                Debug.Log("Recipe background activated.");
            }

            // Resize both player and crafting inventories
            RectTransform playerInvRect = playerInventoryBackground?.GetComponent<RectTransform>();
            RectTransform chestInvRect = craftingInventoryBackground?.GetComponent<RectTransform>();  // Using craftingInventoryBackground's RectTransform

            if (playerInvRect != null && chestInvRect != null)
            {
                // Resize both inventories
                ResizeInventory(playerInvRect);
                ResizeInventory(chestInvRect);

                // Optionally move the crafting inventory relative to the player inventory
                if (craftInitialPosition == Vector2.zero)
                {
                    float additionalOffset = 40f; // Adjust this value to control the distance
                    craftInitialPosition = new Vector2(
                        playerInvRect.anchoredPosition.x + playerInvRect.rect.width + 20 + additionalOffset, // Added offset
                        playerInvRect.anchoredPosition.y
                    );
                    chestInvRect.anchoredPosition = craftInitialPosition;
                    Debug.Log($"Crafting UI moved to: {chestInvRect.anchoredPosition}");
                }
            }
            else
            {
                Debug.LogWarning("Failed to find Player Inventory or Crafting Inventory RectTransforms.");
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
