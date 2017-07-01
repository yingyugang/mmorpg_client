using UnityEngine;
using System.Collections;
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	public class AttackAAA : FsmStateAction {
		
		[RequiredField]
		public FsmFloat nomalizeTime = 0.66f;
		
		Animator mAnimator;
		bool actived = false;
		public override void OnEnter()
		{
			actived = false;
			mAnimator = Fsm.GameObject.GetComponent<Animator>();
		}
		
		public override void OnUpdate()
		{
			if(!actived)
			{
				AnimatorStateInfo animatorStateInfo = mAnimator.GetCurrentAnimatorStateInfo (0);
				if(animatorStateInfo.normalizedTime >= nomalizeTime.Value)
				{
					mAnimator.SetInteger("ActionCMD",3);
				}
			}
		}
	}
}