using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{

	public float speed = 1f;
	Transform mTrans;
	// Use this for initialization
	void Start ()
	{
		mTrans = transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		mTrans.Rotate (new Vector3(0,0,1),speed * Time.deltaTime);
	}
}
