using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	public class AnimatorExit : FsmStateAction 
	{
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
				exitTime = Mathf.Clamp(exitTime.Value,0.0f,1.0f);
			}
		}
		
		public override void OnEnter()
		{
			mAnimator.SetBool (animatorParam.Value,true);			
		}
		
		public override void OnUpdate()
		{
			if(mAnimator.GetCurrentAnimatorStateInfo(0).IsName(animatorStateInfoName.Value) )
			{
				if(mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= exitTime.Value)
				{
					recordParam.Value = false;
				}
			}
			if(!recordParam.Value)
			{
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
			recordParam.Value = false;
			mAnimator.SetBool (animatorParam.Value,false);
		}
	}
}


