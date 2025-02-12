using UnityEngine;

[System.Serializable]
public class InventoryItemInstance
{
    public InventoryItemData itemData;
    public int quantity;

    public InventoryItemInstance(InventoryItemData itemData, int quantity)
    {
        this.itemData = itemData;
        this.quantity = quantity;

        Debug.Log($"Created new item instance: {itemData.itemName}, Quantity: {quantity}");
    }
}
