using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;

public class GroupChoiceGridMgr : MonoBehaviour {

	public struct dungeonInfo
	{
		public string name;
		public string power;
		public string time;
		public string desc;
		public bool isPass;
	}

	public GameObject panelBattlePlan;
	//public GameObject panelHelperList;
	//public GameObject panelDungeon;
	public GameObject GroupChoiceItemPrefab;
	public List<dungeonInfo> dungInfo;
	//public GameObject bgGrid;
	private UIGrid grid;
	private List<FieldInfo> groupList;
	private int currIndex;
	private int leaderPos;
	private int friendPos;
	private int CurrTeamId;
	private Transform scrollView;
	private int Currindex = 0;

	private Dictionary<int, TeamInfo> HeroTeams;

	private bool isScroll;

	// Use this for initialization
	void Awake()
	{
	}

	void Start () {	
		dungInfo = new List<dungeonInfo>();

		scrollView = transform.parent;
		//iTween.MoveFrom(bgGrid,iTween.Hash("x",3,"time",1));		
		//BaseLib.EventSystem.register((int)EVENT_MAINUI.battleStart, onBattleStart, (int)DataCenter.EVENT_GROUP.mainUI);

		//Init();
	}

	public void Init()
	{
		isScroll = false;
		if (!grid)
		{
			grid = transform.GetComponent<UIGrid>();
			//grid.onReposition += onReposition;
		}
		/*for (int i = grid.transform.childCount -1; i >= 0; i--)
		{
			GameObject go = grid.transform.GetChild(i).gameObject;
			Destroy(go);
		}*/
		//groupList = DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).getFieldDataByBattleID(battleID);

		HeroTeams = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams;

		if (grid.transform.childCount == 0)
		{
			for (int i =0; i < 10; i++)
				NGUITools.AddChild(grid.gameObject,GroupChoiceItemPrefab);
					
		}
	

		//grid.repositionNow = true;
		
		scrollView = transform.parent;
	
			
		setGroupItem();

	}

	public void setGroupItem()
	{	
		CurrTeamId = (int)DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).nCurTeamId + 1;
		//int Currindex = 0;


		for (int j = 1; j <= HeroTeams.Count; j ++)	
		{
			//GameObject groupChoiceItem = Instantiate(GroupChoiceItemPrefab) as GameObject;
			//GameObject groupChoiceItem = NGUITools.AddChild(grid.gameObject,GroupChoiceItemPrefab);
			GameObject groupChoiceItem = grid.GetChild(j-1).gameObject;
			//groupChoiceItem.SetActive(true);

			TeamInfo teamInfo = HeroTeams[j];
			currIndex = 1;
			leaderPos = teamInfo.leaderPos;	
			friendPos = teamInfo.friendPos;
			

			setHeroInfo(groupChoiceItem,teamInfo.pos1HeroId,1);
			setHeroInfo(groupChoiceItem,teamInfo.pos2HeroId,2);
			setHeroInfo(groupChoiceItem,teamInfo.pos3HeroId,3);
			setHeroInfo(groupChoiceItem,teamInfo.pos4HeroId,4);
			setHeroInfo(groupChoiceItem,teamInfo.pos5HeroId,5);
			//setHeroInfo(groupChoiceItem,teamInfo.pos6HeroId,6);

			HeroInfo friendHero = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).getFriendHero();
			if (friendHero != null)					
				setHeroInfo(groupChoiceItem,friendHero.id,6,friendHero);

			//groupChoiceItem.transform.Find("groupPnl/groupindex").GetComponent<UILabel>().text = j.ToString();


				/*groupChoiceItem.transform.Find("groupPnl/heroicon1/herolevel").GetComponent<UILabel>().text = teamInfo.pos1HeroId.ToString();
				groupChoiceItem.transform.Find("groupPnl/heroicon2/herolevel").GetComponent<UILabel>().text = teamInfo.pos2HeroId.ToString();
				groupChoiceItem.transform.Find("groupPnl/heroicon3/herolevel").GetComponent<UILabel>().text = teamInfo.pos3HeroId.ToString();
				groupChoiceItem.transform.Find("groupPnl/heroicon4/herolevel").GetComponent<UILabel>().text = teamInfo.pos4HeroId.ToString();
				groupChoiceItem.transform.Find("groupPnl/heroicon5/herolevel").GetComponent<UILabel>().text = teamInfo.pos5HeroId.ToString();
				groupChoiceItem.transform.Find("groupPnl/heroicon6/herolevel").GetComponent<UILabel>().text = teamInfo.pos6HeroId.ToString();*/

			//groupChoiceItem.transform.parent = grid.transform;
			//groupChoiceItem.transform.localScale = Vector3.one;

			if (CurrTeamId == j)
			{
				Currindex = j;
			}


			//UIEventListener.Get(groupChoiceItem).onClick = ItemClick;
		}
			

		/*for (int i = HeroTeams.Count + 1; i < transform.childCount; i ++)
		{
			transform.GetChild(i).gameObject.SetActive(false);
		}*/


		//grid.repositionNow = true;
		//grid.Reposition();


		//onReposition();

	}

	void setHeroInfo(GameObject groupObject, int heroID,int index,HeroInfo pHeroinfo = null)
	{
		if (heroID == 0 && pHeroinfo == null)		
			return;


		GameObject hero;

		if (index == leaderPos)
		{
			hero = groupObject.transform.Find("groupPnl/heroicon0").gameObject;
		}
		else
		if (index == friendPos)
		{
			hero = groupObject.transform.Find("groupPnl/heroicon5").gameObject;
		}
		else
		{
			hero = groupObject.transform.Find("groupPnl/heroicon" + currIndex).gameObject;
			currIndex += 1;
		}
		
		hero.SetActive(true);
			

		if (hero.activeSelf == true)
		{
			//hero.gameObject.SetActive(true);


			HeroInfo heroinfo = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(heroID);
			if (heroinfo != null)
			{
				hero.transform.Find("herolevel").GetComponent<UILabel>().text = "Lv" + heroinfo.level.ToString();
				hero.transform.Find("friendicon").GetComponent<UISprite>().spriteName = heroinfo.spriteName;	
				hero.transform.Find("friendstar").GetComponent<UISprite>().spriteName =  "star" + heroinfo.star.ToString();	
				hero.transform.Find("friendtype").GetComponent<UISprite>().spriteName =  "SERIES" + heroinfo.series.ToString();	
			}
			
			if (index == leaderPos)
			{
				//SkillBase skill = DataManager.getModule<DataSkill>(DATA_MODULE.Data_Skill).getSkillBaseDataBySkillID(heroinfo.skillCaptian);
				//if (skill != null)
				if (heroinfo != null)
				{
					hero.transform.parent.Find("Captain/leaderSkillName").GetComponent<UILabel>().text = heroinfo.skillCaptianName;				
					hero.transform.parent.Find("Captain/leaderSkillDesc").GetComponent<UILabel>().text = heroinfo.skillCaptianDesc;
				}			
			}
			else
			if (index == friendPos)
			{
				if (pHeroinfo == null)
					return;

				hero.transform.Find("herolevel").GetComponent<UILabel>().text = "Lv" + pHeroinfo.level.ToString();
				hero.transform.Find("friendicon").GetComponent<UISprite>().spriteName = pHeroinfo.spriteName;	
                hero.transform.Find("friendstar").GetComponent<UISprite>().spriteName =  "star" + pHeroinfo.star.ToString(); 

				SkillBase skill = DataManager.getModule<DataSkill>(DATA_MODULE.Data_Skill).getSkillBaseDataBySkillID(pHeroinfo.skillCaptian);
				if (skill != null)
				{
					hero.transform.parent.Find("helper/helperSkillName").GetComponent<UILabel>().text = pHeroinfo.skillCaptianName;				
					hero.transform.parent.Find("helper/helperSkillDesc").GetComponent<UILabel>().text = pHeroinfo.skillCaptianDesc;
				}			
			}
		}

	}

	/*void ItemClick(GameObject go)
	{		
		int fieldID = Convert.ToInt32(go.transform.Find("dungeonPnl/dungeonID").GetComponent<UILabel>().text);
		DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).getEnemyInfoList(fieldID);
		DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).currFieldID = fieldID;
		Debug.Log((uint)fieldID);
		DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).battleStart((uint)fieldID,(uint)10000);
	
	}*/

	void onBattleStart(int nEvent, System.Object param)
	{
		Debug.Log("nEvent:" + nEvent);
		BaseLib.EventSystem.sendEvent((int)DataCenter.EVENT_GLOBAL.sys_chgScene,LoadSceneMgr.SCENE_BATTLE);
//		EventSystem.register((int)MsgId._MSG_CLEINT_USE_BATTLE_IEM, onBattleItem, (int)DataCenter.EVENT_GROUP.packet);
	}

	public void onReposition()
	{
		/*if (scrollView != null )
		{
			UIGrid uiGrid = grid.GetComponent<UIGrid>();
			scrollView.localPosition = new Vector3(-(Currindex-1)*uiGrid.cellWidth,0,0);
			//scrollView.localPosition = new Vector3(-grid.GetChild(Currindex-1).localPosition.x,0,0);
			UIPanel panel = scrollView.GetComponent<UIPanel>();
			//panel.clipOffset = new Vector2(grid.GetChild(Currindex-1).localPosition.x,0);
			panel.clipOffset = new Vector2((Currindex-1)*uiGrid.cellWidth,0);				
		}*/




		for (int i = 1; i <= transform.childCount; i ++)
		{

			Transform childItem = transform.GetChild(i - 1);
			childItem.localPosition = new Vector3((i - Currindex) * 650,childItem.localPosition.y,childItem.localPosition.z);	
			childItem.Find("groupPnl/groupindex").GetComponent<UILabel>().text = i.ToString();
		}

	}


	void OnEnable()
	{
		Init();	
	}

	/*void onDisable()
	{	
		if (scrollView != null )
		{
			scrollView.localPosition = Vector3.zero;
			UIPanel panel = scrollView.GetComponent<UIPanel>();
			panel.clipOffset = Vector2.zero;
		}

	}*/

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate()
	{
		if (isScroll == false)
		{
			onReposition();
			isScroll = true;
		}
	}
}
