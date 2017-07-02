using UnityEngine;
using System.Collections;
using DataCenter;
using BaseLib;

public class ReapMaterial : MonoBehaviour {
	
	private GameObject parent;	
	//	public GameObject Lake;
	public GameObject item;
	private GameObject collectItem;
	private buildingInfo buildInfo;
	private uint buidId ;
	private int maxLevy,curLevy;
	private ulong levyTime = 4 * 60 * 60; //收获间隔时间：4小时
	DataServTime dataTime;
	float old_y,new_y,d_y,old_x,new_x,d_x;
	private GameObject effect;
	
	void Start () {
		UIEventListener.Get(this.gameObject).onClick = onCollectClick;
		item.SetActive (false);
		parent = UI.PanelStack.me.FindPanel ("Scale/NewVillage/villagebg");
		EventSystem.register((int)EVENT_MAINUI.buildLevy, onUpdateData, (int)DataCenter.EVENT_GROUP.mainUI);
		dataTime = DataManager.getModule<DataServTime>(DATA_MODULE.Data_ServTime);
		old_y = 0;
		new_y = 0;
		old_x = 0;
		new_x = 0;
		d_x = 0;
		d_y = 0;
		effect = this.transform.FindChild ("TreasureParticle").gameObject;
		effect.SetActive (false);
	}
	
	void onUpdateData(int nEvent, System.Object param)
	{
		MSG_BUILDING_LEVY_RESPONSE msg = (MSG_BUILDING_LEVY_RESPONSE)param;
		
		Debug.Log (msg.isSucc ((int)msg.wErrCode));
	}
	
	bool Reap()
	{
		ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_BUILDING_LEVEL);
		if(table ==null)
			return false;
		ConfigRow row = table.getRow (DICT_BUILDING_LEVEL.BUILDING_TYPEID,(int)BUILD_TYPE.BUILD_FARMLAND);
		if(row ==null)
			return false;
		maxLevy =  row.getIntValue (DICT_BUILDING_LEVEL.EFFECT1);
		
		buildInfo= DataManager.getModule<DataBuilding>(DATA_MODULE.Data_Building).getBuilding(BUILD_TYPE.BUILD_FARMLAND);
		if(buildInfo == null)
			return false;
		buidId = buildInfo.id;
		
		if(dataTime.getCurSvrtime() - (ulong)buildInfo.lastLevy >= levyTime)
		{
			curLevy = 0;
		}
		else
		{
			curLevy =(int) buildInfo.lastLevyTimes;
		}
		if(curLevy >= maxLevy)
		{
			effect.SetActive(false);
			return false;
		}
		effect.SetActive (true);
		return true;
	}
	
	void Update()
	{
		if(Reap())
		{
			new_x = Input.acceleration.x;
			d_x = new_x - old_x;
			old_x = new_x;
			
			new_y=Input.acceleration.y;  
			d_y=new_y-old_y;  
			old_y=new_y;
			
			if(d_y < -2 || d_x < -2)
				ShakeCollect(true);
			if(d_y > 2 || d_x > 2)
				ShakeCollect(false);
		}
	}
	
	void ShakeCollect(bool ori)
	{
		Vector3 pos = new Vector3 (0.0f+Random.Range (-0.2f,0.2f) , 0.2f+Random.Range (-0.2f,0.2f) , 0.0f);
		GameObject prefab = item;
		Debug.Log(pos);
		collectItem = Instantiate(prefab) as GameObject;
		BezierController Bc = collectItem.GetComponent<BezierController>();
		if(Bc ==null)
			return;
		if(ori)
			Bc.orientation = -0.2f;
		else
			Bc.orientation = 0.2f;
		collectItem.transform.localPosition = pos;
		collectItem.SetActive(true);
		collectItem.transform.parent = parent.transform;
		collectItem.transform.localScale = Vector3.one;
		
		DataManager.getModule<DataBuilding> (DATA_MODULE.Data_Building).buildingLevy (buidId,1);
	}
	
	void onCollectClick(GameObject go)
	{
		if(!Reap())
			return;
		
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		//int index = Random.Range(0,3);
		GameObject prefab = item;
		Debug.Log(pos);
		collectItem = Instantiate(prefab) as GameObject;
		BezierController Bc = collectItem.GetComponent<BezierController>();
		if(Bc ==null)
			return;
		if(pos.x < 0)
			Bc.orientation = -0.2f;
		else
			Bc.orientation = 0.2f;
		collectItem.transform.localPosition = pos;
		collectItem.SetActive(true);
		collectItem.transform.parent = parent.transform;
		collectItem.transform.localScale = Vector3.one;
		
		DataManager.getModule<DataBuilding> (DATA_MODULE.Data_Building).buildingLevy (buidId,1);
	}
}
