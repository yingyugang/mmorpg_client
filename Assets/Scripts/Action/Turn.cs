using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	public class Turn : FsmStateAction {

		[RequiredField]
		public FsmFloat turnAngle = 120;
		[RequiredField]
		public FsmFloat turnSweepAngle = 240;
		[RequiredField]
		public FsmFloat turnSweepCooldown = 3;

		EnemyController mEnemyController;
		PlayerController mPlayerController;
		float mDirection;
		float mCurrentCooldown;

		public override void Awake()
		{
			if (Application.isPlaying) 
			{
				if(mEnemyController==null)
					mEnemyController = Fsm.GameObject.GetComponent<EnemyController>();
				if (mPlayerController == null)
					mPlayerController = BattleController.SingleTon ().playerController;
			}
		}

		public override void OnEnter(){
			mCurrentCooldown = 0;	
		}

		public override void OnUpdate()
		{
			if (!mPlayerController.pm.Fsm.GetFsmBool ("isDead").Value) 
			{
				mDirection = CommonUtility.GetDirection (mEnemyController.transform.forward,mPlayerController.transform.position - mEnemyController.transform.position);
				if(mDirection <= Mathf.Cos(turnSweepAngle.Value/2/180 * Mathf.PI))
				{
					if(CommonUtility.GetAngleDirection(mEnemyController.transform,mPlayerController.transform))
					{
						Fsm.GetFsmBool("isTurnLeftSweep").Value = true;
						Fsm.Event("OnTurnLeftSweep");
					}
					else
					{
						Fsm.GetFsmBool("isTurnRightSweep").Value = true;
						Fsm.Event("OnTurnRightSweep");
					}
					Finish();
				}
				else if(mDirection < Mathf.Cos(turnAngle.Value/2/2/180 * Mathf.PI))
				{
					if(CommonUtility.GetAngleDirection(mEnemyController.transform,mPlayerController.transform))
					{
						Fsm.GetFsmBool("isTurnLeft").Value = true;
						Fsm.Event("OnTurnLeft");
					}
					else
					{
						Fsm.GetFsmBool("isTurnRight").Value = true;
						Fsm.Event("OnTurnRight");
					}
					Finish();
				}
			}
		}
	}
}
