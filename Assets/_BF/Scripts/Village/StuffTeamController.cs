using UnityEngine;
using System.Collections;

public class StuffTeamController : MonoBehaviour {

	public GameObject btnBack;
	public GameObject BtnStuffFill;
	public GameObject PanelStuffMgr;
	// Use this for initialization
	void Start () {
		UIEventListener.Get(BtnStuffFill).onClick = onStuffFillClick;
		UIEventListener.Get(btnBack).onClick = onBackClick;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void onStuffFillClick(GameObject go)
	{
	}

	void onBackClick(GameObject go)
	{
		this.transform.gameObject.SetActive(false);
		//PanelStuffMgr.SetActive(true);
	}
}
