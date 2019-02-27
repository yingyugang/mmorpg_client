using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MMO;

namespace MMO
{
	public class TPSCameraController : BaseCameraController
	{

		Camera mCamera;
		Transform mTrans;
		public Transform target;
		public Vector3 targetOffset = new Vector3 (0, 2.3f, 0);
		public float distance = 2;
		public float mRealDistance = 2;
		public float angle = 10;
		public float angle_2_5D = 30;
		public float speed = 2;
		public bool is3D = true;
		public bool isRotate = true;
		public float DISTANCE_GROUND_OFFSET = 1;
		public float DISTANCE_SPEED = 10;
		Vector3 mDirect;

		void Awake ()
		{
			mTrans = transform;
			mRealDistance = distance;
			mCamera = GetComponent<Camera> ();
		}

		void Start ()
		{
			mDirect = (new Vector3 (mTrans.position.x - target.position.x, 0, mTrans.position.z - target.position.z)).normalized;
		}

		RaycastHit hit;

		void LateUpdate ()
		{
			if (!is3D) {
				angle = angle_2_5D;
			}
			angle = Mathf.Clamp (angle, -50, 50);
			//to set the camera position and rotation.
			if (target != null) {
				float y = mRealDistance * Mathf.Sin (angle / 180f * Mathf.PI);
				Vector3 targetPos = mDirect * mRealDistance * Mathf.Cos (angle / 180f * Mathf.PI) + new Vector3 (0, y, 0) + target.position + targetOffset;
				mTrans.position = targetPos;
				mTrans.LookAt (target.position + targetOffset);
			}
			//to move the position of target in the screen.
			//		mTrans.position += new Vector3 (mTrans.forward.x, 0, mTrans.forward.z).normalized * targetOffset.z;
			//		mTrans.position += mTrans.right * targetOffset.x;
			//		mTrans.position += new Vector3 (0, targetOffset.y, 0);
			RaycastHit hit;
			float currentDistance = Vector3.Distance (target.position + targetOffset, mTrans.position);
			if (Physics.Raycast (target.position + targetOffset, (mTrans.position - target.position - targetOffset).normalized, out hit, currentDistance + DISTANCE_GROUND_OFFSET, 1 << LayerConstant.LAYER_GROUND)) {
				mRealDistance = Mathf.Max (0.1f, (target.position + targetOffset - hit.point).magnitude - DISTANCE_GROUND_OFFSET);//       mRealDistance - DISTANCE_SPEED * Time.deltaTime);
				if (target != null) {
					float y = mRealDistance * Mathf.Sin (angle / 180f * Mathf.PI);
					Vector3 targetPos = mDirect * mRealDistance * Mathf.Cos (angle / 180f * Mathf.PI) + new Vector3 (0, y, 0) + target.position + targetOffset;
					mTrans.position = targetPos;
					mTrans.LookAt (target.position + targetOffset);
				}
			} else {
				//mRealDistance = Mathf.Min (distance,mRealDistance + DISTANCE_SPEED);
			}
			if (!Physics.Raycast (target.position + targetOffset, (mTrans.position - target.position - targetOffset).normalized, out hit, currentDistance + DISTANCE_GROUND_OFFSET * 4, 1 << LayerConstant.LAYER_GROUND)) {
				mRealDistance = Mathf.Min (distance, mRealDistance + DISTANCE_SPEED * Time.deltaTime);
			} 
			if (isRotate) {
				Ratate ();
			}
		}

		void OnDrawGizmos ()
		{
			Gizmos.DrawSphere (hit.point, 10);
		}

		bool isTouchMoved = false;
		int mFingerId;

		void Ratate ()
		{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
            MobileRotate();
#else
            NormalRotate();
#endif
        }
        //TODO event 化
        void MobileRotate()
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    if (!isTouchMoved)
                    {
                        if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId) && touch.position.x>Screen.width/2f)
                        {
                            mFingerId = touch.fingerId;
                            isTouchMoved = true;
                        }
                    }
                }
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    if (mFingerId == touch.fingerId)
                    {
                        isTouchMoved = false;
                    }
                }
            }

            if (Input.touchCount > 0)
            {
                if (isTouchMoved)
                {
                    if (Input.GetTouch(mFingerId).deltaPosition.x != 0)
                    {
                        mDirect = Quaternion.AngleAxis(Input.GetTouch(mFingerId).deltaPosition.x * speed / 10, new Vector3(0, 1, 0)) * mDirect;
                        if (!target.GetComponent<MMOUnit>().isDead)
                        {
                            target.forward = Quaternion.AngleAxis(Input.GetTouch(mFingerId).deltaPosition.x * speed / 10, new Vector3(0, 1, 0)) * target.forward;
                        }
                    }
                    if (is3D && Input.GetTouch(mFingerId).deltaPosition.y != 0)
                    {
                        angle -= Input.GetTouch(mFingerId).deltaPosition.y * speed / 10;
                    }
                }
            }
        }

        void NormalRotate()
        {
            if (Input.GetAxis("Mouse X") != 0 && Input.GetMouseButton(1))
            {
                mDirect = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * speed, new Vector3(0, 1, 0)) * mDirect;
                if (!target.GetComponent<MMOUnit>().isDead)
                {
                    target.forward = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * speed, new Vector3(0, 1, 0)) * target.forward;
                }
            }
            if (is3D && Input.GetAxis("Mouse Y") != 0 && Input.GetMouseButton(1))
            {
                angle -= Input.GetAxis("Mouse Y") * speed;
            }
        }
    }
}
