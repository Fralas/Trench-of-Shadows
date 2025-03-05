using UnityEngine;
using UnityEngine.Rendering.Universal;  // Required for Light2D

public class TorchLightActivator : MonoBehaviour
{
    public GameObject torchLightObject; // Reference to the GameObject containing the light
    private Light2D torchLight; // The Light2D component (for 2D lights)

    public InventoryManager inventoryManager; // Reference to the InventoryManager

    private bool isTorchEquipped = false; // Track whether the torch is equipped

    private void Start()
    {
        // Ensure we have a reference to the light object and get the Light2D component from it
        if (torchLightObject != null)
        {
            torchLight = torchLightObject.GetComponent<Light2D>();
            if (torchLight == null)
            {
                Debug.LogWarning("No Light2D component found on the assigned GameObject.");
            }
            else
            {
                Debug.Log("Torch light component (Light2D) found and initialized.");
            }
        }
        else
        {
            Debug.LogWarning("Torch Light GameObject is not assigned.");
        }

        if (inventoryManager == null)
        {
            Debug.LogWarning("InventoryManager is not assigned.");
        }
    }

    private void Update()
    {
        CheckHeldItemForTorch();
        HandleTorchToggle();
    }

    // Check if the held item is "Torch" and activate/deactivate light accordingly
    private void CheckHeldItemForTorch()
    {
        Item heldItem = inventoryManager.GetHeldSlotItem(); // Get the item from the held slot

        if (heldItem != null && heldItem.itemID == "Torch") // Replace "Torch" with the actual item ID
        {
            if (!isTorchEquipped) 
            {
                Debug.Log("Torch is now equipped.");
            }
            isTorchEquipped = true; // Torch is equipped
        }
        else
        {
            if (isTorchEquipped) 
            {
                Debug.Log("Torch is no longer equipped.");
            }
            isTorchEquipped = false; // Torch is not equipped
        }
    }

    // Handle the light toggle with the "E" key
    private void HandleTorchToggle()
    {
        if (isTorchEquipped)
        {
            if (Input.GetKeyDown(KeyCode.E)) // Check if "E" is pressed
            {
                Debug.Log("E key was pressed."); // Debug log to check if "E" is detected
                if (torchLight != null)
                {
                    torchLight.enabled = !torchLight.enabled; // Toggle the light
                    string status = torchLight.enabled ? "activated" : "deactivated";
                    Debug.Log($"Torch light {status}."); // Print the light's status

                    // Additional debug to check if the GameObject is active
                    Debug.Log($"Torch Light GameObject is active: {torchLightObject.activeSelf}");
                }
                else
                {
                    Debug.LogWarning("Torch light is missing or not set.");
                }
            }
        }
    }
}
