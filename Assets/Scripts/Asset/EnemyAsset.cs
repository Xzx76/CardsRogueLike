using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace Asset
{
    [CreateAssetMenu(fileName = "NEW PlayerAsset", menuName = "MyAsset/EnemyAsset", order = 0)]
    public class EnemyAsset : ScriptableObject
    {

        public int maxHp;
        public int currentHp;
        public int strength;
        public int agility;

    }
}

