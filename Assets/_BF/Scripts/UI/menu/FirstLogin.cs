using UnityEngine;
using System.Collections;
using DataCenter;
using BaseLib;

public class FirstLogin : MonoBehaviour {
	
	private int seq ;
	private ulong getTime;
	private bool flag = false,first;
	
	void Start () {
		if(flag)
			return;
		//UI.PanelTools.setBtnFunc(transform, "Bg/menuTitle/back", onBtnBackClick);
		
		UserInfo info = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser;
		seq = info.presentSeq;
		getTime = info.lastGetPresentTime;
		
		flag = true;
	}
	
	void OnEnable()
	{
		Start ();
		if(isFirst())
		{
			first = true;
		}
		else
		{
			FirstClose();			
		}
		init ();
		Dispaly ();
	}
	
	void init()
	{
		for(int i=1; i<=7;i++)
		{
			if(i <= seq)
			{
				this.transform.Find("Bg/"+ i+"/get").GetComponent<UISprite>().spriteName ="yinzhang";
				this.transform.Find("Bg/"+ i+"/get/Label").GetComponent<UILabel>().text = "";
				if(i== seq && first)
				{
					UI.PanelTools.setBtnFunc(transform, "Bg/" + i, onClick);
					this.transform.Find("Bg/"+ i+"/get").GetComponent<UISprite>().spriteName ="BtnblueBg2";
					this.transform.Find("Bg/"+ i+"/get/Label").GetComponent<UILabel>().text = "领取";
				}
			}
			else
			{
				this.transform.Find("Bg/"+ i+"/get").GetComponent<UISprite>().spriteName ="BtnBlueBg";
				this.transform.Find("Bg/"+ i+"/get/Label").GetComponent<UILabel>().text = "待领取";
			}
		}
	}
	
//	void onBtnBackClick(GameObject go)
//	{
//		FirstClose();
//	}
	
	void onClick(GameObject go)
	{
		FirstClose ();
	}
	
	bool isFirst()
	{
		ulong curTime = DataManager.getModule<DataServTime> (DATA_MODULE.Data_ServTime).getCurSvrtime ();
	//	DateTime curDate = DataManager.getModule<DataServTime> (DATA_MODULE.Data_ServTime).getServTime (curTime);
	//	DateTime latest= DataManager.getModule<DataServTime> (DATA_MODULE.Data_ServTime).getServTime (getTime);
		//if(latest.Day != curDate.Day || latest.Month != curDate.Month || latest.Year != curDate.Year)
		if(curTime - getTime < 30)
			return true;
		else
			return false;
	}
	
	void FirstClose()
	{
		this.gameObject.SetActive (false);
	}
	
	void Dispaly()
	{
		ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_DAILY_PRESENT);
		if(table == null)
			return;
		foreach(ConfigRow row in table.rows)
		{
			int id = row.getIntValue(DICT_DAILY_PRESENT.ID);
			int tag = row.getIntValue(DICT_DAILY_PRESENT.TAG);
			int param1 = row.getIntValue(DICT_DAILY_PRESENT.PARA1);
			if(tag == (int)TAG_SORT.coin)
			{
				this.transform.Find("Bg/"+ id+"/reward").GetComponent<UILabel>().text = "金币X" + param1;
			}
			else if(tag == (int)TAG_SORT.soul)
			{
				this.transform.Find("Bg/"+ id+"/reward").GetComponent<UILabel>().text = "魂X" + param1;
			}
			else if(tag == (int)TAG_SORT.diamond)
			{
				this.transform.Find("Bg/"+ id+"/reward").GetComponent<UILabel>().text = "钻石X" + param1;
			}
			else if(tag == (int)TAG_SORT.friendPoints)
			{
				this.transform.Find("Bg/"+ id+"/reward").GetComponent<UILabel>().text =  param1 + "友情点";
			}
			else if(tag == (int)TAG_SORT.item)
			{
				ConfigTable items = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ITEM);
				string iName = BaseLib.LanguageMgr.getString(items.getRow(DICT_ITEM.ITEM_TYPEID, param1).getIntValue(DICT_ITEM.NAME_ID));
				int param2 = row.getIntValue(DICT_DAILY_PRESENT.PARA2);
				this.transform.Find("Bg/"+ id+"/reward").GetComponent<UILabel>().text = iName + "X" + param2;
			}
			else if(tag == (int)TAG_SORT.hero)
			{
				ConfigTable hero = ConfigMgr.getConfig(CONFIG_MODULE.DICT_HERO);
				string hName = BaseLib.LanguageMgr.getString(hero.getRow(DICT_HERO.HERO_TYPEID, param1).getIntValue(DICT_HERO.NAME_ID));
				int param2 = row.getIntValue(DICT_DAILY_PRESENT.PARA2);
				this.transform.Find("Bg/"+ id+"/reward").GetComponent<UILabel>().text = hName + "X" + param2;
			}
		}
	}
}
