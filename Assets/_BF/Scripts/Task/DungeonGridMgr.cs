using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;

public class DungeonGridMgr : MonoBehaviour {

	public struct dungeonInfo
	{
		public string name;
		public string power;
		public string time;
		public string desc;
		public bool isPass;
	}

	public GameObject panelHelperList;
	public GameObject panelDungeon;
	public GameObject dungeonItem;
	public List<dungeonInfo> dungInfo;
	public GameObject bgGrid;
	private UIGrid grid;
	private List<FieldInfo> fieldInfoList;
	public GameObject panelFriendPartner;

	// Use this for initialization
	void Awake()
	{
		//Debug.Log("test");
	}

	void Start () {	
		dungInfo = new List<dungeonInfo>();
		
		//iTween.MoveFrom(this.gameObject,iTween.Hash("x",3,"time",1));		
		iTween.MoveFrom(bgGrid,iTween.Hash("x",3,"time",1));		


		panelDungeon = GameObject.Find("PanelDungeon").gameObject;
		//panelFriendPartner = GameObject.Find("PanelPartner/FriendPartner").gameObject;
		//Invoke("getDungeonList",0.5f);  
		//grid = gameObject.transform.GetComponentInChildren<UIGrid>();
	}

	public void Init(int battleID)
	{
		if (!grid)
			grid = transform.GetComponent<UIGrid>();
		//grid.GetChildList().Clear();
		for (int i = grid.transform.childCount -1; i >= 0; i--)
		{
			GameObject go = grid.transform.GetChild(i).gameObject;
			Destroy(go);
		}
		//grid.GetChildList().Release();
		fieldInfoList = DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).getFieldDataByBattleID(battleID);
		addDungeonItem();

	}

	void addDungeonItem()
	{	
		//for (int i = 0; i < dungInfo.Count; i ++)
		for (int i = fieldInfoList.Count -1; i  >= 0; i --)
		{
			FieldInfo Info = fieldInfoList[i];

			/*GameObject dungItem = null;
			if (!grid.transform.GetChild(i))			
				dungItem = Instantiate(dungeonItem) as GameObject;
			else
				dungeonItem = grid.transform.GetChild(i).gameObject;*/

			GameObject dungItem = Instantiate(dungeonItem) as GameObject;

			//dungItem.GetComponent<UILabel>().text = Info.name;
			if (dungItem.transform.Find("dungeonPnl/dungeonname"))
					dungItem.transform.Find("dungeonPnl/dungeonname").GetComponent<UILabel>().text = Info.NAME;

			dungItem.transform.Find("dungeonPnl/power").GetComponent<UILabel>().text = "Consume:" + Info.COST_ENARGY.ToString();
			dungItem.transform.Find("dungeonPnl/time").GetComponent<UILabel>().text = "Battles:" + Info.MAX_STEP.ToString();
			dungItem.transform.Find("dungeonPnl/desc").GetComponent<UILabel>().text = Info.DESC;
			dungItem.transform.Find("dungeonPnl/dungeonID").GetComponent<UILabel>().text = Info.FIELD_ID.ToString();
												
			dungItem.transform.Find("dungeonPnl/PanelNewDungeonText").gameObject.SetActive(Info.STATE == TASK_STATE.NEW? true: false);
			dungItem.transform.Find("dungeonPnl/state").gameObject.SetActive(Info.STATE != TASK_STATE.NEW? true: false);

			/*if (Info.isPass)
			{
				dungItem.transform.Find("dungeonPnl/PanelNewDungeonText").gameObject.SetActive(false);
				dungItem.transform.Find("dungeonPnl/state").gameObject.SetActive(true);
			}
			else
			{
				dungItem.transform.Find("dungeonPnl/PanelNewDungeonText").gameObject.SetActive(true);
				dungItem.transform.Find("dungeonPnl/state").gameObject.SetActive(false);
			}*/

			dungItem.transform.parent = grid.transform;
			dungItem.transform.localScale = Vector3.one;
			UIEventListener.Get(dungItem).onClick = ItemClick;
		}

		/*for (int j = fieldInfoList.Count; j < grid.transform.childCount -1; j++)
		{
			grid.transform.GetChild(j).gameObject.SetActive(false);
		}*/
		grid.repositionNow = true;
	}

	void ItemClick(GameObject go)
	{
		//Application.LoadLevel("Battle");
		//Debug.Log(go.transform.Find("dungeonPnl/dungeonname").GetComponent<UILabel>().text);

		int fieldID = Convert.ToInt32(go.transform.Find("dungeonPnl/dungeonID").GetComponent<UILabel>().text);
		DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).getEnemyInfoList(fieldID);
		DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).currFieldID = fieldID;
		Debug.Log((uint)fieldID);
		//DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).battleStart((uint)fieldID,(uint)10000);
	

		panelDungeon.SetActive(false);		

		UI.PanelStack.me.goNext(panelFriendPartner,onReturn);
		DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).ShowFriendPartner();  		





		//panelHelperList.SetActive(true);

        //BaseLib.EventSystem.sendEvent((int)DataCenter.EVENT_GLOBAL.sys_chgScene,LoadSceneMgr.SCENE_BATTLE);

		/*List<FieldEnemy> fieldEnemy = DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).getEnemyInfoList(fieldID);
		for (int i = 0; i < fieldEnemy.Count; i++)
		{
			Debug.Log("波次.........." + fieldEnemy[i].STEP);
			//Debug.Log(fieldEnemy[i].FIELD_ID);
			for (int j = 0; j < fieldEnemy[i].EnemyInfo.Count; j++)
			{
				Debug.Log("怪物类型............." + fieldEnemy[i].EnemyInfo[j].MONSTER_TYPEID);
				Debug.Log("怪物位置............." + fieldEnemy[i].EnemyInfo[j].MONSTER_LOCATION);
			}
     		}*/
	}

	void onReturn(System.Object param)
	{
		this.gameObject.SetActive(true);
	}



	// Update is called once per frame
	void Update () {
	
	}
}
