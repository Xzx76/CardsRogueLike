using System;
using System.Collections;
using System.Collections.Generic;
using TocClient;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
/*    private List<CardAsset> cards = new();
    private CardAsset[] cardAsset;*/
    public GameObject[] ExistCard;
    public Transform CardSolt;
    private void Awake()
    {
        //cards是卡资源
/*        for (int i = 0; i < 2; i++)
        {
            cards.Add(Resources.Load<CardAsset>("Cards/"+i));
        }
        //cardAsset卡池
        cardAsset = new CardAsset[] { cards[0], cards[0], cards[0], cards[1], cards[1], cards[1], cards[1], cards[0], cards[0], cards[0], cards[0] };
        Debug.Log("global.cardAsset.length =" + cardAsset.Length.ToString());
        for (int i = 0; i < 5; i++)
        {
            GameObject tmp=ObjectPoolManager.Instance.Creat("Assets/Prefab/Card");
            Text name = UnityHelper.GetComponent<Text>(tmp, "name");
            Text desc = UnityHelper.GetComponent<Text>(tmp, "desc");
            name.text = cardAsset[i].cardName; 
            desc.text = cardAsset[i].desc.Replace("%", cardAsset[i].baseValue.ToString()); 
            tmp.transform.SetParent(CardSolt);
        }*/
    }
}
