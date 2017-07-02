using UnityEngine;
using System.Collections;

public class TeamListController : MonoBehaviour {

	private GameObject drugTeam;	
	private GameObject bgReturn;

	void Start () {
		UI.PanelTools.setBtnFunc(transform, "Bg/bgReturn/back", onReturnClick);
		drugTeam = UI.PanelStack.me.FindPanel ("Scale/NewVillage/PanelStuffMgr/PanelDrugTeam");
		bgReturn = UI.PanelStack.me.FindPanel ("Scale/NewVillage/PanelStuffMgr/PanelDrugTeam/PanelTeamList/Bg/bgReturn");
		iTween.MoveFrom(bgReturn,iTween.Hash("x",-3,"time",1));
	}
	
	void onReturnClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
		iTween.MoveTo(bgReturn,iTween.Hash("x",-3,"time",1));
		StartCoroutine("setDrugListActive");
	}
	
	IEnumerator setDrugListActive()
	{
		yield return new WaitForSeconds(0.5f);
		drugTeam.SetActive(true);
		iTween.MoveTo(bgReturn,iTween.Hash("x",-0.2,"time",1));
		transform.gameObject.SetActive(false);
	}
}
