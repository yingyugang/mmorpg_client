using UnityEngine;
using System.Collections;
using DataCenter;
using UI;

public class GroupChoiceMgr : MonoBehaviour {

	//public GameObject bgReturn;
	//public GameObject GroupChoiceGrid;
	public GameObject bg;

	public GameObject friendPartner;
	public ItemInfo[] itemInfo;

	private GameObject PanelDrugTeam;
	private DrugTeamController drugTeam;

	public UIGrid groupGrid;

	public Transform scrollView;

	public GameObject partnerEdit;
	private GroupChoiceGridMgr groupChoiceGridMgr;

	private int teamIndex;

	private FieldInfo fieldInfo;
	private int fieldID;

	// Use this for initialization
	void Start () {
		//bgReturn = GameObject.Find("dungeontitle").gameObject;

		bg = GameObject.Find("Bg").gameObject;
		//PanelDrugTeam = UI.PanelStack.me.FindPanel("Scale/NewVillage/PanelStuffMgr/PanelDrugTeam");
		//drugTeam = PanelDrugTeam.GetComponent<DrugTeamController>();

		//iTween.MoveFrom(bgReturn,iTween.Hash("x",-3,"time",1));
		NGUITools.AddWidgetCollider(transform.gameObject);
	
		fieldID = DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).currFieldID;
		fieldInfo = DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).getFieldDataByFieldID(fieldID);	
		
		GameObject.Find("PanelBattlePlan/Bg/battleAmount").gameObject.GetComponent<UILabel>().text = fieldInfo.MAX_STEP.ToString();
		GameObject.Find("PanelBattlePlan/Bg/energyAmount").gameObject.GetComponent<UILabel>().text = fieldInfo.COST_ENARGY.ToString();



		if (groupGrid != null)
			groupChoiceGridMgr = groupGrid.GetComponent<GroupChoiceGridMgr>();

		UIEventListener.Get(GameObject.Find("PanelBattlePlan/Bg/back").gameObject).onClick = onReturnClick;
		//UIEventListener.Get(GameObject.Find("btnhome").gameObject).onClick = onHomeClick;

		//UIEventListener.Get(GameObject.Find("btnGroup").gameObject).onClick = onbtnGroupClick;
		UIEventListener.Get(GameObject.Find("btnStart").gameObject).onClick = onbtnStartClick;
		UIEventListener.Get(GameObject.Find("teamGrid").gameObject).onClick = onteamGridClick;
		BaseLib.EventSystem.register((int)EVENT_MAINUI.battleStart, onBattleStart, (int)DataCenter.EVENT_GROUP.mainUI);
		getDrugItem();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void onbtnGroupClick(GameObject go)
	{

		DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).FightShowTeamEdit();
		//BaseLib.EventSystem.sendEvent((int)DataCenter.EVENT_GLOBAL.sys_chgScene, LoadSceneMgr.SCENE_MAIN);
		if (partnerEdit != null)
		{
			//UI.PanelStack.me.clear();
			UI.PanelStack.me.goNext(partnerEdit,onRefreshHeroTeam);
			transform.gameObject.SetActive(false);
		}

	}

	void onbtnStartClick(GameObject go)
	{       
		int nCount = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).heroInfoList.Count;
		uint nMaxCount = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.maxHero;
		//int fieldID = DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).currFieldID;
		int friendID = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).getFriendUserID();
		//FieldInfo fieldInfo = DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).getFieldDataByFieldID(fieldID);	
		UserInfo info = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser;

        uint itemAmount = DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).getItemCount ();
      
        if(itemAmount>=info.maxItem)
        {
            string title = "提示";
            string conteng = "当前物品数量已经达到格子数上限";        
            DataManager.getModule<DataShop>(DATA_MODULE.Data_Shop).ShowShopComfirm(title, conteng, null);
        }
        else           
		if (nCount >= nMaxCount)
		{
			string title = "提示";
			string conteng = "当前英雄数量已经达到格子数上限";		
			DataManager.getModule<DataShop>(DATA_MODULE.Data_Shop).ShowShopComfirm(title, conteng, null);
		}
		else
		if (fieldInfo.COST_ENARGY > info.curpower)
		{
			string title = "提示";
			string conteng = "当前体力值不够";		
			DataManager.getModule<DataShop>(DATA_MODULE.Data_Shop).ShowShopComfirm(title, conteng, null);		
		}
		else
		{

			DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).battleStart((uint)fieldID,(uint)10000,(uint)friendID);
			//DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).battleStart((uint)fieldID,(uint)10000,0);

			Debug.Log("onbtnStartClick");
			//BaseLib.EventSystem.sendEvent((int)DataCenter.EVENT_GLOBAL.sys_chgScene,LoadSceneMgr.SCENE_BATTLE);
			//DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).nCurTeamId = teamIndex -1;
			//DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).SendHeroFormationMsg();

			sendCurrItem();		
			transform.gameObject.SetActive(false);

		}
	}

	void onBattleStart(int nEvent, System.Object param)
	{
		Debug.Log("onBattleStart");
		BaseLib.EventSystem.sendEvent((int)DataCenter.EVENT_GLOBAL.sys_chgScene,LoadSceneMgr.SCENE_BATTLE);
		//		EventSystem.register((int)MsgId._MSG_CLEINT_USE_BATTLE_IEM, onBattleItem, (int)DataCenter.EVENT_GROUP.packet);
	}

	void onteamGridClick(GameObject go)
	{	
		//PanelDrugTeam.SetActive(true);
//		UI.panelBackFunc backFunc = onRefreshDrugTeam;
		//UI.PanelStack.me.clear();
		//DrugTeamController drugTeam = new DrugTeamController();

		PanelDrugTeam = UI.PanelStack.me.FindPanel("Scale/NewVillage/PanelStuffMgr/PanelDrugTeam");
		if (PanelDrugTeam != null)
		{
			drugTeam = PanelDrugTeam.GetComponent<DrugTeamController>();
			drugTeam.showDrugTeam();
			UI.PanelStack.me.goNext(PanelDrugTeam,onRefreshDrugTeam);
			transform.gameObject.SetActive(false);
		}

	}

	void onRefreshDrugTeam(System.Object param)
	{
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, true, (int)EVENT_GROUP.mainUI);
		//BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, true, (int)EVENT_GROUP.mainUI);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, true, (int)EVENT_GROUP.mainUI);
		transform.gameObject.SetActive(true);
		getDrugItem();

	}

	void onRefreshHeroTeam(System.Object param)
	{
		transform.gameObject.SetActive(true);
		groupChoiceGridMgr.setGroupItem();
	}

	void onReturnClick(GameObject go)
	{
		////iTween.MoveTo(bgReturn,iTween.Hash("x",-3,"time",1));
		////iTween.MoveTo(bg,iTween.Hash("x",3,"time",1));
		StartCoroutine("setGroupChoiceActive");
		Debug.Log("");
	}

	IEnumerator setGroupChoiceActive()
	{
		/*if (groupGrid)
		{
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

			BetterList<Transform> childList = groupGrid.GetChildList();
			for (int i = 0; i < childList.size; i ++)
			{
				if (GeometryUtility.TestPlanesAABB(planes,childList[i].gameObject.collider.bounds))
				{					
					teamIndex = int.Parse(childList[i].transform.Find("groupPnl/groupindex").GetComponent<UILabel>().text);
				}
			}
		}

		DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).nCurTeamId = teamIndex -1;
		DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).SendHeroFormationMsg();*/
		//DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).SendHeroGroupMsg();


		sendCurrItem();

		yield return new WaitForSeconds(0.5f);
		transform.gameObject.SetActive(false);
		//friendPartner.SetActive(true);

		UI.PanelStack.me.goBack();


		//iTween.MoveTo(bgReturn,iTween.Hash("x",-0.2,"time",1));
		//iTween.MoveTo(bg,iTween.Hash("x",0,"time",1));

	}

	void OnEnable()
	{
		//iTween.MoveFrom(bgReturn,iTween.Hash("x",-3,"time",1));
		//iTween.MoveFrom(bg,iTween.Hash("x",3,"time",1));	
		//scrollView = PanelTools.findChild(transform,"ScrollView");

		//getDrugItem();
		//Debug.Log("getDrugItem........................................................");
	}

	void OnDisable()
	{
		////iTween.MoveTo(bgReturn,iTween.Hash("x",-0.2,"time",1));
		////iTween.MoveTo(bg,iTween.Hash("x",0,"time",1));
		
		if (scrollView != null )
		{
			scrollView.localPosition = Vector3.zero;
			UIPanel panel = scrollView.GetComponent<UIPanel>();
			panel.clipOffset = Vector2.zero;
		}

		/*for (int i = groupGrid.transform.childCount -1; i >= 0; i--)
		{
			GameObject go = groupGrid.transform.GetChild(i).gameObject;
			Destroy(go);
		}*/
	}


	void onHomeClick(GameObject go)
	{
		transform.gameObject.SetActive(false);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, true, (int)EVENT_GROUP.mainUI);
	}
	
	public void getDrugItem()
	{
		itemInfo = DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).getBattleItemList();
		for(int i=0 ;i<itemInfo.Length; i++)
		{
			GameObject item = GameObject.Find("drugitem" + i).gameObject;
			item.transform.FindChild("count").gameObject.SetActive(true);
			UILabel countLabel = item.transform.FindChild("count").GetComponent<UILabel>();

			if (itemInfo[i].amount > 0)
			{
				countLabel.text = "X" + itemInfo[i].amount.ToString();
			}
			else
				item.transform.FindChild("count").gameObject.SetActive(false);

			UILabel nameLabel = item.transform.FindChild("name").GetComponent<UILabel>();
			nameLabel.text = itemInfo[i].name;

			UISprite icon = item.transform.FindChild("drugicon").GetComponent<UISprite>();
			icon.spriteName = itemInfo[i].type.ToString();		

		}
	}


	public void sendCurrItem()
	{
		if (groupGrid != null)
		{
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
			
			BetterList<Transform> childList = groupGrid.GetChildList();
			for (int i = 0; i < childList.size; i ++)
			{
				if (GeometryUtility.TestPlanesAABB(planes,childList[i].gameObject.GetComponent<Collider>().bounds))
				{					
					teamIndex = int.Parse(childList[i].transform.Find("groupPnl/groupindex").GetComponent<UILabel>().text);
				}
			}
		}
		
		DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).nCurTeamId = teamIndex -1;
		DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).SendHeroFormationMsg();
	}

}
