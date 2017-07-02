using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
public class stuffGridController : MonoBehaviour {

	//public GameObject stuffitem;
	private UIGrid grid;
	private Dictionary<uint,GameObject> stuffList;
	private GameObject drugDetail;

	void Start () {
		iTween.MoveFrom(this.gameObject,iTween.Hash("x",3,"time",1));
		drugDetail = UI.PanelStack.me.FindPanel ("Scale/NewVillage/PanelStuffMgr/PanelStuffList/PanelDrugDetail");
	}
	
	void OnEnable()
	{
		if (grid == null)
		{
			grid = transform.GetComponent<UIGrid>();
		}
		grid.transform.localPosition = new Vector3(-282,60,0);
		addStoneItem();
	}
	
	void addStoneItem()
	{
		clearStoneItem ();
		stuffList = new Dictionary<uint,GameObject>();
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

			string[] a= Info.name.Split('-','－');
			stuffItem.transform.Find("name").GetComponent<UILabel>().text = a[0];
			stuffItem.transform.Find("amount").GetComponent<UILabel>().text= "x"+itemData.amount;
			stuffItem.transform.Find("icon").GetComponent<UISprite>().spriteName = Info.type.ToString();
			stuffItem.transform.Find("e").GetComponent<UILabel>().text="";
			if(DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).isEquipped(itemData.id))
			{
				stuffItem.transform.Find("e").GetComponent<UILabel>().text="E";
			}
			stuffItem.transform.parent = transform;
			stuffItem.transform.localScale = Vector3.one;
			UIEventListener.Get(stuffItem).onClick = ItemClick;
			stuffList[Info.id] = stuffItem;
		}
		grid.repositionNow = true;
		/*
		clearStoneItem ();
		stuffList = new List<GameObject>();
		ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ITEM);
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
			GameObject stuffItem = (GameObject) Instantiate(Resources.Load("Village/stuffitem"));
			stuffItem.SetActive(true);
			StoneItem itemData = stuffItem.AddComponent<StoneItem>();
			itemData.typeid = typeid;
			itemData.maxUsed =(int)itemInfo.maxUsed;
			itemData.Sname= itemInfo.name;
			itemData.desc =itemInfo.desc;
			itemData.desc_ex = itemInfo.descEx;
			itemData.amount=judge;
			string[] a= itemInfo.name.Split('-','－');
			stuffItem.transform.Find("name").GetComponent<UILabel>().text = a[0];
			stuffItem.transform.Find("amount").GetComponent<UILabel>().text= "x"+itemData.amount;
			stuffItem.transform.Find("icon").GetComponent<UISprite>().spriteName = itemInfo.type.ToString();
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
		StoneItem data = click.GetComponent<StoneItem> ();
		drughandBookDetail madeObj = drugDetail.GetComponent<drughandBookDetail> ();
		if (madeObj != null)
			madeObj.curItem = data;
		drugDetail.SetActive(true);
	}
}
