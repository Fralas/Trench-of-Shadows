using UnityEngine;

public class TreeBehavior : MonoBehaviour
{
    public Item woodItem;
    public int woodAmount = 3;
    private bool isCutDown = false;
    private Transform player;
    private Animator animator;
    public InventoryManager playerInventory; // Reference to the player's inventory
    public InventoryManager chestInventory;  // Reference to the chest inventory

    private float timeSinceLastPrint = 0f;
    public float printDelay = 1f;  // Delay in seconds for prints

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        playerInventory = GameObject.Find("Manager").GetComponent<InventoryManager>();
        chestInventory = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();

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
            // Check if the player has an axe in their inventory
            bool hasAxe = playerInventory.HasItem("Axe");
            Debug.Log($"Player has axe: {hasAxe}");
            return hasAxe;
        }
        return false;
    }

    private void CutDownTree()
    {
        if (woodItem == null || isCutDown) return;

        isCutDown = true;

        Debug.Log("Cutting tree...");
        animator.SetTrigger("isCut");

        // **Add wood to the chest inventory instead of the player's inventory**
        if (playerInventory != null)
        {
            for (int i = 0; i < woodAmount; i++)
            {
                AddWoodToInventory(playerInventory);
            }
            Debug.Log($"{woodAmount} Wood added to the player's inventory.");
        }
        else
        {
            Debug.LogWarning("Player inventory not found!");
        }

        Destroy(gameObject, 2f);
    }

    private void AddWoodToInventory(InventoryManager inventory)
    {
        Item woodCopy = woodItem.Clone();
        woodCopy.itemAmt = 1;

        UISlotHandler emptySlot = FindEmptySlot(inventory);

        if (emptySlot != null)
        {
            inventory.PlaceInInventory(emptySlot, woodCopy);
            Debug.Log("Wood added to an empty slot in the chest.");
        }
        else
        {
            Debug.Log("Chest inventory is full!");
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
