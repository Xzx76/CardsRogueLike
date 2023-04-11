using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace TocClient
{
    /// <summary>
    /// 主界面
    /// </summary>
    public class MainMenuPanel : BasePanel
    {
        //CanvasGroup组件可用于统一控制该面板与其子物体能否交互。取消Blocks Raycasts时该面板和其子物体都不能再接收点击。
        private CanvasGroup _canvasGroup;
        private Button _startBtn;
        private Button _exitBtn;

        private void Start()
        {
            _startBtn = UnityHelper.GetComponent<Button>(gameObject, "StartBtn");
            _exitBtn = UnityHelper.GetComponent<Button>(gameObject, "ExitBtn");
            _canvasGroup = GetComponent<CanvasGroup>();
            AddBtnListener();
        }
        public override void OnPause()
        {
            _canvasGroup.blocksRaycasts = false;//当弹出新的面板的时候，主菜单不再可交互
        }
        public override void OnResume()
        {
            _canvasGroup.blocksRaycasts = true;
        }

        private void AddBtnListener()
        {
            _startBtn.onClick.AddListener(()=>
            {
                Debug.Log("startGame");
                //UIManager.Instance.PushPanel(panelType);
            });
            _exitBtn.onClick.AddListener(() =>
            {
                Debug.Log("ExitGame");
                //UIManager.Instance.PushPanel(panelType);
            });
        }
        /// <summary>
        /// 将某面板显示出来
        /// </summary>
        /// <param name="panelTypeString">要显示的面板的名称</param>
        public void OnPushPanel(string panelTypeString)
        {
            //先通过字符串得到枚举类型
            UIPanelType panelType = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), panelTypeString);
            //显示该类型的面板
            //UIManager.Instance.PushPanel(panelType);
        }

    }
}

