using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDisplayManager : MonoBehaviour
{
    public static RecipeDisplayManager instance;

    [Header("Recipe UI References")]
    // Update these references to match your hierarchy
    public GameObject recipeSlot1; // This corresponds to Recipe_Slot1 in the hierarchy
    public GameObject recipeSlot2; // This corresponds to Recipe_Slot2
    public GameObject recipeSlot3; // This corresponds to Recipe_Slot3

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Clears previous recipe display.
    /// </summary>
    public void ClearRecipeDisplay()
    {
        ClearSlot(recipeSlot1);
        ClearSlot(recipeSlot2);
        ClearSlot(recipeSlot3);
    }

    /// <summary>
    /// Clears a single slot by resetting its RecipeSlot component.
    /// </summary>
    public void ClearSlot(GameObject slot)
    {
        if (slot == null)
        {
            Debug.LogWarning("Slot is null!");
            return;
        }

        RecipeSlot recipeSlot = slot.GetComponent<RecipeSlot>();

        if (recipeSlot != null)
        {
            recipeSlot.Clear();
        }
        else
        {
            Debug.LogWarning("RecipeSlot component missing on slot: " + slot.name);
        }
    }

    /// <summary>
    /// Populates the fixed three inventory slots with the recipe of the selected item.
    /// </summary>
    public void ShowRecipe(Item item)
    {
        ClearRecipeDisplay();

        if (item == null || item.recipe == null || item.recipe.Count == 0)
        {
            Debug.Log("This item has no recipe.");
            return;
        }

        // Ensure recipe is not empty or null
        if (item.recipe.Count > 0)
        {
            Ingredient ingredient = item.recipe[0];
            if (ingredient != null && ingredient.item != null)
            {
                Debug.Log("Displaying ingredient 1: " + ingredient.item.itemID);
                recipeSlot1.GetComponent<RecipeSlot>().Setup(ingredient);
            }
            else
            {
                Debug.LogWarning("Ingredient 1 is missing or its item is null.");
            }
        }

        if (item.recipe.Count > 1)
        {
            Ingredient ingredient = item.recipe[1];
            if (ingredient != null && ingredient.item != null)
            {
                Debug.Log("Displaying ingredient 2: " + ingredient.item.itemID);
                recipeSlot2.GetComponent<RecipeSlot>().Setup(ingredient);
            }
            else
            {
                Debug.LogWarning("Ingredient 2 is missing or its item is null.");
            }
        }

        if (item.recipe.Count > 2)
        {
            Ingredient ingredient = item.recipe[2];
            if (ingredient != null && ingredient.item != null)
            {
                Debug.Log("Displaying ingredient 3: " + ingredient.item.itemID);
                recipeSlot3.GetComponent<RecipeSlot>().Setup(ingredient);
            }
            else
            {
                Debug.LogWarning("Ingredient 3 is missing or its item is null.");
            }
        }
    }
}
