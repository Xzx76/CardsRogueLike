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
        [BoxGroup("基础数据"), Label("基础生命")] public int baseHp;
        [BoxGroup("基础数据"), Label("基础力量")] public int baseStrength;
        [BoxGroup("基础数据"), Label("基础灵巧")] public int baseAgility;
        [BoxGroup("基础数据"), Label("基础灵巧")] public int baseCostAddition;
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

