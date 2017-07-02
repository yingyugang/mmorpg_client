using StatusMachines;

public class BattleEnemyAttackAction:BattleBaseAction
{

	int mCurrentIndex = 0;
	Unit mCurrentAttackUnit = null;

	public override void OnEnter ()
	{
		base.OnEnter ();
		mCurrentIndex = 0;
		mCurrentAttackUnit = null;
	}

	public override void OnUpdate ()
	{
		base.OnUpdate ();
		//如果当前人物没有攻击结束就一直等待。
		if (mCurrentAttackUnit != null && mCurrentAttackUnit.sm.CurrentStatus () != _UnitMachineStatus.StandBy.ToString ()) {
			return;
		}
		if (mBattleManager.rightUnits.Count <= mCurrentIndex || BattleUtility.CheckBattleFailure () || BattleUtility.CheckBattleSuccess ()) {
			statusMachine.ChangeStatus (_BattleMachineStatus.ToggleToPlayer.ToString ());
			return;
		}
		if (mBattleManager.leftUnits [mCurrentIndex].GetComponent<Hero> ().heroAttribute.calculateHP > 0) {
			mCurrentAttackUnit = mBattleManager.leftUnits [mCurrentIndex];
			mCurrentAttackUnit.sm.ChangeStatus (_UnitMachineStatus.Scan.ToString ());
		}
		mCurrentIndex++;
	}

}
