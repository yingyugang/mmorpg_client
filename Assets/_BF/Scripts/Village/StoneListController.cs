using UnityEngine;
using System.Collections;
using DataCenter;
using BaseLib;

public class StoneListController : MonoBehaviour {

	private bool init = false;
	private GameObject village;
	private UILabel maxCount,curCount;
	private GameObject bgReturn;
	private GameObject StoneGrid;

	void Start () {
		if(init)
			return;
		UI.PanelTools.setBtnFunc(transform, "Above/dungeontitle/back", onReturnClick);
		StoneGrid = UI.PanelStack.me.FindPanel("Scale/NewVillage/PanelStoneList/Bg/Scroll View/Grid");
		bgReturn = UI.PanelStack.me.FindPanel("Scale/NewVillage/PanelStoneList/Above/dungeontitle");
		village  = UI.PanelStack.me.FindPanel("Scale/NewVillage/villagebg");
		curCount = this.transform.Find ("Above/ItemCount/curItem").GetComponent<UILabel> ();
		maxCount = this.transform.Find ("Above/ItemCount/maxItem").GetComponent<UILabel> ();
		EventSystem.register((int)EVENT_MAINUI.itemMake, onMake, (int)DataCenter.EVENT_GROUP.mainUI);
		init = true;
	}

	public void onMake(int nEvent,System.Object param)
	{
		show ();
	}

	void OnEnable()
	{
		Start ();
		show ();
	}

	void show()
	{
		uint curGrid = DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).getItemCount ();
		UserInfo info = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser;
		if(curGrid>=info.maxItem)
			curCount.color = Color.red;
		else
			curCount.color = Color.white;
		curCount.text = curGrid.ToString ();
		maxCount.text ="/"+ info.maxItem.ToString ();
	}

	void onReturnClick(GameObject go)
	{
		//iTween.MoveTo(bgReturn,iTween.Hash("x",-3,"time",1));
		AudioManager.me.PlayBtnActionClip();
		iTween.MoveTo(StoneGrid,iTween.Hash("x",3,"time",1));
		StartCoroutine("setStoneListActive");
	}

	IEnumerator setStoneListActive()
	{
		yield return new WaitForSeconds(0.5f);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, false, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, false, (int)EVENT_GROUP.mainUI);	
		transform.gameObject.SetActive(false);
		village.SetActive(true);
		//iTween.MoveTo(bgReturn,iTween.Hash("x",-0.2,"time",1));
		//iTween.MoveTo(StoneGrid,iTween.Hash("x",-0.1,"time",1));
	}

}
