using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ScanUtility  {

	//两种方式，每一段攻击切换目标和所有攻击都不切换目标。
	public static List<Unit> GetTargets (BaseSkill baseSkill)
	{
		List<Unit> mainUnits = GetEffectTargets (baseSkill);
		baseSkill.targetUnits = mainUnits;
		if (baseSkill.impactType == ImpactType.NotToggleTarget) {
			for (int i = 0; i < baseSkill.partments.Count; i++) {
				SkillEffectPartment partment = baseSkill.partments [i];
				partment.partmentTargets = mainUnits;
			}
		} else {
			for (int i = 0; i < baseSkill.partments.Count; i++) {
				SkillEffectPartment partment = baseSkill.partments [i];
				if(i>0)
					partment.partmentTargets = GetEffectTargets (baseSkill);
				else
					partment.partmentTargets = mainUnits;
			}
		}
		return mainUnits;
	}

	public static List<Unit> GetEffectTargets(BaseSkill baseSkill)
	{
		int impactCount = baseSkill.impactCount;
		List<Unit> units = new List<Unit>();
		if (baseSkill.unit.side == _Side.Enemy) {
			switch(baseSkill.castTargetType){
			case TargetType.Enemy:
				units = BattleManager.GetInstance ().GetRandomAlivePlayerUnits (impactCount);
				break;
			case TargetType.Friend:
				units = BattleManager.GetInstance ().GetRandomAliveEnemyUnits (impactCount);
				break;
			case TargetType.All:
				units = BattleManager.GetInstance ().GetRandomAliveAllUnits (impactCount);
				break;
			case TargetType.Self:
				units = BattleManager.GetInstance ().GetRandomAlivePlayerUnitsWithTargetUnit (baseSkill.unit,impactCount);
				break;
			}
		} else {
			switch(baseSkill.castTargetType){
			case TargetType.Enemy:
				units = BattleManager.GetInstance ().GetRandomAliveEnemyUnits(impactCount);
				break;
			case TargetType.Friend:
				units = BattleManager.GetInstance ().GetRandomAlivePlayerUnits  (impactCount);
				break;
			case TargetType.All:
				units = BattleManager.GetInstance ().GetRandomAliveAllUnits (impactCount);
				break;
			case TargetType.Self:
				units = BattleManager.GetInstance ().GetRandomAliveEnemyUnitsWithTargetUnit (baseSkill.unit,impactCount);
				break;
			}
		}
		return units;
	}

}
