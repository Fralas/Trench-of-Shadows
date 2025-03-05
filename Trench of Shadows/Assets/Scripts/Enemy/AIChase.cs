using UnityEngine;

public class AIChase : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float stopDistance = 0.5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float roamRadius = 3f;
    [SerializeField] private float minRoamTime = 2f;
    [SerializeField] private float maxRoamTime = 5f;
    [SerializeField] private float stopDuration = 1f;  // Duration of stop between roaming

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isChasing = false;
    private Vector2 spawnPoint;
    private Vector2 roamTarget;
    private float roamTimer;
    private float stopTimer = 0f;  // Timer for stop duration
    private bool isStopped = false;  // Flag to check if AI should be stopped

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        FindNearestSpawnPoint();
        PickNewRoamTarget();
        roamTimer = Random.Range(minRoamTime, maxRoamTime);  // Set initial roam timer with random time
    }

    void Update()
    {
        DetectPlayer();

        if (!isChasing)
        {
            Roam();
        }
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

        if (distanceToPlayer > stopDistance)
        {
            Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
            rb.velocity = direction * moveSpeed;

            animator.SetBool("isWalking", true);
            animator.SetBool("isIdle", false);  // Ensure idle is not triggered while walking

            spriteRenderer.flipX = direction.x < 0;
        }
        else
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", true);  // Trigger idle animation when stopping
        }
    }

    private void Roam()
    {
        // If the AI is stopped, wait for the stop duration to finish
        if (isStopped)
        {
            stopTimer += Time.deltaTime;

            if (stopTimer >= stopDuration)
            {
                // After stop duration, resume roaming
                stopTimer = 0f;
                isStopped = false;
                PickNewRoamTarget();
                roamTimer = Random.Range(minRoamTime, maxRoamTime);  // Set a new random roam time
            }
            else
            {
                animator.SetBool("isWalking", false);  // Trigger idle animation while stopped
                animator.SetBool("isIdle", true);  // Ensure idle is active during stop
                return; // Do nothing while stopped
            }
        }

        // If AI is not stopped, handle roaming behavior
        if (roamTimer <= 0)
        {
            // Stop and wait for stop duration before resuming roam
            isStopped = true;
            roamTimer = Random.Range(minRoamTime, maxRoamTime);  // Set a new random roam time
        }
        else
        {
            roamTimer -= Time.deltaTime;  // Decrease roam timer

            Vector2 direction = (roamTarget - (Vector2)transform.position).normalized;
            rb.velocity = direction * moveSpeed;

            animator.SetBool("isWalking", true);
            animator.SetBool("isIdle", false);  // Disable idle animation when walking

            spriteRenderer.flipX = direction.x < 0;

            // If close to roam target, stop moving and pick a new roam target
            if (Vector2.Distance(transform.position, roamTarget) < 0.2f)
            {
                rb.velocity = Vector2.zero;
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", true);  // Trigger idle animation when stopping
            }
        }
    }

    private void PickNewRoamTarget()
    {
        roamTarget = spawnPoint + Random.insideUnitCircle * roamRadius;
    }

    private void FindNearestSpawnPoint()
    {
        EnemySpawnerLimited[] spawners = FindObjectsOfType<EnemySpawnerLimited>();
        float closestDistance = Mathf.Infinity;
        EnemySpawnerLimited closestSpawner = null;

        foreach (var spawner in spawners)
        {
            float distance = Vector2.Distance(transform.position, spawner.spawnPoint.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestSpawner = spawner;
            }
        }

        if (closestSpawner != null)
        {
            spawnPoint = closestSpawner.spawnPoint.position;
        }
        else
        {
            Debug.LogWarning("No EnemySpawner found! Defaulting to current position.");
            spawnPoint = transform.position;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(spawnPoint, roamRadius);
    }
}
