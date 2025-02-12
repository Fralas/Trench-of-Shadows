using UnityEngine;

public enum ItemType
{
    Food,
    Weapon,
    Armor
}

[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "Inventory/Item Data")]
public class InventoryItemData : ScriptableObject
{
    public ItemType type;
    public string itemName;
    
    [TextArea]
    public string description;
    
    // Assign a Sprite in the Inspector
    public Sprite icon;
    
    // The default quantity (used when initializing the inventory)
    public int defaultQuantity = 1;

    // Additional properties (customize as needed)
    public int damage;      // For weapons
    public int defense;     // For armor
    public int healing;     // For food items (e.g., healing potion)
}
