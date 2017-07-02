using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib; 


public class SkillBase
{
	public int sid;
	public string name;
	public string desc;
	public string icon;
	public string actionDisplay;
	public int skillType;
	public int powerReq;
	public List<int> skillResultID;
	public List<int> skillAdvance;
}

public class SkillResult
{
	public int id;
	public int skillType;
	public AttackType effecttype;
	public int specialcondition;
	public int scdata1;
	public int target;
	public int tdata1;
	public int targetnum;
	public int delaytime;
	public string bufficon;
	public int effectobject;
	public int effectvaluetype;
	public float effectvalue1;
	public float effectvalue2;
	public int skillAdvance;
}


/*public enum BattleState
{
	noCondition = 0,
	RoundCount = 1,
	RoundStart = 2,
	RoundEnd = 3,
	IsCombo = 4,
	IsCritical = 5,
	HeroType = 6,
	Attacking = 7,
	BattleBegin = 8
}*/

public enum SkillCondition
{
	noCondition = 0,
	RoundCount = 1,
	RoundStart = 2,
	RoundEnd = 3,
	IsCombo = 4,
	IsCritical = 5,
	HeroType = 6,
	Attacking = 7
}

public enum TargetHero
{
	enemy = 1,
	friend = 2,
}

public enum EffectObject
{
	MaxHP = 1,
	CurrHP = 2,
	damage = 3,
	defence = 4,
	recover = 5,
	BB = 6,
	criticalRate = 7,
	criticalAdd = 8,
	comboAdd = 9,
	beDamage = 10,
	beAbnormalRate = 11,
	beTriggerAbnormalRate = 12,
	BBRate = 13,
	attackAddHP = 14,
	reboundDamage = 15,
	heartRecover = 16,
	BBRecover = 17,
	poision = 18,
	paralysis = 19,
	noSkillAndBB = 20,
	addWater = 21,
	addFire = 22,
	addWood = 23,
	addWind = 24,
	addHoly = 25,
	addEvil = 26,
}

public enum AnnormalType
{
	none = 0,
	poison = 1,
	sick = 2,
	weak = 3,
	hurt = 4,
	palsy = 5,
	curse = 6,
}

public enum BuffType
{
	maxHP = 1,
	currentHP = 2,
	attack = 3,
	defence = 4,
	recover = 5,
	bc = 6,
	additionCritical = 7,
	promoteCritical = 8,
	currentDamage = 9,
	combo = 10,
	additionHCRevert = 11,
	additionBBDrop = 12,
	isImmuneAbnormal = 13,
	decreaseDamage = 14,
	currentSkillDamageRatio = 15,

	Wind = 16,
	Wood = 17,
	Water = 18,
	Fire = 19,
	Holy = 20,
	Evil = 21,

	groupRecover = 22,
	additionEnergy = 23,

}

public class AnnormalState
{
	public AnnormalType type;
	public int roundcount;
	public float value;
}

public class BuffState
{
	public BuffType buffType;
	public int roundcount;
	//public Hero selfHero;
	//public List<Hero> targetHero;
	public float value;
}

public class SkillManager: MonoBehaviour
{
	

	public SkillBase skillBase;

	public List<SkillResult> skillResultList;

	public HeroAttack heroAttack;
	public List<AttackAttribute> attackAttributes;
	public HeroAttribute currHeroAttr;
	public Hero currHero;

	public List<Hero> targetHeroList;
	public List<Hero> selfHeroList;


	public int currRoundIndex;
	//public BattleState battleState;
	public bool isCritical;
	public bool isCombo;
	public bool isBattleBegin;
	public float advanceRatio; //技能等级系数

	
	public List<AnnormalState> annormalStateList;
	public List<BuffState> buffStateList;

	private GameObject buffPoint;

	public void initSkillbase(int skillID)
	{	
		//Debug.Log("...............skillManager..............");
		if (skillResultList == null)
		{
			skillResultList = new List<SkillResult>();
		}
		targetHeroList = new List<Hero>();
		selfHeroList = new List<Hero>();

		//annormalStateList = new List<AnnormalState>();
		buffStateList = new List<BuffState>();
		
		currHero = transform.GetComponent<Hero>();
		currHeroAttr = currHero.heroAttribute;
		advanceRatio = 0;

		//annormalStateList = currHeroAttr.annormalStateList;
		
		heroAttack = transform.gameObject.GetComponent<HeroAttack>();
		attackAttributes = heroAttack.GetAttackAttributes();

		skillBase = DataManager.getModule<DataSkill>(DATA_MODULE.Data_Skill).getSkillBaseDataBySkillID(skillID);	
		//hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(currHero.id);

		//buffPoint = hero.transform.FindChild("BuffPoint").gameObject;
		//Buff buffIcon = buffPoint.GetComponent<Buff>();
		initSkillResult();

	}

	public void initSkillResult()
	{
		//if (skillResultList.Count > 0)	
		//	skillResultList.Clear();

		bool isFirstSkillResult = false;
		if (skillBase != null && skillBase.skillResultID != null && skillBase.skillResultID.Count > 0)
		{
			for (int i = 0; i < skillBase.skillResultID.Count; i ++ )
			{
				if (skillBase.skillResultID[i] > 0)
				{
					SkillResult skillResult = DataManager.getModule<DataSkill>(DATA_MODULE.Data_Skill).getSkillResultDataBySkillID(skillBase.skillResultID[i]);
					//skillResultList.Add(skillResult);
					if (skillResult != null)
					{
						skillResult.skillType  = skillBase.skillType;
						skillResult.skillAdvance = 0;

						if (i == 0 && skillBase.skillAdvance.Count > 0)
							skillResult.skillAdvance = skillBase.skillAdvance[i];
						else
						if (i == 1 && skillBase.skillAdvance.Count > 1)
							skillResult.skillAdvance = skillBase.skillAdvance[i];

						skillResultList.Add(skillResult);

						if ((skillResult.skillType == 2) && (isFirstSkillResult == false))
						{
							//BB的最大值，取决于英雄技能所需的BB值大小。
							currHeroAttr.maxEnergy = skillBase.powerReq;

							//设置主动技能动作类型,(伤害,增益,减益)
							isFirstSkillResult = true;
							initiativeSkill(skillResult);
						}
					}
				}
			}
		}


		/*for (int i = 0; i < skillResultList.Count; i++)
		{
			if (skillResultList[i].skillType == 0)
			{
				initiativeSkill(); //主动
			}
			else
			if (skillResultList[i].skillType == 1)
			{
				//passitiySkill();  //被动
				if (checkTriggerCondition(skillResultList[i]))
				{
					getTargetHero(skillResultList[i]);
					effectformula(skillResultList[i]);
				}

			}
		}*/

	}



	public void initiativeSkill(SkillResult skillResult)
	{

		AttackAttribute attackAttr = null;
		if (attackAttributes.Count > 1)		
			attackAttr = attackAttributes[1];		
				
		if (attackAttr != null)
		{
			attackAttr.attackType = (AttackType)skillResult.effecttype;
			attackAttr.impactNum = skillResult.targetnum;
			attackAttr.targetType = (_ElementType)skillResult.tdata1;
		}



		/*for (int i = 0; i < skillResultList.Count; i++)
		{
			//Debug.Log("...................................initiativeSkill..................................." + currHero.name);
			passivitySkill();
		}*/
	}

	public void passivitySkill()
	{
		//HeroAttack heroAttack = transform.gameObject.GetComponent<HeroAttack>();
		//List<AttackAttribute> attackAttributes = heroAttack.GetAttackAttributes();
		AttackAttribute attackAttr = null;
		if (attackAttributes.Count > 1)
		{
			attackAttr = attackAttributes[1];		
		}

		if (attackAttr != null)
		{
			//attackAttr.impactNum
				//attackAttr.AttackType = 
			for (int i = 0; i < skillResultList.Count; i++)
			{
				attackAttr.attackType = (AttackType)skillResultList[i].effecttype;
				attackAttr.impactNum = skillResultList[i].targetnum;
				attackAttr.targetType = (_ElementType)skillResultList[i].tdata1;
			}
		}

	}

	public void Addbuff(SkillResult skillResult)
	{
		/*if (skillResult.effecttype == 1)
		{
			//skillResult.scdata1
		}*/
	}

	public bool checkTriggerCondition(SkillResult skillResult)
	{
		bool isOK = false;
		BattleState state = BattleController.SingleTon().battleState;

		//if (skillResult.specialcondition == (int)SkillCondition.noCondition && state == BattleState.BattleBegin)
		if (skillResult.specialcondition == (int)SkillCondition.noCondition)
		{
			isOK = true;
		}
		else
			if (skillResult.specialcondition == (int)SkillCondition.RoundCount && skillResult.scdata1 <= currRoundIndex)
		{
			isOK = true;
		}
		else
			if (skillResult.specialcondition == (int)SkillCondition.RoundStart && state == BattleState.RoundStart)
		{
			isOK = true;
		}
		else
			if (skillResult.specialcondition == (int)SkillCondition.RoundEnd && state == BattleState.RoundEnd)
			//if (skillResult.specialcondition == (int)SkillCondition.RoundEnd && BattleController.SingleTon().Turn == _AttackTurn.PlayerToEnmey)
		{
			//Debug.Log(".....................................................PlayerToEnmey....................");
			isOK = true;
		}
		else
			if (skillResult.specialcondition == (int)SkillCondition.IsCombo && isCombo)
		{
			isOK = true;
		}
		else
			if (skillResult.specialcondition == (int)SkillCondition.IsCritical && isCritical)
		{
			isOK = true;
		}
		else
			if (skillResult.specialcondition == (int)SkillCondition.HeroType && BattleController.SingleTon().getHeroTypeCount() >= skillResult.scdata1)
		{
			isOK = true;
		}
		else
			if (skillResult.specialcondition == (int)SkillCondition.Attacking)
		{
			if (Random.Range(0,100) <= skillResult.scdata1)
				isOK = true;
		}


		//技能效果持续回合
		/*if (skillResult.delaytime > 0)
		{
			isOK = true;
			skillResult.delaytime -= 1;
		}
		else
			isOK = false;*/

		return isOK;
	}

	public void getTargetHero(SkillResult skillResult)
	{
		targetHeroList.Clear();
		selfHeroList.Clear();
		if ((TargetHero)skillResult.target == TargetHero.enemy) 
		{
			if (skillResult.effecttype == AttackType.Damage)
			{
				if (currHero.Side == _Side.Enemy)
					targetHeroList.AddRange(BattleController.SingleTon().LeftHeroes);
				else
					targetHeroList.AddRange(BattleController.SingleTon().RightHeroes);
				/*if (currHero.Side == _Side.Enemy)
					targetHeroList.AddRange(BattleController.SingleTon().RightHeroes);
				else
					targetHeroList.AddRange(BattleController.SingleTon().LeftHeroes);*/
			}
			else
			{
				if (currHero.Side == _Side.Enemy)
					targetHeroList.AddRange(BattleController.SingleTon().RightHeroes);
				else
					targetHeroList.AddRange(BattleController.SingleTon().LeftHeroes);
			}
			//selfHeroList.AddRange(BattleController.SingleTon().RightHeroes);
		}
		else
		if ((TargetHero)skillResult.target == TargetHero.friend) 
		{
			if (skillResult.tdata1 == 0)
			{
				if (currHero.Side == _Side.Enemy)
					targetHeroList.AddRange(BattleController.SingleTon().LeftHeroes);
				else
					targetHeroList.AddRange(BattleController.SingleTon().RightHeroes);
				//selfHeroList.AddRange(BattleController.SingleTon().LeftHeroes);
			}
			else
			{
				if (currHero.Side == _Side.Enemy)
				{
					foreach(Hero hero in BattleController.SingleTon().LeftHeroes)
					{
						if ((_ElementType)skillResult.tdata1 == hero.heroAttribute.elementType)
						{
							targetHeroList.Add(hero);
						}
					}	
				}
				else
				{
					foreach(Hero hero in BattleController.SingleTon().RightHeroes)
					{
						if ((_ElementType)skillResult.tdata1 == hero.heroAttribute.elementType)
						{
							targetHeroList.Add(hero);
						}
					}
				}

			}
		}

		if (skillResult.targetnum  != 0 && skillResult.targetnum < targetHeroList.Count)
		{
			int count = targetHeroList.Count - skillResult.targetnum;
			targetHeroList.RemoveRange(skillResult.targetnum,count);
		}

	}

	public void effectObject(SkillResult skillResult)
	{
		//targetHeroList


	}

	public void getAdvanceRatio(SkillResult skillResult)
	{
		advanceRatio = currHeroAttr.skillLevel * (float)skillResult.skillAdvance / 100;
		//advanceRatio = currHeroAttr.skillLevel * (float)skillBase.skillAdvance[i] / 100;

	}

	public void effectformula(SkillResult skillResult)
	{
		if (skillResult.effectvaluetype == 0)
		{
			foreach (Hero hero in targetHeroList)
			{
				if (skillResult.effectobject == 19)
				{
					//麻痹效果
					hero.skillMgr.setAnnormal(AnnormalType.palsy,skillResult.delaytime);
				}
				else
				if (skillResult.effectobject == 20)
				{
					//诅咒效果
					hero.skillMgr.setAnnormal(AnnormalType.curse,skillResult.delaytime);
				}
				else
				if (skillResult.effectobject == 21)
				{
					if (hero.heroAttribute.skillElementType.IndexOf(_ElementType.Water) == -1) 
					{
						hero.heroAttribute.skillElementType.Add(_ElementType.Water);
						hero.skillMgr.addBuffs(BuffType.Water,skillResult.delaytime);
					}
				}
				else
				if (skillResult.effectobject == 22)
				{
					if (hero.heroAttribute.skillElementType.IndexOf(_ElementType.Fire) == -1) 
					{
						hero.heroAttribute.skillElementType.Add(_ElementType.Fire);
						hero.skillMgr.addBuffs(BuffType.Fire,skillResult.delaytime);
					}
				}
				else
				if (skillResult.effectobject == 23)
				{
					if (hero.heroAttribute.skillElementType.IndexOf(_ElementType.Wood) == -1) 
					{
						hero.heroAttribute.skillElementType.Add(_ElementType.Wood);
						hero.skillMgr.addBuffs(BuffType.Wood,skillResult.delaytime);
					}
				}
				else
				if (skillResult.effectobject == 24)
				{
					if (hero.heroAttribute.skillElementType.IndexOf(_ElementType.Wind) == -1) 
					{
						hero.heroAttribute.skillElementType.Add(_ElementType.Wind);
						hero.skillMgr.addBuffs(BuffType.Wind,skillResult.delaytime);
					}
				}
				else
				if (skillResult.effectobject == 25)
				{
					if (hero.heroAttribute.skillElementType.IndexOf(_ElementType.Holy) == -1) 
					{
						hero.heroAttribute.skillElementType.Add(_ElementType.Holy);
						hero.skillMgr.addBuffs(BuffType.Holy,skillResult.delaytime);
					}
				}
				else
				if (skillResult.effectobject == 26)
				{
					if (hero.heroAttribute.skillElementType.IndexOf(_ElementType.Evil) == -1) 
					{
						hero.heroAttribute.skillElementType.Add(_ElementType.Evil);
						hero.skillMgr.addBuffs(BuffType.Evil,skillResult.delaytime);
					}
				}
			}
		
			return;
		}
		else
		if (skillResult.effectvaluetype == 1)
		{
			//skillResult.effectvalue1 * ()

			if (skillResult.effectobject == 2)
			{
				//foreach (Hero hero in targetHeroList)
				//{
					//Debug.Log("hero.heroAttribute.currentDamage..............before................." + hero.heroAttribute.currentDamage);
					//hero.heroAttribute.currentHP = hero.heroAttribute.currentHP +  
						//Mathf.RoundToInt(skillResult.effectvalue1 * (currHeroAttr.currentDamage - hero.heroAttribute.currentDefence / 3));
					//Debug.Log("currHeroAttr.baseDamage....................................." + currHeroAttr.baseDamage);
					//hero.heroAttribute.currentDamage = hero.heroAttribute.currentDamage +  
						//Mathf.RoundToInt(skillResult.effectvalue1 * (currHeroAttr.baseDamage - hero.heroAttribute.baseDefence / 3));

					//hero.heroAttribute.currentSkillDamageRatio = hero.heroAttribute.currentSkillDamageRatio + hero.heroAttribute.currentSkillDamageRatio * skillResult.effectvalue1 / 100;
					currHero.heroAttribute.currentSkillDamageRatio = (float)skillResult.effectvalue1 / 100;
					Debug.Log("hero.heroAttribute.currentSkillDamageRatio......................................" + currHero.heroAttribute.currentSkillDamageRatio);

					if (attackAttributes.Count > 1)
					{
						attackAttributes[1].impactNum = skillResult.targetnum;
						attackAttributes[1].targetType = (_ElementType)skillResult.tdata1;
					}
													

					currHero.skillMgr.addBuffs(BuffType.currentSkillDamageRatio,skillResult.delaytime);

					//Debug.Log("hero.heroAttribute.currentDamage..............after................." + hero.heroAttribute.currentDamage);
				//}
			}
		}
		else
		if (skillResult.effectvaluetype == 2)			
		{
			if (skillResult.effectobject == 2)
			{
				foreach (Hero hero in targetHeroList)
				{
					//浮动系数
					float floatRatio = Random.Range(1,1);
					//群体直接治疗
					//Debug.Log("hero.heroAttribute.currentHP..............before................." + hero.heroAttribute.currentHP);

					int value = Mathf.FloorToInt(skillResult.effectvalue1 * (currHeroAttr.currentRecover + hero.heroAttribute.currentRecover + skillResult.effectvalue2)* floatRatio / 100);
					if (skillResult.delaytime == 1)
					{
						hero.heroAttribute.AddHP = value;
						hero.heroAttribute.currentHP = Mathf.FloorToInt(hero.heroAttribute.currentHP + value);
						hero.Btn.RequireUpdate = true;
					}
					else
						hero.skillMgr.addBuffs(BuffType.groupRecover,skillResult.delaytime,value);
					//Debug.Log("hero.heroAttribute.currentHP..............after................." + hero.heroAttribute.currentHP);
				}
			}
		}
		else
			if (skillResult.effectvaluetype == 3)			
		{
			foreach (Hero hero in targetHeroList)
			{
				if (skillResult.effectobject == 1)
					hero.heroAttribute.maxHP = Mathf.RoundToInt(skillResult.effectvalue1);
				else
				if (skillResult.effectobject == 2)
					hero.heroAttribute.currentHP = Mathf.RoundToInt(skillResult.effectvalue1);
				else
				if (skillResult.effectobject == 3)
					//hero.heroAttribute.currentDamage = Mathf.RoundToInt(skillResult.effectvalue1);
					hero.heroAttribute.currentSkillDamageRatio = Mathf.RoundToInt(skillResult.effectvalue1);
				else
				if (skillResult.effectobject == 4)
				{
					hero.heroAttribute.currentDefence = Mathf.RoundToInt(skillResult.effectvalue1);
				}
				else
				if (skillResult.effectobject == 5)
					hero.heroAttribute.currentRecover = Mathf.RoundToInt(skillResult.effectvalue1);
				else
				if (skillResult.effectobject == 6)
					hero.heroAttribute.currentEnergy = Mathf.RoundToInt(skillResult.effectvalue1);
				else
				if (skillResult.effectobject == 7)
					hero.heroAttribute.currentCritOdds = Mathf.RoundToInt(skillResult.effectvalue1);
				else
				if (skillResult.effectobject == 11)
				{
					//解除异常状态
					hero.heroAttribute.isImmuneAbnormal = true;
					if (annormalStateList != null && annormalStateList.Count > 0)
						annormalStateList.RemoveRange(0,annormalStateList.Count);
				}
			}
		}
		else
			if (skillResult.effectvaluetype == 4)	
		{
			foreach (Hero hero in targetHeroList)
			{
				if (skillResult.effectobject == 1)
					{
						hero.heroAttribute.currentHP = Mathf.RoundToInt(hero.heroAttribute.currentHP + hero.heroAttribute.currentHP * skillResult.effectvalue1 / 100);
						//hero.heroAttribute.calculateHP = Mathf.RoundToInt(hero.heroAttribute.calculateHP + hero.heroAttribute.calculateHP * skillResult.effectvalue1 / 100);
						hero.heroAttribute.maxHP = Mathf.RoundToInt(hero.heroAttribute.maxHP + hero.heroAttribute.maxHP * skillResult.effectvalue1 / 100);
						//hero.skillMgr.addBuffs(BuffType.maxHP,skillResult.delaytime);
						hero.Btn.RequireUpdate = true;
					}
				else
					if (skillResult.effectobject == 2)
					{
						if (skillResult.effecttype == AttackType.Health)
						{
							hero.heroAttribute.currentHP = Mathf.RoundToInt(hero.heroAttribute.currentHP + hero.heroAttribute.currentHP * skillResult.effectvalue1 / 100);
							//hero.heroAttribute.calculateHP = Mathf.RoundToInt(hero.heroAttribute.calculateHP + hero.heroAttribute.calculateHP * skillResult.effectvalue1 / 100);
							//hero.heroAttribute.maxHP = Mathf.RoundToInt(hero.heroAttribute.maxHP + hero.heroAttribute.maxHP * skillResult.effectvalue1 / 100);
							//hero.skillMgr.addBuffs(BuffType.currentHP,skillResult.delaytime);
							hero.Btn.RequireUpdate = true;
						}
						else
						if (skillResult.effecttype == AttackType.Reduce)
						{
							//中毒效果
							hero.skillMgr.setAnnormal(AnnormalType.poison,skillResult.delaytime,skillResult.effectvalue1 / 100);
						}
					}
				else
					if (skillResult.effectobject == 3)
					{


						if (skillResult.effecttype == AttackType.Damage)
						{
							//提升攻击力
							//Debug.Log("name................." + hero.name + "............baseDamage.........." + hero.heroAttribute.baseDamage);
							//baseDamage: 攻击, currentDamage:攻击
							hero.heroAttribute.currentDamage = Mathf.FloorToInt(hero.heroAttribute.currentDamage + hero.heroAttribute.baseDamage * skillResult.effectvalue1 / 100);
							//hero.heroAttribute.baseDamage = Mathf.RoundToInt(hero.heroAttribute.baseDamage + hero.heroAttribute.baseDamage * skillResult.effectvalue1 / 100);
							
							hero.skillMgr.addBuffs(BuffType.currentDamage,skillResult.delaytime);

							addBuffIcon(hero);


							Debug.Log("name................." + hero.name + "............currentDamage.........." + hero.heroAttribute.currentDamage);
						}
						else
						if (skillResult.effecttype == AttackType.Reduce)
						{
							//负伤效果
							hero.heroAttribute.currentDamage = Mathf.FloorToInt(hero.heroAttribute.currentDamage + hero.heroAttribute.currentDamage * skillResult.effectvalue1 / 100);
							hero.heroAttribute.currentDamage = Mathf.FloorToInt(hero.heroAttribute.currentDamage  + hero.heroAttribute.currentDamage * advanceRatio);
							hero.skillMgr.setAnnormal(AnnormalType.hurt,skillResult.delaytime);
							//Debug.Log(".............................hurt............................................");
						}
					}

				else
					if (skillResult.effectobject == 4)
					{
						if (skillResult.effecttype == AttackType.Health)
						{
							hero.heroAttribute.currentDefence = Mathf.RoundToInt(hero.heroAttribute.currentDefence + hero.heroAttribute.currentDefence * skillResult.effectvalue1 / 100);
						}
						else
						if (skillResult.effecttype == AttackType.Reduce)
						{					

					        //虚弱效果
							hero.heroAttribute.currentDefence = Mathf.RoundToInt(hero.heroAttribute.currentDefence + hero.heroAttribute.currentDefence * skillResult.effectvalue1 / 100);
							hero.skillMgr.setAnnormal(AnnormalType.weak,skillResult.delaytime);
							//Debug.Log("hero.heroAttribute.currentDefence............................");
						}
					}
				else
					if (skillResult.effectobject == 5)
					{
						if (skillResult.effecttype == AttackType.Health)
						{
							hero.heroAttribute.currentRecover = Mathf.RoundToInt(hero.heroAttribute.currentRecover + hero.heroAttribute.baseRecover * skillResult.effectvalue1 / 100);
						}
						else
						if (skillResult.effecttype == AttackType.Reduce)
						{
						    //疾病效果
							hero.heroAttribute.currentRecover = Mathf.RoundToInt(hero.heroAttribute.currentRecover + hero.heroAttribute.baseRecover * skillResult.effectvalue1 / 100);
							hero.skillMgr.setAnnormal(AnnormalType.sick,skillResult.delaytime);
							Debug.Log("hero.heroAttribute.currentRecover........................sick....");
						}
					}
				else
					if (skillResult.effectobject == 6)
						hero.heroAttribute.currentEnergy = Mathf.RoundToInt(hero.heroAttribute.currentEnergy + hero.heroAttribute.currentEnergy * skillResult.effectvalue1 / 100);
				else
					if (skillResult.effectobject == 7)
					{
						//提升暴击率
						hero.heroAttribute.currentCritOdds = Mathf.FloorToInt(hero.heroAttribute.currentCritOdds + hero.heroAttribute.currentCritOdds * skillResult.effectvalue1 / 100); 
						hero.heroAttribute.currentCritOdds = Mathf.FloorToInt(hero.heroAttribute.currentCritOdds + hero.heroAttribute.currentCritOdds * advanceRatio);
						hero.skillMgr.addBuffs(BuffType.promoteCritical,skillResult.delaytime);
					}
				else
					if (skillResult.effectobject == 8)
					{
						//暴击加成
						hero.heroAttribute.additionCritical = hero.heroAttribute.additionCritical + hero.heroAttribute.additionCritical * skillResult.effectvalue1 / 100;
						hero.skillMgr.addBuffs(BuffType.additionCritical,skillResult.delaytime);
					}
				else
					if (skillResult.effectobject == 9)
					{
						//合击加成
						hero.heroAttribute.additionCombo = hero.heroAttribute.additionCombo +  skillResult.effectvalue1 / 100;
						hero.skillMgr.addBuffs(BuffType.combo,skillResult.delaytime);

						Debug.Log("......................additionCombo..................................");

					}
				else 
					if (skillResult.effectobject == 10)
					{
						//降低伤害
						hero.heroAttribute.decreaseDamage = hero.heroAttribute.decreaseDamage + hero.heroAttribute.decreaseDamage * skillResult.effectvalue1 / 100;
						hero.skillMgr.addBuffs(BuffType.decreaseDamage,skillResult.delaytime);
						//Debug.Log("hero.heroAttribute.decreaseDamage.............................." + hero.heroAttribute.decreaseDamage);
					}
				else
					if (skillResult.effectobject == 11)
					{
						//免疫所有异常
						hero.heroAttribute.isImmuneAbnormal = true;
						hero.skillMgr.addBuffs(BuffType.isImmuneAbnormal,skillResult.delaytime);
					}
				else
					if (skillResult.effectobject == 13)
					{
						//BB水晶掉落加成
						hero.heroAttribute.additionBBDrop = hero.heroAttribute.additionBBDrop + hero.heroAttribute.additionBBDrop * skillResult.effectvalue1 / 100;
						hero.skillMgr.addBuffs(BuffType.additionBBDrop,skillResult.delaytime);
					}
				else
					if (skillResult.effectobject == 16)
					{
						//心水晶回复加成
						hero.heroAttribute.additionHCRevert = hero.heroAttribute.additionHCRevert + hero.heroAttribute.additionHCRevert * skillResult.effectvalue1 / 100;
						hero.skillMgr.addBuffs(BuffType.additionHCRevert,skillResult.delaytime);
					}
				
									
			}
		}
		else
			if (skillResult.effectvaluetype == 5)	
		{
			foreach (Hero hero in targetHeroList)
			{
				if (skillResult.effectobject == 1)
					hero.heroAttribute.maxHP = Mathf.FloorToInt(hero.heroAttribute.maxHP + skillResult.effectvalue1);
				else
					if (skillResult.effectobject == 2)
						hero.heroAttribute.currentHP = Mathf.FloorToInt(hero.heroAttribute.currentHP + skillResult.effectvalue1);
				else
					if (skillResult.effectobject == 3)
						hero.heroAttribute.currentDamage = Mathf.FloorToInt(hero.heroAttribute.currentDamage + skillResult.effectvalue1);
				else
					if (skillResult.effectobject == 4)
						hero.heroAttribute.currentDefence = Mathf.FloorToInt(hero.heroAttribute.currentDefence + skillResult.effectvalue1);
				else
					if (skillResult.effectobject == 5)
						hero.heroAttribute.currentRecover = Mathf.FloorToInt(hero.heroAttribute.currentRecover + skillResult.effectvalue1);
				else
					if (skillResult.effectobject == 6)
				{
					//能量回复
					/*Debug.Log("name................." + hero.name + "............energy.........." + hero.heroAttribute.currentEnergy);
					int energy = Mathf.FloorToInt(hero.heroAttribute.currentEnergy + skillResult.effectvalue1);
					hero.heroAttribute.currentEnergy =  energy > hero.heroAttribute.maxEnergy? hero.heroAttribute.maxEnergy : energy;
					hero.Btn.RequireUpdate = true;
					Debug.Log("name................." + hero.name + "............currentEnergy.........." + hero.heroAttribute.currentEnergy);*/

					hero.heroAttribute.additionEnergy = skillResult.effectvalue1;
					hero.skillMgr.addBuffs(BuffType.additionEnergy,skillResult.delaytime);


				}
				else
					if (skillResult.effectobject == 7)
						hero.heroAttribute.currentCritOdds = Mathf.RoundToInt(hero.heroAttribute.currentCritOdds + skillResult.effectvalue1);
			}
		}
	}

	



	/*public List<Hero> getTarget(SkillResult skillResult)
	{

		if (skillResult.target == 1)
		{
			target =  BattleController.SingleTon().LeftHeroes;
		}
		else
		if (skillResult.target == 2)
		{
			if (skillResult.tdata1 == 0)	
			{
				//BattleController.SingleTon().RightHeroes;
			}
			else
			{

			}
		}
	}*/

	//被动
	public void doPassivitySkill()
	{
		//if (currHeroAttr != null)
		//	currHeroAttr.ResetBuffAttributes();


		if (skillResultList != null)
		{
			for (int i = 0; i < skillResultList.Count; i++)
			{
				if (skillResultList[i].skillType == 1 && currHero.isCaptian)
				{

					//passitiySkill();  //被动
					if (checkTriggerCondition(skillResultList[i]))
					{
						getAdvanceRatio(skillResultList[i]);
						getTargetHero(skillResultList[i]);
						effectformula(skillResultList[i]);
					}					
				}
				/*else
				if (skillResultList[i].skillType == 2)
				{
					initiativeSkill(); //主动
				}*/
			}
		}

		doAnnormal();
		decBuffs();
	}

	public void doBuffs()
	{
		doAnnormal();
		decBuffs();
	}


	//主动
	public void doInitiativeSkill(AttackAttribute aa)
	{
		if (skillResultList != null)
		{
			for (int i = 0; i < skillResultList.Count; i++)
			{
				if (skillResultList[i].skillType == 2)
				{
					if (checkTriggerCondition(skillResultList[i]))
					{
						//targetHeroList.Clear();
						//targetHeroList.AddRange(aa.attackTargets);	
						getAdvanceRatio(skillResultList[i]);

						getTargetHero(skillResultList[i]);
						effectformula(skillResultList[i]);
					}	
				}
			}
		}
	}


	/*public enum AnnormalType
	{
		poison = 1,
		sick = 2,
		weak = 3,
		hurt = 4,
		palsy = 5,
		curse = 6
	}*/

	public void setAnnormal(AnnormalType annormalType, int roundCount,float value = 0)
	{
		if (annormalStateList == null)
			annormalStateList = new List<AnnormalState>();

		for (int i = 0; i < annormalStateList.Count; i ++)
		{
			if (annormalStateList[i].type == annormalType)
			{
				annormalStateList[i].roundcount = roundCount;
				return;
			}
		}

		AnnormalState annState = new AnnormalState();
		annState.type = annormalType;
		annState.roundcount = roundCount;
		annState.value = value;

		annormalStateList.Add(annState);
	}

	public void addBuffs(BuffType buffType, int roundCount, float value = 0)
	{
		if (buffStateList == null)
			buffStateList = new List<BuffState>();

		for (int i = 0; i < buffStateList.Count; i ++)
		{
			if (buffStateList[i].buffType == buffType)
			{
				buffStateList[i].roundcount = roundCount;
				return;
			}
		}
		
		BuffState buffState = new BuffState();
		buffState.buffType = buffType;
		buffState.roundcount = roundCount;
		buffState.value = value;
		//buffState.selfHero = selfHero;
		//buffState.targetHero.AddRange(targetHero);
		
		buffStateList.Add(buffState);
	}

	public void decBuffs()
	{
		if (buffStateList != null)
		{
			//foreach(BuffState buffState in buffStateList)
			for (int i = buffStateList.Count -1; i >= 0 ; i--)
			{
				if (buffStateList[i].buffType == BuffType.promoteCritical)
				{
					//Debug.Log("name................." + currHero.name + "............roundcount.........." + buffStateList[i].roundcount);
					buffStateList[i].roundcount -= 1;
					if (buffStateList[i].roundcount <= 0)
					{
						//Debug.Log("name................." + currHero.name + "............currentCritical.........." + currHero.heroAttribute.currentCritical);
						currHero.heroAttribute.currentCritOdds = currHero.heroAttribute.baseCritical;
						//Debug.Log("name................." + currHero.name + "............currentCritical.........." + currHero.heroAttribute.currentCritical);
						buffStateList.Remove(buffStateList[i]);
					}
				}
				else									
				if (buffStateList[i].buffType == BuffType.currentDamage)
				{


					buffStateList[i].roundcount -= 1;
					if (buffStateList[i].roundcount <= 0)
					{
						currHero.heroAttribute.currentDamage = currHero.heroAttribute.baseDamage;
						buffStateList.Remove(buffStateList[i]);
						GameObject buffPoint = currHero.transform.FindChild("BuffPoint").gameObject;
						Buff buffIcon = buffPoint.GetComponent<Buff>();
						buffIcon.OnTurnFinish();	
					}
					
				}
				else
				if (buffStateList[i].buffType == BuffType.currentSkillDamageRatio)
				{
					buffStateList[i].roundcount -= 1;
					if (buffStateList[i].roundcount <= 0)
					{
						currHero.heroAttribute.currentSkillDamageRatio = currHero.heroAttribute.baseSkillDamageRatio;
						buffStateList.Remove(buffStateList[i]);
					}								
				}

				else
				if (buffStateList[i].buffType == BuffType.combo)
				{
					buffStateList[i].roundcount -= 1;
					if (buffStateList[i].roundcount <= 0)
					{
						currHero.heroAttribute.additionCombo = 0;
						buffStateList.Remove(buffStateList[i]);
					}
				}
				else
				if (buffStateList[i].buffType == BuffType.additionCritical)
				{	
					buffStateList[i].roundcount -= 1;
					if (buffStateList[i].roundcount <= 0)
					{
						currHero.heroAttribute.additionCritical = 1;
						buffStateList.Remove(buffStateList[i]);
					}
				}	
				else
				if (buffStateList[i].buffType == BuffType.additionHCRevert)
				{
					buffStateList[i].roundcount -= 1;
					if (buffStateList[i].roundcount <= 0)
					{
						currHero.heroAttribute.additionHCRevert = 1;
						buffStateList.Remove(buffStateList[i]);
					}
				}
				else
				if (buffStateList[i].buffType == BuffType.additionBBDrop)
				{
					buffStateList[i].roundcount -= 1;
					if (buffStateList[i].roundcount <= 0)
					{
						currHero.heroAttribute.additionBBDrop = 1;
						buffStateList.Remove(buffStateList[i]);
					}
	
				}
				else
				if (buffStateList[i].buffType == BuffType.additionEnergy && BattleController.SingleTon().battleState != BattleState.BattleBegin)
				{


					currHero.heroAttribute.currentEnergy += Mathf.FloorToInt(currHero.heroAttribute.additionEnergy);
					currHero.Btn.RequireUpdate = true;
					buffStateList[i].roundcount -= 1;
					if (buffStateList[i].roundcount <= 0)
					{
						currHero.heroAttribute.additionEnergy = 0;
						buffStateList.Remove(buffStateList[i]);
					}									
				}
				else
				if (buffStateList[i].buffType == BuffType.isImmuneAbnormal)
				{
					buffStateList[i].roundcount -= 1;
					if (buffStateList[i].roundcount <= 0)
					{
						currHero.heroAttribute.isImmuneAbnormal = false;
						buffStateList.Remove(buffStateList[i]);
					}
				}
				else				
				if (buffStateList[i].buffType == BuffType.decreaseDamage)
				{
					buffStateList[i].roundcount -= 1;
					if (buffStateList[i].roundcount <= 0)
					{
						currHero.heroAttribute.decreaseDamage = 1;
						buffStateList.Remove(buffStateList[i]);
					}
				}
				else				
				if (buffStateList[i].buffType == BuffType.Water)
				{
					buffStateList[i].roundcount -= 1;
					if (buffStateList[i].roundcount <= 0)
					{
						currHero.heroAttribute.skillElementType.Remove(_ElementType.Water);
						buffStateList.Remove(buffStateList[i]);
					}
				}
				else				
				if (buffStateList[i].buffType == BuffType.Fire)
				{
					buffStateList[i].roundcount -= 1;
					if (buffStateList[i].roundcount <= 0)
					{
						currHero.heroAttribute.skillElementType.Remove(_ElementType.Fire);
						buffStateList.Remove(buffStateList[i]);
					}
				}
				else				
				if (buffStateList[i].buffType == BuffType.Wood)
				{
					buffStateList[i].roundcount -= 1;
					if (buffStateList[i].roundcount <= 0)
					{
						currHero.heroAttribute.skillElementType.Remove(_ElementType.Wood);
						buffStateList.Remove(buffStateList[i]);
						Debug.Log("................................BuffType.Wood.............................");
					}
				}
				else				
				if (buffStateList[i].buffType == BuffType.Wind)
				{
					buffStateList[i].roundcount -= 1;
					if (buffStateList[i].roundcount <= 0)
					{
						currHero.heroAttribute.skillElementType.Remove(_ElementType.Wind);
						buffStateList.Remove(buffStateList[i]);
					}
				}
				else				
				if (buffStateList[i].buffType == BuffType.Holy)
				{
					buffStateList[i].roundcount -= 1;
					if (buffStateList[i].roundcount <= 0)
					{
						currHero.heroAttribute.skillElementType.Remove(_ElementType.Holy);
						buffStateList.Remove(buffStateList[i]);
					}
				}
				else				
				if (buffStateList[i].buffType == BuffType.Evil)
				{
					buffStateList[i].roundcount -= 1;
					if (buffStateList[i].roundcount <= 0)
					{
						currHero.heroAttribute.skillElementType.Remove(_ElementType.Evil);
						buffStateList.Remove(buffStateList[i]);
					}
				}
				else				
				if (buffStateList[i].buffType == BuffType.groupRecover)
				{
					if (buffStateList[i].roundcount > 0)
					{

						//群体直接治疗
						//Debug.Log("hero.heroAttribute.currentHP.........groupRecover.....before................." + currHero.heroAttribute.currentHP);
						currHero.heroAttribute.currentHP = Mathf.FloorToInt(currHero.heroAttribute.currentHP +  buffStateList[i].value);
						int value = Mathf.FloorToInt(buffStateList[i].value);
						currHero.Btn.RequireUpdate = true;
						//Debug.Log("hero.heroAttribute.currentHP.........groupRecover.....alter................." + currHero.heroAttribute.currentHP);
						buffStateList[i].roundcount -= 1;

						BattleUtility.ShowEnhaceEffect(currHero,value);
					}
					else
					{
						buffStateList.Remove(buffStateList[i]);
					}

				}

			}
		}
	}


	public void doAnnormal()
	{
		if (annormalStateList != null && currHeroAttr.isImmuneAbnormal == false)
		//if (annormalStateList != null)
		{
			foreach (AnnormalState annState in annormalStateList)
			{

				if (annState.type == AnnormalType.poison)
				{
					if (annState.roundcount > 0)
					{
						currHeroAttr.currentHP = Mathf.FloorToInt(currHeroAttr.currentHP + currHeroAttr.currentHP * annState.value);
						annState.roundcount -= 1;
					}
					//else
					//	annormalStateList.Remove(annState);

				}
				else
				if (annState.type == AnnormalType.sick)
				{
					annState.roundcount -= 1;
					if (annState.roundcount == 0)
					{
						currHeroAttr.currentRecover = currHeroAttr.baseRecover;
					}
				}
				else
				if (annState.type == AnnormalType.weak)
				{
					annState.roundcount -= 1;
					if (annState.roundcount == 0)
					{
						currHeroAttr.currentDefence = currHeroAttr.baseDefence;
						//annormalStateList.Remove(annState);
					}
				}
				else
				if (annState.type == AnnormalType.hurt)
				{
					annState.roundcount -= 1;
					if (annState.roundcount == 0)
					{
						currHeroAttr.currentDamage = currHeroAttr.baseDamage;
					}

				}
				else
				if (annState.type == AnnormalType.palsy)
				{
					annState.roundcount -= 1;
					if (annState.roundcount <= 0)
					{
						/*if(heroAttack.onFinish!=null)
							heroAttack.onFinish();
						currHero.Status = _HeroStatus.AfterTurn;*/

						//BattleController.SingleTon().Turn = _AttackTurn.EnemyToPlayer;
						//currHero.Status = _HeroStatus.AfterTurn;

						//Debug.Log("annState.roundcount......................................." + annState.roundcount);


						/*if (BattleController.SingleTon().LeftHeroes.Count > 0)
						{
							int index = BattleController.SingleTon().LeftHeroes.IndexOf(currHero);
							if (index >= 0)
							{
								BattleController.SingleTon().LeftHeroes[index].Status = _HeroStatus.AfterTurn;
							}
						}*/




						//currHero.Status = _HeroStatus.AfterTurn;
						annState.type = AnnormalType.none;
						Debug.Log("currHero.Status..................................." + currHero.name);					
					}
					//else
					//	annormalStateList.Remove(annState);
				}
				else
				if (annState.type == AnnormalType.curse)
				{
					if (annState.roundcount > 0)
					{
						currHeroAttr.isCurse = true;
						annState.roundcount -= 1;
						Debug.Log("currHero.Status..................................." + currHeroAttr.isCurse);					
					}
					else
						currHeroAttr.isCurse = false;
				
				}
			}
		}
		//public List<AnnormalState> annormalStateList;
	}


	void addBuffIcon(Hero hero)
	{
		GameObject buffPoint = hero.transform.FindChild("BuffPoint").gameObject;
		if (buffPoint != null)
		{

			BoxCollider2D boxColl = hero.HeroBody.GetComponent<BoxCollider2D>();
			if (boxColl != null)
				buffPoint.transform.localPosition = new Vector3(buffPoint.transform.localPosition.x,boxColl.size.y + boxColl.size.y / 2.0f,buffPoint.transform.localPosition.z);
			Buff buffIcon = buffPoint.GetComponent<Buff>();
			buffIcon.AddSprite("battle_buff_icon_0");
			buffIcon.AddSprite("battle_buff_icon_1");
			buffIcon.AddSprite("battle_buff_icon_2");
			buffIcon.AddSprite("battle_buff_icon_3");
			buffIcon.AddSprite("battle_buff_icon_4");
		}
	}

	// Use this for initialization
	void Start ()
	{

		currHero = transform.GetComponent<Hero>();
		currHeroAttr = currHero.heroAttribute;
		
		heroAttack = transform.gameObject.GetComponent<HeroAttack>();

		/*Debug.Log("...............skillManager..............");
		skillResultList = new List<SkillResult>();
		targetHeroList = new List<Hero>();
		selfHeroList = new List<Hero>();

		currHero = transform.GetComponent<Hero>();
		currHeroAttr = currHero.heroAttribute;

		heroAttack = transform.gameObject.GetComponent<HeroAttack>();*/
	}
	
	// Update is called once per frame
	void Update () {

	}
}
