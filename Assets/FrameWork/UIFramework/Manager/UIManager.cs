using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//本类作用：
//1,从json文件中解析面板信息，保存到panelPathDict
//2,保存实例化的面板(在panelDict)
//2,管理面板的显示与隐藏

/// <summary>
/// UI框架的核心管理类
/// </summary>
public class UIManager{

    //单例模式的核心：
    //1，定义一个静态的对象，在外部访问，在内部构造
    //2, 构造方法私有化

    //将UIManager制作成单例
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance==null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }

    //画布，因为只有一个画布，所以可以直接用名称搜索得到
    private Transform canvasTransform;
    public Transform CanvasTransform
    {
        get
        {
            if (canvasTransform==null)
            {
                //通过名称搜索得到画布
                canvasTransform = GameObject.Find("Canvas").transform;
            }
            return canvasTransform;
        }
    }

    private Dictionary<UIPanelType, string> panelPathDict;//存储所有面板的预制体的路径，存储的是路径不是预制体
    private Dictionary<UIPanelType, BasePanel> panelDict;//保存所有的实例化的面板的游戏物体身上的BasePanel组件
    private Stack<BasePanel> panelStack;//用于存储显示出来的面板(的BasePanel)

    private UIManager()
    {
        ParseUIPanelTypeJson();
    }

    /// <summary>
    /// 将某个页面显示在屏幕上的同时入栈
    /// </summary>
    public void PushPanel(UIPanelType panelType)
    {
        if (panelStack==null)
        {
            panelStack = new Stack<BasePanel>();
        }
        //加页面之前先判断一下栈里面是否已经有面板，如果有面板，原面板暂停交互
        if (panelStack.Count>0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }
        //显示新面板
        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        panelStack.Push(panel);
    }
    /// <summary>
    /// 将某个页面从屏幕移除的同时出栈
    /// </summary>
    public void PopPanel()
    {
        
        if (panelStack==null)
        {
            panelStack = new Stack<BasePanel>();
        }
        if (panelStack.Count<=0)
        {
            return;
        }
        else
        {
            //关闭栈顶页面的显示
            BasePanel topPanel = panelStack.Pop();
            topPanel.OnExit();
            //弹出这个面板后如果栈空了就不用再操作，如果还有面板就继续这个栈里的面板
            if (panelStack.Count<=0)
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
    /// 根据面板类型，得到实例化的面板
    /// </summary>
    /// <returns></returns>
    private BasePanel GetPanel(UIPanelType panelType)
    {
        if (panelDict==null)
        {
            panelDict = new Dictionary<UIPanelType, BasePanel>();
        }

        //从字典中读取BasePanel类型储存到panel。如果还没有实例化则panel为Null。
        BasePanel panel;
        panelDict.TryGetValue(panelType, out panel);

        //如果panel为空，那么就找这个面板的prefab的路径，然后去根据prefab去实例化面板
        if (panel == null)
        {
            
            //先得到路径存入path
            string path;
            panelPathDict.TryGetValue(panelType,out path);

            //实例化
            GameObject instansPanel= GameObject.Instantiate( Resources.Load(path) ) as GameObject;
            instansPanel.transform.SetParent(CanvasTransform,false);
            //存入字典
            panelDict.Add(panelType, instansPanel.GetComponent<BasePanel>());

            return instansPanel.GetComponent<BasePanel>();
        }
        else
        {
            return panel;
        }
    }
    /// <summary>
    /// 存储着一个UIPanelInfo类型的链表
    /// </summary>
    [Serializable]
    class UIPanelTypeJson
    {
        public List<UIPanelInfo> infoList;
    }

    /// <summary>
    /// 解析Json文件，将ui面板类型与对应预制体的存储路径存入字典
    /// </summary>
    private void ParseUIPanelTypeJson()
    {
        //该字典变量存储ui面板类型与对应预制体的存储路径
        panelPathDict = new Dictionary<UIPanelType, string>();
        //加载Resources文件夹内名为UIPanelType的Json文件的内容到ta
        TextAsset ta = Resources.Load<TextAsset>("UIPanelTypeAndPath");
        //JsonUtility封装了json的部分基本功能。
        //将读取的Json的内容转换为UIPanelTypeJson类型，UIPanelTypeJson存储着一个UIPanelInfo类型的链表
        //也就是把json存储的内容转换为了可直接使用的类型
        UIPanelTypeJson jsonObject = JsonUtility.FromJson<UIPanelTypeJson>(ta.text);
        //通过遍历将ui面板类型与对应预制体的存储路径存入panelPathDict
        foreach (UIPanelInfo info in jsonObject.infoList)
        {
            panelPathDict.Add(info.panelType, info.path);
        }
    }


}
