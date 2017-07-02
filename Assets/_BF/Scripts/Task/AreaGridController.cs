using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;

public class AreaGridController : MonoBehaviour {

	/*public struct AreaInfo
	{
		public string name;
		public string desc;
	}*/

	public GameObject panelStageMgr;
	public GameObject panelStageMapPrefab;
	//public GameObject panelStageMap;
	public GameObject areaitem;
	public List<AreaInfo> areaInfoList;
	public GameObject panelAreaList;
	public GameObject bg;	
	public GameObject bgMask;
	private GameObject madeDrug;

	public GameObject panelStageMap;
	private List<GameObject> stageMapList;
	private UIGrid grid;
	private UISprite bgMaskSprite;
	private bool toDark;
	private float times;
	
	// Use this for initialization
	void Awake()
	{
	}
	
	void Start () {	
		toDark = false;
		areaInfoList = new List<AreaInfo>();
		grid = transform.GetComponent<UIGrid>();
		bgMaskSprite = bgMask.GetComponent<UISprite>();

		getDrugList();
		//iTween.ColorTo(bg,Color.red,2f);
		//iTween.MoveFrom(this.gameObject,iTween.Hash("x",3,"time",1));
	

	}

	void OnEnable()
	{
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, true, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, false, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, true, (int)EVENT_GROUP.mainUI);
	}



	void getDrugList()
	{
		#if S2_ONLYCLIENT
			AreaInfo strdungeon1 = new AreaInfo();
			strdungeon1.NAME = "米斯特拉尔地区";
			strdungeon1.DESC = "穿过路西亚斯之门，映入眼帘的是一片广阔的大地。那里被四堕神之一的创造神麦士威尔所支配着。";
			
			areaInfoList.Add(strdungeon1);
			
			AreaInfo strdungeon2 = new AreaInfo();
			strdungeon2.NAME = "克尔德里卡";
			strdungeon2.DESC = "爱克拉斯召唤院发现的新地区。也正因此，有很多没有调查的地方，也有传言说那里生存着大量危险魔物";

			areaInfoList.Add(strdungeon2);
		#else
		
			areaInfoList =  DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).getAreaInfoList();

		#endif
		//areaInfoList.Add(strdungeon2);
		
		addAreaItem();
	}
	
	void addAreaItem()
	{	
		for (int i = 0; i < areaInfoList.Count; i ++)
		{
			AreaInfo Info = areaInfoList[i];
			GameObject areaItemInfo = Instantiate(areaitem) as GameObject;

			//areaItemInfo.GetComponent<AreaItemController>().areaName.text = Info.NAME;		
			//areaItemInfo.GetComponent<AreaItemController>().desc.text = Info.DESC;
			//areaItemInfo.GetComponent<AreaItemController>().areaID = Info.AREA_ID;


			areaItemInfo.transform.Find("areaPnl/areaname").GetComponent<UILabel>().text = Info.NAME;			
			areaItemInfo.transform.Find("areaPnl/desc").GetComponent<UILabel>().text = Info.DESC;
			areaItemInfo.transform.Find("areaPnl/areaID").GetComponent<UILabel>().text = Info.AREA_ID.ToString(); 
			areaItemInfo.transform.parent = transform;
			grid.repositionNow = true;
			areaItemInfo.transform.localScale = Vector3.one;			
			UIEventListener.Get(areaItemInfo).onClick = ItemClick;
		}
	}
	
	void ItemClick(GameObject go)
	{
		times = 0;
		toDark = true;
		bgMask.SetActive(true);
		//iTween.ScaleTo(bg,iTween.Hash("x",10,"y",10,"z",0,"speed",2));	
		//iTween.MoveTo(bg,new Vector3(2,2,0),2);
		//bgSprite.color = Color.black;
		//iTween.ColorTo(bgSprite.gameObject,Color.blue,2);

	
		int areaid = Convert.ToInt32(go.transform.Find("areaPnl/areaID").GetComponent<UILabel>().text);
		StartCoroutine(setAreaActive(areaid));
	}

	IEnumerator setAreaActive(int areaID)
	{
		yield return new WaitForSeconds(1f);
		toDark = false;
		bgMask.SetActive(false);
		//bgSprite.color = Color.black;
		//iTween.ScaleTo(bg,iTween.Hash("x",1,"y",1,"z",0,"speed",1000));	
		//iTween.MoveTo(bg,new Vector3(0,0,0),0.0001f);
		//yield return new WaitForSeconds(0.5f);

		//UI.PanelStack.me.clear();

		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, false, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, false, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, false, (int)EVENT_GROUP.mainUI);

		UI.PanelStack.me.goNext(panelStageMap,onReturn);
		panelAreaList.SetActive(false);


		//panelStageMap.SetActive(true);

		/*panelStageMap = Instantiate(panelStageMapPrefab) as GameObject;
		panelStageMap.transform.parent = panelStageMgr.transform;		
		panelStageMap.SetActive(true);
		stageMapList.Add(panelStageMap);*/


		#if S2_ONLYCLIENT
			BaseLib.EventSystem.sendEvent((int)DataCenter.EVENT_GLOBAL.sys_chgScene,LoadSceneMgr.SCENE_BATTLE);
		#else
			panelStageMap.GetComponent<StageMgr>().initStageMgr(areaID);
		#endif


		//DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).battleStart((uint)1000,(uint)10000,0);
		
	}

	void onReturn(System.Object param)
	{
		panelAreaList.SetActive(true);
	}

	// Update is called once per frame
	void Update () {

		if (toDark)
		{
			times += Time.deltaTime;
			bgMaskSprite.alpha = times;
		}
		/*if (toDark)
		{
			bgSprite.color = Color.Lerp(Color.black,Color.white,Time.deltaTime*30);
		}

		if (!toDark)
			Debug.Log(bgSprite.color);*/
	}
}
