using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class PanelPost : MonoBehaviour
{

    public List<GameObject> PostPages;

    // Use this for initialization
	void Start () {

        DataManager.getModule<DataPost>(DATA_MODULE.Data_Post).panelPost = this;
 
        InitUI();
        BaseLib.EventSystem.register((int)EVENT_MAINUI.showMainUI, onReturnMain, (int)EVENT_GROUP.mainUI);
        gameObject.SetActive(false);
    }

    void onReturnMain(int eventId, System.Object param)
    {
        InitUI();
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
        foreach (GameObject go in PostPages)
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
        int nCount = PostPages.Count;

        for (int i = 0; i < nCount; ++i)
        {
            if (i == nIndex)
            {
                PostPages[i].SetActive(true);
            }
            else
            {
                PostPages[i].SetActive(false);
            }
        }
    }
}
