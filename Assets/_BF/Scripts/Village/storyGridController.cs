using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;

public class storyGridController : MonoBehaviour {

	private int curPage;
	private int totalPage;
	private int PerNum = 6;
	private GameObject storyDetail;
	private UIGrid grid;
	private Dictionary<int,Item> storyList;
	private GameObject nav;
	private bool ini = false;

	public struct Item
	{
		public int storyid;
		public string storyTitle;
		public string spriteName;
		public string desc;
		public string battle;
		public bool unLock;
	}

	void Start () {
		if(ini)
			return;
		UI.PanelTools.setBtnFunc(transform.parent, "nav/up", onUpClick);
		UI.PanelTools.setBtnFunc(transform.parent, "nav/down", onDownClick);
		storyDetail = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse/PanelStoryline/PanelStoryList/Bg/book/storyDetail");
		nav =  UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse/PanelStoryline/PanelStoryList/Bg/book/nav");
		//DataManager.getModule<DataStory> (DATA_MODULE.Data_Story).activateStory (30000);
		//EventSystem.register ((int)EVENT_MAINUI.storyUpdate,onUpdateItem,(int)EVENT_GROUP.mainUI);
		ini = true;
	}

	void onUpdateItem(int nEvent , System.Object param)
	{
		init ();
		Show ();
	}

	void OnEnable()
	{
		Start ();
		if (grid == null)
			grid = transform.GetComponent<UIGrid>();
		if(storyDetail != null)
			storyDetail.SetActive (false);
		init ();
		Show ();
	}

	void onUpClick(GameObject go)
	{
		if(curPage > 1)
		{
			curPage --;
			Show ();
		}
	}

	void onDownClick(GameObject go)
	{
		if(curPage < totalPage)
		{
			curPage ++;
			Show ();
		}

	}

	void init()
	{
		storyList = new Dictionary<int,Item>();

		ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_STORY);
		if (table == null )
			return;
		int total = 0;
		foreach(ConfigRow row in table.rows)
		{
			Item item = new Item();
			item.storyid = row.getIntValue(DICT_STORY.STORY_TYPEID);
			item.storyTitle =  BaseLib.LanguageMgr.getString(item.storyid);
			item.spriteName = "c";//row.getIntValue(DICT_STORY.ICON_ID);
			item.desc = "剧情简介"; //row.getIntValue(DICT_STORY.);
			int bid = row.getIntValue(DICT_STORY.BATTLE_ID);
			item.battle = BaseLib.LanguageMgr.getString(bid);
			item.unLock = DataManager.getModule<DataStory>(DATA_MODULE.Data_Story).isFinish((uint)bid);
			storyList[++total] = item;
		}

		curPage = 1;
		totalPage = (total + PerNum - 1) / PerNum;
		/*
		for(int i=1; i<= total ; i++)
		{
			Item item = new Item();
			item.storyTitle = "剧情标题";
			if(i<6)
				item.spriteName ="c";
			else
				item.spriteName ="d";
			storyList[i] = item;
		}*/
	}

	void Show()
	{
		if(nav != null)
		{
			nav.transform.Find ("curPage").GetComponent<UILabel> ().text = curPage.ToString ();
			nav.transform.Find ("totalPage").GetComponent<UILabel> ().text ="/"+ totalPage.ToString ();
		}
		clearItems ();
		for(int i = (curPage-1)* PerNum +1 ;i <= curPage*PerNum ;i++ )
		{
			if(storyList.ContainsKey(i))
			{
				Item item = storyList[i];
				GameObject storyItem =  (GameObject) Instantiate(Resources.Load("Village/storyItem"));
				StoryItem itemData = storyItem.AddComponent<StoryItem>();
				itemData.typeid = item.storyid;
				itemData.Sname = item.storyTitle;
				itemData.desc = item.desc;
				itemData.sprite = item.spriteName;
				itemData.unLock = item.unLock;

				storyItem.transform.Find("Label").GetComponent<UILabel>().text = item.storyTitle;
				storyItem.GetComponent<UISprite>().spriteName = item.spriteName;
				storyItem.transform.Find("lock").GetComponent<UISprite>().alpha = item.unLock? 0:1;				
				storyItem.transform.parent = transform;
				storyItem.transform.localScale = Vector3.one;
				UIEventListener.Get(storyItem).onClick = ItemClick;
			}
		}
		grid.repositionNow = true;
	}

	void clearItems()
	{
		for (int i = transform.childCount -1; i >=0; i--) 
		{
			Destroy(transform.GetChild(i).gameObject);
		}
	}

	void ItemClick(GameObject click)
	{
		AudioManager.me.PlayBtnActionClip();
		StoryItem data =  click.GetComponent<StoryItem> ();
		storyDetailController detail = storyDetail.GetComponent<storyDetailController>();
		detail.Item = data;
		storyDetail.SetActive (true);
	}
}
