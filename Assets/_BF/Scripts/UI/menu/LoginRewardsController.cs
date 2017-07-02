using UnityEngine;
using System.Collections;
using DataCenter;
using BaseLib;

public class LoginRewardsController : MonoBehaviour {

	private GameObject panelMenuBg;
	private int seq ;
	private bool flag = false;

	void Start () {
		if(flag)
			return;
		UI.PanelTools.setBtnFunc(transform, "Bg/menuTitle/back", onBtnBackClick);
		panelMenuBg = UI.PanelStack.me.FindPanel ("PanelMenu/Bg");

		UserInfo info = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser;
		seq = info.presentSeq;

		flag = true;
	}

	void OnEnable()
	{
		Start ();
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
			}
			else
			{
				this.transform.Find("Bg/"+ i+"/get").GetComponent<UISprite>().spriteName ="BtnBlueBg";
				this.transform.Find("Bg/"+ i+"/get/Label").GetComponent<UILabel>().text = "待领取";
			}
		}
	}

	void onBtnBackClick(GameObject go)
	{
		AudioManager.SingleTon().PlayMusic(AudioManager.SingleTon().MusicMainClip);
		this.transform.gameObject.SetActive (false);
		panelMenuBg.SetActive (true);
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
