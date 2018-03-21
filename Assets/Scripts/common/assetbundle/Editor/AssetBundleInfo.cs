using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
class AssetBundleInfo
{
	static AssetBundleInfo ()
	{
		Debug.Log("AssetBundleInfo");
		EditorWindow.GetWindow<AssetBundleInfoWindow> (true, "AssetBundleInfo", true); 
//		EditorApplication.update += Update;
	}

//	static void Update ()
//	{
//		Debug.Log("Updating");
//	}
}