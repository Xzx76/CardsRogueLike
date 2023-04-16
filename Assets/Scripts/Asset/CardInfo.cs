using Asset;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CardType
{
    Attack,
    Skill,
}
public enum CardQuality
{
    Write,
    Blue,
    Yellow
}
public class CardInfo
{
    Action _cardAttrChangeCb;
    public int CardId { get; set; } 
    public CardType CardType { get; set; }
    public CardQuality CardQuality { get; set; }
    public string CardSprite;
    public int BaseExpend { get; set; }
    public int[] BuffValue { get; set; }
    public int BuffRound { get; set; }
    public string CardName { get; set; }
    public string Target { get; set; }
    public int UpgradeCardId { get; set; }
    public List<CardEffect> Effects { get; set; }
    public int[] AdditionTypes { get; set; }
    public int[] BaseAddition { get; set; }
    public int CurrencyExpend { get; set; }
    public int[] CurrencyAddition { get; set; }
    public string[] EffectNames
    {
        set
        {
            for (int i = 0; i < value.Length; i++)
            {
                var effect = ScriptableObject.CreateInstance(Type.GetType(value[i])) as CardEffect;
                if (effect != null)
                {
                    effect.hideFlags = HideFlags.HideInHierarchy;
                    effect.CardInfo = this;
                    effect.InCardIdx = i;
                    Effects.Add(effect);
                }
            }
        }
    }
    public void UpdateCards(PlayerInfo player)
    {
        for (int i = 0; i < AdditionTypes.Length; i++)
        {
            switch (AdditionTypes[i])
            {
                case 0:
                    CurrencyAddition[i] = BaseAddition[i] + player.Strength;
                    break;
                case 1:
                    CurrencyAddition[i] = BaseAddition[i] + player.Agility;
                    break;
                default:
                    break;
            }
        }
        _cardAttrChangeCb?.Invoke();
    }
    public void ResetCards()
    {
        CurrencyAddition = BaseAddition;
        CurrencyExpend = BaseExpend;
        _cardAttrChangeCb = null;
    }
    public int  GetAdditionByType(int type)
    {
        for (int i = 0; i < AdditionTypes.Length; i++)
        {
            if (AdditionTypes[i]==type)
            {
                return CurrencyAddition[i];
            }
        }
        return 0;
    }
    public void SetChangeEvent(System.Action cb)
    {
        _cardAttrChangeCb = cb;
    }
    private string _desc;
    public string Desc
    {
        set
        {
            _desc = value;
        }
        get
        {
            return  String.Format(_desc,CurrencyAddition);
        }
    }
}
