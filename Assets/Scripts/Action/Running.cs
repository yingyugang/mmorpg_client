using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	public class Running : FsmStateAction {
		
		Animator mAnimator;
		EnemyController mEnemyController;
		Vector3 mTargetPos;

		public override void Awake()
		{
			if(Application.isPlaying)
			{
				mAnimator = Fsm.GameObject.GetComponent<Animator>();
				mEnemyController = Fsm.GameObject.GetComponent<EnemyController>();
			}
		}
		
		public override void OnEnter()
		{
			mTargetPos = mEnemyController.playerAttr.transform.position;
			mEnemyController.transform.LookAt (mTargetPos);
			mAnimator.SetBool ("isRun",true);
		}

		float mCurrentFrameDistance;
		public override void OnUpdate()
		{
			mEnemyController.transform.LookAt (mTargetPos);
			mCurrentFrameDistance = Vector3.Distance (mTargetPos,mEnemyController.transform.position);
			if(mCurrentFrameDistance < 1)
			{
				Fsm.GetFsmBool("isRun").Value = false;
				if(Fsm.GetFsmBool("isBattle").Value)
				{
					Fsm.Event("OnIdle");
				}
				else
				{
					Fsm.Event("OnIdleFree");
				}
				Finish();
			}
		}

		public override void OnExit()
		{
			mAnimator.SetBool ("isRun",false);
		}

	}
}

