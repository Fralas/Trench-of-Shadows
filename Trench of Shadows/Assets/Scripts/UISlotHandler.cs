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
            healthBar = transform.Find("HealthBar")?.GetComponent<Slider>();
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
                healthBar.gameObject.SetActive(true);  // Show HealthBar
                healthBar.value = (float)item.durability / 100f; // Normalize durability
            }
        }
        else
        {
            slotImg.gameObject.SetActive(false);
            itemCount.text = string.Empty;

            if (healthBar != null)
            {
                healthBar.gameObject.SetActive(false);  // Hide HealthBar
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
            UpdateSlotUI();
        }
        else
        {
            MouseManager.instance.UpdateHeldItem(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;

        originalParent = transform.parent;
        originalPosition = transform.position;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
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
                healthBar.value = (float)item.durability / 100f;
            }
            else
            {
                healthBar.gameObject.SetActive(false);
            }
        }
    }

}
