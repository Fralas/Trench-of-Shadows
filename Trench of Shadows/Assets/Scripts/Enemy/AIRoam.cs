using UnityEngine;

public class AIRoam : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    
    // New fields for randomizing roam time and stop time
    [SerializeField] private float minRoamTime = 3f;
    [SerializeField] private float maxRoamTime = 8f;
    
    [SerializeField] private float minStopTime = 2f;
    [SerializeField] private float maxStopTime = 5f;
    
    [SerializeField] private float directionChangeInterval = 3f; // Time interval to pick a new direction (if needed)
    [SerializeField] private Item RawMeatItem; // RawMeat item to drop
    [SerializeField] private int RawMeatAmount = 1; // Amount of RawMeat dropped

    [SerializeField] private InventoryManager playerInventory; // Assign in Inspector

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 movementDirection;
    private float roamTimer;
    private float stopTimer;
    private bool isMoving = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Check if the playerInventory has been assigned.
        if (playerInventory == null)
        {
            Debug.LogError("Player Inventory is not assigned in the Inspector!");
        }
        
        PickNewDirection();
    }

    void Update()
    {
        if (isMoving)
        {
            roamTimer -= Time.deltaTime;
            if (roamTimer <= 0)
            {
                StopMoving();
            }
        }
        else
        {
            stopTimer -= Time.deltaTime;
            if (stopTimer <= 0)
            {
                PickNewDirection();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            rb.velocity = movementDirection * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        
        // Ensure the Z-axis rotation stays at 0
        transform.rotation = Quaternion.Euler(0, 0, 0);
        
        // Update animation state
        animator.SetBool("isWalking", isMoving);
        
        // Flip sprite based on movement direction
        if (movementDirection.x != 0)
        {
            spriteRenderer.flipX = movementDirection.x > 0;
        }
    }

    private void PickNewDirection()
    {
        movementDirection = Random.insideUnitCircle.normalized;
        // Randomize the roam time between the minimum and maximum values.
        roamTimer = Random.Range(minRoamTime, maxRoamTime);
        isMoving = true;
    }

    private void StopMoving()
    {
        // Randomize the stop time between the minimum and maximum values.
        stopTimer = Random.Range(minStopTime, maxStopTime);
        isMoving = false;
    }

    public void DropRawMeat()
    {
        if (RawMeatItem == null || playerInventory == null) return;

        for (int i = 0; i < RawMeatAmount; i++)
        {
            AddRawMeatToInventory(playerInventory);
        }

        Debug.Log(RawMeatAmount + " RawMeat added to the player's inventory.");
        Destroy(gameObject);
    }

    private void AddRawMeatToInventory(InventoryManager inventory)
    {
        // Create a copy of the RawMeat item.
        Item rawMeatCopy = RawMeatItem.Clone();
        rawMeatCopy.itemAmt = 1;

        // Search for an existing slot with the same RawMeat item.
        foreach (Transform child in inventory.inventoryGrid.transform)
        {
            UISlotHandler slot = child.GetComponent<UISlotHandler>();
            if (slot.item != null && slot.item.itemID == rawMeatCopy.itemID)
            {
                // If found, stack the RawMeat in that slot.
                inventory.StackInInventory(slot, rawMeatCopy);
                Debug.Log("RawMeat stacked in existing slot.");
                return; // Exit after stacking
            }
        }

        UISlotHandler emptySlot = FindEmptySlot(inventory);
        if (emptySlot != null)
        {
            inventory.PlaceInInventory(emptySlot, rawMeatCopy);
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
