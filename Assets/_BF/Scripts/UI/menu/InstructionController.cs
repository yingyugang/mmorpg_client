using UnityEngine;
using System.Collections;

public class InstructionController : MonoBehaviour {

	private GameObject panelMenu;
	
	void Start () {
		UI.PanelTools.setBtnFunc(transform, "Bg/menuTitle/back", onBtnBackClick);
		panelMenu = UI.PanelStack.me.FindPanel ("PanelMenu/Bg");
	}
	
	void onBtnBackClick(GameObject go)
	{
		AudioManager.SingleTon().PlayMusic(AudioManager.SingleTon().MusicMainClip);
		this.transform.gameObject.SetActive (false);
		panelMenu.SetActive (true);
	}
}
