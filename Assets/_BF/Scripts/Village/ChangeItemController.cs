using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;

public class ChangeItemController : MonoBehaviour {

	public List<GameObject> itemList;
	public GameObject BtnLeft;
	public GameObject BtnRight;
	public UILabel soul;
	public UILabel desc;
	public UILabel lv;
	public UILabel lvUp;
	public GameObject BtnUpdate;
	public GameObject Buffer;
	public GameObject BgUpdate;
	private bool init = false;
	private UISlider slider;
	private bool pressState;
	private GameObject unOpen;
	private List<Vector3> posList;
	private List<int> posIndex;
	private GameObject UpEffect;
	//private int newItemIndex;
	//private int currItemIndex;
	// Use this for initialization
	private float t;
	private UIPlayTween playTween;
	private TweenPosition tweenPos;
	private TweenScale tweenScale;
	private UISprite itemSprite;
	private UILabel itemLevel;
	private int selectedIndex;

	private bool Finished;

	private uint soulCost ;
	struct unitInfo
	{
		public uint unitID;
		public uint level;
		public uint soul;
		public uint maxSoul;
		public string desc;
		public BUILD_TYPE type;
	}
	private List<unitInfo> unitList;

	void Start () {
		if(init)
			return;
		Finished = true;

		posIndex = new List<int>();
		posIndex.Add(0);
		posIndex.Add(1);
		posIndex.Add(2);
		posIndex.Add(3);
		posIndex.Add(4);
		posIndex.Add(5);

		posList = new List<Vector3>();
		posList.Add(new Vector3(-500,0,0));
		posList.Add(new Vector3(-250,0,0));
		posList.Add(new Vector3(0,0,0));
		posList.Add(new Vector3(250,0,0));
		posList.Add(new Vector3(500,0,0));
		posList.Add(new Vector3(750,0,0));

		//newItemIndex = 3;	
		//currItemIndex = 2;

		//playTween = itemList[newItemIndex].GetComponent<UIPlayTween>();
		//playTween.Play(true);
		iTween.MoveFrom(BgUpdate,iTween.Hash("x",3,"time",1));		
		UIEventListener.Get(BtnLeft).onClick = onLeftClick;
		UIEventListener.Get(BtnRight).onClick = onRightClick;
		UIEventListener.Get(BtnUpdate).onPress = onPressClick;
		//UIEventListener.Get (BtnUpdate).onDrop = onDropClick;
		//UIEventListener.Get(this.gameObject).onClick = onRightClick;
		UIEventListener.Get(this.gameObject).onDrag = onItemDrag;
		UIEventListener.Get(this.gameObject).onPress = onItemPress;
		unOpen = GameObject.Find ("Bg/BgUpdate/unOpen");
		UpEffect = GameObject.Find ("UpEffect");
		UpEffect.SetActive (false);
		slider = Buffer.GetComponent<UISlider>();
		init = true;
		showItems ();
		setUintInfo(2);
		selectedIndex = 2;
		t = Time.time;
		EventSystem.register((int)EVENT_MAINUI.buildUpdate, onLevelUpdated, (int)DataCenter.EVENT_GROUP.mainUI);
	}
	void onLevelUpdated(int nEvent, System.Object param)
	{
		buildingInfo buildInfo= DataManager.getModule<DataBuilding>(DATA_MODULE.Data_Building).getBuilding((BUILD_TYPE)param);
		unitInfo unit = unitList[selectedIndex];
		if(unit.level <buildInfo.level)
		{
			UpEffect.SetActive(true);
			t = Time.time;
		}
		showItems();
	}
	void showItems()
	{
		unitList = new List<unitInfo>();
		BUILD_TYPE[] buildType = new BUILD_TYPE[6];
		buildType [0] = BUILD_TYPE.BUILD_TREE;
		buildType [1] = BUILD_TYPE.BUILD_STONE;
		buildType [2] = BUILD_TYPE.BUILD_SYNTHETIZE;
		buildType [3] = BUILD_TYPE.BUILD_MOUNTAIN;
		buildType [4] = BUILD_TYPE.BUILD_RIVERS;
		buildType [5] = BUILD_TYPE.BUILD_FARMLAND;
		buildingInfo buildInfo;
		unitInfo itemInfo;
		for(int i=0; i<buildType.Length; i++)
		{
			itemInfo = new unitInfo();
			buildInfo= DataManager.getModule<DataBuilding>(DATA_MODULE.Data_Building).getBuilding(buildType[i]);
			if(buildInfo==null)
			{
				itemInfo.unitID = 0;
			}
			else
			{
			itemInfo.unitID = buildInfo.id;
			itemInfo.maxSoul = buildInfo.maxSoul;
			itemInfo.soul =  buildInfo.curSoul;
			itemInfo.level = buildInfo.level;
			itemInfo.type = buildInfo.type;
			if(buildType[i]==BUILD_TYPE.BUILD_STONE || buildType[i]==BUILD_TYPE.BUILD_SYNTHETIZE)
			{
				int type;
				if(buildType[i]==BUILD_TYPE.BUILD_STONE)
					type=(int)ITEM_SORT.stone;
				else
					type=(int)ITEM_SORT.drug;
				ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ITEM);
				ConfigTable tableFormula = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ITEM_FORMULA);
				ConfigRow[] rows = table.getRows (DICT_ITEM.SORT, type);
				if (table == null || tableFormula==null)
					return;
				foreach(ConfigRow row in rows)
				{
					int typeid = row.getIntValue(DICT_ITEM.ITEM_TYPEID);
					
					ConfigRow formula = tableFormula.getRow(DICT_ITEM_FORMULA.ITEM_TYPEID,typeid);
					if(formula==null)
						continue;
					int buildLv = formula.getIntValue(DICT_ITEM_FORMULA.BUILDING_LEVEL);
					if(buildLv == buildInfo.level+1)
					{					
						ItemInfo iInfo= DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).getItem((uint)typeid);
						if(iInfo==null)
						{
							iInfo = new ItemInfo();
							iInfo.init(typeid);
						}
						itemInfo.desc += iInfo.desc +"\n";
					}
				}
			}
			}
			unitList.Add(itemInfo);
		}


	}
	void OnEnable()
	{
		Start ();
		soulCost = 0;

		showItems ();
		pressState = false;
	}

	void Update () {
		if (pressState)
		{
			soulCost += 100;					 
			unitInfo unit = unitList[selectedIndex];
			if(unit.soul + soulCost>=unit.maxSoul)
			{
				soulCost= unit.maxSoul - unit.soul;
			}
			soul.text = (unit.maxSoul - unit.soul - soulCost).ToString();
			slider.value =  (1.0f*( unit.soul + soulCost)/ unit.maxSoul) ;
		}

		if(UpEffect.active== true)
		{
			if(Time.time -t >= 1.5f)
			{
				UpEffect.SetActive(false);
				setUintInfo(selectedIndex);
				//showItems();
			}
		}
		//setUintInfo(selectedIndex);
	}

	void onLeftClick(GameObject go)
	{
		if (!Finished) 
			return;

		Finished = false;
		for (int i = 0; i < itemList.Count; i ++)
		{
			setLeftPosition(i);
		}
		soulCost = 0;
	}
	
	void onRightClick(GameObject go)
	{
		if (!Finished) 
			return;

		Finished = false;
		for (int i = itemList.Count -1; i >= 0 ; i --)
		{
			setRightPosition(i);
		}
		soulCost = 0;
	}

	void setLeftPosition(int i)
	{
	
		tweenPos = itemList[i].GetComponent<TweenPosition>();
		playTween = itemList[i].GetComponent<UIPlayTween>();
		itemSprite = itemList[i].GetComponent<UISprite>();
		itemLevel = itemList[i].transform.Find("Level").GetComponent<UILabel>();
		tweenPos.duration = 0.5f;
		tweenPos.from = tweenPos.transform.localPosition;
		int currindex = posIndex[i];
		if (currindex == 0)
		{
			posIndex[i] = itemList.Count -1;
			tweenPos.to = posList[itemList.Count -1];
			itemSprite.depth = -100;
			itemLevel.depth = -100;
		}
		else
		{
			posIndex[i] = currindex - 1;
			tweenPos.to = posList[currindex - 1];
			itemSprite.depth = 13;
			itemLevel.depth = 14;
		}

		tweenScale = itemList[i].GetComponent<TweenScale>();
		tweenScale.duration = 0.5f;
		if (posIndex[i] == 1)
		{
			tweenScale.from = new Vector3(1,1,1);
			tweenScale.to = new Vector3(0.7f,0.7f,1);
			
		}
		else if (posIndex[i] == 2)
		{
			tweenScale.from = new Vector3(0.7f,0.7f,1);
			tweenScale.to = new Vector3(1,1,1);
			setUintInfo(i);
			selectedIndex = i;
		}
		else
		{
			tweenScale.from = new Vector3(0.7f,0.7f,1);
			tweenScale.to = new Vector3(0.7f,0.7f,1);
		}
		
		playTween.Play(true);
	}

	void setRightPosition(int i)
	{
		
		tweenPos = itemList[i].GetComponent<TweenPosition>();
		playTween = itemList[i].GetComponent<UIPlayTween>();
		itemSprite = itemList[i].GetComponent<UISprite>();
		itemLevel = itemList[i].transform.Find("Level").GetComponent<UILabel>();
		tweenPos.from = tweenPos.transform.localPosition;
		tweenPos.duration = 0.5f;
		int currindex = posIndex[i];
		if (currindex == itemList.Count -1)
		{
			posIndex[i] = 0;
			tweenPos.to = posList[0];
			itemSprite.depth = -100;
			itemLevel.depth = -100;
		}
		else
		{
			posIndex[i] = currindex + 1;
			tweenPos.to = posList[currindex + 1];
			itemSprite.depth = 13;
			itemLevel.depth = 14;
		}
		
		tweenScale = itemList[i].GetComponent<TweenScale>();
		tweenScale.duration = 0.5f;
		
		if (posIndex[i] == 3)
		{
			tweenScale.from = new Vector3(1,1,1);
			tweenScale.to = new Vector3(0.7f,0.7f,1);
			
		}
		else if (posIndex[i] == 2)
		{
			tweenScale.from = new Vector3(0.7f,0.7f,1);
			tweenScale.to = new Vector3(1,1,1);
			setUintInfo(i);
			selectedIndex = i;
		}
		else
		{
			tweenScale.from = new Vector3(0.7f,0.7f,1);
			tweenScale.to = new Vector3(0.7f,0.7f,1);
		}

		playTween.Play(true);
	}

	public void onFinished()
	{
		Finished = true;
	}

	void setUintInfo(int index)
	{
		if(unitList[index].unitID==0)
		{
			unOpen.SetActive(true);
			lvUp.text="暂未开放！";
			lv.text ="";
			soul.text="";
			slider.value=0;
			desc.text="";
		}
		else
		{
			unOpen.SetActive(false);
			desc.text = unitList[index].desc;
	//		if (unitList[index].soul < 0)
	//			soul.text = "0";
	//		else
			soul.text = (unitList[index].maxSoul- unitList[index].soul).ToString();
			lv.text = "Lv"+ (unitList [index].level).ToString ();
			slider.value =  (1.0f* unitList [index].soul / unitList [index].maxSoul) ;		
			if (slider.value >= 1)
			{

				//lvUp.text="升级成功！";
				slider.value = 0;
	//			showItems();
			}
			else
				lvUp.text="";
		}
	}

	void onPressClick(GameObject go,bool state)
	{
		pressState = state;
		unitInfo unit = unitList[selectedIndex];
		if (state) 
		{
			/*
			soulCost = 100;
			slider.value = slider.value + Time.deltaTime * 0.1f;
			unit.soul-=soulCost;
			soul.text=unit.soul;

			//unit.soul -= 50;
			//unitList[selectedIndex] = unit;
			setUintInfo(selectedIndex);
			if (slider.value >= 1)
				slider.value = 0;	
				*/
		}
		else
		{
			unit.soul += soulCost;
			unitList[selectedIndex] = unit;
			DataManager.getModule<DataBuilding>(DATA_MODULE.Data_Building).updateBuilding(unit.unitID,soulCost);
			setUintInfo(selectedIndex);
			soulCost=0;
		}
	}

	void onItemDrag(GameObject go ,Vector2 delta)
	{
		if (delta.x > 20)
			onRightClick(go);
		else if (delta.x < -20)
			onLeftClick(go);
		
		Debug.Log(delta);
	}

	void onItemPress(GameObject go,bool state)
	{
		Debug.Log(state);
	}
}
