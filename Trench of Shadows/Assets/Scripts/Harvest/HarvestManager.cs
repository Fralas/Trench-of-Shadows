using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HarvestManager : MonoBehaviour
{
    public InventoryManager playerInventory;
    public Tilemap groundTilemap;
    public TileBase hoedTile;
    public TileBase wateredTile;
    public GameObject seedPrefab;

    private Dictionary<Vector3Int, bool> plantedCrops = new Dictionary<Vector3Int, bool>();

    private Animator playerAnimator;

    void Start()
    {
        // Get the player's animator to trigger harvesting and watering animations
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Item heldItem = playerInventory?.GetHeldSlotItem();
            Debug.Log("Oggetto in mano: " + (heldItem != null ? heldItem.itemID : "Nessuno"));
            if (heldItem == null)
            {
                Debug.Log("Nessun oggetto in mano.");
                return;
            }

            Vector3Int playerTilePos = GetPlayerTilePosition();
            TileBase currentTile = groundTilemap.GetTile(playerTilePos);

            Debug.Log("Tile corrente: " + (currentTile != null ? currentTile.name : "Nessuna tile trovata"));

            if (heldItem.itemID == "Hoe")
            {
                StartHarvesting();
                HoeGround(playerTilePos, currentTile);
            }
            else if (heldItem.itemID == "Wateringcan")
            {
                WaterGround(playerTilePos, currentTile);
            }
            else if (heldItem.itemID == "Seeds")
            {
                PlantSeed(playerTilePos, currentTile);
            }
        }
    }

    private void StartHarvesting()
    {
        // Set the player's animator to harvesting state
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isHarvesting", true);
        }

        // Disable movement
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetHarvestingOrWateringState(true);

        // After a short delay, stop the harvesting animation and enable movement
        Invoke("StopHarvesting", 1f); // Assuming harvesting takes 1 second
    }

    private void StopHarvesting()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isHarvesting", false);
        }

        // Enable movement again
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetHarvestingOrWateringState(false);
    }

    private void StartWatering()
    {
        // Set the player's animator to watering state
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isWatering", true);
        }

        // Disable movement
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetHarvestingOrWateringState(true);

        // After a short delay, stop the watering animation and enable movement
        Invoke("StopWatering", 1f); // Assuming watering takes 1 second
    }

    private void StopWatering()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isWatering", false);
        }

        // Enable movement again
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetHarvestingOrWateringState(false);
    }

    private void HoeGround(Vector3Int position, TileBase currentTile)
    {
        if (currentTile != hoedTile && currentTile != wateredTile)
        {
            groundTilemap.SetTile(position, hoedTile);
        }
        else
        {
            Debug.Log("Il terreno è già lavorato!");
        }
    }

    private void WaterGround(Vector3Int position, TileBase currentTile)
    {
        if (currentTile == hoedTile)
        {
            StartWatering();
            groundTilemap.SetTile(position, wateredTile);

        }
        else
        {
            Debug.Log("Puoi innaffiare solo terreno zappato!");
        }
    }

    private void PlantSeed(Vector3Int position, TileBase currentTile)
    {
        if (plantedCrops.ContainsKey(position))
        {
            Debug.Log("Ci sono già dei semi piantati qui!");
            return;
        }

        if (currentTile == wateredTile)
        {
            Vector3 spawnPosition = groundTilemap.GetCellCenterWorld(position);
            GameObject newCrop = Instantiate(seedPrefab, spawnPosition, Quaternion.identity);
            plantedCrops[position] = true;
            Debug.Log("Semi piantati in posizione: " + spawnPosition);
        }
        else
        {
            Debug.Log("Puoi piantare solo su terreno bagnato!");
        }
    }

    public void ResetTile(Vector3Int position)
    {
        if (plantedCrops.ContainsKey(position))
        {
            plantedCrops.Remove(position);
            groundTilemap.SetTile(position, hoedTile);
            Debug.Log("Terreno ripristinato dopo il raccolto.");
        }
    }

    private Vector3Int GetPlayerTilePosition()
    {
        Vector3 playerWorldPos = transform.position;
        return groundTilemap.WorldToCell(playerWorldPos);
    }
}
