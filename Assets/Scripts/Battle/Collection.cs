using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;

public class Collection : MonoBehaviour {

	public delegate void OnIdle();
	public static event OnIdle onIdle;

	public BaseAttribute playerAttr;
	public bool isCollectAble;
	public bool isCollecting;
	public bool isCollectDone;
	public Dictionary<Item,int> items;
	public static Material green;
	PlayMakerFSM pm;
	public FsmState[] fss;
	public FsmBool[] fssBool;

	void Awake()
	{
		if (green == null)
			green = Resources.Load<Material> ("Green");
		pm = GetComponent<PlayMakerFSM>();
		fss = pm.FsmStates;
		fssBool = pm.FsmVariables.BoolVariables;
	}

	void InitTempData()
	{
		items = new Dictionary<Item, int> ();
	}

	void Update()
	{
		if (Vector3.Distance (transform.position, playerAttr.transform.position) < 5) {
			pm.FsmVariables.FindFsmBool("isPlayerIn").Value = true;
		}
		else 
		{
			pm.FsmVariables.FindFsmBool("isPlayerIn").Value = false;
		}
	}

	float mCollectRadius = 0;
	public void Collect(float radius)
	{
		if (!pm.FsmVariables.FindFsmBool("isCollecting").Value) {
			pm.FsmVariables.FindFsmBool("isCollecting").Value = true;
			mCollectRadius = 0;
			GetComponent<Renderer>().material = green;
		} else {
			mCollectRadius += Time.deltaTime/3;
		}
		if(mCollectRadius>=1)
		{
			isCollectDone = true;
		}
	}

	private float mWidthRadiu;
	private float mHeightRadiu;
	static float barWidth = 100;
	static float barHeight = 10;

	Vector2 screenPos;
	void OnGUI()
	{
		screenPos = Camera.main.WorldToScreenPoint(transform.position);
		screenPos = new Vector2 (screenPos.x ,Screen.height - screenPos.y);
		if(pm.FsmVariables.FindFsmBool("isPlayerIn").Value)
		{
			GUI.Label(new Rect(screenPos.x- 30,screenPos.y - 50,60,30), "Collect!");
		}
		if(pm.FsmVariables.FindFsmBool("isCollecting").Value)
		{
			mWidthRadiu = (float)Screen.width / GUIBar.defaultWidth;
			mHeightRadiu = (float)Screen.height / GUIBar.defaultHeight;
			GUI.DrawTexture (new Rect( screenPos.x- 30,screenPos.y - 50,barWidth * mCollectRadius * mWidthRadiu, barHeight * mHeightRadiu),GUIBar.collectTex);
		}
	}

}

public class Item
{
	public delegate void OnUseItem();
	public OnUseItem onUseItem;
	public ItemType itemType;
}
