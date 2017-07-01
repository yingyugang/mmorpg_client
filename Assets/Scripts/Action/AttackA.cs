using UnityEngine;
using System.Collections;
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	public class AttackA : FsmStateAction {

		[RequiredField]
		public FsmFloat nomalizeTime = 0.66f;
		public FsmEvent trueEvent;

		Animator mAnimator;
		public override void OnEnter()
		{
			mAnimator = Fsm.GameObject.GetComponent<Animator>();
		}

		public override void OnUpdate()
		{
			AnimatorStateInfo animatorStateInfo = mAnimator.GetCurrentAnimatorStateInfo (0);
			if(animatorStateInfo.normalizedTime >= nomalizeTime.Value && Fsm.GetFsmBool("isAttackAA").Value)
			{
				Fsm.GetFsmBool("isAttackA").Value = false;
				mAnimator.SetInteger("ActionCMD",2);
				Fsm.Event(trueEvent);
			}
		}

	}
}