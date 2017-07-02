using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using UI;

public class StageMgr: MonoBehaviour {

	public GameObject btnBack;
	/*public GameObject stage1;
	public GameObject stage2;
	public GameObject stage3;
	public GameObject stage4;*/

	public GameObject dungeonPrefabs;
	public GameObject btnWorld;
	public GameObject panelAreaMap;
	//public GameObject panelMain;

	public List<UISprite> _leftArrow;
	public List<UISprite> _rightArrow;
	public UILabel lblStageName;
	public GameObject battleItem;
	public GameObject mapBg;

	private UILabel battleID;
	private UILabel battleName;
	private int _nIconIndex = 0;
	private DungeonMgr dungeonmgr;

	void Awake()
	{

	}

	// Use this for initialization
	void Start () {
		//UIEventListener.Get(this.gameObject).onClick = StageClick;	

		dungeonmgr = dungeonPrefabs.GetComponent<DungeonMgr>();



		NGUITools.AddWidgetCollider(this.gameObject);
		UIEventListener.Get(btnBack).onClick = onBtnBackClick;	
		UIEventListener.Get(btnWorld).onClick = onBtnWorldClick;	
		/*UIEventListener.Get(stage1).onClick = onStageClick;	
		UIEventListener.Get(stage2).onClick = onStageClick;	
		UIEventListener.Get(stage3).onClick = onStageClick;	
		UIEventListener.Get(stage4).onClick = onStageClick;*/

		//dungeon = GameObject.Instantiate(dungeonPrefabs) as GameObject;
		//dungeon.SetActive(false);

	}

	
	// Update is called once per frame
	void Update () {
	
	}




	void onBtnBackClick(GameObject obj)
	{
		AudioManager.SingleTon().PlayMusic(AudioManager.SingleTon().MusicMainClip);
		//panelAreaMap.SetActive(true);
		//this.gameObject.SetActive(false);
		UI.PanelStack.me.goBack();

	}


	void onBattleClick(GameObject obj)
	{
		this.gameObject.SetActive(false);
		int battleID = Convert.ToInt32(obj.transform.Find("battleID").GetComponent<UILabel>().text);
		dungeonmgr.init(battleID);
		//dungeonPrefabs.SetActive(!dungeonPrefabs.activeSelf);
		//UI.PanelStack.me.root = this.gameObject;
		//UI.PanelStack.me.clear();

		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, true, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, false, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, true, (int)EVENT_GROUP.mainUI);
		UI.PanelStack.me.goNext(dungeonPrefabs,onReturn);




		/*if (!dungeon)
		{
			dungeon = GameObject.Instantiate(dungeonPrefabs) as GameObject;
			dungeon.transform.parent = transform.parent;
		}*/

		//dungeon.SetActive(true);
	}

	void onReturn(System.Object param)
	{
		this.gameObject.SetActive(true);
	}

	void onBtnWorldClick(GameObject obj)
	{
		this.gameObject.SetActive(false);
		panelAreaMap.SetActive(true);
	}

	void OnEnable()
	{		
		StartCoroutine(showArrow(0.5f));
	}

	IEnumerator showArrow(float fTime)
	{
		foreach (UISprite left in _leftArrow)
			left.gameObject.SetActive(false);
		foreach (UISprite right in _rightArrow)
			right.gameObject.SetActive(false);
		
		_leftArrow[_nIconIndex].gameObject.SetActive(true);
		_rightArrow[_nIconIndex].gameObject.SetActive(true);
		
		_nIconIndex++;
		_nIconIndex = _nIconIndex%3;
		yield return new WaitForSeconds(fTime);
		StartCoroutine(showArrow(0.5f));
	}

	public void initStageMgr(int areaID)
	{
		MapInfo mapinfo = DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).getMapdataByAreaID(areaID);
		lblStageName.text = mapinfo.NAME;

		//if (mapBg.transform.childCount == 0)

		for (int i = mapBg.transform.childCount -1; i >= 0; i--)
		{
			GameObject go = mapBg.transform.GetChild(i).gameObject;
			Destroy(go);
		}

		getBattleInfo(mapinfo.MAP_ID);
		this.gameObject.SetActive(true);
	}

	public void getBattleInfo(int mapID)
	{
		List<BattleInfo> battleInfo = DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).getBattledataByMapID(mapID);


		for(int i = 0; i < battleInfo.Count; i ++)
		{
			GameObject battleinfo = Instantiate(battleItem) as GameObject;
			battleName = PanelTools.findChild<UILabel>(battleinfo,"battleName");
			battleName.text = battleInfo[i].NAME;

			battleID = PanelTools.findChild<UILabel>(battleinfo,"battleID");
			battleID.text = battleInfo[i].BATTLE_ID.ToString();

			/*UICamera NGUICamera = Camera.main.GetComponent<UICamera>();
			Camera camera = NGUITools.FindCameraForLayer(this.gameObject.layer);
			Vector3 pos = camera.ScreenToWorldPoint(new Vector3(battleInfo[i].POSX,battleInfo[i].POSY,0));
			pos.z = 0;*/

			battleinfo.transform.parent = mapBg.transform;
			battleinfo.transform.localPosition = new Vector3(battleInfo[i].POSX,battleInfo[i].POSY,0);
            battleinfo.transform.localScale = new Vector3(0.7f,0.7f,0.7f);

			UIEventListener.Get(battleinfo).onClick = onBattleClick;	
		}
	
		/*battleName = PanelTools.findChild<UILabel>(stage1,"battleName");
		battleName.text = battleInfo[0].NAME;
		Debug.Log(battleName.text);
	
		battleName = PanelTools.findChild<UILabel>(stage2,"battleName");

		battleName.text = battleInfo[1].NAME;
		Debug.Log(battleName.text);
		battleName = PanelTools.findChild<UILabel>(stage3,"battleName");

		battleName.text = battleInfo[2].NAME;
		Debug.Log(battleName.text);
	
		battleName = PanelTools.findChild<UILabel>(stage4,"battleName");

		battleName.text = battleInfo[3].NAME;
		Debug.Log(battleName.text);*/
	
	}

	void OnDisable()
	{
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, true, (int)EVENT_GROUP.mainUI);
	}
}
