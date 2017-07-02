using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using StatusMachines;

public enum _Side{Enemy,Player}
public enum _HeroStatus{Entering,BeforeTurn,Turning,AfterTurn}
//[ExecuteInEditMode]
public class Hero : MonoBehaviour {

	public delegate void OnAttack();
	public OnAttack onAttack;
	public HeroAttribute heroAttribute;
	public HeroAttack heroAttack;
	public HeroAnimation heroAnimation;
	public HeroResEffect heroResEffect;
	public HeroRes heroRes;
	public HeroEffect heroEffect;
	public BoxCollider2D heroCollider;
	public _HeroStatus Status = _HeroStatus.BeforeTurn;
	public UISlider healthBar;
	public const string ACTION_ATTACK = "Attack";
	public const string ACTION_HIT = "Hit";
	public const string ACTION_DEATH = "Death";
	public const string ACTION_RUN = "Run";
	public const string ACTION_STANDBY = "StandBy";
	public const string ACTION_SKILL1 = "Skill1";
	public const string ACTION_POWER = "Power";
	public const string ACTION_SPRINT = "Sprint";
	public const string ACTION_CHEER = "Cheer";
	public int Index = 0;
	public float AttackDelay;
	public _Side Side = _Side.Enemy;
	public Transform HeroBody;
	public HeroBtn Btn;
	public Buff HeroBuff;
	public Transform CenterPoint;
	public List<Transform> HitPoints;
	public List<Transform> AvailableHitPoints;
	public AudioSource HeroAudio;
	public AudioClip HeroAudioClip;
	public int attackerCount;
	public Vector2 BaseHealthBarScale;
	public int BaseDamage = 50;
	public int BaseDefence;
	public float CritRadius = 10;
	public int BaseAttackMode = 0;
	public int BaseAttackTurn = 4;
	public int BaseRevert = 1;
	public float TotalAttackDuration;
	public float[] BaseAttackQueue;
	public int MaxCoinCount = 0;
	public int MaxSoulCount = 0;
	public int ValuePerCoin = 0;
	public int ValuePerSoul = 0;
	public int TotalCoin = 0;
	public int TotalSoul = 0;
	public string HeroName;
	public Skill HeroSkill;
	public float LastAttackTime;
	public float RealHealthBarWidth;
	public bool IsElementATKEnhanced;
	public float ElementATKEnhancePercent;
	public bool IsElementDEFEnhanced;
	public int CurseAttackChanges;
	public int PoisonAttackChanges;
	public int WeakAttackChanges;
	public int PalsyAttackChanges = 10;
	public string OrderLayerName;
	public bool IsCursed;
	public bool IsPoisoned;
	public float ElementDEFEnhancePercent;
	public List<Hero> Attackers;
	public BattleController Controller;
	public SkillManager skillMgr;
	public int SkillOdds;//Only for AI;
	public bool IsShake;
	public float ShakeDur = 2.5f;
	public float ShakeDelay = 1.5f;
	public float HitDelay = 0.5f;
	public GameObject DeathObject;
	public bool IsDead = false;
	public bool IsDeading = false;
	public bool Load = false;
	public bool Movable = true;
	public bool IsFlash = false;
	public bool IsBoss = false;
	public bool isSummonMonster = false;
	public BattleDropChest dropChest;
	public Dictionary<Transform,int> hitPointUseedCount;
	public bool isCaptian = false;
	public Hero targetHero;
	public List<Hero> targetHeros;
	public AttackAttribute currentAttackAttribute;
	public float lastHitedTime;//上次被击中时间
	public Hero lastHitedBy;//上次的攻击者
	public List<BaseSkill> buffs;//所有buff
	public List<BaseSkill> mainBuffs;//主buff
	public UnitAttribute baseAttribute;//基本属性
	public UnitAttribute additionAttribute;//增强属性
	public StatusMachine sm;

	void Awake(){

		baseAttribute = new UnitAttribute ();
		additionAttribute = new UnitAttribute ();

		if(heroAttack==null)
			heroAttack = GetComponent<HeroAttack>();

		if(heroAttack==null)
			heroAttack = gameObject.AddComponent<HeroAttack>();

		if(heroResEffect==null)
			heroResEffect = heroRes.GetComponent<HeroResEffect>();

		if(heroAttribute==null)
			heroAttribute = GetComponent<HeroAttribute>();

		if(heroAttribute==null)
			heroAttribute = gameObject.AddComponent<HeroAttribute>();

		if(heroAnimation==null)
			heroAnimation = heroRes.GetComponent<HeroAnimation>();

		if(heroEffect==null)
		{
			heroEffect = heroRes.gameObject.AddComponent<HeroEffect>();
		}
		if(heroEffect!=null)
		{
			heroEffect.hero = this;
		}
		if(heroCollider==null)
			heroCollider = heroRes.GetComponent<BoxCollider2D>();
		
//		InitStatusMachine ();
	}

	void Start () {
		Controller = BattleController.SingleTon();
		AvailableHitPoints.AddRange (HitPoints);
		HeroAudio = GetComponent<AudioSource>();
		if(heroAttack==null)
		{
			heroAttack = GetComponent<HeroAttack>();
		}
		if(heroAttribute==null)
		{
			heroAttribute = GetComponent<HeroAttribute>();
		}
		if(heroRes==null)
		{
			heroRes=GetComponent<HeroRes>();
		}
		if(skillMgr == null)
		{
			skillMgr = GetComponent<SkillManager>();
		}
		heroAttack.onStart += OnTurning;
		if(Btn!=null)
		{
			heroAttack.onStart += Btn.InActiveButton;
			heroAttack.onStart += Potion.InActiveAll;
		}
		heroAttack.onFinish += OnTurnDone;
		hitPointUseedCount = BattleUtility.InitHitPointUsedCount(heroRes);
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
//		sm.SetEntryStatus (_HeroMachineStatus.StandBy.ToString ());
	}

	public void OnTurnStart()
	{
		Status = _HeroStatus.BeforeTurn;
	}

	public void OnTurning()
	{
		Status = _HeroStatus.Turning;
	}

	public void OnTurnDone()
	{
		Status = _HeroStatus.AfterTurn;
	}

	public bool updateSelfLayerByPostion = false;
	public bool updateHealbar = true;

	void Update(){
		if(heroAttribute.currentHP<=0 && heroAttribute.currentAttackers.Count == 0 && IsDead == false)
		{
			if(Controller.HandleTarget == this)
				Controller.NextHandleTarget();
			GameObject deathObject = null;
			if(Side == _Side.Enemy)
			{
				if(dropChest!=null && dropChest.type != CHEST_TYPE.CHEST_ERROR)
				{
					deathObject = InstantiateChest();
				}
			}
			StartCoroutine(_DeadCoroutine(deathObject));
			if(healthBar!=null)healthBar.gameObject.SetActive(false);
			IsDead = true;
			IsDeading = true;
			BattleController.SingleTon().DeadHeroes.Add(this);
		}
		if(updateSelfLayerByPostion)UpdateRenderOrder();
		if(updateHealbar && healthBar!=null)UpdateBattleHealthBar();
	}

	void OnDisable()
	{
		if(healthBar!=null)
		{
			healthBar.gameObject.SetActive(false);
		}
	}

	void UpdateBattleHealthBar()
	{
		float radiu = (float)heroAttribute.currentHP/heroAttribute.maxHP;
		if((radiu<1 && radiu>0) || Controller.HandleTarget == this)
		{
			healthBar.gameObject.SetActive(true);
			Vector3 pos = transform.position;
			pos = new Vector3(pos.x - 1,pos.y + CommonUtility.GetColliderTop(heroCollider),0);
			pos = BattleController.SingleTon().BattleCamera.WorldToScreenPoint(pos);
			pos = BattleController.SingleTon().UICamera.ScreenToWorldPoint(pos);
			healthBar.transform.position = (Vector2)pos;
			healthBar.transform.localScale = new Vector3(0.161304f,0.429f,0);
			healthBar.value = radiu;
		}
		else
		{
			healthBar.gameObject.SetActive(false);
		}
	}

	GameObject mCurrentBody = null;
	SpriteRenderer[] mSrs = null;
	SkinnedMeshRenderer[] mSmrs = null;
	Renderer[] mrs = null;
	Dictionary<Renderer,int> defaultLayerOrder;
	void UpdateRenderOrder()
	{
		if(heroRes!=null && BattleController.SingleTon()!=null)
		{
			if(heroRes.CurrentAm!=null && heroRes.CurrentAm.body!=null)
			{
				if(mCurrentBody!=heroRes.CurrentAm.body)
				{
					mCurrentBody = heroRes.CurrentAm.body;
					mrs = heroRes.GetComponentsInChildren<Renderer>();
					mSrs = heroRes.GetComponentsInChildren<SpriteRenderer>();
					mSmrs = heroRes.GetComponentsInChildren<SkinnedMeshRenderer>();
					defaultLayerOrder = new Dictionary<Renderer, int>();
					foreach(Renderer r in mrs)
					{
						defaultLayerOrder.Add(r,r.sortingOrder);
					}
				}
				float y = transform.position.y;
				float x = transform.position.x;
//				Debug.Log("transform.position:" + transform.position);
				if(x > 0)
				{
					if(y >= BattleController.SingleTon().RightPoints[3].position.y)
					{
						ChangeLayer(0,y);
					}
					else if(y >= BattleController.SingleTon().RightPoints[0].position.y)
					{
						ChangeLayer(1,y);
					}
					else if(y >= BattleController.SingleTon().RightPoints[1].position.y)
					{
						ChangeLayer(2,y);
					}
					else if(y >= BattleController.SingleTon().RightPoints[2].position.y)
					{
						ChangeLayer(3,y);
					}
					else if(y >= BattleController.SingleTon().RightPoints[4].position.y)
					{
						ChangeLayer(4,y);
					}
				}
				else
				{
					if(y > BattleController.SingleTon().LeftPoints[4].position.y)
					{
						ChangeLayer(0,y);
					}
					else if(y > BattleController.SingleTon().LeftPoints[0].position.y)
					{
						ChangeLayer(1,y);
					}
					else if(y > BattleController.SingleTon().LeftPoints[1].position.y)
					{
						ChangeLayer(2,y);
					}
					else if(y > BattleController.SingleTon().LeftPoints[2].position.y)
					{
						ChangeLayer(3,y);
					}
					else if(y > BattleController.SingleTon().LeftPoints[5].position.y)
					{
						ChangeLayer(4,y);
					}
				}
			}
		}
	}

	void ChangeLayer(int location,float y)
	{
		string orderLayer = "UnitLayer" + location;
		foreach(Renderer r in mrs)
		{
			r.sortingLayerName = orderLayer;
			r.sortingOrder = (int)(defaultLayerOrder[r] - y * 100);
		}
	}

	public void Play(string clipName,List<GameObject> targets = null)
	{
		heroAnimation.Play(clipName);
		_AnimType animType = CommonUtility.AnimCilpNameStringToEnum(clipName);
		heroEffect.PlayEffects(animType);
	}

	IEnumerator _DeadCoroutine(GameObject deathObject)
	{
		Play(Hero.ACTION_DEATH);
		float length = heroAnimation.GetAnimClipLength(Hero.ACTION_DEATH);	
        if(Btn!=null)Btn.ShowDeathMask();
        if(Side == _Side.Enemy)
        {
            heroRes.GetComponent<Collider2D>().enabled = false;
            Controller.LeftHeroes.Remove(this);
        }
        else
        {
            Controller.RightHeroes.Remove(this);
            Controller.DeadHeroes.Add(this);
        }
		yield return new WaitForSeconds(length + 0.5f);
		
		BattleRecvData battleRev = DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).curBattleInfo;			
        if(battleRev!=null)
        {
            BattleStep battleStep = battleRev.getStep(BattleController.SingleTon().CurrentWave);
            Vector3 deathPos = heroRes.HeadTrans!=null ? heroRes.HeadTrans.position : transform.position;
            PoolManager.SingleTon().Spawn(BattleController.SingleTon().prefabDeathBase,deathPos,Quaternion.identity);
            yield return new WaitForSeconds(0.45f);
            PoolManager.SingleTon().Spawn(BattleController.SingleTon().prefabAfterDeath,deathPos + new Vector3(0,3,0),Quaternion.identity);
            if(Side == _Side.Enemy)
            {
                foreach (BattleCatchHero bc in battleStep._catchHeroList)
                {
                    if (bc.typeid == heroAttribute.heroTypeId && bc.isCatch == false)
                    {
                        BattleController.SingleTon().catchIds.Add(heroAttribute.heroTypeId);
                        bc.isCatch = true;
                        if (BattleController.SingleTon().prefabCatch != null)
                        {
                            yield return new WaitForSeconds(0.5f);
                            Vector3 pos = deathPos + new Vector3(0,3,0);
                            GameObject catchEffect = PoolManager.SingleTon().Spawn(BattleController.SingleTon().prefabCatch,pos,Quaternion.identity) as GameObject;
                            Debug.Log("transform.position:" + transform.position);
                            Debug.Log("catchEffect:" + pos);
                            if (catchEffect != null)
                            {
                                catchEffect.SetActive(true);    
                                BattleController.SingleTon().CatchMonsterEffects.Add(catchEffect);
                            }
                        }
                    }
                }
            }
        }

		BattleController.SingleTon().DeadHeroes.Remove(this);
		IsDeading = false;
		if(deathObject!=null)
			deathObject.SetActive(true);
		
		if (!IsBoss)
		{	
			if (this.isSummonMonster)
			{
				BattleController.SingleTon().onBtnDismissSummonClick(this.gameObject);
			}	
			gameObject.SetActive(false);
		}
	}

	GameObject InstantiateChest()
	{
		DeathObject = PoolManager.SingleTon().Spawn(Controller.prefabChest,transform.position,Quaternion.identity);
		Chest chest = DeathObject.GetComponent<Chest>();
		Chest.chests.Add(chest);
		chest.chestObj = DeathObject;
		chest.orderLayerName = OrderLayerName;
		chest.location = Index;
		chest.dropChest = dropChest;
		Renderer[] srs = DeathObject.GetComponentsInChildren<Renderer>(true);
		foreach(Renderer sr in srs)
		{
			sr.sortingLayerName = OrderLayerName;
		}
		if(Side == _Side.Enemy)
		{
			DeathObject.transform.eulerAngles = new Vector3(0,180,0);
		}
		DeathObject.SetActive(false);
		return DeathObject;
	}

	public void Rebirth()
	{
		IsDead = false;
		if(DeathObject!=null)DeathObject.SetActive(false);
		StartCoroutine(_RebirthCoroutine());
	}

	IEnumerator _RebirthCoroutine()
	{
		float t = 0;
		while(t < 1)
		{
			t += Time.deltaTime;
			yield return null;
		}
	}

	public void SetControllBtn(HeroBtn btn)
	{

		Btn = btn;
		btn.gameObject.SetActive(true);
		btn.InitData(this);
	}

	IEnumerator _UnSpawn(float t,Pool pool,GameObject go)
	{
		yield return new WaitForSeconds(t);
		pool.Unspawn(go);
	}

	public void OnHeal(int point)
	{
		AdjustHealth(point);
	}

	public void SpecialAttack(Hero attacker)
	{
		bool IsPalsy = Random.Range(0,100) < attacker.PalsyAttackChanges ? true : false;
		if(IsPalsy)
		{
			Debug.Log("IsPalsy");
			HeroBuff.AddSprite("battle_bad_icon_palsy");
		}
	}

	public void AdjustEnergy(int energy)
	{
		//if (energy == 0)
		//	return;

		if (energy >= 0)
		{
			//没有被诅咒才能累积BB能量
			if (heroAttribute.isCurse == false)
				heroAttribute.currentEnergy = Mathf.Clamp(heroAttribute.currentEnergy+energy,0,heroAttribute.maxEnergy);
				//heroAttribute.currentEnergy = Mathf.Clamp(heroAttribute.currentEnergy+energy + (int)heroAttribute.additionEnergy,0,heroAttribute.maxEnergy);
		}
		else
		{
			heroAttribute.currentEnergy = Mathf.Clamp(heroAttribute.currentEnergy+energy,0,heroAttribute.maxEnergy);
			BattleController.SingleTon().setSummonBar(energy);
		}

		if(Side==_Side.Player && Btn!=null)Btn.RequireUpdate = true;
	}

	public void AdjustHealth(int health)
	{
		heroAttribute.currentHP = Mathf.Clamp(heroAttribute.currentHP+health,0,heroAttribute.maxHP);
		if(heroAttribute.currentHP==0)
		{
			if(Controller.HandleTarget!=null && Controller.HandleTarget == this)
			{
				List<Hero> leftHeros = Controller.LeftHeroes;
				if(leftHeros.Count>0)
				{
					Hero hero = null;
					if(leftHeros[0] != gameObject)
					{  
						hero = leftHeros[0];
					}
					else if(leftHeros.Count>1)
					{
						hero = leftHeros[1];
					}
					Controller.HandleTarget = hero;
					if(hero!=null)Controller.TargetMark.transform.position = hero.transform.position + hero.heroRes.CenterOffset;
				}
			}
		}
		if(Btn!=null)Btn.RequireUpdate = true;
	}

	public IEnumerator SkillCameraEffect()
	{	
		if (Controller.cameraSkillEffectController)
		{
			Controller.cameraSkillEffectController.play(this);
			yield return new WaitForSeconds(1f);
		}
		SkillAttack();
	}

	public void SkillAttack()
	{	
		AudioSource.PlayClipAtPoint(Controller.GlobalSkillAudioClip,Camera.main.transform.position);
		_SkillAttack();
	}

	public void _SkillAttack()
	{
		Debug.Log("OnSkillAttack");
		heroAttack.Attack(1);
		AdjustEnergy(-heroAttribute.currentEnergy);
	}

	Transform currentTargetHitPoint;
	Hero currentTargetHero;

	IEnumerator ExtraEffect()
	{
		float length = heroAnimation.GetAnimClipLength(Hero.ACTION_SKILL1);			
		yield return new WaitForSeconds(length /2);
		GameObject firePrefab = Resources.Load("Effect/GroundFire_ParticalSG") as GameObject; 
		GameObject fire = Instantiate(firePrefab,Vector3.zero,Quaternion.identity) as GameObject;
		fire.SetActive(true);
		Debug.Log("fire.name......................" + fire.name);
		fire.transform.parent = transform;
	}

	public void ReleaseHitTarget(Transform tran)
	{
		if(HitPoints.Contains(tran) && !AvailableHitPoints.Contains(tran))
		{
			AvailableHitPoints.Add(tran);
		}
	}

	public Transform GetHitTarget()
	{
		if (AvailableHitPoints.Count > 0) 
		{
			Transform t = AvailableHitPoints [0];
			return t;
		}
		else 
		{
			return transform;
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position,0.1f);
		Gizmos.color = Color.blue;
		if(HitPoints!=null)
		{
			foreach(Transform t in HitPoints)
			{
				if(t!=null)
					Gizmos.DrawWireSphere(t.position,0.1f);
			}
		}

	}
}


