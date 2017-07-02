using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;

public class UpdateVillage : MonoBehaviour {	

	private GameObject village;
	private GameObject bgReturn;
	private GameObject bgUpdate;
	private List<Vector3> posList;
	
	void Start () {
		village = UI.PanelStack.me.FindPanel("Scale/NewVillage/villagebg");
		bgReturn =  UI.PanelStack.me.FindPanel("Scale/NewVillage/NewlUpdateVillage/Bg/BgBack");
		bgUpdate =  UI.PanelStack.me.FindPanel("Scale/NewVillage/NewlUpdateVillage/Bg/BgUpdate");
		UI.PanelTools.setBtnFunc(transform, "Bg/BgBack/btnBack", onBackClick);
		NGUITools.AddWidgetCollider(this.gameObject);
		//iTween.MoveFrom(bgReturn,iTween.Hash("x",-3,"time",1));
	}

	void onBackClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
		StartCoroutine("setUpdateVillageActive");
	}

	IEnumerator setUpdateVillageActive()
	{
		yield return new WaitForSeconds(0.1f);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, false, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, false, (int)EVENT_GROUP.mainUI);
		transform.gameObject.SetActive(false);
		village.SetActive(true);
		//iTween.MoveTo(bgReturn,iTween.Hash("x",-0.2,"time",1));
		//iTween.MoveTo(bgUpdate,iTween.Hash("x",-0.1,"time",1));
	}
}
