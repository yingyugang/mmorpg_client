using UnityEngine;
using System.Collections;

public class CallbackFunction1: MonoBehaviour {
        
        // Use this for initialization
	
	// Update is called once per frame
	void Update () {
	
	}

	public void NextFun(){
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}

		gameObject.GetComponent<TweenTransform> ().ResetToBeginning ();
		gameObject.GetComponent<TweenTransform> ().enabled = true;

		gameObject.GetComponent<TweenScale> ().ResetToBeginning ();
		gameObject.GetComponent<TweenScale> ().enabled = true;

		gameObject.GetComponent<TweenAlpha> ().ResetToBeginning ();
		gameObject.GetComponent<TweenAlpha> ().enabled = true;

		//Debug.LogWarning ("Name:  "+gameObject.name);
	}
}
