using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

	public SimpleRpgPlayerController simpleRpgPlayerController;

	public SimpleRpgCamera simpleRpgCamera;

	void Awake(){
		#if UNITY_EDITOR
		simpleRpgPlayerController.clickToMove = false;
		simpleRpgPlayerController.keyboardControl = true;
		simpleRpgCamera.allowRotation = true;
		simpleRpgCamera.minAngle = 10;
		simpleRpgCamera.maxAngle = 90;
		#elif UNITY_IOS || UNITY_ANDROID
			simpleRpgPlayerController.clickToMove = true;
			simpleRpgPlayerController.keyboardControl = false;
			simpleRpgCamera.allowRotation = false;
		#endif
	}
}
