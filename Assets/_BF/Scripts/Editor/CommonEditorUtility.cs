using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using UnityEditorInternal;
using System.Reflection;
#endif
public class CommonEditorUtility  {

	static string path = "/_BF/Prefabs/Hero";

	public static List<GameObject> GetAllHeros()
	{
		string[] paths = CommonUtility.GetFileByDirectory(path);
		List<GameObject> hre = new List<GameObject>();
		foreach(string str in paths)
		{
			string assetPath = "Assets" + str.Replace(Application.dataPath, "").Replace('\\', '/');
			GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath,typeof(GameObject));
			if(prefab!=null)
			{
				hre.Add(prefab);
			}
		}
		return hre;
	}


}
