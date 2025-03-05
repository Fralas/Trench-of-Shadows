using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    // Reference to the InventoryBackground GameObject
    public GameObject inventoryBackground;

    // Reference to the ArmorBackground GameObject
    public GameObject armorBackground;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Toggle the active state of the inventory background
            if (inventoryBackground != null)
            {
                inventoryBackground.SetActive(!inventoryBackground.activeSelf);
            }
            else
            {
                Debug.LogError("InventoryBackground is not assigned.");
            }

            // Toggle the active state of the armor background
            if (armorBackground != null)
            {
                armorBackground.SetActive(!armorBackground.activeSelf);
            }
            else
            {
                Debug.LogError("ArmorBackground is not assigned.");
            }
        }
    }
}
