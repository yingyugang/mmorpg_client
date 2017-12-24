using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimator : MonoBehaviour {

	public string stateName = "run";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.H)){
			Animator anim = GetComponent<Animator> ();
			anim.Play (stateName,0,0);
		}
	}
}
