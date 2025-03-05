using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    private SpriteRenderer spriteRenderer;

    private float attackCooldown = 1f;
    private float lastAttackTime = 0f;
    private bool isEditMode = false;
    private InventoryManager playerInventory;
    private int enemyLayer; // ✅ Fixed missing semicolon

    [SerializeField] private int attackDamage = 10;  // Danno inflitto ai nemici
    [SerializeField] private float attackRange = 1f; // Distanza massima per colpire il nemico

    private bool isHarvestingOrWatering = false; // Flag to check if the player is harvesting or watering

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0f;
        playerInventory = GameObject.Find("Manager").GetComponent<InventoryManager>();
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    void Update()
    {
        // Check if player is harvesting or watering, and if true, disable movement
        if (isHarvestingOrWatering) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized * moveSpeed;
        rb.velocity = movement;

        if (horizontalInput < 0) spriteRenderer.flipX = true;
        else if (horizontalInput > 0) spriteRenderer.flipX = false;

        animator.SetBool("isWalking", horizontalInput != 0 || verticalInput != 0);

        if (Time.time - lastAttackTime >= attackCooldown && PlayerHasSword())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("isAttackingHorizontal", true);
                lastAttackTime = Time.time;
                Attack(Vector2.right * (spriteRenderer.flipX ? -1 : 1)); // Attack left or right
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                animator.SetBool("isAttackingVertical", true);
                lastAttackTime = Time.time;
                Attack(Vector2.up); // Attack upward
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) animator.SetBool("isAttackingHorizontal", false);
        if (Input.GetKeyUp(KeyCode.Z)) animator.SetBool("isAttackingVertical", false);
    }

    private void Attack(Vector2 attackDirection)
    {
        Vector2 attackStartPosition = (Vector2)transform.position + attackDirection * 0.5f; // ✅ Offset start position
        RaycastHit2D hit = Physics2D.Raycast(attackStartPosition, attackDirection, attackRange, enemyLayer);

        Debug.DrawRay(attackStartPosition, attackDirection * attackRange, Color.red, 0.5f); // Debug for raycast

        if (hit.collider != null)
        {
            Debug.Log("Colpito: " + hit.collider.name); // Debug log

            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage, transform.position); // Pass player's position
                Debug.Log("Danno inflitto: " + attackDamage);
            }
        }
    }

    private bool PlayerHasSword()
    {
        if (playerInventory != null)
        {
            Item heldItem = playerInventory.GetHeldSlotItem();
            return heldItem != null && heldItem.itemID == "Sword";
        }
        return false;
    }

    // Method to set the harvesting/watering state
    public void SetHarvestingOrWateringState(bool state)
    {
        isHarvestingOrWatering = state;
    }
}
