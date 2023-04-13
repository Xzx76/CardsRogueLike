using UnityEngine;
using System.Collections;
using TocClient;
using UnityEngine.UI;
/// <summary>
/// 商城
/// </summary>
public class BattleTest : BasePanel {

    private CanvasGroup canvasGroup;
    private FightManager fightManager;
    private Button beginTurn;
    private Button endTurn;
    private Button dropFirstCard;
    private Button getOneCard;
    private void Start()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        fightManager = UnityHelper.GetComponent<FightManager>(this.gameObject, "FightManager");
        beginTurn = UnityHelper.GetComponent<Button>(this.gameObject, "BeginTurn");
        endTurn = UnityHelper.GetComponent<Button>(this.gameObject, "EndTurn");
        dropFirstCard = UnityHelper.GetComponent<Button>(this.gameObject, "DropFirstCard");
        getOneCard = UnityHelper.GetComponent<Button>(this.gameObject, "GetOneCard");
        RegisterAllLisenter();
    }
    private void RegisterAllLisenter()
    {
        beginTurn.onClick.AddListener(()=>
        {
            fightManager.DeliverCard(fightManager.roundHeadCardNum, false);
        });
        endTurn.onClick.AddListener(() =>
        {
            fightManager.DropAll();
        });
        dropFirstCard.onClick.AddListener(() =>
        {
            fightManager.DropByIdx();
        });
        getOneCard.onClick.AddListener(() =>
        {
            fightManager.DeliverCard(1, true);
        });
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
