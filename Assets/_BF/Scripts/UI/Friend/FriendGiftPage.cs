using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using System;
using UI;

public class FriendGiftPage: MonoBehaviour {

    GameObject returnBtn;
    GameObject presentBtn;

    GameObject giftList;
    GameObject item;
    GameObject grid;

    GameObject needItem1;
    GameObject needItem2;
    GameObject needItem3;

    GameObject awardAllBtn;

    // Use this for initialization
    void Start()
    {
	}

    void onReturn(GameObject go)
    {
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowPanelFriends();
    }

    void onPresent(GameObject go)
    {
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowFriendPresent();
    }

    void onAwardAll(GameObject go)
    {
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).awardGift(0, 0);
    }

    void onAward(GameObject go)
    {
        UILabel idLabel = PanelTools.findChild<UILabel>(go.transform.parent.gameObject, "idLabel");
        uint nId = uint.Parse(idLabel.text);

        UILabel friendIdLabel = PanelTools.findChild<UILabel>(go.transform.parent.gameObject, "friendIdLabel");
        UInt64 nFirendId = UInt64.Parse(friendIdLabel.text);

        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).awardGift(nFirendId, nId);
    }

    void onItem1(GameObject go)
    {
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).SelectNeedGift(0);
    }

    void onItem2(GameObject go)
    {
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).SelectNeedGift(1);
    }

    void onItem3(GameObject go)
    {
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).SelectNeedGift(2);
    }

    class Item
    {
        public GameObject root;
        public UILabel id;
        public UILabel friendId;
        public UISprite heroIcon;
        public UILabel name;
        public UILabel giftName;
        public UISprite giftIcon; 

        public void Release()
        {
            if (root != null)
            {
                GameObject.Destroy(root);
            }
        }
    }

    void InitUI()
    {
        returnBtn = PanelTools.findChild(gameObject, "Title/btnBack");
        if (returnBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onReturn;
        }

        presentBtn = PanelTools.findChild(gameObject, "Title/giftButton");
        if (presentBtn != null)
        {
            UIEventListener.Get(presentBtn).onClick = onPresent;
        }

        giftList = PanelTools.findChild(gameObject, "Scroll View");
        grid = PanelTools.findChild(giftList, "Grid");
        item = PanelTools.findChild(giftList, "Item");


        awardAllBtn = PanelTools.findChild(gameObject, "NeedGift/recButton");
        UIEventListener.Get(awardAllBtn).onClick = onAwardAll;
        needItem1 = PanelTools.findChild(gameObject, "NeedGift/Item1");
        UIEventListener.Get(needItem1).onClick = onItem1;
        needItem2 = PanelTools.findChild(gameObject, "NeedGift/Item2");
        UIEventListener.Get(needItem2).onClick = onItem2;
        needItem3 = PanelTools.findChild(gameObject, "NeedGift/Item3");
        UIEventListener.Get(needItem3).onClick = onItem3;
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
        InitUI();
        ClearUI();
        ShowNeedGift();

        DataCenter.FriendGift[] giftList = DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).getGiftList();
        int nCount = giftList.Length;

        for (int i = 0; i < nCount; ++i)
        {
            DataCenter.FriendGift fg = giftList[i];

            Item giftItem = new Item();

            giftItem.root = NGUITools.AddChild(grid, item);
            giftItem.root.name = i.ToString();
            giftItem.root.SetActive(true);

            giftItem.id = PanelTools.findChild<UILabel>(giftItem.root, "idLabel");
            giftItem.id.text = fg.giftid.ToString();

            giftItem.friendId = PanelTools.findChild<UILabel>(giftItem.root, "friendIdLabel");
            giftItem.friendId.text = fg.friend.userid.ToString();

            giftItem.heroIcon = PanelTools.findChild<UISprite>(giftItem.root, "heroIcon");
            giftItem.heroIcon.spriteName = fg.friend.hero.portarait;

            giftItem.name = PanelTools.findChild<UILabel>(giftItem.root, "nameLabel");
            giftItem.name.text = fg.friend.username + "送的礼物";

            giftItem.giftName = PanelTools.findChild<UILabel>(giftItem.root, "giftName");
            giftItem.giftIcon = PanelTools.findChild<UISprite>(giftItem.root, "giftIcon");

            switch (fg.giftClassify)
            {
                case 1:
                    giftItem.giftName.text = "金币";
                    giftItem.giftIcon.spriteName = "";//金币图标
                    break;
                case 2:
                    giftItem.giftName.text = "魂";
                    giftItem.giftIcon.spriteName = "";//魂图标
                    break;
                case 3:
                    giftItem.giftName.text = "友情点";
                    giftItem.giftIcon.spriteName = "";//友情点
                    break;
                case 4:
                    giftItem.giftName.text = fg.giftItem.name;
                    giftItem.giftIcon.spriteName = fg.giftItem.icon;

                    break;
            }


            GameObject recBtn = PanelTools.findChild(giftItem.root, "recButton");
            UIEventListener.Get(recBtn).onClick = onAward;
            itemList.Add(giftItem);
        }

        UIGrid uiGrid = grid.GetComponent<UIGrid>();
        uiGrid.repositionNow = true;
    }

    protected void ShowNeedGift()
    {
        int type1 = (int)DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.idGiftType1;
        int type2 = (int)DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.idGiftType2;
        int type3 = (int)DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.idGiftType3;

        DataCenter.FriendGift fg = new DataCenter.FriendGift();
        if (type1 != 0)
        {
            fg.iniGift(type1);
            UISprite icon1 = PanelTools.findChild<UISprite>(gameObject, "NeedGift/Item1");
            icon1.spriteName = fg.giftIcon;
        }

        if (type2 != 0)
        {
            fg.iniGift(type2);
            UISprite icon2 = PanelTools.findChild<UISprite>(gameObject, "NeedGift/Item2");
            icon2.spriteName = fg.giftIcon;
        }

        if (type3 != 0)
        {
            fg.iniGift(type3);
            UISprite icon3 = PanelTools.findChild<UISprite>(gameObject, "NeedGift/Item3");
            icon3.spriteName = fg.giftIcon;
        }
    }
    
    public void UpdateNeedGift(int nIndex,int type)
    {
        DataCenter.FriendGift fg = new DataCenter.FriendGift();
        fg.iniGift(type);
        
        switch (nIndex)
        {
            case 1:
                UISprite icon1 = PanelTools.findChild<UISprite>(gameObject, "NeedGift/Item1");
                icon1.spriteName = fg.giftIcon;
                break;
            case 2:
                UISprite icon2 = PanelTools.findChild<UISprite>(gameObject, "NeedGift/Item2");
                icon2.spriteName = fg.giftIcon;
                break;
            case 3:
                UISprite icon3 = PanelTools.findChild<UISprite>(gameObject, "NeedGift/Item3");
                icon3.spriteName = fg.giftIcon;
                break;
        }
            
    }
}
