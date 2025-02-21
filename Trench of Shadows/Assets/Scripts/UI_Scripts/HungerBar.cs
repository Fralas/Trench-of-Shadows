using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HungerBar : MonoBehaviour
{
    [SerializeField]
    private PlayerDatas _playerData;

    [SerializeField]
    private RectTransform _barRect;

    [SerializeField]
    private RectMask2D _mask;

    [SerializeField]
    private TMP_Text _hungerIndicator;

    private float _maxRightMask;
    private float _initialRightMask;

    void Start()
    {
        _maxRightMask = _barRect.rect.width - _mask.padding.x - _mask.padding.z;
        _hungerIndicator.SetText($"{_playerData.Hunger}/{_playerData.MaxHunger}");
        _initialRightMask = _mask.padding.z;

        _playerData.HungerChanged.AddListener(SetValue);
    }

    public void SetValue(int newValue)
    {
        var targetWidth = newValue * _maxRightMask / _playerData.MaxHunger;
        var newRightMask = _maxRightMask + _initialRightMask - targetWidth;
        var padding = _mask.padding;
        padding.z = newRightMask;
        _mask.padding = padding;
        _hungerIndicator.SetText($"{newValue}/{_playerData.MaxHunger}");
    }
}
