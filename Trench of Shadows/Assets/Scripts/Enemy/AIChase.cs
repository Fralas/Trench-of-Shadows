using UnityEngine;

public class AIChase : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float stopDistance = 0.5f; // Distanza a cui il nemico si ferma prima di toccare il player
    [SerializeField] private LayerMask playerLayer;

    private Transform player;
    private Rigidbody2D rb;
    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        DetectPlayer();
    }

    private void FixedUpdate()
    {
        if (isChasing && player != null)
        {
            ChasePlayer();
        }
    }

    private void DetectPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (playerCollider != null)
        {
            player = playerCollider.transform;
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }
    }

    private void ChasePlayer()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Se il nemico è più vicino della stopDistance, si ferma
        if (distanceToPlayer > stopDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero; // Il nemico si ferma
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance); // Disegna anche la distanza di stop
    }
}
