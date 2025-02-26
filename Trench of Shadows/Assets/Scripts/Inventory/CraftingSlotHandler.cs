using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CraftingSlotHandler : MonoBehaviour, IPointerClickHandler
{
    public Item item;
    public Image slotImg;
    public TextMeshProUGUI itemCount;
    public InventoryManager inventoryManager;

    // References to the recipe icons and their counts
    public Image icon1;
    public Image icon2;
    public Image icon3;

    public Image backgroundImage;
    public TextMeshProUGUI count1;
    public TextMeshProUGUI count2;
    public TextMeshProUGUI count3;

    void Awake()
    {
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager reference is not assigned in " + gameObject.name);
        }
        UpdateSlotUI();
    }

    // Called each time the GameObject is enabled (for example, when opening the crafting inventory)
    void OnEnable()
    {
        UpdateSlotUI();
    }

    private void UpdateSlotUI()
    {
        if (item != null)
        {
            slotImg.sprite = item.itemImg;
            itemCount.text = item.itemAmt.ToString();
            slotImg.gameObject.SetActive(true);

            // Update the Recipe UI
            UpdateRecipeUI();

            // Check if the inventory has the required ingredients
            if (item.recipe != null && item.recipe.Count > 0)
            {
                if (!inventoryManager.HasRequiredIngredientsFor(item))
                {
                    // Change the background to red if any ingredient is missing
                    if (backgroundImage != null)
                        backgroundImage.color = Color.red;
                    else
                        slotImg.color = Color.red; // Fallback: tint the slot image red
                }
                else
                {
                    // Reset to the default color (white) when all ingredients are present
                    if (backgroundImage != null)
                        backgroundImage.color = Color.white;
                    else
                        slotImg.color = Color.white;
                }
            }
        }
        else
        {
            itemCount.text = string.Empty;
            slotImg.gameObject.SetActive(false);

            // Clear any colored feedback
            if (backgroundImage != null)
                backgroundImage.color = Color.white;
            else
                slotImg.color = Color.white;

            ClearRecipeUI();
        }
    }

    private void UpdateRecipeUI()
    {
        // Clear the previous recipe UI
        ClearRecipeUI();

        if (item.recipe != null && item.recipe.Count > 0)
        {
            // Show the icons and counts for the recipe ingredients
            for (int i = 0; i < item.recipe.Count; i++)
            {
                RecipeIngredient ingredient = item.recipe[i];

                // Set the icon and count based on the ingredient
                if (i == 0)
                {
                    icon1.sprite = ingredient.ingredientItem.itemImg;
                    count1.text = ingredient.amountRequired.ToString();
                    icon1.gameObject.SetActive(true);
                    count1.gameObject.SetActive(true);
                }
                else if (i == 1)
                {
                    icon2.sprite = ingredient.ingredientItem.itemImg;
                    count2.text = ingredient.amountRequired.ToString();
                    icon2.gameObject.SetActive(true);
                    count2.gameObject.SetActive(true);
                }
                else if (i == 2)
                {
                    icon3.sprite = ingredient.ingredientItem.itemImg;
                    count3.text = ingredient.amountRequired.ToString();
                    icon3.gameObject.SetActive(true);
                    count3.gameObject.SetActive(true);
                }
            }
        }
    }

    private void ClearRecipeUI()
    {
        // Hide all icons and counts
        icon1.gameObject.SetActive(false);
        icon2.gameObject.SetActive(false);
        icon3.gameObject.SetActive(false);
        count1.gameObject.SetActive(false);
        count2.gameObject.SetActive(false);
        count3.gameObject.SetActive(false);
    }

    public void AssignItem(Item newItem)
    {
        item = newItem;
        UpdateSlotUI();
    }

    public void ClearSlot()
    {
        item = null;
        UpdateSlotUI();
    }

    // On click, try to craft the item.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null) return;

        bool crafted = inventoryManager.CraftItem(item);
        if (crafted)
        {
            Debug.Log("Crafted " + item.itemID + " successfully!");
        }
        else
        {
            Debug.Log("Crafting failed. Missing required ingredients.");
        }
    }
}
