using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMeshEffects : MonoBehaviour {

	public List<GameObject> effects;

	// Use this for initialization
	void Start () {
		MeshRenderer[] mrrs = GetComponentsInChildren<MeshRenderer> ();
		for(int i = 0;i<mrrs.Length;i++){
			GameObject prefab = effects [i % effects.Count];
			GameObject go = Instantiate (prefab);
			go.transform.SetParent (mrrs[i].transform);
			go.transform.localPosition = Vector3.zero;
			PSMeshRendererUpdater psMru = go.GetComponent<PSMeshRendererUpdater> ();
			psMru.MeshObject = mrrs [i].gameObject;
			psMru.UpdateMeshEffect ();
			if(psMru.GetComponentInChildren<Light> ()!=null)
			psMru.GetComponentInChildren<Light> ().enabled = false;
		}
	}
}
