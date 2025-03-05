using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public static MouseManager instance;
    public GameObject Manager;

    public Item heldItem;
    public Item GetHeldItem { get { return heldItem; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);  // 🔥 Mantieni solo MouseManager persistente
    }

    private void Start()
    {
        if (Manager == null)
        {
            Manager = GameObject.Find("Manager"); // 🔍 Cerca Manager nella scena
            if (Manager == null)
            {
                Debug.LogError("⚠️ Manager non trovato nella scena!");
            }
        }
    }

    public void UpdateHeldItem(UISlotHandler activeSlot)
    {
        Item activeItem = activeSlot.item;

        // If the held item is the same as the item in the slot, try to stack it
        if (heldItem != null && activeItem != null && heldItem.itemID == activeItem.itemID)
        {
            Debug.Log($"Clicked on slot: {activeSlot.name}");
            activeSlot.inventoryManager.StackInInventory(activeSlot, heldItem);
            heldItem = null;  // Clear held item after stacking
            return;
        }

        // If the item exists in the active slot, swap items only if the item is allowed
        if (activeItem != null && activeSlot.IsItemAllowed(activeItem))
        {
            activeSlot.inventoryManager.ClearItemSlot(activeSlot);
            
            if (heldItem != null)
            {
                activeSlot.inventoryManager.PlaceInInventory(activeSlot, heldItem);
            }
            heldItem = activeItem;
        }
        else
        {
            // If there is a held item, place it into the slot only if it's valid
            if (heldItem != null && activeSlot.IsItemAllowed(heldItem))
            {
                activeSlot.inventoryManager.PlaceInInventory(activeSlot, heldItem);
                heldItem = null;  // Clear held item only after placing it successfully
            }
        }
    }

    public void PickupFromStack(UISlotHandler activeSlot)
    {
        if (heldItem != null && heldItem.itemID != activeSlot.item.itemID) { return; }

        if (heldItem == null)
        {
            heldItem = activeSlot.item.Clone();
            heldItem.itemAmt = default;
        }
        heldItem.itemAmt++;

        activeSlot.item.itemAmt--;
        activeSlot.itemCount.text = activeSlot.item.itemAmt.ToString();

        if (activeSlot.item.itemAmt <= 0)
        {
            activeSlot.inventoryManager.ClearItemSlot(activeSlot);
        }
    }
}
