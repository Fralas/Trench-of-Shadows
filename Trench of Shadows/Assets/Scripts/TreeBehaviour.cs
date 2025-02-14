using UnityEngine;

public class TreeBehavior : MonoBehaviour
{
    public Item woodItem;
    public int woodAmount = 3;
    private bool isCutDown = false;
    private Transform player;
    private Animator animator;

    private float timeSinceLastPrint = 0f;
    public float printDelay = 1f;  // Delay in seconds for prints

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player == null) return;

        // Calculate distance to the player
        float distance = Vector2.Distance(transform.position, player.position);

        // Check if the player is within cutting range and if the tree hasn't been cut yet
        if (distance < 2f && !isCutDown)
        {
            timeSinceLastPrint += Time.deltaTime;

            // If the player is close enough and the delay has passed, show the print message
            if (timeSinceLastPrint >= printDelay)
            {
                Debug.Log("Player is close enough to cut the tree. Press 'E' to cut.");
                timeSinceLastPrint = 0f;  // Reset the timer
            }

            // If 'E' is pressed and the player has an axe, proceed to cut the tree
            if (Input.GetKeyDown(KeyCode.E) && PlayerHasAxe())
            {
                Debug.Log("Player pressed 'E'. Cutting the tree...");
                CutDownTree();
            }
        }
    }

    // Check if the player has an axe in their inventory
    private bool PlayerHasAxe()
    {
        InventoryManager inventory = FindObjectOfType<InventoryManager>();
        if (inventory != null)
        {
            // Print out all items in the inventory to check if the axe is there
            foreach (Transform child in inventory.inventoryGrid.transform)
            {
                UISlotHandler slot = child.GetComponent<UISlotHandler>();
                if (slot.item != null)
                {
                    Debug.Log($"Item in slot: {slot.item.itemID}");
                }
            }

            // Check if the axe is in the inventory
            bool hasAxe = inventory.HasItem("Axe");
            Debug.Log($"Player has axe: {hasAxe}");
            return hasAxe;
        }
        return false;
    }


    // Cut the tree and add wood to inventory
    private void CutDownTree()
    {
        if (woodItem == null || isCutDown) return;

        // Set the flag to avoid cutting the tree multiple times
        isCutDown = true;

        // Log to confirm the method is being called
        Debug.Log("Cutting tree...");

        // Play the cut animation
        animator.SetTrigger("isCut");

        // Add the specified amount of wood to the player's inventory
        InventoryManager inventory = FindObjectOfType<InventoryManager>();
        for (int i = 0; i < woodAmount; i++)
        {
            AddWoodToInventory(inventory);
        }

        Debug.Log($"{woodAmount} Wood added to the inventory.");
        
        // Destroy the tree after the animation is finished (adjust time if needed)
        Destroy(gameObject, 2f);
    }

    // Add wood to the inventory
    private void AddWoodToInventory(InventoryManager inventory)
    {
        Item woodCopy = woodItem.Clone();
        woodCopy.itemAmt = 1;
        
        // Find an empty slot in the inventory to place the wood
        UISlotHandler emptySlot = FindEmptySlot(inventory);

        if (emptySlot != null)
        {
            inventory.PlaceInInventory(emptySlot, woodCopy);
            Debug.Log("Wood added to an empty slot.");
        }
        else
        {
            Debug.Log("Inventory is full!");
        }
    }

    // Find an empty slot in the inventory
    private UISlotHandler FindEmptySlot(InventoryManager inventory)
    {
        foreach (Transform child in inventory.inventoryGrid.transform)
        {
            UISlotHandler slot = child.GetComponent<UISlotHandler>();
            if (slot.item == null)
            {
                return slot;
            }
        }
        return null;
    }
}
