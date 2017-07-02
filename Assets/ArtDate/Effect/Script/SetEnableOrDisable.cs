using UnityEngine;
using System.Collections;

public class SetEnableOrDisable : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetObjectActive(){
		gameObject.SetActive (true);
	}
	public void SetObjectInactive(){
		gameObject.SetActive (false);
	}
}
