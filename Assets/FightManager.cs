using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using TocClient;
using System.Collections.Generic;
using Asset;
using System;

public class FightManager : MonoBehaviour
{
    public Transform root; //Canvas

    public float width = 120f;//图片间距
    public float angl = 8;//图片角度差
    public Transform cardGroup;
    public Transform usedCardGroup;
    private bool cardLoadFinish;

    [SerializeField]
    private int defaultHeadCardsNum;

    private int seed = 24675120;
    private System.Random random;
    private List<GameObject> headCards;//手卡物体

    public List<CardAsset> CardGroup;//卡组
    public List<CardAsset> CardInGroup;//牌堆
    public List<CardAsset> CardsUsed;//弃牌堆
    public List<CardAsset> CardsInHead;//手卡
    private void Awake()
    {
        random = new System.Random();
        MsgSystem.Instance.AddListener<int>(Constants.Msg_UseCard, UseCard);
    }
    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            CardInGroup.Add(PairMgr.Instance.TestCard);
        }
        CreateDefaultHeadCards();
    }
    void Update()
    {
    }
    private void CreateDefaultHeadCards()
    {
        if (headCards!=null)
        {
            for (int i = 0; i < headCards.Count; i++)
                Destroy(headCards[i]);
        }
        headCards = new();
        CardsInHead = new();
        for (int i = 0; i <= defaultHeadCardsNum; i++)
        {
            CardsInHead.Add(CardInGroup[i]);
        }
        StartCoroutine(CreateDefultHeadCards());
    }
    void UseCard(int cardIdx)
    {
        StartCoroutine(UseCards(cardIdx));
    }
    void ReSortHeadCards()
    {

        for (int i = 0; i < headCards.Count; i++)
        {
            float num = 0;
            if ((Mathf.Sin(Mathf.Deg2Rad * angl * ((float)(headCards.Count - 1) / 2 - i)) == 0) || (headCards.Count == 1))
                num = 0;
            else
                num = -(width * ((float)(headCards.Count - 1) / 2 - i)) / Mathf.Sin(Mathf.Deg2Rad * angl * ((float)(headCards.Count - 1) / 2 - i)) + (1f / (Mathf.Tan(Mathf.Deg2Rad * angl * ((float)(headCards.Count - 1) / 2 - i))) * (width * ((float)(headCards.Count - 1) / 2 - i)));
            headCards[i].transform.DOLocalMove(new Vector3(-(width * ((float)(headCards.Count - 1) / 2 - i)), num, 0),0.2f);
            headCards[i].transform.DOLocalRotate(new Vector3(0, 0, angl * ((float)(headCards.Count - 1) / 2 - i)),0.2f);
            headCards[i].GetComponent<CardViewController>().SetCardSate(new Vector3(-(width * ((float)(headCards.Count - 1) / 2 - i)), num, 0), new Vector3(0, 0, angl * ((float)(headCards.Count - 1) / 2 - i)), i);
        }
    }
    /// <summary>
    /// 打乱List的方法
    /// </summary>
    /// <param name="input">所需打乱的List</param>
    /// <typeparam name="T">List的类型</typeparam>
    /// <returns></returns>
    private List<T> Disorganize<T>(List<T> input)
    {
        List<T> output = new List<T>();
        foreach (var item in input)
        {
            output.Insert(UnityEngine.Random.Range(0, output.Count), item);
        }
        return output;
    }
    IEnumerator CreateDefultHeadCards()
    {
        for (int i = 0; i < CardsInHead.Count; i++)
        {
            int idx = i;
            var animSeque = DOTween.Sequence();
            GameObject tmpCard = AssetManager.Instance.Spawn("CardPrefab", cardGroup).gameObject;
            headCards.Add(tmpCard);
            CardViewController card = tmpCard.GetComponent<CardViewController>();
           card.CardCreateFinish = false;
           card.Init();
            float num = 0;
            if ((Mathf.Sin(Mathf.Deg2Rad * angl * ((float)(defaultHeadCardsNum - 1) / 2 - i))==0) || (defaultHeadCardsNum == 1))
                num = 0;
            else
                num = -(width * ((float)(defaultHeadCardsNum - 1) / 2 - i)) / Mathf.Sin(Mathf.Deg2Rad * angl * ((float)(defaultHeadCardsNum - 1) / 2 - i)) + (1f / (Mathf.Tan(Mathf.Deg2Rad * angl * ((float)(defaultHeadCardsNum - 1) / 2 - i))) * (width * ((float)(defaultHeadCardsNum - 1) / 2 - i)));
           card.SetCardSate(new Vector3(-(width * ((float)(defaultHeadCardsNum - 1) / 2 - i)), num, 0), new Vector3(0, 0, angl * ((float)(defaultHeadCardsNum - 1) / 2 - i)),idx);

            tmpCard.transform.parent = root;
            animSeque.Append(tmpCard.transform.DOScale(Vector3.one, 0.5f));
            animSeque.Insert(0.3f,tmpCard.transform.DOLocalMove(new Vector3(-(width * ((float)(defaultHeadCardsNum - 1) / 2 - i)), num, 0), 0.5f));
            animSeque.Insert(0.1f, card.CardImg.DOColor(Color.white,0.7f));
            animSeque.Insert(0.7f,tmpCard.transform.DOLocalRotate(new Vector3(0, 0, angl * ((float)(defaultHeadCardsNum - 1) / 2 - i)), 0.25f));
            animSeque.InsertCallback(0.95f,()=>
            {
               card.CardCreateFinish = true;
            });
            yield return new WaitForSeconds(0.15f);
        }
    }
    IEnumerator UseCards(int cardIdx)
    {
        var animSeque = DOTween.Sequence();
        headCards[cardIdx].GetComponent<CardViewController>().CardCreateFinish = false;
        animSeque.Append(headCards[cardIdx].transform.DOLocalMoveY(headCards[cardIdx].transform.localPosition.y + 200f, 0.5f));
        animSeque.Append(headCards[cardIdx].transform.DOLocalMove(usedCardGroup.localPosition, 0.5f));
        animSeque.Insert(0.2f, headCards[cardIdx].GetComponent<CardViewController>().CardImg.DOColor(Color.white, 0.8f));
        animSeque.Insert(0.2f, headCards[cardIdx].transform.DOScale(Vector3.zero, 0.6f));
        animSeque.Insert(0.5f, headCards[cardIdx].transform.DOLocalRotate(new Vector3(0, 0, -360), 0.4f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart));
        yield return new WaitForSeconds(0.5f);
        Transform tmp = headCards[cardIdx].transform;
        headCards.RemoveAt(cardIdx);
        ReSortHeadCards();
        yield return new WaitForSeconds(0.5f);
        Destroy(tmp.gameObject);
    }
    IEnumerator DropAllCards()
    {
        for (int i = 0; i < headCards.Count; i++)
        {
            int idx = i;
            var animSeque = DOTween.Sequence();
            headCards[i].GetComponent<CardViewController>().CardCreateFinish = false;
            animSeque.Append(headCards[idx].transform.DOLocalMove(usedCardGroup.localPosition, 0.6f));
            animSeque.Insert(0.2f, headCards[idx].GetComponent<CardViewController>().CardImg.DOColor(Color.white, 0.4f));
            animSeque.Insert(0.2f, headCards[idx].transform.DOScale(Vector3.zero, 0.4f));
            animSeque.Insert(0.2f, headCards[idx].transform.DOLocalRotate(new Vector3(0, 0, -360), 0.4f, RotateMode.FastBeyond360)
                    .SetLoops(-1, LoopType.Restart));
        }
        yield return new WaitForSeconds(0.6f);
    }
}
