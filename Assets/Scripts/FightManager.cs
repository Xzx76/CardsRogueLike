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

    [SerializeField]
    public int roundHeadCardNum;

    private Action roundOver;

    private int seed = 24675120;
    private System.Random random;
    private List<GameObject> headCards;//手卡物体

    public List<CardAsset> CardGroup;//卡组
    public List<CardAsset> CardInGroup;//牌堆
    public List<CardAsset> CardsUsed;//弃牌堆
    public List<CardAsset> CardsInHead;//手卡
    private void Awake()
    {
        random = new System.Random(seed);
        MsgSystem.Instance.AddListener<int>(Constants.Msg_UseCard, UseCard);
    }
    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            if (i<9)
                CardInGroup.Add(PairMgr.Instance.TestCard);
            else
                CardInGroup.Add(PairMgr.Instance.TestCard1);
        }
    }


    void Update()
    {
    }
    /// <summary>
    /// 抽卡
    /// </summary>
    /// <param name="CardNum"></param>
    /// <param name="IsReliver">是否补充抽卡</param>
    public void DeliverCard(int CardNum,bool IsReliver)
    {

        if (!IsReliver)
        {
            if (headCards != null)
            {
                foreach (var ec in headCards)
                    Destroy(ec);
            }
            headCards = new();
            CardsInHead = new();
            for (int i = 0; i < CardNum; i++)
                CardsInHead.Add(CardAppear());
/*            if (CardsInHead.Count > 8)
                width = width * 7 / CardsInHead.Count;*/
            StartCoroutine(CreateDefultHeadCards());
        }else
        {
            for (int i = 0; i < CardNum; i++)
                CardsInHead.Add(CardAppear());
/*            if (CardsInHead.Count > 8)
                width = width * 7 / CardsInHead.Count;*/
            StartCoroutine(CreateExtraHeadCards(CardNum));
        }
    }
    /// <summary>
    /// 牌堆获取卡牌数据
    /// </summary>
    /// <returns></returns>
    private CardAsset CardAppear()
    {
        //当抽牌堆里已经没有牌的时候
        if (CardInGroup.Count == 0)
        {
            //重置抽牌堆和弃牌堆
            CardInGroup = CardsUsed;
            CardsUsed = new();
            //抽牌堆用随机种子洗牌：遍历每个元素，将它和随机的元素交换位置
            for (int j = 0; j < CardInGroup.Count - 1; j++)
            {
                int rd = random.Next(0, CardInGroup.Count - 1);
                var t = CardInGroup[rd];
                CardInGroup[rd] = CardInGroup[j];
                CardInGroup[j] = t;
            }
            //随机种子重置，以免每次洗牌结果相同
            random = new System.Random(++seed);
        }

        //拿到抽牌堆的最后一张牌
        int r = CardInGroup.Count - 1;
        var cardData = CardInGroup[r];
        CardInGroup.RemoveAt(r);
        return cardData;
    }

    public void DropAll()
    {
        for (int i = 0; i < CardsInHead.Count; i++)
        {
            CardsUsed.Add(CardsInHead[i]);
        }
        StartCoroutine(DropAllCards());
    }
    public void DropByIdx(int cardIdx=0)
    {
        var usedCard = CardsInHead[cardIdx];
        CardsInHead.RemoveAt(cardIdx);
        CardsUsed.Add(usedCard);
        StartCoroutine(DropCard(cardIdx));
    }
    void UseCard(int cardIdx)
    {
        var usedCard = CardsInHead[cardIdx];
        for (int i = 0; i < usedCard.Effects.Count; i++)
        {
            usedCard.Effects[i].Resolve();
        }
        CardsInHead.RemoveAt(cardIdx);
        CardsUsed.Add(usedCard);
        StartCoroutine(UseCards(cardIdx));
    }
    /// <summary>
    /// 重排手卡
    /// </summary>
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
    /// 空手卡卡牌生成
    /// </summary>
    /// <returns></returns>
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
            card.CardInfo = CardsInHead[i];
            float num = 0;
            if ((Mathf.Sin(Mathf.Deg2Rad * angl * ((float)(roundHeadCardNum - 1) / 2 - i)) == 0) || (roundHeadCardNum == 1))
                num = 0;
            else
                num = -(width * ((float)(roundHeadCardNum - 1) / 2 - i)) / Mathf.Sin(Mathf.Deg2Rad * angl * ((float)(roundHeadCardNum - 1) / 2 - i)) + (1f / (Mathf.Tan(Mathf.Deg2Rad * angl * ((float)(roundHeadCardNum - 1) / 2 - i))) * (width * ((float)(roundHeadCardNum - 1) / 2 - i)));
            card.SetCardSate(new Vector3(-(width * ((float)(roundHeadCardNum - 1) / 2 - i)), num, 0), new Vector3(0, 0, angl * ((float)(roundHeadCardNum - 1) / 2 - i)), idx);

            tmpCard.transform.parent = root;
            animSeque.Append(tmpCard.transform.DOScale(Vector3.one, 0.5f));
            animSeque.Insert(0.3f, tmpCard.transform.DOLocalMove(new Vector3(-(width * ((float)(roundHeadCardNum - 1) / 2 - i)), num, 0), 0.5f));
            animSeque.Insert(0.1f, card.CardImg.DOColor(Color.white, 0.7f));
            animSeque.Insert(0.7f, tmpCard.transform.DOLocalRotate(new Vector3(0, 0, angl * ((float)(roundHeadCardNum - 1) / 2 - i)), 0.25f));
            animSeque.InsertCallback(0.95f, () =>
             {
                 card.CardCreateFinish = true;
             });
            yield return new WaitForSeconds(0.15f);
        }
    }
    /// <summary>
    /// 补充抽卡卡牌生成
    /// </summary>
    /// <param name="extraCards"></param>
    /// <returns></returns>
    IEnumerator CreateExtraHeadCards(int extraCardNum)
    {
        for (int i = CardsInHead.Count-extraCardNum; i < CardsInHead.Count; i++)
        {
            var animSeque = DOTween.Sequence();
            GameObject tmpCard = AssetManager.Instance.Spawn("CardPrefab", cardGroup).gameObject;
            headCards.Add(tmpCard);
            CardViewController card = tmpCard.GetComponent<CardViewController>();
            card.CardCreateFinish = false;
            card.Init();
            card.CardInfo = card.CardInfo = CardsInHead[i];
            float num = 0;
            if ((Mathf.Sin(Mathf.Deg2Rad * angl * ((float)(CardsInHead.Count - 1) / 2 - i)) == 0) || (CardsInHead.Count == 1))
                num = 0;
            else
                num = -(width * ((float)(CardsInHead.Count - 1) / 2 - i)) / Mathf.Sin(Mathf.Deg2Rad * angl * ((float)(CardsInHead.Count - 1) / 2 - i)) + (1f / (Mathf.Tan(Mathf.Deg2Rad * angl * ((float)(CardsInHead.Count - 1) / 2 - i))) * (width * ((float)(CardsInHead.Count - 1) / 2 - i)));
            card.SetCardSate(new Vector3(-(width * ((float)(CardsInHead.Count - 1) / 2 - i)), num, 0), new Vector3(0, 0, angl * ((float)(CardsInHead.Count - 1) / 2 - i)), i);

            tmpCard.transform.parent = root;
            animSeque.Append(tmpCard.transform.DOScale(Vector3.one, 0.5f));
            animSeque.Insert(0.3f, tmpCard.transform.DOLocalMove(new Vector3(-(width * ((float)(CardsInHead.Count - 1) / 2 - i)), num, 0), 0.5f));
            animSeque.Insert(0.1f, card.CardImg.DOColor(Color.white, 0.7f));
            animSeque.Insert(0.7f, tmpCard.transform.DOLocalRotate(new Vector3(0, 0, angl * ((float)(CardsInHead.Count - 1) / 2 - i)), 0.25f));
            animSeque.InsertCallback(0.95f, () =>
            {
                ReSortHeadCards();
                card.CardCreateFinish = true;
            });
            yield return new WaitForSeconds(0.15f);
        }
    }
    /// <summary>
    /// 卡牌使用
    /// </summary>
    /// <param name="cardIdx"></param>
    /// <returns></returns>
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
    /// <summary>
    /// 丢弃全部卡牌
    /// </summary>
    /// <returns></returns>
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
    IEnumerator DropCard(int cardIdx)
    {
        int idx = cardIdx;
        var animSeque = DOTween.Sequence();
        headCards[idx].GetComponent<CardViewController>().CardCreateFinish = false;
        animSeque.Append(headCards[idx].transform.DOLocalMove(usedCardGroup.localPosition, 0.6f));
        animSeque.Insert(0.2f, headCards[idx].GetComponent<CardViewController>().CardImg.DOColor(Color.white, 0.4f));
        animSeque.Insert(0.2f, headCards[idx].transform.DOScale(Vector3.zero, 0.4f));
        animSeque.Insert(0.2f, headCards[idx].transform.DOLocalRotate(new Vector3(0, 0, -360), 0.4f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart));
        yield return new WaitForSeconds(0.6f);
        var tmp = headCards[cardIdx];
        headCards.RemoveAt(cardIdx);
        ReSortHeadCards();
        Destroy(tmp);
    }
}
