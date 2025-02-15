using UnityEngine;
using System.Collections;

public class Enemy2D : MonoBehaviour
{
    [SerializeField]
    private int damageAmount = 10;
    [SerializeField]
    private float damageCooldown = 1f; // Tempo tra un attacco e l'altro

    private bool canDamage = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerDatas player = collision.gameObject.GetComponent<PlayerDatas>();
        if (player != null && canDamage)
        {
            player.Damage(damageAmount);
            StartCoroutine(DamageCooldown());
        }
    }

    private IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}
