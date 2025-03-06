using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectSorting : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private TilemapRenderer tilemapRenderer;
    private float objectY;
    
    [SerializeField] private bool useTilemapRenderer = false; // Se usi una tilemap

    private void Start()
    {
        FindPlayer(); // Trova il player automaticamente

        if (useTilemapRenderer)
        {
            tilemapRenderer = GetComponent<TilemapRenderer>();
        }
        else
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Recupera la posizione dell'oggetto per il sorting
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        objectY = (collider != null) ? collider.bounds.center.y : transform.position.y;

        UpdateSortingLayer();
    }

    private void Update()
    {
        if (player == null) FindPlayer(); // Riconnetti il player se è stato perso
        if (player == null) return;

        UpdateSortingLayer();

        // Apply sorting to all NPCs in the scene
        ApplySortingToNPCs();
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("[ObjectSorting] Nessun oggetto con tag 'Player' trovato!");
        }
    }

    private void UpdateSortingLayer()
    {
        if (player == null) return;

        string newSortingLayer = player.position.y < objectY ? "behindPlayer" : "abovePlayer";

        if (useTilemapRenderer && tilemapRenderer != null)
        {
            tilemapRenderer.sortingLayerName = newSortingLayer;
        }
        else if (!useTilemapRenderer && spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = newSortingLayer;
        }
    }

    private void ApplySortingToNPCs()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject npc in npcs)
        {
            var npcSorting = npc.GetComponent<ObjectSorting>();
            if (npcSorting != null)
            {
                npcSorting.UpdateSortingLayer();
            }
        }
    }
}
