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

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Ensure we are correctly referencing the player's inventory
        playerInventory = GameObject.Find("Manager").GetComponent<InventoryManager>();
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

            if (Input.GetKeyDown(KeyCode.E) && PlayerHasPickaxe())
            {
                Debug.Log("Player pressed 'E'. Mining the stone...");
                MineStone();
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
        }
        else
        {
            Debug.LogWarning("Player inventory not found!");
        }

        Destroy(gameObject, 2f);
    }

    private void AddStoneToInventory(InventoryManager inventory)
    {
        Item stoneCopy = stoneItem.Clone();
        stoneCopy.itemAmt = 1;

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
}
