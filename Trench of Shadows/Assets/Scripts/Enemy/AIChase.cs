using System.Collections;
using UnityEngine;

public class AIChase : MonoBehaviour
{
    public float moveSpeed = 2f;         // Velocità del nemico
    public float detectionRadius = 5f;   // Raggio di visione
    public LayerMask playerLayer;        // Layer del player

    private Transform player;            // Riferimento al player
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

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
