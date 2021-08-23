using System;
using Test;
using Test.Enum;
using Test.Helper;
using Test.Model;
using UnityEngine;
using UnityEngine.UI;


public class UnitHealth : MonoBehaviour, ISetDamage
{
    [SerializeField] private Slider _hpSlider;
    private Unit _unit;
    private float _unitHp;    
    private float _maxHp;

    public event Action<InfoCollision> OnApplyDamageChange;
    
    private void Start()
    {
        _unit = GetComponent<Unit>();
        _unit.EventOnRevive += ResetHP;
        _maxHp = _unit.MaxHp;
        _unitHp = _maxHp;

        SetSliderColor(_unit.Fraction);
        _hpSlider.minValue = 0;
        _hpSlider.maxValue = _maxHp;
        _hpSlider.value = _maxHp;

        var damage = GetComponentsInChildren<ISetDamage>();
        foreach (var setDamage in damage)
        {
            setDamage.OnApplyDamageChange += SetDamage;
        }
    }

    private void SetSliderColor(Fraction fraction)
    {
        switch (fraction)
        {
            case Fraction.None:
                _hpSlider.fillRect.GetComponent<Image>().color = Color.blue;
                break;
            case Fraction.Red:
                _hpSlider.fillRect.GetComponent<Image>().color = Color.green;
                break;
            case Fraction.Blue:
                _hpSlider.fillRect.GetComponent<Image>().color = Color.red;
                break;
           
            default:
                _hpSlider.fillRect.GetComponent<Image>().color = Color.blue;
                break;
        }        
    }

    public void SetDamage(InfoCollision info)
    {
        if (_unitHp > 0)
        {
            _unitHp -= info.Damage;
            _hpSlider.value = _unitHp;
        }

        if (_unitHp <= 0)
        {
            _hpSlider.value = _hpSlider.minValue;
            _unit.Die();
        };
    }

    public void ResetHP()
    {
        _unitHp = _maxHp;
        _hpSlider.value = _maxHp;
    }

    private void OnDestroy()
    {
        _unit.EventOnRevive -= ResetHP;
    }
}
