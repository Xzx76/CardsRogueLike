using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace Asset
{
    public enum Attributes
    {
        Strength,
        Agility,
        CostAddition
    }
    [CreateAssetMenu(fileName = "NEW PlayerAsset", menuName = "MyAsset/PlayerAsset", order = 0)]
    public class PlayerAsset : ScriptableObject
    {
        [BoxGroup("��������"), Label("��������")] public int baseHp;
        [BoxGroup("��������"), Label("��������")] public int baseStrength;
        [BoxGroup("��������"), Label("��������")] public int baseAgility;
        [BoxGroup("��������"), Label("��������")] public int baseCostAddition;
        public int maxHp;
        public int currentHp;
        public int strength;
        public int agility;
        public int costAddition;
        Action _playerAttrChangeCb;
        public void Init()
        {
            maxHp = baseHp;
            strength = baseStrength;
            agility = baseAgility;
            costAddition = baseCostAddition;
        }
        public void SetAttribute(Attributes attributes,int value)
        {
            switch (attributes)
            {
                case Attributes.Strength:
                    strength += value;
                    _playerAttrChangeCb?.Invoke();
                    break;
                case Attributes.Agility:
                    agility += value;
                    _playerAttrChangeCb?.Invoke();
                    break;
                case Attributes.CostAddition:
                    costAddition += value;
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
}

