using System.Collections.Generic;
using UnityEngine;

public class RecipeDisplayManager : MonoBehaviour
{
    // The background panel that holds the recipe slots.
    public GameObject recipeBackground;

    // The recipe slots (e.g., RecipeSlot_1, RecipeSlot_2, RecipeSlot_3).
    public RecipeSlot[] recipeSlots;

    /// <summary>
    /// Displays the recipe for the given item.
    /// </summary>
    public void ShowRecipe(Item item)
    {
        if (item == null || item.recipe == null || item.recipe.Count == 0)
        {
            // Hide the recipe display if no recipe is available.
            recipeBackground.SetActive(false);
            return;
        }

        // Activate the recipe background.
        recipeBackground.SetActive(true);

        // Loop through the recipe slots.
        for (int i = 0; i < recipeSlots.Length; i++)
        {
            // If there is a recipe ingredient for this slot, display it.
            if (i < item.recipe.Count)
            {
                recipeSlots[i].SetRecipeIngredient(item.recipe[i]);
            }
            else
            {
                // Hide any extra slots that don't have a corresponding ingredient.
                recipeSlots[i].ClearSlot();
            }
        }
    }

    /// <summary>
    /// Hides the recipe display.
    /// </summary>
    public void HideRecipe()
    {
        recipeBackground.SetActive(false);
    }
}
