using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;

public class heroHandbookDetail: MonoBehaviour {
	
	private GameObject bgReturn;
	public HeroItem curItem;
	private UILabel ItemName,desc,NO;
	private bool init=false;
	private int cur;
	void Start () {
		if(init)
			return;
		UI.PanelTools.setBtnFunc(transform, "bgReturn/back", onReturnClick);
		bgReturn = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse/PanelHandbook/PanelDrug/PanelDrugDetail/bgReturn");
		
		ItemName = this.transform.Find ("stuffitem/StuffPnl/Name").GetComponent<UILabel> ();
		desc = this.transform.Find ("stuffitem/StuffPnl/desc").GetComponent<UILabel> ();
		NO = this.transform.Find ("stuffitem/StuffPnl/No").GetComponent<UILabel> ();

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
		NO.text = "NO."+ curItem.library.ToString();
		//this.transform.Find("stuffitem/StuffPnl/icon").GetComponent<UISprite>().spriteName = curItem.typeid.ToString();
	}
	
	
	void onReturnClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
	//	iTween.MoveTo(bgReturn,iTween.Hash("x",-3,"time",1));
		StartCoroutine("setDrugListActive");
	}
	
	IEnumerator setDrugListActive()
	{
		yield return new WaitForSeconds(0.5f);
	//	iTween.MoveTo(bgReturn,iTween.Hash("x",-0.2,"time",1));
		transform.gameObject.SetActive(false);
	}
}
