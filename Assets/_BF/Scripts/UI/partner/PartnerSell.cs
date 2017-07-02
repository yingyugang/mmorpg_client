using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class PartnerSell : MonoBehaviour
{

    public UIButton btnReturn;
    public UIButton btnClear;
    public UIButton btnConfirm;

    public GameObject menu;
    public GameObject Item;
    public GameObject grid;
    public GameObject sortBtn;

    public UILabel heroCount;
    public UILabel sortLabel;
    public UILabel countLabel;
    public UILabel goldLabel;

    int nSortfun = 0;
    int nCountSelect = 0;
    int nMaxCountSelect = 10;

    // Use this for initialization
    void Start()
    {
        if (btnReturn != null)
            UIEventListener.Get(btnReturn.gameObject).onClick = onReturn;
        if (sortBtn != null)
            UIEventListener.Get(sortBtn.gameObject).onClick = onSort;
        if (btnClear != null)
            UIEventListener.Get(btnClear.gameObject).onClick = onClearSelect;
        if (btnConfirm != null)
            UIEventListener.Get(btnConfirm.gameObject).onClick = onConfirm;
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

    void onClearSelect(GameObject go)
    {
        ClearSelectItem();
        RefreshGoldLabel();
    }

    void onSelect(GameObject go)
    {

        UILabel idLabel = PanelTools.findChild<UILabel>(go, "id");
        int id = int.Parse(idLabel.text);


        GameObject redBG = PanelTools.findChild(go, "selIcon");
        UILabel NOLabel = PanelTools.findChild<UILabel>(go, "NOLabel");

        if (redBG.activeSelf)
        {
            redBG.SetActive(false);
            //NOLabel.gameObject.SetActive(false);
            --nCountSelect;
            RemoveSelectItemById(id);
        }
        else
        {
            if (nCountSelect < nMaxCountSelect)
            {
                ++nCountSelect;
                redBG.SetActive(true);
                //NOLabel.gameObject.SetActive(true);
                //NOLabel.text = nCountSelect.ToString();
                AddSelectItemById(id);
            }
        }

        countLabel.text = nCountSelect.ToString();

        //RefreshNOLabel();
        RefreshGoldLabel();
    }

    void onConfirm(GameObject go)
    {
        string strContent = BaseLib.LanguageMgr.getString(66366101);
        DataManager.getModule<DataShop>(DATA_MODULE.Data_Shop).ShowShopComfirm("PROMPT" , strContent, SellHero);
    }

    void SellHero(GameObject go)
    {
        List<uint> idList = new List<uint>();

        foreach (HeroItem hi in selectedItemList)
        {
            uint idHero = uint.Parse(hi.id.text);

            idList.Add(idHero);
        }

        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).sellHero(idList.ToArray());

        ClearSelectItem();
        RefreshGoldLabel();

        gameObject.SetActive(false);
        menu.SetActive(true);
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

    public List<HeroItem> itemList = new List<HeroItem>();
    public List<HeroItem> selectedItemList = new List<HeroItem>();

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
        itemList.Clear();
        selectedItemList.Clear();
        nCountSelect = 0;
    }

    public void ShowHeroBrowse()
    {
        ClearUI();
        FilterHeroList();
        heroCount = PanelTools.findChild<UILabel>(gameObject, "Titlebg/countLabel");

        int nCount = heroList.Count;
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

                if (hi.btCollected == 1 || hi.fight)
                {
                    continue;
                }

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
                UIEventListener.Get(heroItem.root).onClick = onSelect;

                ++_row.child;
                itemList.Add(heroItem);
            }

        }
    }

    public void AddSelectItemById(int id)
    {
        foreach (HeroItem hi in itemList)
        {
            if (hi.id.text == id.ToString())
            {
                selectedItemList.Add(hi);
            }
        }
    }

    public void RemoveSelectItemById(int id)
    {
        foreach (HeroItem hi in itemList)
        {
            if (hi.id.text == id.ToString())
            {
                selectedItemList.Remove(hi);
            }
        }
    }

    public void RefreshNOLabel()
    {
        int nCount = 1;

        foreach (HeroItem hi in selectedItemList)
        {
            UILabel NOLabel = PanelTools.findChild<UILabel>(hi.root, "NOLabel");
            NOLabel.text = nCount.ToString();
            ++nCount;
        }
    }

    public void ClearSelectItem()
    {
        foreach (HeroItem hi in selectedItemList)
        {
            UILabel NOLabel = PanelTools.findChild<UILabel>(hi.root, "NOLabel");
            NOLabel.text = "0";
            NOLabel.gameObject.SetActive(false);

            GameObject redBG = PanelTools.findChild(hi.root, "selIcon");
            redBG.SetActive(false);
        }

        selectedItemList.Clear();
        nCountSelect = 0;
    }

    public void RefreshGoldLabel()
    {
        int nGold = 0;

        foreach (HeroItem hi in selectedItemList)
        {
            UILabel idLabel = PanelTools.findChild<UILabel>(hi.root, "id");

            int id = int.Parse(idLabel.text);
            HeroInfo hero = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(id);

            nGold += hero.coin;
        }

        countLabel.text = selectedItemList.Count.ToString();
        goldLabel.text = nGold.ToString();
    }


    List<HeroInfo> heroList = new List<HeroInfo>();

    public void FilterHeroList()
    {
        heroList.Clear();
        
        foreach(HeroInfo hi in DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).heroInfoList)
        {
            if (hi.btCollected == 1 || hi.fight)
            {
                continue;
            }

            heroList.Add(hi);
        }
    }
}
