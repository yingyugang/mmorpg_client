using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgehogTeam.EasyTouch;

namespace MMO
{

	public class PlayerInputController : SingleMonoBehaviour<PlayerInputController>
	{

		protected override void Awake ()
		{
			base.Awake ();
		}

		void Update(){
			Debug.Log (EasyTouch.current.deltaPosition);
			Debug.Log (ETCInput.GetAxis ("Vertical"));// .current.deltaPosition);
		}
	}

}
