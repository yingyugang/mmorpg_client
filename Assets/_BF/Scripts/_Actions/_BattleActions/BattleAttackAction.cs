using UnityEngine;
using System.Collections;

public class BattleAttackAction : BattleBaseAction {

	Unit mCurrentAttacker;

	public override void OnEnter ()
	{
		base.OnEnter ();
		mCurrentAttacker = null;
		for(int i=0;i<mBattleManager.allUnits.Count;i++){
			Unit unit = mBattleManager.allUnits [i];
			unit.additionUnitAttribute.attributes [EffectType.UsedCD] = 0;
		}
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

	}

	public override void OnUpdate ()
	{
		base.OnUpdate ();
		if(mCurrentAttacker == null || mCurrentAttacker.sm.CurrentStatus() == _UnitMachineStatus.StandBy.ToString()){
			mCurrentAttacker = GetNextAttacker ();
			if (mCurrentAttacker == null) {
				statusMachine.ChangeStatus (_BattleMachineStatus.AttackDone.ToString());
			} else {
				mCurrentAttacker.sm.ChangeStatus (_UnitMachineStatus.Scan.ToString ());
				mCurrentAttacker.additionUnitAttribute.attributes [EffectType.UsedCD] ++;
			}
		}
	}

	Unit GetNextAttacker(){
		Unit speedUnit = null;
		for(int i=0;i<mBattleManager.allUnits.Count;i++){
			Unit unit = mBattleManager.allUnits [i];
			if (unit.GetAttribute (EffectType.CommonCD) >=  unit.GetAttribute(EffectType.UsedCD)) {
				if (speedUnit == null) {
					speedUnit = unit;
				}else if(unit.GetAttribute(EffectType.Speed) > speedUnit.GetAttribute(EffectType.Speed)){
					speedUnit = unit;
				}
			}
		}
		return speedUnit;
	}


}
