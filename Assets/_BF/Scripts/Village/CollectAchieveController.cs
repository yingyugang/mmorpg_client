using UnityEngine;
using System.Collections;

public class CollectAchieveController : MonoBehaviour {

	void Start () {
		UI.PanelTools.setBtnFunc(transform, "Bg/title/back", onBackClick);
	}

	void onBackClick(GameObject go)
	{
		AudioManager.me.PlayBtnActionClip();
		this.gameObject.SetActive(false);
	}
}
