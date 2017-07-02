using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HPCalculation : BaseCalculation
{

	public override void OnEnter ()
	{
		base.OnEnter ();
		for (int i = 0; i < baseSkill.partments.Count; i++) {
			List<Hashtable> mParameters = new List<Hashtable> ();
			for (int j = 0; j < baseSkill.partments [i].partmentTargets.Count; j++) {
				Hashtable param = CalculateDamage (baseSkill.partments [i].partmentTargets [j]);
				mParameters.Add (param);
			}
			baseSkill.partments [i].partmentParams = mParameters;
		}
	}

	public override void OnPartmentCalculation (SkillEffectPartment partment, int index)
	{
		base.OnPartmentCalculation (partment, index);
		if (partment !=null && partment.partmentTargets != null) {
			for (int i = 0; i < partment.partmentTargets.Count; i++) {
				if (baseSkill.basePerform != null) {
					baseSkill.basePerform.OnPartmentByCalculation (partment, partment.partmentTargets [i], partment.partmentParams [i], index);
				}
			}
		}
	}

	int GetAttribute(EffectType effectType)
	{
		int attri = unit.baseUnitAttribute.attributes [effectType] + unit.additionUnitAttribute.attributes [effectType] + baseSkill.skillAttribute.attributes[effectType];
		if(baseSkill.subSkills!=null && baseSkill.subSkills.Count > 0 )
		{
			for (int i = 0; i < baseSkill.subSkills.Count; i++) 
			{
				attri += baseSkill.subSkills [i].skillAttribute.attributes [effectType];
			}
		}
		return attri;
	}

	bool IsPossibility(EffectType effectType)
	{
		int odds = GetAttribute (effectType);
		bool isPossibility = Random.Range (0, 10000) < odds ? true : false;
		return isPossibility;
	}

	Hashtable CalculateDamage (Unit target)
	{
		//1.是否暴击
		bool isCrit = IsPossibility(EffectType.CritOdds);
		//2.是否属性相克(比较攻击者的技能属性和受击者的本体属性)
		bool isRelation = (int)baseSkill.elementType == target.GetAttribute(EffectType.Element) ? true : false;
		//3.是否防御
		bool isDefence = IsPossibility(EffectType.DefenceOdds);
		//4.是否闪避
		bool isDodge = IsPossibility(EffectType.DodgeOdds);
		//5.真实伤害
		int realDamage = GetRealDamage (unit, target, isRelation, isCrit, isDefence, isDodge);
		//6.真实回血
		int suckBloodPercent = GetAttribute(EffectType.SuckBlood);
		int suckBlood = Mathf.RoundToInt(realDamage * suckBloodPercent / 10000f);
		Hashtable param = new Hashtable ();
		param.Add (CalculationResultType.IsCrit, isCrit);
		param.Add (CalculationResultType.IsDefence, isDefence);
		param.Add (CalculationResultType.IsDodge, isDodge);
		param.Add (CalculationResultType.IsRelation, isRelation);
		param.Add (CalculationResultType.RealDamage, realDamage);
		param.Add (CalculationResultType.RealHealth, suckBlood);
		return param;
	}
	
	//伤害计算公式
	int GetRealDamage (Unit attacker, Unit target, bool isRelation, bool isCrit, bool isDenfence, bool isDodge)
	{
		if (isDodge)
			return 0;
		float critMulti = isCrit ? 1 : 0;
		float defenceMulti = isDenfence ? 1 : 0;
		float relationMulti = isRelation ? 1.5f : 0;
		float realDamage = (attacker.GetAttribute(EffectType.BaseDamage) - attacker.GetAttribute(EffectType.BaseDefence)) * (1 + relationMulti) * (1 + critMulti) * (1 - defenceMulti); 
//		//降低伤害
//		realDamage = realDamage * def.heroAttribute.decreaseDamage;
		return Mathf.Max (1, Mathf.RoundToInt (realDamage));
	}

}
