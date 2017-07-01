using UnityEngine;
using System;
using System.Reflection;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Immediately switch to a state with the selected name.")]
	public class GotoStateByName : FsmStateAction
	{
		
		[RequiredField]
		public FsmString stateName;

		public override void Reset()
		{
			stateName = null;
		}
		
		public override void OnEnter()
		{
			FsmState targetState = null;

			foreach (FsmState state in Fsm.States)
			{
				if (state.Name == stateName.Value)
				{
					targetState = state;
					break;
				}
			}
			if (targetState != null)
			{
				MethodInfo switchState = Fsm.GetType().GetMethod("SwitchState", BindingFlags.NonPublic | BindingFlags.Instance);
				Log("Goto State: " + stateName.Value);
				switchState.Invoke(Fsm, new object[] { targetState });
			}
			else LogError("State Switch: State does not exist.");
			Finish();
		}
	}
}
