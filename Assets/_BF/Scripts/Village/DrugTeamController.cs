using UnityEngine;
using System.Collections;

public class DrugTeamController : MonoBehaviour {

	public  GameObject Village,villageBg,stuffMgr,stuffMgrBg,drugTeam;

	public GameObject bgReturn;

	void Start () {
		init ();
		iTween.MoveFrom(bgReturn,iTween.Hash("x",-3,"time",1));
	}

	void onReturnClick(GameObject go)
	{
//		iTween.MoveTo(bgReturn,iTween.Hash("x",-3,"time",1));
//		iTween.MoveTo(teamGrid,iTween.Hash("x",3,"time",1));
//		iTween.MoveTo(btnCharge,iTween.Hash("x",-3,"time",1));
//		iTween.MoveTo(btnReset,iTween.Hash("x",3,"time",1));
		AudioManager.me.PlayBtnActionClip();
		if(UI.PanelStack.me.goBack()==null)
		{
			this.gameObject.SetActive (false);
			stuffMgrBg.SetActive (true);
		}
	}

	public void init ()
	{
		bgReturn = UI.PanelStack.me.FindPanel("Scale/NewVillage/PanelStuffMgr/PanelDrugTeam/Bg/dungteamtitle");
		stuffMgrBg = UI.PanelStack.me.FindPanel("Scale/NewVillage/PanelStuffMgr/Bg");
		UI.PanelTools.setBtnFunc(transform, "Bg/dungteamtitle/back", onReturnClick);
		
		Village = UI.PanelStack.me.FindPanel ("Scale/NewVillage");
		villageBg = UI.PanelStack.me.FindPanel ("Scale/NewVillage/villagebg");
		stuffMgr = UI.PanelStack.me.FindPanel ("Scale/NewVillage/PanelStuffMgr");
		drugTeam = UI.PanelStack.me.FindPanel ("Scale/NewVillage/PanelStuffMgr/PanelDrugTeam");
	}

	public void showDrugTeam()
	{
		init ();
		Village.SetActive (true);
		villageBg.SetActive (false);
		stuffMgr.SetActive (true);
		stuffMgrBg.SetActive (false);
		drugTeam.SetActive (true);
	}
}
