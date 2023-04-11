using UnityEngine;
using System.Collections;
using TocClient;
/// <summary>
/// 主界面
/// </summary>
public class MainMenuPanel :BasePanel {
    //CanvasGroup组件可用于统一控制该面板与其子物体能否交互。取消Blocks Raycasts时该面板和其子物体都不能再接收点击。
    private CanvasGroup canvasGroup;
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        //MsgSystem.AddListener(Constants.Form_MainMenu, Test_NoParam);
    }
    public override void OnPause()
    {
        canvasGroup.blocksRaycasts = false;//当弹出新的面板的时候，主菜单不再可交互
    }
    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// 将某面板显示出来
    /// </summary>
    /// <param name="panelTypeString">要显示的面板的名称</param>
    public void OnPushPanel(string panelTypeString  )
    {
        //先通过字符串得到枚举类型
        UIPanelType panelType = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), panelTypeString);
        //显示该类型的面板
        UIManager.Instance.PushPanel(panelType);
    }

}
