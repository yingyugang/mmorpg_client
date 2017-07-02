using UnityEngine;
using System.Collections;

public class TestBundleController : MonoBehaviour {

	void OnGUI()
	{
		if(GUI.Button(new Rect(10,10,100,30),"Load"))
		{
			StartCoroutine(_Load());

		}
	}


	IEnumerator _Load()
	{
		WWW bundle = new WWW("http://localhost/Test.unity3d");
		yield return bundle;
		
		//加载到游戏中
		yield return Instantiate(bundle.assetBundle.mainAsset);
		bundle.assetBundle.Unload(false);
	}

}
