using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEffect : MonoBehaviour {

	public GameObject effectPrefab;
	public float delay = 0.2f;
	public float speed = 10;
	public Transform trans;
	// Use this for initialization
	void Start () {
		StartCoroutine (_Spawn());
	}
	
	IEnumerator _Spawn(){
		while(true){
			yield return new WaitForSeconds (delay);
			GameObject go = Instantiate (effectPrefab);
			go.transform.position = transform.position;
			StartCoroutine (_Fly(go));
		}
	}

	IEnumerator _Fly(GameObject go){
		float t = 0;
		while(t<1){
			t += Time.deltaTime / 10;
			go.transform.position += Time.deltaTime * speed * trans.forward;
			yield return null;
		}
		Destroy (go);
	}
}
