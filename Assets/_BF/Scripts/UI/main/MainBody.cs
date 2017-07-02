using UnityEngine;
using System.Collections;
using DataCenter;
using System.Collections.Generic;

public class MainBody : MonoBehaviour {

    private GameObject cur;

    void Start()
    {
    }

    void Awake()
    {
		AudioManager.me.PlayMusic(AudioManager.me.MusicMainClip);
        UI.PanelTools.setBtnFunc(transform, "Menu", onMenu);
        UI.PanelTools.setBtnFunc(transform, "Gifts", onGift);
        UI.PanelTools.setBtnFunc(transform, "Announcement", onAnnounce);

        UI.PanelTools.setBtnFunc(transform, "bodyMove/Task", onTask);
        UI.PanelTools.setBtnFunc(transform, "bodyMove/SummonsYard", onSummonsYard);
        UI.PanelTools.setBtnFunc(transform, "bodyMove/Village", onVillage);
        UI.PanelTools.setBtnFunc(transform, "bodyMove/Arena", onArena);
        UI.PanelTools.setBtnFunc(transform, "bodyMove/SummoningMonsterBattle", onSummoningMonsterBattle);
        UI.PanelTools.setBtnFunc(transform, "bodyMove/ChaosGate", onChaosGate);
		
	}
	
    void setCurObj(GameObject obj)
    {
        AudioManager.me.PlayBtnActionClip();
        UI.PanelStack.me.clear();
        if (this.cur != null)
            this.cur.SetActive(false);
        cur = obj;
        if (cur != null)
        {
            cur.SetActive(true);
            //UI.PanelStack.me.goNext(cur, panelBackFunc);
        }
    }
        //回到主界面
    void panelBackFunc(System.Object param)
    {
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showMainUI, true, (int)EVENT_GROUP.mainUI);
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, true, (int)EVENT_GROUP.mainUI);
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, true, (int)EVENT_GROUP.mainUI);
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, true, (int)EVENT_GROUP.mainUI);
        setCurObj(null);
    }


    void onTask(GameObject obj)
    {
        panelMain.showMain();
		GameObject task = UI.PanelStack.me.FindPanel("Scale/PanelStageMgr/PanelAreaMap");
		if(task!=null)
		{
			setCurObj(task);
			AudioManager.SingleTon().PlayMusic(AudioManager.SingleTon().MusicTaskClip);
		}
        Debug.Log("onTask");

		UI.PanelStack.me.goNext("Scale/PanelStageMgr/PanelAreaMap");
    }

	void onVillage(GameObject obj)
    {
        panelMain.showMain();
		GameObject village = UI.PanelStack.me.FindPanel("Scale/NewVillage");
		if(village!=null)
			setCurObj(village);
		//Debug.Log("onVillage");
    }

	void onChaosGate(GameObject obj)
	{
        panelMain.showMain();
		Debug.Log("onChaosGate");	
		UI.PanelStack.me.goNext("Scale/PanelStageMgr/PanelChaos");			
	}

	void onSummonsYard(GameObject obj)
    {
        panelMain.showMain();
		Debug.Log("onSummonsYard");

        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowChallengePage();
    }

	void onSummoningMonsterBattle(GameObject obj)
	{
        panelMain.showMain();
		Debug.Log ("onSummoningMonsterBattle");
	}

    void onArena(GameObject obj)
    {
        panelMain.showMain();
        GameObject go = DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).arenaMain.gameObject;
        if (go != null)
            setCurObj(go);
        
        DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).ShowArenaMain();
    }

	void onAnnounce(GameObject obj)
    {
        AudioManager.me.PlayBtnActionClip();
        panelMain.showMain();
        DataManager.getModule<DataPost>(DATA_MODULE.Data_Post).ShowPlacard();
    }

    void onMenu(GameObject obj)
    {
        AudioManager.me.PlayBtnActionClip();
        panelMain.showMain();
		GameObject menu = UI.PanelStack.me.FindPanel("PanelMenu");
		if(menu!=null)
			setCurObj(menu);
    }
    
	void onGift(GameObject obj)
    {
        AudioManager.me.PlayBtnActionClip();
        panelMain.showMain();
        DataManager.getModule<DataPost>(DATA_MODULE.Data_Post).ShowPost();
    }
}
