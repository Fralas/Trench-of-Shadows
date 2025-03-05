using UnityEngine.UI;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private PlayerDatas _health;

    [SerializeField]
    private RectTransform _barRect;

    [SerializeField]
    private RectMask2D _mask;

    [SerializeField]
    private TMP_Text _hpIndicator;

    private float _maxRightMask;
    private float _initialRightMask;

    void Start()
    {
        // Subscribe to the MaxHealthChanged event
        _health.MaxHealthChanged.AddListener(UpdateHealthBar);

        //x=left, w=top, y=bottom, z=right
        _maxRightMask = _barRect.rect.width - _mask.padding.x - _mask.padding.z;
        _hpIndicator.SetText($"{_health.Hp}/{_health.MaxHp}");
        _initialRightMask = _mask.padding.z;

        // Initial health bar setup
        SetValue(_health.Hp);
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        _health.MaxHealthChanged.RemoveListener(UpdateHealthBar);
    }

    private void UpdateHealthBar(int newMaxHealth)
    {
        // When max health changes, we need to recalculate the bar width and the value
        _maxRightMask = _barRect.rect.width - _mask.padding.x - _mask.padding.z;
        SetValue(_health.Hp);  // Update the current HP display
    }

    public void SetValue(int newValue)
    {
        var targetWidth = newValue * _maxRightMask / _health.MaxHp;
        var newRightMask = _maxRightMask + _initialRightMask - targetWidth;
        var padding = _mask.padding;
        padding.z = newRightMask;
        _mask.padding = padding;
        _hpIndicator.SetText($"{newValue}/{_health.MaxHp}");
    }
}
