using UnityEngine;
using System.Collections;
using StatusMachines;

public class BasePerform : UnitBaseAction {

	//演出的时间
	public float performDuration = 0.5f;
	float mPerformFinishTime;
	public BaseSkill baseSkill;

	public override void OnEnter ()
	{
		base.OnEnter ();
		mPerformFinishTime = Time.time + performDuration;
		OnPerformEnter ();
	}

	public override void OnUpdate ()
	{
		base.OnUpdate ();
		OnPerformUpdate ();
	}

	protected virtual void OnPerformEnter(){
		
	}

	protected virtual void OnPerformUpdate(){
		
	}

	//当进入开始一个攻击段
	public virtual void OnPartmentEnter(SkillEffectPartment partment,int index = 0){
	
	}

	//基于逻辑运算的演出
	public virtual void OnPartmentByCalculation(SkillEffectPartment partment,Unit target,Hashtable parameters,int index = 0){
		
	}

	//当进入开始一个攻击段
	public virtual void OnPartmentExit(SkillEffectPartment partment,int index = 0){

	}

	public bool IsPerformDone(){
		return mPerformFinishTime < Time.time;
	}

	public virtual void OnHit(){
	
	}

}
