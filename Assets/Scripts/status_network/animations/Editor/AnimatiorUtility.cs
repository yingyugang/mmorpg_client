using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace MMO
{
	//to init the trigger and transformation for new animatorcontroller.
	//TODO need to add update animatorcontroller function.
	public class AnimatiorUtility : MonoBehaviour
	{

		const string TRIGGER_ATTACK1 = "attack1";
		const string TRIGGER_ATTACK2 = "attack2";
		const string TRIGGER_ATTACK3 = "attack3";
		const string TRIGGER_MAGIC = "magic";
		const string TRIGGER_RUN = "run";
		const string TRIGGER_WALK = "walk";

		const string STATE_IDEL = "idle";
		const string STATE_ATTACK1 = "attack1";
		const string STATE_ATTACK2 = "attack2";
		const string STATE_ATTACK3 = "attack3";
		const string STATE_MAGIC = "magic";
		const string STATE_RUN = "run";
		const string STATE_WALK = "walk";
		const string STATE_DEAD = "dead";
		const string STATE_HIT = "hit";
		const string STATE_JUMP = "jump";

		static Dictionary<string,AnimatorState> mStatesDic;


		[MenuItem ("Tools/InitAnimatorStates")]
		public static void InitStates ()
		{
			Object obj = Selection.activeObject;
			for (int i = 0; i < Selection.objects.Length; i++) {
				AnimatorController ac = obj as AnimatorController;
				if (ac != null)
					InitAnimatorStates (ac);
			}
		}

		static void InitAnimatorStates(AnimatorController animator){
			animator.layers [0].stateMachine.AddState (STATE_IDEL);
			animator.layers [0].stateMachine.AddState (STATE_ATTACK1);
			animator.layers [0].stateMachine.AddState (STATE_ATTACK2);
			animator.layers [0].stateMachine.AddState (STATE_ATTACK3);
			animator.layers [0].stateMachine.AddState (STATE_MAGIC);
			animator.layers [0].stateMachine.AddState (STATE_RUN);
			animator.layers [0].stateMachine.AddState (STATE_WALK);
			animator.layers [0].stateMachine.AddState (STATE_DEAD);
			animator.layers [0].stateMachine.AddState (STATE_HIT);
			animator.layers [0].stateMachine.AddState (STATE_JUMP);
		}


		[MenuItem ("Tools/InitAnimatorTransformation")]
		public static void Init ()
		{
			Object obj = Selection.activeObject;
			for (int i = 0; i < Selection.objects.Length; i++) {
				AnimatorController ac = obj as AnimatorController;
				if (ac != null)
					InitAnimatorController (ac);
			}
		}

		static void InitAnimatorStateTransition (AnimatorState stateFrom, AnimatorState stateTo,bool hasExitTime = true, string conditionName = null)
		{
			AnimatorStateTransition trans = stateFrom.AddTransition (stateTo);
			if (!string.IsNullOrEmpty (conditionName))
				trans.AddCondition (AnimatorConditionMode.If, 1, conditionName);
			trans.exitTime = 0.9f;
			trans.duration = 0.1f;
			trans.hasFixedDuration = false;
			trans.offset = 0;
			trans.hasExitTime = hasExitTime;
		}

		static void InitAnimatorController (AnimatorController animatorController)
		{
			ClearTransitions (animatorController);
			ClearParameters (animatorController);
			InitParameters (animatorController);
			InitStateTransitions (animatorController);
		}

		static void ClearTransitions (AnimatorController animatorController)
		{
			for (int i = 0; i < animatorController.layers [0].stateMachine.states.Length; i++) {
				AnimatorState state = animatorController.layers [0].stateMachine.states [i].state;
				while (state.transitions.Length > 0) {
					state.RemoveTransition (state.transitions [0]);
				}
			}
		}

		static void ClearParameters (AnimatorController animatorController)
		{
			while (animatorController.parameters.Length > 0) {
				animatorController.RemoveParameter (animatorController.parameters [0]);
			}
		}

		static void InitParameters (AnimatorController animatorController)
		{
			animatorController.AddParameter (TRIGGER_ATTACK1, AnimatorControllerParameterType.Trigger);
			animatorController.AddParameter (TRIGGER_ATTACK2, AnimatorControllerParameterType.Trigger);
			animatorController.AddParameter (TRIGGER_ATTACK3, AnimatorControllerParameterType.Trigger);
			animatorController.AddParameter (TRIGGER_MAGIC, AnimatorControllerParameterType.Trigger);
			animatorController.AddParameter (TRIGGER_RUN, AnimatorControllerParameterType.Trigger);
			animatorController.AddParameter (TRIGGER_WALK, AnimatorControllerParameterType.Trigger);
		}

		static void InitStateTransitions (AnimatorController animatorController)
		{
			mStatesDic = new Dictionary<string, AnimatorState> ();
			var rootStateMachine = animatorController.layers [0].stateMachine;
			foreach (ChildAnimatorState state in rootStateMachine.states) {
				mStatesDic.Add (state.state.name, state.state);
			}
			rootStateMachine.defaultState = mStatesDic[STATE_IDEL];
			InitAnimatorStateTransition (mStatesDic[STATE_IDEL], mStatesDic[STATE_ATTACK1],false, TRIGGER_ATTACK1);
			InitAnimatorStateTransition (mStatesDic[STATE_IDEL], mStatesDic[STATE_ATTACK2],false, TRIGGER_ATTACK2);
			InitAnimatorStateTransition (mStatesDic[STATE_IDEL], mStatesDic[STATE_ATTACK3],false, TRIGGER_ATTACK3);
			InitAnimatorStateTransition (mStatesDic[STATE_IDEL], mStatesDic[STATE_MAGIC],false, TRIGGER_MAGIC);

			InitAnimatorStateTransition (mStatesDic[STATE_ATTACK1], mStatesDic[STATE_IDEL],true);
			InitAnimatorStateTransition (mStatesDic[STATE_ATTACK2], mStatesDic[STATE_IDEL],true);
			InitAnimatorStateTransition (mStatesDic[STATE_ATTACK3], mStatesDic[STATE_IDEL],true);
			InitAnimatorStateTransition (mStatesDic[STATE_MAGIC], mStatesDic[STATE_IDEL],true);

			InitAnimatorStateTransition (mStatesDic[STATE_RUN], mStatesDic[STATE_ATTACK1],false, TRIGGER_ATTACK1);
			InitAnimatorStateTransition (mStatesDic[STATE_RUN], mStatesDic[STATE_ATTACK2],false, TRIGGER_ATTACK2);
			InitAnimatorStateTransition (mStatesDic[STATE_RUN], mStatesDic[STATE_ATTACK3],false, TRIGGER_ATTACK3);
			InitAnimatorStateTransition (mStatesDic[STATE_RUN], mStatesDic[STATE_MAGIC],false, TRIGGER_MAGIC);

			InitAnimatorStateTransition (mStatesDic[STATE_ATTACK1], mStatesDic[STATE_RUN],true);
			InitAnimatorStateTransition (mStatesDic[STATE_ATTACK2], mStatesDic[STATE_RUN],true);
			InitAnimatorStateTransition (mStatesDic[STATE_ATTACK3], mStatesDic[STATE_RUN],true);
			InitAnimatorStateTransition (mStatesDic[STATE_MAGIC], mStatesDic[STATE_RUN],true);

			InitAnimatorStateTransition (mStatesDic[STATE_WALK], mStatesDic[STATE_ATTACK1],false, TRIGGER_ATTACK1);
			InitAnimatorStateTransition (mStatesDic[STATE_WALK], mStatesDic[STATE_ATTACK2],false, TRIGGER_ATTACK2);
			InitAnimatorStateTransition (mStatesDic[STATE_WALK], mStatesDic[STATE_ATTACK3],false, TRIGGER_ATTACK3);
			InitAnimatorStateTransition (mStatesDic[STATE_WALK], mStatesDic[STATE_MAGIC],false, TRIGGER_MAGIC);

			InitAnimatorStateTransition (mStatesDic[STATE_IDEL], mStatesDic[STATE_RUN],false, TRIGGER_RUN);
			InitAnimatorStateTransition (mStatesDic[STATE_IDEL], mStatesDic[STATE_WALK],false, TRIGGER_WALK);
//			InitAnimatorStateTransition (mIdolB, mIdolA, TRIGGER_TO_A);
//		    InitAnimatorStateTransition (stateDic ["joy"], stateDic ["a"]);
		}

	}
}