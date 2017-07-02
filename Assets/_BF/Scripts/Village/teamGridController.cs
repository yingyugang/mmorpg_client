using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
public class teamGridController : MonoBehaviour {
	
	private GameObject panelTeamList;

	private List<GameObject> teamList;
//	public GameObject panelDrugTeam;
	//public static ItemSet[] Iset = new ItemSet[5];
	private UIGrid grid;
	public ItemInfo[] itemInfo;
	public static uint[] itemID = new uint[5];

	
	void Start () {	
		if (grid == null)
			grid = transform.GetComponent<UIGrid>();
		//teamItem = Resources.LoadAssetAtPath("Assets/_BF/Prefabs/Item.prefab",typeof(GameObject)) as GameObject;//UI.PanelStack.me.FindPanel ("Scale/NewVillage/PanelStuffMgr/PanelDrugTeam/Bg/Item");
		//teamItem = (GameObject)Instantiate (Resources.Load ("Item"));
//		if(teamItem !=null)
//			teamItem.SetActive (false);
		iTween.MoveFrom(this.gameObject,iTween.Hash("x",3,"time",1));
		panelTeamList = UI.PanelStack.me.FindPanel ("Scale/NewVillage/PanelStuffMgr/PanelDrugTeam/PanelTeamList");
		UI.PanelTools.setBtnFunc(transform.parent.parent, "btnCharge", btnChargeClick);
		UI.PanelTools.setBtnFunc(transform.parent.parent, "btnReset", btnResetClick);
		EventSystem.register((int)EVENT_MAINUI.itemSetBattle, onUpdateData, (int)DataCenter.EVENT_GROUP.mainUI);

	}
	
	void onUpdateData(int nEvent, System.Object param)
	{
		showTeam();
	}

	void OnEnable()
	{
		if (grid == null)
			grid = transform.GetComponent<UIGrid>();
		if(panelTeamList !=null)
			panelTeamList.SetActive (false);
		showTeam ();
	}

	void showTeam()
	{
		clearStoneItem ();
		teamList = new List<GameObject>();
		itemInfo = DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).getBattleItemList();
		for(int i=0;i<itemInfo.Length;i++)
		{
			GameObject TeamItem = (GameObject) Instantiate(Resources.Load("Village/Item"));

		//	GameObject TeamItem = Instantiate(teamItem) as GameObject;
			TeamItem.SetActive(true);
			StoneItem itemData = TeamItem.AddComponent<StoneItem>();
			itemData.index = i;
			itemData.typeid=(int)itemInfo[i].type;
			itemData.amount=(int)itemInfo[i].amount;
			itemData.id=(int)itemInfo[i].id;
			if(itemData.typeid!=0 && itemData.amount!=0)
			{
				itemID[i] = itemInfo[i].type;
				TeamItem.transform.Find("Icon").GetComponent<UISprite>().alpha=1;
				TeamItem.transform.Find("Icon").GetComponent<UISprite>().spriteName = itemInfo[i].type.ToString();
				TeamItem.transform.Find("Background").GetComponent<UISprite>().spriteName= "PropSelectionIconBar2";
				TeamItem.transform.Find("nullBG").GetComponent<UISprite>().alpha=0;
				TeamItem.transform.Find("name").GetComponent<UILabel>().text = itemInfo[i].name;
				TeamItem.transform.Find("num").GetComponent<UILabel>().text= "x"+((int)itemInfo[i].maxUsed>(int)itemInfo[i].amount?(int)itemInfo[i].amount:(int)itemInfo[i].maxUsed);
			}
			else
			{ 
				TeamItem.transform.Find("Icon").GetComponent<UISprite>().alpha=0;
				TeamItem.transform.Find("Background").GetComponent<UISprite>().alpha=0;
				TeamItem.transform.Find("nullBG").GetComponent<UISprite>().alpha=1;
				TeamItem.transform.Find("name").GetComponent<UILabel>().text = "";
				TeamItem.transform.Find("num").GetComponent<UILabel>().text= "";
			}
				TeamItem.transform.Find("equ").GetComponent<UILabel>().text= "";
				TeamItem.transform.parent = transform;
				TeamItem.transform.localScale = Vector3.one;
				UIEventListener.Get(TeamItem).onClick = ItemClick;
				teamList.Add(TeamItem);
		}
		grid.repositionNow = true;
	}

	void clearStoneItem()
	{
		for (int i = transform.childCount -1; i >=0; i--) 
		{
			Destroy(transform.GetChild(i).gameObject);
		}	
		for(int i=0; i<itemID.Length; i++)
			itemID[i] = 0;
	}

	void ItemClick(GameObject click)
	{
		AudioManager.me.PlayBtnActionClip();
		showTeam ();
	//	panelDrugTeam.SetActive(false);
		StoneItem data = click.GetComponent<StoneItem> ();
	//	TeamListGrid madeObj = panelTeamList.GetComponent<TeamListGrid> ();
		//if (madeObj != null)
		TeamListGrid.curItem = data;		
		panelTeamList.SetActive(true);
		//UI.PanelStack.me.goNext(panelTeamList);
	}

	void btnChargeClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
		Charge (itemInfo);
		/*
		List<ItemSet> theList = new List<ItemSet> ();
		for(int i=0; i < 5; i++)
		{
			if(itemInfo[i].type!=0)
			{
				ItemSet item = new ItemSet ();
				item.index = (uint)i;
			//	item.itemId = (uint)itemInfo[i].id;
				ItemInfo[] Info = DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).getItemListByType(itemInfo[i].type);
				item.itemId = Info[0].id;
				uint temp=(uint)DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).getItemCountByType(itemInfo[i].type);
				if(temp + itemInfo[i].amount > itemInfo[i].maxUsed)
					item.count = itemInfo[i].maxUsed;
				else
					item.count = temp + itemInfo[i].amount;
				//item.count = (uint)0;
				theList.Add(item);
			}
		}
		DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).setBattleItem (theList.ToArray());
		*/
	}

	public void Charge(ItemInfo[] iInfo)
	{
		List<ItemSet> theList = new List<ItemSet> ();
		for(int i=0; i < 5; i++)
		{
			if(iInfo[i].type!=0)
			{
				ItemSet item = new ItemSet ();
				item.index = (uint)i;
				//	item.itemId = (uint)iInfo[i].id;
				ItemInfo[] Info = DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).getItemListByType(iInfo[i].type);
				item.itemId = Info[0].id;
				uint temp=(uint)DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).getItemCountByType(iInfo[i].type);
				if(temp + iInfo[i].amount > iInfo[i].maxUsed)
					item.count = iInfo[i].maxUsed;
				else
					item.count = temp + iInfo[i].amount;
				//item.count = (uint)0;
				theList.Add(item);
			}
		}
		DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).setBattleItem (theList.ToArray());
	}

	void btnResetClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
		List<ItemSet> theList = new List<ItemSet> ();
		for(int i=0; i < 5; i++)
		{
			if(itemInfo[i].type!=0)
			{
				ItemSet item = new ItemSet ();
				item.index = (uint)i;
				item.itemId = (uint)0;
				item.count = (uint)0;
				theList.Add(item);
			}
		}
		DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).setBattleItem (theList.ToArray());
	}
}
