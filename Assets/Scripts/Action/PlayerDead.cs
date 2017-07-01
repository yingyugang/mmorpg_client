using UnityEngine;
using System.Collections;
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	public class PlayerDead : FsmStateAction {

		public override void OnEnter()
		{
			Fsm.GetFsmBool ("isDead").Value = true;
			Fsm.GameObject.GetComponent<Animator> ().SetBool ("isDead",true);
		}

		public override void OnUpdate()
		{
			if(!Fsm.GetFsmBool("isDead").Value)
			{
				Fsm.GameObject.GetComponent<Animator> ().SetBool ("isDead",false);
				PlayMakerFSM pm = Fsm.GameObject.GetComponent<PlayMakerFSM>();
				FsmBool[] fssBool = pm.FsmVariables.BoolVariables;
				foreach(FsmBool fb in fssBool)
				{
					fb.Value = false;
				}
				Finish();
			}
		}
	}
}
