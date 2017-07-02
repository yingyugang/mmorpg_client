using UnityEngine;
using System.Collections;

public class AchievementController : MonoBehaviour {

	private GameObject collecthouse;
	private GameObject panelCollect;

	void Start () {
		UI.PanelTools.setBtnFunc(transform, "Bg/title/back", onBackClick);
		UI.PanelTools.setBtnFunc(transform, "Bg/Collect", onCollectClick);
		collecthouse = UI.PanelStack.me.FindPanel("Scale/NewVillage/CollectionHouse/Bg");
		panelCollect = UI.PanelStack.me.FindPanel("Scale/NewVillage/CollectionHouse/PaneAchievement/PanelCollect");
	}

	void onBackClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
		this.gameObject.SetActive(false);
		collecthouse.SetActive(true);
	}

	void onCollectClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
		this.panelCollect.SetActive (true);
	}
}
