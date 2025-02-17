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
public class RecipeIngredient {
    public Item ingredientItem; // Reference to the ingredient item (e.g., Wheat)
    public int amountRequired;  // How many of this ingredient are needed (e.g., 3)
}

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items")]
public class Item : ScriptableObject
{
    public string itemID;
    public Sprite itemImg;
    public int itemAmt; // This is used to represent the current stack amount in inventory
    
    // Added item type field to be displayed in the Inspector
    public ItemType type;
    
    // Use a list of RecipeIngredient instead of a list of Items.
    public List<RecipeIngredient> recipe;
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
