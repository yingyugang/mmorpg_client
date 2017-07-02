using UnityEngine;
using System.Collections;

public class AnnounceCtroller : MonoBehaviour {

	// Use this for initialization
	void Start () {
		UI.PanelTools.setBtnFunc(transform, "Bg/AnnounceTitle/back", onBtnBackClick);
	}
	
	void onBtnBackClick(GameObject go)
	{
		AudioManager.SingleTon().PlayMusic(AudioManager.SingleTon().MusicMainClip);
		UI.PanelStack.me.goBack ();
		//this.transform.gameObject.SetActive(false);
	}

	void Update () {
	
	}
}
