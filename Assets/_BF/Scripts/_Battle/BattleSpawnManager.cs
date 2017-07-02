using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;

public class BattleSpawnManager : SingleMonoBehaviour<BattleSpawnManager> {

	public GameObject unitPrefab;
	List<string> mTempPlayers;
	List<string> mTempEnemys;
	public List<GameObject> leftUnits;
	public List<GameObject> rightUnits;
	public GameObject battleField;
	public List<ScrollingScript> scrolls;

	protected override void Awake ()
	{
		base.Awake ();
		mTempPlayers = new List<string> ();
		mTempPlayers.Add ("10282");
		mTempPlayers.Add ("10282");
		mTempPlayers.Add ("10282");
		mTempEnemys = new List<string> ();
		mTempEnemys.Add ("10043");
		mTempEnemys.Add ("10044");
		mTempEnemys.Add ("10045");
		mTempEnemys.Add ("10046");
		mTempEnemys.Add ("10047");
	}

	void Start(){
		Init ();
	}

	void Init(){
		for(int i=0;i<mTempPlayers.Count;i++){
			rightUnits.Add(InitUnitPrefab (mTempPlayers[i]));
		}
		for(int i=0;i<mTempEnemys.Count;i++){
			leftUnits.Add(InitUnitPrefab (mTempEnemys[i]));
		}
		GameObject fieldPrefab = Resources.Load<GameObject> ("World/GobiGrassland1");
		battleField = Instantiate (fieldPrefab) as GameObject;
		battleField.transform.localScale = new Vector3 (1.5f,1.5f,1);
		scrolls.AddRange (battleField.GetComponentsInChildren<ScrollingScript>());
	}

	GameObject InitUnitPrefab(string heroId){
		GameObject unitGo = Instantiate (unitPrefab) as GameObject;
		GameObject prefab = Resources.Load<GameObject> ("_Units/" + heroId);
		GameObject body = Object.Instantiate (prefab) as GameObject;
		body.transform.SetParent (unitGo.transform);
		body.transform.localPosition = Vector2.zero;
		body.transform.localEulerAngles = Vector3.zero;
		unitGo.name = heroId;
		unitGo.GetComponent<Unit>().unitId = heroId;
		return unitGo;
	}

}
