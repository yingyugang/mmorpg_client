using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using System;
using UI;

public class FriendList : MonoBehaviour {

    GameObject returnBtn;

    GameObject friendsList;
    GameObject item;
    GameObject grid;


	void Awake () {

        returnBtn = PanelTools.findChild(gameObject, "Title/btnBack");
        if (returnBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onReturn;
        }

        friendsList = PanelTools.findChild(gameObject, "Scroll View");
        grid = PanelTools.findChild(friendsList, "Grid");
        item = PanelTools.findChild(friendsList, "Item");

	}

    void OnEnable() 
    { 
        ShowFriend(); 
    }

    void onReturn(GameObject go)
    {
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowPanelFriends();
    }

    void onItem(GameObject go)
    {
        UILabel idLabel = PanelTools.findChild<UILabel>(go, "idLabel");

        UInt64 nId = UInt64.Parse(idLabel.text);
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowFriendDetail(nId);
    }

    class Item
    {
        public GameObject root;
        public UILabel id;
        public UISprite heroIcon;
        public UISprite itemIcon1;
        public UISprite itemIcon2;
        public UISprite itemIcon3;
        public UILabel nameLabel;
        public UILabel levelLabel;
        public UILabel rankLabel;
        public UILabel timeLabel;

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

    public void ShowFriend()
    {
        ClearUI();

        uint nMaxFriend = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.maxFriend;

        FriendInfo[] friendList = DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).getFriendList();
        int nCount = friendList.Length;

        UILabel countLabel = PanelTools.findChild<UILabel>(gameObject, "Title/countLabel");
        countLabel.text = nCount.ToString() +  "/" + nMaxFriend.ToString();


        for (int i = 0; i < nCount; ++i)
        {

            FriendInfo ff = friendList[i];

            if (ff == null || ff.status != FRIEND_STATUS.FRIEND_STATUS_FRIEND)
            {
                continue;
            }

            Item friendItem = new Item();

            friendItem.root = NGUITools.AddChild(grid, item);
            friendItem.root.name = i.ToString();
            friendItem.root.SetActive(true);
            UIEventListener.Get(friendItem.root).onClick = onItem;

            friendItem.id = PanelTools.findChild<UILabel>(friendItem.root, "idLabel");
            friendItem.id.text = ff.userid.ToString();

            friendItem.heroIcon = PanelTools.findChild<UISprite>(friendItem.root, "heroIcon");
            friendItem.heroIcon.spriteName = ff.hero.portarait;

            friendItem.itemIcon1 = PanelTools.findChild<UISprite>(friendItem.root, "needIcon1");
            friendItem.itemIcon2 = PanelTools.findChild<UISprite>(friendItem.root, "needIcon2");
            friendItem.itemIcon3 = PanelTools.findChild<UISprite>(friendItem.root, "needIcon3");

            DataCenter.FriendGift fg = new DataCenter.FriendGift();
            fg.iniGift((int)ff.gift1);
            friendItem.itemIcon1.spriteName = fg.giftIcon;

            fg.iniGift((int)ff.gift2);
            friendItem.itemIcon2.spriteName = fg.giftIcon;

            fg.iniGift((int)ff.gift3);
            friendItem.itemIcon3.spriteName = fg.giftIcon;



            friendItem.nameLabel = PanelTools.findChild<UILabel>(friendItem.root, "nameLabel");
            friendItem.nameLabel.text = ff.username;

            friendItem.levelLabel = PanelTools.findChild<UILabel>(friendItem.root, "levelLabel");
            friendItem.levelLabel.text = "LV." + ff.level.ToString();

            friendItem.rankLabel = PanelTools.findChild<UILabel>(friendItem.root, "rankName");
            friendItem.rankLabel.text = ff.arenaHonor;

            friendItem.timeLabel = PanelTools.findChild<UILabel>(friendItem.root, "onLineTime");
            friendItem.timeLabel.text = DataManager.getModule<DataServTime>(DATA_MODULE.Data_ServTime).getServTime((ulong)ff.applyTime, "yyyy-MM-dd HH:mm");

            itemList.Add(friendItem);
        }

        UIGrid uiGrid = grid.GetComponent<UIGrid>();
        uiGrid.repositionNow = true;
    }


}
