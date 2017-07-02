using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;

public class drughandBookDetail: MonoBehaviour {

	private GameObject bgReturn;
	public StoneItem curItem;
	private UILabel ItemName,desc,desc_ex;
	private bool init=false;
	private int cur;
	void Start () {
		if(init)
			return;
		UI.PanelTools.setBtnFunc(transform, "bgReturn/back", onReturnClick);
		//bgReturn = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse/PanelHandbook/PanelDrug/PanelDrugDetail/bgReturn");
		bgReturn = this.transform.FindChild ("bgReturn").gameObject;
		ItemName = this.transform.Find ("stuffitem/StuffPnl/Name").GetComponent<UILabel> ();
		desc = this.transform.Find ("stuffitem/StuffPnl/desc").GetComponent<UILabel> ();
		desc_ex = this.transform.Find ("stuffitem/StuffPnl/desc_ex").GetComponent<UILabel> ();
		
	}
	
	void OnEnable()
	{
		Start ();
		showInfo ();
	}
	
	void showInfo()
	{
		ItemName.text = curItem.Sname;
		desc.text = curItem.desc;
		desc_ex.text = curItem.desc_ex;
		this.transform.Find("stuffitem/StuffPnl/bgicon/drugicon").GetComponent<UISprite>().spriteName = curItem.typeid.ToString();
	}


	void onReturnClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
		//iTween.MoveTo(bgReturn,iTween.Hash("x",-3,"time",1));
		StartCoroutine("setDrugListActive");
	}
	
	IEnumerator setDrugListActive()
	{
		yield return new WaitForSeconds(0.5f);
	//	iTween.MoveTo(bgReturn,iTween.Hash("x",-0.2,"time",1));
		transform.gameObject.SetActive(false);
	}
}
