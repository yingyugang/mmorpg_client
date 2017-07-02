using UnityEngine;
using System.Collections;
using DataCenter;
using UI;

public class ArenStep2 : MonoBehaviour {

    public ArenaMain mainController;

    GameObject rankLevel, rankList, team;

	// Use this for initialization
	void Start () 
    {
        rankLevel = PanelTools.findChild(transform.parent.gameObject, "rankLevel");
        rankList = PanelTools.findChild(transform.parent.gameObject, "rankList");
        team = PanelTools.findChild(transform.parent.gameObject, "team");

        PanelTools.setBtnFunc(transform, "Title/btnBack", onBack);
		PanelTools.setBtnFunc(transform, "btnList/btnStart", onStart);
        PanelTools.setBtnFunc(transform, "Title/btnFresh", onFresh);


        PanelTools.setBtnFunc(transform, "btnList/btnRankLv", onRankLv);
        PanelTools.setBtnFunc(transform, "btnList/btnRankList", onRankList);
        PanelTools.setBtnFunc(transform, "btnList/btnTeam", onTeamEdit);
    }

    void onStart(GameObject obj)
    {
        DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).SendArenaPVPStartMsg(ati.id, ati.isRobot);
    }

    void onBack(GameObject obj)
    {
        DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).ShowStep1();
    }

    void onRankLv(GameObject obj)
    {
        DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).ShowArenaLevel(1);
    }

    void onRankList(GameObject obj)
    {
        gameObject.SetActive(false);
        rankList.SetActive(true);
        ArenaRankList arl = rankList.GetComponent<ArenaRankList>();
        arl.pageIndex = 1;

        DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).SendArenaTotalRankingMsg();
    }

    void onTeamEdit(GameObject obj)
    {
        team.SetActive(true);

        ArenaTeam at = team.GetComponent<ArenaTeam>();
        at.InitShowTeam();
        at.pageIndex = 1;
    }

    void OnEnable()
    {
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, true, (int)EVENT_GROUP.mainUI);
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, false, (int)EVENT_GROUP.mainUI);
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, true, (int)EVENT_GROUP.mainUI);
    }

    void onFresh(GameObject obj)
    {
        ShowTarget();
    }

    public void ShowInfo()
    {
        ShowTarget();
        ShowSelf();
    }

    ArenaTargetInfo ati = null;
    public void ShowTarget()
    {
        ati = DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).GetTargetInfo();

        UISprite heroIcon = PanelTools.findChild<UISprite>(gameObject, "Info/enemy/heroIcon");
        heroIcon.spriteName = ati.leaderHero.portarait;

        UILabel levelLabel = PanelTools.findChild<UILabel>(gameObject, "Info/enemy/lv");
        levelLabel.text = "LV." + ati.level.ToString();

        UILabel nameLabel = PanelTools.findChild<UILabel>(gameObject, "Info/enemy/name");
        //nameLabel.text = ati.name;
        nameLabel.text = ati.id.ToString();

        UILabel pointLabel = PanelTools.findChild<UILabel>(gameObject, "Info/enemy/pt");
        pointLabel.text = ati.arenaName;

        UILabel resultLabel = PanelTools.findChild<UILabel>(gameObject, "Info/enemy/result");
        resultLabel.text = ati.totalWin.ToString() + "胜" + ati.totalLose.ToString() + "负";
    }

    public void ShowSelf()
    {
        int idTeam = DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).nPvpTeamId;
        HeroInfo hi = DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).dicArenaTeams[idTeam].GetLeaderHero();
        UISprite heroIcon = PanelTools.findChild<UISprite>(gameObject, "Info/enemy/heroIcon");
        heroIcon.spriteName = hi.portarait;


        UILabel levelLabel = PanelTools.findChild<UILabel>(gameObject, "Info/me/lv");
        levelLabel.text = "LV." + DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.level.ToString();

        UILabel nameLabel = PanelTools.findChild<UILabel>(gameObject, "Info/me/name");
        //nameLabel.text = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.name;
        nameLabel.text = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.id.ToString();

        UILabel pointLabel = PanelTools.findChild<UILabel>(gameObject, "Info/me/pt");
        pointLabel.text = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.arenaHonor;

        UILabel resultLabel = PanelTools.findChild<UILabel>(gameObject, "Info/me/result");
        resultLabel.text = DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).totalWin.ToString() + "胜" 
            + DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).totalLose.ToString() + "负";
    }
}
