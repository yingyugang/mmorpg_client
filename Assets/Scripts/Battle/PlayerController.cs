using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using DG.Tweening;

public class PlayerController : UnitController
{

	const string IdleState = "Base Layer.Idle";
	const string IdleStateFree = "Base Layer.Idle Free";
	const string RollState = "Base Layer.Roll";
	const string RollStateFree = "Base Layer.RollFree";
	const string AttackX = "Base Layer.AttackX";
	const string AttackA = "Base Layer.AttackA";
	const string AttackAA = "Base Layer.AttackAA";
	const string UseItemState = "Base Layer.UseItem";

	// 动画状态机参数Key
	const string ActionCMD = "ActionCMD";

	public BaseAttribute playerAttr;
	public UnitResources playerResource;

	Vector3 moveDirection;
	public bool IsCanAttack = false;
	public GameObject enemy;

	public float runningPowerRequire = 50;
	public float rollPowerRequire = 200;

	ETCJoystick joystick;
	GameObject mControllObject;

	protected override void Awake ()
	{
		base.Awake ();
		GameObject go = Instantiate (Resources.Load<GameObject> ("Controller"));
		mControllObject = go;
		joystick = go.GetComponentInChildren<ETCJoystick> (true);
		joystick.onMove.AddListener (this.On_JoystickMove);
		mControllObject.SetActive (false);
	}

	void Start ()
	{
		playerAttr = GetComponent<BaseAttribute> ();
		playerResource = GetComponent<UnitResources> ();
		ShowWeapon (0);
	}
	
	//Animation Events 调用此脚本，并返回hit值，判断值改变IsCanAttack状态。
	void Hit (int hit)
	{
		if (hit == 0) {
			IsCanAttack = false;
		} else if (hit == 1) {
			IsCanAttack = true;
		}

	}

	AnimatorStateInfo stateInfo;
	float h;
	float v;

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.X) || ETCInput.GetButtonDown ("btn_x")) {
			BtnX ();
		}
		if (Input.GetKeyUp (KeyCode.Z) || ETCInput.GetButtonDown ("btn_a")) { 
			BtnA (); 
		} 
		if (Input.GetKeyDown (KeyCode.B) || ETCInput.GetButtonDown ("btn_b")) {
			BtnB ();
		} 
		if (Input.GetKeyDown (KeyCode.Y) || ETCInput.GetButtonDown ("btn_y")) {
			BtnY ();
		}

		if (Input.GetKeyDown (KeyCode.R) || ETCInput.GetButtonDown ("btn_r")) {
			BtnR ();
		}
		if (Input.GetKeyUp (KeyCode.R) || ETCInput.GetButtonUp ("btn_r")) {
			BtnRRelease ();
		}

		if (playerAttr.currentHealth <= 0) {
			BattleController.SingleTon ().PlayPlayerDeathAnim ();
			pm.SendEvent ("OnDead");
			enabled = false;
		}

//		
//		foreach(EasyButton eb in easyButtons)
//		{
//			if(eb.name == "Btn_R")
//			{
//				if(eb.buttonState == EasyButton.ButtonState.None && pm.ActiveStateName == "Defense")
//				{
//					pm.FsmVariables.FindFsmBool("isDefense").Value = false;
//				}
//			}
//		}

		OnMove (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
//		stateInfo = this.animator.GetCurrentAnimatorStateInfo (0); 
//		h = Input.GetAxis ("Horizontal");
//		v = Input.GetAxis ("Vertical");
//		animator.SetFloat ("Volx", h * 2, 0.1f, Time.deltaTime);
//		animator.SetFloat ("Voly", v * 2, 0.1f, Time.deltaTime);
		
//		//连续3段攻击
//		if (!stateInfo.IsName (IdleState) && !stateInfo.IsName (IdleStateFree)) {  
//			// 每次设置完参数之后，都应该在下一帧开始时将参数设置清空，避免连续切换  
//			animator.SetInteger (ActionCMD, 0);
////			animator.SetBool ("X", false);
//			animator.SetBool ("Roll", false);
//
//		} else if (stateInfo.IsName (IdleState) && v != 0 || h != 0) {  
//				
//			moveDirection = new Vector3 (h, 0, v);
//			moveDirection.Normalize ();
//			transform.forward = Vector3.Slerp (transform.forward, moveDirection, 0.1f);
//			//改变转向,Slerp球形插值在两个向量之间.0.07f是转向速度
//		}		
//
//		if (stateInfo.IsName (AttackA) && (stateInfo.normalizedTime > 0.54f) && (this.curComboCount == 2)) {  
//			// 当在攻击1状态下，并且当前状态运行了0.54正交化时间（即动作时长的54%），并且用户在攻击1状态下又按下了“攻击键”  
//			animator.SetInteger (ActionCMD, 1);  
//		}  
//		if (stateInfo.IsName (AttackAA) && (stateInfo.normalizedTime > 0.6f) && (this.curComboCount == 3)) {  
//			// 当在攻击2状态下（同理攻击1状态）  
//			animator.SetInteger (ActionCMD, 1);  
//		} 
//		StateMachine ();
	}

	//	void StateMachine()
	//	{
	//		if(previousStatus!=status)
	//		{
	//			switch(previousStatus)
	//			{
	//			case PlayerState.IdleFree:break;
	//			case PlayerState.Idle:break;
	//			case PlayerState.Run:OnOutRun();break;
	//			case PlayerState.Roll:OnRollOut();break;
	//			case PlayerState.RollFree:OnRollFreeOut();break;
	//			case PlayerState.PowerLock:OnPowerLockOut();break;
	//			case PlayerState.Defense:OnDefenseOut();break;
	//			case PlayerState.Other:break;
	//			}
	//			previousStatus=status;
	//		}
	//
	//		switch(status)
	//		{
	//		case PlayerState.IdleFree:OnIdleFree();break;
	//		case PlayerState.Idle:OnIdle();break;
	//		case PlayerState.Run:OnRun();break;
	//		case PlayerState.Roll:OnRoll();break;
	//		case PlayerState.RollFree:OnRollFree();break;
	//		case PlayerState.PowerLock:OnPowerLock();break;
	//		case PlayerState.Defense:OnDefense();break;
	//		case PlayerState.Collect:OnCollect();break;
	//		case PlayerState.Other:break;
	//		}
	//	}



	void Attack ()
	{  
		AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo (0);
		if (pm.FsmVariables.FindFsmBool ("isAttackAA").Value && pm.ActiveStateName == "AttackAA" && animatorStateInfo.IsName (AttackAA)) {
			pm.FsmVariables.FindFsmBool ("isAttackAAA").Value = true;
		} else if (pm.FsmVariables.FindFsmBool ("isAttackA").Value && animatorStateInfo.IsName (AttackA)) {
			pm.FsmVariables.FindFsmBool ("isAttackAA").Value = true;
		} else if (!pm.FsmVariables.FindFsmBool ("isAttackA").Value && pm.ActiveStateName == "Idle") {
			pm.FsmVariables.FindFsmBool ("isAttackA").Value = true;
		}
//		if (stateInfo.IsName (IdleState)) { 
//			
//			// 在待命状态下，按下攻击键，进入攻击1状态，并记录连击数为1  
//			this.animator.SetInteger (ActionCMD, 1); 
//			//延时调用两个方法，分别是攻击判定的生效和失效。
//			//Invoke ("AttackStart", 1.16f);
//			//Invoke ("AttackStop", 1.33f);
//			this.curComboCount = 1;  
//		} else if (stateInfo.IsName (AttackA)) { 
//			
//			// 在攻击1状态下，按下攻击键，记录连击数为2（切换状态在Update()中）  
//			this.curComboCount = 2;  
//		} else if (stateInfo.IsName (AttackAA)) {  
//			
//			// 在攻击2状态下，按下攻击键，记录连击数为3（切换状态在Update()中）  
//			this.curComboCount = 3;  
//		} 
	}

	void OnEnable ()
	{
		
//		EasyJoystick.On_JoystickMove += On_JoystickMove;	
		//EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
		//EasyButton.On_ButtonPress += On_ButtonPress;
		//EasyButton.On_ButtonUp += On_ButtonUp;	
		//EasyButton.On_ButtonDown += On_ButtonDown;
	}

	void OnDisable ()
	{
//		EasyJoystick.On_JoystickMove -= On_JoystickMove;	
	}

	public float moveSpeed = 2;

	void On_JoystickMove (Vector2 move)
	{
		OnMove (move.x, move.y);
	}

	void OnMove (float x, float y)
	{
		if (pm.Fsm.ActiveStateName == "Idle" || pm.Fsm.ActiveStateName == "IdleFree" || pm.Fsm.ActiveStateName == "Run") {
			Vector3 dirction;
			if (CameraController.SingleTon ().battlePerspective == BattlePerspective.AroundBoss) {
				dirction = enemy.transform.position - playerAttr.transform.position;
				dirction = Vector3.Normalize (new Vector3 (dirction.x, 0, dirction.z));
				moveDirection = dirction * y;
				moveDirection = moveDirection + Vector3.Normalize (Vector3.Cross (dirction, new Vector3 (0, -1, 0))) * x;
			} else if (CameraController.SingleTon ().battlePerspective == BattlePerspective.StayBehind) {
				dirction = transform.forward;
				moveDirection = dirction * Mathf.Max (0, y);
				moveDirection = moveDirection + Vector3.Normalize (Vector3.Cross (dirction, new Vector3 (0, -1, 0))) * x;
			}

			this.transform.forward = Vector3.Slerp (transform.forward, moveDirection, 0.07f);
			animator.SetFloat ("Volx", x, 0.1f, Time.deltaTime);
			animator.SetFloat ("Voly", y, 0.1f, Time.deltaTime);
		}
	}


	public float collectDistance = 2;
	public float collectAngle = 60;
	public Collection currentCollection;

	public void Collect ()
	{
		List<Collection> collections = BattleController.SingleTon ().collections;
		float minDistance = Mathf.Infinity;
		Collection nearestCollection = null;
		if (collections != null && collections.Count > 0) {
			foreach (Collection c in collections) {
				float direction = CommonUtility.GetDirection (transform.forward, c.transform.position - transform.position);
				float distance = Vector3.Distance (transform.position, c.transform.position);
				if (direction >= 0 && distance <= 1) {
					if (minDistance > distance) {
						minDistance = distance;
						nearestCollection = c;
					}
				}
			}
			if (nearestCollection == null) {
				foreach (Collection c in collections) {
					float direction = CommonUtility.GetDirection (transform.forward, c.transform.position - transform.position);
					float distance = Vector3.Distance (transform.position, c.transform.position);
					if (direction >= Mathf.Cos (collectAngle * Mathf.PI / 180) && distance <= collectDistance) {
						if (minDistance > distance) {
							minDistance = distance;
							nearestCollection = c;
						}
					}

				}	
			}
			currentCollection = nearestCollection;
			if (currentCollection != null) {
				transform.LookAt (currentCollection.transform);
				pm.FsmVariables.FindFsmBool ("isCollect").Value = true;
			}
		}
	}

	public void UseItem ()
	{

	}

	#region Btn Controll

	public void ShowControllBtns(){
		mControllObject.SetActive (true);
		mControllObject.GetComponentInChildren<CanvasGroup> (true).alpha = 0;
		mControllObject.GetComponentInChildren<CanvasGroup> (true).DOFade (1,1f);
	}

	void BtnY ()
	{
		if (!CheckControllAble ())
			return;
//		if (pm.ActiveStateName == "Idle") {
//			pm.FsmVariables.FindFsmBool ("isBattle").Value = false;
//
		if (pm.ActiveStateName == "Idle") {
			pm.FsmVariables.FindFsmBool ("isAttackX").Value = true;
		} else if (pm.ActiveStateName == "IdleFree") {
			pm.FsmVariables.FindFsmBool ("isUseItem").Value = true;
		}
	}

	void BtnA ()
	{
		if (!CheckControllAble ())
			return;
		if (pm.FsmVariables.FindFsmBool ("isBattle").Value && !(pm.ActiveStateName == "AttackX" || pm.ActiveStateName == "Roll" || pm.ActiveStateName == "Defense")) {
			Attack ();
		} else if (pm.ActiveStateName == "IdleFree" || pm.ActiveStateName == "Run") {
			Collect ();
		}
	}

	public float rollCooldownDur = 1.0f;

	void BtnB ()
	{
		if (!CheckControllAble ())
			return;
		if (!pm.FsmVariables.FindFsmBool ("isRoll").Value && (pm.ActiveStateName == "Idle" || pm.ActiveStateName == "IdleFree" || pm.ActiveStateName == "Run"))
			pm.FsmVariables.FindFsmBool ("isRoll").Value = true;
	}

	public bool isBtnXPressed = false;
	//拔刀and收刀
	void BtnX ()
	{
		if (!CheckControllAble ())
			return;

		if (pm.ActiveStateName == "IdleFree" || pm.ActiveStateName == "Run") {
			pm.FsmVariables.FindFsmBool ("isBattle").Value = true;
		} else if (pm.ActiveStateName == "Idle") {
			pm.FsmVariables.FindFsmBool ("isBattle").Value = false;
		} else if (pm.ActiveStateName == "Run") {
			pm.FsmVariables.FindFsmBool ("isRun").Value = false;
		}
		return;

		if (pm.ActiveStateName == "Run") {
			pm.FsmVariables.FindFsmBool ("isRun").Value = false;
		}
		if (pm.ActiveStateName == "Idle") {
			pm.FsmVariables.FindFsmBool ("isAttackX").Value = true;
		} else if (pm.ActiveStateName == "IdleFree" || pm.ActiveStateName == "Run") {
			pm.FsmVariables.FindFsmBool ("isBattle").Value = true;
		}




	}

	void BtnR ()
	{
		if (!CheckControllAble ())
			return;
		if (pm.ActiveStateName == "Idle") {
			pm.FsmVariables.FindFsmBool ("isDefense").Value = true;
		} else if (pm.ActiveStateName == "IdleFree") {
			pm.FsmVariables.FindFsmBool ("isRun").Value = true;
		}
	}

	void BtnRPress ()
	{
//		Debug.Log ("BtnRPress");
	}

	void BtnRRelease ()
	{
		Debug.Log ("BtnRRelease");
		if (pm.ActiveStateName == "Defense") {
			pm.FsmVariables.FindFsmBool ("isDefense").Value = false;
		} else {
			pm.FsmVariables.FindFsmBool ("isRun").Value = false;
		}
	}

	#endregion

	#region Change Weapon

	public GameObject weapon0;
	public GameObject weapon1;
	public GameObject weapon2;

	public void ShowWeapon (int index)
	{
		weapon0.SetActive (false);
		weapon1.SetActive (false);
		weapon2.SetActive (false);
		switch (index) {
		case 0:
			weapon0.SetActive (true);
			break;
		case 1:
			weapon1.SetActive (true);
			break;
		case 2:
			weapon2.SetActive (true);
			break;
		default:
			break;
		}
	}

	#endregion

	public override void OnHit (int hitType, float damage, UnitController attacker, bool isRepel = false)
	{
		if (pm.ActiveStateName == "Defense" && CommonUtility.GetDirection (transform, attacker.transform) < 0) {
			BattleController.SingleTon ().spawnManager.SpawnDefensePrefab (playerResource.rightHandTrans.position, Quaternion.identity);
		} else {
			if (hitType == 1) {
				if (CheckControllAble ())//确保没有在几种受击状态中的任何一个
					pm.FsmVariables.FindFsmBool ("isLie").Value = true;
				playerAttr.AdjustHealthByPercent (damage);
			} else if (hitType == 0) {
				if (CheckControllAble ())
					pm.FsmVariables.FindFsmBool ("isFall").Value = true;
				playerAttr.AdjustHealthByPercent (damage);
			} else if (hitType == 2) {
				if (CheckControllAble ())
					pm.FsmVariables.FindFsmBool ("isInjured").Value = true;
				playerAttr.AdjustHealthByPercent (damage);
			}
		}
	}

	protected override bool CheckControllAble ()
	{
		string currentState = pm.ActiveStateName;
		if (currentState == "Injured" || currentState == "Repel" || currentState == "Fall") {
			return false;
		}
		return true;
	}

	public void ResetOtherState (string state)
	{
		foreach (FsmBool fsmBool in fssBool) {
			if (fsmBool.Name != state && fsmBool.Name != "isBattle") {
				fsmBool.Value = false;
			}
		}
		animator.SetBool ("isDefense", false);
		animator.SetInteger ("ActionCMD", 0);
		animator.SetBool ("isAttackX", false);
		animator.SetBool ("isRoll", false);
		animator.SetBool ("isUseItem", false);
		animator.SetBool ("isCollect", false);
		animator.speed = 1;
	}

	IEnumerator ResetState (string state)
	{
		yield return null;
		animator.SetBool (state, false);
	}

}

