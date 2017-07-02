using UnityEngine;
using System.Collections;

public class BattleAttackDoneAction : BattleBaseAction {

	public override void OnAwake ()
	{
		base.OnAwake ();
	}

	float mMaxWaiting = 0.1f;
	float mOutTime = 0;

	public override void OnEnter ()
	{
		mOutTime = Time.time + mMaxWaiting;
		DropManager.GetInstance ().Collect ();
	}

	public override void OnUpdate ()
	{
		base.OnUpdate ();
		if (mOutTime >= Time.time) {
			return;
		}
		if (BattleUtility.CheckBattleFailure ()) {
			statusMachine.ChangeStatus (_BattleMachineStatus.Failure.ToString ());
		} else if (BattleUtility.CheckBattleSuccess ()) {
			statusMachine.ChangeStatus (_BattleMachineStatus.Success.ToString ());
		} else {
			statusMachine.ChangeStatus (_BattleMachineStatus.Attack.ToString ());
		}
	}

}
