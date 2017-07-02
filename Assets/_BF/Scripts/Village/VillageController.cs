using UnityEngine;
using System.Collections;
using DataCenter;

public class VillageController : MonoBehaviour {

	public GameObject btnBack;
	public GameObject btnupdateVillage;
	public GameObject drug;
	public GameObject btnStone;
	public GameObject itemwarehouse;

	public GameObject updateVillage;
	public GameObject drugList;
	public GameObject stuffMgr;
	public GameObject stoneList;

	// Use this for initialization
	void Start () {

		UIEventListener.Get(btnBack).onClick = onBtnBackClick;
		UIEventListener.Get(btnupdateVillage).onClick = onUpdateVillageClick;
		UIEventListener.Get(drug).onClick = onDrugClick;
		UIEventListener.Get(btnStone).onClick = onStoneClick;
		UIEventListener.Get(itemwarehouse).onClick = onItemwarehouseClick;
		NGUITools.AddWidgetCollider(this.gameObject);
		BaseLib.EventSystem.register((int)EVENT_MAINUI.showMainUI, onReturnMain, (int)EVENT_GROUP.mainUI);
	}
	
	// Update is called once per frame
	void Update () {	
		//Debug.Log("village...................." + this.transform.gameObject.activeSelf);
	}

	void onBtnBackClick(GameObject go)
	{
		AudioManager.SingleTon().PlayMusic(AudioManager.SingleTon().MusicMainClip);
		this.transform.gameObject.SetActive(false);
	}

	void onUpdateVillageClick(GameObject go)
	{
		this.transform.gameObject.SetActive(false);	
		updateVillage.SetActive(true);
	}

	void onDrugClick(GameObject go)
	{
		this.transform.gameObject.SetActive(false);
		drugList.SetActive(true);
	}

	void onStoneClick(GameObject go)
	{
		this.transform.gameObject.SetActive(false);
		stoneList.SetActive(true);
	}

	void onItemwarehouseClick(GameObject go)
	{
		this.transform.gameObject.SetActive(false);
		stuffMgr.SetActive(true);
	}

	void onReturnMain(int eventId,System.Object param)
	{
		gameObject.SetActive(false);
	}

}
