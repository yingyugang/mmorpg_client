using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	public class SetAnimatorBoolOnExit : FsmStateAction
	{
		[RequiredField]
		public FsmString variable;
		[RequiredField]
		public FsmBool stateValue = false;

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
			if (animator != null) {
				animator.SetBool (variable.Value,stateValue.Value);
			}
		}
	}
}