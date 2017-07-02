using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;

public class HeroAttribute : MonoBehaviour {

	Hero mHero;
	static public float commonHitInterval = 0.011f;
	static public float shakeRadius = 0.1f;


	public HeroInfo heroInfo;
	public int heroTypeId;
	public float speed;
	public int baseHP;
	public int maxHP = 3000;
	public int currentHP = 3000;
	public int calculateHP = 3000;
	public int maxEnergy;
	public int currentEnergy;
	public string heroName;
	public _ElementType elementType;
	public string spriteName;
	public int star;
	public int AddHP = 0;

	//public float baseATK;
	public float baseSkillDamageRatio;
	public float baseDamage;
	public float baseDefence;
	public float baseRecover;
	public int baseCritical;

	public float currentCritMulti = 0.5f;//当前暴击伤害加成倍数
	public float currentDefenceMulti = 0.5f;//当前防御值加成倍数
	public int currentDodgeOdds = 0;//当前闪避几率
	public int currentDefenceOdds = 5000;//当前防御几率
	public float currentAttackMulti = 0f;//当前伤害加成倍数
	public float currentRelationMulti = 0.2f;//属性相克加成

	public float currentSkillOddsAdd = 2000;//当前技能释放几率加成，基本释放几率是在技能上面


	public int skillCaptian;
	public int SkillBase;
	public int skillLevel;

	public float basePoison;
	public float baseWeak;
	public float baseSick;
	public float baseHurt;
	public float baseParalysis;
	public float baseCurse;

	public int coinCount;
	public int soulCount;
	public int coinPerDrop;
	public int soulPerDrop;

	public SkillManager skillMgr;

	public int dropChest = 0;//max is 10000

	public List<_ElementType> elementEnhances = new List<_ElementType>();
	public List<BuffAttribute> buffs = new List<BuffAttribute>();
	public List<AttackQueue> hitQueue = new List<AttackQueue>();
	public List<Hero> currentAttackers = new List<Hero>();
	public List<_ElementType> skillElementType = new List<_ElementType>();
	public List<AnnormalState> annormalStateList;

	public void Awake()
	{
		if(mHero==null)
			mHero=GetComponent<Hero>();

		annormalStateList = new List<AnnormalState>();
		if(skillMgr == null)
		{
			skillMgr = GetComponent<SkillManager>();
			skillMgr.annormalStateList = annormalStateList;
		}
	}

	float tmpDelay = 0;
	void Update()
	{
		tmpDelay += Time.deltaTime;
		if(tmpDelay>1)
		{
			if(mHero.heroRes.CurrentAm.clipName == Hero.ACTION_HIT)
			{
				mHero.heroAnimation.Play(Hero.ACTION_STANDBY);
			}
		}
#region used to debug
		if(isDebugMod)
		{
			currentDebugAttr = this;
			isDebugMod = false;
		}
#endregion
	}

#region used to debug
	public bool isDebugMod;
	public static HeroAttribute currentDebugAttr;
	void OnGUI()
	{
		if(currentDebugAttr == this)
		{
			if(GUI.Button(new Rect(10,10,80,30),"Reload ATk Que"))
			{
				List<AttackAttribute> aas = mHero.heroAttack.GetAttackAttributes(); 
				ConfigMgr.releaseConfig(CONFIG_MODULE.ATTACKPART);
                Debug.Log("Reload ATk Que");
				SpawnUtility.InitAttackQueues();
				foreach(AttackAttribute aa in aas)
				{
					aa.attackQueue = SpawnUtility.GetAttackQueue(heroTypeId);
				}
			}
			if(GUI.Button(new Rect(10,50,80,30),"Debug Battle"))
			{
				foreach(Hero hero in BattleController.SingleTon().LeftHeroes)
				{
					hero.heroAttribute.maxHP = 9999999;
					hero.heroAttribute.currentHP = 9999999;
				}
				foreach(Hero hero in BattleController.SingleTon().RightHeroes)
				{
					hero.heroAttribute.maxHP = 9999999;
					hero.heroAttribute.currentHP = 9999999;
					hero.Status = _HeroStatus.BeforeTurn;
					hero.Btn.ActiveButton();
				}
			}

		}
	}
#endregion

	public float currentDamage;
	public float currentDefence;
	public float currentRecover;
	public int currentCritOdds;
	public float additionEnergy; //能量回复加成
	public float additionCombo; //合击伤害增加百分比
	public float additionCritical; //暴击伤害增加百分比
	public float additionHCRevert;//心回复量增加百分比
	public float additionHCDrop;
	public float additionBBDrop; //BB掉落量增加百分比
	public float additionCoinDrop;
	public float additionSoulDrop;

	public float currentPoison;
	public float currentWeak;
	public float currentSick;
	public float currentHurt;
	public float currentParalysis;
	public float currentCurse;

	public float currentSkillDamageRatio;
	public bool isImmuneAbnormal;//免疫异常
	public float decreaseDamage;
	public bool isCurse;



	public void ResetBuffAttributes()
	{
		currentDamage = baseDamage;
		currentDefence = baseDefence;
		currentRecover = baseRecover;
		currentCritOdds = baseCritical;
		currentSkillDamageRatio = baseSkillDamageRatio;

		additionCombo = 0;
		additionHCDrop = 0;
		additionBBDrop = 1;
		additionHCRevert = 1;
		additionCoinDrop = 0;
		additionSoulDrop = 0;
		additionCritical = 1;

		currentPoison = 0;
		currentWeak = 0;
		currentSick = 0;
		currentHurt = 0;
		currentParalysis = 0;
		currentCurse = 0;
		isImmuneAbnormal = false;
		decreaseDamage = 1;
		isCurse = false;

	}


	static public float posionDamagePercent = 0.2f;
	public void RecalculateBuffs()
	{
		ResetBuffAttributes();
		foreach(BuffAttribute buffAttribute in buffs)
		{
			if(buffAttribute.attackType==AttackType.Enhance)
			{
				if(buffAttribute.subAttackType == SubAttackType.Attack)
				{
					currentDamage += baseDamage * buffAttribute.power;
				}
				else if(buffAttribute.subAttackType == SubAttackType.Defence)
				{
					currentDefence += baseDefence * buffAttribute.power;;
				}
				else if(buffAttribute.subAttackType == SubAttackType.Recover)
				{
					currentDefence += baseRecover * buffAttribute.power;
				}
				else if(buffAttribute.subAttackType == SubAttackType.Element)
				{
					if(!elementEnhances.Contains(elementType))
						elementEnhances.Remove(elementType);
				}
			}
			else if(buffAttribute.attackType== AttackType.Reduce)
			{
				if(buffAttribute.subAttackType == SubAttackType.Attack)
				{
					currentDamage -= baseDamage * buffAttribute.power;
				}
				else if(buffAttribute.subAttackType == SubAttackType.Defence)
				{
					currentDefence -= baseDefence * buffAttribute.power;;
				}
				else if(buffAttribute.subAttackType == SubAttackType.Recover)
				{
					currentDefence -= baseRecover * buffAttribute.power;
				}
			}
			else if(buffAttribute.attackType==AttackType.Health)
			{
				Vector3 hitPos = BattleUtility.GetHitPos(mHero);
				int healthPoint = BattleUtility.GetHealBuffPoint(mHero,buffAttribute);
				AdjustHeelth(healthPoint);
				BattleUtility.ShowDamageBeat(0.0f,healthPoint,hitPos,1);
			}
			else if(buffAttribute.attackType==AttackType.Damage)
			{
				if(buffAttribute.subAttackType==SubAttackType.Poison)
				{
					Vector3 hitPos = BattleUtility.GetHitPos(mHero);
					int healthPoint = Mathf.RoundToInt(posionDamagePercent * mHero.heroAttribute.maxHP);
					AdjustHeelth(-healthPoint);
					BattleUtility.ShowDamageBeat(0.0f,healthPoint,hitPos,1);
				}
			}
		}
	}

	public void AdjustHeelth(int realDamage)
	{
		currentHP = Mathf.Clamp(currentHP + realDamage,0,maxHP);
	}

	public void AddBuff(AttackType type,SubAttackType subType,_ElementType eleType,float power,int turnCount)
	{
		BuffAttribute buffAttr = new BuffAttribute();
		buffAttr.attackType = type;
		buffAttr.subAttackType = subType;
		buffAttr.turnCount = turnCount;
		buffAttr.power = power;
		bool isExist = false;
		for(int i=0;i<buffs.Count;i++) 
		{
			if(buffs[i].attackType==type && buffs[i].subAttackType==subType && buffs[i].eleType==eleType)
			{
				buffs[i].turnCount = buffAttr.turnCount;
				isExist = true;
				break;
			}
		}
		if(!isExist)
		{
			buffs.Add(buffAttr);
			if(type==AttackType.Health)
			{
				
			}
			else if(type==AttackType.Enhance)
			{
				if(subType==SubAttackType.Attack)
				{
					currentDamage += baseDamage*power;
				}
				else if(subType==SubAttackType.Defence)
				{
					currentDefence += baseDefence*power;
				}
				else if(subType==SubAttackType.Element)
				{
					if(!elementEnhances.Contains(elementType))
						elementEnhances.Add(elementType);
				}
			}
		}
	}

	public static float offsetDelay = 0;
	public void Hit(Hero attacker,AttackAttribute aa,float moveDur)
	{
		currentAttackers.Add(mHero);
		bool isCrit = false;
		if(aa.attackType == AttackType.Damage)
			isCrit = Random.Range(0,10000) < attacker.heroAttribute.currentCritOdds ? true : false;//TODO ,need get real crit

		Debug.Log("......................skillElementType.Count............................." + attacker.heroAttribute.skillElementType.Count);
		float relationAddition = 1;
		float relationAdditionBySkill = 0;


		if (attacker.heroAttribute.skillElementType.Count > 0)
		{
			relationAdditionBySkill = BattleUtility.GetRestrainRelationBySkill(attacker.heroAttribute.skillElementType,mHero.heroAttribute.elementType);
		}
		
		relationAddition = BattleUtility.GetRestrainRelation(attacker.heroAttribute.elementType,mHero.heroAttribute.elementType);

		if (relationAdditionBySkill > relationAddition)
			relationAddition = relationAdditionBySkill;

		float realDamage  = BattleUtility.GetRealDamage (attacker,mHero,isCrit,relationAddition);

		List<AttackQueue> queues = InsertQueues(attacker,aa.attackQueue,moveDur + offsetDelay);
		float realDamageCombo = 0;
		foreach(AttackQueue aq in queues)
		{
			if(aq.isCombo)
			{
				//BattleController.SingleTon().battleState = BattleState.IsCombo;
				//BattleController.SingleTon().doSkill();
				//realDamageCombo += aq.power * realDamage * 1.5f;
				realDamageCombo += aq.power * realDamage * (1.5f + attacker.heroAttribute.additionCombo);
				aq.realDamage = aq.power * realDamage * (1.5f + attacker.heroAttribute.additionCombo);
//				Debug.Log(aq.realDamage);
			}
			else
			{
				realDamageCombo += aq.power * realDamage;
				aq.realDamage = aq.power * realDamage;
			}
		}

		calculateHP -= Mathf.RoundToInt(realDamageCombo);
		StartCoroutine(_Hit(aa,attacker,moveDur,queues,isCrit,relationAddition));
	}

	public float lastHitTime;
	IEnumerator _Hit(AttackAttribute aa,Hero attacker,float totalDelay,List<AttackQueue> queue,bool isCrit,float relationAddition)
	{
//		yield return new WaitForSeconds(totalDelay);
		for(int i = 0;i < queue.Count;i++)
		{
			yield return new WaitForSeconds(queue[i].hitTime - Time.time);
//			if(attacker.heroRes.HitEffect!=null)
//			{
//				GameObject eff = PoolManager.SingleTon().Spawn(attacker.heroRes.HitEffect,BattleUtility.GetHitPos(mHero),Quaternion.identity);
//				eff.transform.localScale = mHero.heroRes.BodyScale;
//			}
			Vector3 hitPos;
			Vector3 offset;
			Vector3 pos;
			if(i ==0 && isCrit)
			{
				hitPos = BattleUtility.GetHitPos(mHero);
				offset = new Vector3(Random.Range(-0.5f,0.5f),Random.Range(1.5f,3.4f),0);
//				pos = BattleUtility.GetNGUIPosFromWorldPos(hitPos + offset);
				BattleUtility.ShowCritEffect(hitPos + offset);
			}
			if(queue[i].isCombo)
			{
				hitPos = BattleUtility.GetHitPos(mHero);
				offset = new Vector3(Random.Range(-1.5f,1.5f),Random.Range(2.5f,3.5f),0);
//				pos = BattleUtility.GetNGUIPosFromWorldPos(hitPos + offset);
				SpawnManager.SingleTon().comboCount++;
				BattleUtility.ShowMultiEffect(hitPos + offset);
			}
			iTween.ShakePosition(mHero.gameObject,new Vector3(0.01f,0.5f,0),0.1f);
			offset = new Vector3(Random.Range(-0.5f,0.5f),Random.Range(1.5f,3.4f),0);
			hitPos = BattleUtility.GetHitPos(mHero);
			pos = BattleUtility.GetNGUIPosFromWorldPos(hitPos + relationAddition * i * new Vector3(Random.Range(-0.2f,0.2f),
		         (queue[i].hitTime - Time.time) * Random.Range(4f,6f),0));
			int realDamage = Mathf.Max(1,Mathf.RoundToInt(queue[i].realDamage));
			//Debug.Log("realDamage per:" + realDamage);
			if(aa.attackType == AttackType.Damage)
			{
				BattleUtility.ShowDamageBeat(shakeRadius,realDamage,pos,relationAddition);
				if(mHero.heroAttack.isDefense)
				{
//					BattleUtility.ShowDefenseHitEffect(attacker,hitPos);
					mHero.heroAttack.guardShield.ShowFlash();
					AudioManager.SingleTon().PlayDefenseClip();
				}
				else
				{
					BattleUtility.ShowElementHitEffect(attacker,hitPos);
					mHero.GetComponent<AudioSource>().clip = queue[i].audioClip;
					mHero.GetComponent<AudioSource>().Play();
				}

				AdjustHeelth(-realDamage);
				if(mHero.Btn != null)
				{
					mHero.Btn.ShakeHead();
					mHero.Btn.RequireUpdate = true;
				}
				mHero.heroAnimation.Play(Hero.ACTION_HIT);
//				if (mHero.heroResEffect != null)
//					mHero.heroResEffect.PlayEffect(_AnimType.Hit);
				if(mHero.heroEffect != null)
					mHero.heroEffect.PlayEffects(_AnimType.Hit);
				lastHitTime = Time.time;
				tmpDelay = 0;
				Drop(aa,attacker,i);
			}
			else
			{
				if (AddHP > 0)
				{
					BattleUtility.ShowDamageBeat(shakeRadius,AddHP,pos,relationAddition);
					AddHP = 0;
				}
				//BattleUtility.ShowDamageBeat(shakeRadius,realDamage,pos,relationAddition,false);
				//AdjustHeelth(realDamage);
			}
		}
		currentAttackers.Remove(mHero);
	}

	void Drop(AttackAttribute attackAttribute,Hero attacker,int i)
	{
		int dropNum; 
		if(mHero.Side == _Side.Enemy || BattleSimpleController.SingleTon()!=null)
		{
			//Debug.Log("attackAttribute.impactNum:" + attackAttribute.impactNum);
			if(attackAttribute.impactNum==1)
			{
				if(i==0)
					dropNum = Random.Range(0,2) == 0 ? 1 : 0;
				else
					dropNum = Random.Range(0,10) == 0 ? 1 : 0;
				if(dropNum==1)
					_DropOut(mHero,_DropType.HC);//HC
				
				if(i==0)
					dropNum = Random.Range(0,2) == 0 ? 2 : 1;
				else
					dropNum = Random.Range(0,10) < 4 ? 1 : 0;

				dropNum = Mathf.FloorToInt(dropNum * attacker.heroAttribute.additionBBDrop);
				for(int z = 0;z < dropNum;z++)
				{
					//Debug.Log("BC.......................NUM...................." + dropNum);

					_DropOut(mHero,_DropType.BC);//BC
				}

				if (i == 0)
					dropNum = Random.Range(0,2) == 0 ? 1 : 0;
				else
					dropNum = Random.Range(0,10) == 0 ? 1 : 0;
				if (dropNum == 1)
					_DropOut(mHero,_DropType.BattleMaterial);

			}
			else if(attackAttribute.impactNum > 1)
			{
				if(i==0)
					dropNum = Random.Range(0,4) == 0 ? 1 : 0;
				else
					dropNum = Random.Range(0,20) == 0 ? 1 : 0;
				if(dropNum==1)
					_DropOut(mHero,_DropType.HC);//HC
				
				if(i==0)
					dropNum = Random.Range(0,4) == 0 ? 2 : 1;
				else
					dropNum = Random.Range(0,10) < 2 ? 1 : 0;

				dropNum = Mathf.FloorToInt(dropNum * attacker.heroAttribute.additionBBDrop);
				for(int z = 0;z < dropNum;z++)
				{
					//Debug.Log("BC.......................NUM...................." + dropNum);
					_DropOut(mHero,_DropType.BC);//BC
				}
			}
			//TODO should amend the drop of coin and soul
			if(mHero.heroAttribute.coinCount > 0)
			{
				bool drop = Random.Range(0,2) == 0 ? true : false;
				if(drop)
				{
					_DropOut(mHero,_DropType.Coin,mHero.heroAttribute.coinPerDrop);//Coin
					mHero.heroAttribute.coinCount --;
				}
			}
			if(mHero.heroAttribute.soulCount > 0)
			{
				bool drop = Random.Range(0,2) == 0 ? true : false;
				if(drop)
				{
					_DropOut(mHero,_DropType.Soul,mHero.heroAttribute.soulPerDrop);//Soul
					mHero.heroAttribute.soulCount --;
				}
			}
		}				
	}
	
	void _DropOut(Hero target,_DropType type,int dropValue = 0)
	{
		Vector3 pos = BattleUtility.GetCenterPos (target);
		Vector3 offset =  new Vector3(Random.Range(-1f,1f),Random.Range(-2.0f,2.0f),0);
		pos = pos + offset;
		GameObject go = null;
		Drop drop = null;
		if(type == _DropType.BC)
		{
			GameObject prefabBB = BattleController.SingleTon()!=null ? BattleController.SingleTon().prefabBB : BattleSimpleController.SingleTon().prefabBB;
			go = PoolManager.SingleTon().Spawn(prefabBB,pos,Quaternion.identity);
			drop = go.GetComponent<Drop>();
		}
		else if(type == _DropType.HC)
		{
			GameObject prefabHC = BattleController.SingleTon()!=null ? BattleController.SingleTon().prefabHC : BattleSimpleController.SingleTon().prefabHC;
			go = PoolManager.SingleTon().Spawn(prefabHC,pos,Quaternion.identity);
			drop = go.GetComponent<Drop>();
		}
		else if(type == _DropType.Soul)
		{
			if(dropValue>0)
			{
				go = PoolManager.SingleTon().Spawn(BattleController.SingleTon().prefabSoul,pos,Quaternion.identity);
				drop = go.GetComponent<Drop>();
				drop.Val = dropValue;
			}
		}
		else if(type == _DropType.Coin)
		{
			if(dropValue>0)
			{
				go = PoolManager.SingleTon().Spawn(BattleController.SingleTon().prefabCoin,pos,Quaternion.identity);
				drop = go.GetComponent<Drop>();
				drop.Val = dropValue;
			}
		}
		else if(type == _DropType.BattleMaterial)
		{
			if(dropValue>0)
			{
				go = PoolManager.SingleTon().Spawn(BattleController.SingleTon().prefabMaterial,pos,Quaternion.identity);
				drop = go.GetComponent<Drop>();
				drop.Val = dropValue;
			}
		}

		if(drop!=null)
		{
			drop.ChangeOrderLayer(mHero.OrderLayerName,(int)(go.transform.position.y * -100));
			drop.OnSpawn(target.transform.position,offset);
			drop.type = type;
			if(BattleController.SingleTon()!=null)
			{
				BattleController.SingleTon().Drops.Add(drop);
			}
			else
			{
				BattleSimpleController.SingleTon().Drops.Add(drop);
			}
		}
	}

	List<AttackQueue> InsertQueues(Hero attacker,List<AttackQueue> queue,float totalDelay)
	{
		float delay = 0;
		List<AttackQueue> realQueue = new List<AttackQueue>();
		for(int i=0; i<queue.Count; i++)
		{
			delay += queue[i].delay;
			AttackQueue aq0 = AttackQueue.CloneAttackQueue(queue[i]);
			float hitTime = Time.time + delay + totalDelay;
			aq0.hitTime = hitTime;
			int index = hitQueue.Count;

			for(int j = 0;j < hitQueue.Count;j++)
			{
				if(Mathf.Abs(hitQueue[j].hitTime - hitTime) <= commonHitInterval)
				{
					if(!aq0.isCombo)
					{
						aq0.isCombo = true;
					}
				}
				if(aq0.hitTime < hitQueue[j].hitTime)
				{
					index = j;
					break;
				}
			}
			if(index<hitQueue.Count)
				hitQueue.Insert(index,aq0);
			else
				hitQueue.Add(aq0);
			realQueue.Add(aq0);
		}
		return realQueue;
	}

}

[System.Serializable]
public class BuffAttribute
{
	public int turnCount;
	public float baseNum;
	public float power;
	public string spirteName;
	public AttackType attackType;
	public SubAttackType subAttackType;
	public _ElementType eleType;
}



