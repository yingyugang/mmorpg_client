using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillAttriEnhanceCalculation  : BaseCalculation
{

	public override void OnEnter ()
	{
		base.OnEnter ();
		baseSkill.skillAttribute = new UnitAttribute();
		baseSkill.skillAttribute.attributes [baseSkill.effectType] += baseSkill.skillValue;
	}

	public override void OnPartmentCalculation (SkillEffectPartment partment, int index)
	{
		base.OnPartmentCalculation (partment, index);
	}

}
