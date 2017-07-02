using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class SelectArenaHero : MonoBehaviour
{

    public UIButton btnReturn;

    public GameObject ArenaTeam;
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

    void onReturn(GameObject go)
    {
        gameObject.SetActive(false);
        ArenaTeam.SetActive(true);
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
        ArenaTeam at = ArenaTeam.GetComponent<ArenaTeam>();
        
        UILabel idLabel = PanelTools.findChild<UILabel>(go, "id");
        int id = int.Parse(idLabel.text);

        if (idGround == -1)
        {
            return;
        }

        IsOverLeader(id, at.nCurTeamId);

        if (IsOverLeader(id, at.nCurTeamId))
        {
            return;
        }

        if (0 == DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).dicArenaTeams[at.nCurTeamId + 1].GetHeroCount())
        {
            DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).dicArenaTeams[at.nCurTeamId + 1].leaderPos = idGround + 1;
        }

        DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).ChangeTeamerById(idGround, id, at.nCurTeamId);
        gameObject.SetActive(false);
        ArenaTeam.SetActive(true);

        at.ShowTeamHero();
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
        public UILabel star;
        public UILabel fight;

        public HeroInfo heroData;
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
        uint nMaxCount = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.maxHero;
        heroCount.text = nCount.ToString() + "/" + nMaxCount.ToString();

        //grid.transform.localPosition = new UnityEngine.Vector3(0,0,0);

        for (int i = 0; i < (nCount / 5 + 1); ++i)
        {
            row _row = new row();

            _row.root = NGUITools.AddChild(grid, Item);
            _row.root.name = i.ToString();
            _row.root.SetActive(true);

            Vector3 position = _row.root.transform.position;

            _row.root.transform.localPosition = new UnityEngine.Vector3(position.x, position.y - i * 120, position.z);

            rowList.Add(_row);

            for (int j = i * 5; j < (i * 5 + 5); ++j)
            {
                if (j >= nCount)
                {
                    return;
                }

                HeroInfo hi = heroList[j];

                HeroItem heroItem = new HeroItem();
                heroItem.heroData = hi;
                heroItem.root = PanelTools.findChild(_row.root, _row.child.ToString());
                heroItem.id = PanelTools.findChild<UILabel>(heroItem.root, "id");
                heroItem.id.text = hi.id.ToString();

                heroItem.icon = PanelTools.findChild<UISprite>(heroItem.root, "icon");
                heroItem.icon.spriteName = hi.portarait;

                heroItem.level = PanelTools.findChild<UILabel>(heroItem.root, "levelLabel");

                if (hi.isMaxLevel())
                {
                    heroItem.level.text = "LV.MAX";
                }
                else
                {
                    heroItem.level.text = hi.level.ToString();
                }

                heroItem.star = PanelTools.findChild<UILabel>(heroItem.root, "starLabel");
                heroItem.star.text = hi.star.ToString();

                heroItem.fight = PanelTools.findChild<UILabel>(heroItem.root, "fightLabel");
                heroItem.fight.gameObject.SetActive(hi.fight);

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
