using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RPGCameraController : MonoBehaviour
{

	Camera mCamera;
	Transform mTrans;
	public Transform target;
	public Vector3 targetOffset = new Vector3 (0, 0, 3);
	public float distance = 10;
	public float angle = 45;
	public float angle_2_5D = 40;
	public float speed = 2;
	public bool is3D = true;
	public bool isRotate = true;
	Vector3 mDirect;

	void Awake ()
	{
		mTrans = transform;
		mCamera = GetComponent<Camera> ();
		mDirect = (new Vector3 (mTrans.position.x - target.position.x, 0, mTrans.position.z - target.position.z)).normalized;
	}

	void LateUpdate ()
	{
		if (!is3D) {
			angle = angle_2_5D;
		}
		//to set the camera position and rotation.
		if (target != null) {
			float y = distance * Mathf.Sin (angle / 180f * Mathf.PI);
			Vector3 targetPos = mDirect * distance * Mathf.Cos (angle / 180f * Mathf.PI) + new Vector3 (0, y, 0) + target.position;
			mTrans.position = targetPos;
			mTrans.LookAt (target.position);
		}
		//to move the position of target in the screen.
		mTrans.position += new Vector3 (mTrans.forward.x, 0, mTrans.forward.z).normalized * targetOffset.z;
		mTrans.position += mTrans.right * targetOffset.x;
		mTrans.position += new Vector3 (0, targetOffset.y, 0);
		if (isRotate) {
			Ratate ();
		}
	}

	bool isTouchMoved = false;
	int mFingerId;

	void Ratate ()
	{
		#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				if (!isTouchMoved) {
					if (!EventSystem.current.IsPointerOverGameObject (touch.fingerId)) {
						mFingerId = touch.fingerId;
						isTouchMoved = true;
					}
				}
			}
			if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
				if (mFingerId == touch.fingerId) {
					isTouchMoved = false;
				}
			}
		}

		if(Input.touchCount>0){
			if(isTouchMoved){
				if (Input.GetTouch(mFingerId).deltaPosition.x != 0 ) {
					mDirect = Quaternion.AngleAxis (Input.GetTouch(mFingerId).deltaPosition.x * speed, new Vector3 (0, 1, 0)) * mDirect;
					target.forward = Quaternion.AngleAxis (Input.GetTouch(mFingerId).deltaPosition.x * speed, new Vector3 (0, 1, 0)) * target.forward;
				}
				if (is3D && Input.GetTouch(mFingerId).deltaPosition.y != 0 ) {
					angle -= Input.GetTouch(mFingerId).deltaPosition.y * speed;
				}
			}
		}
//		Debug.Log(string.Format("{0}:{1}:{2}",Input.GetTouch(mFingerId).fingerId,isTouchMoved,Input.GetTouch(mFingerId).deltaPosition.x));
		#else
		if (Input.GetAxis ("Mouse X") != 0 && Input.GetMouseButton (1)) {
			mDirect = Quaternion.AngleAxis (Input.GetAxis ("Mouse X") * speed, new Vector3 (0, 1, 0)) * mDirect;
			target.forward = Quaternion.AngleAxis (Input.GetAxis ("Mouse X") * speed, new Vector3 (0, 1, 0)) * target.forward;
		}
		if (is3D && Input.GetAxis ("Mouse Y") != 0 && Input.GetMouseButton (1)) {
			angle -= Input.GetAxis ("Mouse Y") * speed;
		}
		#endif
	}


}
