using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int _maxHp = 50;
    private int _hp;
    private EnemyKnockback _knockback;
    private EnemyManager _enemyManager; // Reference to the EnemyManager
    private WaveManager _waveManager; // Reference to the WaveManager

    public int MaxHp => _maxHp;
    public int Hp
    {
        get => _hp;
        private set
        {
            _hp = Mathf.Clamp(value, 0, _maxHp);
            Damaged?.Invoke(_hp);

            if (_hp <= 0) Die();
        }
    }

    public UnityEvent<int> Damaged;
    public UnityEvent Died;

    private void Awake()
    {
        _hp = _maxHp;
        _knockback = GetComponent<EnemyKnockback>();
        _enemyManager = FindObjectOfType<EnemyManager>(); // Find the EnemyManager
        _waveManager = FindObjectOfType<WaveManager>(); // Find the WaveManager
    }

    public void TakeDamage(int amount, Vector2 attackerPosition)
    {
        if (_hp <= 0) return;
        Hp -= amount;
        Debug.Log($"Nemico colpito! Vita rimanente: {Hp}");

        if (_knockback != null)
        {
            _knockback.ApplyKnockback(attackerPosition);
        }
    }

    private void Die()
    {
        // Ensure the enemy is removed from the list of alive enemies before destroying the object
        if (_enemyManager != null)
        {
            _enemyManager.RemoveEnemy(this.gameObject);
        }

        // Notify the WaveManager that an enemy has died
        if (_waveManager != null)
        {
            _waveManager.OnEnemyDied(); // Call OnEnemyDied in WaveManager
        }

        Died?.Invoke();
        Debug.Log($"{gameObject.name} è morto!");

        // Check if the AIRoam component exists and call DropRawMeat if it does.
        AIRoam aiRoam = GetComponent<AIRoam>();
        if (aiRoam != null)
        {
            aiRoam.DropRawMeat(); // Drop RawMeat first
        }

        // Now, destroy the enemy object after handling the drop.
        Destroy(gameObject);
    }
}
