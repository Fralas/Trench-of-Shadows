using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator; // Reference to the Animator
    private Rigidbody2D rb; // Reference to Rigidbody2D for movement
    public float moveSpeed = 5f; // Movement speed
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer for flipping

    private float attackCooldown = 0.5f; // Cooldown time between attacks (in seconds)
    private float lastAttackTime = 0f; // Time of the last attack

    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component

        // Remove gravity by setting gravity scale to 0
        rb.gravityScale = 0f;
    }

    void Update()
    {
        // Get horizontal and vertical input (e.g., arrow keys or WASD)
        float horizontalInput = Input.GetAxis("Horizontal");  // Left/Right movement
        float verticalInput = Input.GetAxis("Vertical");  // Up/Down movement

        // Move the player using Rigidbody2D
        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized * moveSpeed;
        rb.velocity = movement;

        // Flip the sprite based on horizontal input
        if (horizontalInput < 0) // Moving left
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0) // Moving right
        {
            spriteRenderer.flipX = false;
        }

        // Set walking animation based on horizontal or vertical movement
        if (horizontalInput != 0 || verticalInput != 0)
        {
            animator.SetBool("isWalking", true); // Set walking to true when moving
        }
        else
        {
            animator.SetBool("isWalking", false); // Set walking to false when not moving
        }

        // Check if enough time has passed for a new attack
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            // Handle attack input and flip sprite during horizontal attack
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

        // Handle vertical movement for up and down
        if (verticalInput > 0) // Moving up
        {
            animator.SetBool("isWalkingUpwards", true); // Trigger upward walk animation
            animator.SetBool("isWalkingDownwards", false); // Disable downward walk animation
        }
        else if (verticalInput < 0) // Moving down
        {
            animator.SetBool("isWalkingDownwards", true); // Trigger downward walk animation
            animator.SetBool("isWalkingUpwards", false); // Disable upward walk animation
        }
        else // No vertical movement
        {
            animator.SetBool("isWalkingUpwards", false); // Disable upward walk animation
            animator.SetBool("isWalkingDownwards", false); // Disable downward walk animation
        }
    }
}
