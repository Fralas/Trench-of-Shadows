using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FishingBehaviour : MonoBehaviour
{
    public InventoryManager playerInventory;
    public Tilemap waterTilemap;
    public TileBase fishingTile; // The tile where fishing is allowed
    public Item fishItem; // The item to add to the inventory
    public int fishAmount = 1; // Number of fish received per successful fishing
    public AudioClip fishingSound; // Sound played when fishing starts

    private Animator playerAnimator;
    private Transform player;
    private PlayerController playerController;
    private AudioSource audioSource;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerAnimator = player.GetComponent<Animator>();
        playerController = player.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PlayerHasFishingRod())
        {
            Vector3Int playerTilePos = GetPlayerTilePosition();
            TileBase currentTile = waterTilemap.GetTile(playerTilePos);
            
            if (currentTile == fishingTile)
            {
                StartCoroutine(FishRoutine());
            }
            else
            {
                Debug.Log("You can only fish in designated fishing areas!");
            }
        }
    }

    private bool PlayerHasFishingRod()
    {
        if (playerInventory != null)
        {
            Item heldItem = playerInventory.GetHeldSlotItem();
            return heldItem != null && heldItem.itemID == "FishingRod";
        }
        return false;
    }

    private IEnumerator FishRoutine()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isHarvesting", true);
        }

        if (playerController != null)
        {
            playerController.SetHarvestingOrWateringState(true);
        }

        if (fishingSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(fishingSound);
        }

        Debug.Log("Fishing...");
        yield return new WaitForSeconds(8f); // Fishing animation now lasts 8 seconds

        AddFishToInventory();

        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isHarvesting", false);
        }

        if (playerController != null)
        {
            playerController.SetHarvestingOrWateringState(false);
        }
    }

    private void AddFishToInventory()
    {
        if (playerInventory != null && fishItem != null)
        {
            for (int i = 0; i < fishAmount; i++)
            {
                Item fishCopy = fishItem.Clone();
                fishCopy.itemAmt = 1;

                UISlotHandler emptySlot = FindEmptySlot(playerInventory);
                if (emptySlot != null)
                {
                    playerInventory.PlaceInInventory(emptySlot, fishCopy);
                    Debug.Log("Fish added to inventory.");
                }
                else
                {
                    Debug.Log("Inventory full! Can't add more fish.");
                }
            }
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

    private Vector3Int GetPlayerTilePosition()
    {
        Vector3 playerWorldPos = player.position;
        return waterTilemap.WorldToCell(playerWorldPos);
    }
}
