using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using System;
using UI;

public class FriendDetail : MonoBehaviour {

    GameObject returnBtn;
    GameObject delBtn;
    GameObject collectBtn;

    UInt64 friendId;
    bool isCollect;

    // Use this for initialization
	void Start () {

        returnBtn = PanelTools.findChild(gameObject, "Title/btnBack");
        if (returnBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onReturn;
        }

        delBtn = PanelTools.findChild(gameObject, "delButton");
        if (delBtn != null)
        {
            UIEventListener.Get(delBtn).onClick = onDel;
        }


        collectBtn = PanelTools.findChild(gameObject, "collectButton");
        if (collectBtn != null)
        {
            UIEventListener.Get(collectBtn).onClick = onCollect;
        }
	}

    void onReturn(GameObject go)
    {
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowFriendsList();
    }

    void onDel(GameObject go)
    {
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).delFriend(friendId);
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowFriendsList();
    }

    void onCollect(GameObject go)
    {
        //DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowFriendsList();
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).Collect(friendId);

//         if (isCollect)
//         {
//             DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).Collect(friendId);
//         }
//         else
//         {
//             DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).Collect(friendId);
//         }
    }

    public void ShowInfo(FriendInfo friend)
    {
        isCollect = friend.bCollect;
        
        uint nMaxFriend = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.maxFriend;
        FriendInfo[] friendList = DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).getFriendList();
        int nCount = friendList.Length;

        UILabel countLabel = PanelTools.findChild<UILabel>(gameObject, "Title/countLabel");
        countLabel.text = nCount.ToString() + "/" + nMaxFriend.ToString();
        
        
        UILabel userName = PanelTools.findChild<UILabel>(gameObject, "nameLabel");
        userName.text = friend.username;

        UILabel userLevel = PanelTools.findChild<UILabel>(gameObject, "levelLabel");
        userLevel.text = friend.level.ToString();

        UILabel userID = PanelTools.findChild<UILabel>(gameObject, "idLabel");
        friendId = friend.userid;
        userID.text = friend.userid.ToString();

        GameObject hero = PanelTools.findChild(gameObject, "hero");

        HeroInfo hi = friend.hero;

        UILabel heroName = PanelTools.findChild<UILabel>(hero, "heroName");
        heroName.text = hi.name;

        UILabel heroLevel = PanelTools.findChild<UILabel>(hero, "levelLabel");
        heroLevel.text = "LV." + hi.level.ToString();

        UILabel heroHp = PanelTools.findChild<UILabel>(hero, "hpLabel");
        heroHp.text = hi.hp.ToString();

        UILabel heroDef = PanelTools.findChild<UILabel>(hero, "defLabel");
        heroDef.text = hi.def.ToString();

        UILabel heroAtk = PanelTools.findChild<UILabel>(hero, "atkLabel");
        heroAtk.text = hi.atk.ToString();

        UILabel heorRe = PanelTools.findChild<UILabel>(hero, "reLabel");
        heorRe.text = hi.recover.ToString();

        UILabel rankLabel = PanelTools.findChild<UILabel>(hero, "rankLabel");
        rankLabel.text = friend.arenaHonor;
    }
}
