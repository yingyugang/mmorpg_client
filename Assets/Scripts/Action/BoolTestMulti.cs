

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if all the given Bool Variables are are equal to thier Bool States.")]
	public class BoolTestMulti : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("This must be the same number used for Bool States.")]
		public FsmBool[] boolVariables;
		
		[RequiredField]
		[Tooltip("This must be the same number used for Bool Variables.")]
		public FsmBool[] boolStates;
		
		public FsmEvent trueEvent;
		public FsmEvent falseEvent;
		
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;
		
		public bool everyFrame;
		
		public override void Reset()
		{
			boolVariables = null;
			boolStates = null;
			trueEvent = null;
			falseEvent = null;
			storeResult = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			DoAllTrue();
			
			if (!everyFrame)
				Finish();		
		}
		
		public override void OnUpdate()
		{
			DoAllTrue();
		}
		
		void DoAllTrue()
		{
			if (boolVariables.Length == 0 || boolStates.Length == 0) return;
			if (boolVariables.Length != boolStates.Length) return;
			
			bool allTrue = true;
			
			for (int i = 0; i < boolVariables.Length; i++) 
			{
				if (boolVariables[i].Value != boolStates[i].Value)
				{
					allTrue = false;
					break;
				}
			}
			
			storeResult.Value = allTrue;
			
			if (allTrue)
				Fsm.Event(trueEvent);
			else
				Fsm.Event (falseEvent);
		}
	}
}