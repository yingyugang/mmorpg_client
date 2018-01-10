using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KomiyaArmd : MonoBehaviour
{

	Animator mAnimator;
	const string TRIGGER_JUMP = "jump";
	const string TRIGGER_BACKFLIP = "backFlip";

	void Start ()
	{
		mAnimator = GetComponent<Animator> ();
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), Mathf.Infinity, 1 << LayerConstant.LAYER_KOMIYA)) {
				if (Random.Range (0, 2) == 0) {
					mAnimator.SetTrigger (TRIGGER_JUMP);
				} else {
					mAnimator.SetTrigger (TRIGGER_BACKFLIP);
				}
			}
		}
	}

}
