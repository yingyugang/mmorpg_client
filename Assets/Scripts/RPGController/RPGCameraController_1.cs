using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGCameraController_1 : MonoBehaviour {

	Camera mCamera;
	Transform mTrans;
	public Transform target;
	public Vector2 targetOffset = new Vector2(0,2);
	public float distance = 10;
	public float angle = 45;
	public float angle_2_5D = 45;
	public bool is3D = true;
	Vector3 mDirect ;
	void Awake(){
		mTrans = transform;
		mCamera = GetComponent<Camera> ();
		mDirect= (new Vector3(mTrans.position.x - target.position.x,0,mTrans.position.z - target.position.z)).normalized;
	}
	
	void LateUpdate () {
		if (!is3D) {
			angle = angle_2_5D;
		}
		if(target!=null){
			float y = distance * Mathf.Sin (angle/180f * Mathf.PI);
			Vector3 targetPos = mDirect * distance * Mathf.Cos (angle / 180f * Mathf.PI) + new Vector3 (0,y,0) + target.position ;
			mTrans.position = targetPos ;
			mTrans.LookAt (target.position );
		}

		if(Input.GetAxis("Mouse X") != 0 && Input.GetMouseButton(1)){
			mDirect = Quaternion.AngleAxis (Input.GetAxis("Mouse X"),new Vector3(0,1,0)) * mDirect;
			target.forward = Quaternion.AngleAxis (Input.GetAxis ("Mouse X"), new Vector3 (0, 1, 0)) * target.forward;
		}

		if(is3D && Input.GetAxis("Mouse Y") != 0 && Input.GetMouseButton(1)){
			angle -= Input.GetAxis("Mouse Y");
		}

	}
}
