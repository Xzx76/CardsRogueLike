using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 一个UI面板类型的预制体存储路径
/// </summary>
[Serializable] //Serializable表示可进行序列化
public class UIPanelInfo:ISerializationCallbackReceiver
{
    //ISerializationCallbackReceiver接口的作用：底部2个系统自动调用的函数来自于该接口

    //UIPanelType类型无法序列化，所以不序列化而用string类型的panelTypeString代替
    [NonSerialized]//NonSerialized表示不进行序列化
    public UIPanelType panelType;

    public string panelTypeString;
    public string path;

    /// <summary>
    /// 反序列化(从文本信息到对象)之后系统自动调用，
    /// </summary>
    public void OnAfterDeserialize()
    {
        //由于没有直接存储panelType，反序列化之后通过本函数通过panelTypeString得到panelType。
        UIPanelType type = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), panelTypeString);
        panelType = type;
    }
    /// <summary>
    /// 序列化之前系统自动调用
    /// </summary>
    public void OnBeforeSerialize()
    {
        //panelTypeString= panelType.ToString();
    }
}
