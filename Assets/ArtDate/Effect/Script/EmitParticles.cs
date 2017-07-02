using UnityEngine;
using System.Collections;

public class EmitParticles : MonoBehaviour {
	public GameObject particle;
	//public Camera mcamera;
	// Use this for initialization
	void Start () {
		//mcamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Explosion()
	{
		GameObject xffect = Instantiate (particle,transform.position,Quaternion.identity) as GameObject;
		xffect.transform.GetChild(0).gameObject.SetActive (true);

		//iTween.ShakePosition (mcamera.gameObject,new Vector3(0.2f,0.8f,0.2f),0.3f);

	}
}
