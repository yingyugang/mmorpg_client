using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;

public class subSaleController: MonoBehaviour {

	//public GameObject stuffSale;	
	private GameObject bgReturn;
	public StoneItem curItem;
	private UILabel ItemName,desc,amount,price,total,most,curNum;
	private UISlider slider;
	private bool init=false;
	private int cur;
	void Start () {
		if(init)
			return;
		UI.PanelTools.setBtnFunc(transform, "bgReturn/back", onReturnClick);
		UI.PanelTools.setBtnFunc(transform, "salePnl/btnSale", onSaleClick);
		bgReturn = UI.PanelStack.me.FindPanel ("Scale/NewVillage/PanelStuffMgr/PanelStuffSale/PanelStuffSubSale/bgReturn");
		//iTween.MoveFrom(bgReturn,iTween.Hash("x",-3,"time",1));

		ItemName = this.transform.Find ("stuffitem/StuffPnl/Name").GetComponent<UILabel> ();
		desc = this.transform.Find ("stuffitem/StuffPnl/desc").GetComponent<UILabel> ();
		amount = this.transform.Find ("salePnl/amount").GetComponent<UILabel> ();
		price = this.transform.Find ("salePnl/price").GetComponent<UILabel> ();
		total = this.transform.Find ("salePnl/total").GetComponent<UILabel> ();
		most = this.transform.Find ("salePnl/most").GetComponent<UILabel> ();
		curNum = this.transform.Find ("salePnl/curNum").GetComponent<UILabel> ();
		slider = this.transform.Find ("salePnl/Slider").GetComponent<UISlider> ();
	
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
		amount.text = curItem.amount.ToString();
		price.text = curItem.price.ToString();
		this.transform.Find("stuffitem/StuffPnl/bgicon/drugicon").GetComponent<UISprite>().spriteName = curItem.typeid.ToString();
	//	int temp=(int)DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).getItemCountByType((uint)curItem.typeid);
	//	amount.text = (curItem.amount + temp).ToString ();
	//	if(temp + curItem.amount > curItem.maxUsed)
	//		max = curItem.maxUsed;
	//	else
	//		max = temp + curItem.amount;
		//max =(int)( curItem.amount > curItem.maxUsed ? curItem.maxUsed : curItem.amount);
	//	maxUsed.text = max.ToString ()+"个";
		most.text = amount.text + "个";
		if(curItem.amount==1)
			slider.value=1;
		else
		{	
			slider.value = 1;
			cur = curItem.amount;
		}
		curNum.text = "1";
	}

	void Update () {
		cur = ((int)(slider.value * curItem.amount));
		if(cur<1)
			cur=1;
		curNum.text = cur.ToString ();
		total.text = (curItem.price * cur).ToString ();
	}
	void onSaleClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
		DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).saleItem ((uint)curItem.id, (uint)cur);
		//iTween.MoveTo(bgReturn,iTween.Hash("x",-3,"time",1));
		StartCoroutine("setDrugListActive");
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
	//	stuffSale.SetActive(true);
	//	iTween.MoveTo(bgReturn,iTween.Hash("x",-0.2,"time",1));
		transform.gameObject.SetActive(false);
	}
}
