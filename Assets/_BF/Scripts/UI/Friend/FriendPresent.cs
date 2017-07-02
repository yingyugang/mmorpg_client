using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;
using System;

public class FriendPresent: MonoBehaviour {

    GameObject returnBtn;
    GameObject allBtn;
    GameObject toggleBtn;

    GameObject giftList;
    GameObject item;
    GameObject grid;
    GameObject rightBtn;
    GameObject leftBtn;


    uint CurGiftType = 1;
    bool bToggle = false;
    // Use this for initialization
	void Start () {

        returnBtn = PanelTools.findChild(gameObject, "Title/btnBack");
        if (returnBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onReturn;
        }

        allBtn = PanelTools.findChild(gameObject, "NeedGift/allButton");
        if (allBtn != null)
        {
            UIEventListener.Get(allBtn).onClick = onAll;
        }

        toggleBtn = PanelTools.findChild(gameObject, "NeedGift/Toggle");
        if (toggleBtn != null)
        {
            UIEventListener.Get(toggleBtn).onClick = onToggle;
        }

        rightBtn = PanelTools.findChild(gameObject, "NeedGift/rightButton");
        if (rightBtn != null)
        {
            UIEventListener.Get(rightBtn).onClick = onRight;
        }

        leftBtn = PanelTools.findChild(gameObject, "NeedGift/leftButton");
        if (leftBtn != null)
        {
            UIEventListener.Get(leftBtn).onClick = onLeft;
        }
	}

    void OnEnable()
    {
        ShowFriend();
        ShowPresent();
    }

    void onReturn(GameObject go)
    {
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowFriendGift();
    }

    List<UInt64> targetIdList = new List<UInt64>();
    
    void onPresent(GameObject go)
    {
        targetIdList.Clear();
        UILabel idLabel = PanelTools.findChild<UILabel>(go.transform.parent.gameObject, "idLabel");

        UInt64 idUser = UInt64.Parse(idLabel.text);
        targetIdList.Add(idUser);

        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).sendGift(CurGiftType, targetIdList.ToArray());
    }

    void onAll(GameObject go)
    {
        targetIdList.Clear();

        FriendInfo[] friends = DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).getFriendList(CurGiftType);

        foreach (FriendInfo fi in friends)
        {
            targetIdList.Add(fi.userid);
        }

        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).sendGift(CurGiftType, targetIdList.ToArray());
    }

    void onRight(GameObject go)
    {
        if (CurGiftType < DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).giftTypeList.Count)
        {
            ++CurGiftType;
        }

        ShowPresent();
        ShowFriend();
    }

    void onLeft(GameObject go)
    {
        if (CurGiftType > 1)
        {
            --CurGiftType;
        }

        ShowPresent();
        ShowFriend();
    }

    void onToggle(GameObject go)
    {
        UIToggle uiToggle = go.GetComponent<UIToggle>();

        if (uiToggle.value == true)
        {
            bToggle = true;
        }
        else
        {
            bToggle = false;
        }


        ShowFriend();
    }

    void InitUI()
    {
        giftList = PanelTools.findChild(gameObject, "Scroll View");
        grid = PanelTools.findChild(giftList, "Grid");
        item = PanelTools.findChild(giftList, "Item");
    }

    class Item
    {
        public GameObject root;
        public UILabel id;
        public UISprite heroIcon;
        public UILabel name;
        public UISprite itemIcon1;
        public UISprite itemIcon2;
        public UISprite itemIcon3;


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
        InitUI();
        ClearUI();

        FriendInfo[] friendList = null;

        if (bToggle)
        {
            friendList = DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).getFriendList(CurGiftType);
        }
        else
        {
            friendList = DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).getFriendList();
        }

        int nCount = friendList.Length;

        //送礼时间判断


        for (int i = 0; i < nCount; ++i)
        {
            FriendInfo fi = friendList[i];

            Item giftItem = new Item();

            giftItem.root = NGUITools.AddChild(grid, item);
            giftItem.root.name = i.ToString();
            giftItem.root.SetActive(true);

            giftItem.id = PanelTools.findChild<UILabel>(giftItem.root, "idLabel");
            giftItem.id.text = fi.userid.ToString();

            giftItem.name = PanelTools.findChild<UILabel>(giftItem.root, "giftName");
            giftItem.name.text = fi.username;

            giftItem.heroIcon = PanelTools.findChild<UISprite>(giftItem.root, "heroIcon");
            giftItem.heroIcon.spriteName = fi.hero.portarait;

            DataCenter.FriendGift fg = new DataCenter.FriendGift();

            giftItem.itemIcon1 = PanelTools.findChild<UISprite>(giftItem.root, "needIcon1");
            fg.iniGift((int)fi.gift1);
            giftItem.itemIcon1.spriteName = fg.giftIcon;


            giftItem.itemIcon2 = PanelTools.findChild<UISprite>(giftItem.root, "needIcon2");
            fg.iniGift((int)fi.gift2);
            giftItem.itemIcon2.spriteName = fg.giftIcon;


            giftItem.itemIcon3 = PanelTools.findChild<UISprite>(giftItem.root, "needIcon3");
            fg.iniGift((int)fi.gift3);
            giftItem.itemIcon3.spriteName = fg.giftIcon;

            GameObject pstBtn = PanelTools.findChild(giftItem.root, "presentButton");
            UIEventListener.Get(pstBtn).onClick = onPresent;


            UILabel timeLabel = PanelTools.findChild<UILabel>(giftItem.root, "timeLabel");
            timeLabel.text = DataManager.getModule<DataServTime>(DATA_MODULE.Data_ServTime).getServTime((ulong)fi.giveGiftTime, "yyyy-MM-dd HH:mm");



            itemList.Add(giftItem);
        }

        UIGrid uiGrid = grid.GetComponent<UIGrid>();
        uiGrid.repositionNow = true;
    }


    public void ShowPresent()
    {
        FriendGift fg = DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).giftTypeList[(int)CurGiftType - 1];

        UILabel nameLabel = PanelTools.findChild<UILabel>(gameObject, "NeedGift/nameLabel");
        nameLabel.text = fg.giftName + fg.amount.ToString();

        UILabel giftName = PanelTools.findChild<UILabel>(gameObject, "NeedGift/giftName");
        giftName.text = "赠送好友" + fg.giftName;

        UISprite giftIcon = PanelTools.findChild<UISprite>(gameObject, "NeedGift/giftIcon");
        giftIcon.spriteName = fg.giftIcon;

        UILabel giftType = PanelTools.findChild<UILabel>(gameObject, "NeedGift/giftType");
        giftType.text = CurGiftType.ToString();
    }

}
