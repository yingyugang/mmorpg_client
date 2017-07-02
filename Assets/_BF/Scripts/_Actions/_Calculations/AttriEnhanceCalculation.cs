using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttriEnhanceCalculation : BaseCalculation {

	public override void OnEnter ()
	{
		base.OnEnter ();
		baseSkill.maxCastRound = 0;
		for (int i = 0; i < baseSkill.targetUnits.Count; i++) {
//			Debug.Log ("baseSkill.skillValue:" + baseSkill.skillValue);
			baseSkill.targetUnits[i].additionUnitAttribute.attributes [baseSkill.effectType] += baseSkill.skillValue;
		}
	}

	public override void OnPartmentCalculation (SkillEffectPartment partment, int index)
	{
		base.OnPartmentCalculation (partment, index);
	}

}
