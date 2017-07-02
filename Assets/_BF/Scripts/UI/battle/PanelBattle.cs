using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class PanelBattle : MonoBehaviour
{

    public GameObject Result;

    // Use this for initialization
	void Start () {
		if (DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).isShowResult)
		{
			EventSystem.register((int)EVENT_MAINUI.battleReward, onBattleWin, (int)DataCenter.EVENT_GROUP.mainUI);
			int fieldId = DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).currFieldID;
			BattleResultInfo bri = DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).GetBattleResult();
			DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).battleEnd((uint)fieldId,(uint)bri.remainHp,(uint)bri.gold,(uint)bri.soul);//TODO
			Result.SetActive(true);
			BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, false, (int)EVENT_GROUP.mainUI);
		}

    }

	void onBattleWin(int nEvent, System.Object param)
	{
		if (DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).isShowResult)
		{
			Debug.Log(Result);
//			BattleResult br = Result.GetComponent<BattleResult>();
//			DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).
			BattleResult battleResult = Result.GetComponent<BattleResult>();
//			battleResult.Ex
			DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).GetBattleResult();
			battleResult.UpdateUI();
		}
		else
		{
			Result.SetActive(false);
		}
		Debug.Log("onBattleWin");
	}

	// Update is called once per frame
	void Update () {
        
	}

    public void InitUI()
    {
        Result.SetActive(false);
    }
}
