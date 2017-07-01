using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	public class Collect : FsmStateAction {
		
		[RequiredField]
		public FsmFloat exitTime;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmBool recordParam;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmEvent trueEvent;
		
		Animator mAnimator;
		PlayerController mPlayerController;
		const string stateInfoName = "Base Layer.Cutting";

		Collection mCurrentCollection;

		public override void Awake()
		{
			if(Application.isPlaying)
			{
				mAnimator = Fsm.GameObject.GetComponent<Animator>();
				mPlayerController = Fsm.GameObject.GetComponent<PlayerController>();
				exitTime = Mathf.Clamp(exitTime.Value,0.0f,1.0f);
			}
		}
		
		public override void OnEnter()
		{
			mCurrentCollection = mPlayerController.currentCollection;
			recordParam.Value = true;
			mAnimator.SetBool ("isCollect",true);
		}
		
		public override void OnUpdate()
		{
			OnCollect ();
			if(mAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateInfoName) )
			{
				if(mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= exitTime.Value)
				{

					OnCollectDone();
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
			mAnimator.SetBool ("isCollect",false);
		}

		void OnCollect()
		{
//			if (mCurrentCollection != null) 
//			{
//				mCurrentCollection.Collect();
//			}
//			if(mCurrentCollection.isCollectDone)
//			{
//				OnCollectDone();
//			}
		}

		void OnCollectDone()
		{
			recordParam.Value = false;
			BattleController.SingleTon().collections.Remove(mCurrentCollection);
			mCurrentCollection.gameObject.SetActive(false);
			Debug.Log ("OnCollectDone");
		}

	}
}