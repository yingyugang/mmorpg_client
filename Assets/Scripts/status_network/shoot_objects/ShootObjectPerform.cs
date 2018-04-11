using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class ShootObjectPerform : MonoBehaviour
	{

		public List<GameObject> closeImmeGOs;

		public void OnHit ()
		{
			StartCoroutine (_UnSpawnDelay(BattleConst.DEFAULT_UNSPAWN_DELAY));
		}

		IEnumerator _UnSpawnDelay (float delay)
		{
			ParticleSystem[] pss = gameObject.GetComponentsInChildren<ParticleSystem> ();
			for (int i = 0; i < pss.Length; i++) {
				pss [i].Stop ();
			}
			if (closeImmeGOs!=null && closeImmeGOs.Count > 0) {
				for (int i = 0; i < closeImmeGOs.Count; i++) {
					closeImmeGOs [i].SetActive (false);
				}
			}
			yield return new WaitForSeconds (delay);
			Instantiater.UnSpawn (false, gameObject);
		}

	}
}
