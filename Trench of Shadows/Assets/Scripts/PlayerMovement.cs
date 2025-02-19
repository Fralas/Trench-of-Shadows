using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;            
    private Rigidbody2D rb;               
    public float moveSpeed = 5f;          
    private SpriteRenderer spriteRenderer; 

    private float attackCooldown = 1f;  
    private float lastAttackTime = 0f;    
    private bool isEditMode = false;
    private InventoryManager playerInventory;
    // Ensure you have a playerInventory reference defined if needed:
    void Start()
    {
        animator = GetComponent<Animator>();        
        rb = GetComponent<Rigidbody2D>();              
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        rb.gravityScale = 0f;
        // Initialize playerInventory if it's attached to the same GameObject
        playerInventory = GameObject.Find("Manager").GetComponent<InventoryManager>();
    }

    void Update()
    {
        // --- Movement Code ---
        float horizontalInput = Input.GetAxis("Horizontal");  
        float verticalInput = Input.GetAxis("Vertical");        

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

        // Set walking animation based on movement
        animator.SetBool("isWalking", horizontalInput != 0 || verticalInput != 0);

        // --- Attack Code ---
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            if (Input.GetKeyDown(KeyCode.Space) && PlayerHasSword())  // Corrected: added parentheses to call method
            {
                animator.SetBool("isAttackingHorizontal", true);
                animator.SetBool("isAttackingVertical", false);

                // Flip the sprite during horizontal attack
                if (spriteRenderer.flipX)
                {
                    animator.SetBool("isAttackingHorizontal", true);
                }
                else
                {
                    spriteRenderer.flipX = true;
                }

                lastAttackTime = Time.time;
            }
            else if (Input.GetKeyDown(KeyCode.Z) && PlayerHasSword())
            {
                animator.SetBool("isAttackingVertical", true);
                animator.SetBool("isAttackingHorizontal", false);

                lastAttackTime = Time.time;
            }
        }

        // Reset attack parameters after attack ends
        if (Input.GetKeyUp(KeyCode.Space) && PlayerHasSword())
        {
            animator.SetBool("isAttackingHorizontal", false);
            if (spriteRenderer.flipX)
            {
                spriteRenderer.flipX = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Z) && PlayerHasSword())
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
    } // <-- Properly closing Update() here

    public bool IsEditMode()
    {
        return isEditMode;
    }

    private bool PlayerHasSword()
    {
        if (playerInventory != null)
        {
            Item heldItem = playerInventory.GetHeldSlotItem();
            bool hasSword = heldItem != null && heldItem.itemID == "Sword";
            Debug.Log("Held slot item: " + (heldItem != null ? heldItem.itemID : "None") + " | Has Sword: " + hasSword);
            return hasSword;
        }
        return false;
    }
}
