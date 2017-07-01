using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {

	Camera mCamera;
	public Transform player;
	public Transform target;

	public float currentCameraDistance = 10;
	public float maxCameraDistance = 10;
	public float minCameraDistance = 3;


	public float cameraHeight = 5;
	public float minCameraHeight = 1;


	// Use this for initialization
	void Start () {
		mCamera = Camera.main;
		if (target!=null && target.GetComponent<UnitResources> () != null && target.GetComponent<UnitResources> ().centerTrans != null) {
			target = target.GetComponent<UnitResources> ().centerTrans;
		} 
	}

	List<GameObject> mHideObjs = new List<GameObject>();

	void Update(){
		RaycastHit[] rhs = Physics.RaycastAll (transform.position, player.position - transform.position, Mathf.Infinity,1 << LayerMask.NameToLayer ("Objs"));
		if (rhs != null && rhs.Length > 0) {
			foreach(RaycastHit rh in rhs)
			{
				if(!mHideObjs.Contains(rh.transform.gameObject))
				{
					mHideObjs.Add(rh.transform.gameObject);
					rh.transform.gameObject.SetActive(false);
				}
			}
		}
	}

	Vector3 direction ;
	public float offsetHeight = 10;
	void LateUpdate(){
		direction = player.position - target.transform.position;
		currentCameraDistance = direction.magnitude;
		currentCameraDistance = Mathf.Clamp (currentCameraDistance, minCameraDistance, maxCameraDistance);
		direction = Vector3.Normalize (direction);
		mCamera.transform.position = player.position + direction * currentCameraDistance;
		mCamera.transform.position = new Vector3 (mCamera.transform.position.x,Mathf.Max(minCameraHeight,player.position.y + cameraHeight), mCamera.transform.position.z);
		mCamera.transform.LookAt (new Vector3(target.transform.position.x ,offsetHeight,target.transform.position.z));
	}


}
