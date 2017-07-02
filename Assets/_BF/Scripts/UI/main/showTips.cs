using UnityEngine;
using System.Collections;

public class showTips : MonoBehaviour {
	GameObject touch;
	GameObject finger;
	float t;
	
	void Start () {
		touch = GameObject.Find ("tips/touch");
		finger = GameObject.Find ("tips/finger");
		//touch.active = false;
		touch.SetActive(false);
		//finger.active = false;
		finger.SetActive(false);
		t = Time.time;
	}
	
	void Update () {
		if (Time.time - t >= 5)
		{
			touch.SetActive(!touch.activeSelf);
			finger.SetActive(!finger.activeSelf);
			//touch.active=!touch.active;
			//finger.active=!finger.active;
			t=Time.time;
		}
	}
}
