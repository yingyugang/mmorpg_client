using UnityEngine;
using System.Collections;

public class storyDetailController : MonoBehaviour {

	private GameObject panelReplay;
	public StoryItem Item;

	void Start () {
		UI.PanelTools.setBtnFunc(transform, "replay", onRepalyClick);
		UI.PanelTools.setBtnFunc(transform, "btnClose", onCloseClick);
		panelReplay = UI.PanelStack.me.FindPanel ("Scale/NewVillage/CollectionHouse/PanelStoryline/PanelStoryList/PanelRepaly");
	}

	void onRepalyClick(GameObject go)
	{
		ReplayController replay = panelReplay.GetComponent<ReplayController>();
		replay.storyid = Item.typeid;
		panelReplay.SetActive (true);
	}

	void onCloseClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
		this.transform.gameObject.SetActive (false);
	}

	void OnEnable()
	{
		Show ();
	}

	void Show()
	{
		this.transform.Find ("title").GetComponent<UILabel> ().text = Item.Sname;
		this.transform.Find ("pic").GetComponent<UISprite> ().spriteName = Item.sprite;
		if(Item.unLock)
		{
			this.transform.Find ("lock").GetComponent<UISprite> ().alpha = 0;
			this.transform.Find ("desc").GetComponent<UILabel> ().text = Item.desc;
		}
		else
		{
			this.transform.Find ("lock").GetComponent<UISprite> ().alpha = 1;
			this.transform.Find ("desc").GetComponent<UILabel> ().text = "通关"+ Item.battle +"解锁";
		}

	}
}
