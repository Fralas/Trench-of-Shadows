using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HarvestManager : MonoBehaviour
{
    public InventoryManager playerInventory;
    public Tilemap groundTilemap;
    public TileBase hoedTile;       // Tile del terreno zappato
    public TileBase wateredTile;    // Tile del terreno innaffiato
    public GameObject seedPrefab;   // Prefab della piantina



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

    private void HoeGround(Vector3Int position, TileBase currentTile)
    {
        if (groundTilemap == null || hoedTile == null)
        {
            Debug.LogError("Errore: GroundTilemap o HoedTile non assegnato!");
            return;
        }

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
        if (groundTilemap == null || wateredTile == null)
        {
            Debug.LogError("Errore: GroundTilemap o WateredTile non assegnato!");
            return;
        }

        if (currentTile == hoedTile)
        {
            groundTilemap.SetTile(position, wateredTile);
        }
        else
        {
            Debug.Log("Puoi innaffiare solo terreno zappato! (Tile attuale: " + (currentTile != null ? currentTile.name : "Nessuna") + ")");
        }
    }

    private void PlantSeed(Vector3Int position, TileBase currentTile)
{
    if (seedPrefab == null)
    {
        Debug.LogError("Errore: SeedPrefab non assegnato!");
        return;
    }

    if (currentTile == wateredTile)
    {
        Vector3 spawnPosition = groundTilemap.GetCellCenterWorld(position);
        GameObject newCrop = Instantiate(seedPrefab, spawnPosition, Quaternion.identity);

        Crop cropScript = newCrop.GetComponent<Crop>(); // Recupera lo script della pianta
        if (cropScript == null)
        {
            Debug.LogError("Errore: Il prefab del seme non ha lo script Crop!");
        }

        Debug.Log("Semi piantati in posizione: " + spawnPosition);
    }
    else
    {
        Debug.Log("Puoi piantare solo su terreno bagnato!");
    }
}



    private Vector3Int GetPlayerTilePosition()
    {
        Vector3 playerWorldPos = transform.position;
        return groundTilemap.WorldToCell(playerWorldPos);
    }


}