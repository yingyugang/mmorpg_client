using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
public class StoneGridController : MonoBehaviour {

	private GameObject panelMadeStone;
	private List<GameObject> stoneList;
	//public List<ItemInfo> stoneInfoList;
	//public ItemInfo[] stoneInfoList;
	//public GameObject panelStoneList;
	
	private GameObject madeDrug;
	
	private UIGrid grid;
	private uint build_lv;

	void Start () {	
		panelMadeStone = UI.PanelStack.me.FindPanel ("Scale/NewVillage/PanelStoneList/PanelMadeStone");
	}	

	void OnEnable()
	{
		if (grid == null)
			grid = transform.GetComponent<UIGrid>();
		grid.transform.localPosition = new Vector3 (-65,58,0);
		iTween.MoveFrom(this.gameObject,iTween.Hash("x",3,"time",1));
		if(panelMadeStone !=null)
			panelMadeStone.SetActive (false);
		addStoneItem();
	}

	void addStoneItem()
	{	
		clearStoneItem ();
		stoneList = new List<GameObject>();
		//stoneList.Clear ();
		ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ITEM);
		ConfigTable tableFormula = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ITEM_FORMULA);
		if (table == null || tableFormula==null)
		{
			return;
		}

		buildingInfo buildinfo= DataManager.getModule<DataBuilding>(DATA_MODULE.Data_Building).getBuilding(BUILD_TYPE.BUILD_STONE);
		if(buildinfo == null)
		{
			return;
		}
		ConfigRow[] rows = table.getRows (DICT_ITEM.SORT, (int)ITEM_SORT.stone);

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

			GameObject stoneItem = (GameObject) Instantiate(Resources.Load("Village/stoneitem"));				
			stoneItem.SetActive(true);
			StoneItem itemData = stoneItem.AddComponent<StoneItem>();
			itemData.typeid = typeid;
			itemData.maxUsed =(int)itemInfo.maxUsed;
			itemData.Sname= itemInfo.name;
			itemData.desc =itemInfo.desc;
			itemData.amount=(int)DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).getItemCountByType(itemInfo.type);

			stoneItem.transform.Find("stonePnl/stonename").GetComponent<UILabel>().text = itemInfo.name;
			stoneItem.transform.Find("stonePnl/amount").GetComponent<UILabel>().text="持有数x" + itemData.amount;
			stoneItem.transform.Find("stonePnl/desc").GetComponent<UILabel>().text=itemInfo.desc;
			stoneItem.transform.Find("stonePnl/bgicon/drugicon").GetComponent<UISprite>().spriteName = itemInfo.type.ToString();
			stoneItem.transform.parent = transform;
			stoneItem.transform.localScale = Vector3.one;
			UIEventListener.Get(stoneItem).onClick = ItemClick;
			stoneList.Add(stoneItem);
		}
		grid.repositionNow = true;
	}
	void clearStoneItem()
	{
		for (int i = transform.childCount -1; i >=0; i--) 
		{
			Destroy(transform.GetChild(i).gameObject);
		}
		//grid.GetChildList().Clear();
		/*if (stoneList != null)
		{
			foreach(GameObject obj in stoneList)
			{
				obj.SetActive(false);
			//	obj.transform.parent = null;
				//grid.RemoveChild(obj.transform);
				//DestroyImmediate(obj);
			}
			stoneList.Clear ();
		}*/				
	}
	void ItemClick(GameObject click)
	{
		//panelStoneList.SetActive(false);
		AudioManager.me.PlayBtnActionClip();
		StoneItem data = click.GetComponent<StoneItem> ();
		MadeStoneController madeObj = panelMadeStone.GetComponent<MadeStoneController> ();
		if (madeObj != null)
			madeObj.curItem = data;
		panelMadeStone.SetActive(true);
	}
}
