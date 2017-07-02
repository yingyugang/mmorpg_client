using UnityEngine;
using System.Collections;

public class storyController : MonoBehaviour {
	private GameObject collecthouse;
	private GameObject panelList;
	private GameObject bg;
	
	void Start () {
		UI.PanelTools.setBtnFunc(transform, "Bg/title/back", onBackClick);
		UI.PanelTools.setBtnFunc(transform, "Bg/book/main", onMainClick);
		UI.PanelTools.setBtnFunc(transform, "Bg/book/branch", onBranchClick);

		bg =  UI.PanelStack.me.FindPanel("Scale/NewVillage/CollectionHouse/PanelStoryline/Bg");
		collecthouse = UI.PanelStack.me.FindPanel("Scale/NewVillage/CollectionHouse/Bg");
		panelList = UI.PanelStack.me.FindPanel("Scale/NewVillage/CollectionHouse/PanelStoryline/PanelStoryList");
	}

	void OnEnable()
	{
		if(bg != null)
			bg.SetActive (true);
		if(panelList != null)
			panelList.SetActive(false);
	}

	void onBackClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
		this.gameObject.SetActive(false);
		collecthouse.SetActive(true);
	}
	
	void onMainClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
		bg.SetActive (false);
		this.panelList.SetActive (true);
	}

	void onBranchClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
		bg.SetActive (false);
		this.panelList.SetActive (true);
	}
}
