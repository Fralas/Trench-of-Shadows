using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UISlotHandler : MonoBehaviour, IPointerClickHandler
{
    public Item item;
    public Image slotImg;
    public TextMeshProUGUI itemCount;
    public InventoryManager inventoryManager;

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
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if(item == null) { return; }

            MouseManager.instance.PickupFromStack(this);
            return;
        }

        MouseManager.instance.UpdateHeldItem(this);
    }
}
