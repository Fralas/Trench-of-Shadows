using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class RealEstateActivation : MonoBehaviour, IPointerClickHandler
{
    public Item item;
    public Image slotImg;
    public TextMeshProUGUI itemCount;
    public GameObject objectToActivate; // Assign this in the inspector

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
        UpdateSlotUI();
    }

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
            UpdateRecipeUI();
        }
        else
        {
            itemCount.text = string.Empty;
            slotImg.gameObject.SetActive(false);
            ClearRecipeUI();
        }
    }

    private void UpdateRecipeUI()
    {
        ClearRecipeUI();

        if (item.recipe != null && item.recipe.Count > 0)
        {
            for (int i = 0; i < item.recipe.Count; i++)
            {
                RecipeIngredient ingredient = item.recipe[i];
                
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


   public void OnPointerClick(PointerEventData eventData)
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            Debug.Log("Activated: " + objectToActivate.name);
        }
        else
        {
            Debug.LogWarning("No GameObject assigned to activate.");
        }
    }

 /*    public void OnPointerClick(PointerEventData eventData)
{
    if (objectToActivate != null)
    {
        objectToActivate.SetActive(true);
        Debug.Log("Activated: " + objectToActivate.name);

        // Segna la casa come comprata nel HouseStateManager
        HouseStateManager houseManager = objectToActivate.GetComponent<HouseStateManager>();
        if (houseManager != null)
        {
            houseManager.SetBought();
        }
    }
    else
    {
        Debug.LogWarning("No GameObject assigned to activate.");
    }
}*/


}