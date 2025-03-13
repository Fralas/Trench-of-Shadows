using System.Collections;
using UnityEngine;

public class TreeBehavior : MonoBehaviour
{
    public Item woodItem;
    public int woodAmount = 3;
    private bool isCutDown = false;
    private Transform player;
    private Animator animator;
    public InventoryManager playerInventory;
    private PlayerController playerController;

    private float timeSinceLastPrint = 0f;
    public float printDelay = 1f;

    public Sprite treeIdleSprite;
    public Sprite stumpSprite;
    private SpriteRenderer spriteRenderer;

    // New variables for audio
    public AudioClip cuttingSound;  // Reference to the cutting sound
    private AudioSource audioSource; // Audio source to play the sound

    public AudioClip axeBreakSound; // Sound for when the axe breaks

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerInventory = GameObject.Find("Manager").GetComponent<InventoryManager>();
        playerController = player.GetComponent<PlayerController>();

        // Get or add an AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
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

            if (Input.GetKeyDown(KeyCode.E) && PlayerHasAxe() && IsPlayerIdle())
            {
                Debug.Log("Player pressed 'E'. Cutting the tree...");
                CutDownTree();
            }
        }
    }

    private bool IsPlayerIdle()
    {
        if (playerController == null) return false;

        bool isIdle = !playerController.animator.GetBool("isWalking") &&
                      !playerController.animator.GetBool("isWalkingUpwards") &&
                      !playerController.animator.GetBool("isWalkingDownwards") &&
                      !playerController.animator.GetBool("isAttackingHorizontal") &&
                      !playerController.animator.GetBool("isAttackingVertical") &&
                      !playerController.animator.GetBool("isMining") &&
                      !playerController.animator.GetBool("isCutting") &&
                      !playerController.animator.GetBool("isHarvesting") &&
                      !playerController.animator.GetBool("isWatering");

        return isIdle;
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

        if (playerController != null)
        {
            playerController.animator.SetBool("isCutting", true);
            playerController.SetHarvestingOrWateringState(true); // Lock player movement
        }

        animator.SetBool("isCut", true);
        Debug.Log("Tree is now in 'isCut' state.");

        // Play the cutting sound
        if (cuttingSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(cuttingSound);
        }

        if (playerInventory != null)
        {
            for (int i = 0; i < woodAmount; i++)
            {
                AddWoodToInventory(playerInventory);
            }
            Debug.Log($"{woodAmount} Wood added to the player's inventory.");
            DecreaseAxeDurability();
        }
        else
        {
            Debug.LogWarning("Player inventory not found!");
        }

        StartCoroutine(TreeStateCoroutine());
    }

    private IEnumerator TreeStateCoroutine()
    {
        yield return new WaitForSeconds(1.8f);

        animator.SetBool("isCut", false);
        animator.SetBool("TreeDead", true);
        Debug.Log("Tree state updated: 'isCut' = false, 'TreeDead' = true.");

        if (spriteRenderer != null && stumpSprite != null)
        {
            spriteRenderer.sprite = stumpSprite;
            Debug.Log("Tree sprite changed to stump.");
        }

        if (playerController != null)
        {
            playerController.animator.SetBool("isCutting", false);
            playerController.SetHarvestingOrWateringState(false); // Unlock player movement
            Debug.Log("Player cutting animation stopped.");
        }

        yield return new WaitForSeconds(5f);

        animator.SetBool("TreeDead", false);
        animator.SetBool("isCut", false);
        Debug.Log("Tree state reset: 'TreeDead' and 'isCut' set to false.");

        if (spriteRenderer != null && treeIdleSprite != null)
        {
            spriteRenderer.sprite = treeIdleSprite;
            Debug.Log("Tree sprite reset to idle.");
        }

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
            heldItem.durability -= 5;
            Debug.Log("Axe durability decreased to: " + heldItem.durability);

            if (heldItem.durability <= 0)
            {
                playerInventory.RemoveHeldItem();
                Debug.Log("Axe broke and has been removed from the inventory.");

                // Play axe breaking sound
                if (axeBreakSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(axeBreakSound);
                }
            }
            else
            {
                playerInventory.UpdateHeldSlotUI();
            }
        }
    }

}
