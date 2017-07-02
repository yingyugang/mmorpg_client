using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class StrengthenMenu : MonoBehaviour
{

    GameObject levelBtn;
    GameObject skillBtn;
    GameObject qualityBtn;
    GameObject returnBtn;

    // Use this for initialization
    void Start()
    {

        levelBtn = PanelTools.findChild(gameObject, "levelBtn");
        if (levelBtn != null)
        {
            UIEventListener.Get(levelBtn).onClick = onLevelBtn;
        }

        skillBtn = PanelTools.findChild(gameObject, "skillBtn");
        if (skillBtn != null)
        {
            UIEventListener.Get(skillBtn).onClick = onSkillBtn;
        }

        qualityBtn = PanelTools.findChild(gameObject, "qualityBtn");
        if (qualityBtn != null)
        {
            UIEventListener.Get(qualityBtn).onClick = onQualityBtn;
        }

        returnBtn = PanelTools.findChild(gameObject, "Title/btnBack");
        if (returnBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onReturn;
        }
    }

    void onReturn(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowPanelSummons();
    }

    void onLevelBtn(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowSelectStrengthen();
    }

    void onSkillBtn(GameObject go)
    {
        //DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowSelectStrengthen();
        //DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowStrengthenMenu();
    }

    void onQualityBtn(GameObject go)
    {
        //DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowSelectBreak();


        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowSelectQualityStrengthen();
    }
}
