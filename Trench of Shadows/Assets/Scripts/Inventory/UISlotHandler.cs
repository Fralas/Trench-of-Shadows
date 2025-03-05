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
    public Slider healthBar;
    public InventoryManager inventoryManager;
    
    [Header("Allowed Items (Leave empty for no restriction)")]
    public List<Item> allowedItems; // List of allowed items

    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector3 originalPosition;

    void Awake()
    {
        if (healthBar == null)
        {
            healthBar = GetComponentInChildren<Slider>();
        }

        if (item != null)
        {
            item = item.Clone();
        }

        UpdateSlotUI();

        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
    }

    public virtual void UpdateSlotUI()
    {
        if (item != null)
        {
            slotImg.sprite = item.itemImg;
            slotImg.gameObject.SetActive(true);
            itemCount.text = item.itemAmt.ToString();

            if (healthBar != null)
            {
                healthBar.gameObject.SetActive(true);
                float normalizedDurability = (float)item.durability / 100f;
                healthBar.value = normalizedDurability;

                Image fillImage = healthBar.fillRect.GetComponent<Image>();
                if (normalizedDurability > 0.66f)
                {
                    fillImage.color = Color.green;
                }
                else if (normalizedDurability > 0.33f)
                {
                    fillImage.color = Color.yellow;
                }
                else
                {
                    fillImage.color = Color.red;
                }
            }
        }
        else
        {
            slotImg.gameObject.SetActive(false);
            itemCount.text = string.Empty;
            
            if (healthBar != null)
            {
                healthBar.gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item == null) return;
            
            MouseManager.instance.PickupFromStack(this);
            item = null;
        }
        else
        {
            MouseManager.instance.UpdateHeldItem(this);
        }
        
        UpdateSlotUI();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        eventData.pointerDrag = null; // Prevent dragging
    }
    
    public void OnDrag(PointerEventData eventData) { }
    public void OnEndDrag(PointerEventData eventData) { }

    public virtual void OnDrop(PointerEventData eventData)
    {
        UISlotHandler sourceSlot = eventData.pointerDrag?.GetComponent<UISlotHandler>();
        
        // Check if item is allowed and the source slot is not the same as the target slot
        if (sourceSlot != null && sourceSlot != this && IsItemAllowed(sourceSlot.item))
        {
            // Swap items between slots if allowed
            Item temp = this.item;
            inventoryManager.PlaceInInventory(this, sourceSlot.item);
            sourceSlot.inventoryManager.PlaceInInventory(sourceSlot, temp);

            // Update UI
            UpdateSlotUI();
            sourceSlot.UpdateSlotUI();

            RefreshHealthBar();
            sourceSlot.RefreshHealthBar();

            // Only reset the held item if the item was successfully placed
            MouseManager.instance.heldItem = null;  // Only clear held item when drop is successful
        }
        else
        {
            Debug.Log("Item not allowed in this slot: " + (sourceSlot?.item?.name ?? "null"));
            // Do not clear the held item here, since the drop is invalid
        }
    }

    public bool IsItemAllowed(Item itemToCheck)
    {
        if (itemToCheck == null) return false;
        if (allowedItems == null || allowedItems.Count == 0) return true; // No restrictions

        foreach (Item allowedItem in allowedItems)
        {
            if (allowedItem.itemID == itemToCheck.itemID)
            {
                return true;
            }
        }

        // Log to debug why the item is not allowed
        Debug.Log("Item not allowed: " + itemToCheck.name);
        return false;
    }

    public void RefreshHealthBar()
    {
        if (healthBar != null)
        {
            if (item != null)
            {
                healthBar.gameObject.SetActive(true);
                float normalizedDurability = (float)item.durability / 100f;
                healthBar.value = normalizedDurability;
                
                Image fillImage = healthBar.fillRect.GetComponent<Image>();
                if (normalizedDurability > 0.66f)
                {
                    fillImage.color = Color.green;
                }
                else if (normalizedDurability > 0.33f)
                {
                    fillImage.color = Color.yellow;
                }
                else
                {
                    fillImage.color = Color.red;
                }
            }
            else
            {
                healthBar.gameObject.SetActive(false);
            }
        }
    }
}