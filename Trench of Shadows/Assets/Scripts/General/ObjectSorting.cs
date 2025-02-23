using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectSorting : MonoBehaviour
{
    [SerializeField] private Transform player;
    
    [Header("Seleziona il tipo di Renderer")]
    [SerializeField] private bool useTilemapRenderer = true; // Se true usa TilemapRenderer, altrimenti SpriteRenderer
    [SerializeField] private TilemapRenderer tilemapRenderer;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float objectY;

    private void Start()
    {
        // Seleziona automaticamente il renderer se non è assegnato
        if (useTilemapRenderer)
        {
            if (tilemapRenderer == null) tilemapRenderer = GetComponent<TilemapRenderer>();
            if (tilemapRenderer == null)
            {
                Debug.LogError("[ObjectSorting] TilemapRenderer non trovato!");
                return;
            }
        }
        else
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("[ObjectSorting] SpriteRenderer non trovato!");
                return;
            }
        }

        // Recupera la posizione dal centro del BoxCollider2D, se presente
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            objectY = collider.bounds.center.y;
        }
        else
        {
            objectY = transform.position.y; // Se non c'è un collider, usa la posizione dell'oggetto
        }

        Debug.Log($"[ObjectSorting] Oggetto Y: {objectY}");
    }

    private void Update()
    {
        if (player == null) return;

        Debug.Log($"[ObjectSorting] Player Y: {player.position.y}, Oggetto Y: {objectY}");

        // Determina se l'oggetto deve stare sopra o sotto il player
        string newSortingLayer = player.position.y < objectY ? "behindPlayer" : "abovePlayer";

        // Applica il sorting layer corretto al renderer selezionato
        if (useTilemapRenderer && tilemapRenderer != null)
        {
            tilemapRenderer.sortingLayerName = newSortingLayer;
        }
        else if (!useTilemapRenderer && spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = newSortingLayer;
        }
    }
}
