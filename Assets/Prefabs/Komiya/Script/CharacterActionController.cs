//
//  ReactionCharacterController
//  Created by Yoshinao Izumi on 2017/04/19.
//  Copyright © 2017 Yoshinao Izumi All rights reserved.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class CharacterActionController : MonoBehaviour {

	public GameObject arrow;
	private GameObject arrowObj;
	private float arrowPower = 40f;
	private bool onBowForm=false;
	private rightArrowPoint rightHand;
	private Quaternion boneBodyQ;
	private bool onBoneAction = false;
	private bool onControlBowAction = false;

	private bool plullsetA = false;
	private bool plullsetB = false;

	protected static int BodyAnimationLayor = 0;
	protected AnimatorStateInfo currentBaseState;	

	// base Action --------------------------------------------------------------------
	static int StandingState = Animator.StringToHash ("Base Layer.Standing@loop");
	static int WalkingState = Animator.StringToHash ("Base Layer.Walking@loop");
	static int HeadPinchUpState = Animator.StringToHash ("Base Layer.headPinchi");
	static int JanpingState = Animator.StringToHash ("Base Layer.Jumping");
	static int BackFlipState = Animator.StringToHash ("Base Layer.backFlip");

	static int BowStartState = Animator.StringToHash ("Base Layer.BowStart");
	private bool startThrowWeight = false;
	private float bowTimeElapse=0f;
	private float bowTimeUp = 2.0f;

	static int RunningState = Animator.StringToHash ("Base Layer.Running@loop");
	static int FrontFlipState = Animator.StringToHash ("Base Layer.FrontFlip");
	static int PickUpDownState = Animator.StringToHash ("Base Layer.PickUpDown");
	static int PickUpUpState = Animator.StringToHash ("Base Layer.PickUpUp");

	private bool onJump=false;
	private bool onBackFlick=false;


	protected static int RootAnimationBaseLayor = 0;

	private const float CharacterSize = 1.0f;
	private float maxSlideOnGround=CharacterSize/4f;

	private Animator animator;
	private Rigidbody rigid;
	private bool loadFirst = true;

	// for agent0
	private float walkSpeed=0.2f;
	private bool onTurn=false;
	private bool onTurnInit=false;
	private float onTurnFlg=1.0f;

	public AudioClip audioShot;
	private AudioSource mAudio;

	void Start () {
		animator = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody> ();
		mAudio = GetComponent<AudioSource> ();
	}
	
	void Update () {
		if (loadFirst) {
			loadFirst = false;
		}

		animator.SetBool ("idle", false);
		animator.SetBool ("walk", false);
		animator.SetBool ("headPinchi", false);

		currentBaseState = animator.GetCurrentAnimatorStateInfo (BodyAnimationLayor);

		if (onAnimationProc ()) {
			return;
		}

		if (updateBowState ()) {
			return;
		}

	}

	void LateUpdate () {

		if (onAnimationProc ()) {
			return;
		}

		if (LateUpdateBowState ()) {
			return;
		}
	}
		
	public void backFlipExecute(){
		if (animator != null) {
			onBackFlick = true;
			transform.position = new Vector3 (transform.position.x, transform.position.y - 0.1f, transform.position.z);
			animator.SetTrigger ("backFlip");
		}
	}

	public void onBackflipRecForce(){
		rigid.AddForce (new Vector3 (-1*transform.forward.x*2.5f,3.8f,-1*transform.forward.z*2.5f),
			ForceMode.Impulse);
	}

	public void jumpExecute(){
		if (animator != null) {
			onJump = true;
			animator.SetTrigger ("jump");
		}
	}

	public void jumpStep1(){
	}

	public void jumpStep2(){
	}

	private bool onAnimationProc(){
		if (onJump) {
			return true;
		}
		if (onBackFlick) {
			return true;
		}
		return false;
	}

	public void endJump(){
		onJump = false;
	}

	public void endBackFlip(){
		onBackFlick = false;
	}


	private bool updateBowState(){
		if (startThrowWeight) {
			bowTimeElapse += Time.deltaTime;
			if (bowTimeElapse > bowTimeUp) {
				startThrowWeight = false;
				bowTimeElapse = 0;
				setBowAction(false);
			}
		}

		return  plullsetA || plullsetB ||onBowForm;
	}

	private bool LateUpdateBowState(){ 
		if (onBoneAction) {
			//bone_Character1_Spine.localRotation= bodyQ;
		}
		return  plullsetA || plullsetB ||onBowForm;
	}

	public bool startBowAction(){

		Vector3 targetPos= GameObject.FindWithTag ("Player").transform.position;
		transform.rotation = Quaternion.LookRotation (targetPos);
		Quaternion q =  Quaternion.AngleAxis(-180f,new Vector3 (0, 1, 0));
		transform.rotation = transform.rotation * q;
		onBoneAction = true;
		if (!onBowForm) {
			transform.Rotate (new Vector3 (0, 1, 0), -90f);
			onBowForm = true;
		}
		animator.SetTrigger ("BowStart");
		return true;
	}

	private IEnumerator waitPullend() {
		yield return new WaitForSeconds(2);
	}

	protected void throwBowAction(){
		if (plullsetA && plullsetB) {
		} else{
			return;
		}
		if (animator.GetCurrentAnimatorStateInfo (BodyAnimationLayor).fullPathHash
			!= BowStartState) {
			return;
		}
		BowActionScript bow = GetComponentInChildren<BowActionScript> (true);
		bow.setFloat (0.0f);
		animator.SetBool ("Next", true);
		if (arrowObj != null) {
			rightHand.removeChild (arrowObj);
			Rigidbody arrig = arrowObj.GetComponent<Rigidbody> ();
			Vector3 direction;

			float rd = 1.0f+((float)UnityEngine.Random.Range (-1, 3)) / 10.0f;
			arrowPower = 26f* rd;
			float xr = (float)UnityEngine.Random.Range (-3, 3)/10.0f;
			direction = new Vector3 (xr, 0.2f, 1);//direction = new Vector3 (xr, 0.5f, 1);


			arrig.isKinematic = false;
			arrig.useGravity = true;
			arrig.AddForce (direction * arrowPower, ForceMode.Impulse);

			arrowObj.GetComponentInChildren<ArrowToched> ().startParticle ();
		}
		mAudio.PlayOneShot (audioShot);

		plullsetA = false;
		plullsetB = false;

		Invoke("ExitBack", 1.0f);
	}

	public void startBowPull(){
		rigid.isKinematic = true;
		rightHand =(GetComponentInChildren<rightArrowPoint> (true));
		rightHand.transform.localRotation = Quaternion.identity;
		arrowObj = Instantiate (arrow,rightHand.getPosition(),transform.rotation) as GameObject;

		Rigidbody arrig= arrowObj.GetComponent<Rigidbody> ();
		arrig.isKinematic = true;
		arrig.useGravity = false;
		BowActionScript bow = GetComponentInChildren<BowActionScript> (true);
		bow.setFloat (1.0f);
		bowTimeElapse = 0;
		startThrowWeight = true;
		rightHand.addChild(arrowObj);
		plullsetA = true;
	}

	public void endBowPull(){
		plullsetB = true;
	}

	public void endPowThrow(){
		transform.Rotate (new Vector3 (0, 1, 0), 90);
		onBowForm = false;
		Vector3 targetPos= new Vector3(100,0,0);
		transform.rotation = Quaternion.LookRotation (targetPos);
		animator.SetBool ("Next", false);
	}


	public void setBowAction(bool onset){ 
		onControlBowAction = onset;
		if (onset) {
			if (startBowAction ()) {
				onControlBowAction = true;
			} else {
				onControlBowAction = false;
			}
		} else {
			throwBowAction ();
		}
	}

	private void freeWalk(){

		Vector3 fowardVec=transform.rotation.eulerAngles;
		Vector3 seekPois = transform.position
			+ (new Vector3 (fowardVec.x * walkSpeed, transform.position.y - 0.02f, fowardVec.y * walkSpeed));

		Vector3 direction = new Vector3 (fowardVec.x, 0, fowardVec.z);

		if (!canGoForward (seekPois)) {
			onTurn = true;
			if (onTurnInit) {
				onTurnInit = false;
				if (Random.value < 0.5f) {
					onTurnFlg = 1.0f;
				} else {
					onTurnFlg = -1.0f;
				}
			}
		} else {
			onTurn = false;
			onTurnInit = true;
		}

		if (onTurn) {
			transform.rotation =transform.rotation*Quaternion.AngleAxis (onTurnFlg * 10f, new Vector3 (0, 1f, 0));
		} else {
			transform.rotation = Quaternion.LookRotation (direction);
			transform.localPosition += transform.forward * walkSpeed * Time.fixedDeltaTime; 
		}

	}

	private bool canGoForward(Vector3 seekPois){
		//ground check
		bool cango = false;
		Collider[] edges = Physics.OverlapSphere (seekPois, 0.01f, -1, QueryTriggerInteraction.Collide);
		for (int i = 0; i < edges.Length; i++) {
			if (edges [i].gameObject.tag == CommonStatic.GROUND_TAG) {
				cango = true;
				break;
			}
		}

		//		Collider[] objes = Physics.OverlapSphere (seekPois, 0.1f, -1, QueryTriggerInteraction.Collide);
		//		for (int i = 0; i < objes.Length; i++) {
		//			if ((objes [i].gameObject.tag == CommonStatic.T1) ||
		//				(objes [i].gameObject.tag == CommonStatic.T2) ||
		//				(objes [i].gameObject.tag == CommonStatic.T3)) {
		//				entryEny = true;
		//				break;
		//			}
		//		}


		return cango;
	}

}
