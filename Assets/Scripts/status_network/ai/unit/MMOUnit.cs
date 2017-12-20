using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class MMOUnit : MonoBehaviour
	{
		public UnitInfo unitInfo;
		Transform mTrans;

		void Awake(){
			mTrans = transform;
		}

		void Update(){
			#if NET_SERVER
			unitInfo.transform.playerPosition = mTrans.position;
			unitInfo.transform.playerForward = mTrans.forward;
			#endif
		}

	}
}
