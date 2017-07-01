using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;

public class EnemyController : UnitController {

	public BaseAttribute enemyAttr;
	public BaseAttribute playerAttr;
	public UnitResources enemyResources;
	public Transform spawnPoint;
	public EnemyGizmos enemyGizmos;
	public EnemyState state = EnemyState.Idle;
	public string AttackID;
	public bool isCanAttack;

	protected override void Awake()
	{
		base.Awake ();
		enemyAttr = GetComponent<BaseAttribute>();
		enemyResources = GetComponent<UnitResources> ();
		enemyGizmos = GetComponent<EnemyGizmos>();
	}

	void Start()
	{
		Enemy[] enemys = GetComponentsInChildren<Enemy>();
		foreach(Enemy enemy in enemys)
		{
			enemy.player = playerAttr.GetComponent<PlayerController>();
			enemy.enemy = this;
			enemy.playerAnimator = playerAttr.GetComponent<Animator>();
			enemy.enemyAnimator = GetComponent<Animator>();
		}
	}

	//Animation Events 调用此脚本，并返回hit值，判断值改变IsCanAttack状态。
	void Hit0 (int hit)
	{
		if (hit == 0) {
			isCanAttack = false;
		} else if (hit == 1) {
			isCanAttack = true;
		}
		Debug.Log ("Hit:" + hit + ";isCanAttack:" + isCanAttack);
	}

	public float attackCooldown = 5;
	public float sprintCooldown = 5;
	public float fireBallCooldown = 5;
	public float commonCooldown = 3;

	[SerializeField]float mAttackCooldown;
	[SerializeField]float mSprintCooldown;
	[SerializeField]float mFireBallCooldown;
	[SerializeField]float mCommonCooldown;

	void Update()
	{
		UpdateCooldown ();

		if(Input.GetKeyDown(KeyCode.N))
		{
			enemyAttr.currentHealth = 0;
		}
		if(Input.GetKeyDown(KeyCode.M))
		{
			playerAttr.currentHealth = 0;
		}

		if(enemyAttr.currentHealth <= 0)
		{
			BattleController.SingleTon().PlayEnemyDeathAnim();
			pm.SendEvent("OnDead");
			enabled = false;
		}
	}

	void UpdateCooldown()
	{
		mAttackCooldown = Mathf.Clamp (mAttackCooldown+Time.deltaTime,0,attackCooldown);
		mSprintCooldown = Mathf.Clamp (mSprintCooldown+Time.deltaTime,0,sprintCooldown);
		mFireBallCooldown = Mathf.Clamp (mFireBallCooldown+Time.deltaTime,0,fireBallCooldown);
		mCommonCooldown = Mathf.Clamp (mCommonCooldown+Time.deltaTime,0,commonCooldown);
	}

	public bool AttackHead()
	{
		if(mAttackCooldown >= attackCooldown && mCommonCooldown >= commonCooldown)
		{
			pm.FsmVariables.FindFsmBool("isAttackHead").Value = true;
			mAttackCooldown = 0;
			mCommonCooldown = 0;
			return true;
		}
		return false;
	}

	public bool AttackSprint()
	{
		if (mSprintCooldown >= sprintCooldown && mCommonCooldown >= commonCooldown) 
		{
			pm.FsmVariables.FindFsmBool ("isAttackSprint").Value = true;
			mSprintCooldown = 0;
			mCommonCooldown = 0;
			return true;
		}
		return false;
	}

	public bool AttackFireBall()
	{
		if(mFireBallCooldown >= fireBallCooldown && mCommonCooldown >= commonCooldown)
		{
			pm.FsmVariables.FindFsmBool("isAttackFireBall").Value = true;
			mFireBallCooldown = 0;
			mCommonCooldown = 0;
			return true;
		}
		return false;
	}

	public bool isInFireBall;
	public float minFireDistance = 10;
	public float maxFireDistance = 50;
	public float fireCooldownDur = 2;
	public GameObject fireBall;
	public GameObject fireExplosion;
	public float firePowerRequire = 0.5f;
	public void FireBall()
	{
		if(!isInFireBall && enemyAttr.currentPower/enemyAttr.maxPower >= firePowerRequire)
		{

			animator.SetBool("is051Fire",true);
			isInFireBall = true;
			enemyAttr.AdjustPowerByPercent(-firePowerRequire);
		}
	}

	public void _FireBall()
	{	
		StartCoroutine ("Fire");
		StartCoroutine ("FireCooldown");
	}

	IEnumerator FireCooldown()
	{
		yield return new WaitForSeconds (fireCooldownDur);
		animator.SetBool("is051Fire",false);
		yield return new WaitForSeconds (fireCooldownDur);
		isInFireBall = false;
	}

	public float fireDelay = 1;
	public float fireSpeed = 10;
	public Vector3 fireTargetPos;
	public void SetFireTargetPos()
	{
		fireTargetPos = playerAttr.transform.position;
		fireTargetPos += (playerAttr.transform.position - transform.position).normalized * 1;
	}

	IEnumerator Fire(){
		Transform head = enemyResources.headTrans;
		if (fireBall == null) 
		{
			fireBall = Resources.Load<GameObject> ("FireBall");
		}
		GameObject go = Instantiate (fireBall,head.position,head.rotation) as GameObject;
		ShootObject so = go.AddComponent<ShootObject> ();
		so.attacker = this;
		SphereCollider coll = go.AddComponent<SphereCollider> ();
		coll.radius = 0.8f;
		coll.isTrigger = true;

		Vector3 targetPos = fireTargetPos + new Vector3(0,1,0);
		Vector3 startPos = go.transform.position;
		float distance = Vector3.Distance (startPos,targetPos);
		float maxT = distance / fireSpeed;
		float t = 0;
		while(t < 1)
		{
			t += Time.deltaTime/maxT;
			go.transform.position = Vector3.Lerp(startPos,targetPos,t);
			yield return null;
		}
		if (fireExplosion == null) {
			fireExplosion = Resources.Load<GameObject>("Fire");	
		}
		Vector3 pos = new Vector3 (go.transform.position.x,0,go.transform.position.z);
		Instantiate (fireExplosion,pos,Quaternion.identity);
		go.SetActive (false);
	}

	public bool isInSprint;
	public float sprintPowRequire = 0.4f;
	void Sprint()
	{
		if(enemyAttr.currentPower / enemyAttr.maxPower > sprintPowRequire && !isInSprint)
		{
			isInSprint = true;
			animator.SetBool("is048bait",true);
			enemyAttr.AdjustPowerByPercent(-sprintPowRequire);
			StartCoroutine ("SprintCooldown");
		}
	}

	IEnumerator SprintCooldown()
	{
		yield return new WaitForSeconds(sprintCooldown);
		animator.SetBool ("is048bait",false);
		isInSprint = false;
	}

	public bool isInHeadAttack;
	public float headAttackCooldown = 0.5f;
	public float headAttackPowRequire = 0.1f;
	void HeadAttack()
	{
		if(enemyAttr.currentPower / enemyAttr.maxPower > headAttackPowRequire && !isInHeadAttack)
		{
			isInHeadAttack = true;
			animator.SetBool("is047bait",true);
			enemyAttr.AdjustPowerByPercent(-headAttackPowRequire);
			StartCoroutine ("HeadAttackCooldown");
		}
	}
	IEnumerator HeadAttackCooldown()
	{
		yield return new WaitForSeconds(headAttackCooldown);
		animator.SetBool ("is047bait",false);
		isInHeadAttack = false;
	}

	public void ResetOtherState(string state)
	{
		foreach (FsmBool fsmBool in fssBool)
		{
			if(fsmBool.Name != state && fsmBool.Name != "isBattle")
			{
				fsmBool.Value = false;
			}
		}
//		animator.SetBool ("isDefense",false);
//		animator.SetInteger ("ActionCMD",0);
//		animator.SetBool ("isAttackX",false);
//		animator.SetBool ("isRoll",false);
//		animator.SetBool ("isUseItem",false);
		animator.speed = 1;
	}

	public override void OnHit(int hitType,float damage,UnitController attacker,bool isRepel = false)
	{
		if (hitType == 0) {
			pm.FsmVariables.FindFsmBool("isInjured").Value = true;
		} else if (hitType == 1) {
			pm.FsmVariables.FindFsmBool("isRepel").Value = true;
		} else if (hitType == 2) {
			pm.FsmVariables.FindFsmBool("isFall").Value = true;
		}
	}

	public void LookAtPlayer()
	{
		if(playerAttr!=null)
		{
			Debug.Log("LookAtPlayer");
			transform.LookAt(playerAttr.transform.position);
		}
	}

}
