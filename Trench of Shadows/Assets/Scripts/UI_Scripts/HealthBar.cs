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
        //x=left, w=top, y=bottom, z=right
        _maxRightMask = _barRect.rect.width - _mask.padding.x - _mask.padding.z;
        _hpIndicator.SetText($"{_health.Hp}/{_health.MaxHp}");
        _initialRightMask = _mask.padding.z; 
    }

    public void SetValue(int newValue){
        var targetWidth = newValue * _maxRightMask / _health.MaxHp;
        var newRightMask = _maxRightMask + _initialRightMask - targetWidth;
        var padding = _mask.padding;
        padding.z = newRightMask;
        _mask.padding = padding;
        _hpIndicator.SetText($"{newValue}/{_health.MaxHp}");
    }

   
}