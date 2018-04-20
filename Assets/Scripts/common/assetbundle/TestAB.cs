using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAB : MonoBehaviour {


	public string abName;

	public string prefabName;

	int index =0;
	void Update () {
		if(Input.GetKeyDown(KeyCode.H)){
			LoadPrefab ();
		}
	}

	void LoadPrefab(){
		string path = Application.streamingAssetsPath + "/" + abName;
		Debug.Log (string.Format("path:{0}",path));
		AssetBundle ab = AssetBundle.LoadFromFile (path);
		GameObject prefab = ab.LoadAsset<GameObject> (prefabName);
		GameObject go = Instantiate (prefab);
		go.SetActive (true);
		go.transform.localPosition = Vector3.zero;


		Material[] mats = ab.LoadAllAssets<Material> ();
		for(int i=0;i<mats.Length;i++){
			string shaderName = mats [i].shader.name;
			Debug.LogError (shaderName);
			Shader shader = Shader.Find (shaderName);
			Debug.LogError (shader);
		}
	}

}
