using Asset;
using System;
using System.Collections;
using System.Collections.Generic;
using TocClient;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public PlayerAsset Asset;

    private void Awake()
    {
        
    }
    public void InitPlayerAsset()
    {
        Asset.Init();
        Asset.SetChangeEvent(()=>
        {
            MsgSystem.Instance.Dispatch(Constants.Msg_PlayerAttributeChange);
        });
    }
    public void OnAttack(int damage)
    {
        Asset.currentHp -= damage;
        MsgSystem.Instance.Dispatch<int,int>(Constants.Msg_PlayerHealthChange, Asset.currentHp, Asset.maxHp);
    }
    public void OnShiled(int ShiledNum)
    {

    }
    public void OnHealing(int healNum)
    {

    }
}
