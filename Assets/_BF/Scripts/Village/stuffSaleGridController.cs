using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
public class stuffSaleGridController : MonoBehaviour {

	//public GameObject stuffitem;
	//public GameObject stuffSale;
	private GameObject subSale;
	private UIGrid grid;
	private Dictionary<uint,GameObject> stuffList;

	void Start () {
		subSale = UI.PanelStack.me.FindPanel ("Scale/NewVillage/PanelStuffMgr/PanelStuffSale/PanelStuffSubSale");
		iTween.MoveFrom(this.gameObject,iTween.Hash("x",3,"time",1));
		//EventSystem.register((int)EVENT_MAINUI.itemList, onList, (int)DataCenter.EVENT_GROUP.mainUI);
		EventSystem.register((int)EVENT_MAINUI.itemDelete,onDelete,(int)EVENT_GROUP.mainUI);
		EventSystem.register ((int)EVENT_MAINUI.itemUpdate,onUpdateItem,(int)EVENT_GROUP.mainUI);
	}
	
	void OnEnable()
	{
		if (grid == null)
		{
			grid = transform.GetComponent<UIGrid>();
		}
		grid.transform.localPosition = new Vector3(-282,60,0);
		if(subSale !=null)
			subSale.SetActive (false);
		addStoneItem();
	}

	void onUpdateItem(int nEvent , System.Object param)
	{
		uint id = (uint)param;
		if (stuffList != null) 
		{
			if(stuffList.ContainsKey(id))
			{
				GameObject item = stuffList[id];
				if(item !=null)
				{
					ItemInfo ItemInfo = DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).getItem(id);
					item.transform.Find("amount").GetComponent<UILabel>().text= "x" + ItemInfo.amount;
					StoneItem data = item.GetComponent<StoneItem> ();
					data.amount = (int)ItemInfo.amount;
				}
			}
		}
	}

	void onDelete(int nEvent, System.Object param)
	{
		uint id = (uint)param;
		if (stuffList != null) 
		{
			if(stuffList.ContainsKey(id))
			{
				GameObject item = stuffList[id];
				stuffList.Remove(id);
				if(item!=null)
					Destroy(item);
				grid.repositionNow = true;
			}
		}

	}



	void addStoneItem()
	{	
		clearStoneItem ();
		stuffList = new Dictionary<uint,GameObject>();

		/*
		ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.dict_item);
		if (table == null )
			return;
		foreach(ConfigRow row in table.rows)
		{
			int typeid = row.getIntValue(DICT_ITEM.ITEM_TYPEID);
			ItemInfo Info= DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).getItem((uint)typeid);
			if(Info==null)
			{
				Info = new ItemInfo();
				Info.init(typeid);
			}
			int judge=(int)DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).getItemCountByType(Info.type);
			if(judge==0)
				continue;
//*/
		ItemInfo[] InfoList= DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).getItemList (ITEM_SORT.all);
		foreach(ItemInfo Info in InfoList)
		{
			GameObject stuffItem = (GameObject) Instantiate(Resources.Load("Village/stuffitem"));
			stuffItem.SetActive(true);
			StoneItem itemData = stuffItem.AddComponent<StoneItem>();
			itemData.typeid = (int)Info.type;
			itemData.maxUsed =(int)Info.maxUsed;
			itemData.Sname= Info.name;
			itemData.desc =Info.desc;
			itemData.price=(int)Info.price;
			itemData.amount=(int) Info.amount;
			itemData.id = (int)Info.id;
			itemData.invalid = false;
			string[] a= Info.name.Split('-','－');
			stuffItem.transform.Find("name").GetComponent<UILabel>().text = a[0];
			stuffItem.transform.Find("amount").GetComponent<UILabel>().text= "x"+itemData.amount;
			stuffItem.transform.Find("icon").GetComponent<UISprite>().spriteName = Info.type.ToString();
			stuffItem.transform.Find("e").GetComponent<UILabel>().text="";

			if(DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).isEquipped(itemData.id))
			{
				stuffItem.transform.Find("e").GetComponent<UILabel>().text="E";
				itemData.invalid = true;
			}
			stuffItem.transform.parent = transform;
			stuffItem.transform.localScale = Vector3.one;
			UIEventListener.Get(stuffItem).onClick = ItemClick;
			stuffList[Info.id] = stuffItem;
		}
		grid.repositionNow = true;

		/*
		ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.dict_item);
		if (table == null )
			return;
		foreach(ConfigRow row in table.rows)
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
			GameObject stuffItem = Instantiate(stuffitem) as GameObject;
			stuffItem.SetActive(true);
			StoneItem itemData = stuffItem.AddComponent<StoneItem>();
			itemData.typeid = typeid;
			itemData.maxUsed =(int)itemInfo.maxUsed;
			itemData.name= itemInfo.name;
			itemData.desc =itemInfo.desc;
			itemData.price=(int)itemInfo.price;
			itemData.amount=judge;
			itemData.id = (int)itemInfo.id;
			string[] a= itemInfo.name.Split('-','－');
			stuffItem.transform.Find("name").GetComponent<UILabel>().text = a[0];
			stuffItem.transform.Find("amount").GetComponent<UILabel>().text= "x"+itemData.amount;
			stuffItem.transform.parent = transform;
			stuffItem.transform.localScale = Vector3.one;
			UIEventListener.Get(stuffItem).onClick = ItemClick;
			stuffList.Add(stuffItem);
		}
		grid.repositionNow = true;
		*/
	}
	void clearStoneItem()
	{
		if(stuffList!=null)
			stuffList.Clear ();
		for (int i = transform.childCount -1; i >=0; i--) 
		{
			Destroy(transform.GetChild(i).gameObject);
		}			
	}
	void ItemClick(GameObject click)
	{
		AudioManager.me.PlayBtnActionClip();
		//stuffSale.SetActive(false);
		StoneItem data = click.GetComponent<StoneItem> ();
		if(!data.invalid)
		{
			subSaleController madeObj = subSale.GetComponent<subSaleController> ();
			if (madeObj != null)
				madeObj.curItem = data;
			subSale.SetActive(true);
		}
	}
}
