using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

	public string defaultEffectPath = "";
	public GameObject hitPrefab0;
	public GameObject defensePrefab;
	public string playerSourcePath = "Girl";
	public string enemySourcePath = "Boss";

	static SpawnManager instance;
	public static SpawnManager SingleTon(){
		return instance;
	}

	void Awake(){
		if (instance == null)
			instance = this;
		if (hitPrefab0 == null) {
			hitPrefab0 = Resources.Load<GameObject>(defaultEffectPath + "BloodFly01");		
		}
		if(defensePrefab == null)
		{
			defensePrefab = Resources.Load<GameObject>(defaultEffectPath + "Defense");		
		}
	}

	public GameObject SpawnDefensePrefab(Vector3 pos,Quaternion qua)
	{
		return Spawn (defensePrefab,pos,qua,2);
	}

	public GameObject SpawnHitPrefab0(Vector3 pos,Quaternion qua)
	{
		return Spawn (hitPrefab0,pos,qua,2);
	}

	public GameObject Spawn(GameObject prefab,Vector3 pos,Quaternion qua,float unSpawnDelay)
	{
		GameObject go = Instantiate (prefab,pos,qua) as GameObject;
		StartCoroutine (_UpSpawn (go, unSpawnDelay));
		return go;
	}

	public GameObject InitPlayer(){
		GameObject player = Resources.Load<GameObject> (playerSourcePath);
		GameObject go = Instantiate (player) as GameObject;
		return go;
	}

	public GameObject InitPlayer(Vector3 pos,Quaternion rotate){
		GameObject go = InitPlayer();
		go.transform.position = pos;
		go.transform.rotation = rotate;
		return go;
	}

	public GameObject InitEnemy(){
		GameObject enemy = Resources.Load<GameObject> (enemySourcePath);
		GameObject go = Instantiate (enemy) as GameObject;
		return go;
	}

	public GameObject InitEnemy(Vector3 pos,Quaternion rotate){
		GameObject go = InitEnemy();
		go.transform.position = pos;
		go.transform.rotation = rotate;
		return go;
	}

	IEnumerator _UpSpawn(GameObject go,float delay)
	{
		yield return new WaitForSeconds(delay);
		go.SetActive(false);
	}
}
