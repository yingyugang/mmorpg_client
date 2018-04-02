﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MMO
{
	[RequireComponent(typeof(MMOUnitSkill))]
	public class MMOUnit : MonoBehaviour
	{
		public UnitInfo unitInfo;
		public float animationSpeedOffset = 1;
		//用于判断是否是当前帧数据
		public int frame;
		Collider mCollider;
		Transform mTrans;
		public SimpleRpgAnimator mSimpleRpgAnimator;
		Animator mAnimator;

		CapsuleCollider mCapsuleCollider;
		CharacterController mCharacterController;

		void Awake(){
			mTrans = transform;
			mSimpleRpgAnimator = GetComponent<SimpleRpgAnimator> ();
			mAnimator =  GetComponentInChildren<Animator> (true);
			mCapsuleCollider = GetComponent<CapsuleCollider> ();
			mCharacterController = GetComponent<CharacterController> ();
			mCollider = GetComponent<Collider> ();
		}

		void Update(){
			#if NET_SERVER
			unitInfo.transform.playerPosition = mTrans.position;
			unitInfo.transform.playerForward = mTrans.forward;
			#endif
		}

		public UnityAction onDeath;
		public void Death(){
			if (onDeath != null) {
				onDeath ();
				onDeath = null;
			}
		}

		public void UnCollider(){
			mCollider.enabled = false;
		}

		public Vector3 GetHeadPos(){
			if (mCharacterController != null)
				return mTrans.position + new Vector3 (0, mCharacterController.height, 0);
			else if (mCapsuleCollider != null)
				return mTrans.position + new Vector3 (0, mCapsuleCollider.height, 0);
			else
				return mTrans.position;
		}

		public Vector3 GetBodyPos(){
			if (mCharacterController != null)
				return mTrans.position + new Vector3 (0, mCharacterController.height / 2, 0);
			else if (mCapsuleCollider != null)
				return mTrans.position + new Vector3 (0, mCapsuleCollider.height / 2, 0);
			else
				return mTrans.position;
		}

		public float GetBodyHeight(){
			if (mCharacterController != null)
				return mCharacterController.height;
			else if (mCapsuleCollider != null)
				return mCapsuleCollider.height;
			else
				return 1;
		}

		public bool IsInState(string state){
			return mSimpleRpgAnimator.IsInState (state);
		}

		public bool GetTrigger(string trigger){
			return mSimpleRpgAnimator.GetTrigger (trigger);
		}

		public void SetTrigger(string trigger){
			if(mSimpleRpgAnimator!=null)
				mSimpleRpgAnimator.SetTrigger (trigger);
			else if(mAnimator !=null){
				mAnimator.SetTrigger (trigger);
			}
		}

		public void ResetAllTrigger(){
			if(mAnimator !=null){
				for(int i=0;i<mAnimator.parameters.Length;i++){
					mAnimator.SetBool (mAnimator.parameters[0].name,false);
				}
			}
		}

		string mPreAction;
		public void SetAnimation(string action,float speed){
			if (mSimpleRpgAnimator != null) {
				mSimpleRpgAnimator.Play (action);
				mSimpleRpgAnimator.SetSpeed (speed * animationSpeedOffset);
			} else if (mAnimator !=null){
				mAnimator.Play (action, 0, 0);
				//TODO
//				if (mPreAction != action) {
//					if (mPreAction == "walk")
//						mAnimator.Play (action, 0, Random.Range (0, 1f));
//					else
//						mAnimator.Play (action, 0, 0);
//					mAnimator.speed = speed * animationSpeedOffset;
//					mPreAction = action;
//				}
			}
		}
	}
}
