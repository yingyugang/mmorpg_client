using UnityEngine;
using System.Collections;

namespace MMO
{
	public static class Instantiater
	{

		//TODO
		public static GameObject Spawn (bool isPool, GameObject prefab, Vector3 pos, Quaternion rotation)
		{ 
			if (isPool) {
				return PoolManager.SingleTon ().Spawn (prefab, pos, rotation);
			} else {
				return GameObject.Instantiate (prefab, pos, rotation) as GameObject;
			}
		}

		public static void New (GameObject prefab, int count)
		{ 
			PoolManager.SingleTon ().AddPool (prefab, count, null);
		}

		public static void UnSpawn (bool isPool, GameObject go)
		{
			if (isPool) {
				PoolManager.SingleTon ().UnSpawn (go);
			} else {
				go.SetActive (false);
			}

		}
	}
}

