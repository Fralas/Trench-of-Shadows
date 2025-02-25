using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeSlot : MonoBehaviour
{
    // References to UI components in this slot.
    public Image icon;
    public TextMeshProUGUI amountText;

    // Fills this slot with the information from a RecipeIngredient.
    public void SetRecipeIngredient(RecipeIngredient ingredient)
    {
        if (ingredient != null && ingredient.ingredientItem != null)
        {
            icon.sprite = ingredient.ingredientItem.itemImg;
            amountText.text = ingredient.amountRequired.ToString();
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Optionally, a method to clear the slot.
    public void ClearSlot()
    {
        icon.sprite = null;
        amountText.text = string.Empty;
        gameObject.SetActive(false);
    }
}


