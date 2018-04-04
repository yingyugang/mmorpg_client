using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SkilledMeshShadowUtility : MonoBehaviour {

	public bool load;

	void Update () {
		if (load) {
			load = false;
			SkinnedMeshRenderer[] rsr = gameObject.GetComponentsInChildren<SkinnedMeshRenderer> (true);
			for(int i=0;i<rsr.Length;i++){
				rsr [i].receiveShadows = false;
				rsr [i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			}
		}
	}
}
