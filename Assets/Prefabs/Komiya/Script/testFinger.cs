//
//  testFinger
//  Created by Yoshinao Izumi on 2017/04/19.
//  Copyright © 2017 Yoshinao Izumi All rights reserved.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//仮想指
public class testFinger : MonoBehaviour {

	public int index=0;

	public delegate void onFingerGrapStateChangedDelegate(bool grap);
	public onFingerGrapStateChangedDelegate onLeftFingerGrapStateChanged;
	public onFingerGrapStateChangedDelegate onRightFingerGrapStateChanged;

	public AncherController ancherLeft;
	public AncherController ancherRight;

	private float rate = 0.03f;
	private bool first=true;

	private bool currentLeftGrap =false;
	private bool currentRightGrap = false;

	private bool grapLeft=false;
	private bool grapRight=false;

	void Start () {
		
	}

	void Update () {

		if (first) {
			onLeftFingerGrapStateChanged = ancherLeft.onFingerGrapStateChangedDelegate;
			onRightFingerGrapStateChanged = ancherRight.onFingerGrapStateChangedDelegate;
		}
		
		if (Input.GetKey (KeyCode.W)) {
			ancherRight.transform.position = new Vector3 (ancherRight.transform.position.x, ancherRight.transform.position.y + rate, ancherRight.transform.position.z);
		} else if (Input.GetKey (KeyCode.X)) {
			ancherRight.transform.position = new Vector3 (ancherRight.transform.position.x, ancherRight.transform.position.y - rate, ancherRight.transform.position.z);
		} else if (Input.GetKey (KeyCode.A)) {
			if (Input.GetKey (KeyCode.LeftShift)) {
				ancherRight.transform.position = new Vector3 (ancherRight.transform.position.x, ancherRight.transform.position.y, ancherRight.transform.position.z + rate);
			} else {
				ancherRight.transform.position = new Vector3 (ancherRight.transform.position.x + rate, ancherRight.transform.position.y, ancherRight.transform.position.z);
			}
		} else if (Input.GetKey (KeyCode.D)) {
			if (Input.GetKey (KeyCode.LeftShift)) {
				ancherRight.transform.position = new Vector3 (ancherRight.transform.position.x, ancherRight.transform.position.y, ancherRight.transform.position.z - rate);
			} else {
				ancherRight.transform.position = new Vector3 (ancherRight.transform.position.x - rate, ancherRight.transform.position.y, ancherRight.transform.position.z);
			}
		} else if (Input.GetKey (KeyCode.I)) {
			ancherLeft.transform.transform.position = new Vector3 (ancherLeft.transform.position.x, ancherLeft.transform.position.y + rate, ancherLeft.transform.position.z);
		} else if (Input.GetKey (KeyCode.M)) {
			ancherLeft.transform.transform.position = new Vector3 (ancherLeft.transform.position.x, ancherLeft.transform.position.y - rate, ancherLeft.transform.position.z);
		} else if (Input.GetKey (KeyCode.J)) {
			if (Input.GetKey (KeyCode.RightShift)) {
				ancherLeft.transform.position = new Vector3 (ancherLeft.transform.position.x, ancherLeft.transform.position.y, ancherLeft.transform.position.z + rate);
			} else {
				ancherLeft.transform.position = new Vector3 (ancherLeft.transform.position.x + rate, ancherLeft.transform.position.y, ancherLeft.transform.position.z);
			}
		} else if (Input.GetKey (KeyCode.L)) {
			if (Input.GetKey (KeyCode.RightShift)) {
				ancherLeft.transform.position = new Vector3 (ancherLeft.transform.position.x, ancherLeft.transform.position.y, ancherLeft.transform.position.z - rate);
			} else {
				ancherLeft.transform.position = new Vector3 (ancherLeft.transform.position.x - rate, ancherLeft.transform.position.y, ancherLeft.transform.position.z);
			}
		} 
						
		if (Input.GetKey (KeyCode.R)) {
			grapRight = true;
		}

		if (Input.GetKey (KeyCode.T)) {
			grapRight = false;
		}

		if (Input.GetKey (KeyCode.U)) {
			grapLeft = true;
		}

		if (Input.GetKey (KeyCode.Y)) {
			grapLeft = false;
		}

		if (grapRight!=currentRightGrap) {
			currentRightGrap = grapRight;
			onRightFingerGrapStateChanged (currentRightGrap);

		} 
		if (grapLeft!=currentLeftGrap) {
			currentLeftGrap=grapLeft;
			onLeftFingerGrapStateChanged (currentLeftGrap);
		}

		
	}
}
