using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	//TODO better set to every renderer.
	public class UnitPerform : MonoBehaviour
	{
		public Material defaultMat;
		public Material dissolveMat;
		public float duration = 2f;
		public List<Renderer> bodyRenderer;
		public Transform shootPoint;

		void Awake ()
		{
			MMOUnit mmoUnit = GetComponent<MMOUnit> ();
			mmoUnit.onDeath = ShowDeathDissolve;
			ShowSpawnDissolve ();
		}

		public void ShowDeathDissolve ()
		{
			for(int i=0;i<bodyRenderer.Count;i++){
				bodyRenderer[i].material = new Material(dissolveMat);
			}
			StartCoroutine (_ShowDeathDissolve ());
		}

		public void ShowSpawnDissolve ()
		{
			for(int i=0;i<bodyRenderer.Count;i++){
				bodyRenderer[i].material = new Material(dissolveMat);
			}
			StartCoroutine (_ShowSpawnDissolve ());
		}

		IEnumerator _ShowDeathDissolve ()
		{
			yield return new WaitForSeconds (5);
			float t = 0;
			while (t < 1) {
				t += Time.deltaTime / duration;
				for(int i=0;i<bodyRenderer.Count;i++){
					bodyRenderer[i].material.SetFloat ("_Amount", t);
				}
				yield return null;
			}
			gameObject.SetActive (false);
		}

		IEnumerator _ShowSpawnDissolve ()
		{
			for(int i=0;i<bodyRenderer.Count;i++){
				bodyRenderer[i].material.SetFloat ("_Amount", 1);
			}
			float t = 0;
			while (t < 1) {
				t += Time.deltaTime / duration;
				for(int i=0;i<bodyRenderer.Count;i++){
					bodyRenderer[i].material.SetFloat ("_Amount", 1- t);
				}
				yield return null;
			}
			for(int i=0;i<bodyRenderer.Count;i++){
				bodyRenderer[i].material = defaultMat;
			}
		}
	}
}
