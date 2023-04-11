using UnityEngine;
using System.Collections;
//using DG.Tweening;
/// <summary>
/// 物品信息面板
/// </summary>
public class ItemMessagePanel : BasePanel {

    private CanvasGroup canvasGroup;
    private void Start()
    {
        if (canvasGroup == null)
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

        //面板关闭时将透明度调为了0，显示面板时要调回来
        canvasGroup.alpha = 1;
        //允许点击
        canvasGroup.blocksRaycasts = true;

        //使用dotween做一个从小变到大的动画
        transform.localScale = Vector3.zero;
        //transform.DOScale(1, 0.5f);
    }
    /// <summary>
    /// 处理页面关闭
    /// </summary>
    public override void OnExit()
    {
        
        //不再可点击
        canvasGroup.blocksRaycasts = false;

        //使用Dotween做一个从大变到小的动画，之后变透明
        //transform.DOScale(0, 0.5f).OnComplete(() => canvasGroup.alpha = 0);
    }
    /// <summary>
    /// 点叉
    /// </summary>
    public void OnClosePanel()
    {
        //UIManager.Instance.PopPanel();
    }
}
