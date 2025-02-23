using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int _maxHp = 50;
    private int _hp;
    private EnemyKnockback _knockback;

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
