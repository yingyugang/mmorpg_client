using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using System;
using UI;

public class FriendApply : MonoBehaviour {

    GameObject returnBtn;

    GameObject friendsList;
    GameObject item;
    GameObject grid;


    // Use this for initialization
    void Awake()
    {
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

    void onAccept(GameObject go)
    {
        UILabel idLabel = PanelTools.findChild<UILabel>(go.transform.parent.gameObject, "idLabel");
        int id = int.Parse(idLabel.text);


        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).accept((UInt64)id);
    }

    void onRefuse(GameObject go)
    {
        UILabel idLabel = PanelTools.findChild<UILabel>(go.transform.parent.gameObject, "idLabel");
        int id = int.Parse(idLabel.text);

        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).refuse((UInt64)id);
    }

    void onCancel(GameObject go)
    {
        UILabel idLabel = PanelTools.findChild<UILabel>(go.transform.parent.gameObject , "idLabel");
        int id = int.Parse(idLabel.text);

        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).cancelApply((UInt64)id);
    }


    class Item
    {
        public GameObject root;
        public UILabel id;
        public UISprite icon;
        public UILabel name;
        public UILabel level;
        public UILabel time;
        public UILabel state;

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
        countLabel.text = nCount.ToString() + "/" + nMaxFriend.ToString();

        for (int i = 0; i < nCount; ++i)
        {
            FriendInfo ff = friendList[i];

            if (ff.status != FRIEND_STATUS.FRIEND_STATUS_APPLY &&
                    ff.status != FRIEND_STATUS.FRIEND_STATUS_BE_APPLIED)
            {
                continue;
            }

            Item applyItem = new Item();

            applyItem.root = NGUITools.AddChild(grid, item);
            applyItem.root.name = i.ToString();
            applyItem.root.SetActive(true);

            applyItem.id = PanelTools.findChild<UILabel>(applyItem.root, "idLabel");
            applyItem.id.text = ff.userid.ToString();

            applyItem.icon = PanelTools.findChild<UISprite>(applyItem.root, "heroIcon");
            applyItem.icon.spriteName = ff.hero.portarait;

            applyItem.name = PanelTools.findChild<UILabel>(applyItem.root, "nameLabel");
            applyItem.name.text = ff.username;

            applyItem.level = PanelTools.findChild<UILabel>(applyItem.root, "levelLabel");
            applyItem.level.text = "LV."+ ff.level.ToString();


            applyItem.time = PanelTools.findChild<UILabel>(applyItem.root, "timeLabel");
            //TimeData td = DataManager.getModule<DataServTime>(DATA_MODULE.Data_ServTime).GetTimeDataByTime(ff.applyTime);
            //applyItem.time.text = td.hour.ToString() + ":" + td.minute.ToString() + ":" + td.second.ToString();
            applyItem.time.text = DataManager.getModule<DataServTime>(DATA_MODULE.Data_ServTime).getServTime((ulong)ff.applyTime, "yyyy-MM-dd HH:mm");


            applyItem.state = PanelTools.findChild<UILabel>(applyItem.root, "stateLabel");

            if (ff.status == FRIEND_STATUS.FRIEND_STATUS_APPLY)
            {
                applyItem.state.text = "申请中";

                GameObject acceptBtn = PanelTools.findChild(applyItem.root, "acceptButton");
                acceptBtn.SetActive(false);
                
                GameObject refuseBtn = PanelTools.findChild(applyItem.root, "refuseButton");
                UILabel btnLabel = PanelTools.findChild<UILabel>(refuseBtn, "btnLabel");
                btnLabel.text = "取消";
                UIEventListener.Get(refuseBtn).onClick = onCancel;
            }
            else
            {
                applyItem.state.text = "等待确认";

                GameObject acceptBtn = PanelTools.findChild(applyItem.root, "acceptButton");
                acceptBtn.SetActive(true);
                UIEventListener.Get(acceptBtn).onClick = onAccept;

                GameObject refuseBtn = PanelTools.findChild(applyItem.root, "refuseButton");
                UILabel btnLabel = PanelTools.findChild<UILabel>(refuseBtn, "btnLabel");
                btnLabel.text = "拒绝";
                UIEventListener.Get(refuseBtn).onClick = onRefuse;
            }

            itemList.Add(applyItem);
        }

        UIGrid uiGrid = grid.GetComponent<UIGrid>();
        uiGrid.repositionNow = true;
    }


}
