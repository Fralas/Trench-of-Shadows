using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int _maxHp = 50;
    private int _hp;

    public int MaxHp => _maxHp;
    public int Hp
    {
        get => _hp;
        private set
        {
            var isDamage = value < _hp;
            _hp = Mathf.Clamp(value, 0, _maxHp);

            if (isDamage) Damaged?.Invoke(_hp);
            else Healed?.Invoke(_hp);

            if (_hp <= 0) Die();
        }
    }

    public UnityEvent<int> Healed;
    public UnityEvent<int> Damaged;
    public UnityEvent Died;

    private void Awake()
    {
        _hp = _maxHp;
    }

    public void Damage(int amount) => Hp -= amount;
    public void Heal(int amount) => Hp += amount;
    public void TakeDamage(int amount)
{
    Hp -= amount;
    Debug.Log($"Nemico colpito! Vita rimanente: {Hp}");

    if (Hp <= 0)
    {
        Die();
    }
}


    private void Die()
    {
        Died?.Invoke();
        Destroy(gameObject);
    }
}
