using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class ChanllengeTeam : MonoBehaviour
{

    public UISprite spLeader;
    public UISprite spAdjust;
    public UILabel titleLabel;
    public GameObject rightBtn;
    public UIButton leftBtn;
    public GameObject grid;

    public GameObject btnReturn;
    public GameObject selectTeamer;
    public GameObject changeTeamer;
    public GameObject Teams;

    public int nSelectedGroundId = -1;
    public List<GameObject> groundsList = new List<GameObject>();
    public List<GameObject> ItemsList = new List<GameObject>();
    public List<GameObject> TeamsList = new List<GameObject>();

    public int nDragGround = -1;
    public int nCurTeamId = 0;

    public UIAtlas atlas;

    public int nState = 0;      //0 选择队友， 1 选择队长 ， 2交换位置
    public int nTeamCount = 0;

    // Use this for initialization
	void Start ()
    {
        btnReturn = PanelTools.findChild(gameObject, "Titlebg/ReturnButton");
        if (btnReturn != null)
            UIEventListener.Get(btnReturn.gameObject).onClick = onReturn;

        if (spLeader != null)
        {
            UIEventListener.Get(spLeader.gameObject).onClick = onLeader;
        }

        if (spAdjust != null)
        {
            UIEventListener.Get(spAdjust.gameObject).onClick = onAdjust;
        }

        if (rightBtn != null)
        {
            UIEventListener.Get(rightBtn.gameObject).onClick = onRight;
        }

        if (leftBtn != null)
        {
            UIEventListener.Get(leftBtn.gameObject).onClick = onLeft;
        }


        AlineOnChild aoc = grid.GetComponent<AlineOnChild>();
        if (aoc != null)
        {
            aoc.onFinished = SetCurTeamID;
        }

    }
	
	// Update is called once per frame
	void Update () 
    {
	}

    void OnEnable() { InitUI(); }
    
    void OnDisable()
    {
    }

    void onReturn(GameObject go)
    {

        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).SendHeroFormationTrialMsg();
        
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowOrgTeam();

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
        spLeader.spriteName = "sub_square1_btn1";
        spAdjust.spriteName = "sub_square1_btn1";
        titleLabel.text = "编组队伍";


        if (nState == 1)
        {
            spLeader.spriteName = "sub_square1_btn2";
            titleLabel.text = "变更队长";
        }
        
        if (nState == 2)
        {
            spAdjust.spriteName = "sub_square1_btn2";
            titleLabel.text = "变更配置";

            foreach (GameObject item in ItemsList)
            {
                ChallengerDragItem cdi = item.GetComponent<ChallengerDragItem>();
                cdi.enabled = true;

                UIDragScrollView dsv = item.GetComponent<UIDragScrollView>();
                dsv.enabled = false;
            }
        }
        else
        {
            foreach (GameObject item in ItemsList)
            {
                ChallengerDragItem cdi = item.GetComponent<ChallengerDragItem>();
                cdi.enabled = false;

                UIDragScrollView dsv = item.GetComponent<UIDragScrollView>();
                dsv.enabled = true;
            }
        }


        isGroundTurn();
    }

    void onRight(GameObject go)
    {
        AlineOnChild aoc = grid.GetComponent<AlineOnChild>();

        if (nCurTeamId < 3)
        {
            aoc.CenterOn(TeamsList[++nCurTeamId].transform);

        }
    }

    void onLeft(GameObject go)
    {
        AlineOnChild aoc = grid.GetComponent<AlineOnChild>();

        if (nCurTeamId > 0)
        {
            aoc.CenterOn(TeamsList[--nCurTeamId].transform);

        }
    }

    void onGround(GameObject go)
    {
        DragDropGround ddg = go.GetComponent<DragDropGround>();

        if (ddg != null)
        {
            if (4 < curTeamInfo.GetHeroCount())
            {
                return;
            }
            
            gameObject.SetActive(false);
            selectTeamer.SetActive(true);

            SelectChanllenger sc = selectTeamer.GetComponent<SelectChanllenger>();
            sc.idGround = ddg.idGround;
            nSelectedGroundId = ddg.idGround;
            sc.ShowHeroBrowse();
        }
        else
        {
            nSelectedGroundId = -1;
        }

        isGroundTurn();
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

        if (leader.gameObject.activeSelf)
        {
            onGround(go.transform.parent.gameObject);
            return;
        }
        
        if (nState == 1)
        {
            DragDropGround ddg = go.transform.parent.GetComponent<DragDropGround>();
            DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).dicHeroTeams[nCurTeamId + 1].leaderPos = ddg.idGround + 1;

            ShowTeamHero();
        }
        else if (nState == 2)
        {
        }
        else
        {
            gameObject.SetActive(false);
            changeTeamer.SetActive(true);

            DragDropGround ddg = go.transform.parent.GetComponent<DragDropGround>();
            ChangeChanllenger cc = changeTeamer.GetComponent<ChangeChanllenger>();
            cc.idHero = id;
            cc.idGround = ddg.idGround;
            cc.ShowHeroBrowse();
        }
    }

    public void InitShowTeam(int nTeam)
    {

        int nCount = DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).dicHeroTeams.Count;

        for (nCurTeamId = 0; nCurTeamId < nCount; ++nCurTeamId)
        {
            ShowTeamHero();
        }

        if (nTeam > 2 || nTeam < 0)
        {
            return;
        }

        nCurTeamId = nTeam;

        AlineOnChild aoc = grid.GetComponent<AlineOnChild>();
        aoc.CenterOn(TeamsList[nTeam].transform);
        SetCurTeam();

    }

    public void InitUI()
    {
        nState = 0;
        ChangeState();
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
    public void ShowTeamHero()
    {
        ClearUI();

        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).dicHeroTeams.TryGetValue(nCurTeamId + 1, out curTeamInfo);

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
                

                //模型
                Transform icon = item.transform.FindChild("icon");
                Transform modelOld = icon.FindChild("model");
                if (modelOld != null)
                {
                    Destroy(modelOld.gameObject);
                }

                GameObject modelPrefab = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroPrefabs[hi.fbxFile];
                //GameObject modelPrefab = ResourceCenter.LoadAsset<GameObject>(panelPrebPath);
                GameObject model = NGUITools.AddChild(icon.gameObject, modelPrefab);

                model.transform.localScale = new UnityEngine.Vector3(30,30,1);
                model.transform.localPosition = new UnityEngine.Vector3(0,-70,0);
                model.name = "model";

                UILabel levelLabel = PanelTools.findChild<UILabel>(item, "level");
                levelLabel.text = hi.level.ToString();

                UILabel hpLabel = PanelTools.findChild<UILabel>(item, "hp");
                hpLabel.text = hi.hp.ToString();

                UILabel costLabel = PanelTools.findChild<UILabel>(item, "cost");
                costLabel.text = hi.leader.ToString();

                UILabel atkLabel = PanelTools.findChild<UILabel>(item, "atk");
                atkLabel.text = hi.atk.ToString();

                UILabel defLabel = PanelTools.findChild<UILabel>(item, "def");
                defLabel.text = hi.def.ToString();

                UILabel reLabel = PanelTools.findChild<UILabel>(item, "re");
                reLabel.text = hi.recover.ToString();

                UISprite type = PanelTools.findChild<UISprite>(item, "type");
                type.atlas = atlas;

                switch (hi.series)
                {
                    case 1:
                        type.spriteName = "cls_blue_38";
                        break;
                    case 2:
                        type.spriteName = "cls_red_38";
                        break;
                    case 3:
                        type.spriteName = "cls_green_38";
                        break;
                    case 4:
                        type.spriteName = "cls_yellow_38";
                        break;
                    case 5:
                        type.spriteName = "cls_white_38";
                        break;
                    case 6:
                        type.spriteName = "cls_purple_38";
                        break;
                }

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

    public void ShowLeader()
    {
        UILabel leadLabel = PanelTools.findChild<UILabel>(gameObject, "Titlebg/leadLabel");

        nleader = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetTeamLeader(nCurTeamId + 1);
        nMaxleader = DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).nMaxLeader;
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
                if (child.gameObject.name == "icon" && int.Parse(idLabel.text) != 0)
                {
                    child.gameObject.SetActive(true);
                    continue;
                }

                if (int.Parse(idLabel.text) == 0 && child.gameObject.name == "friengSprite")
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

        UILabel teamLabel = PanelTools.findChild<UILabel>(gameObject, "Titlebg/teamLabel");

        teamLabel.text = (nCurTeamId + 1).ToString() + "/3";
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
           

            Debug.Log("nCurTeamId = " + nCurTeamId.ToString());
        }

        if (teamid != nCurTeamId)
        {
            SetCurTeam();
            DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).nCurTeamId = nCurTeamId;
        }
    }
}
