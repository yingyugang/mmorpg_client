using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class SelectTeamer: MonoBehaviour
{

    public UIButton btnReturn;

    public GameObject menu;
    public GameObject partnerEdit;
    public GameObject Item;
    public GameObject grid;
    public GameObject sortBtn;


    public UILabel heroCount;
    public UILabel sortLabel;
    int nSortfun = 0;

    public int idGround = -1;

    // Use this for initialization
	void Start ()
    {
        if (btnReturn != null)
            UIEventListener.Get(btnReturn.gameObject).onClick = onReturn;
        if (btnReturn != null)
            UIEventListener.Get(sortBtn.gameObject).onClick = onSort;

    }

	// Update is called once per frame
	void Update () 
    {	
	}

    void onReturn(GameObject go)
    {
        gameObject.SetActive(false);
        partnerEdit.SetActive(true);
    }

    void onSort(GameObject go)
    {
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).SortHeroList(nSortfun);

        if (sortLabel != null)
        {
            switch (nSortfun)
            {
                case 0:
                    sortLabel.text = "等级";

                    break;
                case 1:
                    sortLabel.text = "星级";

                    break;
            }
                
        }

        if (nSortfun < 1)
        {
            ++nSortfun;
        }
        else
        {
            nSortfun = 0;
        }

        ShowHeroBrowse();
    }

    void onHero(GameObject go)
    {
        PartnerEdit pe = partnerEdit.GetComponent<PartnerEdit>();
        
        UILabel idLabel = PanelTools.findChild<UILabel>(go, "id");
        int id = int.Parse(idLabel.text);

        if (idGround == -1)
        {
            return;
        }

        if (IsOverLeader(id, pe.nCurTeamId))
        {
            return;
        }

        TeamInfo curTeamInfo = null;
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams.TryGetValue(pe.nCurTeamId + 1, out curTeamInfo);

        if (0 == curTeamInfo.GetHeroCount())
        {
            DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams[pe.nCurTeamId + 1].leaderPos = idGround + 1;
            DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).addNewTeam();
            pe.ShowAddTeam();
        }

        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).ChangeTeamerById(idGround, id, pe.nCurTeamId);
        gameObject.SetActive(false);
        partnerEdit.SetActive(true);

        pe.ShowTeamHero();
    }

    public List<row> rowList = new List<row>();

    public class row
    {
        public GameObject root;
        public int child = 0;


        public void Release()
        {
            if (root != null)
            {
                GameObject.Destroy(root);
            }
        }
    }

    public class HeroItem
    {
        public GameObject root;
        public UILabel id;
        public UISprite icon;
        public UILabel level;
        public UILabel equip;
        public UISprite star;
        public UISprite fight;
        public UISprite equipicon;
        public UISprite seriesSprite;
        public UISprite frameSprite;
        public UISprite framebg;
    }

    public void ClearUI()
    {
        foreach (row r in rowList)
        {
            r.Release();
        }

        rowList.Clear();
    }

    public void ShowHeroBrowse()
    {
        ClearUI();
        FilterHeroList();
        heroCount = PanelTools.findChild<UILabel>(gameObject, "Titlebg/countLabel");

        int nCount = heroList.Count;

        AlineOnChild aoc = grid.GetComponent<AlineOnChild>();
        if (nCount > 50)
        {
            aoc.enabled = true;
        }
        else
        {
            aoc.enabled = false;
        }

        uint nMaxCount = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.maxHero;
        heroCount.text = nCount.ToString() + "/" + nMaxCount.ToString();

        for (int i = 0; i < (nCount / 4 + 1); ++i)
        {
            row _row = new row();

            _row.root = NGUITools.AddChild(grid, Item);
            _row.root.name = i.ToString();
            _row.root.SetActive(true);

            Vector3 position = _row.root.transform.position;
            _row.root.transform.localPosition = new UnityEngine.Vector3(position.x, position.y - i * 140, position.z);

            rowList.Add(_row);

            for (int j = i * 4; j < (i * 4 + 4); ++j)
            {
                if (j >= nCount)
                {
                    return;
                }

                HeroInfo hi = heroList[j];

                HeroItem heroItem = new HeroItem();
                heroItem.root = PanelTools.findChild(_row.root, _row.child.ToString());
                heroItem.id = PanelTools.findChild<UILabel>(heroItem.root, "id");
                heroItem.id.text = hi.id.ToString();

                heroItem.icon = PanelTools.findChild<UISprite>(heroItem.root, "icon");
                heroItem.icon.spriteName = hi.portarait;
                heroItem.icon.MakePixelPerfect();

                heroItem.level = PanelTools.findChild<UILabel>(heroItem.root, "levelLabel");

                if (hi.isMaxLevel())
                {
                    heroItem.level.text = "LV.MAX";
                }
                else
                {
                    heroItem.level.text = "LV." + hi.level.ToString();
                }

                heroItem.star = PanelTools.findChild<UISprite>(heroItem.root, "starSprite");
                heroItem.star.spriteName = "star" + hi.star.ToString();

                heroItem.fight = PanelTools.findChild<UISprite>(heroItem.root, "fightSprite");
                heroItem.fight.gameObject.SetActive(hi.fight);

                heroItem.equip = PanelTools.findChild<UILabel>(heroItem.root, "equipLabel");

                if (hi.equipId != 0)
                {
                    heroItem.equip.gameObject.SetActive(true);
                }
                else
                {
                    heroItem.equip.gameObject.SetActive(false);
                }

                heroItem.seriesSprite = PanelTools.findChild<UISprite>(heroItem.root, "seriesSprite");
                heroItem.seriesSprite.spriteName = "SERIES" + hi.series.ToString();

                heroItem.frameSprite = PanelTools.findChild<UISprite>(heroItem.root, "frameSprite");
                heroItem.frameSprite.spriteName = "frame" + hi.series.ToString();

                heroItem.framebg = PanelTools.findChild<UISprite>(heroItem.root, "framebg");
                heroItem.framebg.spriteName = "framebg" + hi.series.ToString();

                heroItem.root.SetActive(true);
                UIEventListener.Get(heroItem.root).onClick = onHero;

                ++_row.child;
            }

        }
    }

    public bool IsOverLeader(int id, int teamId)
    {
        int leader = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(id).leader;
        int nCurLeader = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetTeamLeader(teamId + 1);
        int nMaxleader = (int)DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.maxLeader;

        if (leader + nCurLeader > nMaxleader)
        {
            return true;
        }

        return false;
    }

    List<HeroInfo> heroList = new List<HeroInfo>();

    public void FilterHeroList()
    {
        heroList.Clear();

        int teamId = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).nCurTeamId;
        TeamInfo team = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams[teamId + 1];

        foreach (HeroInfo hi in DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).heroInfoList)
        {
            if (team.isInTeam(hi.id))
            {
                continue;
            }

            heroList.Add(hi);
        }
    }
}
