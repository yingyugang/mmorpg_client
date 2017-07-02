using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
[System.Serializable]
public class Pool {
	
	public int ID=-1;
	private GameObject parent;
	private GameObject prefab;
	private int totalObjCount;

	[SerializeField]private List<GameObject> available=new List<GameObject>();
	[SerializeField]private List<GameObject> allObject=new List<GameObject>();
	
	private bool setActiveRecursively=false;
	
	public Pool(){}
	
	public Pool(GameObject obj, int num, int id,Transform specialParent = null){
#if UNITY_EDITOR
		parent = new GameObject();
		parent.name = "Pool_" + obj.name;
#endif
		if(specialParent!=null)
		{
			if (parent != null) {
				parent.transform.SetParent(specialParent);
				parent.transform.localPosition = Vector3.zero;
				parent.transform.localScale = Vector3.one;
			}
			parent = specialParent.gameObject;
		}
		prefab=obj;
		ID=id;
		if(prefab.transform.childCount>0) setActiveRecursively=true;
		PrePopulate(num);
	}
	
	public void MatchPopulation(int num){
		//Debug.Log(num-totalObjCount);
		PrePopulate(num-totalObjCount);
	}
	
	public void PrePopulate(int num){
		for(int i=0; i<num; i++){
			GameObject obj=(GameObject)GameObject.Instantiate(prefab);
			if(parent!=null)
			{
				obj.transform.SetParent(parent.transform);
				obj.transform.localPosition = Vector3.zero;
				obj.transform.localScale = Vector3.one;
			}
			obj.AddComponent<ObjectID>().SetID(ID);
			available.Add(obj);
			allObject.Add(obj);
			
			totalObjCount+=1;
			
//			#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2
//			obj.SetActive(false);
//			#else
			obj.SetActive(false);
//			else obj.SetActiveRecursively(false);
//			#endif
		}
	}
	
	public GameObject Spawn(Vector3 pos, Quaternion rot){
		GameObject spawnObj;
		
		if(available.Count>0){
			spawnObj=available[0];
			available.RemoveAt(0);
			
//			#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2
//			spawnObj.SetActive(true);
//			#else
//			if(!setActiveRecursively) 
			if(spawnObj!=null)spawnObj.SetActive(true);
//			else spawnObj.SetActiveRecursively(true);
//			#endif
			
			Transform tempT=spawnObj.transform;
			tempT.position=pos;
			tempT.localPosition = new Vector3(spawnObj.transform.localPosition.x,spawnObj.transform.localPosition.y,70f);
			tempT.rotation=rot;
		}
		else{
//			Debug.Log("spawn new");
			spawnObj=(GameObject)GameObject.Instantiate(prefab, pos, rot);
			if(parent!=null)
			{
				spawnObj.transform.SetParent(parent.transform);
			}
//			if(spawnObj.GetComponent<Drop>()!=null)spawnObj.GetComponent<Drop>().ThisPool = this;
			spawnObj.AddComponent<ObjectID>().SetID(ID);
			allObject.Add(spawnObj);
			totalObjCount+=1;
		}
		return spawnObj;
	}
	
	public void Unspawn(GameObject obj){
		available.Add(obj);
		
//		#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2
//		obj.SetActive(false);
//		#else
//		if(!setActiveRecursively) 
		obj.SetActive(false);
//		else obj.SetActiveRecursively(false);
//		#endif
	}
	
	public void UnspawnAll(){
		foreach(GameObject obj in allObject){
			if(obj!=null) GameObject.Destroy(obj);
		}
	}
	
	public void HideInHierarchy(Transform t){
		foreach(GameObject obj in allObject){
			obj.transform.parent=t;
		}
	}
	
	public GameObject GetPrefab(){
		return prefab;
	}
	
	public int GetTotalCount(){
		return totalObjCount;
	}
	
	public List<GameObject> GetFullList(){
		Debug.Log("getting list, list length= "+allObject.Count);
		return allObject;
	}
}


[System.Serializable]
public class ObjectID : MonoBehaviour{
	public int ID=-1;
	
	public void SetID(int id){
		ID=id;
	}
	
	public int GetID(){
		return ID;
	}

	void OnDestory()
	{
		Debug.Log(gameObject.name + ":OnDestory");
	}

}