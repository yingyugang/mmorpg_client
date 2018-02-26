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

public class CharacterPinchController : MonoBehaviour {

	public enum PinchState {
		none,
		head,
		handLeft,
		handRight,
		body,
		foot,
		head_body,
		head_handLeft,
		head_handRight,
		head_foot,
		body_handLeft,
		body_handRight,
		body_foot,
		foot_handLeft,
		foot_handRight,
		hand_hand,
		cheek_cheek 
	}

	enum AncHandPrior {
		leftHand,
		rightHand
	}
		
	public static string PINCH_POINT_PREFIX = "pinch";
	private const string PINCH_HEAD_NAME = "pinchHeadPoint";
	private const string PINCH_BODY_NAME = "pinchBodyPoint";
	private const string PINCH_HANDLEFT_NAME = "pinchHandLeftPoint";
	private const string PINCH_HANDRIGHT_NAME = "pinchHandRightPoint";
	private const string PINCH_FOOT_NAME = "pinchFootPoint";

	private float groundPos = 0;
	private bool onGround=false;
	private GameObject ground;

	//hand transform substitute - ipute in case begin grasped
	private Transform headAnchor;
	private Transform handLeftAnchor;
	private Transform handRightAnchor;
	private Transform bodyAnchor;
	private Transform footAnchor;

	public Transform RootPoint;//center of gravity should trim by charcter
	public Transform HeadPoint;//head refpoint
	public Transform bone_Character1_LeftArm;
	public Transform bone_Character1_LeftForeArm;
	public Transform bone_Character1_RightArm;
	public Transform bone_Character1_RightForeArm;
	public Transform bone_Character1_LeftUpLeg;
	public Transform bone_Character1_LeftFoot;
	public Transform bone_Character1_RightUpLeg;
	public Transform bone_Character1_RightFoot;

	public Transform bone_Character1_Spine;

	private Vector3 leftArmStartlocalDir;
	private Vector3 rightArmStartlocalDir;
	private Vector3 leftLegStartlocalDir;
	private Vector3 rightLegStartlocalDir;

	private PinchState currentPinchState = PinchState.none;
	private PinchState lastPinchState = PinchState.none;
	private bool pinchStateChanged=false;
	private Vector3 preAnchorPosition;
	private Vector3 prepreAnchorPosition;
	private bool pinchUped=false;
	private float pinchedJudgeCounter;

	private Vector3 ancherBaseDir;
	private Quaternion ancherBaseQ;

	private AncHandPrior handPrior = AncHandPrior.leftHand;

	private const float CharacterSize = 1.0f;
	private float maxSlideOnGround=CharacterSize/4f;

	private float decSinFrameCnt;
	private float decSinFreq = 4;
	private float decSwingElapse;
	private float decSwingTimeOut=3;//sec

	private float armSinframeCnt;
	private float armSignFreq = 12.0f;
	private float armSwingElapse;
	private float armSwingTimeOut=2;//sec
	private float footSinframeCnt;
	private float footSignFreq = 12.0f;
	private float footSwingElapse;
	private float footSwingTimeOut=2;//sec

	private const float stayingJudgeDist = 0.0f;//

	private bool onFootFall=false;
	private float footFallStartY;

	private Rigidbody rigid;
	private bool loadFirst = true;

	private Animator mAnimation;

	void Start () {
		mAnimation = GetComponent<Animator> ();
		rigid = GetComponent<Rigidbody> ();
	}
	
	void Update () {
		if (loadFirst) {
			loadFirst = false;
			initPinchRelated ();
		}
		currentPinchState = getPinchState ();
		pinchStateChanged = (lastPinchState != currentPinchState);

		if (currentPinchState == PinchState.none) none_Update ();
		else if (currentPinchState == PinchState.head) onePoint_Update (headAnchor);
		else if (currentPinchState == PinchState.handLeft)  onePoint_Update (handLeftAnchor);
		else if (currentPinchState == PinchState.handRight) onePoint_Update (handRightAnchor);
		else if (currentPinchState == PinchState.body) onePoint_Update (bodyAnchor);
		else if(currentPinchState == PinchState.foot) onePoint_Update(footAnchor);
		else if(currentPinchState == PinchState.head_handLeft) twoPoint_Update(headAnchor,handLeftAnchor);
		else if(currentPinchState == PinchState.head_handRight) twoPoint_Update(headAnchor,handRightAnchor);
		else if(currentPinchState == PinchState.head_body) twoPoint_Update(headAnchor,bodyAnchor);
		else if(currentPinchState == PinchState.head_foot) twoPoint_Update(headAnchor,footAnchor);
		else if(currentPinchState == PinchState.body_handLeft) twoPoint_Update(bodyAnchor,handLeftAnchor);
		else if(currentPinchState == PinchState.body_handRight) twoPoint_Update(bodyAnchor,handRightAnchor);
		else if(currentPinchState == PinchState.foot_handLeft) twoPoint_Update(footAnchor,handLeftAnchor);
		else if(currentPinchState == PinchState.foot_handRight) twoPoint_Update(footAnchor,handRightAnchor);
		else if(currentPinchState == PinchState.hand_hand) hand_hand_Update();
	}

	void LateUpdate () {

		if (currentPinchState == PinchState.none) none_Update ();
		else if (currentPinchState == PinchState.head) onePoint_LateUpdate (headAnchor, true, true, true);
		else if (currentPinchState == PinchState.handLeft) onePoint_LateUpdate (handLeftAnchor, false, true, true);
		else if (currentPinchState == PinchState.handRight) onePoint_LateUpdate (handRightAnchor, true, false, true);
		else if (currentPinchState == PinchState.body)  onePoint_LateUpdate (bodyAnchor, true, true, true);
		else if(currentPinchState == PinchState.foot) onePoint_LateUpdate(footAnchor,true,true,false);
		else if(currentPinchState == PinchState.head_handLeft) twoPoint_LateUpdate(headAnchor,handLeftAnchor,false,true,true);
		else if(currentPinchState == PinchState.head_handRight) twoPoint_LateUpdate(headAnchor,handRightAnchor,true,false,true);
		else if(currentPinchState == PinchState.body_handLeft) twoPoint_LateUpdate(bodyAnchor,handLeftAnchor,false,true,true);
		else if(currentPinchState == PinchState.body_handRight) twoPoint_LateUpdate(bodyAnchor,handRightAnchor,true,false,true);
		else if(currentPinchState == PinchState.head_foot) twoPoint_LateUpdate(headAnchor,footAnchor,true,true,false);
		else if(currentPinchState == PinchState.foot_handLeft) twoPoint_LateUpdate(footAnchor,handLeftAnchor,false,true,false);
		else if(currentPinchState == PinchState.foot_handRight) twoPoint_LateUpdate(footAnchor,handRightAnchor,true,false,false);
		else if(currentPinchState == PinchState.hand_hand) hand_hand_LateUpdate();

		lastPinchState = currentPinchState;
	}

	public void pinchChanged(string partName,bool pinch,Transform anchor){
		Debug.Log ("pinchChanged:" + partName+":"+pinch.ToString());

		if (partName == PINCH_HEAD_NAME) {
			if (pinch) {
				headAnchor = anchor;
			} else {
				headAnchor = null;
			}
		}else 	if (partName == PINCH_BODY_NAME) {
			if (pinch) {
				bodyAnchor = anchor;
			} else {
				bodyAnchor = null;
			}
		}else if(partName==PINCH_HANDLEFT_NAME){
			if (pinch) {
				handLeftAnchor = anchor;
			} else {
				handLeftAnchor = null;
			}
		}else if(partName==PINCH_HANDRIGHT_NAME){
			if (pinch) {
				handRightAnchor = anchor;
			} else {
				handRightAnchor = null;
			}
		}else if(partName==PINCH_FOOT_NAME){
			if (pinch) {
				footAnchor=anchor;
			} else {
				footAnchor = null;
			}
		}
	}

	public PinchState getPinchState(){

		if (headAnchor != null) {
			if (handLeftAnchor != null) {
				return PinchState.head_handLeft;
			} else if (handRightAnchor != null) {
				return PinchState.head_handRight;
			} else if (bodyAnchor != null) {
				return PinchState.head_body;
			} else if (footAnchor != null) {
				return PinchState.head_foot;
			}
			return PinchState.head;
		} else {
			if (bodyAnchor != null) {
				if (handLeftAnchor != null) {
					return PinchState.body_handLeft;
				} else if (handRightAnchor != null) {
					return PinchState.body_handRight;
				} else if (footAnchor != null) {
					return PinchState.body_foot;
				}
				return PinchState.body;
			} else {
				if (footAnchor != null) {
					if (handLeftAnchor != null) {
						return PinchState.foot_handLeft;
					} else if (handRightAnchor != null) {
						return PinchState.foot_handRight;
					}
					return PinchState.foot;
				} else {
					if (handRightAnchor != null) {
						if (handLeftAnchor != null) {
							return PinchState.hand_hand;
						}
						return PinchState.handRight;
					} else {
						if (handLeftAnchor != null) {
							return PinchState.handLeft;
						} else {
							return PinchState.none;
						}
					}
				}
			}
		}
	}
		
	//====================================================================================
	private void initPinchRelated(){

		ground = GameObject.FindGameObjectWithTag(CommonStatic.GROUND_TAG);
		groundPos = ground.transform.position.y + (ground.transform.transform.localScale.y / 2);

		leftArmStartlocalDir = bone_Character1_LeftArm.InverseTransformPoint (bone_Character1_LeftForeArm.position).normalized;
		rightArmStartlocalDir = bone_Character1_RightArm.InverseTransformPoint (bone_Character1_RightForeArm.position).normalized;

		leftLegStartlocalDir = bone_Character1_LeftUpLeg.InverseTransformPoint (bone_Character1_LeftFoot.position).normalized;
		rightLegStartlocalDir = bone_Character1_RightUpLeg.InverseTransformPoint (bone_Character1_RightFoot.position).normalized;

	}

	//====================================================================================
	private void none_Update (){
		
		if (pinchStateChanged) {
			transform.parent = null;
			if (lastPinchState != PinchState.foot) {
				transform.rotation = Quaternion.identity;
			} else {
				onFootFall = true;
				footFallStartY = transform.position.y;
			}
		}
		mAnimation.enabled = true;
		rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;	
		rigid.useGravity = true;
		rigid.isKinematic = false;

		if (onFootFall) {
			if (((footFallStartY - transform.position.y) > (CharacterSize/2))||(onGround)) {
				transform.rotation = Quaternion.identity;
				onFootFall = false;
			}
		}
			
	}

	private void none_LateUpdate(){

	}

	private void onePoint_Update(Transform ancher){
		if (pinchStateChanged) {
			onePoint_Init (ancher);
		}
	}

	private void onePoint_Init(Transform ancher){
		
		rigid.useGravity = false;
		rigid.isKinematic = true;
		mAnimation.enabled = false;
		ancher.rotation = Quaternion.identity;
		preAnchorPosition = ancher.position;
		prepreAnchorPosition = preAnchorPosition;
		transform.parent = ancher;
		pinchUped = false;

	}

	private void onePoint_LateUpdate(Transform ancher,bool handlefSing, bool handRightSwing, bool footSwing){

		if (ancher == null) {
			return;
		}
			
		if (!onGround) {
			gravityEffect (ancher);
			if (!pinchUped) {
				pinchedJudgeCounter += Time.deltaTime;
				if (pinchedJudgeCounter > 0.5) {
					pinchUped = true;
				}
			}

			if (pinchUped) {

				if (!decrecAmp (ancher)) {
					if ((ancher.position - preAnchorPosition).magnitude <= stayingJudgeDist) {
						if ((prepreAnchorPosition - preAnchorPosition).magnitude > stayingJudgeDist) {
							decSwingElapse = 0f;
							decSinFrameCnt = 0f;
						}
					}
				}
				if (!swingArm (handlefSing, handRightSwing)) {
					if (handlefSing) {
						jointTiredEffect (bone_Character1_LeftArm, leftArmStartlocalDir);
					}
					if (handRightSwing) {
						jointTiredEffect (bone_Character1_RightArm, rightArmStartlocalDir);
					}
				}
				if (footSwing) {
					if (!swingLeg ()) {
						jointTiredEffect (bone_Character1_LeftUpLeg, leftLegStartlocalDir);
						jointTiredEffect (bone_Character1_RightUpLeg, rightLegStartlocalDir);
					}
				}
			}
		} else {
			pinchUped = false;
			pinchedJudgeCounter = 0;
		}

		prepreAnchorPosition = preAnchorPosition;
		preAnchorPosition= ancher.position;
	}



	private void twoPoint_Update(Transform primalAncher, Transform slaveAncher){

		if ((primalAncher == null) || (slaveAncher == null)) {
			return;
		}
		if (pinchStateChanged) {
			rigid.useGravity = false;
			rigid.isKinematic = true;
			ancherBaseDir = (slaveAncher.position - primalAncher.position).normalized;
			ancherBaseQ = primalAncher.rotation;
			transform.parent = primalAncher;
		}
	}
		
	private void twoPoint_LateUpdate(Transform primalAncher, Transform slaveAncher,
		bool handlefSing, bool handRightSwing, bool footSwing){

		if ((primalAncher == null) || (slaveAncher == null)) {
			return;
		}
			
		Vector3 currentDir = (slaveAncher.position - primalAncher.position).normalized;
		Quaternion bodyQ = Quaternion.FromToRotation (ancherBaseDir, currentDir);
		primalAncher.rotation = bodyQ*ancherBaseQ;

		if (!swingArm (handlefSing, handRightSwing)) {
			if (handlefSing) {
				jointTiredEffect (bone_Character1_LeftArm, leftArmStartlocalDir);
			}
			if (handRightSwing) {
				jointTiredEffect (bone_Character1_RightArm, rightArmStartlocalDir);
			}
		}
		if (footSwing) {
			if (!swingLeg ()) {
				jointTiredEffect (bone_Character1_LeftUpLeg, leftLegStartlocalDir);
				jointTiredEffect (bone_Character1_RightUpLeg, rightLegStartlocalDir);
			}
		}

	}
		
	//---------------------------------------------------------------------------
	private void hand_hand_Update(){
		
		if (pinchStateChanged) {
			int v =UnityEngine.Random.Range (0, 10);
			if ((v % 2)==0) {
				handPrior = AncHandPrior.leftHand;
			} else {
				handPrior = AncHandPrior.rightHand;
			}
		}

		if (handPrior == AncHandPrior.leftHand) {
			twoPoint_Update(handLeftAnchor,handRightAnchor);
		} else {
			twoPoint_Update(handRightAnchor,handLeftAnchor);
		}
	}

	private void hand_hand_LateUpdate(){
		if (handPrior == AncHandPrior.leftHand) {
			twoPoint_LateUpdate(handLeftAnchor,handRightAnchor,false,false,true);
		} else {
			twoPoint_LateUpdate(handRightAnchor,handLeftAnchor,false,false,true);
		}
	}

					
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == CommonStatic.GROUND_TAG){
			if (!onGround) {
				//Debug.Log ("onGround");
				onGround = true;
			}		
		}
	}

	void OnCollisionExit(Collision other) {
		if (other.gameObject.tag == CommonStatic.GROUND_TAG){
			if (onGround) {
				//Debug.Log ("remove Ground");
				onGround = false;
			}
		}
	}

	private void gravityEffect(Transform target){
		Vector3 rootDir = (RootPoint.transform.position - target.position).normalized;
		Vector3 gravityDir = new Vector3 (0, -1, 0);
		Quaternion q = Quaternion.FromToRotation (rootDir, gravityDir);
		float z = q.eulerAngles.z;
		float zmod = (z % 360f);
		if (zmod > 180) {
			zmod = -(360 - zmod);
		}
		zmod=zmod * Time.deltaTime*2;//require 0.5sec
		Quaternion qz = Quaternion.AngleAxis (zmod, new Vector3 (0, 0, 1));
		target.rotation =target.rotation*qz;
	}
		
	private void clampOnGround(Transform ancher, Vector3 startPoint, Transform SeekTarget,
		bool handlefSing, bool handRightSwing){

		pinchedJudgeCounter = 0;
		Vector2 startPointXZ = new Vector2 (startPoint.x, startPoint.z);
		Vector2 seekPointXZ = new Vector2 (SeekTarget.transform.position.x, SeekTarget.transform.position.z);
		float slideDist = (startPointXZ - seekPointXZ).magnitude;
	
		if (slideDist < maxSlideOnGround) {
			Vector3 toStartPointDir = (ancher.position-startPoint).normalized;
			Vector3 toCurrentDir =    (ancher.position-SeekTarget.position).normalized;
			Quaternion q = Quaternion.FromToRotation (toCurrentDir,toStartPointDir);
			ancher.transform.rotation =ancher.transform.rotation* q;
		}
			
	}

	private bool decrecAmp(Transform target){
		bool ret = true;
		float Amp = 0;
		Quaternion q = Quaternion.identity;
		decSwingElapse+= Time.deltaTime;
		if (decSwingElapse < decSwingTimeOut) {
			decSinFrameCnt += Time.deltaTime + 1000;
			float decrec = 1000f / decSinFrameCnt;
			float bodyFreqTime = decSinFreq * (decSinFrameCnt++ % 200) / 199.0f;
			Amp = decrec * Mathf.Sin ((float)(2.0f * Mathf.PI * bodyFreqTime));
		} else {
			ret= false;
		}
		q = Quaternion.AngleAxis (30f * Amp, new Vector3 (0, 0, 1));
		target.rotation = target.rotation * q;
		return ret;
	}
		
	private bool swingArm(bool doleftHand,bool doRightHand){
		armSwingElapse+= Time.deltaTime;
		if (armSwingElapse>armSwingTimeOut) {
			return false;
		}
		float signAmp = Mathf.Sin ((float)(2.0f * Mathf.PI * armSignFreq * (armSinframeCnt++ % 200) / 199.0f));
		if (armSinframeCnt > 10000) {
			armSinframeCnt = 0;
		}
		Quaternion handLeftQue = Quaternion.AngleAxis (54f*signAmp, new Vector3 (0, 1, 0));
		Quaternion handRightQue = Quaternion.AngleAxis (-54f*signAmp, new Vector3 (0, 1, 0));
		if(doleftHand) bone_Character1_LeftArm.localRotation = handLeftQue;
		if(doRightHand)bone_Character1_RightArm.localRotation = handRightQue;
		return true;
	}

	private bool swingLeg(){
		footSwingElapse+= Time.deltaTime;
		if (footSwingElapse>footSwingTimeOut) {
			return false;
		}
		float signAmp = Mathf.Sin ((float)(2.0f * Mathf.PI * footSignFreq * (footSinframeCnt++ % 200) / 199.0f));
		if (footSinframeCnt > 10000) {
			footSinframeCnt = 0;
		}
		Quaternion footLeftQue = Quaternion.AngleAxis (24f*signAmp, new Vector3 (1, 0, 0));
		Quaternion footRightQue = Quaternion.AngleAxis (-24f*signAmp, new Vector3 (1, 0, 0));//-にすれば交互
		bone_Character1_LeftUpLeg.localRotation = footLeftQue;
		bone_Character1_RightUpLeg.localRotation = footRightQue;
		return true;
	}

	//====================================================================================
	private void jointTiredEffect(Transform taggetBone,Vector3 localDirection){

		Vector3 gravityTarget= new Vector3(transform.position.x,groundPos,transform.position.z);
		Vector3 gravTargetDir = taggetBone.InverseTransformPoint (gravityTarget).normalized;

		Quaternion q = Quaternion.FromToRotation (localDirection, gravTargetDir);
		bool dig = (q.eulerAngles.z > 180);

		if (taggetBone == bone_Character1_LeftArm) {
			if (dig) {
				moveLeftArmZ (false);
			} else {
				moveLeftArmZ (true);
			}
		} else if (taggetBone == bone_Character1_RightArm) {
	
			if (dig) {
				moveRightArmZ (false);
			} else {
				moveRightArmZ (true);
			}

		} else if (taggetBone == bone_Character1_LeftUpLeg) {
			if (dig) {
				moveLeftLegZ (false); 
			} else {
				moveLeftLegZ (true); 
			}

		} else if (taggetBone == bone_Character1_RightUpLeg) {
			if (dig) {
				moveRightLegZ (false);
			} else {
				moveRightLegZ (true);
			}

		}
			
	}

	public void moveLeftLegZ(bool inside){
		Quaternion q=bone_Character1_LeftUpLeg.localRotation;
		float flg = (inside) ? 1 : -1;
		Quaternion m = Quaternion.AngleAxis(flg*2f,new Vector3(0,0,1f));

		Quaternion mq = bone_Character1_LeftUpLeg.localRotation * m;
		float z = mq.eulerAngles.z;

		if (((0f <= z) && (z < 34.0f)) || ((340f <= z) && (z <= 360f))) {
		} else {
			return;
		}
		bone_Character1_LeftUpLeg.localRotation = mq;
	}

	public void moveRightLegZ(bool inside){  //out 0-34  //in 360-340
		Quaternion q=bone_Character1_RightUpLeg.localRotation;
		float flg = (inside) ? 1 : -1;
		Quaternion m = Quaternion.AngleAxis(flg*2f,new Vector3(0,0,1f));
		Quaternion mq = bone_Character1_RightUpLeg.localRotation * m;
		float z = mq.eulerAngles.z;

		if (((0f <= z) && (z < 44.0f)) || ((340f <= z) && (z <= 360f))) {
		} else {
			return;
		}
		bone_Character1_RightUpLeg.localRotation = mq;

	}

	public void moveLeftArmZ(bool inside){//inv true down 0-44 up 360-290
		Quaternion q=bone_Character1_LeftArm.localRotation;
		float flg = (inside) ? 1 : -1;
		Quaternion m = Quaternion.AngleAxis(flg*2f,new Vector3(0,0,1f));
		Quaternion mq = bone_Character1_LeftArm.localRotation * m;
		float z = mq.eulerAngles.z;
		if (((0f <= z) && (z < 44.0f)) || ((290f <= z) && (z <= 360f))) {
		} else {
			return;
		}
		bone_Character1_LeftArm.localRotation = mq;

	}

	public void moveRightArmZ(bool inside){//inv true->down  360-333 up 0-68
		Quaternion q=bone_Character1_RightArm.localRotation;
		float flg = (inside) ? 1 : -1;
		Quaternion m = Quaternion.AngleAxis(flg*2f,new Vector3(0,0,1f));
		Quaternion mq = bone_Character1_RightArm.localRotation * m;
		float z = mq.eulerAngles.z;
		if (((0f <= z) && (z < 68.0f)) || ((333f <= z) && (z <= 360f))) {
		} else {
			return;
		}
		bone_Character1_RightArm.localRotation =mq;
	}

}
