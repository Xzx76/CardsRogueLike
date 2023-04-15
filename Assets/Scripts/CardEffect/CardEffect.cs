using Asset;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asset
{
    public class CardEffect : ScriptableObject
    {
        public CardAsset Card;
        public int InCardIdx;
        public virtual void Resolve(PlayerController player = null, List<EnemyController> enemys = null, EnemyController enemy = null)
        {

        }
    }
}

