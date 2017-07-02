using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class Test : MonoBehaviour {

	public GameObject prefab;

	void Awake()
	{

	}

	void OnGUI()
	{

		if(GUI.Button(new Rect(10,10,100,30),"Test"))
		{
			Instantiate(prefab);
		}
		if(GUI.Button(new Rect(10,40,100,30),"Time Scale 0"))
		{
			Time.timeScale = 0;
		}
		if(GUI.Button(new Rect(10,70,100,30),"Time Scale 1"))
		{
			Time.timeScale = 1;
		}
	}

}
