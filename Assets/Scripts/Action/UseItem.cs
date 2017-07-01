using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	public class UseItem : FsmStateAction {

		[RequiredField]
		public FsmFloat exitTime;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmBool recordParam;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmEvent trueEvent;

		Animator mAnimator;
		const string stateInfoName = "Base Layer.UseItem";

		string mCurrentStateName;
		string mCurrentTransitionName;

		public override void Awake()
		{
			if(Application.isPlaying)
			{
				mAnimator = Fsm.GameObject.GetComponent<Animator>();
				exitTime = Mathf.Clamp(exitTime.Value,0.0f,1.0f);
			}
		}

		public override void OnEnter()
		{
			recordParam.Value = true;
			mAnimator.SetBool ("isUseItem",true);
		}

		public override void OnUpdate()
		{
			OnUseItem ();
			if(mAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateInfoName) )
			{
				if(mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= exitTime.Value)
				{
					OnUseItemDone();
				}
			}
			if(!recordParam.Value)
			{
				Fsm.Event(trueEvent);
				Finish();
			}
		}

		public override void OnExit()
		{
			recordParam.Value = false;
			mAnimator.SetBool ("isUseItem",false);
		}

		void OnUseItem()
		{

		}

		void OnUseItemDone()
		{
			recordParam.Value = false;
			Debug.Log ("OnUseItemDone");
		}

	}
}