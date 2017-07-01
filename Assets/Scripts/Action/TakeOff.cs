using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	public class TakeOff : FsmStateAction {

		[RequiredField]
		public FsmFloat exitTime;
		[RequiredField]
		public FsmString animatorParam;
		[RequiredField]
		public FsmString animatorStateInfoName;
		[RequiredField]
		public FsmFloat takeOffHeight = 5;
		[RequiredField]
		public FsmFloat takeOffTime = 0.5f;

		Animator mAnimator;
		EnemyController mEnemyController;
		float mCurrentTakeOff;

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
			mCurrentTakeOff = 0;
			mAnimator.SetBool (animatorParam.Value,true);
		}

		public override void OnUpdate()
		{
			if(mAnimator.GetCurrentAnimatorStateInfo(0).IsName(animatorStateInfoName.Value) )
			{
				mEnemyController.transform.position = new Vector3(mEnemyController.transform.position.x,Mathf.Lerp(mEnemyController.transform.position.y,takeOffHeight.Value,0.1f),mEnemyController.transform.position.z); 
				if(mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= exitTime.Value)
				{
					if(Fsm.GetFsmBool("isFly").Value)
					{
						Fsm.Event("OnFly");
					}
					else if(Fsm.GetFsmBool("isAttackFlip").Value)
					{
						Fsm.Event("OnAttackFlip");
					}
					Finish();
				}
			}
		}

		public override void OnExit()
		{
			mAnimator.SetBool (animatorParam.Value,false);
		}

	}
}
