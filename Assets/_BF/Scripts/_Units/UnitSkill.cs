using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitSkill : MonoBehaviour
{

	Unit mUnit;
	//由AI强制施放的技能。
	public bool isHandleSkill;
	public BaseSkill handledSkill;
	public List<BaseSkill> handledSkills;

	void Awake ()
	{
		mUnit = GetComponent<Unit> ();
	}

	public void InitUnitSkills ()
	{
		mUnit.activeSkills = new List<BaseSkill> ();
		int unit_id = int.Parse(mUnit.unitId) ;
		Debug.Log ("unit_id:" + unit_id);
		List<UnitSkillCSVStructure> unitSkillCSVStructures = CSVManager.GetInstance ().unitSkillGroupDic [unit_id];
		for (int i = 0; i < unitSkillCSVStructures.Count; i++) {
			SkillCSVStructure skillCSVStructure = CSVManager.GetInstance ().skillDic [unitSkillCSVStructures [i].skill_id];
			BaseSkill baseSkill = UnitSkill.InitBaseSkill (skillCSVStructure, mUnit);
			if (skillCSVStructure.subSkills!=null && skillCSVStructure.subSkills.Count > 0) {
				for (int j = 0; j < skillCSVStructure.subSkills.Count; j++) {
					if (baseSkill.subSkills == null) {
						baseSkill.subSkills = new List<BaseSkill> ();
					}
					SkillCSVStructure subSkillCSVStructure = null;
					if (CSVManager.GetInstance ().skillDic.TryGetValue (skillCSVStructure.subSkills [j].subSkillId, out subSkillCSVStructure)) {
						BaseSkill subSkill = UnitSkill.InitBaseSkill (subSkillCSVStructure, mUnit);
						baseSkill.subSkills.Add (subSkill);
						subSkill.superSkill = baseSkill;
					}
				}
			}
			mUnit.activeSkills.Add (baseSkill);
		}
		#region old
//		DefaultAttackSkill defaultAttackSkill = new DefaultAttackSkill ();
//		defaultAttackSkill.buffCastType = BuffCastType.ActiveSkill;
//		defaultAttackSkill.castTargetType = TargetType.Enemy;
//		defaultAttackSkill.caster = gameObject;
//		defaultAttackSkill.impactCount = 1;
//		defaultAttackSkill.maxCastRound = 999999999;
//		defaultAttackSkill.castPriority = 1;
//		defaultAttackSkill.castPossibility = 100;
//		if (GetComponent<Hero> ().heroAttribute.heroInfo.movable == 2) {
//			defaultAttackSkill.skillRange = 100;
//		} else {
//			defaultAttackSkill.skillRange = 0;
//		}
//		BasePerform basePerform = new DefaultAttackPerform ();
//		defaultAttackSkill.basePerform = basePerform;
//		List<AttackEffectPartment> partments = new List<AttackEffectPartment>();
//		AttackEffectPartment partment = new AttackEffectPartment ();
//		partment.delay = 1.0f;
//		partment.partCount = 10;
//		partments.Add (partment);
//		partment = new AttackEffectPartment ();
//		partment.delay = 1f;
//		partments.Add (partment);
//		defaultAttackSkill.partments = partments;
//		mUnit.activeSkills.Add(defaultAttackSkill);
//		#endregion
//
//		#region 2.skill 
//		defaultAttackSkill = new DefaultAttackSkill ();
//		partments = new List<AttackEffectPartment>();
//		partment = new AttackEffectPartment ();
//		partment.delay = 1.6f;
//		partment.partCount = 10;
//		partments.Add (partment);
//		partment = new AttackEffectPartment ();
//		partment.delay = 2f;
//		partments.Add (partment);
//		defaultAttackSkill.partments = partments;
//		defaultAttackSkill.buffCastType = BuffCastType.ActiveSkill;
//		defaultAttackSkill.caster = gameObject;
//		defaultAttackSkill.maxCastRound = 999999999;
//		defaultAttackSkill.castTargetType = TargetType.Enemy;
//		defaultAttackSkill.castPossibility = 20;
//		defaultAttackSkill.castPriority = 2;
//		defaultAttackSkill.impactCount = 2;
//		if (GetComponent<Hero> ().heroAttribute.heroInfo.movable == 2) {
//			defaultAttackSkill.skillRange = 100;
//		} else {
//			defaultAttackSkill.skillRange = 0;
//		}
//		basePerform = new SkillAttackPerform ();
//		defaultAttackSkill.basePerform = basePerform;
//		mUnit.activeSkills.Add(defaultAttackSkill);
//		#endregion
//
//		#region 3.战斗开始时的buffskill
//		AttributeEnhanceSkill attributeEnhanceSkill = new AttributeEnhanceSkill ();
//		attributeEnhanceSkill.attributeType = AttributeType.Defence;
//		attributeEnhanceSkill.buffValue = 10;
//		attributeEnhanceSkill.maxCastRound = 1;
//		attributeEnhanceSkill.skillRange = 100;
//		attributeEnhanceSkill.impactCount = 5;
//		attributeEnhanceSkill.castPriority = 3;
//		attributeEnhanceSkill.castPossibility = 100;
//		attributeEnhanceSkill.castTargetType = TargetType.Self;
//		basePerform = new AttributeEnhancePerform ();
//		attributeEnhanceSkill.basePerform = basePerform;
//		attributeEnhanceSkill.GO = gameObject;
//		attributeEnhanceSkill.OnAwake ();
//		mUnit.activeSkills.Add (attributeEnhanceSkill);
		#endregion
	}

	static public BaseSkill InitBaseSkill (SkillCSVStructure skillCSVStructure, Unit unit)
	{
		BaseSkill baseSkill = new BaseSkill ();
		baseSkill.buffCastType = (BuffCastType)skillCSVStructure.type;
		baseSkill.castTargetType = (TargetType)skillCSVStructure.cast_target_type;
		baseSkill.impactCount = skillCSVStructure.impact_count;
		baseSkill.impactType = (ImpactType)skillCSVStructure.impact_type;
		baseSkill.effectType = (EffectType)skillCSVStructure.effect_type;
		int maxCastRound = skillCSVStructure.max_cast_round;
		baseSkill.maxCastRound = maxCastRound <= 0 ? 99999999 : maxCastRound;
		baseSkill.castPriority = skillCSVStructure.cast_priority;
		baseSkill.castPossibility = skillCSVStructure.cast_possibility;
		baseSkill.skillRange = skillCSVStructure.range;
		baseSkill.skillValue = Random.Range (skillCSVStructure.effect_percent_value_min, skillCSVStructure.effect_percent_value_max);
		if (skillCSVStructure.element_type != 0) {
			baseSkill.elementType = (_ElementType)skillCSVStructure.element_type;
		} 
//		else {
//			baseSkill.elementType = unit.hero.heroAttribute.elementType;
//		}
		if (skillCSVStructure.partments != null && skillCSVStructure.partments.Count > 0) {
			baseSkill.partments = skillCSVStructure.partments;
		} else {
			//保证partment至少有两段
			baseSkill.partments = new List<SkillEffectPartment> ();
			SkillEffectPartment ae = new SkillEffectPartment ();
			ae.delay = 0;
			ae.partCount = 1;
			ae.effectValue = 1;
			baseSkill.partments.Add (ae);
			ae = new SkillEffectPartment ();
			ae.delay = 0;
			ae.partCount = 1;
			ae.effectValue = 0;
			baseSkill.partments.Add (ae);
		}
//		switch (baseSkill.buffCastType) {
//		case BuffCastType.Self:
//			baseSkill.baseSkillTrigger = new SelfSkillTrigger ();
//			break;
//		}
		switch (skillCSVStructure.perform_type) {
		case 0:
			baseSkill.basePerform = new DefaultAttackPerform ();
			break;
		case 1:
			baseSkill.basePerform = new SkillAttackPerform ();
			break;
		case 2:
			baseSkill.basePerform = new DefaultAttackWithPosChangePerform ();
			break;
		case 3:
			baseSkill.basePerform = new SkillAttackWithPosChangePerform ();
			break;
		}
		switch (baseSkill.effectType) {
		case EffectType.HP:
			baseSkill.baseCalculation = new HPCalculation ();
			break;
		default:
			if (baseSkill.castTargetType == TargetType.Skill)
				baseSkill.baseCalculation = new SkillAttriEnhanceCalculation ();
			else
				baseSkill.baseCalculation = new AttriEnhanceCalculation ();
			break;
		}
		baseSkill.caster = unit.gameObject;
		baseSkill.GO = unit.gameObject;
		baseSkill.OnAwake ();
		return baseSkill;
	}


}

