using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHp = 50; // Vita massima del nemico
    private int currentHp;

    private void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"{gameObject.name} ha subito {damage} danni. Vita rimanente: {currentHp}");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} è stato sconfitto!");
        Destroy(gameObject);
    }
}
