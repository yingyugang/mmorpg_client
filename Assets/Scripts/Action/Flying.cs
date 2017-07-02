using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	public class Flying : FsmStateAction 
	{
		[RequiredField]
		public FsmString animatorParam;
		[RequiredField]
		public FsmString animatorStateInfoName;
		[RequiredField]
		public FsmFloat maxFlyHeight;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmBool recordParam;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmEvent flipEvent;

		public FsmFloat flyDur = 3;
		public FsmFloat landDur = 2;



		Animator mAnimator;
		EnemyController mEnemyController;
		float mCurrentFlyTime;
		float mCurrentFlyHeight;

		public override void Awake()
		{
			if(Application.isPlaying)
			{
				mAnimator = Fsm.GameObject.GetComponent<Animator>();
				mEnemyController = Fsm.GameObject.GetComponent<EnemyController>();
			}
		}
		
		public override void OnEnter()
		{
			mCurrentFlyTime = 0;
			mAnimator.SetBool (animatorParam.Value,true);			
		}
		
		public override void OnUpdate()
		{
			if (mAnimator.GetCurrentAnimatorStateInfo (0).IsName (animatorStateInfoName.Value)) {
				if(recordParam.Value)
				{
					Fsm.GameObject.transform.position = new Vector3 (Fsm.GameObject.transform.position.x, Mathf.Lerp (Fsm.GameObject.transform.position.y, maxFlyHeight.Value, 0.1f), Fsm.GameObject.transform.position.z);
					mCurrentFlyTime += Time.deltaTime;
//					if (mCurrentFlyTime >= flyDur.Value) 
//					{
//						Fsm.GetFsmBool("isAttackFlip").Value=true;
//						Fsm.Event(flipEvent);
//						Finish();
//					}
				}
//				else
//				{
//					Fsm.GameObject.transform.position = new Vector3 (Fsm.GameObject.transform.position.x, Mathf.Lerp (Fsm.GameObject.transform.position.y, 0, 0.1f), Fsm.GameObject.transform.position.z);
//					mCurrentFlyTime += Time.deltaTime;
//					if (mCurrentFlyTime >= landDur.Value) 
//					{
//						mAnimator.SetBool(animatorParam.Value,false);
//						Fsm.GameObject.transform.position = new Vector3 (Fsm.GameObject.transform.position.x, 0, Fsm.GameObject.transform.position.z);
//						Fsm.Event("OnIdle");
//						Finish();
//					}
//				}
			}

//			if(mAnimator.GetCurrentAnimatorStateInfo(0).IsName(animatorStateInfoName.Value) )
//			{
//				if(mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= exitTime.Value)
//				{
//					recordParam.Value = false;
//				}
//			}
//			if(!recordParam.Value)
//			{
//				if(Fsm.GetFsmBool("isBattle").Value)
//				{
//					Fsm.Event("OnIdle");
//				}
//				else
//				{
//					Fsm.Event("OnIdleFree");
//				}
//				Finish();
//			}
		}
		
		public override void OnExit()
		{
//			recordParam.Value = false;
//			mAnimator.SetBool (animatorParam.Value,false);
		}
	}
}


