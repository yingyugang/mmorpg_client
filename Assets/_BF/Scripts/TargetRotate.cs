using UnityEngine;
using System.Collections;

public class TargetRotate : MonoBehaviour {

	Transform thisT;
	public float Speed = 10;

	void Start () {
		thisT = transform;
	}

	void Update () {
		thisT.Rotate( -Vector3.forward * Time.deltaTime * Speed);
	}
}
