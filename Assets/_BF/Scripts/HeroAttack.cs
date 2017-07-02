using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;

[RequireComponent(typeof(Hero))]
public class HeroAttack : MonoBehaviour {

	Hero mHero;
	[SerializeField]
	List<AttackAttribute> mAttackAttributes;
	AttackAttribute mCurrentAttackAttribute;
	public Vector3 defaultPos;
	//public int maxTurn = 2;
	public int maxTurn = 0;
	public delegate void OnStart();
	public OnStart onStart;
	public delegate void OnFinish();
	public OnFinish onFinish;
	public SkillManager skillMgr;

	// Use this for initialization
	void Start () {
		mHero = GetComponent<Hero>();
		if(skillMgr == null)
		{
			skillMgr = GetComponent<SkillManager>();
		}
	}

	public void SetAttackAttributes(List<AttackAttribute> attrs)
	{
		mAttackAttributes = attrs;
	}

	public List<AttackAttribute> GetAttackAttributes()
	{
		return mAttackAttributes;
	}

	public bool isDefense =false;
	public GuardShield guardShield;
	public void Defense()
	{
		if(isDefense == false)
		{
			if(mHero.Btn!=null)mHero.Btn.InActiveButton();
			StartCoroutine(_Defense());
		}
	}

	IEnumerator _Defense()
	{
		mHero.Status = _HeroStatus.AfterTurn;
		AudioManager.SingleTon().PlayDefenseClip();
		mHero.heroAttribute.currentDefence += mHero.heroAttribute.baseDefence;
		isDefense = true;
		GameObject go = PoolManager.SingleTon().Spawn(BattleController.SingleTon().prefabDefense,BattleUtility.GetCenterPos(mHero),Quaternion.identity) as GameObject;
		guardShield = go.GetOrAddComponent<GuardShield>();
		mHero.heroEffect.enableHitEffect = false;
		mHero.heroEffect._ChangeColor(BattleController.SingleTon().defenseColor);
		yield return new WaitForSeconds(1.5f);
	}

	public void UnDefense()
	{
		if (isDefense)
		{		
			mHero.heroAttribute.currentDefence -= mHero.heroAttribute.baseDefence;
			isDefense = false;
			mHero.heroEffect.enableHitEffect = true;
			mHero.heroEffect.RevertMaterial();
			if(guardShield!=null)
			{
				PoolManager.SingleTon().UnSpawn(guardShield.gameObject);
			}
		}
	}

	public void Attack(int index = 0)
	{
		if(index!=0)
		{
			//mHero.heroAttribute.currentEnergy = 0;
			mHero.AdjustEnergy(-mHero.heroAttribute.currentEnergy);
			if (mHero.Side == _Side.Player)
				mHero.Btn.RequireUpdate = true;
			skillMgr.doInitiativeSkill(mAttackAttributes[index]);
		}

		GetHitTargets(mAttackAttributes[index]);

		if(mAttackAttributes[index].attackTargets.Count==0)
			return;
		if (onStart != null)onStart();
		StartCoroutine (TurningAttack(index));
	}

	int currentTurn = 1;
	bool isAttacking = false;
	IEnumerator TurningAttack(int index)
	{
		if(BattleSimpleController.SingleTon()!=null)
		{
			currentTurn = 1;
		}else{
			currentTurn = Random.Range(1,maxTurn + 1);
//            currentTurn = 1;
		}
		while(currentTurn>0)
		{
			if(!isAttacking)
			{
				GetHitTargets(mAttackAttributes[index]);
				isAttacking = true;
				StartCoroutine (_Attack(mAttackAttributes[index]));
			}
			yield return null;
		}
		while(isAttacking)
		{
			yield return null;
		}
		if(onFinish!=null)onFinish();
	}

	public void GetPlayerTargets(AttackAttribute aa)
	{
		List<Hero> targets = new List<Hero>();

		BattleSimpleController simpleController = BattleSimpleController.SingleTon();
		BattleController controller = BattleController.SingleTon();
		List<Hero> tmpTargets = null;
		if(simpleController!=null)
		{
			tmpTargets = simpleController.LeftHeroes;
		}
		else
		{
			tmpTargets = controller.LeftHeroes;
		}
		aa.attackTargets = targets;
		int j = aa.impactNum;

		if(controller!=null)
		{
			if(j>0 && BattleController.SingleTon().HandleTarget!=null && !BattleController.SingleTon().HandleTarget.IsDead)
			{
				if (aa.targetType == 0)
				{
					targets.Add(BattleController.SingleTon().HandleTarget);
					j --;
				}
				else
					if (aa.targetType == BattleController.SingleTon().HandleTarget.heroAttribute.elementType)
				{
					targets.Add(BattleController.SingleTon().HandleTarget);
					j --;
				}
			}
		}

//		tmpTargets = BattleUtility.GetRandomTargets(tmpTargets);
		tmpTargets = BattleUtility.GetCalculateTargets(tmpTargets);

		for(int i=0;i < tmpTargets.Count;i ++)
		{
			if(j<=0)
			{
				break;
			}
			if(!targets.Contains(tmpTargets[i]))
			{				 
				if (aa.targetType == 0)
				{
					targets.Add(tmpTargets[i]);
					j --;
				}
				else
				if (aa.targetType == tmpTargets[i].heroAttribute.elementType)
				{
					targets.Add(tmpTargets[i]);
					j --;
				}
			}
		}
		aa.attackTargets = targets;
	}

	//TODO
	//should discuss the ai get target logic
	//1.repeat able e.g mechine gun 
	//2.not repeat able
	public void GetEnemyTargets(AttackAttribute aa)
	{
		List<Hero> targets = new List<Hero>();

		BattleSimpleController simpleController = BattleSimpleController.SingleTon();
		BattleController controller = BattleController.SingleTon();

		List<Hero> tmpTargets = null;
		if(simpleController!=null)
		{
			tmpTargets = simpleController.RightHeroes;
		}
		else
		{
			tmpTargets = new List<Hero>();
			for (int i = 0; i < controller.RightHeroes.Count; i ++)
			{
				if (controller.RightHeroes[i].gameObject.activeSelf)
				{
					tmpTargets.Add(controller.RightHeroes[i]);
				}
			}
		}

		aa.attackTargets = targets;
		int j = aa.impactNum;
		tmpTargets = BattleUtility.GetRandomTargets(tmpTargets);
		for(int i=0;i < tmpTargets.Count;i ++)
		{
			if(j<=0)
			{
				break;
			}
			if(!targets.Contains(tmpTargets[i]))
			{
				if (aa.targetType == 0)
				{
					targets.Add(tmpTargets[i]);
					j --;
				}
				else
				if (aa.targetType == tmpTargets[i].heroAttribute.elementType)
				{
					targets.Add(tmpTargets[i]);
					j --;
				}

			}
		}
		aa.attackTargets = targets;
	}

	public void GetHitTargets(AttackAttribute aa)
	{
		if(aa.attackType == AttackType.Damage || aa.attackType == AttackType.Other)
		{
			if(mHero.Side == _Side.Player)
			{
				GetPlayerTargets(aa);
			}
			else
			{
				GetEnemyTargets(aa);
			}
		}
		else if(aa.attackType == AttackType.Health || aa.attackType == AttackType.Enhance || aa.attackType == AttackType.Remove)
		{
			if(mHero.Side == _Side.Player)
			{
				GetEnemyTargets(aa);
			}
			else
			{
				GetPlayerTargets(aa);
			}
		}
	}

	static public float moveDuration = 1;
	IEnumerator _Attack(AttackAttribute attackAttribute)
	{
		/*if (mHero.Side==_Side.Enemy)
		{
			//NPC 不存在过量击杀，所以攻击前判断目标的血量是否大于0
			bool canAttack = false;
			for (int i = 0; i < attackAttribute.attackTargets.Count; i++)
			{
				if (attackAttribute.attackTargets[i].heroAttribute.currentHP > 0)
				{
					canAttack = true;
					break;
				}
			}
			if (canAttack == false)
				yield return null;
		}*/


		if(mHero.Side==_Side.Player && attackAttribute.attackTargets!=null && attackAttribute.attackTargets.Count>0)
		{
			if(BattleController.SingleTon()!=null)
				BattleController.SingleTon().CurrentAttackTarget = attackAttribute.attackTargets[0];
		}
		mCurrentAttackAttribute = attackAttribute;
		float speed = mHero.heroAttribute.speed <= 0 ? 1 : mHero.heroAttribute.speed;

		//如果是技能攻击，移动速度快2倍
		if (attackAttribute.attackClip == Hero.ACTION_SKILL1)
			speed = speed / 2;

		if(mHero.heroAttribute.speed <= 0)
		{
			Debug.LogError("The hero " + mHero.heroAttribute.heroTypeId + " speed can't be 0!");
		}
		float durT = speed / 10;
		float delayByLocation = BattleUtility.GetDelayByLocation(mHero);
		attackAttribute.hitDelay1 = durT;
		attackAttribute.hitDelay = delayByLocation;

		SpawnManager.SingleTon().delay = durT + delayByLocation;

		for(int j =0;j < attackAttribute.attackTargets.Count;j++)
		{
			float hitDelay = 0;
			if(attackAttribute.moveable)
			{
				hitDelay = durT + delayByLocation;
			}
			if(attackAttribute.moveable && attackAttribute.attackType == AttackType.Damage)
			{
				hitDelay += attackAttribute.moveForwardDelay;
				hitDelay += attackAttribute.moveForwardDelay1;
				Debug.Log("attackAttribute.moveForwardDelay:" + attackAttribute.moveForwardDelay);
				Debug.Log("attackAttribute.moveForwardDelay1:" + attackAttribute.moveForwardDelay1);
			}
			attackAttribute.attackTargets[j].heroAttribute.Hit(mHero,attackAttribute,hitDelay);
		}
		float t = 0;
		Vector2 startPos;
		Vector2 targetPos = transform.position;
		Transform curHitTarget = null;
		if(attackAttribute.moveable && attackAttribute.attackType == AttackType.Damage)
		{
			yield return new WaitForSeconds(attackAttribute.moveForwardDelay);

			if (mAttackAttributes.IndexOf(mCurrentAttackAttribute) != 0)
			{
				if(attackAttribute.onMoveForwardSprint!=null)
				{
					attackAttribute.onMoveForwardSprint();
				}
			}
			else
			{
				if(attackAttribute.onMoveForwardStart!=null)
				{
					attackAttribute.onMoveForwardStart();
				}
			}
			yield return new WaitForSeconds(attackAttribute.moveForwardDelay1);
			startPos = transform.position;
			Vector2 pos_offset = Vector2.zero;
			if (mHero.heroRes.HitPoints != null && mHero.heroRes.HitPoints.Count > 0) {
				if(mHero.heroRes.HitPoints[0]!=null)
					pos_offset = (Vector2)mHero.heroRes.HitPoints [0].position - startPos;
			}
//			if(attackAttribute.moveTarget == null)
//			{
			int index = mAttackAttributes.IndexOf(attackAttribute);
			if (index > 0)
			{
				targetPos = (Vector2)BattleUtility.GetLeftSkillTargetPos();
			}
			else
			{
				curHitTarget = BattleUtility.GetLeastUsedHitPoint(attackAttribute.attackTargets[0]);
                if(curHitTarget!=null)
                {
					targetPos = (Vector2)curHitTarget.position - pos_offset;
                }
                else
                {
					targetPos = (Vector2)attackAttribute.attackTargets[0].transform.position - pos_offset;
                }
			}
//			}
//			else
//			{
//				targetPos = attackAttribute.moveTarget.position;
//			}
			mHero.updateSelfLayerByPostion = true;
			float moveDuration = durT + delayByLocation;
			while(t<1)
			{
				t += Time.deltaTime/moveDuration;
				t = Mathf.Min(1,t);
				transform.position = Vector2.Lerp(startPos,targetPos,t);
				yield return null;
			}
			if(attackAttribute.onMoveForwardFinish!=null)
			{
				attackAttribute.onMoveForwardFinish();

			}
		}

		yield return new WaitForSeconds (attackAttribute.hitDelay);
		float attackStartTime = Time.time;
		if (attackAttribute.onAttackStart != null)
		{
			if (attackAttribute.attackClip == Hero.ACTION_SKILL1)
				attackAttribute.onSkillAttackStart();
			else
				attackAttribute.onAttackStart ();//need csv file to read 
		}
		yield return new WaitForSeconds (attackAttribute.hitDelay1);

		if(mHero.heroRes!=null)
		{
			float clipLength = CommonUtility.GetRealAnimLength(mHero.heroRes.CurrentAm);
			Debug.Log("clipLength.................................................." + clipLength);
			if(clipLength - (Time.time - attackStartTime) > 0)
			{
				yield return new WaitForSeconds(clipLength - (Time.time - attackStartTime));
				//float length = clipLength - (Time.time - attackStartTime);
				//Debug.Log("length.................................................." + length);
				//yield return new WaitForSeconds(length);

			}
		}
		yield return new WaitForSeconds(0.1f);
		if (curHitTarget != null)
			attackAttribute.attackTargets [0].ReleaseHitTarget (curHitTarget);
		if (attackAttribute.onAttackFinish!=null)attackAttribute.onAttackFinish ();
		if(attackAttribute.moveable)
		{
			yield return new WaitForSeconds(attackAttribute.moveBackDelay);
			if(attackAttribute.onMoveRevertStart!=null)attackAttribute.onMoveRevertStart();
			yield return new WaitForSeconds(attackAttribute.moveBackDelay1);
			startPos = transform.position;
			targetPos = defaultPos;
			Vector2 controllPos = (targetPos + startPos) / 2 + new Vector2(0,1);
			t = 0;
//			float backSpeed = mHero.heroAttribute.speed * 2;
			float maxT = Vector2.Distance(startPos,targetPos) / BattleController.SingleTon().moveBackSpeed;
			while(t<1)
			{
				t += Time.deltaTime/maxT;
				t = Mathf.Min(1,t);
				transform.position = Curve.Bezier2(startPos,controllPos,targetPos,t);
				yield return null;
			}
			mHero.updateSelfLayerByPostion = false;
			CommonUtility.SetSortingLayerWithChildren(mHero.heroRes.gameObject,mHero.OrderLayerName);

			if(attackAttribute.onMoveRevertFinish!=null)attackAttribute.onMoveRevertFinish();
		}
		yield return new WaitForSeconds(0.1f);

		isAttacking = false;
		currentTurn --;
	}

#if UNITY_EDITOR
	public Hero target;
	public bool debug;
	void OnDrawGizmos()
	{
		if(target!=null)Gizmos.DrawWireSphere (target.transform.position,0.5f);
	}
#endif


}

//public enum AttackType{Health=1,Enhance=2,Remove=4,Rebirth=8,Damage=16,Reduce=32}
public enum AttackType{Damage=1,Health=2,Enhance=3,Reduce=4,Other=5,Remove=16,Rebirth=32}
public enum SubAttackType{None=0,Attack=1,Defence=2,Curse=3,Poison=4,Element=5,Recover=6}
[System.Serializable]
public class AttackAttribute
{
	public Hero hero;
	public HeroInfo heroInfo;
	public List<AttackAttribute> subAttackAttributes;
	public AttackType attackType;
	public SubAttackType subAttackType;
	public int impactNum;
	public List<Hero> attackTargets;
	public List<AttackQueue> attackQueue;
	public List<AttackQueue> attackFullQueue;
	public Transform moveTarget;
//	public float moveDuration;
	public GameObject shootObject;
	public float shootDelay;
	public bool moveable = true;
	public bool isShake = false;
	public float shakeDelay;//shake delay since attack begin;
	public float hitDelay;//delay before onAttackStart;
	public float hitDelay1;//delay after onAttackStart;
	public float moveForwardDelay;//delay before onMoveForwardStart;
	public float moveForwardDelay1;//delay after onMoveForwardStart;
	public float moveBackDelay;//delay before onMoveBackStart;
	public float moveBackDelay1;//delay after onMoveBackStart;
	public string moveClip = "Run";
	public string attackClip = "Attack";
	public float power;
	public _ElementType targetType;
	
	public delegate void OnMoveForwardStart();
	public OnMoveForwardStart onMoveForwardStart;
	public delegate void OnMoveForwardFinish();
	public OnMoveForwardFinish onMoveForwardFinish;
	public delegate void OnMoveRevertStart();
	public OnMoveRevertStart onMoveRevertStart;
	public delegate void OnMoveRevertFinish();
	public OnMoveRevertFinish onMoveRevertFinish;
	public delegate void OnAttackStart();
	public OnAttackStart onAttackStart;
	public delegate void OnAttackFinish();
	public OnAttackFinish onAttackFinish;
	public delegate void OnMoveForwardSprint();
	public OnMoveForwardSprint onMoveForwardSprint;
	public delegate void OnSkillAttackStart();
	public OnSkillAttackStart onSkillAttackStart;
	
	public AttackAttribute()
	{
		onMoveForwardStart +=  PlayAnimRun;
		onMoveForwardFinish += PlayAnimStandBy;
		onMoveRevertStart += PlayAnimStandBy;
		onMoveRevertFinish += PlayAnimStandBy;
		onAttackStart  += PlayAnimAttack;
		onAttackFinish += PlayAnimStandBy;
		onMoveForwardSprint += PlayAnimSprint;
		onSkillAttackStart += PlayAnimSkill;
	}

	List<GameObject> GetTargets()
	{
		List<GameObject> goList = new List<GameObject>();
		foreach(Hero hero in attackTargets)
		{
			if (hero != null && hero.gameObject != null)
				goList.Add(hero.gameObject);
		}
		return goList;
	}

	public void PlayAnimRun(){
		hero.heroAnimation.Play(Hero.ACTION_RUN);
		hero.heroEffect.PlayEffects(_AnimType.Run,GetTargets());
	}
	
	public void PlayAnimStandBy(){
		hero.heroAnimation.Play(Hero.ACTION_STANDBY);
		hero.heroEffect.PlayEffects(_AnimType.StandBy,GetTargets());
	}
	
	public void PlayAnimAttack(){
//		float t = GetTotalAttackDuration();
		hero.heroAnimation.Play (Hero.ACTION_ATTACK);
		hero.heroEffect.PlayEffects(_AnimType.Attack,GetTargets());
	}

	public void PlayAnimSkill()
	{
		hero.heroAnimation.Play (Hero.ACTION_SKILL1);
		hero.heroEffect.PlayEffects(_AnimType.Skill1,GetTargets());
	}

	public void PlayAnimSprint()
	{
		hero.heroAnimation.Play(Hero.ACTION_SPRINT);
		hero.heroEffect.PlayEffects(_AnimType.Sprint,GetTargets());
	}

	public void Shoot()
	{
		foreach(Hero h in attackTargets)
		{
			PoolManager.SingleTon().Spawn(shootObject,h.transform.position,Quaternion.identity,shootDelay,2);
		}
	}

	float GetTotalAttackDuration()
	{
		float totalDur = 0;
		totalDur += hitDelay1;
		for(int i =0;i < attackQueue.Count;i ++)
		{
			totalDur += attackQueue[i].delay;
		}
		return totalDur;
	}

}
[System.Serializable]
public class AttackQueue
{
	public float hitTime;
	public bool isCombo;
	public float delay;
	public float delayBefore;
	public float delayAfter;
	public float power;
	public float realDamage;
	public List<Unit> impactUnits;
	public Hero attacker;
	public AudioClip audioClip;

	static public AttackQueue CloneAttackQueue(AttackQueue aq)
	{
		AttackQueue aq0 = new AttackQueue();
		aq0.power = aq.power;
		aq0.audioClip = aq.audioClip;
		aq0.hitTime = aq.hitTime;
		aq0.delay = aq.delay;
		aq0.delayAfter = aq.delayAfter;
		aq0.delayBefore = aq.delayBefore;
		return aq0;
	}

	static public AttackQueue Instance(AudioClip clip,float delayBefore,float delayAfter,float power)
	{
		AttackQueue queue = new AttackQueue ();
		queue.audioClip = clip;
		queue.delay = delayBefore + delayAfter;
		queue.delayBefore = delayBefore;
		queue.delayAfter = delayAfter;
		queue.power = power;
		return queue;
	}
}

