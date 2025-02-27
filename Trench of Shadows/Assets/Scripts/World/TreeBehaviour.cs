using System.Collections;
using UnityEngine;

public class TreeBehavior : MonoBehaviour
{
    public Item woodItem;
    public int woodAmount = 3;
    private bool isCutDown = false;
    private Transform player;
    private Animator animator;
    public InventoryManager playerInventory; // Reference to the player's inventory
    private PlayerController playerController;

    private float timeSinceLastPrint = 0f;
    public float printDelay = 1f;  // Delay in seconds for prints

    // Fields for sprite swapping
    public Sprite treeIdleSprite;
    public Sprite stumpSprite;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ensure we are correctly referencing the player's inventory and controller
        playerInventory = GameObject.Find("Manager").GetComponent<InventoryManager>();
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < 2f && !isCutDown)
        {
            timeSinceLastPrint += Time.deltaTime;

            if (timeSinceLastPrint >= printDelay)
            {
                Debug.Log("Player is close enough to cut the tree. Press 'E' to cut.");
                timeSinceLastPrint = 0f;
            }

            if (Input.GetKeyDown(KeyCode.E) && PlayerHasAxe())
            {
                Debug.Log("Player pressed 'E'. Cutting the tree...");
                CutDownTree();
            }
        }
    }

    private bool PlayerHasAxe()
    {
        if (playerInventory != null)
        {
            Item heldItem = playerInventory.GetHeldSlotItem();
            bool hasAxe = heldItem != null && heldItem.itemID == "Axe";
            Debug.Log("Held slot item: " + (heldItem != null ? heldItem.itemID : "None") + " | Has Axe: " + hasAxe);
            return hasAxe;
        }
        return false;
    }

    private void CutDownTree()
    {
        if (woodItem == null || isCutDown) return;

        isCutDown = true;
        // Set player cutting animation
        if (playerController != null)
        {
            playerController.animator.SetBool("isCutting", true);
        }

        // Immediately set the tree's "isCut" state to true.
        animator.SetBool("isCut", true);
        Debug.Log("Tree is now in 'isCut' state.");

        // Add wood to the player's inventory.
        if (playerInventory != null)
        {
            for (int i = 0; i < woodAmount; i++)
            {
                AddWoodToInventory(playerInventory);
            }
            Debug.Log($"{woodAmount} Wood added to the player's inventory.");

            // Decrease axe durability by 20 when cutting the tree.
            DecreaseAxeDurability();
        }
        else
        {
            Debug.LogWarning("Player inventory not found!");
        }

        // Start the coroutine to handle state transitions.
        StartCoroutine(TreeStateCoroutine());
    }

    private IEnumerator TreeStateCoroutine()
    {
        // Wait for 2 seconds before updating the state.
        yield return new WaitForSeconds(1.8f);

        // Set "isCut" to false and "TreeDead" to true.
        animator.SetBool("isCut", false); // This line should now be here to stop the cutting animation.
        animator.SetBool("TreeDead", true);
        Debug.Log("Tree state updated: 'isCut' = false, 'TreeDead' = true.");

        // Change the sprite to the stump.
        if (spriteRenderer != null && stumpSprite != null)
        {
            spriteRenderer.sprite = stumpSprite;
            Debug.Log("Tree sprite changed to stump.");
        }

        // Reset player cutting animation immediately after the tree sprite changes.
        if (playerController != null)
        {
            playerController.animator.SetBool("isCutting", false);
            Debug.Log("Player cutting animation stopped.");
        }

        // Wait for 5 seconds while in the TreeDead state.
        yield return new WaitForSeconds(5f);

        // Reset the animator's booleans back to idle (both false).
        animator.SetBool("TreeDead", false);
        animator.SetBool("isCut", false);
        Debug.Log("Tree state reset: 'TreeDead' and 'isCut' set to false.");

        // Reset the sprite back to the idle tree sprite.
        if (spriteRenderer != null && treeIdleSprite != null)
        {
            spriteRenderer.sprite = treeIdleSprite;
            Debug.Log("Tree sprite reset to idle.");
        }

        // Allow the tree to be cut again.
        isCutDown = false;
    }

    private void AddWoodToInventory(InventoryManager inventory)
    {
        Item woodCopy = woodItem.Clone();
        woodCopy.itemAmt = 1;

        foreach (Transform child in inventory.inventoryGrid.transform)
        {
            UISlotHandler slot = child.GetComponent<UISlotHandler>();
            if (slot.item != null && slot.item.itemID == woodCopy.itemID)
            {
                inventory.StackInInventory(slot, woodCopy);
                Debug.Log("Wood stacked in existing slot.");
                return;
            }
        }

        UISlotHandler emptySlot = FindEmptySlot(inventory);

        if (emptySlot != null)
        {
            inventory.PlaceInInventory(emptySlot, woodCopy);
            Debug.Log("Wood added to an empty slot in the player's inventory.");
        }
        else
        {
            Debug.Log("Player's inventory is full!");
        }
    }

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

    private void DecreaseAxeDurability()
    {
        Item heldItem = playerInventory.GetHeldSlotItem();
        if (heldItem != null && heldItem.itemID == "Axe")
        {
            heldItem.durability -= 20;
            Debug.Log("Axe durability decreased to: " + heldItem.durability);
            if (heldItem.durability <= 0)
            {
                playerInventory.RemoveHeldItem();
                Debug.Log("Axe broke and has been removed from the inventory.");
            }
            else
            {
                playerInventory.UpdateHeldSlotUI();
            }
        }
    }
}