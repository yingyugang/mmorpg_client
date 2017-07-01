using UnityEngine;
using System.Collections;
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	public class AttackFlip : FsmStateAction {
		
		[RequiredField]
		public FsmFloat exitTime;
		[RequiredField]
		public FsmString animatorParam;
		[RequiredField]
		public FsmString animatorStateInfoName;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmBool recordParam;

		Animator mAnimator;
		EnemyController mEnemyController;

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
			recordParam.Value = true;
			mAnimator.SetBool (animatorParam.Value,true);
		}
		
		public override void OnUpdate()
		{
			if(mAnimator.GetCurrentAnimatorStateInfo(0).IsName(animatorStateInfoName.Value) )
			{
//				Debug.Log(mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime + "--" +exitTime.Value);
				if(mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= exitTime.Value)
				{
					recordParam.Value = false;
				}
			}
			if(!recordParam.Value)
			{
				Fsm.Event("OnFly");
				Finish();
			}
		}

		public override void OnExit()
		{
			recordParam.Value = false;
			Fsm.GetFsmBool("isFly").Value=false;
			mAnimator.SetBool (animatorParam.Value,false);
		}
		
	}
}
