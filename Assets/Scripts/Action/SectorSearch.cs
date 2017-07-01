using UnityEngine;
using System.Collections;
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	public class SectorSearch : FsmStateAction {

		[RequiredField]
		public FsmFloat maxRadius = 5;
		[RequiredField]
		public FsmFloat minRadius = 3;
		[RequiredField]
		public FsmFloat searchAngle = 360;
		[RequiredField]
		public FsmFloat attackAngle = 120;
		[RequiredField]
		public FsmFloat cooldown = 2;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat cooldownParam;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmBool recordParam;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmEvent trueEvent;

		public FsmColor gizmosColor = Color.green;

		EnemyController mEnemyController;
		PlayerController mPlayerController;
		EnemyGizmos mEnemyGizmos;

		float mDirection;
		float mDistance;

		public override void Awake()
		{
			if (Application.isPlaying) {
				if(mEnemyController==null)
					mEnemyController = Fsm.GameObject.GetComponent<EnemyController>();
				if(mEnemyGizmos==null)
					mEnemyGizmos = Fsm.GameObject.GetComponent<EnemyGizmos>();
				if (mPlayerController == null)
					mPlayerController = BattleController.SingleTon ().playerController;
				if(mEnemyGizmos!=null)
				{
					mEnemyGizmos.Add(maxRadius.Value,gizmosColor.Value);
					mEnemyGizmos.Add(minRadius.Value,gizmosColor.Value);
					mEnemyGizmos.AddCooldown(cooldownParam,cooldown);
				}
			}
		}

		public override void OnUpdate()
		{
			if (!mPlayerController.pm.Fsm.GetFsmBool ("isDead").Value) 
			{
				mDistance = Vector3.Distance(mEnemyController.transform.position,mPlayerController.transform.position);
				if(mDistance >= minRadius.Value && mDistance < maxRadius.Value)
				{
					mDirection = CommonUtility.GetDirection (mEnemyController.transform.forward,mPlayerController.transform.position - mEnemyController.transform.position);
					if(Mathf.Abs(searchAngle.Value)>=360 || mDirection>=Mathf.Cos(searchAngle.Value/2/180 * Mathf.PI))
					{
						cooldownParam.Value = cooldownParam.Value + Time.deltaTime;
						if(Mathf.Abs(attackAngle.Value)>=360 || mDirection>=Mathf.Cos(attackAngle.Value/2/180 * Mathf.PI))
						{
							if(cooldownParam.Value >= cooldown.Value)
							{
								recordParam.Value = true;
								cooldownParam.Value = 0;
								Fsm.Event(trueEvent);
								Finish();
							}
						}
					}
				}
			}
		}
	}
}
