using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class FriendRankList : MonoBehaviour {

    public GameObject totalRank;
    
    GameObject RankList;
    GameObject grid;
    GameObject item;


    public int pageIndex = 0;
    // Use this for initialization
	void Start () 
    {
        PanelTools.setBtnFunc(transform, "Title/btnBack", onBack);

        RankList = PanelTools.findChild(gameObject, "Scroll View");
        grid = PanelTools.findChild(RankList, "Grid");
        item = PanelTools.findChild(RankList, "Item");


        PanelTools.setBtnFunc(transform, "Title/totalBtn", onTotal);

	}

    void onBack(GameObject obj)
    {
        DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).arenaMain.ShowPageByIndex(pageIndex);
    }

    void onTotal(GameObject obj)
    {
        gameObject.SetActive(false);
        totalRank.SetActive(true);

        DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).SendArenaTotalRankingMsg();
    }

    class Item
    {
        public GameObject root;
        public UILabel id;
        public UISprite icon;
        public UISprite frame;
        public UILabel playerName;
        public UILabel point;
        public UILabel rankName;
        public UILabel record;
        public UILabel rank;

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

    public void ShowRankList()
    {
        ClearUI();

        int nCount = DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).friendRankList.Count;

        for (int i = 0; i < nCount; ++i)
        {
            ArenaRankUser aru = DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).friendRankList[i];

            if (aru == null)
            {
                return;
            }

            Item rankItem = new Item();
            rankItem.root = NGUITools.AddChild(grid, item);
            rankItem.root.name = aru.id.ToString();
            rankItem.root.SetActive(true);

            rankItem.icon = PanelTools.findChild<UISprite>(rankItem.root, "heroIcon");
            rankItem.icon.spriteName = aru.hero.portarait;

            rankItem.id = PanelTools.findChild<UILabel>(rankItem.root, "idLabel");
            rankItem.id.text = aru.id.ToString();

            rankItem.playerName = PanelTools.findChild<UILabel>(rankItem.root, "playerName");
            rankItem.playerName.text = aru.name;

            rankItem.point = PanelTools.findChild<UILabel>(rankItem.root, "pointLabel");
            rankItem.point.text = aru.arenaPoint.ToString();

            rankItem.rankName = PanelTools.findChild<UILabel>(rankItem.root, "rankName");
            rankItem.rankName.text = aru.arenaName;

            rankItem.record = PanelTools.findChild<UILabel>(rankItem.root, "record");
            rankItem.record.text = aru.totalWin.ToString() + "胜" + aru.totalLose.ToString() + "负";

            rankItem.rank = PanelTools.findChild<UILabel>(rankItem.root, "rankLabel");
            rankItem.rank.text = "第" + aru.arenaRank.ToString() + "名";


            itemList.Add(rankItem);
        }


        UIGrid uigrid = grid.GetComponent<UIGrid>();
        uigrid.repositionNow = true;
    }
}
