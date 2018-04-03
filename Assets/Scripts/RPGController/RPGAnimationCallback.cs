using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGAnimationCallback : MonoBehaviour {

	Animator mAnimator;
	void Awake(){
		mAnimator = GetComponent<Animator> ();
	}

	public void ResetCount(){
		Debug.Log ("ResetCount");
		mAnimator.SetInteger ("Count",0);
	}
}
