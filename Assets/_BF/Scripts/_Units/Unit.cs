using UnityEngine;
using System.Collections;
using StatusMachines;
using System.Collections.Generic;

public enum _UnitMachineStatus{Cast,MoveToAttack,MoveToLocal,BuffRefresh,StandBy,Death,Cheer,Scan,Perform,Hit}
public class Unit : MonoBehaviour {

	public StatusMachine sm;
	public UnitAttribute baseUnitAttribute;
	public UnitAttribute additionUnitAttribute;

	public string unitId;
	public _Side side;
	public Vector3 defaultPos;
	public float moveSpeed = 25;

	public Dictionary<int,List<SkillEffectCSVStructure>> buffs;//keys is effect_type.

	public List<Unit> targetUnits;
	public UnitSkill unitSkill;
//	public Hero hero;
//	public HeroRes heroRes;
//	public HeroAnimation heroAnimation;

	public UnitAnimation unitAnimation;
	public UnitRes unitRes;
	public UnitEffect unitEffect;

//	public HeroEffect heroEffect;
	public BaseSkill currentSkill;
	public List<BaseSkill> activeSkills;//主动技能

//	public Animation animation;
//	public Animator animator;

	void Awake(){
		InitStatusMachine ();
//		hero = GetComponent<Hero> ();
//		heroRes = GetComponentInChildren<HeroRes> (true);
		unitSkill = GetComponent<UnitSkill> ();
		unitAnimation = GetComponentInChildren<UnitAnimation> ();
		unitRes = GetComponentInChildren<UnitRes> ();
		unitEffect = GetComponentInChildren<UnitEffect> ();
		baseUnitAttribute = new UnitAttribute ();
	}

	void InitStatusMachine(){
		sm = gameObject.AddComponent<StatusMachine> ();
		StatusAction action = new StandByAction();//待机
		sm.AddAction (_UnitMachineStatus.StandBy.ToString (), action);
		action = new ScanAction ();//寻找敌人，并决定是否使用技能
		sm.AddAction (_UnitMachineStatus.Scan.ToString (), action);
		action = new MoveToAttackAction();//移动到攻击
		sm.AddAction (_UnitMachineStatus.MoveToAttack.ToString(),action);
		action = new CastAction ();//攻击
		sm.AddAction (_UnitMachineStatus.Cast.ToString(),action);
		action = new MoveToLocalAction ();//移动到原始位置
		sm.AddAction (_UnitMachineStatus.MoveToLocal.ToString(),action);
		action = new DeathAction ();//死亡
		sm.AddAction (_UnitMachineStatus.Death.ToString(),action);
		action = new CheerAction ();//胜利
		sm.AddAction (_UnitMachineStatus.Cheer.ToString(),action);
		action = new HitAction ();//收击
		sm.AddAction(_UnitMachineStatus.Hit.ToString(),action);
	}

	public int GetAttribute(EffectType effectType){
		return baseUnitAttribute.attributes [effectType] + additionUnitAttribute.attributes [effectType];
	}

	public void PlayAnimAttack(){
//		hero.heroAnimation.Play (Hero.ACTION_ATTACK);
//		hero.heroEffect.PlayEffects(_AnimType.Attack, targetUnits);
	}

	public void Play(_UnitArtActionType actionName,List<Unit> targets = null)
	{
//		if (heroAnimation == null) {
//			heroAnimation = GetComponentInChildren<HeroAnimation> (true);
//		}
//		if (heroEffect == null) {
//			heroEffect = GetComponentInChildren<HeroEffect> (true);
//		}
//		if (heroAnimation != null) {
//			_AnimType animType = CommonUtility.AnimCilpNameStringToEnum (actionName.ToString());
//			heroAnimation.Play (actionName.ToString());
//		}
//		if (heroEffect != null) {
//			_AnimType animType = CommonUtility.AnimCilpNameStringToEnum (actionName.ToString());
//			heroEffect.PlayEffects (animType);
//		}

		if (unitAnimation != null) {
			unitAnimation.PlayAnimation (actionName.ToString());
		}
		if (unitEffect != null) {
			unitEffect.PlayEffects (actionName,targets);
		}
	}

	public float GetAnimationClipLenth(_UnitArtActionType clipName){
		return unitAnimation.GetAnimationClipLenth (clipName.ToString());
	}


}
