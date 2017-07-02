using UnityEngine;
using System.Collections;
using DataCenter;
using BaseLib;

public class MadeDrugController : MonoBehaviour {
	
	private UILabel tips;
	private float times,pressTime,t;
	private bool isActive, isFull;
	private UIPlayTween playtween;
	public StoneItem curItem;
	private bool result,init=false;
	private GameObject[] Items = new GameObject[4];
	private GameObject DrugList;
	private UILabel drugName,desc,amount/*,mName1,mName2,need1,need2,got1,got2*/,maxCount,curCount;
	private bool isPress;
	private int count , drugAmount, mRecords;
	private int[] needList = new int[4];
	private int[] gotList = new int[4];
	private uint curGrid ;
	private UserInfo info ;

	public  UIButton setBtnPress(Transform tfParent, string strName, UIEventListener.BoolDelegate func)
	{
		UIButton btn =UI.PanelTools.findChild<UIButton>(tfParent, strName);
		if (btn == null)
			return null;
		UIEventListener.Get(btn.gameObject).onPress = func;
		return btn;
	}

	void Start () {
		if (init)
			return;
		isPress = false;
		count = 0;
		init = true;
		DrugList = UI.PanelStack.me.FindPanel ("Scale/NewVillage/PanelDrugList/Bg");
		setBtnPress (transform, "mItem/btnMade",onMadePress);
		UI.PanelTools.setBtnFunc(transform, "dungeontitle/back", onBackClick);
		tips = this.transform.Find ("tips").GetComponent<UILabel> ();
		playtween = tips.GetComponent<UIPlayTween>();
		drugName = this.transform.Find ("drugitem/drugPnl/drugname").GetComponent<UILabel> ();
		desc = this.transform.Find ("drugitem/drugPnl/desc").GetComponent<UILabel> ();
		amount=this.transform.Find ("drugitem/drugPnl/got").GetComponent<UILabel> ();
		curCount = this.transform.Find ("ItemCount/curItem").GetComponent<UILabel> ();
		maxCount = this.transform.Find ("ItemCount/maxItem").GetComponent<UILabel> ();
		this.transform.FindChild("eff").gameObject.SetActive(false);
		for(int i= 0; i< Items.Length; i++)
		{
			Items[i] = UI.PanelStack.me.FindPanel("Scale/NewVillage/PanelDrugList/PanelMadeDrug/mItem/Item"+(i+1).ToString());
		}
		EventSystem.register((int)EVENT_MAINUI.itemMake, onMake, (int)DataCenter.EVENT_GROUP.mainUI);
	}
	void OnEnable()
	{
		Start ();
		result = true;
		isPress = false;
		showItem();
	}

	void onMake(int nEvent, System.Object param)
	{
		showItem ();
	}
	void showItem()
	{
		mRecords = 0;
		curGrid = DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).getItemCount ();
		info = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser;
		if(curGrid>=info.maxItem)
		{
			curCount.color = Color.red;
			isFull = true;
		}
		else
		{
			curCount.color = Color.white;
			isFull = false;
		}
		curCount.text = curGrid.ToString ();
		maxCount.text ="/"+ info.maxItem.ToString ();

		ConfigTable tableFormula = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ITEM_FORMULA);
		if (tableFormula==null)
			return;
		ConfigRow formula = tableFormula.getRow(DICT_ITEM_FORMULA.ITEM_TYPEID,curItem.typeid);

		this.transform.Find("drugitem/drugPnl/bgicon/drugicon").GetComponent<UISprite>().spriteName = curItem.typeid.ToString();
		drugName.text = curItem.Sname;
		desc.text = curItem.desc;

		drugAmount = DataManager.getModule<DataItem> (DATA_MODULE.Data_Item).getItemCountByType ((uint)curItem.typeid);
		amount.text = "持有数x"+drugAmount.ToString();
	

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
			Items[i].transform.Find("need").GetComponent<UILabel>().text ="/"+ tempNeed.ToString();
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
			needList[i]=tempNeed;
			gotList[i]=tempGot;
			mRecords++;
		}
	}

	bool tempShow()
	{
		if(isFull)
			return false;
		amount.text = (drugAmount + count).ToString();
		for(int i= 0; i<mRecords ; i++)
		{
			UILabel got= Items[i].transform.Find("got").GetComponent<UILabel>();
			int tempGot = gotList[i] - needList[i] * count;
			got.text = "持有数x" + tempGot.ToString();
			if(tempGot < needList[i])
			{
				got.color = Color.red;
				tips.transform.gameObject.SetActive(true);
				playtween.Play(true);
				isActive = true;
				times = 0;
				return false;
			}
			else
			{
				got.color=Color.white;
				this.transform.FindChild("eff").gameObject.SetActive(true);
			}
		}
		return true;
	}

	void Update () {
		times = times + Time.deltaTime;
		if (isActive && times > 0.8f)
		{
			isActive = false;
			tips.transform.gameObject.SetActive(false);
			this.transform.FindChild("eff").gameObject.SetActive(false);
		}

		if(isPress)
		{
			if(Time.time - pressTime < 1)
			{
				return;
			}
			else
			{
				if(Time.time - t > 0.2f)
				{
					if(tempShow())
						count ++;
					else
					{
						if(isFull)
							tips.text ="背包已满！";
						else
							tips.text = "材料不足！";
						this.transform.FindChild("eff").gameObject.SetActive(false);
					}
					t= Time.time;
				}
			}
		}
	}

	void MadeOnce()
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
				this.transform.FindChild("eff").gameObject.SetActive(true);
				count = 1;
				tips.text="";
			}
		}
	}

	void  MadeMore()
	{
		DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).makeItem((uint)curItem.typeid,(uint)(count));
		count=0;
	}

	void onMadePress(GameObject go , bool state)
	{
		isPress = state;
		pressTime = Time.time;
		if(isPress)
		{
			MadeOnce();
		}
		if(!isPress )
		{
			MadeMore();
			isActive = true;
		}
	}

	void onBackClick(GameObject go)
	{
		this.transform.gameObject.SetActive(false);
		DrugList.SetActive (false);
		DrugList.SetActive (true);
	}
}
