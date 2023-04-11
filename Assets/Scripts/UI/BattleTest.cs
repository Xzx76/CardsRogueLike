using UnityEngine;
using System.Collections;
/// <summary>
/// 商城
/// </summary>
public class BattleTest : BasePanel {

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
    }
    /// <summary>
    /// 处理页面关闭
    /// </summary>
    public override void OnExit()
    {
        
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
    /// <summary>
    /// 点叉
    /// </summary>
    public void OnClosePanel()
    {
        //UIManager.Instance.PopPanel();
    }
}
