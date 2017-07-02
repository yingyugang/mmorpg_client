using UnityEngine;
using System.Collections;
using StatusMachines;

public enum CalculationResultType{
	IsCrit,
	IsDefence,
	IsDodge,
	IsRelation,
	RealDamage,
	RealHealth,
}

public class BaseCalculation : UnitBaseAction {

	public BaseSkill baseSkill;

	public virtual void OnPartmentCalculation(SkillEffectPartment partment,int index){
		
	}

	public virtual void OnCalculation(){
		
	}


}
