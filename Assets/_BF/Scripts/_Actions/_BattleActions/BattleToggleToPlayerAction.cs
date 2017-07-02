using StatusMachines;
using UnityEngine;

public class BattleToggleToPlayerAction:BattleBaseAction
{
	int mCurrentRound = 0;

	public override void OnAwake ()
	{
		base.OnAwake ();
	}

	float mMaxWaiting = 0.1f;
	float mOutTime = 0;

	public override void OnEnter ()
	{
		mOutTime = Time.time + mMaxWaiting;
		mCurrentRound++;
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
		}else{
			statusMachine.ChangeStatus (_BattleMachineStatus.PlayerAttack.ToString ());
		}

	}
}