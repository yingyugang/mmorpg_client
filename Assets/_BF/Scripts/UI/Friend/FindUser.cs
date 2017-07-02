using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using System;
using UI;

public class FindUser : MonoBehaviour {

    GameObject returnBtn;
    GameObject applyBtn;
    GameObject closeBtn;

    // Use this for initialization
	void Start () {

        returnBtn = PanelTools.findChild(gameObject, "Title/btnBack");
        if (returnBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onReturn;
        }

        applyBtn = PanelTools.findChild(gameObject, "applyButton");
        if (applyBtn != null)
        {
            UIEventListener.Get(applyBtn).onClick = onApply;
        }

        closeBtn = PanelTools.findChild(gameObject, "closeButton");
        if (closeBtn != null)
        {
            UIEventListener.Get(closeBtn).onClick = onClose;
        }
	}

    void onReturn(GameObject go)
    {
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowPanelFriends();
    }

    void onApply(GameObject go)
    {
        UILabel userID = PanelTools.findChild<UILabel>(gameObject, "idLabel");

        if (userID.text != "" && userID.text != "0")
        {
            UInt64 id = UInt64.Parse(userID.text);
            DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).apply(id);
        }
    }

    void onClose(GameObject go)
    {
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowPanelFriends();
    }

    public void ShowInfo(FriendInfo friend)
    {
        uint nMaxFriend = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.maxFriend;

        FriendInfo[] friendList = DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).getFriendList();
        int nCount = friendList.Length;

        UILabel countLabel = PanelTools.findChild<UILabel>(gameObject, "Title/countLabel");
        countLabel.text = nCount.ToString() + "/" + nMaxFriend.ToString();
        
        
        UILabel userName = PanelTools.findChild<UILabel>(gameObject, "nameLabel");
        userName.text = friend.username;

        UILabel userLevel = PanelTools.findChild<UILabel>(gameObject, "levelLabel");
        userLevel.text = "LV." + friend.level.ToString();

        UILabel userID = PanelTools.findChild<UILabel>(gameObject, "idLabel");
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
