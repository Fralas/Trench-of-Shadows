using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthBar : MonoBehaviour
{
    private Enemy _enemy; 
    [SerializeField] private RectTransform _barRect; 
    [SerializeField] private RectMask2D _mask; 
    [SerializeField] private TMP_Text _hpIndicator;

    private float _maxRightMask;
    private float _initialRightMask;

    void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
        _maxRightMask = _barRect.rect.width - _mask.padding.x - _mask.padding.z;
        _initialRightMask = _mask.padding.z;
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        if (_enemy == null) return;

        int currentHealth = _enemy.CurrentHealth;
        int maxHealth = _enemy.MaxHealth;

        float targetWidth = currentHealth * _maxRightMask / maxHealth;
        float newRightMask = _maxRightMask + _initialRightMask - targetWidth;

        var padding = _mask.padding;
        padding.z = newRightMask;
        _mask.padding = padding;

        _hpIndicator.SetText($"{currentHealth}/{maxHealth}");
    }
}