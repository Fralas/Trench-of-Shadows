using System.Collections;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public Sprite[] growthStages;
    public float growthTime = 5f;
    public Item cornItem;
    private SpriteRenderer spriteRenderer;
    private int currentStage = 0;
    private Transform player;
    public InventoryManager playerInventory;
    private Vector3Int tilePosition;
    private HarvestManager harvestManager;

    [SerializeField] private GameObject cornItemPrefab; // Prefab da clonare
    [SerializeField] private Transform ParentReference; // Contenitore nella gerarchia

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerInventory = GameObject.Find("Manager").GetComponent<InventoryManager>();
        harvestManager = GameObject.FindObjectOfType<HarvestManager>();
        tilePosition = harvestManager.groundTilemap.WorldToCell(transform.position);

        if (growthStages.Length > 0)
        {
            spriteRenderer.sprite = growthStages[currentStage];
            StartCoroutine(GrowCrop());
        }
        else
        {
            Debug.LogError("Nessuna immagine di crescita assegnata!");
        }
    }

    private IEnumerator GrowCrop()
    {
        while (currentStage < growthStages.Length - 1)
        {
            yield return new WaitForSeconds(growthTime);
            currentStage++;
            spriteRenderer.sprite = growthStages[currentStage];
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckForNearbyCrop();
        }
    }

    void CheckForNearbyCrop()
    {
        float maxDistance = 0.5f;
        Collider2D cropCollider = Physics2D.OverlapCircle(transform.position, maxDistance, LayerMask.GetMask("Player"));

        if (cropCollider != null)
        {
            CropCut();
        }
    }

void CropCut()
{
    if (currentStage == growthStages.Length - 1)
    {
        Debug.Log("Raccolto ottenuto!");
        AddCropToInventory(playerInventory);
        harvestManager.ResetTile(tilePosition); // Reset terreno

        // Clona il prefab dell'oggetto e imposta il suo genitore
        GameObject clonedCrop = Instantiate(gameObject); // Crea una copia dell'oggetto
        clonedCrop.transform.SetParent(harvestManager.transform); // Imposta il genitore (cambia 'harvestManager.transform' con l'oggetto che vuoi come genitore)
        clonedCrop.transform.localPosition = Vector3.zero; // Imposta la posizione locale a (0,0,0) rispetto al genitore

        // Distruggi l'oggetto originale
        Destroy(gameObject);
    }
    else
    {
        Debug.Log("Il raccolto non è ancora pronto!");
    }
}




    private void AddCropToInventory(InventoryManager inventory)
    {
        if (cornItemPrefab == null || inventory == null || ParentReference == null)
        {
            Debug.LogWarning("Prefab del raccolto, inventario o ParentReference non validi!");
            return;
        }

        // Istanziare l'oggetto raccolto e posizionarlo dentro ParentReference
        GameObject cropClone = Instantiate(cornItemPrefab, ParentReference);
        cropClone.transform.localPosition = Vector3.zero; // Reset posizione locale

        Item cropCopy = cornItem.Clone();
        cropCopy.itemAmt = 1;

        UISlotHandler emptySlot = FindEmptySlot(inventory);

        if (emptySlot != null)
        {
            inventory.PlaceInInventory(emptySlot, cropCopy);
            Debug.Log("Crop aggiunto all'inventario!");
        }
        else
        {
            Debug.Log("Inventario pieno!");
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
