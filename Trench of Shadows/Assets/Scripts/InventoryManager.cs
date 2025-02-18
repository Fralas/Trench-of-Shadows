using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryGrid;
    public bool messyInventory;
    public RecipeSlotHandler RecipeSlot_1;
    public RecipeSlotHandler RecipeSlot_2;
    public RecipeSlotHandler RecipeSlot_3;


    private static InventoryManager instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Debug.Log("InventoryManager initialized.");
        ConfigureInventory();
    }

    private void Update()
    {
        if (inventoryGrid == null)
        {
            Debug.LogWarning("Inventory grid not found, trying to reassign...");
            inventoryGrid = GameObject.Find("ContentContainer"); // Ensure this name is correct
        }
    }

    public void PlaceInInventory(UISlotHandler activeSlot, Item item)
    {
        activeSlot.item = item;
        activeSlot.slotImg.sprite = item.itemImg;
        activeSlot.itemCount.text = item.itemAmt.ToString();
        activeSlot.slotImg.gameObject.SetActive(true);
        ConfigureInventory();
    }

    public void StackInInventory(UISlotHandler activeSlot, Item item)
    {
        if (activeSlot.item.itemID != item.itemID) { return; }

        activeSlot.item.itemAmt += item.itemAmt;
        activeSlot.itemCount.text = activeSlot.item.itemAmt.ToString();
        ConfigureInventory();
    }

    public void ClearItemSlot(UISlotHandler activeSlot)
    {
        activeSlot.slotImg.sprite = null;
        activeSlot.slotImg.gameObject.SetActive(false);
        activeSlot.itemCount.text = string.Empty;
        activeSlot.item = null;
    }

    public void ConfigureInventory()
    {
        if (messyInventory) { return; }

        // Loop through each child of inventory grid and rearrange by populated items
        List<Transform> uiSlots = new List<Transform>();
        for (int i = 0; i < inventoryGrid.transform.childCount; i++)
        {
            uiSlots.Add(inventoryGrid.transform.GetChild(i));
        }

        uiSlots.Sort((a, b) =>
        {
            UISlotHandler itemA = a.GetComponent<UISlotHandler>();
            UISlotHandler itemB = b.GetComponent<UISlotHandler>();

            bool hasItemA = itemA.item != null;
            bool hasItemB = itemB.item != null;

            return hasItemB.CompareTo(hasItemA);
        });

        for (int i = 0; i < uiSlots.Count; i++)
        {
            uiSlots[i].SetSiblingIndex(i);
        }
    }

    // New method: attempt to craft an item.
    public bool CraftItem(Item craftedItem)
    {
        // Check that a recipe exists.
        if (craftedItem.recipe == null || craftedItem.recipe.Count == 0)
        {
            Debug.Log("No recipe defined for this item.");
            return false;
        }

        // Verify that all required ingredients are present in sufficient quantities.
        foreach (RecipeIngredient ingredient in craftedItem.recipe)
        {
            int requiredAmount = ingredient.amountRequired;
            int availableAmount = 0;
            foreach (Transform child in inventoryGrid.transform)
            {
                UISlotHandler slot = child.GetComponent<UISlotHandler>();
                if (slot.item != null && slot.item.itemID == ingredient.ingredientItem.itemID)
                {
                    availableAmount += slot.item.itemAmt;
                }
            }
            if (availableAmount < requiredAmount)
            {
                Debug.Log("Not enough ingredient: " + ingredient.ingredientItem.itemID);
                return false;
            }
        }

        // Remove the required ingredients from inventory.
        foreach (RecipeIngredient ingredient in craftedItem.recipe)
        {
            int requiredAmount = ingredient.amountRequired;
            foreach (Transform child in inventoryGrid.transform)
            {
                UISlotHandler slot = child.GetComponent<UISlotHandler>();
                if (slot.item != null && slot.item.itemID == ingredient.ingredientItem.itemID)
                {
                    if (slot.item.itemAmt > requiredAmount)
                    {
                        slot.item.itemAmt -= requiredAmount;
                        slot.itemCount.text = slot.item.itemAmt.ToString();
                        requiredAmount = 0;
                        break;
                    }
                    else
                    {
                        requiredAmount -= slot.item.itemAmt;
                        ClearItemSlot(slot);
                    }
                }
            }
        }

        // Add the crafted item to inventory.
        bool added = AddItemToInventory(craftedItem);
        if (!added)
        {
            Debug.Log("No available slot to add the crafted item.");
            return false;
        }

        ConfigureInventory();
        return true;
    }
    
    // New method: Add the crafted item by stacking or placing in an empty slot.
    public bool AddItemToInventory(Item newItem)
    {
        // Try stacking on an existing slot with the same item.
        foreach (Transform child in inventoryGrid.transform)
        {
            UISlotHandler slot = child.GetComponent<UISlotHandler>();
            if (slot.item != null && slot.item.itemID == newItem.itemID)
            {
                StackInInventory(slot, newItem);
                return true;
            }
        }
        // Find an empty slot.
        foreach (Transform child in inventoryGrid.transform)
        {
            UISlotHandler slot = child.GetComponent<UISlotHandler>();
            if (slot.item == null)
            {
                PlaceInInventory(slot, newItem);
                return true;
            }
        }
        return false;
    }

    // Existing method to check for an item in inventory.
    public bool HasItem(string itemID)
    {
        foreach (Transform child in inventoryGrid.transform)
        {
            UISlotHandler slot = child.GetComponent<UISlotHandler>();
            if (slot.item != null && slot.item.itemID == itemID)
            {
                return true;
            }
        }
        return false;
    }

    public void RightClickOnCraftSlot(UISlotHandler slot)
    {
        if (slot.item == null || slot.item.recipe == null || slot.item.recipe.Count == 0)
        {
            Debug.Log("No recipe available for this item.");
            return;
        }

        // Loop through the recipe and update the RecipeSlot UI elements
        for (int i = 0; i < slot.item.recipe.Count; i++)
        {
            RecipeIngredient ingredient = slot.item.recipe[i];

            // Update the correct RecipeSlot (1, 2, or 3)
            switch (i)
            {
                case 0:
                    RecipeSlot_1.icon.sprite = ingredient.ingredientItem.itemImg;
                    RecipeSlot_1.itemCount.text = ingredient.amountRequired.ToString();
                    RecipeSlot_1.gameObject.SetActive(true);
                    break;
                case 1:
                    RecipeSlot_2.icon.sprite = ingredient.ingredientItem.itemImg;
                    RecipeSlot_2.itemCount.text = ingredient.amountRequired.ToString();
                    RecipeSlot_2.gameObject.SetActive(true);
                    break;
                case 2:
                    RecipeSlot_3.icon.sprite = ingredient.ingredientItem.itemImg;
                    RecipeSlot_3.itemCount.text = ingredient.amountRequired.ToString();
                    RecipeSlot_3.gameObject.SetActive(true);
                    break;
                default:
                    Debug.LogWarning("More than 3 ingredients, please review the recipe setup.");
                    break;
            }
        }
    }

}
