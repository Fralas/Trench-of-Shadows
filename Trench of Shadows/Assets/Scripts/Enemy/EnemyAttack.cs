using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;  
    [SerializeField] private float attackInterval = 1f; 

    private bool isAttacking = false;
    private Coroutine attackCoroutine;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerDatas player = collision.GetComponent<PlayerDatas>();
        if (player != null && !isAttacking)
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);
            attackCoroutine = StartCoroutine(AttackPlayer(player));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerDatas>() != null)
        {
            isAttacking = false;
            animator.SetBool("isAttacking", false);
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    private IEnumerator AttackPlayer(PlayerDatas player)
    {
        while (isAttacking) 
        {
            player.Damage(damageAmount);
            Debug.Log($"{gameObject.name} ha inflitto {damageAmount} danni a {player.gameObject.name}!");
            yield return new WaitForSeconds(attackInterval);
        }
    }
}
