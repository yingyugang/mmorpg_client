using UnityEngine;
using System.Collections;
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	public class SearchPlayer : FsmStateAction {

		[RequiredField]
		public FsmFloat distance = 20;

		public FsmEvent isTrue;


		public override void Reset()
		{
		}
		
		public override void OnEnter()
		{
			if (playerController == null) {
				playerController = BattleController.SingleTon().playerController;		
			}
		}

		PlayerController playerController;
		float mDistance;
		public override void OnUpdate()
		{
			mDistance = Vector3.Distance (Fsm.GameObject.transform.position,playerController.transform.position);
			if(mDistance <= distance.Value)
			{
//				Fsm.Event(isTrue);
				Fsm.Variables.FindFsmBool("isBattle").Value = true;
			}
		}
	}

}