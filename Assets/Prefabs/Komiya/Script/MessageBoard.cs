using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBoard : MonoBehaviour {

	private TextMesh texMesh;
	void Start () {
		texMesh = GetComponent<TextMesh> ();
		texMesh.text = "aaaaa";
	}
	void Update () {
		
	}
		

}
