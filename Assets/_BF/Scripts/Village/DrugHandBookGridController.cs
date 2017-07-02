using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
public class DrugHandBookGridController : MonoBehaviour {

	private GameObject drugDetail;
	private UIGrid grid;
	private List<GameObject> stuffList;
	
	void Start () {
		if (grid == null)
		{
			grid = transform.GetComponent<UIGrid>();
			grid.transform.position= new Vector3(-290,30,0);
		}
		drugDetail = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse/PanelHandbook/PanelDrug/PanelDrugDetail");
		iTween.MoveFrom(this.gameObject,iTween.Hash("x",3,"time",1));
	}
	
	void OnEnable()
	{
		if (grid == null)
			grid = transform.GetComponent<UIGrid>();
		if(drugDetail !=null)
			drugDetail.SetActive (false);
		addStoneItem();
	}

	bool isOpen(uint[] list, int NO)
	{

		int a = (NO-1) / 32 ;
		if((list[a] >> ((NO-1) % 32) & 1) == 1)
			return true;
		return false;
	}

	void addStoneItem()
	{	
		clearStoneItem ();
		stuffList = new List<GameObject>();
		

		ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ITEM);
		if (table == null )
			return;
		foreach(ConfigRow row in table.rows)
		{
			int lid = row.getIntValue(DICT_ITEM.LIBRARY);
			int typeid = row.getIntValue(DICT_ITEM.ITEM_TYPEID);
			GameObject stuffItem = (GameObject) Instantiate(Resources.Load("Village/stuffitem"));				
			stuffItem.SetActive(true);
			StoneItem itemData = stuffItem.AddComponent<StoneItem>();
			itemData.typeid = typeid;
			itemData.libary = lid;
			itemData.Sname= BaseLib.LanguageMgr.getString(row.getIntValue(DICT_ITEM.NAME_ID));
			itemData.desc = BaseLib.LanguageMgr.getString(row.getIntValue(DICT_ITEM.DESC_ID));
			itemData.desc_ex = BaseLib.LanguageMgr.getString(row.getIntValue(DICT_ITEM.DESC_EX_ID));

			string[] a= itemData.Sname.Split('-','－');
			stuffItem.transform.Find("name").GetComponent<UILabel>().text = "?";
			stuffItem.transform.Find("amount").GetComponent<UILabel>().text= "";
			stuffItem.transform.Find("icon").GetComponent<UISprite>().spriteName = typeid.ToString();
		//	stuffItem.transform.Find("Background").GetComponent<UISprite>().alpha=0.5f;
			stuffItem.transform.Find("bg").GetComponent<UIWidget>().depth = 2;
			stuffItem.transform.parent = transform;
			stuffItem.transform.localScale = Vector3.one;
			UIEventListener.Get(stuffItem).onClick = ItemClick;

			//图鉴是否被开判断
			UserInfo info = DataManager.getModule<DataUser> (DATA_MODULE.Data_User).mainUser;
			if(info.itemIsOpen((uint)lid))
			//if(isOpen(info.itemLst,lid))
			{
				stuffItem.transform.Find("name").GetComponent<UILabel>().text = a[0];
				stuffItem.transform.Find("bg").GetComponent<UIWidget>().depth = 1;
			}
			//Debug.Log(stuffList.Count+":"+ isOpen( info.itemLst,stuffList.Count+1));
			stuffList.Add(stuffItem);
		}
		/*		
		ItemInfo[] InfoList= DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).getItemList (ITEM_SORT.all);
		foreach(ItemInfo Info in InfoList)
		{
			GameObject stuffItem = (GameObject)Instantiate(Resources.LoadAssetAtPath("Assets/_BF/Prefabs/stuffitem.prefab",typeof(GameObject)));
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
			stuffItem.transform.Find("amount").GetComponent<UILabel>().text= "";
			stuffItem.transform.Find("icon").GetComponent<UISprite>().spriteName = Info.type.ToString();
			stuffItem.transform.parent = transform;
			stuffItem.transform.localScale = Vector3.one;
			UIEventListener.Get(stuffItem).onClick = ItemClick;
			stuffList[Info.id] = stuffItem;
		}
		*/
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
		StoneItem data = click.GetComponent<StoneItem> ();
		drughandBookDetail madeObj = drugDetail.GetComponent<drughandBookDetail> ();
		if (madeObj != null)
			madeObj.curItem = data;
		drugDetail.SetActive(true);
		Debug.Log ("onclick");
	}
}
