using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class UnitPerform : MonoBehaviour
	{

		public Material defaultMat;
		public Material dissolveMat;
		public float duration = 2f;
		public Renderer bodyRenderer;

		void Awake ()
		{
			MMOUnit mmoUnit = GetComponent<MMOUnit> ();
			mmoUnit.onDeath = ShowDeathDissolve;
			ShowSpawnDissolve ();
		}

		public void ShowDeathDissolve ()
		{
			bodyRenderer.material = new Material(dissolveMat);
			StartCoroutine (_ShowDeathDissolve ());
		}

		public void ShowSpawnDissolve ()
		{
			bodyRenderer.material =  new Material(dissolveMat);
			StartCoroutine (_ShowSpawnDissolve ());
		}

		IEnumerator _ShowDeathDissolve ()
		{
			yield return new WaitForSeconds (5);
			float t = 0;
			while (t < 1) {
				t += Time.deltaTime / duration;
				bodyRenderer.material.SetFloat ("_Amount", t);
				yield return null;
			}
			gameObject.SetActive (false);
		}

		IEnumerator _ShowSpawnDissolve ()
		{
			bodyRenderer.material.SetFloat ("_Amount", 1);
			float t = 0;
			while (t < 1) {
				t += Time.deltaTime / duration;
				bodyRenderer.material.SetFloat ("_Amount", 1 - t);
				yield return null;
			}
			bodyRenderer.material = defaultMat;
		}
	}
}
