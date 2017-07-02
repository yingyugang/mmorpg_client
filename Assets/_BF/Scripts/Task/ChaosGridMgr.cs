using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;

public class ChaosGridMgr : MonoBehaviour {

	public struct chaosInfo
	{
		public string name;
		public string power;
		public string time;
		public string desc;
		public bool isPass;
	}

	private GameObject paneldungeon;
	public GameObject panelChaos;
	public GameObject chaosItemPrefab;
	private UIGrid grid;
	private List<BattleInfo> chaosInfoList;
	private UserInfo userInfo;
	private uint keyCount;
	private DungeonMgr dungeonmgr;

	// Use this for initialization
	void Awake()
	{

	}

	void Start () {	

		/*DayOfWeek week =  System.DateTime.Now.DayOfWeek;
		DateTime nextDate = System.DateTime.Now;
		TimeSpan ts1 = new TimeSpan(System.DateTime.Now.Ticks);
		TimeSpan ts2 = new TimeSpan(nextDate.Ticks);
		TimeSpan ts = ts2.Subtract(ts1).Duration();
		Debug.Log("nextDate......................................" + nextDate.ToString());
		Debug.Log("ts......................................" + ts.ToString());*/
		

		panelChaos = GameObject.Find("PanelChaos").gameObject;
			
		//Camera
		paneldungeon =  UI.PanelStack.me.FindPanel("Scale/PanelStageMgr/PanelDungeon").gameObject;
		dungeonmgr = paneldungeon.GetComponent<DungeonMgr>();
		//Init();
	}

	public void Init()
	{	

		/*DateTime dt = DateTime.Now;
		DateTime newdt = dt.AddDays(1);
		DateTime nextdt = Convert.ToDateTime(newdt.Year + "-" + newdt.Month + "-" + newdt.Day + " 00:00:00");
		TimeSpan ts = nextdt.Subtract(dt);
		Debug.Log(ts.ToString());*/


		/*TimeSpan ts3 = DateTime.Now.TimeOfDay;
		TimeSpan ts4 = new TimeSpan(time.Ticks);
		TimeSpan ts5 = ts3.Subtract(ts4).Duration();
		Debug.Log(ts5.ToString());*/


		if (!grid)
			grid = transform.GetComponent<UIGrid>();
		//grid.GetChildList().Clear();
		for (int i = grid.transform.childCount -1; i >= 0; i--)
		{
			GameObject go = grid.transform.GetChild(i).gameObject;
			Destroy(go);
		}

		chaosInfoList = DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).getChaosInfoList();
		addChaosItem();

	}

	void addChaosItem()
	{	
		for (int i = chaosInfoList.Count -1; i  >= 0; i --)
		{
			BattleInfo Info = chaosInfoList[i];
			//每日活动，定期活动
			if (Info.STYLE == 2 || Info.STYLE == 3)
			{			

				if (Info.STYLE == 3)
				{
					//每周几，开启的活动副本
					DayOfWeek week =  System.DateTime.Now.DayOfWeek;
					if ((int)week != Info.COND_PARAM1)
						continue;
				}

				GameObject dungItem = Instantiate(chaosItemPrefab) as GameObject;
				ChaosInfoMgr chaoinfo = dungItem.GetComponent<ChaosInfoMgr>();


				if (dungItem.transform.Find("chaosPnl/name"))
				{
					//dungItem.transform.Find("chaosPnl/name").GetComponent<UILabel>().text = Info.NAME;
					if (Info.STYLE == 2)
                        dungItem.transform.Find("chaosPnl/name").GetComponent<UILabel>().text = Info.NAME;
                        
					else
                        dungItem.transform.Find("chaosPnl/name").GetComponent<UILabel>().text = Info.NAME;
				}


				if (dungItem.transform.Find("chaosPnl/time"))
					dungItem.transform.Find("chaosPnl/time").GetComponent<UILabel>().text = "";

				if (dungItem.transform.Find("chaosPnl/tips"))
					dungItem.transform.Find("chaosPnl/tips").GetComponent<UILabel>().text = "";

				//dungItem.transform.Find("chaosPnl/desc").GetComponent<UILabel>().text = Info.DESC;
				dungItem.transform.Find("chaosPnl/chaosID").GetComponent<UILabel>().text = Info.BATTLE_ID.ToString();
												
				//dungItem.transform.Find("chaosPnl/state").gameObject.SetActive(Info.STATE != TASK_STATE.NEW? true: false);

				if (Info.STYLE == 3)
				{
					if (dungItem.transform.Find("chaosPnl/time"))
					{
						DateTime dt = DateTime.Now;
						DateTime newdt = dt.AddDays(1);
						DateTime nextdt = Convert.ToDateTime(newdt.Year + "-" + newdt.Month + "-" + newdt.Day + " 00:00:00");
						TimeSpan ts = nextdt.Subtract(dt);											
						dungItem.transform.Find("chaosPnl/time").GetComponent<UILabel>().text = "还有" + ts.Hours.ToString() + "小时";
					}
					if (dungItem.transform.Find("chaosPnl/tips"))
					{
						dungItem.transform.Find("chaosPnl/tips").GetComponent<UILabel>().text = getWeekName((DayOfWeek)Info.COND_PARAM1);
					}
				}



				chaoinfo.chaosID = Info.BATTLE_ID;
				chaoinfo.style = Info.STYLE;
				chaoinfo.condition = Info.COND;
				chaoinfo.cond_param1 = Info.COND_PARAM1;
				chaoinfo.cond_param2 = Info.COND_PARAM2;

				dungItem.transform.parent = grid.transform;
				dungItem.transform.localScale = Vector3.one;
				UIEventListener.Get(dungItem).onClick = ItemClick;
			}
		}

		grid.repositionNow = true;
	}

	void ItemClick(GameObject go)
	{	
		ChaosInfoMgr chaoinfo = go.GetComponent<ChaosInfoMgr>();
		int chaosID = chaoinfo.chaosID;
		int style = chaoinfo.style;

		//钥匙
		if (chaoinfo.style == 2 && chaoinfo.condition == 1)
		{
			userInfo = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser;
			keyCount = userInfo.keyCoin;
			if (keyCount < 0)
				return;
		}
		else
		if (chaoinfo.style == 3)
		{
			//DayOfWeek week =  System.DateTime.Now.DayOfWeek;
			//DateTime nextData = System.DateTime.Now.Date + 1;
			//DayOfWeek.Monday
		}

		panelChaos.SetActive(false);
		int battleID = chaosID;
		DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).currBattleID = battleID;				
		dungeonmgr.init(battleID);

		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, true, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, false, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, true, (int)EVENT_GROUP.mainUI);

		UI.PanelStack.me.goNext(paneldungeon,onReturn);
		//paneldungeon.SetActive(!paneldungeon.activeSelf);

	}

	string getWeekName(DayOfWeek week)
	{

		if (week == DayOfWeek.Sunday)
		{
			return " 周日副本";
		}
		else
		if (week == DayOfWeek.Monday)
		{
			return "周一副本";
		}
		else
		if (week == DayOfWeek.Tuesday)
		{
			return "周二副本";
		}
		else
		if (week == DayOfWeek.Wednesday)
		{
			return "周三副本";
		}
		else
		if (week == DayOfWeek.Thursday)
		{
			return "周四副本";
		}
		else
		if (week == DayOfWeek.Friday)
		{
			return "周五副本";
		}
		else
		if (week == DayOfWeek.Saturday)
		{
			return "周六副本";
		}
		return "";
	}

	void onReturn(System.Object param)
	{
		panelChaos.SetActive(true);
	}
}
