using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float changeDirectionTime = 2f;
    public Tilemap groundTilemap;
    public TileBase walkableTile;
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 movement;
    private float timer;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeDirection();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ChangeDirection();
        }

        bool isWalkingHorizontal = movement.x != 0;
        bool isWalkingUpwards = movement.y > 0;
        bool isWalkingDownwards = movement.y < 0;
        bool isIdle = movement == Vector2.zero;

        animator.SetBool("isWalkingHorizontal", isWalkingHorizontal);
        animator.SetBool("isWalkingUpwards", isWalkingUpwards);
        animator.SetBool("isWalkingDownwards", isWalkingDownwards);
        animator.SetBool("isIdle", isIdle);

        // Flip sprite when moving left
        if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    void FixedUpdate()
    {
        if (movement != Vector2.zero)
        {
            Vector2 targetVelocity = movement * moveSpeed;
            Vector3 targetPosition = (Vector3)rb.position + (Vector3)targetVelocity * Time.fixedDeltaTime;
            Vector3Int tilePos = groundTilemap.WorldToCell(targetPosition);

            if (groundTilemap.GetTile(tilePos) == walkableTile)
            {
                rb.velocity = targetVelocity;
            }
            else
            {
                rb.velocity = Vector2.zero;
                ChangeDirection(); // Pick a new direction if blocked
            }
        }
    }


    void ChangeDirection()
    {
        for (int i = 0; i < 10; i++) // Try 10 times to find a valid direction
        {
            Vector2 newDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            Vector3 targetPosition = (Vector3)rb.position + (Vector3)newDirection;
            Vector3Int tilePos = groundTilemap.WorldToCell(targetPosition);

            if (groundTilemap.GetTile(tilePos) == walkableTile)
            {
                movement = newDirection;
                timer = changeDirectionTime;
                return;
            }
        }
        movement = Vector2.zero;
        timer = changeDirectionTime;
    }

}