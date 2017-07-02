using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StatusMachines;

public enum Element{
	
}

public enum BuffCastType
{
	//被动技能,在OnAwake里面就释放的技能
	Self = 0,
	//主动技能
	ActiveSkill = 1,
	//装备增强
	Equipment = 2}
;
//反伤害技能，阳极解放伤害，阳极解放的能量回复值
public enum EffectType
{
	//血量
	HP = 0,
	//最大血量
	MaxHP = 1,
	//暴击率
	CritOdds = 2,
	//伤害增强百分比
	DamageMuilt = 3,
	//防御力增强百分比
	DefenceMuilt = 4,
	//伤害减免百分比
	DamageReduceMuilt = 9,
	//元素相克加成百分比
	ElementMuilt = 5,
	//技能释放几率
	SkillOdds = 6,
	//闪避几率
	DodgeOdds = 7,
	//防御几率
	DefenceOdds = 8,
	//吸血
	SuckBlood = 10,
	//影响攻击顺序
	Speed = 11,
	//回合中是否攻击
	CommonCD = 12,
	//回合中是否攻击
	UsedCD = 13,
	//Element
	Element = 14,
	//基础伤害
	BaseDamage = 15,
	//基础防御
	BaseDefence = 16

}
;
//覆盖类型
public enum BuffCoverType
{
	//直接累加,各自计算持续回合数
	AddUp = 0,
	//直接覆盖
	Cover = 1
}
;
//攻击时每段的攻击方式
public enum ImpactType
{
	//在每段攻击中不切换目标
	NotToggleTarget = 0,
	//在每段攻击中切换目标
	ToggleTargetWithPartment = 1
}
;
//技能对象类型(用于搜寻目标)
public enum TargetType
{
	Friend = 0,
	Enemy = 1,
	All = 2,
	Self = 3,
	//作用于技能本身
	Skill = 4
}
;
//技能演出类型
public enum PerformType
{
	NormalAttack = 0,
	ActiveSkill = 1,
	AttributeEnhance = 2
}
;

public class BaseSkill : UnitBaseAction
{
	//最大施放回合数；
	public int maxCastRound;
	//当前回合数；
	public int currentCastCount;
	//施放优先级，优先级越高越先施放
	public int castPriority = 0;
	//施放几率
	public int castPossibility = 100;
	//施放的对象
	public TargetType castTargetType = TargetType.Enemy;
	//施放者
	public GameObject caster;
	//子buff
	public List<BaseSkill> subSkills;
	//根据buffCastType和buffEffectType和buffValue决定是覆盖还是叠加
	public BuffCastType buffCastType;
	//技能类型
	public EffectType effectType;
	//技能值
	public int skillValue;
	//技能覆盖方式
	public BuffCoverType coverType;
	//技能触发器（之所以要提出来，是因为可以重用）
	public BaseSkillTrigger baseSkillTrigger;
	//技能演出相关（之所以要提出来，是因为可以重用）
	public BasePerform basePerform;
	//技能数值Calculate
	public BaseCalculation baseCalculation;
	//攻击距离，0近战，1远程，只针对rpg.（攻击距离，如果是）
	public int skillRange = 0;
	//受影响的单位数
	public int impactCount = 1;
	//技能攻击次数（这个属性会影响攻击的结果，比如一个两次攻击技能，可以能是击中一个目标两次，也可能是分别攻击两个目标一次，对数值也是有影响的）
	public int castNum = 1;
	//攻击方式
	public ImpactType impactType = ImpactType.NotToggleTarget;
	//攻击段数（至少会有2段，最后一段作为技能的后摇时间）
	public List<SkillEffectPartment> partments;
	//攻击元素类型
	public _ElementType elementType;
	//冷却时间
	public float coolDownTime = 100;
	//主技能
	public BaseSkill superSkill;
	//基于技能属性增强，暴击，吸血等。
	public UnitAttribute skillAttribute;
	//攻击或者移动的首要目标（第一次）
	public List<Unit> targetUnits;
	int mCurrentAttackIndex;
	float mNextAttackQueueTime;
//	float mPower;
	SkillEffectPartment mCurrentPartment;

	public override void OnAwake ()
	{
		base.OnAwake ();
		if (basePerform != null) {
			basePerform.GO = GO;
			basePerform.statusMachine = statusMachine;
			basePerform.baseSkill = this;
			basePerform.OnAwake ();
		}
		if (baseCalculation != null) {
			baseCalculation.GO = GO;
			baseCalculation.statusMachine = statusMachine;
			baseCalculation.baseSkill = this;
			baseCalculation.OnAwake ();
		}
		if (baseSkillTrigger != null) {
			baseSkillTrigger.GO = GO;
			baseSkillTrigger.statusMachine = statusMachine;
			baseSkillTrigger.baseSkill = this;
			baseSkillTrigger.OnAwake ();
		}
		if(subSkills!=null){
			for(int i=0;i<subSkills.Count;i++){
				subSkills [i].OnAwake ();
			}
		}
		skillAttribute = new UnitAttribute ();
	}

	public override void OnEnter ()
	{
		base.OnEnter ();
		if(subSkills!=null){
			for(int i=0;i<subSkills.Count;i++){
				subSkills [i].OnEnter ();
			}
		}
		OnSkillEnter ();
	}

	public override void OnUpdate ()
	{
		base.OnUpdate ();
		if(subSkills!=null){
			for(int i=0;i<subSkills.Count;i++){
				subSkills [i].OnUpdate ();
			}
		}
		OnSkillUpdate ();
	}

	protected virtual void OnSkillEnter ()
	{
		if(currentCastCount >= maxCastRound){
			return;
		}
		if (basePerform != null) {
			basePerform.OnEnter ();
		}
		if (baseCalculation != null) {
			baseCalculation.OnEnter ();
		}
		if (baseSkillTrigger != null) {
			baseSkillTrigger.OnEnter ();
		}
		currentCastCount++;
		mCurrentAttackIndex = -1;
		mNextAttackQueueTime = Time.time;
		NextCurrentPartment ();
	}
	//技能Update
	protected virtual void OnSkillUpdate ()
	{
		if (mCurrentAttackIndex >= partments.Count) {
			OnSkillDone ();
			return;
		}
		if (basePerform != null) {
			basePerform.OnUpdate ();
		}
		if (baseCalculation != null) {
			baseCalculation.OnUpdate ();
		}
		if (baseSkillTrigger != null) {
			baseSkillTrigger.OnUpdate ();
		}
		if (mNextAttackQueueTime <= Time.time) {
			if (baseCalculation != null) {
				baseCalculation.OnPartmentCalculation (mCurrentPartment, mCurrentAttackIndex);
			}
			NextCurrentPartment ();
		}
	}

	protected virtual void OnSkillDone(){
		//没有super skill证明是主要技能
		if (superSkill == null) {
			unit.sm.ChangeStatus (_UnitMachineStatus.MoveToLocal.ToString ());
		}
	}

	bool NextCurrentPartment ()
	{
		if (basePerform != null && mCurrentPartment != null) {
			basePerform.OnPartmentExit (mCurrentPartment,mCurrentAttackIndex);
		}
		mCurrentAttackIndex++;
		if (partments != null && partments.Count > mCurrentAttackIndex) {
			float diff = Time.time - mNextAttackQueueTime;
			mCurrentPartment = partments [mCurrentAttackIndex];
			mNextAttackQueueTime = Time.time + mCurrentPartment.delay - diff;
//			mPower = mCurrentPartment.effectValue / mCurrentPartment.partCount;
			if (basePerform != null) {
				basePerform.OnPartmentEnter (mCurrentPartment,mCurrentAttackIndex);
			}
			return true;
		}
		return false;
	}

}
