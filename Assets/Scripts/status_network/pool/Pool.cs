using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MMO
{
	[System.Serializable]
	public class Pool
	{
	
		public int ID = -1;
		private GameObject parent;
		private GameObject prefab;
		private int totalObjCount;

		[SerializeField]private List<GameObject> available = new List<GameObject> ();
		[SerializeField]private List<GameObject> allObject = new List<GameObject> ();
	
		private bool setActiveRecursively = false;

		public Pool ()
		{
		}

		public Pool (GameObject obj, int num, int id, Transform specialParent = null)
		{
#if UNITY_EDITOR
			parent = new GameObject ();
			parent.name = "Pool_" + obj.name;
#endif
			if (specialParent != null) {
				if (parent != null)
					parent.transform.parent = specialParent;
				parent = specialParent.gameObject;
			}
			prefab = obj;
			ID = id;
			if (prefab.transform.childCount > 0)
				setActiveRecursively = true;
			PrePopulate (num);
		}

		public void MatchPopulation (int num)
		{
			PrePopulate (num - totalObjCount);
		}

		public void PrePopulate (int num)
		{
			for (int i = 0; i < num; i++) {
				GameObject obj = (GameObject)GameObject.Instantiate (prefab);
				if (parent != null) {
					obj.transform.parent = parent.transform;
				}
				obj.AddComponent<ObjectID> ().SetID (ID);
				available.Add (obj);
				allObject.Add (obj);
				totalObjCount += 1;
				obj.SetActive (false);
			}
		}

		public GameObject Spawn (Vector3 pos, Quaternion rot)
		{
			GameObject spawnObj;
			if (available.Count > 0) {
				spawnObj = available [0];
				available.RemoveAt (0);
				if (spawnObj != null)
					spawnObj.SetActive (true);
				Transform tempT = spawnObj.transform;
				tempT.position = pos;
				tempT.rotation = rot;
			} else {
				spawnObj = (GameObject)GameObject.Instantiate (prefab, pos, rot);
				if (parent != null) {
					spawnObj.transform.parent = parent.transform;
				}
				spawnObj.AddComponent<ObjectID> ().SetID (ID);
				allObject.Add (spawnObj);
				totalObjCount += 1;
			}
			return spawnObj;
		}

		public void Unspawn (GameObject obj)
		{
			available.Add (obj);
			obj.SetActive (false);
		}

		public void UnspawnAll ()
		{
			foreach (GameObject obj in allObject) {
				if (obj != null)
					GameObject.Destroy (obj);
			}
		}

		public void HideInHierarchy (Transform t)
		{
			foreach (GameObject obj in allObject) {
				obj.transform.parent = t;
			}
		}

		public GameObject GetPrefab ()
		{
			return prefab;
		}

		public int GetTotalCount ()
		{
			return totalObjCount;
		}

		public List<GameObject> GetFullList ()
		{
			Debug.Log ("getting list, list length= " + allObject.Count);
			return allObject;
		}
	}


	[System.Serializable]
	public class ObjectID : MonoBehaviour
	{
		public int ID = -1;

		public void SetID (int id)
		{
			ID = id;
		}

		public int GetID ()
		{
			return ID;
		}

		void OnDestory ()
		{
			Debug.Log (gameObject.name + ":OnDestory");
		}
	}
}