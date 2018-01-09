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

public class CharacterFaceController : MonoBehaviour {


	public SkinnedMeshRenderer faceMeshRender;
	public SkinnedMeshRenderer matugeRender;
	private float winkValue = 0f;
	private float winkElapse=0f;
	private float winkSpeed=2f;//normal term 1s
	private float onWinkWaitElapse=0;
	private float onWinkWaitTimeOut=2.0f;
	private bool onWinkWait;

	private float mouthValue = 0f;
	private float speakElapse=0f;
	private float speakSpeed=1f;//normal term 1s
	private float onSpeakWaitElapse=0;
	private float onSpeakWaitTimeOut=0.5f;
	private bool onSpeakWait;


	void Start () {
	}
	
	void Update () {

	}
	void LateUpdate () {

	}

	public void wink(){

		if (onWinkWait) {
			onWinkWaitElapse += Time.deltaTime;
			if (onWinkWaitElapse > onWinkWaitTimeOut) {
				onWinkWaitElapse = 0;
				onWinkWait = false;
			}
			return;
		}

		winkElapse += Time.deltaTime;
		if (winkElapse < (1/(2*winkSpeed))) {
			winkValue += Time.deltaTime * winkSpeed*2;
		} else {
			winkValue -= Time.deltaTime * winkSpeed*2;
		}
		if ((winkElapse > 1)||(winkElapse <0)) {
			onWinkWait = true;
			winkValue = 0;
			winkElapse = 0;
		}
		setWink (winkValue);
	}

	private void setWink(float v){
		if (v > 1) {
			v = 1;
		} else if (v < 0) {
			v = 0;
		}
		faceMeshRender.SetBlendShapeWeight(0,v*100f);
		matugeRender.SetBlendShapeWeight(0, v*100f);
	}

	public void openEye(){
		onWinkWait = false;
		winkElapse = 0;
		setWink (0);

	}

	public void closeEye(){
		onWinkWait = false;
		winkElapse = 0;
		setWink (1);

	}


	public void speak(){

		if (onSpeakWait) {
			onSpeakWaitElapse += Time.deltaTime;
			if (onSpeakWaitElapse > onSpeakWaitTimeOut) {
				onSpeakWaitElapse = 0;
				onSpeakWait = false;
			}
			return;
		}

		speakElapse += Time.deltaTime;
		if (speakElapse < (1/(2*speakSpeed))) {
			mouthValue += Time.deltaTime * speakSpeed*2;
		} else {
			mouthValue -= Time.deltaTime * speakSpeed*2;
		}
		if ((mouthValue > 1)||(mouthValue <0)){
			onSpeakWait = true;
			speakElapse = 0;
			mouthValue = 0;
		}
		setMouth (mouthValue);
	}

	private void setMouth(float v){
		if (v > 1) {
			v = 1;
		} else if (v < 0) {
			v = 0;
		}
		faceMeshRender.SetBlendShapeWeight(1,v*100f);
	}
		
	private void openMouth(){
		onSpeakWait = false;
		speakElapse = 0;
		setMouth (1);
	}

	private void closeMouth(){
		onSpeakWait = false;
		speakElapse = 0;
		setMouth (0);
	}


}
