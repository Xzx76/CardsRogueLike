using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attributes
{
    Strength=1,
    Agility,
    CostAddition
}
public class PlayerInfo
{
    public int BaseHp { get; set; }
    public int BaseStrength { get; set; }
    public int BaseAgility { get; set; }
    public int BaseCostAddition { get; set; }
    public int MaxHp { get; set; }
    public int CurrentHp { get; set; }
    public int Strength { get; set; }
    public int Agility { get; set; }
    public int CostAddition { get; set; }
    Action _playerAttrChangeCb;
    public void Init()
    {
        MaxHp = BaseHp;
        CurrentHp = BaseHp;
        Strength = BaseStrength;
        Agility = BaseAgility;
        CostAddition = BaseCostAddition;
    }
    public void SetAttribute(Attributes attributes, int value)
    {
        switch (attributes)
        {
            case Attributes.Strength:
                Strength += value;
                _playerAttrChangeCb?.Invoke();
                break;
            case Attributes.Agility:
                Agility += value;
                _playerAttrChangeCb?.Invoke();
                break;
            case Attributes.CostAddition:
                CostAddition += value;
                _playerAttrChangeCb?.Invoke();
                break;
            default:
                break;
        }
    }
    public void SetChangeEvent(System.Action cb)
    {
        _playerAttrChangeCb = cb;
    }
}
