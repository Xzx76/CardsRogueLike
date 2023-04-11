using UnityEngine;
using System.Collections;
//using DG.Tweening;
/// <summary>
/// 任务
/// </summary>
public class TaskPanel : BasePanel {
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
        //透明度与能否点击
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        //使用Dotween制作一个逐渐从透明变不透明的动画
        canvasGroup.alpha = 0;
        //canvasGroup.DOFade(1, 0.5f);
    }
    /// <summary>
    /// 处理页面关闭
    /// </summary>
    public override void OnExit()
    {


        canvasGroup.blocksRaycasts = false;
        
        //使用Dotween制作一个逐渐变透明的动画
        //canvasGroup.DOFade(0, 0.5f);
    }
    /// <summary>
    /// 点叉
    /// </summary>
    public void OnClosePanel()
    {
        //UIManager.Instance.PopPanel();
    }
}
