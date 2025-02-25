using UnityEngine;
using UnityEngine.EventSystems;

public class CraftSlotRightClick : MonoBehaviour, IPointerClickHandler
{
    public InventoryManager inventoryManager;  // Reference to InventoryManager
    public UISlotHandler slot;                 // Reference to the UISlotHandler for the specific slot

    // This method is called when a mouse button is clicked on the UI element
    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if the right mouse button (secondary button) was clicked
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Call the method in InventoryManager to show the recipe for the item in the slot
            inventoryManager.RightClickOnCraftSlot(slot);
        }
    }
}
