using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class SelectEvolution : MonoBehaviour
{

    public UIButton btnReturn;

    public GameObject menu;
    public GameObject Evolution;
    public GameObject Item;
    public GameObject grid;
    public GameObject sortBtn;


    public UILabel heroCount;
    public UILabel sortLabel;
    int nSortfun = 0;

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
        menu.SetActive(true);
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

    void onEvolution(GameObject go)
    {
        UILabel idLabel = PanelTools.findChild<UILabel>(go, "id");

        int id = int.Parse(idLabel.text);
        HeroInfo hero = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(id);

        if (DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).isEvolvability(hero.type))
        {
            gameObject.SetActive(false);
            Evolution.SetActive(true);

            PartnerEvolution pe = Evolution.GetComponent<PartnerEvolution>();
            pe.ShowBeforeInfo(id, hero.type);
        }

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
        public UISprite cover;
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

                heroItem.cover = PanelTools.findChild<UISprite>(heroItem.root, "cover");

                if (DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).isEvolvability(hi.type))
                {
                    heroItem.cover.gameObject.SetActive(false);
                }
                else
                {
                    heroItem.cover.gameObject.SetActive(true);
                }
                
                heroItem.root.SetActive(true);
                UIEventListener.Get(heroItem.root).onClick = onEvolution;

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
            if (hi.lvlupMethod > 3)
            {
                continue;
            }

            heroList.Add(hi);
        }
    }
}
