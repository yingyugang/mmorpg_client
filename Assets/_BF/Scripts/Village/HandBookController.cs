using UnityEngine;
using System.Collections;

public class HandBookController : MonoBehaviour {
	private GameObject bg;
	private GameObject drughandbook;
	private GameObject collecthouse;
	private GameObject herohandbook;

	
	void Start () {
		UI.PanelTools.setBtnFunc(transform, "Bg/title/back", onBackClick);
		UI.PanelTools.setBtnFunc(transform, "Bg/Drug", onDrugHandbook);
		UI.PanelTools.setBtnFunc(transform, "Bg/Hero", onHeroHandbook);

		collecthouse = UI.PanelStack.me.FindPanel("Scale/NewVillage/CollectionHouse/Bg"); 
		bg = UI.PanelStack.me.FindPanel("Scale/NewVillage/CollectionHouse/PanelHandbook/Bg");
		drughandbook = UI.PanelStack.me.FindPanel("Scale/NewVillage/CollectionHouse/PanelHandbook/PanelDrug"); 
		herohandbook = UI.PanelStack.me.FindPanel("Scale/NewVillage/CollectionHouse/PanelHandbook/PanelHeroHandbook");
	}
	void OnEnable()
	{
		if(bg != null)
			bg.SetActive (true);
		if(drughandbook != null)
			drughandbook.SetActive(false);

		if(herohandbook != null)
			herohandbook.SetActive (false);
	}

	void commonFun(GameObject obj1, GameObject obj2)
	{
		AudioManager.me.PlayBtnActionClip();
		obj1.SetActive (false);
		obj2.SetActive (true);
	}

	void onBackClick(GameObject go)
	{
		commonFun (this.gameObject,collecthouse);
	}
	
	void onDrugHandbook(GameObject go)
	{
		commonFun (bg,drughandbook);
	}
	
	void onHeroHandbook(GameObject go)
	{
		commonFun (bg,herohandbook);
	}

}
