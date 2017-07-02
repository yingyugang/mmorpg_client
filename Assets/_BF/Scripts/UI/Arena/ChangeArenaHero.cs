using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class ChangeArenaHero : MonoBehaviour
{

    public UIButton btnReturn;

    public GameObject arenaTeam;
    public GameObject Item;
    public GameObject grid;
    public GameObject sortBtn;


    public UILabel heroCount;
    public UILabel sortLabel;
    int nSortfun = 0;

    public int idGround = -1;
    public int idHero = 0;

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
        arenaTeam.SetActive(true);
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
        ArenaTeam at = arenaTeam.GetComponent<ArenaTeam>();
        
        UILabel idLabel = PanelTools.findChild<UILabel>(go, "id");
        int id = int.Parse(idLabel.text);

        if (idGround == -1)
        {
            return;
        }

        if (IsOverLeader(id, at.nCurTeamId, idHero))
        {
            return;
        }

        DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).ChangeTeamerById(idGround, id, at.nCurTeamId);
        
        gameObject.SetActive(false);
        arenaTeam.SetActive(true);
        at.ShowTeamHero();

    }

    void onDelBtn(GameObject go)
    {
        ArenaTeam at = arenaTeam.GetComponent<ArenaTeam>();
        
        if (idGround == -1)
        {
            return;
        }

        DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).DelTeamerById(idGround, idHero, at.nCurTeamId);

        gameObject.SetActive(false);
        arenaTeam.SetActive(true);

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
                if (j > nCount)
                {
                    return;
                }

                GameObject delBtn = PanelTools.findChild(_row.root, "delButton");
                UIEventListener.Get(delBtn).onClick = onDelBtn;

                if (i == 0 && j == 0)
                {
                    delBtn.SetActive(true);
                    ++_row.child;
                    continue;
                }


                HeroInfo hi = heroList[j - 1];

                HeroItem heroItem = new HeroItem();
                heroItem.root = PanelTools.findChild(_row.root, _row.child.ToString());
                heroItem.id = PanelTools.findChild<UILabel>(heroItem.root, "id");
                heroItem.id.text = hi.id.ToString();

                heroItem.icon = PanelTools.findChild<UISprite>(heroItem.root, "icon");
                heroItem.icon.spriteName = hi.portarait;

                heroItem.level = PanelTools.findChild<UILabel>(heroItem.root, "levelLabel");
                heroItem.level.text = hi.level.ToString();

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

    List<HeroInfo> heroList = new List<HeroInfo>();

    public void FilterHeroList()
    {
        heroList.Clear();

        foreach (HeroInfo hi in DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).heroInfoList)
        {
            if (DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).isOnGround(hi.id))
            {
                continue;
            }

            heroList.Add(hi);
        }
    }


    public bool IsOverLeader(int curId, int teamId, int befId)
    {
        int leader = 0;

        HeroInfo curHero = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(curId);

        if (curHero != null)
        {
            leader = curHero.leader;
        }

        int nCurLeader = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetTeamLeader(teamId + 1);
        int nMaxleader = (int)DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.maxLeader;
        int nBefleader = 0;

        HeroInfo befHero = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(befId);

        if (befHero != null)
        {
            nBefleader = befHero.leader;
        }

        if (leader + nCurLeader - nBefleader > nMaxleader)
        {
            return true;
        }

        return false;
    }
}
