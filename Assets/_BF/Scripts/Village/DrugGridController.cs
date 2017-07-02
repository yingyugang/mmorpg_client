using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;

public class DrugGridController : MonoBehaviour {

	public struct drugInfo
	{
		public string name;
		public string amount;
		public string icon;
		public string desc;
	}
	
	public List<drugInfo> dungInfo;
	private GameObject PanelMadeDrug;
	private List<GameObject> drugList;
	
	private UIGrid grid;

	void Start () {	
		dungInfo = new List<drugInfo>();
		PanelMadeDrug = UI.PanelStack.me.FindPanel ("Scale/NewVillage/PanelDrugList/PanelMadeDrug");
		//Invoke("getDrugList",0.5f);  
		//grid = gameObject.transform.GetComponentInChildren<UIGrid>();
		//EventSystem.register((int)EVENT_MAINUI.itemUpdate, onItemUpdate, (int)DataCenter.EVENT_GROUP.mainUI);
	}
	void OnEnable()
	{
		if (grid == null)
			grid = transform.GetComponent<UIGrid>();
		grid.transform.localPosition = new Vector3 (-65,58,0);
		iTween.MoveFrom(this.gameObject,iTween.Hash("x",3,"time",1));
		if(PanelMadeDrug !=null)
			PanelMadeDrug.SetActive(false);
		addDrugItem();

	}

	void addDrugItem()
	{	
		clearDrugItem ();
		drugList = new List<GameObject>();

		ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ITEM);
		ConfigTable tableFormula = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ITEM_FORMULA);
		if (table == null || tableFormula==null)
			return;
		
		buildingInfo buildinfo= DataManager.getModule<DataBuilding>(DATA_MODULE.Data_Building).getBuilding(BUILD_TYPE.BUILD_SYNTHETIZE);
		if(buildinfo==null)
			return;
		ConfigRow[] rows = table.getRows (DICT_ITEM.SORT, (int)ITEM_SORT.drug);
		
		foreach(ConfigRow row in rows)
		{
			int typeid = row.getIntValue(DICT_ITEM.ITEM_TYPEID);
			
			ConfigRow formula = tableFormula.getRow(DICT_ITEM_FORMULA.ITEM_TYPEID,typeid);
			if(formula==null)
				continue;
			int buildLv = formula.getIntValue(DICT_ITEM_FORMULA.BUILDING_LEVEL);
			if(buildLv > buildinfo.level)
				continue;
			
			ItemInfo itemInfo= DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).getItem((uint)typeid);
			if(itemInfo==null)
			{
				itemInfo = new ItemInfo();
				itemInfo.init(typeid);
			}
			
			GameObject drugItem = (GameObject) Instantiate(Resources.Load("Village/stoneitem"));
			drugItem.SetActive(true);
			StoneItem itemData = drugItem.AddComponent<StoneItem>();
			itemData.typeid = typeid;
			itemData.maxUsed =(int)itemInfo.maxUsed;
			itemData.Sname= itemInfo.name;
			itemData.desc =itemInfo.desc;
			itemData.amount=(int)DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).getItemCountByType(itemInfo.type);
			
			drugItem.transform.Find("stonePnl/stonename").GetComponent<UILabel>().text = itemInfo.name;
			drugItem.transform.Find("stonePnl/amount").GetComponent<UILabel>().text="持有数x" + itemData.amount;
			drugItem.transform.Find("stonePnl/desc").GetComponent<UILabel>().text=itemInfo.desc;
			drugItem.transform.Find("stonePnl/bgicon/drugicon").GetComponent<UISprite>().spriteName = itemInfo.type.ToString();
			drugItem.transform.parent = transform;
			drugItem.transform.localScale = Vector3.one;
			UIEventListener.Get(drugItem).onClick = ItemClick;
			drugList.Add(drugItem);
		}
		grid.repositionNow = true;
	}
	void clearDrugItem()
	{
		for (int i = transform.childCount -1; i >=0; i--) 
		{
			Destroy(transform.GetChild(i).gameObject);
		}
	}
	void ItemClick(GameObject click)
	{
		AudioManager.me.PlayBtnActionClip();
		StoneItem data = click.GetComponent<StoneItem> ();
		MadeDrugController madeObj = PanelMadeDrug.GetComponent<MadeDrugController> ();
		if (madeObj != null)
			madeObj.curItem = data;
		PanelMadeDrug.SetActive(true);
	}
}
