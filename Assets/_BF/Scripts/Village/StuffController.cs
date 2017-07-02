using UnityEngine;
using System.Collections;
using DataCenter;

public class StuffController : MonoBehaviour {
	private GameObject bg;
	private GameObject stuffList;
	private GameObject villageMgr;
	private GameObject drugTeam;
	private GameObject stuffSale;

	void Start () {
		UI.PanelTools.setBtnFunc(transform, "Bg/stonetitle/back", onBackClick);
		UI.PanelTools.setBtnFunc(transform, "Bg/btnStuffList", onBtnStuffList);
		UI.PanelTools.setBtnFunc(transform, "Bg/btnTeam", onBtnTeam);
		UI.PanelTools.setBtnFunc(transform, "Bg/btnSale", onBtnSale);

		bg = UI.PanelStack.me.FindPanel("Scale/NewVillage/PanelStuffMgr/Bg");
		stuffList = UI.PanelStack.me.FindPanel("Scale/NewVillage/PanelStuffMgr/PanelStuffList"); 
		villageMgr = UI.PanelStack.me.FindPanel("Scale/NewVillage/villagebg"); 
		drugTeam = UI.PanelStack.me.FindPanel("Scale/NewVillage/PanelStuffMgr/PanelDrugTeam");
		stuffSale = UI.PanelStack.me.FindPanel("Scale/NewVillage/PanelStuffMgr/PanelStuffSale");
	}

	void OnEnable()
	{
		if(bg != null)
			bg.SetActive (true);
		if(stuffList != null)
			stuffList.SetActive(false);
		if(stuffSale != null)
			stuffSale.SetActive (false);
		if(drugTeam != null)
			drugTeam.SetActive (false);
	}

	void CommonFun(GameObject obj1 , GameObject obj2)
	{
		AudioManager.me.PlayBtnActionClip();
		obj1.SetActive (false);
		obj2.SetActive (true);
	}

	void onBackClick(GameObject go)
	{
		CommonFun (this.gameObject,villageMgr);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, false, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, false, (int)EVENT_GROUP.mainUI);
	}

	void onBtnStuffList(GameObject go)
	{
		CommonFun (bg,stuffList);
	}

	void onBtnTeam(GameObject go)
	{
		CommonFun (bg, drugTeam);
	}

	void onBtnSale(GameObject go)
	{
		CommonFun (bg,stuffSale);
	}
}
