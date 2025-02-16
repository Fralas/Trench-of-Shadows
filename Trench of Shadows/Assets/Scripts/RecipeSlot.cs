using UnityEngine;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour
{
    public Image itemIcon;
    public Text itemName;

    /// <summary>
    /// Sets up the recipe slot with the given ingredient.
    /// </summary>
    public void Setup(Ingredient ingredient)
    {
        if (ingredient == null || ingredient.item == null)
        {
            Debug.LogWarning("Ingredient or Item is null!");
            return;
        }

        if (itemIcon != null)
        {
            itemIcon.sprite = ingredient.item.itemImg;  // Set the item icon
            itemIcon.enabled = true;
        }

        if (itemName != null)
        {
            itemName.text = ingredient.item.itemID;  // Set the item name
        }
    }


    /// <summary>
    /// Clears the slot by removing the displayed ingredient.
    /// </summary>
    public void Clear()
    {
        if (itemIcon != null)
        {
            itemIcon.sprite = null;
            itemIcon.enabled = false;
        }
        
        if (itemName != null)
        {
            itemName.text = "";
        }
    }
}
