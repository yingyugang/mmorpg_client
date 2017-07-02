using UnityEngine;
using System.Collections;

public class StuffSaleController : MonoBehaviour {

	public GameObject btnReturn;
	public GameObject stufSalefList;	
	public GameObject bgReturn;
	
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
		StartCoroutine("setDrugListActive");
	}
	
	IEnumerator setDrugListActive()
	{
		yield return new WaitForSeconds(0.5f);
		stufSalefList.SetActive(true);
		iTween.MoveTo(bgReturn,iTween.Hash("x",-0.2,"time",1));
		transform.gameObject.SetActive(false);
	}
}
