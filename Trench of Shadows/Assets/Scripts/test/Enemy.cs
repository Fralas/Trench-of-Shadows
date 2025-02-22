using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int MaxHealth = 100;
    public int CurrentHealth { get; private set; }

    private EnemyHealthBar _healthBar;

    void Start()
    {
        CurrentHealth = MaxHealth;
        _healthBar = GetComponentInChildren<EnemyHealthBar>(); // Trova la barra della vita
        _healthBar.UpdateHealth();
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        _healthBar.UpdateHealth();

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " è morto!");
        Destroy(gameObject); // Elimina il nemico
    }
}
