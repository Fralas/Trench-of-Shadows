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

        // Avvia una coroutine per ignorare la collisione dopo un frame
        StartCoroutine(IgnoreCollisionWithPlayer(newCrop));

        Debug.Log("Semi piantati in posizione: " + spawnPosition);
    }
    else
    {
        Debug.Log("Puoi piantare solo su terreno bagnato!");
    }
}

private IEnumerator IgnoreCollisionWithPlayer(GameObject crop)
{
    yield return null; // Aspetta un frame per assicurarsi che il collider sia inizializzato

    Collider2D cropCollider = crop.GetComponent<Collider2D>();
    Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Collider2D>();
    if (cropCollider != null && playerCollider != null)
    {
        Physics2D.IgnoreCollision(cropCollider, playerCollider);
        Debug.Log("Collisione ignorata tra il seme e il Player.");
    }
}

    private Vector3Int GetPlayerTilePosition()
    {
        Vector3 playerWorldPos = transform.position;
        return groundTilemap.WorldToCell(playerWorldPos);
    }
}