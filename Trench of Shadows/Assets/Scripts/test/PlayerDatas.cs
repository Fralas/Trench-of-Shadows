using UnityEngine;

public class PlayerDatas : MonoBehaviour
{
    public static PlayerDatas instance;

    [SerializeField] private int health = 10;   // Vita (0-10)
    [SerializeField] private int armor = 5;     // Armatura (0-5)
    [SerializeField] private int stamina = 20;  // Stamina (0-20)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene il player tra le scene
        }
        else
        {
            Destroy(gameObject); // Evita duplicati
        }
    }

    // Getter per ottenere i valori
    public int GetHealth() => health;
    public int GetArmor() => armor;
    public int GetStamina() => stamina;

    // Setter con controllo limiti
    public void SetHealth(int value)
    {
        health = Mathf.Clamp(value, 0, 10);
        Debug.Log("Vita aggiornata: " + health);
    }

    public void SetArmor(int value)
    {
        armor = Mathf.Clamp(value, 0, 5);
        Debug.Log("Armatura aggiornata: " + armor);
    }

    public void SetStamina(int value)
    {
        stamina = Mathf.Clamp(value, 0, 20);
        Debug.Log("Stamina aggiornata: " + stamina);
    }

    // Metodi per modificare i valori in modo incrementale
    public void TakeDamage(int damage)
    {
        int remainingDamage = damage;

        if (armor > 0)
        {
            remainingDamage -= armor;
            SetArmor(armor - damage);
        }

        if (remainingDamage > 0)
        {
            SetHealth(health - remainingDamage);
        }
    }

    public void Heal(int amount)
    {
        SetHealth(health + amount);
    }

    public void RestoreStamina(int amount)
    {
        SetStamina(stamina + amount);
    }

    public void UseStamina(int amount)
    {
        SetStamina(stamina - amount);
    }
}
