using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HarvestManager : MonoBehaviour
{
    public static HarvestManager Instance;
    public InventoryManager playerInventory;
    public Tilemap groundTilemap;
    public TileBase hoedTile;
    public TileBase wateredTile;
    public GameObject seedPrefab;

    private Dictionary<Vector3Int, bool> plantedCrops = new Dictionary<Vector3Int, bool>();
    private Animator playerAnimator;
    private PlayerController playerController;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsPlayerIdle())
        {
            Item heldItem = playerInventory?.GetHeldSlotItem();
            if (heldItem == null) return;

            Vector3Int playerTilePos = GetPlayerTilePosition();
            TileBase currentTile = groundTilemap.GetTile(playerTilePos);

            if (heldItem.itemID == "Hoe")
            {
                StartHarvesting();
                HoeGround(playerTilePos, currentTile);
            }
            else if (heldItem.itemID == "Wateringcan")
            {
                StartWatering();
                WaterGround(playerTilePos, currentTile);
            }
            else if (heldItem.itemID == "Seeds")
            {
                PlantSeed(playerTilePos, currentTile);
            }
        }
    }

    private bool IsPlayerIdle()
    {
        return !(playerAnimator.GetBool("isWalking") ||
                 playerAnimator.GetBool("isWalkingUpwards") ||
                 playerAnimator.GetBool("isWalkingDownwards") ||
                 playerAnimator.GetBool("isAttackingHorizontal") ||
                 playerAnimator.GetBool("isAttackingVertical") ||
                 playerAnimator.GetBool("isMining") ||
                 playerAnimator.GetBool("isCutting") ||
                 playerAnimator.GetBool("isHarvesting") ||
                 playerAnimator.GetBool("isWatering"));
    }

    private void StartHarvesting()
    {
        playerAnimator.SetBool("isHarvesting", true);
        playerController.SetHarvestingOrWateringState(true);
        Invoke("StopHarvesting", 1f);
    }

    private void StopHarvesting()
    {
        playerAnimator.SetBool("isHarvesting", false);
        playerController.SetHarvestingOrWateringState(false);
    }

    private void StartWatering()
    {
        playerAnimator.SetBool("isWatering", true);
        playerController.SetHarvestingOrWateringState(true);
        Invoke("StopWatering", 1f);
    }

    private void StopWatering()
    {
        playerAnimator.SetBool("isWatering", false);
        playerController.SetHarvestingOrWateringState(false);
    }

    private void HoeGround(Vector3Int position, TileBase currentTile)
    {
        if (currentTile != hoedTile && currentTile != wateredTile)
        {
            groundTilemap.SetTile(position, hoedTile);
        }
    }

    private void WaterGround(Vector3Int position, TileBase currentTile)
    {
        if (currentTile == hoedTile)
        {
            groundTilemap.SetTile(position, wateredTile);
        }
    }

    private void PlantSeed(Vector3Int position, TileBase currentTile)
    {
        if (plantedCrops.ContainsKey(position) || currentTile != wateredTile) return;

        Vector3 spawnPosition = groundTilemap.GetCellCenterWorld(position);
        Instantiate(seedPrefab, spawnPosition, Quaternion.identity);
        plantedCrops[position] = true;
    }

    public void ResetTile(Vector3Int position)
    {
        if (plantedCrops.Remove(position))
        {
            groundTilemap.SetTile(position, hoedTile);
        }
    }

    private Vector3Int GetPlayerTilePosition()
    {
        return groundTilemap.WorldToCell(transform.position);
    }
}