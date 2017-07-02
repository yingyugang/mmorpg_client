using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
public class SuitUpController : MonoBehaviour {

	private GameObject PnlTeamList;
	//private GameObject PnlDrugTeam;

	public StoneItem curItem;
	private UILabel drugName,desc,amount,maxUsed,curNum;
	private UISlider slider;
	private bool init=false;
	private int max,cur;
	void Start () {
		if(init)
			return;
		UI.PanelTools.setBtnFunc(transform, "InfoPnl/btnSubmit", onSuitUp);
		UI.PanelTools.setBtnFunc(transform, "bgReturn/back", onBackClick);
		PnlTeamList = UI.PanelStack.me.FindPanel ("Scale/NewVillage/PanelStuffMgr/PanelDrugTeam/PanelTeamList");
	//	PnlDrugTeam = UI.PanelStack.me.FindPanel ("Scale/NewVillage/PanelStuffMgr/PanelDrugTeam");

		drugName = this.transform.Find ("stuffitem/StuffPnl/Name").GetComponent<UILabel> ();
		desc = this.transform.Find ("stuffitem/StuffPnl/desc").GetComponent<UILabel> ();
		amount = this.transform.Find ("InfoPnl/amount").GetComponent<UILabel> ();
		maxUsed = this.transform.Find ("InfoPnl/most").GetComponent<UILabel> ();
		curNum = this.transform.Find ("InfoPnl/curNum").GetComponent<UILabel> ();
		slider = this.transform.Find ("InfoPnl/Slider").GetComponent<UISlider> ();
		init = true;
	}
	void OnEnable()
	{
		Start ();
		showInfo ();
	}
	void showInfo()
	{
		drugName.text = curItem.Sname;
		desc.text = curItem.desc;
		int temp=(int)DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).getItemCountByType((uint)curItem.typeid);
		amount.text = (curItem.amount + temp).ToString ();
		if(temp + curItem.amount > curItem.maxUsed)
			max = curItem.maxUsed;
		else
			max = temp + curItem.amount;
		//max =(int)( curItem.amount > curItem.maxUsed ? curItem.maxUsed : curItem.amount);
		maxUsed.text = max.ToString ()+"个";
		slider.value = max;
		curNum.text = max.ToString ();
		this.transform.Find("stuffitem/StuffPnl/bgicon/drugicon").GetComponent<UISprite>().spriteName = curItem.typeid.ToString();
	}
	void Update () {
		cur = ((int)(slider.value * max));
		if(cur<1)
			cur=1;
		curNum.text = cur.ToString ();
	}
	void onSuitUp(GameObject go)
	{
		this.transform.gameObject.SetActive(false);
//		teamGridController.Iset [curItem.index].count = (uint)cur;
//		teamGridController.Iset [curItem.index].itemId = (uint)curItem.typeid;
		List<ItemSet> theList = new List<ItemSet> ();
		ItemSet item = new ItemSet ();
		item.index = (uint)curItem.index;
		item.itemId = (uint)curItem.id;
		item.count = (uint)cur;
		theList.Add(item);
		DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).setBattleItem (theList.ToArray());
		//PnlDrugTeam.SetActive (true);
		this.gameObject.SetActive (false);
		PnlTeamList.SetActive (false);
	}
	
	void onBackClick(GameObject go)
	{
		this.transform.gameObject.SetActive(false);
		PnlTeamList.SetActive(true);
	}
}
