using UnityEngine;
using System.Collections;
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	public class AttackAA : FsmStateAction {
		
		[RequiredField]
		public FsmFloat nomalizeTime = 0.6f;
		public FsmEvent trueEvent;
		Animator mAnimator;
		public override void OnEnter()
		{
			mAnimator = Fsm.GameObject.GetComponent<Animator>();
		}
		
		public override void OnUpdate()
		{
			AnimatorStateInfo animatorStateInfo = mAnimator.GetCurrentAnimatorStateInfo (0);
			if(animatorStateInfo.normalizedTime >= nomalizeTime.Value && Fsm.GetFsmBool("isAttackAAA").Value)
			{
				Fsm.GetFsmBool("isAttackAA").Value = false;
				mAnimator.SetInteger("ActionCMD",3);
				Fsm.Event(trueEvent);
			}
		}
		
	}
}