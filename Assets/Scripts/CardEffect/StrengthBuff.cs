using Asset;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asset
{
    public class StrengthBuff : CardEffect
    {
        public override void Resolve(PlayerController player = null, List<EnemyController> enemys = null, EnemyController enemy = null)
        {
            player.Asset.SetAttribute(Attributes.Strength, Card.BuffValue[InCardIdx]);
        }
    }
}
