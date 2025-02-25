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
    public Slider healthBar;  // Reference to HealthBar Slider
    public InventoryManager inventoryManager;

    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector3 originalPosition;

    void Awake()
    {
        if (healthBar == null) 
        {
            // Try to find the HealthBar GameObject automatically
            healthBar = GetComponentInChildren<Slider>();
        }

        if (item != null)
        {
            item = item.Clone();
        }

        UpdateSlotUI();

        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
    }

    public void UpdateSlotUI()
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

                // Update the color of the slider based on durability percentage.
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
        
        // Refresh the slot UI (including the health bar) on every click
        UpdateSlotUI();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        // Prevent dragging completely
        eventData.pointerDrag = null;
    }

    public void OnDrag(PointerEventData eventData) { }
    public void OnEndDrag(PointerEventData eventData) { }

    public void OnDrop(PointerEventData eventData)
    {
        UISlotHandler sourceSlot = eventData.pointerDrag.GetComponent<UISlotHandler>();
        if (sourceSlot != null && sourceSlot != this)
        {
            // Swap items between slots
            Item temp = this.item;
            inventoryManager.PlaceInInventory(this, sourceSlot.item);
            sourceSlot.inventoryManager.PlaceInInventory(sourceSlot, temp);

            // Update both slots UI
            UpdateSlotUI();
            sourceSlot.UpdateSlotUI();

            // Refresh health bar for both slots
            RefreshHealthBar();
            sourceSlot.RefreshHealthBar();
        }
    }


    // Ensure the health bar is updated correctly
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
