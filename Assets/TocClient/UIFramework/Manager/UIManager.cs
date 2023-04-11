using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using LitJson;
using System.Reflection;

/// <summary>
/// UI框架的核心管理类
/// </summary>

namespace TocClient
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            LoadUIPanelInfo();
        }

        //画布，因为只有一个画布，所以可以直接用名称搜索得到
        private Transform canvasTransform;
        public void InitUiRoot(Transform transform)
        {
            canvasTransform = transform;
        }
        public Transform CanvasTransform
        {
            get
            {
                if (canvasTransform == null)
                {
                    //通过名称搜索得到画布
                    canvasTransform = GameObject.Find("Prefab_UI_UIRoot(Clone)").transform;
                }
                return canvasTransform;
            }
        }
        private Dictionary<string, UIPanelInfo> _uiPanelInfo;
        private Dictionary<string, BasePanel> panelDict;//保存所有的实例化的面板的游戏物体身上的BasePanel组件
        private Stack<BasePanel> panelStack;//用于存储显示出来的面板(的BasePanel)

        /// <summary>
        /// 将某个页面显示在屏幕上的同时入栈
        /// </summary>
        public void PushPanel(string panelName)
        {
            if (panelStack == null)
            {
                panelStack = new Stack<BasePanel>();
            }
            //加页面之前先判断一下栈里面是否已经有面板，如果有面板，原面板暂停交互
            if (panelStack.Count > 0)
            {
                BasePanel topPanel = panelStack.Peek();
                topPanel.OnPause();
            }
            //显示新面板
            BasePanel panel = GetPanel(panelName);
            panel.OnEnter();
            panelStack.Push(panel);
        }
        /// <summary>
        /// 将某个页面从屏幕移除的同时出栈
        /// </summary>
        public void PopPanel()
        {

            if (panelStack == null)
            {
                panelStack = new Stack<BasePanel>();
            }
            if (panelStack.Count <= 0)
            {
                return;
            }
            else
            {
                //关闭栈顶页面的显示
                BasePanel topPanel = panelStack.Pop();
                topPanel.OnExit();
                //弹出这个面板后如果栈空了就不用再操作，如果还有面板就继续这个栈里的面板
                if (panelStack.Count <= 0)
                {
                    return;
                }
                else
                {
                    BasePanel topPanel2 = panelStack.Peek();
                    topPanel2.OnResume();//恢复点击
                }
            }

        }

        /// <summary>
        /// 根据面板名称，得到实例化的面板
        /// </summary>
        /// <returns></returns>
        private BasePanel GetPanel(string panelName)
        {
            if (panelDict == null)
            {
                panelDict = new Dictionary<string, BasePanel>();
            }

            //从字典中读取BasePanel类型储存到panel。如果还没有实例化则panel为Null。
            BasePanel panel;
            panelDict.TryGetValue(panelName, out panel);

            //如果panel为空，那么就找这个面板的prefab的路径，然后去根据prefab去实例化面板
            if (panel == null)
            {
                _uiPanelInfo.TryGetValue(panelName, out UIPanelInfo uIPanelInfo);
                string path = uIPanelInfo.UIPanelPrefab;
                //实例化
                GameObject panelObject = AssetManager.Instance.LoadAsset<GameObject>(path);
                panelObject = Instantiate(panelObject);
                panelObject.transform.SetParent(CanvasTransform, false);
                //存入字典
                panelDict.Add(panelName, panelObject.GetComponent<BasePanel>());
                return panelObject.GetComponent<BasePanel>();
            }
            else
            {
                return panel;
            }
        }
        private void LoadUIPanelInfo()
        {
            LoadCheck check = new LoadCheck("UI信息");
            GameLaunch.AddloadCheck(check);
            AssetManager.Instance.LoadAssetAsync<TextAsset>(Constants.Cfg_UIPanel, asset =>
            {
                List<Cfg_Cs_UIPanel> result = JsonMapper.ToObject<List<Cfg_Cs_UIPanel>>(asset.text);
                int count = result.Count;
                if (_uiPanelInfo == null)
                    _uiPanelInfo = new Dictionary<string, UIPanelInfo>();
                for (int i = 0; i < count; i++)
                {
                    UIPanelInfo uIPanelInfo = new UIPanelInfo();
                    uIPanelInfo.UIPanelScripts = Typen(result[i].UiPanelScripts);
                    uIPanelInfo.UIPanelName = result[i].UiPanelName;
                    uIPanelInfo.UIPanelPrefab = result[i].UiPrefabPath;
                    _uiPanelInfo[uIPanelInfo.UIPanelName] = uIPanelInfo;
                }
                check.SetReady();
            });
        }
        public static Type Typen(string typeName)

        {
            Type type = null;
            Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();
            int assemblyArrayLength = assemblyArray.Length;
            for (int i = 0; i < assemblyArrayLength; ++i)
            {
                type = assemblyArray[i].GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }
            for (int i = 0; (i < assemblyArrayLength); ++i)
            {
                Type[] typeArray = assemblyArray[i].GetTypes();
                int typeArrayLength = typeArray.Length;
                for (int j = 0; j < typeArrayLength; ++j)
                {
                    if (typeArray[j].Name.Equals(typeName))
                    {
                        return typeArray[j];
                    }
                }
            }
            return type;
        }
    }
}

