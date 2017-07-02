using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using System;
using UI;

public class FriendFind : MonoBehaviour {

    GameObject returnBtn;
    GameObject findBtn;
    UILabel idLabel;
    
    // Use this for initialization
	void Start () {

        returnBtn = PanelTools.findChild(gameObject, "Title/btnBack");
        if (returnBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onReturn;
        }

        findBtn = PanelTools.findChild(gameObject, "Info/findButton");
        if (findBtn != null)
        {
            UIEventListener.Get(findBtn).onClick = onFind;
        }

        idLabel = PanelTools.findChild<UILabel>(gameObject, "Info/idLabel");

        ShowInfo();
	}

    void onReturn(GameObject go)
    {
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowPanelFriends();
    }

    void onFind(GameObject go)
    {
        UILabel applyID = PanelTools.findChild<UILabel>(gameObject, "Info/applyID");

        if (applyID.text != "" && applyID.text != "0")
        {
            UInt64 id = UInt64.Parse(applyID.text);
            DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).search(id);
        }

        //DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowFindUser();
    }

    public void ShowInfo()
    {
        idLabel.text = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.id.ToString();
    }
}
