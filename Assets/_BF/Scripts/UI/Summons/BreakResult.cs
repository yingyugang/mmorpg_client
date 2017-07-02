using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class BreakResult : MonoBehaviour
{
    GameObject returnBtn;

    // Use this for initialization
	void Start () {

        returnBtn = PanelTools.findChild(gameObject, "Title/returnButton");

        if (returnBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onReturn;
        }
        
	}

    void OnEnable()
    {
        ShowBreak();
    }

    void onReturn(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowSelectBreak();
    }

    public void ShowBreak()
    {
        SummonInfo si = DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).newSummon;

        UILabel lvLabel = PanelTools.findChild<UILabel>(gameObject, "info/lvLabel");
        lvLabel.text = si.level.ToString();

        UILabel hpLabel = PanelTools.findChild<UILabel>(gameObject, "info/hpLabel");
        hpLabel.text = si.hp.ToString();

        UILabel reLabel = PanelTools.findChild<UILabel>(gameObject, "info/reLabel");
        reLabel.text = si.recover.ToString();

        UILabel atkLabel = PanelTools.findChild<UILabel>(gameObject, "info/atkLabel");
        atkLabel.text = si.atk.ToString();

        UILabel defLabel = PanelTools.findChild<UILabel>(gameObject, "info/defLabel");
        defLabel.text = si.def.ToString();

        UILabel inhLabel = PanelTools.findChild<UILabel>(gameObject, "info/inhLabel");
        inhLabel.text = si.inherit.ToString() + "%";
    }
}
