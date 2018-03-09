using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformUtility : MonoBehaviour {

	void Awake(){
		#if UNITY_IOS || UNITY_ANDROID
		gameObject.SetActive (false);
		#endif
	}

}
