using UnityEngine;
using System.Collections;
using DataCenter;


public class DungeonMgr : MonoBehaviour {

	public GameObject btnReturn;
	public GameObject btnHome;
	public GameObject stagePrefab;
	public GameObject homePrefab;
	public GameObject bgReturn;
	public GameObject dungeonGrid;
	public GameObject bgGrid;
	public DungeonGridMgr dungeonGridMgr;
	public GameObject title;


	// Use this for initialization
	void Start () {
		UIEventListener.Get(btnReturn).onClick = onReturnClick;
		UIEventListener.Get(btnHome).onClick = onHomeClick;
		iTween.MoveFrom(bgReturn,iTween.Hash("x",-3,"time",1));
		NGUITools.AddWidgetCollider(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void onReturnClick(GameObject go)
	{
		iTween.MoveTo(bgReturn,iTween.Hash("x",-3,"time",1));
		iTween.MoveTo(bgGrid,iTween.Hash("x",3,"time",1));
		StartCoroutine("setDungeonActive");
	}

	IEnumerator setDungeonActive()
	{
		yield return new WaitForSeconds(0.5f);
		//transform.gameObject.SetActive(false);
		UI.PanelStack.me.goBack();
		//stagePrefab.SetActive(true);
		//iTween.MoveTo(bgReturn,iTween.Hash("x",-0.2,"time",1));
		//iTween.MoveTo(bgGrid,iTween.Hash("x",0,"time",1));

	}

	void OnEnable()
	{
		//iTween.MoveFrom(bgReturn,iTween.Hash("x",-3,"time",1));
		//iTween.MoveFrom(bgGrid,iTween.Hash("x",3,"time",1));		
	}

	void OnDisable()
	{
		iTween.MoveTo(bgReturn,iTween.Hash("x",-0.2,"time",1));
		iTween.MoveTo(bgGrid,iTween.Hash("x",0,"time",1));
	}


	void onHomeClick(GameObject go)
	{
		transform.gameObject.SetActive(false);
		homePrefab.SetActive(true);
	}

	public void init(int battleID)
	{
		if (!dungeonGridMgr)
			dungeonGridMgr = dungeonGrid.GetComponent<DungeonGridMgr>();

		dungeonGridMgr.GetComponent<DungeonGridMgr>().Init(battleID);

		if (title != null)
		{
			BattleInfo battleInfo = DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).getBattledatabyBattleID(battleID);
			title.GetComponent<UILabel>().text = battleInfo.NAME;			
		}
	
	}


}
