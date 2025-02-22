using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform _barRect;
    [SerializeField] private RectMask2D _mask;
    [SerializeField] private TMP_Text _hpIndicator;
    private EnemyHealth _enemyHealth;

    private float _maxRightMask;
    private float _initialRightMask;
    private bool _initialized = false;

    void Start()
    {
        _enemyHealth = GetComponentInParent<EnemyHealth>();

        if (_enemyHealth == null)
        {
            Debug.LogError("EnemyHealthBar: Nessun EnemyHealth trovato nel genitore!");
            return;
        }

        // Assicura il calcolo corretto della dimensione
        Invoke(nameof(InitializeBar), 0.1f);
    }

    private void InitializeBar()
    {
        _maxRightMask = _barRect.rect.width - _mask.padding.x - _mask.padding.z;
        _initialRightMask = _mask.padding.z;

        _enemyHealth.Damaged.AddListener(SetValue);
        SetValue(_enemyHealth.Hp);

        _initialized = true;
    }

    public void SetValue(int currentHealth)
    {
        if (_enemyHealth == null || !_initialized) return;

        float targetWidth = currentHealth * _maxRightMask / _enemyHealth.MaxHp;
        float newRightMask = _maxRightMask + _initialRightMask - targetWidth;

        var padding = _mask.padding;
        padding.z = newRightMask;
        _mask.padding = padding;

        _hpIndicator.SetText($"{currentHealth}/{_enemyHealth.MaxHp}");
    }
}
