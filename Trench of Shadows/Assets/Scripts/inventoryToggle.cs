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
            inventoryBackground.SetActive(!inventoryBackground.activeSelf);
        }
    }
}
