using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class PlatformController : SingleMonoBehaviour<PlatformController>
	{

		public GameObject etcJoystick;

		protected override void Awake ()
		{
			base.Awake ();
			#if UNITY_EDITOR
			etcJoystick.SetActive (false);
			#elif UNITY_IOS || UNITY_ANDROID
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
