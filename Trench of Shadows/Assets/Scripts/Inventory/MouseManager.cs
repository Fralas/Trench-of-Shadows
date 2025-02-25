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

        if (heldItem != null && activeItem != null && heldItem.itemID == activeItem.itemID)
        {
            Debug.Log($"Clicked on slot: {activeSlot.name}");
            activeSlot.inventoryManager.StackInInventory(activeSlot, heldItem);
            heldItem = null;
            return;
        }

        if (activeItem != null)
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
            if (heldItem != null)
            {
                activeSlot.inventoryManager.PlaceInInventory(activeSlot, heldItem);
                heldItem = null;
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
