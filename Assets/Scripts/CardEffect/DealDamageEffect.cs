using Asset;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asset
{
    public class DealDamageEffect : CardEffect
    {

        public override void Resolve(PlayerController player = null, List<EnemyController> enemys = null, EnemyController enemy = null)
        {
            foreach (var t in Card.dictData)
            {
                var value = 0;
                if (t.key=='X')
                {
                    if (Card.dictAddData.Count != 0)
                        value = Card.dictAddData.FirstOrDefault(m => m.key == t.key).data + t.data;
                    else
                        value = t.data;

                }
                player.OnAttack(value);
                break;
            }
        }
    }
}

