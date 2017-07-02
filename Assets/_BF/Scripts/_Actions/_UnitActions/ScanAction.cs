using UnityEngine;
using System.Collections;
using StatusMachines;
using System.Collections.Generic;

public class ScanAction : UnitBaseAction
{

	public override void OnAwake ()
	{
		base.OnAwake ();
	}

	public override void OnEnter ()
	{
		base.OnEnter ();
		#region 1.Get current Skill.
		//获取到强制施放的技能。
		BaseSkill currentSkill = null;
		if(unit.unitSkill.isHandleSkill)
		{
			currentSkill = unit.unitSkill.handledSkill;
			unit.unitSkill.handledSkill = null;
			unit.unitSkill.isHandleSkill = false;
		}else{
//			hero.currentAttackAttribute = hero.heroAttack.GetAttackAttributes () [0];
			for (int i = 0; i < unit.activeSkills.Count; i++) {
				if (unit.activeSkills [i].maxCastRound > unit.activeSkills [i].currentCastCount) {
					if (currentSkill == null) {
						currentSkill = unit.activeSkills [i];
					} else {
						if (currentSkill.castPriority < unit.activeSkills [i].castPriority) {
							//施放几率
							int random = Random.Range (0, 10000);
							if (random < (unit.activeSkills [i].castPossibility + unit.baseUnitAttribute.attributes[EffectType.SkillOdds] + unit.additionUnitAttribute.attributes[EffectType.SkillOdds])) {
								currentSkill = unit.activeSkills [i];
							}
						}
					}
				}
			}
		}
		unit.currentSkill = currentSkill;
		//如果没有技能直接切换到待机，等于跳过这轮。
		if(currentSkill == null){
			statusMachine.ChangeStatus (_UnitMachineStatus.StandBy.ToString ());
			return;
		}
		#endregion
		#region 2.Get targets.
		//在回合游戏中，把对方所有单位都认为是在攻击搜索范围以内的。
		unit.targetUnits = ScanUtility.GetTargets(currentSkill);
		if(currentSkill.subSkills!=null){
			for(int i=0;i<currentSkill.subSkills.Count;i++){
				ScanUtility.GetTargets (currentSkill.subSkills[i]);
			}
		}
		#endregion
		#region 3.Get next status.
		Unit nearestTarget;
		bool needMove = CheckNeedMove(out nearestTarget);
		if (needMove)
			statusMachine.ChangeStatus (_UnitMachineStatus.MoveToAttack.ToString ());
		else
			statusMachine.ChangeStatus (_UnitMachineStatus.Cast.ToString ());
		#endregion
	}

	public override void OnUpdate ()
	{
		
	}

	protected virtual bool CheckNeedMove(out Unit nearestTarget){
		bool needMove = true;
	 	nearestTarget = null;
		float minDistance = Mathf.Infinity;
		for(int i=0;i<unit.targetUnits.Count;i++){
			float sqrDis = Vector2.SqrMagnitude((Vector2)(unit.targetUnits[i].transform.position - unit.transform.position));
			if(sqrDis < unit.currentSkill.skillRange * unit.currentSkill.skillRange){
				needMove = false;//是否在攻击范围内
			}
			if (sqrDis < minDistance) {
				minDistance = sqrDis;
				nearestTarget = unit.targetUnits [i];
			}
		}
		return needMove;
	}
}
