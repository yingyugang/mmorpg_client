using UnityEngine;
using System.Collections;

public class storyListController : MonoBehaviour {

	private GameObject storyLine;
//	private GameObject storyDetail;
	private GameObject panelReplay;

	void Start () {
		UI.PanelTools.setBtnFunc(transform, "Bg/title/back", onBackClick);
		//UI.PanelTools.setBtnFunc(transform, "Bg/book/tempCollider", onDetailClick);
		storyLine = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse/PanelStoryline/Bg");
	//	storyDetail = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse/PanelStoryline/PanelStoryList/Bg/book/storyDetail");
		panelReplay = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse/PanelStoryline/PanelStoryList/PanelRepaly");
	}
	
	void onBackClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
		this.gameObject.SetActive(false);
		storyLine.SetActive(true);
	}

	void OnEnable()
	{
	//	if(storyDetail != null)
	//		storyDetail.SetActive(false);
		if(panelReplay != null)
			panelReplay.SetActive(false);
	}
}
