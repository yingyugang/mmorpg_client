using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class ArenaRank : MonoBehaviour {

    GameObject RankList;
    GameObject grid;
    GameObject item;

    public int pageIndex = 0;

    // Use this for initialization
	void Start () {
        PanelTools.setBtnFunc(transform, "Title/btnBack", onBack);

        RankList = PanelTools.findChild(gameObject, "Scroll View");
        grid = PanelTools.findChild(RankList, "Grid");
        item = PanelTools.findChild(RankList, "Item");
       
        ShowRankInfo();
	}

    void onBack(GameObject obj)
    {
        DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).arenaMain.ShowPageByIndex(pageIndex);
    }

    class Item
    {
        public GameObject root;
        public UILabel id;
        public UISprite icon;
        public UILabel rankName;
        public UILabel point;
        public UILabel rewardName;
        public UILabel reward;

        public void Release()
        {
            if (root != null)
            {
                GameObject.Destroy(root);
            }
        }
    }

    public void ClearUI()
    {
        foreach (Item r in itemList)
        {
            r.Release();
        }

        itemList.Clear();
    }

    List<Item> itemList = new List<Item>();

    public void ShowRankInfo()
    {
        ClearUI();

        ConfigTable arenarank = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ARENA_RANK);
        int nCount = arenarank.rows.Length;

        for (int i = 0; i < nCount; ++i)
        {
            ConfigRow cr = arenarank.rows[i];

            if (cr == null)
            {
                return;
            }

            Item rankItem = new Item();
            rankItem.root = NGUITools.AddChild(grid, item);
            rankItem.root.name = cr.getIntValue(DICT_ARENA_RANK.ID).ToString();
            rankItem.root.SetActive(true);

            rankItem.id = PanelTools.findChild<UILabel>(rankItem.root, "idLabel");
            rankItem.id.text = cr.getStringValue(DICT_ARENA_RANK.ID);

            rankItem.rankName = PanelTools.findChild<UILabel>(rankItem.root, "nameLabel");

            string strTemp = cr.getStringValue(DICT_ARENA_RANK.NAME_ID);
            rankItem.rankName.text = BaseLib.LanguageMgr.getString(strTemp);

            rankItem.point = PanelTools.findChild<UILabel>(rankItem.root, "pointLabel");
            rankItem.point.text = cr.getStringValue(DICT_ARENA_RANK.ARENA_POINT);

            rankItem.rewardName = PanelTools.findChild<UILabel>(rankItem.root, "rewardName");

            if (cr.getIntValue(DICT_ARENA_RANK.ITEMTYPE_ID) == 0)
            {
                rankItem.rewardName.text = cr.getStringValue(DICT_ARENA_RANK.DIAMOND);
            }
            else
            {
                rankItem.rewardName.text = cr.getStringValue(DICT_ARENA_RANK.ITEMTYPE_ID);
            }

            itemList.Add(rankItem);
            
        }


        UIGrid uiGrid = grid.GetComponent<UIGrid>();

        uiGrid.repositionNow = true;
    }

}
