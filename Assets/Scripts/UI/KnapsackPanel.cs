using UnityEngine;
using System.Collections;
//using DG.Tweening;

/// <summary>
/// 背包
/// </summary>
public class KnapsackPanel : BasePanel
{
    private CanvasGroup canvasGroup;
    private void Start()
    {
        if (canvasGroup==null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }
    public override void OnEnter()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        //透明度与能否点击
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        //使用dotween做一个移动动画，从右边移动到中心
        Vector3 temp = transform.localPosition;
        temp.x = 900;
        transform.localPosition = temp;
        //transform.DOLocalMoveX(0, 0.5f);
        
    }
    public override void OnExit()
    {
        //canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        //使用Dotween做一个向右移动的动画，之后变透明
        //transform.DOLocalMoveX(1500, 0.5f).OnComplete(()=>canvasGroup.alpha = 0);
    }
    /// <summary>
    /// 暂停
    /// </summary>
    public override void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }
    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }
    /// <summary>
    /// 关闭本面板
    /// </summary>
    public void OnClosePanel()
    {
        //UIManager.Instance.PopPanel();
    }
    /// <summary>
    /// 点击物品
    /// </summary>
    public void OnItemButtonClick()
    {
        //UIManager.Instance.PushPanel(UIPanelType.ItemMessage);
    }
}
