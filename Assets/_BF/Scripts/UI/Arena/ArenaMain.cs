using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;
public class ArenaMain : MonoBehaviour 
{
    public GameObject step1;

    public List<GameObject> ArenaPages;

	// Use this for initialization
	void Start () 
    {
        BaseLib.EventSystem.register((int)EVENT_MAINUI.showMainUI, onReturnMain, (int)EVENT_GROUP.mainUI);
        PanelTools.setBtnFunc(transform, "step1/Title/btnBack", onBack);
        step1 = PanelTools.findChild(gameObject, "step1");

        DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).arenaMain = this;

        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        showStep1();
    }

    void OnDisable()
    {
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, true, (int)EVENT_GROUP.mainUI);
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, true, (int)EVENT_GROUP.mainUI);
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, true, (int)EVENT_GROUP.mainUI);
    }

    void showStep1()
    {
        if (step1 == null)
            return;

        UI.PanelStack.me.clear();
        UI.PanelStack.me.goNext(step1);
    }

    void onReturnMain(int eventId, System.Object param)
    {
        gameObject.SetActive(false);
    }

    public void onBack(GameObject obj)
    {
        UI.PanelStack.me.clear();
        this.gameObject.SetActive(false);
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, true, (int)EVENT_GROUP.mainUI);
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, true, (int)EVENT_GROUP.mainUI);
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, true, (int)EVENT_GROUP.mainUI);
    }


    public void ShowPageByIndex(int nIndex)
    {
        gameObject.SetActive(true);
        int nCount = ArenaPages.Count;

        for (int i = 0; i < nCount; ++i)
        {
            if (i == nIndex)
            {
                ArenaPages[i].SetActive(true);
            }
            else
            {
                ArenaPages[i].SetActive(false);
            }
        }
    }
}
