using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MMO
{
	public class PoolManager : MonoBehaviour
	{

		public Dictionary<GameObject,Pool> poolDictionary = new Dictionary<GameObject,Pool> ();
		public Dictionary<int,Pool> poolDictionary1 = new Dictionary<int, Pool> ();
		public List<Pool> pools = new List<Pool> ();
		public int currentId;

		static PoolManager instance;

		static public PoolManager SingleTon ()
		{
			if (instance == null) {
				GameObject go = new GameObject ();
				go.name = "_PoolManager";
				instance = go.AddComponent<PoolManager> ();
			}
			return instance;
		}

		void Awake ()
		{
			if (instance == null) {
				instance = this;
			}
		}

		public void Spawn (GameObject prefab, Vector3 pos, Quaternion qua, float delay, float unSpawnDelay)
		{
			StartCoroutine (_SpawnAndUnspawn (prefab, pos, qua, delay, unSpawnDelay));
		}

		IEnumerator _SpawnAndUnspawn (GameObject prefab, Vector3 pos, Quaternion qua, float delay, float unSpawnDelay)
		{
			yield return new WaitForSeconds (delay);
			GameObject go = Spawn (prefab, pos, qua);
			yield return new WaitForSeconds (unSpawnDelay);
			UnSpawn (go);
		}

		public GameObject Spawn (GameObject prefab, Vector3 pos, Quaternion qua)
		{
			if (!poolDictionary.ContainsKey (prefab)) {
				AddPool (prefab, 10);
			}
			return poolDictionary [prefab].Spawn (pos, qua);
		}

		public void UnSpawn (float delay, GameObject go)
		{
			StartCoroutine (_UnSpawn (delay, go));
		}

		IEnumerator _UnSpawn (float delay, GameObject go)
		{
			yield return new WaitForSeconds (delay);
			UnSpawn (go);
		}

		public void UnSpawn (GameObject go)
		{
			ObjectID objectID = go.GetComponent<ObjectID> ();
			if (objectID != null && poolDictionary1.ContainsKey (objectID.ID)) {
				poolDictionary1 [objectID.ID].Unspawn (go);
			} else {
				Destroy (go);
			}
		}

		public void AddPool (GameObject prefab, int num, Transform parentNode = null)
		{
			if (poolDictionary.ContainsKey (prefab)) {
				Pool pool = poolDictionary [prefab];
				pool.PrePopulate (num);
			} else {
				Pool pool = new Pool (prefab, num, currentId, parentNode);
				poolDictionary.Add (prefab, pool);
				poolDictionary1.Add (currentId, pool);
				pools.Add (pool);
				currentId++;
			}
		}
	}
}