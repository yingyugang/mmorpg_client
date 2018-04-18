using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MMO
{
	[RequireComponent(typeof(MMOUnitSkill))]
	public class MMOUnit : MonoBehaviour
	{
		public UnitInfo unitInfo;
		public bool isDead;
		public float animationSpeedOffset = 1;
		//用于判断是否是当前帧数据
		public int frame;
		Collider mCollider;
		Transform mTrans;
		CapsuleCollider mCapsuleCollider;
		CharacterController mCharacterController;
		public UnitAnimator unitAnimator;

		void Awake(){
			mTrans = transform;
			unitAnimator = gameObject.GetOrAddComponent<UnitAnimator> ();
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
			}
			mCollider.enabled = false;
			HeadUIBase headUIBase = GetComponentInChildren<HeadUIBase> (true);
			if(headUIBase!=null)
				headUIBase.container_health_bar.SetActive (false);
		}

		public void UnCollider(){
			mCollider.enabled = false;
		}

		public void EnCollider(){
			mCollider.enabled = true;
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
	}
}
