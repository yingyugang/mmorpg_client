using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class PartnerEdit : MonoBehaviour
{

    public UIButton btnReturn;
    public UILabel titleLabel;
    public GameObject adjustBtn;
    public GameObject leaderBtn;
    public GameObject rightBtn;
    public GameObject leftBtn;
    public GameObject grid;
    public GameObject pvpBtn;

    public GameObject menu;
    public GameObject selectTeamer;
    public GameObject changeTeamer;
    public GameObject Teams;


    public int nSelectedGroundId = -1;
    public List<GameObject> groundsList = new List<GameObject>();
    public List<GameObject> ItemsList = new List<GameObject>();
    public List<GameObject> TeamsList = new List<GameObject>();

    public int nDragGround = -1;
    public int nCurTeamId = 0;

    public bool isFightShow = false;
    public bool isSengMsg = true;

    public int nState = 0;      //0 选择队友， 1 选择队长 ， 2交换位置
    public int nTeamCount = 0;

    GameObject block = null;
    
    // Use this for initialization
	void Start ()
    {
        if (btnReturn != null)
            UIEventListener.Get(btnReturn.gameObject).onClick = onReturn;
        if (leaderBtn != null)
        {
            UIEventListener.Get(leaderBtn).onClick = onLeader;
        }

        if (adjustBtn != null)
        {
            UIEventListener.Get(adjustBtn).onClick = onAdjust;
        }

        if (rightBtn != null)
        {
            UIEventListener.Get(rightBtn.gameObject).onClick = onRight;
        }

        if (leftBtn != null)
        {
            UIEventListener.Get(leftBtn.gameObject).onClick = onLeft;
        }

        if (pvpBtn != null)
        {
            UIEventListener.Get(pvpBtn).onClick = onPVP;
        }

        nCurTeamId = (int)DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).nCurTeamId;
        AlineOnChild aoc = grid.GetComponent<AlineOnChild>();
        aoc.CenterOn(TeamsList[nCurTeamId].transform);
        SetCurTeam();
    }

    Vector3 teamsPostion = new Vector3();
    
    void LateUpdate () 
    {
        if (teamsPostion != Teams.transform.position)
        {
            teamsPostion = Teams.transform.position;

            SetCurTeamID();
        }

//         foreach (KeyValuePair<int, GameObject> go in dicHeroModel)
//         {
//             HeroInfo.ChangeModel(go.Value, 0.1f);
//         }

        foreach (GameObject go in heroModelList)
        {
            HeroInfo.ChangeModel(go, 0.1f);
        }
	}


    void OnEnable()
    {
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, false, (int)EVENT_GROUP.mainUI);
        InitUI(); 
    }
    
    void OnDisable()
    {
        isFightShow = false;

        if (isSengMsg)
        {
            DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).SendHeroFormationMsg();
        }

        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, true, (int)EVENT_GROUP.mainUI);
    }

    void onReturn(GameObject go)
    {
        isSengMsg = true;
        
        if (nState != 0)
        {
            nState = 0;
            ChangeState();
            return;
        }
        
        if (isFightShow)
        {
            transform.parent.gameObject.SetActive(false);
			UI.PanelStack.me.goBack();
        }
		else
		{
        	menu.SetActive(true);
            gameObject.SetActive(false);
		}

    }

    void onLeader(GameObject go)
    {
        if (nState == 1)
        {
            nState = 0;
        }
        else
        {
            nState = 1;
        }
        
        ChangeState();
    }

    void onAdjust(GameObject go)
    {
        if (nState == 2)
        {
            nState = 0;
        }
        else
        {
            nState = 2;
        }

        ChangeState();
    }

    protected void ChangeState()
    {

        titleLabel.text = "编组队伍";

        if (nState == 1)
        {
            block.SetActive(true);
            rightBtn.SetActive(false);
            leftBtn.SetActive(false);

            titleLabel.text = "变更队长";

            foreach (GameObject item in ItemsList)
            {
                GroundDragItem gdi = item.GetComponent<GroundDragItem>();
                gdi.enabled = true;

                UIDragScrollView dsv = item.GetComponent<UIDragScrollView>();
                dsv.enabled = false;


            }

            foreach (GameObject ground in groundsList)
            {
                UIDragScrollView dsv = ground.GetComponent<UIDragScrollView>();
                dsv.enabled = false;
            }
        }
        else if (nState == 2)
        {
            block.SetActive(true);
            rightBtn.SetActive(false);
            leftBtn.SetActive(false);

            titleLabel.text = "变更位置";

            foreach (GameObject item in ItemsList)
            {
                GroundDragItem gdi = item.GetComponent<GroundDragItem>();
                gdi.enabled = true;

                UIDragScrollView dsv = item.GetComponent<UIDragScrollView>();
                dsv.enabled = false;

            }

            foreach (GameObject ground in groundsList)
            {
                UIDragScrollView dsv = ground.GetComponent<UIDragScrollView>();
                dsv.enabled = false;
            }
        }
        else
        {
            block.SetActive(false);
            rightBtn.SetActive(true);
            leftBtn.SetActive(true);

            foreach (GameObject item in ItemsList)
            {
                GroundDragItem gdi = item.GetComponent<GroundDragItem>();
                gdi.enabled = false;

                UIDragScrollView dsv = item.GetComponent<UIDragScrollView>();
                dsv.enabled = true;
            }

            foreach (GameObject ground in groundsList)
            {
                UIDragScrollView dsv = ground.GetComponent<UIDragScrollView>();
                dsv.enabled = true;
            }
        }


        isGroundTurn();
    }

    void onRight(GameObject go)
    {
        AlineOnChild aoc = grid.GetComponent<AlineOnChild>();

        if (nCurTeamId < nTeamCount - 1)
        {
            aoc.CenterOn(TeamsList[++nCurTeamId].transform);
            DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).nCurTeamId = nCurTeamId;
        }

        SetCurTeam();
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).nCurTeamId = nCurTeamId;
    }

    void onLeft(GameObject go)
    {
        AlineOnChild aoc = grid.GetComponent<AlineOnChild>();

        if (nCurTeamId > 0)
        {
            aoc.CenterOn(TeamsList[--nCurTeamId].transform);
            DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).nCurTeamId = nCurTeamId;
        }

        SetCurTeam();
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).nCurTeamId = nCurTeamId;
    }

    void onPVP(GameObject go)
    {
        transform.parent.gameObject.SetActive(false);
        DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).ShowArenaTeam();
        gameObject.SetActive(false);
    }

    void onGround(GameObject go)
    {
        GameObject item = go.transform.FindChild("Item").gameObject;

        GameObject friengSprite = PanelTools.findChild(item, "friengSprite");
        if (item.activeSelf && friengSprite.activeSelf)
        {
            return;
        }

        
        if(nState != 0)
        {
            if (item != null)
            {
                onItem(item);
            }
            return;
        }
        
        DragDropGround ddg = go.GetComponent<DragDropGround>();

        if (ddg != null)
        {
            if (5 < curTeamInfo.GetHeroCount())
            {
                return;
            }

            isSengMsg = false;
            selectTeamer.SetActive(true);

            SelectTeamer st = selectTeamer.GetComponent<SelectTeamer>();
            st.idGround = ddg.idGround;
            nSelectedGroundId = ddg.idGround;
            st.ShowHeroBrowse();
            gameObject.SetActive(false);
        }
        else
        {
            nSelectedGroundId = -1;
        }

        //isGroundTurn();
    }

    void onItem(GameObject go)
    {
        
        UISprite leader = PanelTools.findChild<UISprite>(go, "leader");
        UILabel idLabel = PanelTools.findChild<UILabel>(go, "idLabel");
        int id = int.Parse(idLabel.text);

        if (id == 0)
        {
            return;
        }

        if (leader.gameObject.activeSelf && nState == 0)
        {
            onGround(go.transform.parent.gameObject);
            return;
        }
        
        if (nState == 1)
        {
            DragDropGround ddg = go.transform.parent.GetComponent<DragDropGround>();
            DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams[nCurTeamId + 1].leaderPos = ddg.idGround + 1;

            ShowTeamHero();
        }
        else if (nState == 2)
        {
        }
        else
        {
            isSengMsg = false;
            changeTeamer.SetActive(true);

            DragDropGround ddg = go.transform.parent.GetComponent<DragDropGround>();
            ChangeTeamer ct = changeTeamer.GetComponent<ChangeTeamer>();
            ct.idHero = id;
            ct.idGround = ddg.idGround;
            ct.ShowHeroBrowse();
            gameObject.SetActive(false);
        }
    }

    public void InitShowTeam()
    {
        nTeamCount = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams.Count;

        for (nCurTeamId = 0; nCurTeamId < nTeamCount; ++nCurTeamId)
        {
            ShowTeamHero();
        }

        nCurTeamId = (int)DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).nCurTeamId;
        AlineOnChild aoc = grid.GetComponent<AlineOnChild>();
        aoc.CenterOn(TeamsList[nCurTeamId].transform);
        SetCurTeam();

        for (int i = 0; i < 10; ++i)
        {
            if (i < nTeamCount)
            {
                TeamsList[i].SetActive(true);
            }
            else
            {
                TeamsList[i].SetActive(false);
            }
        }
    }

    public void ShowAddTeam()
    {
        nTeamCount = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams.Count;

        TeamsList[nTeamCount-1].SetActive(true);

        grid.GetComponent<UIGrid>().repositionNow = true;
    }

    public void InitUI()
    {
        isSengMsg = true;
        nState = 0;
        //dicHeroModel.Clear();
    }
    
    public void ClearUI()
    {
        SetCurTeam();
        
        foreach(GameObject go in ItemsList)
        {
            GameObject leader = PanelTools.findChild(go, "leader");
            leader.SetActive(false);

            UILabel idLable = PanelTools.findChild<UILabel>(go, "idLabel");
            idLable.text = "0";

            go.SetActive(false);
        }
    }

    TeamInfo curTeamInfo = null;
    int nleader = 0;
    int nMaxleader = 0;
    //Dictionary<int, GameObject> dicHeroModel = new Dictionary<int, GameObject>();
    List<GameObject> heroModelList = new List<GameObject>();
    public void ShowTeamHero()
    {
        ClearUI();

        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams.TryGetValue(nCurTeamId + 1, out curTeamInfo);

        if (curTeamInfo.leaderPos != 0)
        {
            GameObject leader = PanelTools.findChild(ItemsList[curTeamInfo.leaderPos - 1], "leader");
            leader.SetActive(true);
        }

        if (curTeamInfo != null)
        {
            for (int i = 0; i < ItemsList.Count; ++i)
            {
                HeroInfo hi = null;

                switch (i)
                {
                    case 0:
                        if (curTeamInfo.pos1HeroId != 0)
                            hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(curTeamInfo.pos1HeroId);
                        break;
                    case 1:
                        if (curTeamInfo.pos2HeroId != 0)
                            hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(curTeamInfo.pos2HeroId);
                        break;
                    case 2:
                        if (curTeamInfo.pos3HeroId != 0)
                            hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(curTeamInfo.pos3HeroId);
                        break;
                    case 3:
                        if (curTeamInfo.pos4HeroId != 0)
                            hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(curTeamInfo.pos4HeroId);
                        break;
                    case 4:
                        if (curTeamInfo.pos5HeroId != 0)
                            hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(curTeamInfo.pos5HeroId);
                        break;
                    case 5:
                        if (curTeamInfo.pos6HeroId != 0)
                            hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(curTeamInfo.pos6HeroId);
                        break;
                }

                if (hi == null)
                {
                    continue;
                }

                GameObject item = ItemsList[i];

                UILabel idLabel = PanelTools.findChild<UILabel>(item, "idLabel");
                idLabel.text = hi.id.ToString();
                idLabel.gameObject.SetActive(false);

                //模型
                Transform modelOld = item.transform.FindChild("model1");
                if (modelOld != null)
                {
                    heroModelList.Remove(modelOld.gameObject);
                    Destroy(modelOld.gameObject);
                }

                if (DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroPrefabs.ContainsKey(hi.fbxFile))
                {
                    GameObject modelPrefab = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroPrefabs[hi.fbxFile];
                    GameObject model1 = NGUITools.AddChild(item.gameObject, modelPrefab);

                    model1.transform.localScale = new UnityEngine.Vector3(30, 30, 1);
                    model1.transform.localPosition = new UnityEngine.Vector3(0, -70, 0);
                    model1.name = "model1";

                    //dicHeroModel[hi.id] = model1;
                    heroModelList.Add(model1);
                }

                UILabel levelLabel = PanelTools.findChild<UILabel>(item, "info/level");
                levelLabel.text = hi.level.ToString();

                UILabel hpLabel = PanelTools.findChild<UILabel>(item, "info/hp");
                hpLabel.text = hi.hp.ToString();

                UILabel costLabel = PanelTools.findChild<UILabel>(item, "info/cost");
                costLabel.text = hi.leader.ToString();

                UILabel atkLabel = PanelTools.findChild<UILabel>(item, "info/atk");
                atkLabel.text = hi.atk.ToString();

                UILabel defLabel = PanelTools.findChild<UILabel>(item, "info/def");
                defLabel.text = hi.def.ToString();

                UILabel reLabel = PanelTools.findChild<UILabel>(item, "info/re");
                reLabel.text = hi.recover.ToString();

                UISprite typeSprite = PanelTools.findChild<UISprite>(item, "info/type");
                typeSprite.spriteName = "SERIES" + hi.series.ToString();

                UISprite starSprite = PanelTools.findChild<UISprite>(item, "info/star");
                starSprite.spriteName = "star" + hi.star.ToString();

                UILabel typeID = PanelTools.findChild<UILabel>(item, "info/typeID");
                typeID.text = hi.type.ToString();

                item.gameObject.SetActive(true);

                //隐藏好友图标
                UISprite fSprite = PanelTools.findChild<UISprite>(item, "friengSprite");
                fSprite.gameObject.SetActive(false);
            }
        }
        
        //好友位置处理
        GameObject friendItem = ItemsList[curTeamInfo.friendPos - 1];

        UISprite friengSprite = PanelTools.findChild<UISprite>(friendItem, "friengSprite");
        friengSprite.gameObject.SetActive(true);

        friendItem.SetActive(true);

        foreach (Transform child in friendItem.transform)
        {
            if (child.name != "friengSprite")
            {
                child.gameObject.SetActive(false);
            }
        }
        
    }

    public void ShowLeaderSkill()
    {
        UILabel skillName = PanelTools.findChild<UILabel>(gameObject, "skillInfo/skillName");
        UILabel skillEffect = PanelTools.findChild<UILabel>(gameObject, "skillInfo/skillEffect");

        TeamInfo ti = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams[nCurTeamId + 1];
        HeroInfo hi = ti.GetLeaderHero();

        if (hi != null)
        {
            skillName.text = hi.skillCaptianName;
            skillEffect.text = hi.skillCaptianDesc;
        }

    }

    public void ShowLeader()
    {
        UILabel leadLabel = PanelTools.findChild<UILabel>(gameObject, "Titlebg/leadLabel");
        
        nleader = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetTeamLeader(nCurTeamId + 1);
        nMaxleader = (int)DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.maxLeader;
        leadLabel.text = nleader.ToString() + " / " + nMaxleader.ToString();

        if (nleader > nMaxleader)
        {
            leadLabel.color = Color.red;
        }
    }

    public void ShowInfo()
    {
        SetCurTeam();
        
        foreach (GameObject go in ItemsList)
        {
            foreach (Transform child in go.transform)
            {
                if (child.name != "leader"
                    && child.gameObject.name != "friengSprite")
                {
                    child.gameObject.SetActive(true);
                }
            }
        }

        ShowTeamHero();
    }

    public void HideInfo()
    {
        SetCurTeam();
        
        foreach (GameObject go in ItemsList)
        {
            UILabel idLabel = PanelTools.findChild<UILabel>(go, "idLabel");
            
            foreach (Transform child in go.transform)
            {
                if (int.Parse(idLabel.text) == 0)
                {
                    continue;
                }

                if (int.Parse(idLabel.text) != 0 && child.gameObject.name == "model1")
                {
                    child.gameObject.SetActive(true);
                    continue;
                }

                child.gameObject.SetActive(false);
            }
        }
    }

    public void isGroundTurn()
    {
        if (nState != 0)
        {
            foreach (GameObject team in TeamsList)
            {
                BoxCollider bc = team.GetComponent<BoxCollider>();

                bc.enabled = false;
            }
        }
        else
        {
            foreach (GameObject team in TeamsList)
            {
                BoxCollider bc = team.GetComponent<BoxCollider>();

                bc.enabled = true;
            }
        }
    }

    public void SetCurTeam()
    {
        TeamGround tg = TeamsList[nCurTeamId].GetComponent<TeamGround>();

        if (tg != null)
        {
            block = tg.block;
            ItemsList = tg.ItemsList;
            groundsList = tg.GroundList;

            foreach(GameObject item in ItemsList)
            {
                UIEventListener.Get(item).onClick = onItem;
            }

            foreach (GameObject ground in groundsList)
            {
                UIEventListener.Get(ground).onClick = onGround;
            }
        }

        ShowLeader();
        ShowLeaderSkill();

        UILabel teamLabel = PanelTools.findChild<UILabel>(gameObject, "Titlebg/teamLabel");

        teamLabel.text = (nCurTeamId + 1).ToString() + "/" + nTeamCount.ToString();
    }

    public void SetCurTeamID()
    {
        SpringPanel sp = Teams.GetComponent<SpringPanel>();

        int teamid = nCurTeamId;

        if (sp != null)
        {
            if (sp.target.x > -640)
            {
                nCurTeamId = 0;
            }
            else if (sp.target.x > -1280)
            {
                nCurTeamId = 1;
            }
            else if (sp.target.x > -1920)
            {
                nCurTeamId = 2;
            }
            else if (sp.target.x > -2560)
            {
                nCurTeamId = 3;
            }
            else if (sp.target.x > -3200)
            {
                nCurTeamId = 4;
            }
            else if (sp.target.x > -3840)
            {
                nCurTeamId = 5;
            }
            else if (sp.target.x > -4480)
            {
                nCurTeamId = 6;
            }
            else if (sp.target.x > -5120)
            {
                nCurTeamId = 7;
            }
            else if (sp.target.x > -5760)
            {
                nCurTeamId = 8;
            }
            else
            {
                nCurTeamId = 9;
            }

            //Debug.Log("nCurTeamId = " + nCurTeamId.ToString());
        }

        if (teamid != nCurTeamId)
        {
            SetCurTeam();
            DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).nCurTeamId = nCurTeamId;
        }
    }
}
