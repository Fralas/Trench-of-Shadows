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

    [SerializeField] private int attackDamage = 10;  // Danno inflitto ai nemici
    [SerializeField] private float attackRange = 1f; // Distanza massima per colpire il nemico

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0f;
        playerInventory = GameObject.Find("Manager").GetComponent<InventoryManager>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized * moveSpeed;
        rb.velocity = movement;

        if (horizontalInput < 0) spriteRenderer.flipX = true;
        else if (horizontalInput > 0) spriteRenderer.flipX = false;

        animator.SetBool("isWalking", horizontalInput != 0 || verticalInput != 0);

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            if (Input.GetKeyDown(KeyCode.Space) && PlayerHasSword())
            {
                animator.SetBool("isAttackingHorizontal", true);
                lastAttackTime = Time.time;
                Attack();
            }
            else if (Input.GetKeyDown(KeyCode.Z) && PlayerHasSword())
            {
                animator.SetBool("isAttackingVertical", true);
                lastAttackTime = Time.time;
                Attack();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) animator.SetBool("isAttackingHorizontal", false);
        if (Input.GetKeyUp(KeyCode.Z)) animator.SetBool("isAttackingVertical", false);
    }

    private void Attack()
    {
        Vector2 attackDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, attackRange);

        Debug.DrawRay(transform.position, attackDirection * attackRange, Color.red, 0.5f); // Debug per controllare il raycast

        if (hit.collider != null)
        {
            Debug.Log("Colpito: " + hit.collider.name); // Controllo in console

            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage, transform.position); // Passa la posizione del player
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
}
