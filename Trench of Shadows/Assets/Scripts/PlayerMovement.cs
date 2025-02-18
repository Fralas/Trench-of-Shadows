using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;            // Reference to the Animator
    private Rigidbody2D rb;               // Reference to Rigidbody2D for movement
    public float moveSpeed = 5f;          // Movement speed
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer for flipping

    private float attackCooldown = 0.5f;  // Cooldown time between attacks (in seconds)
    private float lastAttackTime = 0f;    // Time of the last attack
    private bool isEditMode = false;

    // NEW: Reference to the InventoryManager
    private InventoryManager inventoryManager;

    void Start()
    {
        animator = GetComponent<Animator>();         // Get the Animator component
        rb = GetComponent<Rigidbody2D>();              // Get the Rigidbody2D component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component

        // Remove gravity by setting gravity scale to 0
        rb.gravityScale = 0f;
        
        // NEW: Get the InventoryManager instance from the scene.
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    void Update()
    {
        // --- Movement Code ---
        float horizontalInput = Input.GetAxis("Horizontal");  // Left/Right movement
        float verticalInput = Input.GetAxis("Vertical");        // Up/Down movement

        // Move the player using Rigidbody2D
        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized * moveSpeed;
        rb.velocity = movement;

        // Flip the sprite based on horizontal input
        if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }

        // Set walking animation based on horizontal or vertical movement
        animator.SetBool("isWalking", horizontalInput != 0 || verticalInput != 0);

        // --- Attack Code (Only allow if the player has a "Sword") ---
        // Check if enough time has passed for a new attack
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            // NEW: Verify the player has a "Sword" in the inventory before attacking
            if (inventoryManager != null && inventoryManager.HasItem("Sword"))
            {
                if (Input.GetKeyDown(KeyCode.Space))  // For horizontal attack (e.g., spacebar)
                {
                    animator.SetBool("isAttackingHorizontal", true);
                    animator.SetBool("isAttackingVertical", false);

                    // Flip the sprite during horizontal attack
                    if (spriteRenderer.flipX)
                    {
                        // If facing left, keep the flip during attack
                        animator.SetBool("isAttackingHorizontal", true);
                    }
                    else
                    {
                        // If facing right, flip the sprite for the attack
                        spriteRenderer.flipX = true;
                    }

                    // Update the last attack time
                    lastAttackTime = Time.time;
                }
                else if (Input.GetKeyDown(KeyCode.Z))  // For vertical attack (e.g., Z key)
                {
                    animator.SetBool("isAttackingVertical", true);
                    animator.SetBool("isAttackingHorizontal", false);

                    // Update the last attack time
                    lastAttackTime = Time.time;
                }
            }
        }

        // Reset attack parameters after attack ends
        if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("isAttackingHorizontal", false);
            // Reset sprite flip after horizontal attack ends
            if (spriteRenderer.flipX)
            {
                spriteRenderer.flipX = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            animator.SetBool("isAttackingVertical", false);
        }

        // --- Vertical Movement Animations ---
        if (verticalInput > 0) // Moving up
        {
            animator.SetBool("isWalkingUpwards", true);
            animator.SetBool("isWalkingDownwards", false);
        }
        else if (verticalInput < 0) // Moving down
        {
            animator.SetBool("isWalkingDownwards", true);
            animator.SetBool("isWalkingUpwards", false);
        }
        else // No vertical movement
        {
            animator.SetBool("isWalkingUpwards", false);
            animator.SetBool("isWalkingDownwards", false);
        }
    }

    public bool IsEditMode()
    {
        return isEditMode;
    }
}
