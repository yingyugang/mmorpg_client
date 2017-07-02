using UnityEngine;
using System.Collections;
using DataCenter;
using BaseLib;

public class MadeStoneController : MonoBehaviour {
	
	private UILabel tips;
	private float times;
	private bool isActive;
	private UIPlayTween playtween;
	public StoneItem curItem;
	private bool result,init=false;
	private GameObject[] Items = new GameObject[4];
	private UILabel stoneName,desc,amount,soul,gotSoul,mName1,mName2,need1,need2,got1,got2;
	private GameObject StoneList;
	private UILabel maxCount,curCount;
	private uint curGrid;
	private UserInfo info;

	void Start () {
		init = true;
		StoneList = UI.PanelStack.me.FindPanel ("Scale/NewVillage/PanelStoneList/Bg");
		UI.PanelTools.setBtnFunc(transform, "mItem/btnMade", onMadeDrugClick);
		UI.PanelTools.setBtnFunc(transform, "stonetitle/back", onBackClick);
		tips = this.transform.Find ("tips").GetComponent<UILabel> ();
		playtween = tips.GetComponent<UIPlayTween>();
		stoneName = this.transform.Find ("stoneitem/StonePnl/StoneName").GetComponent<UILabel> ();
		desc = this.transform.Find ("stoneitem/StonePnl/desc").GetComponent<UILabel> ();
		amount=this.transform.Find ("stoneitem/StonePnl/got").GetComponent<UILabel> ();
		soul= this.transform.Find ("mItem/soul").GetComponent<UILabel> ();
		gotSoul = this.transform.Find ("mItem/gotSoul").GetComponent<UILabel> ();
		curCount = this.transform.Find ("ItemCount/curItem").GetComponent<UILabel> ();
		maxCount = this.transform.Find ("ItemCount/maxItem").GetComponent<UILabel> ();

		for(int i= 0; i< Items.Length; i++)
		{
			Items[i] = UI.PanelStack.me.FindPanel("Scale/NewVillage/PanelStoneList/PanelMadeStone/mItem/Item"+(i+1).ToString());
		}

		EventSystem.register((int)EVENT_MAINUI.itemMake, onMake, (int)DataCenter.EVENT_GROUP.mainUI);
	}
	void OnEnable()
	{
		if(!init)
			Start ();
		result = true;
		showItem();
	}

	void onMake(int nEvent, System.Object param)
	{
		AudioManager.me.PlayBtnActionClip();
		showItem ();
	}

	void showItem()
	{
		curGrid = DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).getItemCount ();
		info = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser;
		if(curGrid>=info.maxItem)
			curCount.color = Color.red;
		else
			curCount.color = Color.white;
		curCount.text = curGrid.ToString ();
		maxCount.text ="/"+ info.maxItem.ToString ();

		ConfigTable tableFormula = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ITEM_FORMULA);
		if (tableFormula==null)
			return;
		ConfigRow formula = tableFormula.getRow(DICT_ITEM_FORMULA.ITEM_TYPEID,curItem.typeid);

		this.transform.Find("stoneitem/StonePnl/bgicon/drugicon").GetComponent<UISprite>().spriteName = curItem.typeid.ToString();
		stoneName.text = curItem.Sname;
		desc.text = curItem.desc;
		amount.text ="持有数x"+  DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).getItemCountByType ((uint)curItem.typeid).ToString();

		int soulValue = formula.getIntValue (DICT_ITEM_FORMULA.COST_SOUL);
		gotSoul.text = "[ Hold:" + info.soul + " ]";
		if(info.soul < (ulong)soulValue)
		{
			soul.color = Color.red;
			result = false;
		}
		else
		{
			soul.color = Color.green;
		}
		soul.text = soulValue.ToString();

		DICT_ITEM_FORMULA[] itemTypeId = new DICT_ITEM_FORMULA[4];
		itemTypeId [0] = DICT_ITEM_FORMULA.COST_ITEM1_TYPEID;
		itemTypeId [1] = DICT_ITEM_FORMULA.COST_ITEM2_TYPEID;
		itemTypeId [2] = DICT_ITEM_FORMULA.COST_ITEM3_TYPEID;
		itemTypeId [3] = DICT_ITEM_FORMULA.COST_ITEM4_TYPEID;
		
		DICT_ITEM_FORMULA[] itemSoulCost = new DICT_ITEM_FORMULA[4];
		itemSoulCost [0] = DICT_ITEM_FORMULA.COST_AMOUNT1;
		itemSoulCost [1] = DICT_ITEM_FORMULA.COST_AMOUNT2;
		itemSoulCost [2] = DICT_ITEM_FORMULA.COST_AMOUNT3;
		itemSoulCost [3] = DICT_ITEM_FORMULA.COST_AMOUNT4;
		
		for(int i=0 ; i< Items.Length; i++)
		{
			int mId = formula.getIntValue (itemTypeId[i]);
			if(mId == 0)
			{
				Items[i].SetActive(false);
				continue;
			}
			Items[i].SetActive(true);
			Items[i].transform.Find("logo/Sprite").GetComponent<UISprite>().spriteName = mId.ToString();
			int tempNeed = formula.getIntValue(itemSoulCost[i]);
			Items[i].transform.Find("need").GetComponent<UILabel>().text = "/" + tempNeed.ToString();
			ItemInfo item = new ItemInfo ();
			item.init (mId);
			Items[i].transform.Find("mName").GetComponent<UILabel>().text = item.name;
			int tempGot = DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).getItemCountByType ((uint)mId);
			UILabel got= Items[i].transform.Find("got").GetComponent<UILabel>();
			got.text = tempGot.ToString();
			if(tempNeed>tempGot)
			{
				got.color = Color.red;
				result = false;
			}
			else
			{
				got.color=Color.green;
			}
		}
	}

	void Update () {
		times = times + Time.deltaTime;
		if (isActive && times > 1)
		{
			isActive = false;
			tips.transform.gameObject.SetActive(false);
		}
	}
	
	void onMadeDrugClick(GameObject go)
	{
		tips.transform.gameObject.SetActive(true);
		playtween.Play(true);
		isActive = true;
		times = 0;
		if(curGrid >= info.maxItem)
		{
			tips.text = "背包已满！";
		}
		else
		{
			if (!result)
				tips.text = "材料不足！";
			else
			{
				DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).makeItem((uint)curItem.typeid,1);
				tips.text="合成成功！";
				showItem();
			}
		}

	}
	
	void onBackClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
		this.transform.gameObject.SetActive(false);
		StoneList.SetActive (false);
		StoneList.SetActive (true);
		//panelStoneList.SetActive(true);
	}
}

