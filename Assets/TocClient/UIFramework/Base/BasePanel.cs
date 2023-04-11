using UnityEngine;
using System.Collections;


/// <summary>
/// UI面板脚本的公共基类
/// </summary>
public class BasePanel : MonoBehaviour
{
    public string PanelName { get; set; }

    /// <summary>
    /// 界面显示出来
    /// </summary>
	public virtual void OnEnter()
    {

    }
    /// <summary>
    /// 界面暂停
    /// </summary>
    public virtual  void OnPause()
    {

    }
    /// <summary>
    /// 界面继续
    /// </summary>
    public virtual void OnResume()
    {

    }
    /// <summary>
    /// 界面关闭
    /// </summary>
    public virtual void OnExit()
    {
        
    }


}
