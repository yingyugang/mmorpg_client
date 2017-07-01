using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	public class Fly : FsmStateAction {

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
				if(mEnemyController.enemyAttr.GetHealthPercent()<0.9f)
				{
					Fsm.GetFsmBool("isFly").Value = true;
					Fsm.Event("OnFly");
					Finish();
				}
			}
		}
	}
}
