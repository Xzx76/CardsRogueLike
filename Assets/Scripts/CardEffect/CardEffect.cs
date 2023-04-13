using Asset;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect: ScriptableObject
{
    public virtual void Resolve(PlayerAsset player)
    {

    }
    public virtual void Resolve(PlayerAsset player,EnemyAsset enemy)
    {

    }
    public virtual void Resolve(PlayerAsset player, List<EnemyAsset> enemy)
    {

    }

}
