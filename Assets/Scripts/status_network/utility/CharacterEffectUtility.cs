using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectUtility : MonoBehaviour {

	public GameObject slash;

	public void ShowSlash(){
		slash.SetActive (false);
		slash.SetActive (true);
	}


}
