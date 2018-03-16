using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAB : MonoBehaviour {

	AssetBundle ab; 
	Sprite s;
	public SpriteRenderer sr;
	// Use this for initialization
	void Start () {
		string path = Application.dataPath + "/Assetbundles/ios/seeding_15_4.assetbundle";
		ab = AssetBundle.LoadFromFile (path);
	}

	int index =0;
	// Update is called once per frame
	void Update () {
//		if (s != null && s.texture != null) {
//			DestroyImmediate (s,true);
//		}
//		AssetBundle.
//		if(ab==null)
//		ab = AssetBundle.LoadFromFile (path);

		s = ab.LoadAsset<Sprite> (index.ToString());
//		ab.Unload (false);
		sr.sprite = s;
		index++;
		index = index % 30;
		Resources.UnloadUnusedAssets ();
//		if(Input.GetKeyDown(KeyCode.H)){
//			
//		}
//		if(Input.GetKeyDown(KeyCode.J)){
//			if(s!=null)
//				Destroy (s.texture);
//			s = ab.LoadAsset<Sprite> ("33");
//		}
//		if(Input.GetKeyDown(KeyCode.G)){
//			
//		}
	}
}
