using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    // Reference to the InventoryBackground GameObject
    public GameObject inventoryBackground;

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
        }
    }
}
