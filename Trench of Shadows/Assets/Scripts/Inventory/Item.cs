using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Consumable,
    Equipment,
    Weapon,
    Armor,
    Material,
    Miscellaneous
}

[System.Serializable]
public class RecipeIngredient
{
    public Item ingredientItem; // Reference to the ingredient item (e.g., Wheat)
    public int amountRequired;  // How many of this ingredient are needed (e.g., 3)
}

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items")]
public class Item : ScriptableObject
{
    public string itemID;
    public Sprite itemImg;
    public int itemAmt;
    public ItemType type;
    public int durability; 
    public List<RecipeIngredient> recipe;
    
    [Header("Armor Specific")]
    public int bonusHealth;  // Bonus health provided by armor items

    // Method to equip/unequip an armor item
    public void Equip(PlayerDatas player)
    {
        if (type == ItemType.Armor)
        {
            // Increase max health when equipping armor
            player.UpdateBonusHealth(bonusHealth);
        }
    }

    public void Unequip(PlayerDatas player)
    {
        if (type == ItemType.Armor)
        {
            // Decrease max health when unequipping armor
            player.UpdateBonusHealth(-bonusHealth);
        }
    }
}

public static class ScriptableObjectExtension
{
    /// <summary>
    /// Creates and returns a clone of any given scriptable object.
    /// </summary>
    public static T Clone<T>(this T scriptableObject) where T : ScriptableObject
    {
        if (scriptableObject == null)
        {
            Debug.LogError($"ScriptableObject was null. Returning default {typeof(T)} object.");
            return (T)ScriptableObject.CreateInstance(typeof(T));
        }

        T instance = UnityEngine.Object.Instantiate(scriptableObject);
        instance.name = scriptableObject.name; // remove (Clone) from name
        return instance;
    }
}
