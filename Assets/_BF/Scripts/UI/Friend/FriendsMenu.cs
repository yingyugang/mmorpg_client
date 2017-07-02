using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class FriendsMenu : MonoBehaviour
{

    GameObject ListBtn;
    GameObject ApplyBtn;
    GameObject FindBtn;
    GameObject GiftBtn;

    // Use this for initialization
    void Start()
    {

        ListBtn = PanelTools.findChild(gameObject, "Buttons/listButton");
        if (ListBtn != null)
        {
            UIEventListener.Get(ListBtn).onClick = onList;
        }

        ApplyBtn = PanelTools.findChild(gameObject, "Buttons/applyButton");
        if (ApplyBtn != null)
        {
            UIEventListener.Get(ApplyBtn).onClick = onApply;
        }

        FindBtn = PanelTools.findChild(gameObject, "Buttons/findButton");
        if (FindBtn != null)
        {
            UIEventListener.Get(FindBtn).onClick = onFind;
        }

        GiftBtn = PanelTools.findChild(gameObject, "Buttons/giftButton");
        if (GiftBtn != null)
        {
            UIEventListener.Get(GiftBtn).onClick = onGift;
        }
    }

    void onList(GameObject go)
    {
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowFriendsList();
    }

    void onApply(GameObject go)
    {
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowFriendApply();
    }

    void onFind(GameObject go)
    {
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowFriendFind();
    }

    void onGift(GameObject go)
    {
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowFriendGift();
    }
}
