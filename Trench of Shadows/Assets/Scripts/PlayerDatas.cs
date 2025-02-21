using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerDatas : MonoBehaviour
{
    public static PlayerDatas Instance { get; private set; }  // Singleton

    [SerializeField] private int _maxHp = 100;
    private int _hp;

    [SerializeField] private int _maxHunger = 100;  
    private int _hunger;

    [Header("Hunger Settings")]
    public float HungerDecayRate = 1f; // Di quanto diminuisce la fame
    public float HungerDecayInterval = 5f; // Ogni quanti secondi diminuisce
    public int HungerDamageAmount = 5; // Danno ricevuto per fame 0
    public float HungerDamageInterval = 3f; // Tempo tra un danno e l'altro

    private bool isStarving = false; // Controlla se il player sta prendendo danno per fame

    public int MaxHp => _maxHp;
    public int MaxHunger => _maxHunger;

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

    public int Hunger
    {
        get => _hunger;
        private set
        {
            _hunger = Mathf.Clamp(value, 0, _maxHunger);
            HungerChanged?.Invoke(_hunger);

            if (_hunger == 0 && !isStarving)
            {
                StartCoroutine(StarvationDamage());
            }
        }
    }

    public UnityEvent<int> Healed;
    public UnityEvent<int> Damaged;
    public UnityEvent Died;
    public UnityEvent<int> HungerChanged;

    private void Awake()
    {
        if (Instance == null)  
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _hp = _maxHp;
            _hunger = _maxHunger;
            StartCoroutine(ConsumeHungerOverTime());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Damage(int amount) => Hp -= amount;
    public void Heal(int amount) => Hp += amount;
    public void HealFull() => Hp = _maxHp;
    public void Kill() => Hp = 0;
    public void Adjust(int value) => Hp = value;

    public void ConsumeHunger(int amount) => Hunger -= amount;
    public void RestoreHunger(int amount) => Hunger += amount;
    public void AdjustHunger(int value) => Hunger = value;

    private IEnumerator ConsumeHungerOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(HungerDecayInterval);
            ConsumeHunger((int)HungerDecayRate);
        }
    }

    private IEnumerator StarvationDamage()
    {
        isStarving = true;

        while (_hunger == 0) // Continua finché la fame è a 0
        {
            Damage(HungerDamageAmount);
            Debug.Log($"Il player ha fame! Danno ricevuto: {HungerDamageAmount} HP.");
            yield return new WaitForSeconds(HungerDamageInterval);
        }

        isStarving = false;
    }
}
