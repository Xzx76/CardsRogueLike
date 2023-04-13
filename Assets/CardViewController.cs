using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using TocClient;
using UnityEngine.UI;
using Asset;

public class CardViewController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static bool EnablePreview = true;

    private Transform cardImgTrans;
    public Image CardImg;
    private Text costNum;
    private Text cardName;
    private Image cardIcon;
    private Text cardDesc;

    private Vector3 savePot;
    private Vector3 saveRot;
    [SerializeField]
    private float upmove;
    [SerializeField]
    private float scaleUper;
    [SerializeField]
    private float animTime;
    [SerializeField]
    private int saveOrder;

    private DragWithTarget dragNoTarget;
    private BezierArrows arrows;
    private RectTransform ArrowSlot;
    private Canvas cv;

    public bool CardCreateFinish;
    public bool CanUse;
    public CardAsset CardInfo
    {
        set
        {
            CardImg.sprite = PairMgr.Instance.GetCardQuility((int)value.cardQuality);
            cardIcon.sprite = value.cardSprite;
            costNum.text = value.Expend.ToString();
            cardName.text = value.cardName;
            cardDesc.text = value.desc;
        }
    }
    public void Init()
    {
        /* cardImgTrans = UnityHelper.GetComponent<Transform>(this.gameObject, "CardShow");*/
        cardImgTrans = this.transform;
        CardImg = UnityHelper.GetComponent<Image>(this.gameObject, "CardShow");
        ArrowSlot = UnityHelper.GetComponent<RectTransform>(this.gameObject, "CardShow/ArrowSlot");

        costNum = UnityHelper.GetComponent<Text>(this.gameObject, "CardShow/CostNum");
        cardName = UnityHelper.GetComponent<Text>(this.gameObject, "CardShow/CardName");
        cardDesc = UnityHelper.GetComponent<Text>(this.gameObject, "CardShow/Desc");
        cardIcon = UnityHelper.GetComponent<Image>(this.gameObject, "CardShow/Icon");
        if (GetComponent<DragWithTarget>() != null)
        {
            dragNoTarget = GetComponent<DragWithTarget>();
        }
        else
        {
            Debug.Log("没有添加拖拽脚本");
        }
        if (GetComponent<BezierArrows>() != null)
        {
            arrows = GetComponent<BezierArrows>();
            arrows.Origin = ArrowSlot;
            arrows.Init();
        }
        else
        {
            Debug.Log("没有添加箭头脚本");
        }
        if (GetComponent<Canvas>() != null)
        {
            cv = GetComponent<Canvas>();
        }
        else
        {
            Debug.Log("没有添加Canvas");
        }
    }
    #region public
    public void TurnArrowColor(int target)
    {
        switch (target)
        {
            case 0:
                arrows.TurnWhite();
                CanUse = false;
                break;
            case 1:
                arrows.TurnRed();
                CanUse = true;
                break;
            case 2:
                arrows.TurnGreen();
                CanUse = true;
                break;
            default:
                break;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!CardCreateFinish)
            return;
        Debug.Log("开始预览");
        if (EnablePreview)
        {
            StartPreView();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!CardCreateFinish)
            return;
        Debug.Log("退出预览");
        if (EnablePreview)
        {
            EndPreView();
        }
    }
    //拖拽时的预览
    public void DragPreview()
    {
        //1.关闭普通预览功能
        EnablePreview = false;
        //2.进入拖拽预览状态
        StartDragPreView();
    }

    //结束拖拽
    public void EndDrag()
    {
        if (CanUse)
        {
            ArrowSlot.gameObject.SetActive(false);
            arrows.StartDraw = false;
            EnablePreview = true;
            MsgSystem.Instance.Dispatch<int>(Constants.Msg_UseCard, saveOrder);
            return;
        }
        cardImgTrans.DOLocalMove(savePot, 0.1f);
        cardImgTrans.DOLocalRotate(saveRot, 0.1f);
        cardImgTrans.localScale = Vector3.one;
        cv.sortingOrder = saveOrder;
        ArrowSlot.gameObject.SetActive(false);
        arrows.StartDraw = false;
        TurnArrowColor(0);
        //开启预览功能
        EnablePreview = true;
    }
    #endregion
    private void StartDragPreView()
    {
        cardImgTrans.localScale = new Vector3(scaleUper*1.2f, scaleUper * 1.2f, scaleUper * 1.2f);
        ArrowSlot.gameObject.SetActive(true);
        arrows.StartDraw = true;
    }
    private void StartPreView()
    {
        cardImgTrans.DOLocalMoveY(upmove, animTime);
        cardImgTrans.DOLocalRotate(new Vector3(0, 0,0), animTime);
        cardImgTrans.localScale = new Vector3(scaleUper, scaleUper, scaleUper);
        cv.sortingOrder += 100;
    }

    private void EndPreView()
    {
        cardImgTrans.DOLocalMove(savePot, 0.1f);
        cardImgTrans.DOLocalRotate(saveRot, 0.1f);
        cardImgTrans.localScale = Vector3.one;
        cv.sortingOrder = saveOrder;
    }

    //储存卡牌的初始状态
    public void SetCardSate(Vector3 position,Vector3 rotation,int idx)
    {
        savePot = position;
        saveRot = rotation;
        saveOrder = idx;
        cv.sortingOrder = idx;
    }
}
