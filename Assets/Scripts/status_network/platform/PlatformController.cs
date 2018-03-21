using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class PlatformController : SingleMonoBehaviour<PlatformController>
	{

		public SimpleRpgPlayerController simpleRpgPlayerController;

		public SimpleRpgCamera simpleRpgCamera;

		public GameObject etcJoystick;

		void Awake ()
		{
			#if UNITY_EDITOR
			simpleRpgPlayerController.clickToMove = false;
			simpleRpgPlayerController.keyboardControl = true;
			simpleRpgCamera.allowRotation = true;
			simpleRpgCamera.minAngle = -90;
			simpleRpgCamera.maxAngle = 90;
			etcJoystick.SetActive (false);
			#elif UNITY_IOS || UNITY_ANDROID
			simpleRpgPlayerController.clickToMove = false;
			simpleRpgPlayerController.keyboardControl = true;
			simpleRpgCamera.allowRotation = true;
			#endif
		}

		public void ShowJoystick(){
			#if UNITY_EDITOR

			#elif UNITY_IOS || UNITY_ANDROID
			etcJoystick.SetActive(true) ;
			#endif
		}

	}
}
