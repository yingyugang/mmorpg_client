using System;
using UnityEngine;
using System.Collections;
using DataCenter;

public class AreaMgr : MonoBehaviour {

	public GameObject btnReturn;
	public GameObject bg;

	public GameObject btnTarget;
	public UITexture bgFade;
	public GameObject panelStageMgr;
	// Use this for initialization	
	void Start () {
		//iTween.ScaleTo(bg,new Vector3(100,10,0),5);	
		UIEventListener.Get(btnReturn).onClick = onReturnClick;
		NGUITools.AddWidgetCollider(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void onReturnClick(GameObject go)
	{

		this.gameObject.SetActive(false);

		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, true, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, true, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, true, (int)EVENT_GROUP.mainUI);

		//iTween.ScaleTo(bg,new Vector3(50,50,0),3);	
		//iTween.ScaleTo(bg,iTween.Hash("x",10,"y",10,"z",0,"speed",3));	
		//iTween.MoveTo(bg,new Vector3(5,5,0),3);
		//StartCoroutine("setAreaActive");
	}

	IEnumerator setAreaActive()
	{
		yield return new WaitForSeconds(2f);
		//this.gameObject.SetActive(false);
		//panelStageMgr.gameObject.SetActive(true);	
		//iTween.ScaleTo(bg,iTween.Hash("x",1,"y",1,"z",0,"speed",1000));	
		//iTween.MoveTo(bg,new Vector3(0,0,0),0.1f);
	}
}
