using UnityEngine;
using System.Collections;

public class VillageInterface : MonoBehaviour {

	public GameObject Replay,storyList,storyLine,lineBg,collectionHouse,collectBg,village,villageBg;

	void Start () {
		village = UI.PanelStack.me.FindPanel ("Scale/NewVillage");
		villageBg = UI.PanelStack.me.FindPanel ("Scale/NewVillage/villagebg");
		collectionHouse = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse");
		collectBg = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse/Bg");
		storyLine = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse/PanelStoryline");
		lineBg = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse/PanelStoryline/Bg");
		storyList = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse/PanelStoryline/PanelStoryList");
		Replay = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse/PanelStoryline/PanelStoryList/PanelReplay");
	}

	//调用剧情对话页面
	public void showReplay()
	{
		village.SetActive (true);
		villageBg.SetActive (false);
		collectionHouse.SetActive (true);
		collectBg.SetActive (false);
		storyLine.SetActive (true);
		lineBg.SetActive (false);
		storyList.SetActive (true);
		Replay.SetActive (true);
	}
}
