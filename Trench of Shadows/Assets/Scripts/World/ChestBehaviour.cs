using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    [Header("Chest UI Settings")]
    public GameObject chestInventoryUI;  // Reference to chest's inventory UI
    private Transform player;
    private float timeSinceLastPrint = 0f;
    public float printDelay = 1f;  // Delay for showing messages
    public float interactionRange = 2f; // Distance required to interact

    private GameObject playerInventoryBackground; // Reference to the player inventory background
    private GameObject chestInventoryBackground; // Reference to the chest inventory background

    [Header("Interaction Prompt")]
    public GameObject interactionPrompt; // UI or object that appears when near the chest

    [Header("Chest Sound Settings")]
    public AudioClip chestOpenSound;  // Sound to play when the chest is opened
    private AudioSource audioSource; // Audio source to play the sound

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Reference to the player inventory background
        playerInventoryBackground = GameObject.Find("InventoryBackground");

        // Reference to the chest inventory background
        chestInventoryBackground = GameObject.Find("ChestInventoryBackground");

        // Add or get the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();

        // Ensure the interaction prompt starts disabled
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    private void Update()
    {
        if (player == null) return;

        // Calculate distance to the player
        float distance = Vector2.Distance(transform.position, player.position);

        // Check if the player is within interaction range
        if (distance < interactionRange)
        {
            // Show interaction prompt
            if (interactionPrompt != null && !interactionPrompt.activeSelf)
            {
                interactionPrompt.SetActive(true);
            }

            timeSinceLastPrint += Time.deltaTime;

            // Show the interaction message with a delay
            if (timeSinceLastPrint >= printDelay)
            {
                Debug.Log("Press 'I' to open the chest.");
                timeSinceLastPrint = 0f;  // Reset the timer
            }

            // Trigger the chest inventory toggle when 'I' is pressed
            if (Input.GetKeyDown(KeyCode.I))
            {
                OpenChestInventory();
            }
        }
        else
        {
            // Hide interaction prompt if player is out of range
            if (interactionPrompt != null && interactionPrompt.activeSelf)
            {
                interactionPrompt.SetActive(false);
            }

            // Ensure the chest inventory is closed
            if (chestInventoryUI != null && chestInventoryUI.activeSelf)
            {
                chestInventoryUI.SetActive(false);
            }
        }
    }

    private void OpenChestInventory()
    {
        // Only open the chest inventory if it's not already active
        if (chestInventoryUI != null && !chestInventoryUI.activeSelf)
        {
            chestInventoryUI.SetActive(true);
            Debug.Log("Chest inventory opened.");

            // Play the chest opening sound if it's set
            if (chestOpenSound != null)
            {
                audioSource.clip = chestOpenSound;
                audioSource.Play();
            }

            // Ensure both player and chest inventories are active
            if (playerInventoryBackground != null && !playerInventoryBackground.activeSelf)
            {
                playerInventoryBackground.SetActive(true);
                Debug.Log("Player inventory activated.");
            }
            if (chestInventoryBackground != null && !chestInventoryBackground.activeSelf)
            {
                chestInventoryBackground.SetActive(true);
                Debug.Log("Chest inventory activated.");
            }
        }
    }
}
