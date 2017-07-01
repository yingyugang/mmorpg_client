using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	public class SetAnimatorSpeedOnExit : FsmStateAction
	{
		[RequiredField]
		public FsmFloat speed = 1;
		private Animator animator;
		
		public override void OnEnter()
		{
			if (animator == null) 
			{
				animator = Fsm.GameObject.GetComponent<Animator>();
			}
		}

		public override void OnExit()
		{
			if(animator!=null)
				animator.speed = speed.Value;
		}
	}
}

