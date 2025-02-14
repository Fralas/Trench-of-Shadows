using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public static MouseManager instance;

    public Item heldItem;
    public Item GetHeldItem {  get { return heldItem; } }

    private void Awake()
    {
        instance = this;
    }

    public void UpdateHeldItem(UISlotHandler activeSlot)
    {
        // Get the item currently in the slot.
        Item activeItem = activeSlot.item;
        
        // If both the held item and the slot item exist and are the same, stack them.
        if (heldItem != null && activeItem != null && heldItem.itemID == activeItem.itemID)
        {
            activeSlot.inventoryManager.StackInInventory(activeSlot, heldItem);
            heldItem = null;
            return;
        }
        
        // Case 1: Slot has an item (swapping scenario)
        if (activeItem != null)
        {
            // Clear the slot and place the held item (if any) into it.
            activeSlot.inventoryManager.ClearItemSlot(activeSlot);
            if (heldItem != null)
            {
                activeSlot.inventoryManager.PlaceInInventory(activeSlot, heldItem);
            }
            // Now swap: pick up the item that was in the slot.
            heldItem = activeItem;
        }
        else // Case 2: Slot is empty (placing scenario)
        {
            if (heldItem != null)
            {
                activeSlot.inventoryManager.PlaceInInventory(activeSlot, heldItem);
                heldItem = null; // Clear held item after placing.
            }
        }
    }


    public void PickupFromStack(UISlotHandler activeSlot)
    {
        if(heldItem != null && heldItem.itemID != activeSlot.item.itemID) { return; }

        if(heldItem == null)
        {
            heldItem = activeSlot.item.Clone();
            heldItem.itemAmt = default;
        }
        heldItem.itemAmt++;

        activeSlot.item.itemAmt--;
        activeSlot.itemCount.text = activeSlot.item.itemAmt.ToString();

        if(activeSlot.item.itemAmt <= 0)
        {
            activeSlot.inventoryManager.ClearItemSlot(activeSlot);
        }
    }
}
