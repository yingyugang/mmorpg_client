using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class MMOUnit : MonoBehaviour
	{

		public MMOAttribute unitAttribute;
		public MMOTransform unitTransform;
		public MMOAnimation unitAnimation;

		void Update(){
			unitTransform.playerPosition = transform.position;
			unitTransform.playerForward = transform.forward;
		}

	}
}