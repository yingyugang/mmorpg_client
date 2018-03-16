using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text;

public class AssetBundlePacker : MonoBehaviour
{
	private const string comma = ",";
	/*
	[MenuItem ("Tools/Create Hash Code")]
	static void GetFileHashCode ()
	{
		Caching.CleanCache ();
		Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
		foreach (Object obj in SelectedAsset) {
			Debug.Log (obj.name + " :" + FileManager.GetFileHash (AssetDatabase.GetAssetPath (obj)));
		}
	}

	[MenuItem ("Tools/Create CSV")]
	static void CreateCSV ()
	{
		Caching.CleanCache ();
		string csv_path = "csv/";
		string resourcesPathPrefix = "Assets/Resources/";
		string originalPath = Application.dataPath;  
		string stockPath = Application.streamingAssetsPath + "/ios/csv.assetbundle";

		Object[] objs = Resources.LoadAll (csv_path);
		List<Object> outputList = new List<Object> ();
		List<string> assetPathList = new List<string> ();
		foreach (Object obj in objs) {
			string fileAssetPath = AssetDatabase.GetAssetPath (obj);
			string fileWholePath = originalPath + "/" + fileAssetPath.Substring (fileAssetPath.IndexOf ("/"));
			SoCsv csv = ScriptableObject.CreateInstance<SoCsv> ();
			csv.fileName = obj.name;
			csv.content = File.ReadAllBytes (fileWholePath);
			string assetPath = resourcesPathPrefix + csv_path + obj.name + ".asset";
			assetPathList.Add (assetPath);
			AssetDatabase.CreateAsset (csv, assetPath);
			outputList.Add (AssetDatabase.LoadAssetAtPath (assetPath, typeof(SoCsv)));
		}
		Object[] outputArray = outputList.ToArray ();
		BuildTarget buildTarget = BuildTarget.iOS;
		#if UNITY_ANDROID
		buildTarget = BuildTarget.Android;
		stockPath = Application.streamingAssetsPath + "/android/csv.assetbundle";
		#endif
		if (BuildPipeline.BuildAssetBundle (null, outputArray, stockPath, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.UncompressedAssetBundle, buildTarget)) {
			AssetDatabase.Refresh ();
			Debug.Log ("CreateCSV successfully");
		} else {
			Debug.LogWarning ("CreateCSV unsuccessfully");
		}
		int length = assetPathList.Count;
		for (int i = 0; i < length; i++) {
			FileManager.DeleteFile (assetPathList [i]);
		}
	}

	[MenuItem ("Tools/Create Local CSV")]
	static void CreateLocalCSV ()
	{
		Caching.CleanCache ();
		string csv_path = "csv_local/";
		string resourcesPathPrefix = "Assets/Resources/";
		string originalPath = Application.dataPath;  
		string stockPath = Application.streamingAssetsPath + "/ios/csv_local.assetbundle";

		Object[] objs = Resources.LoadAll (csv_path);
		List<Object> outputList = new List<Object> ();
		List<string> assetPathList = new List<string> ();
		foreach (Object obj in objs) {
			string fileAssetPath = AssetDatabase.GetAssetPath (obj);
			string fileWholePath = originalPath + "/" + fileAssetPath.Substring (fileAssetPath.IndexOf ("/"));
			SoCsv csv = ScriptableObject.CreateInstance<SoCsv> ();
			csv.fileName = obj.name;
			csv.content = File.ReadAllBytes (fileWholePath);
			string assetPath = resourcesPathPrefix + csv_path + obj.name + ".asset";
			assetPathList.Add (assetPath);
			AssetDatabase.CreateAsset (csv, assetPath);
			outputList.Add (AssetDatabase.LoadAssetAtPath (assetPath, typeof(SoCsv)));
		}
		Object[] outputArray = outputList.ToArray ();
		BuildTarget buildTarget = BuildTarget.iOS;
		#if UNITY_ANDROID
		buildTarget = BuildTarget.Android;
		stockPath = Application.streamingAssetsPath + "/android/csv_local.assetbundle";
		#endif
		if (BuildPipeline.BuildAssetBundle (null, outputArray, stockPath, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.UncompressedAssetBundle, buildTarget)) {
			AssetDatabase.Refresh ();
			Debug.Log ("CreateCSV successfully");
		} else {
			Debug.LogWarning ("CreateCSV unsuccessfully");
		}
		int length = assetPathList.Count;
		for (int i = 0; i < length; i++) {
			FileManager.DeleteFile (assetPathList [i]);
		}
	}

	[MenuItem ("Tools/Create Asset")]
	static void CreateAsset ()
	{
		Caching.CleanCache ();

		Object[] outputArray = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
		string assetName = Selection.GetFiltered (typeof(Object), SelectionMode.Assets) [0].name;

		GetAsset (assetName, outputArray);
		AssetDatabase.Refresh ();
		Debug.Log ("CreateAsset " + assetName + " successfully");
	}

	[MenuItem ("Tools/Create Atlas")]
	static void CreateAtlas ()
	{
		Caching.CleanCache ();

		string stockPath = Application.streamingAssetsPath + "/ios/";
		BuildTarget buildTarget = BuildTarget.iOS;

		#if UNITY_ANDROID
		buildTarget = BuildTarget.Android;
		stockPath = Application.streamingAssetsPath + "/android/";
		#endif
		BuildPipeline.BuildAssetBundles (stockPath, BuildAssetBundleOptions.ForceRebuildAssetBundle, buildTarget);
		AssetDatabase.Refresh ();
		Debug.Log ("CreateAtlas successfully");
	}

	[MenuItem ("Tools/Create Version")]
	static void CreateVersion ()
	{
		Caching.CleanCache ();

		StringBuilder result = new StringBuilder ();
		string title = "FileName,FileSize,IsCSV,HashCode";
		string csvName = "csv.assetbundle";
		List<string> exceptArray = new List<string> {
			"downloading.assetbundle",
			"tutorial.assetbundle",
			"sound_local.assetbundle",
			".meta",
			".DS_Store",
			".csv"
		};
		int length = exceptArray.Count;
		result.AppendLine (title);

		#if UNITY_ANDROID
		string[] stringArray = FileManager.GetFiles (Application.streamingAssetsPath + "/android/", "*", SearchOption.TopDirectoryOnly);
		#else
		string[] stringArray = FileManager.GetFiles (Application.streamingAssetsPath + "/ios/", "*", SearchOption.TopDirectoryOnly);
		#endif

		foreach (var item in stringArray) {
			string[] nameArray = item.Split ('/');
			string fileName = nameArray [nameArray.Length - 1];
			bool hasExcept = false;
			for (int i = 0; i < length; i++) {
				if (fileName.Contains (exceptArray [i])) {
					hasExcept = true;
					break;
				}
			}
			if (!hasExcept) {
				result.AppendLine (CreateLine (fileName, new FileInfo (item).Length, fileName == csvName ? 1 : 0, FileManager.GetFileHash (item)).ToString ());
			}
		}
		#if UNITY_ANDROID
		FileManager.WriteString (Application.streamingAssetsPath + "/android/server.csv", result.ToString ());
		#else
		FileManager.WriteString (Application.streamingAssetsPath + "/ios/server.csv", result.ToString ());
		#endif
		AssetDatabase.Refresh ();
		Debug.Log ("CreateVersion successfully");
	}

	[MenuItem ("Tools/Create All Game Scenes")]
	static void CreateAllGameScenes ()
	{
		string[] sceneArray = new string[] {
			"Daruma",
			"GetOut",
			"Swimming",
			"BreakoutClone",
			"Shee",
			"Biking"
		};
		BuildTarget buildTarget = BuildTarget.iOS;
		#if UNITY_ANDROID
		buildTarget = BuildTarget.Android;
		#endif
		string path = Application.dataPath + "/Scenes/games/";
		int length = sceneArray.Length;
		for (int i = 0; i < length; i++) {
			string[] levels = { path + sceneArray [i] + ".unity" };
//			BuildPipeline.BuildPlayer (levels, path + sceneArray [i] + ".unity3d", buildTarget, BuildOptions.BuildAdditionalStreamedScenes);
			BuildPipeline.BuildStreamedSceneAssetBundle (levels, path + sceneArray [i] + ".unity3d", buildTarget);
		}
		AssetDatabase.Refresh ();
		Debug.Log ("CreateAllGameScenes successfully");
	}

	[MenuItem ("Tools/Create All Assets")]
	static void CreateAllAssets ()
	{
		Caching.CleanCache ();
		string[] array = FileManager.GetDirectories (Application.dataPath + "/Resources/FrameResources/", "*", SearchOption.TopDirectoryOnly);
		for (int i = 0; i < array.Length; i++) {
			string[] nameArray = array [i].Split ('/');
			string assetName = nameArray [nameArray.Length - 1];
			Object[] outputArray = Resources.LoadAll ("FrameResources/" + assetName, typeof(Object));
			GetAsset (assetName, outputArray);
		}
		AssetDatabase.Refresh ();
		Debug.Log ("CreateAllAssets successfully");
	}

	[MenuItem ("Tools/Create All Assets And CSV")]
	static void CreateAllAssetsAndCSV ()
	{
		CreateAllAssets ();
		CreateCSV ();
		Debug.Log ("CreateAllAssetsAndCSV successfully");
	}

	[MenuItem ("Tools/Create All Assets And CSV And Version")]
	static void CreateAllAssetsAndCSVAndVersion ()
	{
		CreateAllAssets ();
		CreateCSV ();
		CreateVersion ();
		Debug.Log ("CreateAllAssetsAndCSVAndVersion successfully");
	}

	private static void GetAsset (string assetName, Object[] outputArray)
	{
		string stockPath = Application.streamingAssetsPath + "/ios/" + assetName + ".assetbundle";
		BuildTarget buildTarget = BuildTarget.iOS;

		#if UNITY_ANDROID
		buildTarget = BuildTarget.Android;
		stockPath = Application.streamingAssetsPath + "/android/" + assetName + ".assetbundle";
		#endif
		if (BuildPipeline.BuildAssetBundle (null, outputArray, stockPath, BuildAssetBundleOptions.CollectDependencies, buildTarget)) {
			AssetDatabase.Refresh ();
			Debug.Log ("GetAsset " + assetName + " successfully");
		} else {
			Debug.LogWarning ("GetAsset " + assetName + " unsuccessfully");
		}
	}

	private static StringBuilder CreateLine (string name, long size, int isCsv, string hashCode)
	{
		StringBuilder line = new StringBuilder ();
		line.Append (name);
		line.Append (comma);
		line.Append (size);
		line.Append (comma);
		line.Append (isCsv);
		line.Append (comma);
		line.Append (hashCode);
		return line;
	}*/
}
