using UnityEngine;
using System.Collections;

//被动技能触发器(在技能初始化完毕就施放)
public class SelfSkillTrigger : BaseSkillTrigger
{
	public override void OnAwake ()
	{
		base.OnAwake ();
		baseSkill.targetUnits = GetTargets ();
//		if (baseSkill.subSkills != null) {
//			for (int i = 0; i < baseSkill.subSkills.Count; i++) {
//				baseSkill.subSkills [i].baseSkillTrigger.GetTargets ();
//			}
//		}
		baseSkill.maxCastRound = 0;
		for (int i = 0; i < baseSkill.targetUnits.Count; i++) {
			baseSkill.targetUnits[i].additionUnitAttribute.attributes [baseSkill.effectType] += baseSkill.skillValue;
		}
	}

}
