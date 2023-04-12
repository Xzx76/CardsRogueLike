using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using TocClient;
using UnityEngine.UI;

public class PreView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static bool EnablePreview = true;

    [SerializeField]
    private Vector3 savePos;
    private Vector3 saveRot;
    [SerializeField]
    private float upmove;
    [SerializeField]
    private float scaleUper;
    [SerializeField]
    private float animTime;
    [SerializeField]
    private int saveOrder;
    private Transform cardImg;
    public Image CardImg;
    private DragWithTarget dragNoTarget;
    private BezierArrows arrows;
    private RectTransform ArrowSlot;
    private Canvas cv;
    public bool CardCreateFinish;
    public bool CanUse;
    public void Init()
    {
        cardImg = UnityHelper.GetComponent<Transform>(this.gameObject, "Image");
        CardImg = UnityHelper.GetComponent<Image>(this.gameObject, "Image");
        ArrowSlot = UnityHelper.GetComponent<RectTransform>(this.gameObject, "ArrowSlot");
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
        cardImg.DOLocalMove(Vector3.zero, 0.1f);
        cardImg.DOLocalRotate(Vector3.zero, 0.1f);
        cardImg.localScale = Vector3.one;
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
        cardImg.localScale = new Vector3(scaleUper*1.2f, scaleUper * 1.2f, scaleUper * 1.2f);
        ArrowSlot.gameObject.SetActive(true);
        arrows.StartDraw = true;
    }
    private void StartPreView()
    {
        cardImg.DOLocalMoveY(upmove, animTime);
        cardImg.DOLocalRotate(new Vector3(-transform.rotation.eulerAngles.x, transform.eulerAngles.y,-transform.rotation.eulerAngles.z), animTime);
        cardImg.localScale = new Vector3(scaleUper, scaleUper, scaleUper);
        cv.sortingOrder += 100;
    }

    private void EndPreView()
    {
        cardImg.DOLocalMove(Vector3.zero, 0.1f);
        cardImg.DOLocalRotate(Vector3.zero, 0.1f);
        cardImg.localScale = Vector3.one;
        cv.sortingOrder = saveOrder;
    }

    //储存卡牌的初始状态
    public void SetCardSate(Vector3 position,Vector3 rotation,int idx)
    {
        savePos = position;
        saveRot = rotation;
        saveOrder = idx;
        cv.sortingOrder = idx;
    }
}
