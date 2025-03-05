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
    
    [Header("Armor Slot Flag")]
    public bool isArmorSlot = false;  // Flag to indicate if this is an armor slot

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

            // Check if the slot is a reserved armor slot and there is an item in it
            if (isArmorSlot && item != null)
            {
                // Decrease health when the item is removed from a reserved armor slot
                int bonusHealth = item.bonusHealth;
                PlayerDatas.Instance.UpdateBonusHealth(-bonusHealth); // Decrease max HP
                Debug.Log("Armor item removed: " + item.name + " with bonus health: " + bonusHealth);
            }

            MouseManager.instance.PickupFromStack(this);
            item = null; // Clear the item from the slot
        }
        else
        {
            MouseManager.instance.UpdateHeldItem(this);
        }

        UpdateSlotUI(); // Update the UI to reflect the changes
    }




    public void OnBeginDrag(PointerEventData eventData)
    {
        eventData.pointerDrag = null; // Prevent dragging
    }
    
    public void OnDrag(PointerEventData eventData) { }
    public void OnEndDrag(PointerEventData eventData) { }

    public virtual void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop triggered");

        UISlotHandler sourceSlot = eventData.pointerDrag?.GetComponent<UISlotHandler>();

        if (sourceSlot != null)
        {
            Debug.Log("Source Slot found, Item: " + sourceSlot.item.name);

            if (sourceSlot != this)
            {
                // Check if the item is allowed
                if (IsItemAllowed(sourceSlot.item))
                {
                    Debug.Log("Item allowed: " + sourceSlot.item.name); // Log allowed item

                    // If the item is armor, increase max health
                    if (sourceSlot.item.type == ItemType.Armor)
                    {
                        int bonusHealth = sourceSlot.item.bonusHealth;
                        PlayerDatas.Instance.UpdateBonusHealth(bonusHealth); // Update max HP
                        Debug.Log("Armor item dropped: " + sourceSlot.item.name + " with bonus health: " + bonusHealth);
                    }

                    // Swap the items between source and this slot
                    Item temp = this.item;
                    inventoryManager.PlaceInInventory(this, sourceSlot.item);
                    sourceSlot.inventoryManager.PlaceInInventory(sourceSlot, temp);

                    // Update UI for both slots
                    UpdateSlotUI();
                    sourceSlot.UpdateSlotUI();

                    // Refresh health bars
                    RefreshHealthBar();
                    sourceSlot.RefreshHealthBar();

                    // Clear the held item (if the drop is successful)
                    MouseManager.instance.heldItem = null;  // Reset the held item

                    Debug.Log("Drop successful!");
                }
                else
                {
                    Debug.Log("Item not allowed: " + sourceSlot.item.name);  // Log when item is not allowed
                }
            }
            else
            {
                Debug.Log("Source slot is the same as the target slot!");  // Log when source and target are the same
            }
        }
        else
        {
            Debug.Log("No valid source slot found in the drop event.");  // Log if sourceSlot is null
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
                Debug.Log("Item allowed: " + itemToCheck.name);  // Add log here

                // If the item is armor, increase the player's max health
                if (itemToCheck.type == ItemType.Armor)
                {
                    int bonusHealth = itemToCheck.bonusHealth;
                    PlayerDatas.Instance.UpdateBonusHealth(bonusHealth); // Update max HP
                    Debug.Log("Armor item: " + itemToCheck.name + " with bonus health: " + bonusHealth);
                }

                return true;
            }
        }

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
