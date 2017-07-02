using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class SummonsMenu : MonoBehaviour
{

    GameObject browseBtn;
    GameObject strengthenBtn;
    GameObject breakBtn;
    GameObject returnBtn;
    GameObject closeBtn;


    // Use this for initialization
    void Start()
    {

        browseBtn = PanelTools.findChild(gameObject, "browseButton");
        if (browseBtn != null)
        {
            UIEventListener.Get(browseBtn).onClick = onBrowse;
        }

        strengthenBtn = PanelTools.findChild(gameObject, "StrengthenButton");
        if (strengthenBtn != null)
        {
            UIEventListener.Get(strengthenBtn).onClick = onStrengthen;
        }

        breakBtn = PanelTools.findChild(gameObject, "breakButton");
        if (breakBtn != null)
        {
            UIEventListener.Get(breakBtn).onClick = onBreak;
        }

        returnBtn = PanelTools.findChild(gameObject, "Titlebg/ReturnBtn");
        if (returnBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onReturn;
        }

        closeBtn = PanelTools.findChild(gameObject, "Titlebg/closeBtn");
        if (closeBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onClose;
        }
    }

    void onClose(GameObject go)
    {
        transform.parent.gameObject.SetActive(false);
    }

    void onReturn(GameObject go)
    {
        transform.parent.gameObject.SetActive(false);
    }


    void onBrowse(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowSummonsBrowse();
    }

    void onStrengthen(GameObject go)
    {
        //DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowSelectStrengthen();
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowStrengthenMenu();
    }

    void onBreak(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowSelectBreak();
    }
}
