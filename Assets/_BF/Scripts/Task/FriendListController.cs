using UnityEngine;
using System.Collections;

public class FriendListController : MonoBehaviour {

	public GameObject btnReturn;
	
	public GameObject bgReturn;
	public GameObject FriendGrid;
	public GameObject panelDungeonList;

	// Use this for initialization
	void Start () {
		UIEventListener.Get(btnReturn).onClick = onReturnClick;
		iTween.MoveFrom(bgReturn,iTween.Hash("x",-3,"time",1));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void onReturnClick(GameObject go)
	{
		iTween.MoveTo(bgReturn,iTween.Hash("x",-3,"time",1));
		iTween.MoveTo(FriendGrid,iTween.Hash("x",3,"time",1));
		StartCoroutine("setDungeonListActive");
	}
	
	IEnumerator setDungeonListActive()
	{
		yield return new WaitForSeconds(0.5f);
		transform.gameObject.SetActive(false);
		panelDungeonList.SetActive(true);
		iTween.MoveTo(bgReturn,iTween.Hash("x",-0.2,"time",1));
		iTween.MoveTo(FriendGrid,iTween.Hash("x",-0.1,"time",1));
	}

}
