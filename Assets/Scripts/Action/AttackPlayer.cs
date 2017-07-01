using UnityEngine;
using System.Collections;
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	public class AttackPlayer : FsmStateAction {

		[RequiredField]
		public FsmVector2 attackDistance = new Vector2 (1,5);
		[RequiredField]
		public FsmVector2 sprintDistance = new Vector2 (5,10);
		[RequiredField]
		public FsmVector2 fireDistance = new Vector2 (10,20);
		[RequiredField]
		public FsmFloat rotateSpeed = 1;

		private PlayerController mPlayerController;
		private EnemyController mEnemyController;

		private float mDistance;
		public override void Reset()
		{

		}
		
		public override void OnEnter()
		{
			if (mPlayerController == null) 
			{
				mPlayerController = BattleController.SingleTon().playerController;		
			}
			if (mEnemyController==null)
			{
				mEnemyController = Fsm.GameObject.GetComponent<EnemyController>();

//				mEnemyController.enemyGizmos.Add(attackDistance.Value.x,Color.green);
//				mEnemyController.enemyGizmos.Add(attackDistance.Value.y,Color.green);
//				mEnemyController.enemyGizmos.Add(sprintDistance.Value.x,Color.yellow);
//				mEnemyController.enemyGizmos.Add(sprintDistance.Value.y,Color.yellow);
//				mEnemyController.enemyGizmos.Add(fireDistance.Value.x,Color.red);
//				mEnemyController.enemyGizmos.Add(fireDistance.Value.y,Color.red);

			}
		}

		void RotateToPlayer()
		{
			Vector3 direction = mPlayerController.transform.position - mEnemyController.transform.position;
			Quaternion qua = Quaternion.LookRotation (direction);
			mEnemyController.transform.rotation = Quaternion.Slerp (mEnemyController.transform.rotation, qua, Time.deltaTime * rotateSpeed.Value);
		}


		float mDirection = 0;
		public override void OnUpdate()
		{
			if (!mPlayerController.pm.Fsm.GetFsmBool ("isDead").Value) {
				mDirection = CommonUtility.GetDirection (mEnemyController.transform.forward,mPlayerController.transform.position - mEnemyController.transform.position);
				RotateToPlayer ();
				if(mDirection > 0.9f)
				{
					mDistance = Vector3.Distance(mPlayerController.transform.position,mEnemyController.transform.position);
					if(mDistance >= attackDistance.Value.x && mDistance < attackDistance.Value.y)
					{
//						if(mEnemyController.AttackHead())
//						{
//							Finish();
//						}
					}
					else if(mDistance >= sprintDistance.Value.x && mDistance < sprintDistance.Value.y)
					{
//						if(mEnemyController.AttackSprint())
//						{
//							Finish();
//						}
					}
					else if(mDistance >= fireDistance.Value.x && mDistance < fireDistance.Value.y)
					{
//						if(mEnemyController.AttackFireBall())
//						{
//							Finish();
//						}
					}
				}
			}
		}
	}



}