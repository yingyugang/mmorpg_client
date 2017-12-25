using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMeshEffects : MonoBehaviour
{

	public List<GameObject> effects;

	public Light directionLight;

	public List<PSMeshRendererUpdater> mrus;

	Color mDefaultColor;

	Color mNeightColor;

	// Use this for initialization
	void Start ()
	{
		mrus = new List<PSMeshRendererUpdater> ();
		MeshRenderer[] mrrs = GetComponentsInChildren<MeshRenderer> ();
		mDefaultColor = directionLight.color;
		mNeightColor = mDefaultColor / 4;
		for (int i = 0; i < mrrs.Length; i++) {
			GameObject prefab = effects [i % effects.Count];
			GameObject go = Instantiate (prefab);
			go.transform.SetParent (mrrs [i].transform);
			go.transform.localPosition = Vector3.zero;
			PSMeshRendererUpdater psMru = go.GetComponent<PSMeshRendererUpdater> ();
			mrus.Add (psMru);
			psMru.MeshObject = mrrs [i].gameObject;
			psMru.UpdateMeshEffect ();
			if (psMru.GetComponentInChildren<Light> () != null)
				psMru.GetComponentInChildren<Light> ().enabled = false;
		}
	}


	bool isNeight;

	void Update ()
	{
//		if (Time.time % 20 > 10) {
//			if (!isNeight) {
//				isNeight = true;
//				StartCoroutine (_ChangeColor (mDefaultColor, mNeightColor, true));
//			}
//		} else if(Time.time % 20 < 1) {
//			if (isNeight) {
//				isNeight = false;
//				StartCoroutine (_ChangeColor (mNeightColor, mDefaultColor, false));
//			}
//		}
	}

	IEnumerator _ChangeColor (Color fromColor, Color toColor, bool isLight)
	{
		float t = 0;
		float duration = 3f;
		while (t < 1) {
			t += Time.deltaTime / duration;
			directionLight.color = Color.Lerp (fromColor, toColor, t);
			yield return null;
		}
		for (int i = 0; i < mrus.Count; i++) {
			if (mrus[i].GetComponentInChildren<Light> () != null)
				mrus[i].GetComponentInChildren<Light> ().enabled = isLight;
		}
	}

}
