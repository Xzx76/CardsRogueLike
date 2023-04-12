using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using TocClient;
using System.Collections.Generic;

public class FightManager : MonoBehaviour
{
    public Transform root; //Canvas
    public Image pic;
    List<Image> pics;
    public int length = 10;//数组长度
    public float width = 80f;//图片间距
    public float angl = 8;//图片角度差
    public Transform cardGroup;
    public Transform usedCardGroup;
    private bool cardLoadFinish;
    private void Awake()
    {
        MsgSystem.Instance.AddListener<int>(Constants.Msg_UseCard, UseCard);
    }
    void Start()
    {
        
        //init();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            init();
        }
    }
    void init()
    {
        if (pics !=null)
        {
            for (int i = 0; i < pics.Count; i++)
            {
                Destroy(pics[i].gameObject);
            }
        }

        pics = new();
        if (length>8)
            width = width * 7 / length;
        StartCoroutine(CreateCards());
        
    }
    void UseCard(int cardIdx)
    {
        StartCoroutine(UseCards(cardIdx));
    }
    void sort()
    {

        for (int i = 0; i < length; i++)
        {
            float num = 0;
            if ((Mathf.Sin(Mathf.Deg2Rad * angl * ((float)(length - 1) / 2 - i)) == 0) || (length == 1))
                num = 0;
            else
                num = -(width * ((float)(length - 1) / 2 - i)) / Mathf.Sin(Mathf.Deg2Rad * angl * ((float)(length - 1) / 2 - i)) + (1f / (Mathf.Tan(Mathf.Deg2Rad * angl * ((float)(length - 1) / 2 - i))) * (width * ((float)(length - 1) / 2 - i)));
            pics[i].rectTransform.DOLocalMove(new Vector3(-(width * ((float)(length - 1) / 2 - i)), num, 0),0.2f);
            pics[i].rectTransform.DOLocalRotate(new Vector3(0, 0, angl * ((float)(length - 1) / 2 - i)),0.2f);
            pics[i].GetComponent<PreView>().SetCardSate(new Vector3(-(width * ((float)(length - 1) / 2 - i)), num, 0), new Vector3(0, 0, angl * ((float)(length - 1) / 2 - i)), i);
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
            output.Insert(Random.Range(0, output.Count), item);
        }
        return output;
    }
    IEnumerator CreateCards()
    {
        for (int i = 0; i < length; i++)
        {
            int idx = i;
            var animSeque = DOTween.Sequence();
            pics.Add(Instantiate(pic));
            pics[idx].GetComponent<PreView>().CardCreateFinish = false;
            pics[idx].GetComponent<PreView>().Init();
            float num = 0;
            if ((Mathf.Sin(Mathf.Deg2Rad * angl * ((float)(length - 1) / 2 - i))==0) || (length == 1))
                num = 0;
            else
                num = -(width * ((float)(length - 1) / 2 - i)) / Mathf.Sin(Mathf.Deg2Rad * angl * ((float)(length - 1) / 2 - i)) + (1f / (Mathf.Tan(Mathf.Deg2Rad * angl * ((float)(length - 1) / 2 - i))) * (width * ((float)(length - 1) / 2 - i)));
            pics[idx].GetComponent<PreView>().SetCardSate(new Vector3(-(width * ((float)(length - 1) / 2 - i)), num, 0), new Vector3(0, 0, angl * ((float)(length - 1) / 2 - i)),idx);
            pics[idx].transform.parent = root.parent;
            pics[idx].rectTransform.localPosition = cardGroup.localPosition;
            pics[idx].transform.parent = root;
            animSeque.Append(pics[idx].transform.DOScale(Vector3.one, 0.5f));
            animSeque.Insert(0.3f,pics[idx].transform.DOLocalMove(new Vector3(-(width * ((float)(length - 1) / 2 - i)), num, 0), 0.5f));
            animSeque.Insert(0.1f, pics[i].GetComponent<PreView>().CardImg.DOColor(Color.white,0.7f));
            animSeque.Insert(0.7f,pics[idx].transform.DOLocalRotate(new Vector3(0, 0, angl * ((float)(length - 1) / 2 - i)), 0.25f));
            animSeque.InsertCallback(0.95f,()=>
            {
                pics[idx].GetComponent<PreView>().CardCreateFinish = true;
            });
            yield return new WaitForSeconds(0.15f);
        }
    }
    IEnumerator UseCards(int cardIdx)
    {
        var animSeque = DOTween.Sequence();
        pics[cardIdx].GetComponent<PreView>().CardCreateFinish = false;
        animSeque.Append(pics[cardIdx].transform.DOLocalMoveY(pics[cardIdx].transform.localPosition.y + 200f, 0.5f));
        animSeque.Append(pics[cardIdx].transform.DOLocalMove(usedCardGroup.localPosition, 0.5f));
        animSeque.Insert(0.2f, pics[cardIdx].GetComponent<PreView>().CardImg.DOColor(Color.white, 0.8f));
        animSeque.Insert(0.2f, pics[cardIdx].transform.DOScale(Vector3.zero, 0.6f));
        animSeque.Insert(0.5f, pics[cardIdx].transform.DOLocalRotate(new Vector3(0, 0, -360), 0.4f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart));
        yield return new WaitForSeconds(0.5f);
        length--;
        Transform tmp = pics[cardIdx].transform;
        pics.RemoveAt(cardIdx);
        sort();
        yield return new WaitForSeconds(0.5f);
        Destroy(tmp.gameObject);
    }
}
