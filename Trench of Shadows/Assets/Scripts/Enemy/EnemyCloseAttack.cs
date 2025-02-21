using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;  // Danno inflitto al player
    [SerializeField] private float attackInterval = 1f; // Tempo tra un attacco e l'altro

    private bool isAttacking = false;  // Controllo per evitare più coroutine attive

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerDatas player = collision.GetComponent<PlayerDatas>();
        if (player != null && !isAttacking)
        {
            StartCoroutine(AttackPlayer(player));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerDatas player = collision.GetComponent<PlayerDatas>();
        if (player != null)
        {
            isAttacking = false; // Ferma l'attacco se il player si allontana
        }
    }

    private IEnumerator AttackPlayer(PlayerDatas player)
    {
        isAttacking = true;

        while (isAttacking) // Continua finché il player è nel trigger
        {
            player.Damage(damageAmount);
            Debug.Log($"{gameObject.name} ha inflitto {damageAmount} danni a {player.gameObject.name}!");
            yield return new WaitForSeconds(attackInterval);
        }
    }
}
