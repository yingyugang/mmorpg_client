using UnityEngine;
using System;
using System.Collections;
using BaseLib;
using DataCenter;



public class ChaosMgr : MonoBehaviour {

	public GameObject btnReturn;
	public GameObject chaosGrid;
	private ChaosGridMgr chaosGridMgr;
	private int keyTime;
	private uint keyCount;
	private UserInfo userInfo;
	private UILabel keycountlbl;
	private GameObject btnGetkey; 
	private UILabel lblGetKey; 
	private UInt64 lastGetKeyTime;
	private UILabel lblKeyCount;

	private bool isCanGain;

	void Awake()
	{
		keyTime = 6;
		isCanGain = false;
	}

	// Use this for initialization
	void Start () {
		UIEventListener.Get(btnReturn).onClick = onReturnClick;
		EventSystem.register((int)EVENT_MAINUI.userUpdate, onUserUpdate,(int)EVENT_GROUP.mainUI);
		//iTween.MoveFrom(bgReturn,iTween.Hash("x",-3,"time",1));
		NGUITools.AddWidgetCollider(transform.gameObject);	

		//UI.PanelStack.me.root = this.gameObject;
		//UI.PanelStack.me = GameObject.Find("Camera");
		btnGetkey = UI.PanelStack.me.FindPanel("Scale/PanelStageMgr/PanelChaos/Bg/btnGetKey");
		UIEventListener.Get(btnGetkey).onClick = onGetKeyClick;


		DateTime time = DateTime.Now;
		int stamp = toStamp(time);

		DateTime time2 = toTime(stamp.ToString());

		lblGetKey = UI.PanelStack.me.FindPanel("Scale/PanelStageMgr/PanelChaos/Bg/btnGetKey/lblGetKey").GetComponent<UILabel>();
		//lblGetKey.text = time2.ToString();

		UserInfo info = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser;
		lastGetKeyTime = info.LastGetKeyTime;
		Debug.Log("lastGetKeyTime............................." + lastGetKeyTime);	


		DateTime lastTime = toTime(lastGetKeyTime.ToString());
		TimeSpan span = time.Subtract(lastTime);

		if (lastGetKeyTime == 0)
		{
			lblGetKey.text = "可领取";
			isCanGain = true;
		}
		else 
		if (span.Days >= 1)
		{
			lblGetKey.text = "可领取";
			isCanGain = true;
		}
		else 
		if (lastTime.ToShortDateString() == time.ToShortDateString() && (lastTime.Hour < 18 && lastTime.Hour > 8) )
		{
			lblGetKey.text = "已领取";
			isCanGain = false;
		}


		//init();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void onReturnClick(GameObject go)
	{
		//iTween.MoveTo(bgReturn,iTween.Hash("x",-3,"time",1));
		//iTween.MoveTo(bgGrid,iTween.Hash("x",3,"time",1));
		StartCoroutine("setChaosActive");
	}

	IEnumerator setChaosActive()
	{


		yield return new WaitForSeconds(0.2f);
		transform.gameObject.SetActive(false);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, true, (int)EVENT_GROUP.mainUI);
		//iTween.MoveTo(bgReturn,iTween.Hash("x",-0.2,"time",1));
		//iTween.MoveTo(bgGrid,iTween.Hash("x",0,"time",1));

	}

	public void init()
	{
		if (!chaosGridMgr)
			chaosGridMgr = chaosGrid.GetComponent<ChaosGridMgr>();

		chaosGridMgr.Init();
	}

	void OnEnable()
	{
		userInfo = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser;
		keyCount = userInfo.keyCoin;

		UILabel[] lbls = transform.GetComponentsInChildren<UILabel>();
		for (int i = 0; i < lbls.Length; i ++)
		{
			Debug.Log("com[i].name...................." + lbls[i].name);
			if (lbls[i].name == "keycountlbl")
			{
				lblKeyCount = lbls[i];
				lblKeyCount.text = "钥匙" + keyCount.ToString() + "/10";
				break;
			}
		}

		init();
	
	}

	void onGetKeyClick(GameObject go)
	{
		if (isCanGain == false)
			return;

		int KeyAmount = (int)keyCount;
		if (KeyAmount >= 10)
		{
			string title = "提示";
			string conteng = "钥匙数已经达到最大数量，不能领取";		
			DataManager.getModule<DataShop>(DATA_MODULE.Data_Shop).ShowShopComfirm(title, conteng, onReturn);
		}
		else		
		{
			DataManager.getModule<DataUser>(DATA_MODULE.Data_User).getKey();
            lblGetKey.text = "已领取";
		}
	}

	void onReturn(GameObject go)
	{
	}

	//时间戳转日期
	public DateTime toTime(string timeStamp)
	{
		DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970,1,1));
		long lTime = long.Parse(timeStamp + "0000000");
		TimeSpan toNow = new TimeSpan(lTime);
		return dtStart.Add(toNow);

	}

	//日期转时间戳
	public int toStamp(DateTime time)
	{
		DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970,1,1));
		return (int)(time - startTime).TotalSeconds;
	}

	void onUserUpdate(int nEvent,System.Object param)
	{
		if ((USER_ATTR)param == USER_ATTR.USER_ATTR_KEY_GOLD)
		{
			//lblGetKey.text = "已领取";
			lblKeyCount.text = "钥匙" + DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.keyCoin.ToString() + "/10";
		}
	}

}
