using UnityEngine;
using UnityEngine.Events;

public class PlayerDatas : MonoBehaviour
{
    public static PlayerDatas Instance { get; private set; }  // Singleton

    [SerializeField] 
    private int _maxHp = 100;
    private int _hp;

    public int MaxHp => _maxHp;

    public int Hp
    {
        get => _hp;
        private set
        {
            var isDamage = value < _hp;
            _hp = Mathf.Clamp(value, 0, _maxHp);

            if (isDamage)
                Damaged?.Invoke(_hp);
            else
                Healed?.Invoke(_hp);

            if (_hp <= 0)
                Died?.Invoke();
        }
    }

    public UnityEvent<int> Healed;
    public UnityEvent<int> Damaged;
    public UnityEvent Died;

    private void Awake()
    {
        if (Instance == null)  
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene il player tra le scene
            _hp = _maxHp;  // Inizializza la vita solo alla prima istanza
        }
        else
        {
            Destroy(gameObject); // Evita duplicati nei nuovi livelli
        }
    }

    public void Damage(int amount) => Hp -= amount;
    public void Heal(int amount) => Hp += amount;
    public void HealFull() => Hp = _maxHp;
    public void Kill() => Hp = 0;
    public void Adjust(int value) => Hp = value;
}
