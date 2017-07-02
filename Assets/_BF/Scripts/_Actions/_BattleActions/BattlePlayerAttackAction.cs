using StatusMachines;
using UnityEngine;

public class BattlePlayerAttackAction:BattleBaseAction
{

	public override void OnAwake ()
	{
		base.OnAwake ();
	}

	int mCurrentIndex = 0;
	Unit mCurrentAttackUnit = null;

	public override void OnEnter ()
	{
		base.OnEnter ();
		mCurrentIndex = 0;
		mCurrentAttackUnit = null;
		bool hasPerformSkill = false;
		for(int i = 0;i < mBattleManager.allUnits.Count;i++){
			Unit unit = mBattleManager.allUnits [i];
			if(unit.unitSkill.handledSkills.Count > 0){
				unit.unitSkill.handledSkill = unit.unitSkill.handledSkills [0];
				unit.unitSkill.handledSkills.RemoveAt (0);
				unit.unitSkill.isHandleSkill = true;
				hasPerformSkill = true;
				mBattleManager.isPerformed = false;
			}
		}
		if (hasPerformSkill) {
			for (int i = 0; i < mBattleManager.allUnits.Count; i++) {
				Unit unit = mBattleManager.allUnits [i];
				unit.unitSkill.isHandleSkill = true;
			}
		}
		mBattleManager.hasPerformSKill = hasPerformSkill;
		if (!mBattleManager.isPerformed && !mBattleManager.hasPerformSKill) {
			Debug.Log ("Perform");
			statusMachine.ChangeStatus (_BattleMachineStatus.Perform.ToString ());
		}
		//		Debug.Log ("mBattleManager.isPerformed:" + mBattleManager.isPerformed + "||" + "mBattleManager.hasPerformSKill:" + mBattleManager.hasPerformSKill);
	}

	public override void OnUpdate ()
	{
		base.OnUpdate ();
		//如果当前人物没有攻击结束就一直等待。
		if (mCurrentAttackUnit != null && mCurrentAttackUnit.sm.CurrentStatus () != _UnitMachineStatus.StandBy.ToString ()) {
			return;
		}
		if (mBattleManager.rightUnits.Count <= mCurrentIndex || BattleUtility.CheckBattleFailure () || BattleUtility.CheckBattleSuccess ()) {
			statusMachine.ChangeStatus (_BattleMachineStatus.ToggleToEnemy.ToString ());
			return;
		}
		if (mBattleManager.rightUnits [mCurrentIndex].GetComponent<Hero> ().heroAttribute.calculateHP > 0) {
			mCurrentAttackUnit = mBattleManager.rightUnits [mCurrentIndex];
			mCurrentAttackUnit.sm.ChangeStatus (_UnitMachineStatus.Scan.ToString ());
		}
		mCurrentIndex++;
	}
}
