using UnityEngine;

public class StoneBehavior : MonoBehaviour
{
    public Item stoneItem;
    public int stoneAmount = 3;
    private bool isMined = false;
    private Transform player;
    public InventoryManager playerInventory; // Reference to the player's inventory

    private float timeSinceLastPrint = 0f;
    public float printDelay = 1f;  // Delay in seconds for prints

    private Animator playerAnimator;
    private PlayerController playerController; // Reference to the player controller

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerInventory = GameObject.Find("Manager").GetComponent<InventoryManager>();

        // Get the player's animator and controller
        playerAnimator = player.GetComponent<Animator>();
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < 2f && !isMined)
        {
            timeSinceLastPrint += Time.deltaTime;

            if (timeSinceLastPrint >= printDelay)
            {
                Debug.Log("Player is close enough to mine the stone. Press 'E' to mine.");
                timeSinceLastPrint = 0f;
            }

            if (Input.GetKeyDown(KeyCode.E) && PlayerHasPickaxe() && IsPlayerIdle())
            {
                Debug.Log("Player pressed 'E'. Mining the stone...");
                StartMining();
            }
        }
    }

    private bool PlayerHasPickaxe()
    {
        if (playerInventory != null)
        {
            Item heldItem = playerInventory.GetHeldSlotItem();
            bool hasPickaxe = heldItem != null && heldItem.itemID == "Pickaxe";
            Debug.Log("Held slot item: " + (heldItem != null ? heldItem.itemID : "None") + " | Has Pickaxe: " + hasPickaxe);
            return hasPickaxe;
        }
        return false;
    }

    private void StartMining()
    {
        // Disable player movement
        if (playerController != null)
        {
            playerController.SetHarvestingOrWateringState(true);
        }

        // Set the player's animator to mining state
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isMining", true);
        }

        // Begin mining stone
        MineStone();

        // After a short time, stop the mining animation and re-enable movement
        Invoke("StopMining", 1f); // Assuming the mining takes 1 second
    }

    private void StopMining()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isMining", false);
        }
        
        // Re-enable player movement
        if (playerController != null)
        {
            playerController.SetHarvestingOrWateringState(false);
        }
    }

    private void MineStone()
    {
        if (stoneItem == null || isMined) return;

        isMined = true;

        Debug.Log("Mining stone...");

        // Add stone to the player's inventory
        if (playerInventory != null)
        {
            for (int i = 0; i < stoneAmount; i++)
            {
                AddStoneToInventory(playerInventory);
            }
            Debug.Log($"{stoneAmount} Stone added to the player's inventory.");

            // Decrease the pickaxe durability by 20 for mining the stone
            DecreasePickaxeDurability();
        }
        else
        {
            Debug.LogWarning("Player inventory not found!");
        }

        Destroy(gameObject, 2f);
    }

    private void AddStoneToInventory(InventoryManager inventory)
    {
        // Check if the inventory already contains the stone item.
        Item stoneCopy = stoneItem.Clone();
        stoneCopy.itemAmt = 1;

        // Search for an existing slot with the same stone item
        foreach (Transform child in inventory.inventoryGrid.transform)
        {
            UISlotHandler slot = child.GetComponent<UISlotHandler>();
            if (slot.item != null && slot.item.itemID == stoneCopy.itemID)
            {
                // If found, stack the stone in that slot.
                inventory.StackInInventory(slot, stoneCopy);
                Debug.Log("Stone stacked in existing slot.");
                return; // Exit after stacking
            }
        }

        // If no existing stone item is found, add it to an empty slot
        UISlotHandler emptySlot = FindEmptySlot(inventory);

        if (emptySlot != null)
        {
            inventory.PlaceInInventory(emptySlot, stoneCopy);
            Debug.Log("Stone added to an empty slot in the player's inventory.");
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

    // New method: Decrease pickaxe durability by 20 each time the stone is mined.
    private void DecreasePickaxeDurability()
    {
        Item heldItem = playerInventory.GetHeldSlotItem();
        if (heldItem != null && heldItem.itemID == "Pickaxe")
        {
            heldItem.durability -= 20;
            Debug.Log("Pickaxe durability decreased to: " + heldItem.durability);
            if (heldItem.durability <= 0)
            {
                playerInventory.RemoveHeldItem();
                Debug.Log("Pickaxe broke and has been removed from the inventory.");
            }
            else
            {
                playerInventory.UpdateHeldSlotUI();
            }
        }
    }

    // Check if the player is idle (all animation booleans are false)
    private bool IsPlayerIdle()
    {
        return !(playerAnimator.GetBool("isWalking") ||
                 playerAnimator.GetBool("isAttackingHorizontal") ||
                 playerAnimator.GetBool("isAttackingVertical") ||
                 playerAnimator.GetBool("isWalkingUpwards") ||
                 playerAnimator.GetBool("isWalkingDownwards") ||
                 playerAnimator.GetBool("isMining") ||
                 playerAnimator.GetBool("isCutting") ||
                 playerAnimator.GetBool("isHarvesting") ||
                 playerAnimator.GetBool("isWatering"));
    }
}
