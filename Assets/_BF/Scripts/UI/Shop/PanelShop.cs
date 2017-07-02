using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class PanelShop : MonoBehaviour
{

    public List<GameObject> ShopPages;

    
    // Use this for initialization
	void Start () {

        DataManager.getModule<DataShop>(DATA_MODULE.Data_Shop).panelShop = this;

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
        foreach (GameObject go in ShopPages)
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
        int nCount = ShopPages.Count;

        for (int i = 0; i < nCount; ++i)
        {
            if (i == nIndex)
            {
                ShopPages[i].SetActive(true);
            }
            else
            {
                ShopPages[i].SetActive(false);
            }
        }
    }
}
