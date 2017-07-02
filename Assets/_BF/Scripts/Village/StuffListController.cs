using UnityEngine;
using System.Collections;
using DataCenter;
using BaseLib;

public class StuffListController : MonoBehaviour {

	private GameObject btnReturn;
	private GameObject stuffMgr;
	
	private GameObject bgReturn;

	private UILabel maxCount,curCount;
	private bool ini=false;

	void Start () {
		if(ini)
			return;
		UI.PanelTools.setBtnFunc(transform, "Above/bgReturn/back", onReturnClick);
		bgReturn = UI.PanelStack.me.FindPanel("Scale/NewVillage/PanelStuffMgr/PanelStuffList/Above/bgReturn");
		stuffMgr = UI.PanelStack.me.FindPanel ("Scale/NewVillage/PanelStuffMgr/Bg");
		//iTween.MoveFrom(bgReturn,iTween.Hash("x",-3,"time",1));
		curCount = this.transform.Find ("Above/ItemCount/curItem").GetComponent<UILabel> ();
		maxCount = this.transform.Find ("Above/ItemCount/maxItem").GetComponent<UILabel> ();
		ini = true;
	}

	void OnEnable () {
		Start ();
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
		AudioManager.me.PlayBtnActionClip();
		//iTween.MoveTo(bgReturn,iTween.Hash("x",-3,"time",1));
		StartCoroutine("setDrugListActive");
	}
	
	IEnumerator setDrugListActive()
	{
		yield return new WaitForSeconds(0.5f);
		stuffMgr.SetActive(true);
		//iTween.MoveTo(bgReturn,iTween.Hash("x",-0.2,"time",1));
		transform.gameObject.SetActive(false);
	}
}
