using UnityEngine;
using System.Collections;
using DataCenter;

public class NewVillageController : MonoBehaviour {

	private GameObject bg;

	private GameObject updateVillage;
	private GameObject drugList;
	private GameObject stuffMgr;
	private GameObject stoneList;
	private GameObject collectHouse;

	// Use this for initialization
	void Start () {
		bg = UI.PanelStack.me.FindPanel("Scale/NewVillage/villagebg");
		updateVillage = UI.PanelStack.me.FindPanel("Scale/NewVillage/NewlUpdateVillage");
		drugList =UI.PanelStack.me.FindPanel("Scale/NewVillage/PanelDrugList");
		stuffMgr = UI.PanelStack.me.FindPanel("Scale/NewVillage/PanelStuffMgr");
		stoneList = UI.PanelStack.me.FindPanel("Scale/NewVillage/PanelStoneList");
		collectHouse = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse");

		//UI.PanelStack.me.goNext(drugTeam,this.onBack);
		UI.PanelTools.setBtnFunc(transform, "villagebg/btnBack", onBtnBackClick);
		UI.PanelTools.setBtnFunc(transform, "villagebg/btnupdateVillage", onUpdateVillageClick);
		UI.PanelTools.setBtnFunc(transform, "villagebg/drug", onDrugClick);
		UI.PanelTools.setBtnFunc(transform, "villagebg/Stone", onStoneClick);
		UI.PanelTools.setBtnFunc(transform, "villagebg/itemwarehouse", onItemwarehouseClick);
		UI.PanelTools.setBtnFunc (transform, "villagebg/collectHouse", onCollectHouse);

		NGUITools.AddWidgetCollider(this.gameObject);
		BaseLib.EventSystem.register((int)EVENT_MAINUI.showMainUI, onReturnMain, (int)EVENT_GROUP.mainUI);
	}

	void OnEnable()
	{
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, false, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, false, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, false, (int)EVENT_GROUP.mainUI);
		if (bg != null)
			bg.SetActive (true);
		if(updateVillage != null)
			updateVillage.SetActive(false);
		if(drugList!=null)
			drugList.SetActive(false);
		if(stuffMgr != null)
			stuffMgr.SetActive(false);
		if(stoneList != null)
			stoneList.SetActive(false);
		if(collectHouse != null)
			collectHouse.SetActive(false);
	}
	void OnDisable()
	{
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, true, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, true, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, true, (int)EVENT_GROUP.mainUI);
	}

	void onBtnBackClick(GameObject go)
	{
		AudioManager.SingleTon().PlayMusic(AudioManager.SingleTon().MusicMainClip);
		this.transform.gameObject.SetActive(false);
	}

	void onUpdateVillageClick(GameObject go)
	{
		commonOperation (updateVillage);
	}

	void onDrugClick(GameObject go)
	{
		commonOperation (drugList);
	}

	void onStoneClick(GameObject go)
	{
		commonOperation (stoneList);
	}

	void onItemwarehouseClick(GameObject go)
	{
		commonOperation (stuffMgr);
	}


	void onCollectHouse(GameObject go)
	{
		commonOperation (collectHouse);
	}

	void onReturnMain(int eventId,System.Object param)
	{
		gameObject.SetActive(false);
	}

	void commonOperation(GameObject obj)
	{
		AudioManager.me.PlayBtnActionClip();
		bg.SetActive (false);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, true, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, true, (int)EVENT_GROUP.mainUI);	
		obj.SetActive(true);
	}
}
