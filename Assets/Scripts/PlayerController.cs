using Asset;
using System;
using System.Collections;
using System.Collections.Generic;
using TocClient;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerAsset asset;

    private void Awake()
    {
        
    }
    public void OnAttack(int damage)
    {
        asset.currentHp -= damage;
        MsgSystem.Instance.Dispatch<int,int>(Constants.Msg_PlayerHealthChange, asset.currentHp,asset.maxHp);
    }
    public void OnShiled(int ShiledNum)
    {

    }
    public void OnHealing(int healNum)
    {

    }
}
