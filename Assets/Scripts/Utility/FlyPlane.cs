using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyPlane : MonoBehaviour {

	public AnimationCurve curve;

	float mDefaultHeight;
	public float endHeight = 40;
	public float moveDuration = 10;
	float mInterval = 3;
	float mNextMoveTime;

	void Awake () {
		mDefaultHeight = transform.position.y;
	}

	void Update () {
		if (mNextMoveTime < Time.time) {
			StartCoroutine (_Move());
		}
	}

	IEnumerator _Move(){
		float t = 0;
		while(t < 1){
			t += Time.deltaTime / moveDuration;
			transform.position = new Vector3 (transform.position.x,Mathf.Lerp(mDefaultHeight,endHeight,curve.Evaluate(t)) , transform.position.z);
			yield return null;
		}
		mNextMoveTime = Time.time + mInterval;
	}
}
