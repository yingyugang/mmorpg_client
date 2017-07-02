using UnityEngine;
using System.Collections;
using DataCenter;
using BaseLib;
using System.Collections.Generic;
using UI;

public class panelMain : MonoBehaviour {

    public GameObject _header;
    public GameObject _body;
    public GameObject _footer;
    public GameObject _main;
	//private GameObject buyPower;

	void Start ()
    {
        EventSystem.register((int)EVENT_MAINUI.showMainUI, this.onshowMain, (int)EVENT_GROUP.mainUI);
        EventSystem.register((int)EVENT_MAINUI.showHeader, this.onShowHeader, (int)EVENT_GROUP.mainUI);
        EventSystem.register((int)EVENT_MAINUI.showBody, this.onShowBody, (int)EVENT_GROUP.mainUI);
        EventSystem.register((int)EVENT_MAINUI.showFooter, this.onShowFooter, (int)EVENT_GROUP.mainUI);
    }

 	void Update () 
    {

	}

    public static void showMain()
    {
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showMainUI, true, (int)EVENT_GROUP.mainUI);
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, true, (int)EVENT_GROUP.mainUI);
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, true, (int)EVENT_GROUP.mainUI);
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, true, (int)EVENT_GROUP.mainUI);
    }

    void onshowMain(int nEvent, System.Object param)
    {
        if (this._header != null)
            this._header.SetActive(false);
        if (this._body != null)
            this._body.SetActive(false);
        if (this._footer != null)
            this._footer.SetActive(false);
    }

    void onShowHeader(int nEvent, System.Object param)
    {
        bool flag = (bool)param;
        if (this._header != null)
            this._header.SetActive(flag);
    }

    void onShowBody(int nEvent, System.Object param)
    {
        bool flag = (bool)param;
        if (this._body != null)
            this._body.SetActive(flag);
    }

    void onShowFooter(int nEvent, System.Object param)
    {
        bool flag = (bool)param;
        if (this._footer != null)
            this._footer.SetActive(flag);
    }

	/*void onBuyPower(GameObject go)
	{
		UserInfo info = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser;
		Debug.Log(".........................onBuyPower...............................");
		if (info.curpower < info.maxpower)
		{
			DataManager.getModule<DataUser>(DATA_MODULE.Data_User).BuyPower();
		}
	}*/
}
