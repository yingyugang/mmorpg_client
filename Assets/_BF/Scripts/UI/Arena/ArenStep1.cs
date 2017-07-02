using UnityEngine;
using System.Collections;
using UI;
using DataCenter;

public class ArenStep1 : MonoBehaviour 
{
    GameObject step2, rankLevel, rankList, arenaResult, team;
	// Use this for initialization
	void Start () 
    {
        step2 = PanelTools.findChild(transform.parent.gameObject, "step2");
        team = PanelTools.findChild(transform.parent.gameObject, "team");
        rankLevel = PanelTools.findChild(transform.parent.gameObject, "rankLevel");
        rankList = PanelTools.findChild(transform.parent.gameObject, "rankList");
        arenaResult = PanelTools.findChild(transform.parent.gameObject, "step2");

        PanelTools.setBtnFunc(transform, "btnList/btnStart", onStart);
        PanelTools.setBtnFunc(transform, "btnList/btnRankLv", onShowRankLevel);
        PanelTools.setBtnFunc(transform, "btnList/btnTeam", onTeamEdit);
        PanelTools.setBtnFunc(transform, "btnList/btnRankList", onShowRankList);
        PanelTools.setBtnFunc(transform, "btnList/btnResult", onShowResult);
        Init();
    }

    void Init()
    {
        if (step2 != null)
            step2.SetActive(false);
        if (rankLevel != null)
            rankLevel.SetActive(false);
        if (rankList != null)
            rankList.SetActive(false);
        if (arenaResult != null)
            arenaResult.SetActive(false);

        ShowInfo();
    }

    void onStart(GameObject go)
    {
        DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).ShowStep2();

        ArenStep2 as2 = step2.GetComponent<ArenStep2>();
        as2.ShowInfo();
    }

    void OnEnable()
    {
        Init();
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, false, (int)EVENT_GROUP.mainUI);
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, false, (int)EVENT_GROUP.mainUI);
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, true, (int)EVENT_GROUP.mainUI);

        
    }

    //显示段位信息
    void onShowRankLevel(GameObject go)
    {
        DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).ShowArenaLevel(0);
    }

    //编组
    void onTeamEdit(GameObject go)
    {
        team.SetActive(true);

        ArenaTeam at = team.GetComponent<ArenaTeam>();
        at.InitShowTeam();
        at.pageIndex = 0;
    }

    //排行榜
    void onShowRankList(GameObject go)
    {
        gameObject.SetActive(false);
        rankList.SetActive(true);
        ArenaRankList arl = rankList.GetComponent<ArenaRankList>();
        arl.pageIndex = 0;

        DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).SendArenaTotalRankingMsg();
    }

    //战绩
    void onShowResult(GameObject go)
    {
        if (arenaResult == null)
            return;
        UI.PanelStack.me.goNext(arenaResult);
    }

    public void ShowInfo()
    {
        uint point = DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).arenaPoint;
        uint win = DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).totalWin;
        uint lose = DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).totalLose;
        int nextPoint = DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).GetNextLevelPoint(point);
        
        UILabel levelLabel = PanelTools.findChild<UILabel>(gameObject, "userInfo/Level");
        levelLabel.text = UserInfo.getArenaHonor(point);

        UILabel resultLabel = PanelTools.findChild<UILabel>(gameObject, "userInfo/result");
        resultLabel.text = win.ToString() + "胜" + lose.ToString() + "负";

        UISlider pb = PanelTools.findChild<UISlider>(gameObject, "userInfo/Progress Bar");
        pb.value = (float)point / (float)nextPoint;

        UILabel pointLabel = PanelTools.findChild<UILabel>(gameObject, "userInfo/Progress Bar/point");
        pointLabel.text = point.ToString() + "/" + nextPoint.ToString();

        UISprite reward = PanelTools.findChild<UISprite>(gameObject, "userInfo/reward");
        reward.spriteName = DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).GetNextAward(point);
    }
}
