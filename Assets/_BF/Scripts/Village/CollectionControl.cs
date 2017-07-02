using UnityEngine;
using System.Collections;
using DataCenter;

public class CollectionControl : MonoBehaviour {
	private GameObject bg;
	private GameObject illustratedHandbook;
	private GameObject villageMgr;
	private GameObject achievement;
	private GameObject storyline;
	private GameObject record;

	void Start () {
		UI.PanelTools.setBtnFunc(transform, "Bg/title/back", onBackClick);
		UI.PanelTools.setBtnFunc(transform, "Bg/achievement", onAchievement);
		UI.PanelTools.setBtnFunc(transform, "Bg/storyline", onBtoryline);
		UI.PanelTools.setBtnFunc(transform, "Bg/Record", onRecord);
		UI.PanelTools.setBtnFunc(transform, "Bg/handbook", onHandbook);

		bg = UI.PanelStack.me.FindPanel("Scale/NewVillage/CollectionHouse/Bg");
		villageMgr = UI.PanelStack.me.FindPanel("Scale/NewVillage/villagebg"); 
		illustratedHandbook = UI.PanelStack.me.FindPanel("Scale/NewVillage/CollectionHouse/PanelHandbook"); 
		achievement = UI.PanelStack.me.FindPanel("Scale/NewVillage/CollectionHouse/PaneAchievement");
		storyline = UI.PanelStack.me.FindPanel("Scale/NewVillage/CollectionHouse/PanelStoryline");
		record = UI.PanelStack.me.FindPanel("Scale/NewVillage/CollectionHouse/PanelRecord");
	}
	void OnEnable()
	{
		if(bg != null)
			bg.SetActive (true);
		if(illustratedHandbook != null)
			illustratedHandbook.SetActive(false);
		if(storyline != null)
			storyline.SetActive (false);
		if(achievement != null)
			achievement.SetActive (false);
	}

	void commonFun(GameObject obj1, GameObject obj2)
	{
		AudioManager.me.PlayBtnActionClip();
		obj1.SetActive (false);
		obj2.SetActive (true);
	}

	void onBackClick(GameObject go)
	{
		commonFun (this.gameObject,villageMgr);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, false, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, false, (int)EVENT_GROUP.mainUI);
	}
	
	void onHandbook(GameObject go)
	{
		commonFun (bg,illustratedHandbook);
	}
	
	void onAchievement(GameObject go)
	{
		commonFun (bg,achievement);
	}
	
	void onBtoryline(GameObject go)
	{
		commonFun (bg,storyline);
	}

	void onRecord(GameObject go)
	{
		commonFun (bg,record);	
	}
}
