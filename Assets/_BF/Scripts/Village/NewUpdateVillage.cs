using UnityEngine;
using System.Collections;
using DataCenter;
using BaseLib;

public class NewUpdateVillage : MonoBehaviour {

	protected GameObject BtnUpdate;
	protected GameObject UpEffect;
	protected UIProgressBar soulBar;
	protected bool pressState;
	protected UILabel title,desc,curSoul,maxSoul;
	protected uint BuidID,soulCost ,soul,max,level ,UserSoul;
	protected bool ini;
	protected float t,space;
	protected BUILD_TYPE buildType;
	protected int itemSort;
	protected string ex;
	protected bool isValid,isUpdating;
	protected UILabel soulLabel;
	public void Awake()
	{
		ini = false;
		init ();
	}
	public void Start () {
		if(ini)
			return;
		soulLabel =UI.PanelStack.me.FindPanel("Scale/NewVillage/NewlUpdateVillage/Bg/curSoul/Label").GetComponent<UILabel> () ;
		title = transform.Find ("Title/Lv").GetComponent<UILabel>();
		desc = transform.Find ("Des/con").GetComponent<UILabel> ();
		soulBar = transform.Find ("soulBar").GetComponent<UIProgressBar> ();
		curSoul = transform.Find ("soulBar/cursoul").GetComponent<UILabel> ();
		maxSoul = transform.Find ("soulBar/maxsoul").GetComponent<UILabel> ();
		EventSystem.register((int)EVENT_MAINUI.buildUpdate, onLevelUpdated, (int)DataCenter.EVENT_GROUP.mainUI);
		UpEffect = transform.FindChild ("UpEffect"+ex).gameObject;
		UpEffect.SetActive (false);
		BtnUpdate = GameObject.Find ("BtnUpdate"+ex);
		UIEventListener.Get(BtnUpdate).onPress = onPressClick;
		ini = true;
		isValid = true;
		isUpdating = false;
		t = Time.time;
		EventSystem.register((int)EVENT_MAINUI.userUpdate, onUserUpdate,(int)EVENT_GROUP.mainUI);
	}

	public virtual void init(){	}

	public void onUserUpdate(int nEvent,System.Object param)
	{
		showSoulInfo ();
	}

	public void showSoulInfo()
	{
		UserInfo info = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser;
		UserSoul = (uint)info.soul;
		soulLabel.text = "当前魂："+ UserSoul.ToString ();
		if(UserSoul == 0)
			isValid = false;
	}

	void OnEnable()
	{
		Start ();
		showSoulInfo ();
		soulCost = 0;	
		Validate ();
		showItems ();
		pressState = false;	
	}	

	public void onLevelUpdated(int nEvent, System.Object param)
	{
		BUILD_TYPE build = (BUILD_TYPE)param;
		if (build == this.buildType)
		{	
			buildingInfo buildInfo= DataManager.getModule<DataBuilding>(DATA_MODULE.Data_Building).getBuilding(build);
			if(level <buildInfo.level)
			{
				UpEffect.SetActive(true);
				t = Time.time;
			}
			Validate ();
			showItems();
			soulCost=0;
		}
	}

	public void Update () {
		if (pressState)
		{
			if(Time.time - space > 0.3f)
			{
				if(!isUpdating && isValid)
				{
					soulCost += max/50;					 
					if(soul + soulCost >= max)
					{
						isUpdating = true;
						soulCost= max - soul;
						DataManager.getModule<DataBuilding>(DATA_MODULE.Data_Building).updateBuilding(BuidID,soulCost);
						t= Time.time;
					}
					else
					{
						UserSoul -= soulCost;
						soulLabel.text = "当前魂："+ UserSoul.ToString();
						curSoul.text = (soul + soulCost).ToString();
						soulBar.value =  (1.0f*( soul + soulCost)/ max) ;
					}
				}	
			}
		}

		if(UpEffect.active == true)
		{
			if(Time.time -t >= 1.2f)
			{
				UpEffect.SetActive(false);
				isUpdating = false;
//				soulBar.value =  0 ;
//				curSoul.text = "0";
			}
		}
	}

	public void Validate()
	{
		buildingInfo buildInfo = DataManager.getModule<DataBuilding>(DATA_MODULE.Data_Building).getBuilding(buildType);
		if(buildInfo == null)
			return;
		BuidID = buildInfo.id;
		soul = buildInfo.curSoul;
		max = buildInfo.maxSoul;
		level = buildInfo.level;
		ConfigTable tableBuild = ConfigMgr.getConfig(CONFIG_MODULE.DICT_BUILDING);
		if(tableBuild == null)
			return;
		ConfigRow buildRow = tableBuild.getRow(DICT_BUILDING.BUILDING_TYPEID,(int)buildInfo.type);
		if(buildRow == null)
			return;
		int maxLevel = buildRow.getIntValue (DICT_BUILDING.MAX_LEVEL);
		Debug.Log (maxLevel);
		if(level == maxLevel)
			isValid = false;
	}

	public void showItems()
	{
		if(isValid)
		{
			string tempDesc ="";

			ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ITEM);
			ConfigTable tableFormula = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ITEM_FORMULA);
			ConfigRow[] rows = table.getRows (DICT_ITEM.SORT, itemSort);
			if (table ==null || tableFormula==null)
				return;
			foreach(ConfigRow row in rows)
			{
				int typeid = row.getIntValue(DICT_ITEM.ITEM_TYPEID);
				
				ConfigRow formula = tableFormula.getRow(DICT_ITEM_FORMULA.ITEM_TYPEID,typeid);
				if(formula==null)
					continue;
				int buildLv = formula.getIntValue(DICT_ITEM_FORMULA.BUILDING_LEVEL);
				if(buildLv == level+1)
				{					
					ItemInfo iInfo= DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).getItem((uint)typeid);
					if(iInfo==null)
					{
						iInfo = new ItemInfo();
						iInfo.init(typeid);
					}
					tempDesc += iInfo.name +"\n";
				}
			}
			title.text = "LV" + level.ToString ();
			curSoul.text = soul.ToString ();
			maxSoul.text = "/"+max.ToString ();
			soulBar.value =  (1.0f * soul / max) ;
			desc.text = tempDesc;
		}
		else
		{
			title.text = "LvMAX";
			curSoul.text = "";
			maxSoul.text = "";
			soulBar.value = 1;
			desc.text = "";
		}
	}
	
	public void onPressClick(GameObject go,bool state)
	{
		if(isValid)
		{
			pressState = state;
			if (!pressState && !isUpdating) 		
			{
				DataManager.getModule<DataBuilding>(DATA_MODULE.Data_Building).updateBuilding(BuidID,soulCost);
				space = Time.time;
			}
		}
	}
}
