using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UISlotHandler : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item;
    public Image slotImg;
    public TextMeshProUGUI itemCount;
    public InventoryManager inventoryManager;

    // For drag-and-drop support
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector3 originalPosition;

    void Awake()
    {
        if (item != null)
        {
            item = item.Clone();
            slotImg.sprite = item.itemImg;
            itemCount.text = item.itemAmt.ToString();
            slotImg.gameObject.SetActive(true);
        }
        else
        {
            itemCount.text = string.Empty;
            slotImg.gameObject.SetActive(false);
        }
        
        // Ensure the slot has a CanvasGroup component for proper drag handling
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        // When clicking an item slot, update the recipe display
        if (item != null)
        {
            RecipeDisplayManager.instance.ShowRecipe(item);
        }
        else
        {
            RecipeDisplayManager.instance.ClearRecipeDisplay();
        }
    }

    // DRAG & DROP METHODS

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;

        originalParent = transform.parent;
        originalPosition = transform.position;
        
        transform.SetParent(transform.root); // Move to root to avoid masking issues
        canvasGroup.blocksRaycasts = false; // Prevent raycasting issues while dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            transform.position = eventData.position; // Follow the mouse
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Return to original position if not dropped on a valid slot
        if (transform.parent == transform.root)
        {
            transform.SetParent(originalParent);
            transform.position = originalPosition;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        UISlotHandler sourceSlot = eventData.pointerDrag.GetComponent<UISlotHandler>();

        if (sourceSlot != null && sourceSlot != this)
        {
            if (item == null) // If this slot is empty, just move the item
            {
                inventoryManager.PlaceInInventory(this, sourceSlot.item);
                sourceSlot.inventoryManager.ClearItemSlot(sourceSlot);
            }
            else // Swap items if both slots have an item
            {
                Item temp = item;
                inventoryManager.PlaceInInventory(this, sourceSlot.item);
                sourceSlot.inventoryManager.PlaceInInventory(sourceSlot, temp);
            }
        }
    }
}
