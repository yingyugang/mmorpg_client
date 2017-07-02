using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;

public class ReplayController : MonoBehaviour {

	private float delay;
	private GameObject dialog,shakeObject;
	private string txt;
	private Dictionary<int,Item> storyDialog;
	public int storyid;
	private int seq , charsPerSecond;
	private bool ini = false;
	private bool isInterrupt;

	public struct Item
	{
		public string npcPic;
		public string npcName;
		public string sceneBg;
		public int delay;
		public int shake;
		public string content;
	}

	void Start () {
		if(ini)
			return;
		dialog = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse/PanelStoryline/PanelStoryList/PanelRepaly/Bg/Above/dialog");
		shakeObject = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse/PanelStoryline/PanelStoryList/PanelRepaly/Bg/Above");
		UI.PanelTools.setBtnFunc(transform, "Bg/Above/btnClose", onBackClick);
		UI.PanelTools.setBtnFunc(transform, "Bg/Above", onBgClick);
		charsPerSecond = 30;
		ini = true;
	}

	void onBackClick(GameObject go)
	{
		if(UI.PanelStack.me.goBack()==null)
		{
			this.gameObject.SetActive(false);
		}
	}

	void onBgClick(GameObject go)
	{
		if(!isInterrupt)
		{
			if(Time.time - delay < (txt.Length *1f/ charsPerSecond) )
			{				
				isInterrupt = true;
				Destroy (dialog.GetComponent<TypewriterEffect> ());
				dialog.GetComponent<UILabel> ().text = txt;
			}
			else
			{
				Play();
			}
		}
		else
		{
			Play();
		}		
	}

	void init()
	{
		seq = 0;
		isInterrupt = false;
		storyDialog = new Dictionary<int,Item>();
		ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_STORY_DIALOG);
		if (table == null )
			return;
		int order;
		foreach(ConfigRow row in table.rows)
		{
			int id = row.getIntValue(DICT_STORY_DIALOG.STORY_TYPEID);
			if(id != storyid)
				continue;
			order = row.getIntValue(DICT_STORY_DIALOG.ORDER);
			Item item = new Item();
			item.npcPic = "GachaEffectTylis1_143x249";//row.getIntValue(DICT_STORY_DIALOG.CHARACTER_ICON_ID);
			item.npcName = "媞莉丝";//BaseLib.LanguageMgr.getString(row.getIntValue(DICT_STORY_DIALOG.CHARACTER_NAME_ID));
			item.sceneBg = "BattleDungeon10200MagmaRiverBgBack_240x354";//row.getIntValue(DICT_STORY_DIALOG.CONTENT_ICON_ID);
			item.delay = row.getIntValue(DICT_STORY_DIALOG.DELAY);
			item.shake = row.getIntValue(DICT_STORY_DIALOG.SHAKE);
			item.content =  BaseLib.LanguageMgr.getString(row.getIntValue(DICT_STORY_DIALOG.CONTENT_STR_ID)) + order.ToString();;
			storyDialog[order]= item;
		}
		delay = Time.time;
		dialog.GetComponent<UILabel> ().text = "";
		Destroy (dialog.GetComponent<TypewriterEffect> ());
	}

	void OnEnable()
	{
		Start ();
		init ();
		Play ();
	}

	IEnumerator YieldDelay()
	{
		yield return new WaitForSeconds (1);
	}

	void Play()
	{
		seq++;
		isInterrupt = false;
		if(storyDialog.ContainsKey(seq))
		{
			Item item = storyDialog[seq];
			if(item.shake == 1)
				iTween.ShakePosition(shakeObject,Vector3.one * 0.3f,0.3f);
			StartCoroutine("YieldDelay");
			dialog.GetComponent<UILabel> ().text = item.content;
			Destroy (dialog.GetComponent<TypewriterEffect> ());
			dialog.AddComponent<TypewriterEffect> ();
			dialog.GetComponent<TypewriterEffect> ().charsPerSecond = 30;
			delay = Time.time;
			txt = item.content;
		}
		else
		{
			this.gameObject.SetActive(false);
		}
	}

	void  Finish()
	{
		isInterrupt = true;
		Destroy (dialog.GetComponent<TypewriterEffect> ());
		dialog.GetComponent<UILabel> ().text = txt;
	}

}
