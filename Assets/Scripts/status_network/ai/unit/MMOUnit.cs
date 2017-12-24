using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class MMOUnit : MonoBehaviour
	{
		public UnitInfo unitInfo;
		Transform mTrans;
		SimpleRpgAnimator mSimpleRpgAnimator;
		Animator mAnimator;

		void Awake(){
			mTrans = transform;
			mSimpleRpgAnimator = GetComponent<SimpleRpgAnimator> ();
			mAnimator =  GetComponentInChildren<Animator> (true);
		}

		void Update(){
			#if NET_SERVER
			unitInfo.transform.playerPosition = mTrans.position;
			unitInfo.transform.playerForward = mTrans.forward;
			#endif
		}

		string mPreAction;
		public void SetAnimation(string action,float speed){
			if (mSimpleRpgAnimator != null) {
				mSimpleRpgAnimator.Action = action;
				mSimpleRpgAnimator.SetSpeed (speed);
			} else if (mAnimator !=null){
				//TODO
				if (mPreAction != action) {
					mAnimator.Play (action, 0, 0);
					mAnimator.speed = speed;
					mPreAction = action;
				}
			}
		}
	}
}
