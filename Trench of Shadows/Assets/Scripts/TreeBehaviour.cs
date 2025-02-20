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

    private float timeSinceLastPrint = 0f;
    public float printDelay = 1f;  // Delay in seconds for prints

    // New fields for sprite swapping
    public Sprite treeIdleSprite;
    public Sprite stumpSprite;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ensure we are correctly referencing the player's inventory
        playerInventory = GameObject.Find("Manager").GetComponent<InventoryManager>();
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
        }
        else
        {
            Debug.LogWarning("Player inventory not found!");
        }

        // Start the coroutine that will wait and then update the state.
        StartCoroutine(TreeStateCoroutine());
    }

    private IEnumerator TreeStateCoroutine()
    {
        // Wait for 2 seconds.
        yield return new WaitForSeconds(2f);

        // Set "isCut" to false and "TreeDead" to true.
        animator.SetBool("isCut", false);
        animator.SetBool("TreeDead", true);
        Debug.Log("Tree state updated: 'isCut' = false, 'TreeDead' = true.");

        // Change the sprite to the stump.
        if (spriteRenderer != null && stumpSprite != null)
        {
            spriteRenderer.sprite = stumpSprite;
            Debug.Log("Tree sprite changed to stump.");
        }
    }

    private void AddWoodToInventory(InventoryManager inventory)
    {
        Item woodCopy = woodItem.Clone();
        woodCopy.itemAmt = 1;

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
}
