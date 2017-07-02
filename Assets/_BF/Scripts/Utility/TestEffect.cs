using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TestEffect : MonoBehaviour {

	public GameObject Hero;
	public GameObject Effect_DeathBase;
	public float Effect_DeathBase_Delay;
	public Vector3 Effect_DeathBase_Offset;
	public GameObject Effect_AfterDeath;
	public float Effect_AfterDeath_Delay;
	public Vector3 Effect_AfterDeath_Offset;
	public GameObject Effect_Catch;
	public float Effect_Catch_Delay;
	public Vector3 Effect_Catch_Offset;

	 bool isTested;
	List<GameObject> tempGos = new List<GameObject>();

	void OnGUI()
	{
		if(isTested)
		{
			if(GUI.Button(new Rect(10,10,100,30),"Reset"))
			{
				isTested = false;
				StopCoroutine("Test");
				Hero.SetActive(true);
				foreach(GameObject go in tempGos)
				{
					Destroy(go);
				}
				tempGos.Clear();
			}
		}
		else
		{
			if(GUI.Button(new Rect(10,10,100,30),"Test"))
			{
				StopCoroutine("Test");
				Hero.SetActive(true);
				StartCoroutine("Test");
				isTested = true;
				foreach(GameObject go in tempGos)
				{
					Destroy(go);
				}
				tempGos.Clear();
			}
		}
	}

	IEnumerator Test()
	{
		Hero.SetActive(false);
		yield return new WaitForSeconds(Effect_DeathBase_Delay);
		GameObject go = Instantiate(Effect_DeathBase,Hero.transform.position + Effect_DeathBase_Offset,Quaternion.identity) as GameObject;
		tempGos.Add(go);
		yield return new WaitForSeconds(Effect_AfterDeath_Delay);
		go = Instantiate(Effect_AfterDeath,Hero.transform.position + Effect_AfterDeath_Offset,Quaternion.identity) as GameObject;
		tempGos.Add(go);
		yield return new WaitForSeconds(Effect_Catch_Delay);
		go = Instantiate(Effect_Catch,Hero.transform.position + Effect_Catch_Offset,Quaternion.identity) as GameObject;
		tempGos.Add(go);
	}


}
