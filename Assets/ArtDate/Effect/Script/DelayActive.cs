using UnityEngine;
using System.Collections;

public class DelayActive : MonoBehaviour {
	public float delayTime=1;
	public GameObject[] targetObjects;
	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds (delayTime);
		for (int i=0; i<targetObjects.Length; i++) {
			targetObjects[i].SetActive(true);
		}
		yield return null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
