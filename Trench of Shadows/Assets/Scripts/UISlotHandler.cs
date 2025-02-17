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
        }
        else
        {
            itemCount.text = string.Empty;
            slotImg.gameObject.SetActive(false);
        }
        
        // Add or get a CanvasGroup component for drag handling.
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item == null) { return; }

            MouseManager.instance.PickupFromStack(this);
            return;
        }

        MouseManager.instance.UpdateHeldItem(this);
    }

    // Drag-and-drop methods

    // Called when the drag starts.
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            // Remember original parent and position to return to if needed.
            originalParent = transform.parent;
            originalPosition = transform.position;
            // Bring the dragged slot to the top of the hierarchy.
            transform.SetParent(transform.root);
            canvasGroup.blocksRaycasts = false;
        }
    }

    // Called while dragging.
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            transform.position = eventData.position;
        }
    }

    // Called when the drag ends.
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        // If the item wasn’t dropped onto a valid slot, return it to its original position.
        if (transform.parent == transform.root)
        {
            transform.SetParent(originalParent);
            transform.position = originalPosition;
        }
    }

    // Called when another draggable item is dropped on this slot.
    public void OnDrop(PointerEventData eventData)
    {
        UISlotHandler sourceSlot = eventData.pointerDrag.GetComponent<UISlotHandler>();
        if (sourceSlot != null && sourceSlot != this)
        {
            // Swap items between the source slot and this slot.
            Item temp = this.item;
            inventoryManager.PlaceInInventory(this, sourceSlot.item);
            sourceSlot.inventoryManager.PlaceInInventory(sourceSlot, temp);
        }
    }
}
