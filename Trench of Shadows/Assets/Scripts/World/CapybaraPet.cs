using UnityEngine;

public class CapybaraPet : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float moveSpeed = 3f;
    public float stopDistance = 2f; // Distance within which the pet stops moving
    public float idleDistance = 1f; // Larger distance within which the pet goes to idle
    private Animator animator; // Reference to the Animator component

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Move towards the player
        MoveTowardsPlayer();

        // Update animations based on movement and distance
        UpdateAnimations();
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = player.position - transform.position;

        // If the pet is farther than the stop distance, it moves towards the player
        if (direction.magnitude > stopDistance)
        {
            // Move towards the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // Flip the pet based on direction
            FlipPet(direction);
        }
        else if (direction.magnitude > idleDistance)
        {
            // Within stop range but outside idle range: stop moving but don't go idle
            animator.SetBool("isIdle", false);
        }
        else
        {
            // If within the idle range, go into idle mode
            animator.SetBool("isIdle", true);
            animator.SetBool("isWalkingHorizontal", false);
            animator.SetBool("isWalkingUpwards", false);
            animator.SetBool("isWalkingDownwards", false);
        }
    }

    void UpdateAnimations()
    {
        Vector3 direction = player.position - transform.position;

        // If the pet is moving (outside the stop distance)
        if (direction.magnitude > stopDistance)
        {
            // Set isIdle to false since the pet is moving
            animator.SetBool("isIdle", false);

            // Prioritize horizontal movement: if movement on X-axis is greater than on Y-axis
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Set horizontal movement animation
                animator.SetBool("isWalkingHorizontal", true);
                animator.SetBool("isWalkingUpwards", false);
                animator.SetBool("isWalkingDownwards", false);
            }
            else
            {
                // Set vertical movement animation (up or down)
                if (direction.y > 0)
                {
                    animator.SetBool("isWalkingUpwards", true);
                    animator.SetBool("isWalkingHorizontal", false);
                    animator.SetBool("isWalkingDownwards", false);
                }
                else
                {
                    animator.SetBool("isWalkingDownwards", true);
                    animator.SetBool("isWalkingHorizontal", false);
                    animator.SetBool("isWalkingUpwards", false);
                }
            }
        }
        else
        {
            // If the pet is within the stop distance but outside the idle range, stop walking animation
            if (direction.magnitude > idleDistance)
            {
                animator.SetBool("isWalkingHorizontal", false);
                animator.SetBool("isWalkingUpwards", false);
                animator.SetBool("isWalkingDownwards", false);
            }
        }
    }

    void FlipPet(Vector3 direction)
    {
        // Flip the pet when moving right
        if (direction.x > 0 && transform.localScale.x < 0)
        {
            // Pet moves right, flip it (make scale positive)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        // Flip the pet when moving left
        else if (direction.x < 0 && transform.localScale.x > 0)
        {
            // Pet moves left, flip it (make scale negative)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
