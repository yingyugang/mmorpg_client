using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleScale : MonoBehaviour {

	public float targetScale = 1.2f;
	public float delay = 0;
	public float speed = 1;
	Transform mTrans;
	void Awake(){
		mTrans = transform;
		StartCoroutine (_Scale());
	}

	IEnumerator _Scale(){
		while(true){
			if ((int)Time.time % 2 == 0) {
				mTrans.localScale = Vector3.one * Mathf.Lerp (1, targetScale, ((Time.time + delay) * speed) % 1);
			} else {
				mTrans.localScale = Vector3.one * Mathf.Lerp (1, targetScale,1- ((Time.time + delay) * speed) % 1);
			}
			yield return null;
		}
	}
}
