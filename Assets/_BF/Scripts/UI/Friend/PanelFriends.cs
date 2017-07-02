using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class PanelFriends : MonoBehaviour
{

    public List<GameObject> FriendPages;

    
    // Use this for initialization
	void Start () {

        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).panelFriends = this;

        PanelTools.setBtnFunc(transform, "menu/closeButton", onReturn);
        InitUI();
        BaseLib.EventSystem.register((int)EVENT_MAINUI.showMainUI, onReturnMain, (int)EVENT_GROUP.mainUI);
        gameObject.SetActive(false);
    }

    void onReturnMain(int eventId, System.Object param)
    {
        InitUI();
        gameObject.SetActive(false);
    }

    void onReturn(GameObject go)
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, false, (int)EVENT_GROUP.mainUI);
    }

    void OnDisable()
    {
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, true, (int)EVENT_GROUP.mainUI);
    }

    public void InitUI()
    {
        foreach (GameObject go in FriendPages)
        {
            if (go.name == "menu")
            {
                go.SetActive(true);
            }
            else
            {
                go.SetActive(false);
            }
        }
    }

    public void ShowPageByIndex(int nIndex)
    {
        gameObject.SetActive(true);
        int nCount = FriendPages.Count;

        for (int i = 0; i < nCount; ++i)
        {
            if (i == nIndex)
            {
                FriendPages[i].SetActive(true);
            }
            else
            {
                FriendPages[i].SetActive(false);
            }
        }
    }
}
