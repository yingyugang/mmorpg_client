using UnityEngine;
using System.Collections;

public class HeroHandbookController : MonoBehaviour {
	
	private GameObject handbook;	
	private GameObject bgReturn;
	
	// Use this for initialization
	void Start () {
		UI.PanelTools.setBtnFunc(transform, "Above/bgReturn/back", onReturnClick);
		handbook = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse/PanelHandbook/Bg");
		bgReturn = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse/PanelHandbook/PanelHeroHandbook/Above/bgReturn");
		//iTween.MoveFrom(bgReturn,iTween.Hash("x",-3,"time",1));
	}
	
	void onReturnClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
		//iTween.MoveTo(bgReturn,iTween.Hash("x",-3,"time",1));
		StartCoroutine("setDrugListActive");
	}
	
	IEnumerator setDrugListActive()
	{
		yield return new WaitForSeconds(0.5f);
		handbook.SetActive(true);
		//iTween.MoveTo(bgReturn,iTween.Hash("x",-0.2,"time",1));
		transform.gameObject.SetActive(false);
	}
}
