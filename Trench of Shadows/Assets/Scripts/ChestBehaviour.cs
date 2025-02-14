using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    public GameObject chestInventoryUI;  // Reference to chest's inventory UI
    private bool isPlayerNearby = false;  // Check if player is near chest
    private Transform player;
    private float timeSinceLastPrint = 0f;
    public float printDelay = 1f;  // Delay in seconds for print messages

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (isPlayerNearby)
        {
            timeSinceLastPrint += Time.deltaTime;

            // Show message when the player is near the chest
            if (timeSinceLastPrint >= printDelay)
            {
                Debug.Log("Press 'I' to open the chest.");
                timeSinceLastPrint = 0f;  // Reset the timer
            }

            // Check for input to open the chest
            if (Input.GetKeyDown(KeyCode.I))  // Use 'I' to interact with the chest
            {
                ToggleChestInventory();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Assuming the player has the "Player" tag
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    private void ToggleChestInventory()
    {
        // Toggle the chest inventory visibility
        chestInventoryUI.SetActive(!chestInventoryUI.activeSelf);
    }
}
