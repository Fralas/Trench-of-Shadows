using UnityEngine;

public class CraftingTableBehaviour : MonoBehaviour
{
    [Header("Crafting Table UI Settings")]
    // Reference to the Crafting Table UI background (e.g., CraftBackground)
    public GameObject craftBackground;
    // Reference to the Recipe UI background (e.g., RecipeBackground)
    public GameObject recipeBackground;
    
    // How close the player must be to interact
    public float interactionRange = 2f;
    
    // Delay between showing the "press key" message
    public float printDelay = 1f;
    private float timeSinceLastPrint = 0f;
    
    private Transform player;

    private void Start()
    {
        // Find the player using the "Player" tag
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // If craftBackground is not assigned in the Inspector, try to find it as a child named "CraftBackground"
        if (craftBackground == null)
        {
            Transform craftTransform = transform.Find("CraftBackground");
            if (craftTransform != null)
            {
                craftBackground = craftTransform.gameObject;
            }
            else
            {
                Debug.LogWarning("CraftBackground not assigned and not found as a child!");
            }
        }
        
        // If recipeBackground is not assigned in the Inspector, try to find it as a child named "RecipeBackground"
        if (recipeBackground == null)
        {
            Transform recipeTransform = transform.Find("RecipeBackground");
            if (recipeTransform != null)
            {
                recipeBackground = recipeTransform.gameObject;
            }
            else
            {
                Debug.LogWarning("RecipeBackground not assigned and not found as a child!");
            }
        }

        // Ensure both UI elements are deactivated at the start of the game
        if (craftBackground != null)
        {
            craftBackground.SetActive(false);
        }
        if (recipeBackground != null)
        {
            recipeBackground.SetActive(false);
        }
    }

    private void Update()
    {
        if (player == null) return;

        // Calculate the distance between the player and the CraftingTable
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < interactionRange)
        {
            timeSinceLastPrint += Time.deltaTime;
            if (timeSinceLastPrint >= printDelay)
            {
                Debug.Log("Press 'I' to open the Crafting Table.");
                timeSinceLastPrint = 0f;
            }

            // Open the crafting and recipe UIs when 'I' is pressed
            if (Input.GetKeyDown(KeyCode.I))
            {
                OpenCraftingUI();
            }
        }
        else
        {
            // If the player moves away, close both UIs if they're open
            if (craftBackground != null && craftBackground.activeSelf)
            {
                craftBackground.SetActive(false);
                Debug.Log("Crafting UI closed (player left interaction range).");
            }
            if (recipeBackground != null && recipeBackground.activeSelf)
            {
                recipeBackground.SetActive(false);
                Debug.Log("Recipe UI closed (player left interaction range).");
            }
        }
    }

    private void OpenCraftingUI()
    {
        // Activate the CraftBackground UI if it's not already active
        if (craftBackground != null && !craftBackground.activeSelf)
        {
            craftBackground.SetActive(true);
            Debug.Log("Crafting UI opened.");
        }
        
        // Activate the RecipeBackground UI if it's not already active
        if (recipeBackground != null && !recipeBackground.activeSelf)
        {
            recipeBackground.SetActive(true);
            Debug.Log("Recipe UI opened.");
        }
    }
}
