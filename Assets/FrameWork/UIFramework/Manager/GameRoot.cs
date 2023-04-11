using UnityEngine;
using System.Collections;
using TocClient;

/// <summary>
/// 作为UI启动器
/// </summary>
public class GameRoot : MonoBehaviour {

	// Use this for initialization
	void Start () {

		this.gameObject.AddComponent<ObjectPoolManager>();
		//创建UI主界面
		//UIManager.Instance.PushPanel(UIPanelType.MainMenu);
	}
}
