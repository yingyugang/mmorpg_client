using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
public class TeamListGrid : MonoBehaviour {

	private UIGrid grid;
	private List<GameObject> teamList;
	public static StoneItem curItem;
	private GameObject suitUp;
	private GameObject PnlTeamList;
	public GameObject PnlDrugTeam;
	void Start () {
		if (grid == null)
		{
			grid = transform.GetComponent<UIGrid>();
			grid.transform.position= new Vector3(-290,30,0);
		}
		UI.PanelTools.setBtnFunc(transform.parent.parent, "del", onDelClick);
		suitUp = UI.PanelStack.me.FindPanel ("Scale/NewVillage/PanelStuffMgr/PanelDrugTeam/PanelTeamList/PanleSuitUp");
		PnlTeamList = UI.PanelStack.me.FindPanel ("Scale/NewVillage/PanelStuffMgr/PanelDrugTeam/PanelTeamList");
		PnlDrugTeam = UI.PanelStack.me.FindPanel ("Scale/NewVillage/PanelStuffMgr/PanelDrugTeam");
		iTween.MoveFrom(this.gameObject,iTween.Hash("x",3,"time",1));
	}
	
	void OnEnable()
	{
		if (grid == null)
			grid = transform.GetComponent<UIGrid>();
		if(suitUp != null)
			suitUp.SetActive (false);
		addStoneItem();
	}

	void addStoneItem()
	{	
		clearStoneItem ();
		teamList = new List<GameObject>();

		ItemInfo[] InfoList= DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).getItemList (ITEM_SORT.drug);
		foreach(ItemInfo Info in InfoList)
		{
			GameObject stuffItem =(GameObject) Instantiate(Resources.Load("Village/Item"));
			stuffItem.SetActive(true);
			StoneItem itemData = stuffItem.AddComponent<StoneItem>();
			itemData.id = (int)Info.id;
			itemData.maxUsed =(int)Info.maxUsed;
			itemData.Sname= Info.name;
			itemData.desc =Info.desc;
			itemData.amount=0;
			itemData.index = curItem.index;
			itemData.typeid=(int)Info.type;
			stuffItem.transform.Find("nullBG").GetComponent<UISprite>().alpha=0;
			stuffItem.transform.Find("Icon").GetComponent<UISprite>().spriteName = Info.type.ToString();
			stuffItem.transform.Find("name").GetComponent<UILabel>().text = Info.name;
			stuffItem.transform.Find("num").GetComponent<UILabel>().text= "x"+Info.amount;
			stuffItem.transform.Find("equ").GetComponent<UILabel>().text="";
			stuffItem.transform.Find("Background").GetComponent<UISprite>().spriteName = "PropSelectionIconBar";
			for(int i=0; i<5 ; i++)
			{
				if(Info.type == teamGridController.itemID[i])
				{
					stuffItem.transform.Find("equ").GetComponent<UILabel>().text="E";
					stuffItem.transform.Find("Background").GetComponent<UISprite>().spriteName = "PropSelectionIconBar2";
					if(curItem.index != i)
					{
						stuffItem.transform.Find("Background").GetComponent<UISprite>().alpha=0.5f;
						itemData.invalid=true;
					}
					else
					{
						itemData.amount=(int)curItem.amount;
					}
					break;
				}
			}
			stuffItem.transform.parent = transform;
			stuffItem.transform.localScale = Vector3.one;
			UIEventListener.Get(stuffItem).onClick = ItemClick;
			teamList.Add(stuffItem);
		}
		grid.repositionNow = true;
		/*
		ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.dict_item);
		if (table == null )
			return;
		ConfigRow[] rows = table.getRows (DICT_ITEM.SORT, (int)ITEM_SORT.drug);
		foreach(ConfigRow row in rows)
		{
			int typeid = row.getIntValue(DICT_ITEM.ITEM_TYPEID);
			ItemInfo itemInfo= DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).getItem((uint)typeid);
			if(itemInfo==null)
			{
				itemInfo = new ItemInfo();
				itemInfo.init(typeid);
			}
			int judge=(int)DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).getItemCountByType(itemInfo.type);
			if(judge==0)
				continue;
			GameObject stuffItem = Instantiate(teamItem) as GameObject;
			stuffItem.SetActive(true);
			StoneItem itemData = stuffItem.AddComponent<StoneItem>();
			itemData.typeid = typeid;
			itemData.maxUsed =(int)itemInfo.maxUsed;
			itemData.name= itemInfo.name;
			itemData.desc =itemInfo.desc;
			itemData.amount=judge;

			stuffItem.transform.Find("nullBG").GetComponent<UISprite>().alpha=0;
			stuffItem.transform.Find("name").GetComponent<UILabel>().text = itemInfo.name;
			stuffItem.transform.Find("num").GetComponent<UILabel>().text= "x"+judge;
			stuffItem.transform.parent = transform;
			stuffItem.transform.localScale = Vector3.one;
			UIEventListener.Get(stuffItem).onClick = ItemClick;
			teamList.Add(stuffItem);
		}
		*/

	}
	void clearStoneItem()
	{
		for (int i = transform.childCount -1; i >=0; i--) 
		{
			Destroy(transform.GetChild(i).gameObject);
		}			
	}

	void ItemClick(GameObject click)
	{
		StoneItem data = click.GetComponent<StoneItem> ();
		if(!data.invalid)
		{
			//PnlTeamList.SetActive (false);
			SuitUpController suitup = suitUp.GetComponent<SuitUpController> ();
			if(suitup!=null)
				suitup.curItem=data;
			suitUp.SetActive (true);
		}
	}

	void onDelClick(GameObject click)
	{

		if(curItem.typeid!=0)
		{
			List<ItemSet> theList = new List<ItemSet> ();
			ItemSet item = new ItemSet ();
			item.index = (uint)curItem.index;
			item.itemId = (uint)0;
			item.count = (uint)0;
			theList.Add(item);
			DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).setBattleItem (theList.ToArray());
		}
		PnlTeamList.SetActive (false);
		//PnlDrugTeam.SetActive (true);
	}
}
