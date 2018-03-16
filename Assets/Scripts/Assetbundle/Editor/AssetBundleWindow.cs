using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BattleFramework.Data;


public class AssetBundleWindow : EditorWindow
{
	[MenuItem ("Tools/AB Manager")] 
	static void Init ()
	{ 
		InitPath ();
		EditorWindow.GetWindow<AssetBundleWindow> (true, "AB Manager", true); 
	}

	static GUIStyle styleRed;
	static string[] allAssetBundleNames;
	static List<ABBuildEntity> allAssetBundleEntitys;
	static string[] allStreamAssetBundleNames;
	static string[] allStreamAssetPath;
	static string fullResourceAssetPath;
	static string tempResourceAssetPath;
	#if UNITY_IOS
	static string fullStreamPath;
	static string fullTmpOutputPath;
	static string fullTmpVersionOutputPath;
	static string fullServerCSVPath;
	#elif UNITY_ANDROID
	static string fullStreamPath;
	static string fullTmpOutputPath;
	static string fullTmpVersionOutputPath;
	static string fullServerCSVPath;
	#endif
	static string serverCSV;

	static void InitPath ()
	{
		fullResourceAssetPath = Application.dataPath + "/DownloadResources/assets/";
		tempResourceAssetPath = Application.dataPath + "/Assetbundles/temp/";
		#if UNITY_IOS
		fullStreamPath = Application.dataPath + "/StreamingAssets/ios/";
		fullTmpOutputPath = Application.dataPath + "/Assetbundles/ios/";
		fullTmpVersionOutputPath = Application.dataPath + "/Assetbundles/ios/version/";
		fullServerCSVPath = Application.dataPath + "/Assetbundles/ios/server.csv";
		#elif UNITY_ANDROID
		fullStreamPath = Application.dataPath + "/StreamingAssets/android/";
		fullTmpOutputPath = Application.dataPath + "/Assetbundles/android/";
		fullTmpVersionOutputPath = Application.dataPath + "/Assetbundles/android/version/";
		fullServerCSVPath = Application.dataPath + "/Assetbundles/android/server.csv";
		#endif
		serverCSV = "server_resource.csv";
	}

	void OnEnable ()
	{
		Reload ();
	}

	static void Reload ()
	{
		InitBuild ();
		styleRed = new GUIStyle ();
		styleRed.active.textColor = Color.red;
		if (!FileManager.DirectoryExists (tempResourceAssetPath)) {
			FileManager.CreateDirectory (tempResourceAssetPath);
		}
		if (!FileManager.DirectoryExists (fullTmpOutputPath)) {
			FileManager.CreateDirectory (fullTmpOutputPath);
		}
		if (!FileManager.DirectoryExists (fullTmpVersionOutputPath)) {
			FileManager.CreateDirectory (fullTmpVersionOutputPath);
		}
	}

	void OnGUI ()
	{
		OnBuildGUI ();
	}

	static string serverHash = "";
	Vector2 srollPos;

	void OnBuildGUI ()
	{
		srollPos = EditorGUILayout.BeginScrollView (srollPos, GUILayout.Height (400));
		if (allAssetBundleEntitys != null) {
			for (int i = 0; i < allAssetBundleEntitys.Count; i++) {
				if (allAssetBundleEntitys [i].isSelected) {
					GUI.color = Color.green;
				} else {
					GUI.color = Color.white;
				}
				EditorGUILayout.BeginHorizontal ();
				allAssetBundleEntitys [i].isSelected = GUILayout.Toggle (allAssetBundleEntitys [i].isSelected, "", GUILayout.Width (30));
				GUILayout.Label (allAssetBundleEntitys [i].abName, GUILayout.Width (180));
				GUILayout.TextField (allAssetBundleEntitys [i].abHash, GUILayout.Width (300));
				if (allAssetBundleEntitys [i].realLength > 1024 * 1024) {
					GUI.color = Color.red;
				} else if (allAssetBundleEntitys [i].realLength > 1024) {
					GUI.color = Color.yellow;
				} else {
					GUI.color = Color.white;
				}
				GUILayout.Label (allAssetBundleEntitys [i].length, GUILayout.Width (90));
				GUI.color = Color.white;
				if (GUILayout.Button ("SelectDP", GUILayout.Width (100))) {
					SelectDP (allAssetBundleNames [i]);
				}
				EditorGUILayout.EndHorizontal ();
			}
		} else {
			if (GUILayout.Button ("Reload", GUILayout.Width (100))) {
				Reload ();
			}
		}
		EditorGUILayout.EndScrollView ();

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("RemoveUnusedABName", GUILayout.Width (150))) {
			AssetDatabase.RemoveUnusedAssetBundleNames ();
			InitBuild ();
		}
		GUILayout.Label ("Totle Size : " + GetLengthStr (totalSize));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Build", GUILayout.Width (100))) {
			BuildABs ();
		}
		if (GUILayout.Button ("CopyToTemp", GUILayout.Width (120))) {
			CopyToTemp ();
		}
		if (GUILayout.Button ("CopyToServer", GUILayout.Width (120))) {
			CopyToServer ();
		}
//		if(GUILayout.Button("CopyToServerCSV",GUILayout.Width(120))){
//			CopyToServerCSV ();
//			serverHash = FileManager.GetFileHash (fullServerCSVPath);
//		}
		if (GUILayout.Button ("SelectAll", GUILayout.Width (120))) {
			for (int i = 0; i < allAssetBundleEntitys.Count; i++) {
				allAssetBundleEntitys [i].isSelected = true;
			}
		}
		if (GUILayout.Button ("UnSelectAll", GUILayout.Width (120))) {
			for (int i = 0; i < allAssetBundleEntitys.Count; i++) {
				allAssetBundleEntitys [i].isSelected = false;
			}
		}

//		if (GUILayout.Button ("GetMovieHash", GUILayout.Width (120))) {
//			this.GetMovieHash ();
//		}
		EditorGUILayout.EndHorizontal ();

		GUILayout.Space (20);

		EditorGUILayout.BeginHorizontal ();
		GUILayout.TextField (serverHash, GUILayout.Width (400));
		if (GUILayout.Button ("GetServerCSVHash", GUILayout.Width (150))) {
			serverHash = FileManager.GetFileHash (fullServerCSVPath);
		}
		EditorGUILayout.EndHorizontal ();
	}

	static string GetLengthStr (long length)
	{
		string key = " B";
		float f = length;
		if (f >= 1024) {
			f = f / 1024f;
			key = " K";
		}
		if (f >= 1024) {
			f = f / 1024f;
			key = " M";
		}

		int v = Mathf.RoundToInt (f * 100);
		return v / 100f + key;
	}

	static void InitBuild ()
	{
		InitPath ();
		allAssetBundleNames = AssetDatabase.GetAllAssetBundleNames ();
		allAssetBundleEntitys = new List<ABBuildEntity> ();
		for (int i = 0; i < allAssetBundleNames.Length; i++) {
			ABBuildEntity ab = new ABBuildEntity ();
			ab.abName = allAssetBundleNames [i];
			allAssetBundleEntitys.Add (ab);
		}
		GetHash ();
		serverHash = FileManager.GetFileHash (fullServerCSVPath);
	}

	static long totalSize;

	static void GetHash ()
	{
		totalSize = 0;
		for (int i = 0; i < allAssetBundleEntitys.Count; i++) {
			allAssetBundleEntitys [i].abHash = FileManager.GetFileHash (fullTmpOutputPath + allAssetBundleEntitys [i].abName);
			if (allAssetBundleEntitys [i].abHash != null && allAssetBundleEntitys [i].abHash.Trim () != "") {
				allAssetBundleEntitys [i].realLength = new FileInfo (fullTmpOutputPath + allAssetBundleEntitys [i].abName).Length;
				allAssetBundleEntitys [i].length = GetLengthStr (allAssetBundleEntitys [i].realLength);
				totalSize += allAssetBundleEntitys [i].realLength;
			}
		}
	}

	void SelectDP (string abName)
	{
		List<Object> objs = new List<Object> ();
		string[] paths = AssetDatabase.GetAssetPathsFromAssetBundle (abName);
		foreach (string p in paths) {
			Object obj = AssetDatabase.LoadAssetAtPath (p, typeof(Object));
			objs.Add (obj);
		}
		Selection.objects = objs.ToArray ();
	}

	int mBuildMode = 0;

	void BuildABs ()
	{
		AssetDatabase.RemoveUnusedAssetBundleNames ();
		string[] paths1 = Directory.GetFiles (fullTmpOutputPath);
		for (int i = 0; i < paths1.Length; i++) {
			FileManager.DeleteFile (paths1 [i]);
		}

		if (mBuildMode == 0) {
			#if UNITY_IOS
			BuildPipeline.BuildAssetBundles (fullTmpOutputPath, BuildAssetBundleOptions.None, BuildTarget.iOS);
			#elif UNITY_ANDROID
			BuildPipeline.BuildAssetBundles (fullTmpOutputPath, BuildAssetBundleOptions.None, BuildTarget.Android);
			#else
			BuildPipeline.BuildAssetBundles (fullTmpOutputPath,BuildAssetBundleOptions.None);
			#endif
		} else {
			List<AssetBundleBuild> abList = new List<AssetBundleBuild> ();
			foreach (ABBuildEntity entity in allAssetBundleEntitys) {
				if (entity.isSelected) {
					AssetBundleBuild abb = new AssetBundleBuild ();
					abb.assetBundleName = entity.abName.Split ('.') [0];// entity.abName;
					abb.assetBundleVariant = entity.abName.Split ('.') [1];// entity.abName;
					abb.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle (entity.abName);
					abList.Add (abb);
				}
			}
			#if UNITY_IOS
			if (abList.Count > 0) {
				BuildPipeline.BuildAssetBundles (fullTmpOutputPath, abList.ToArray (), BuildAssetBundleOptions.None, BuildTarget.iOS);
			}
			#elif UNITY_ANDROID
			if(abList.Count>0){
				BuildPipeline.BuildAssetBundles (fullTmpOutputPath,abList.ToArray(),BuildAssetBundleOptions.None, BuildTarget.Android);
			}
			#else
			BuildPipeline.BuildAssetBundles (fullTmpOutputPath,abList.ToArray(),BuildAssetBundleOptions.None);
			#endif
		}

//		InitBuild ();
//		#if UNITY_IOS
//		BuildPipeline.BuildAssetBundles (fullTmpOutputPath, BuildAssetBundleOptions.None, BuildTarget.iOS);
//		#elif UNITY_ANDROID
//		BuildPipeline.BuildAssetBundles (fullTmpOutputPath,BuildAssetBundleOptions.None,BuildTarget.Android);
//		#else
//		BuildPipeline.BuildAssetBundles (fullTmpOutputPath,BuildAssetBundleOptions.None);
//		#endif
		HashSet<string> setStrs = new HashSet<string> ();
		foreach (ABBuildEntity entity in allAssetBundleEntitys) {
			//if(entity.isSelected)
			setStrs.Add (entity.abName);
		}
//		string[] paths = Directory.GetFiles (fullTmpOutputPath);
//		for (int i = 0; i < paths.Length; i++) {
//			string fileName = paths [i].Substring (paths [i].LastIndexOf ("/") + 1);
//			if (!setStrs.Contains (fileName) && fileName != serverCSV)
//				FileManager.DeleteFile (paths [i]);
//		}
		AssetDatabase.Refresh ();
		GetHash ();
		CreateVersion ();
	}

	//	static	string GetMovieHash ()
	//	{
	//		string[] movies = FileManager.GetFiles (movieResourceAssetPath, "*.mp4", SearchOption.AllDirectories);
	//		string str = "";
	//		for (int i = 0; i < movies.Length; i++) {
	//			string fileName = movies [i].Substring (movies [i].LastIndexOf ("/") + 1);
	//			string hash = FileManager.GetFileHash (movies [i]);
	//			long length = new FileInfo (movies [i]).Length;
	////			string lengthStr = GetLengthStr (length);
	//			str += (fileName + "," + length + ",0," + hash + "\r\n");
	//		}
	//		return str;
	//	}
	const string resourcePath = "/Applications/XAMPP/xamppfiles/htdocs/kingofhero/Assetbundles/";

	void CopyToServer ()
	{
//		FileManager.CopyFile (fullTmpOutputPath, resourcePath);
			string subDir = fullTmpOutputPath.Substring (fullTmpOutputPath.LastIndexOf ("/Assetbundles/") + "/Assetbundles/".Length);
		FileManager.CreateDirectory (resourcePath + subDir);
		string[] files = FileManager.GetFiles (fullTmpOutputPath, "*.*", SearchOption.TopDirectoryOnly);
		for (int i = 0; i < files.Length; i++) {
			string fileName = files [i].Substring (fullTmpOutputPath.LastIndexOf ('/') + 1);
			FileManager.CopyFile (files[i],resourcePath + subDir + "/" + fileName);
		}
//		foreach (string dirPath in Directory.GetDirectories(fullTmpOutputPath, "*", SearchOption.AllDirectories)) {
//			Directory.CreateDirectory (dirPath.Replace (fullTmpOutputPath, resourcePath));
//		}
//		foreach (string newPath in Directory.GetFiles(fullTmpOutputPath, "*.*", SearchOption.AllDirectories)) {
//			File.Copy (newPath, newPath.Replace (fullTmpOutputPath, resourcePath), true);
//		}
	}

	void CopyToTemp ()
	{
		FileManager.DeleteFolder (tempResourceAssetPath);
		Directory.CreateDirectory (tempResourceAssetPath);
		for (int i = 0; i < allAssetBundleEntitys.Count; i++) {
			if (allAssetBundleEntitys [i].isSelected) {
				if (FileManager.Exists (fullTmpOutputPath + allAssetBundleEntitys [i].abName))
					FileManager.CopyFile (fullTmpOutputPath + allAssetBundleEntitys [i].abName, tempResourceAssetPath + allAssetBundleEntitys [i].abName);
			}
		}
		AssetDatabase.Refresh ();
	}

	void CopyToResources ()
	{
		for (int i = 0; i < allAssetBundleEntitys.Count; i++) {
			if (allAssetBundleEntitys [i].isSelected) {
				FileManager.CopyFile (fullTmpOutputPath + allAssetBundleEntitys [i].abName, fullResourceAssetPath + allAssetBundleEntitys [i].abName);
			}
		}
		AssetDatabase.Refresh ();
	}

	void CopyToStream ()
	{
		for (int i = 0; i < allAssetBundleEntitys.Count; i++) {
			if (allAssetBundleEntitys [i].isSelected) {
				FileManager.CopyFile (fullTmpOutputPath + allAssetBundleEntitys [i].abName, fullStreamPath + allAssetBundleEntitys [i].abName);
			}
		}
		AssetDatabase.Refresh ();
	}

	void CopyToServerCSV ()
	{
//		FileManager.CopyFile (fullServerCSVPath);
//		string serverCSV = FileManager.ReadString (fullServerCSVPath); 
//		string[] line = serverCSV.Split("\n"[0]);
//		Dictionary<string,VersionCSV> versionDic = new Dictionary<string, VersionCSV> ();
//		for(int i=1;i<line.Length;i++){
//			if (line [i].Trim () == "")
//				continue;
//			VersionCSV v = new VersionCSV ();
//			string[] fields = line [i].Split (","[0]);
//			v.FileName = fields [0].Trim();
//			v.HashCode = fields [3].Trim();
//			v.FileSize = int.Parse (fields[1]);
//			v.IsCSV = int.Parse (fields[2]);
//			versionDic.Add (v.FileName,v);
//		}
//		Debug.Log (versionDic.Count.ToString ());
//		for(int i =0;i<allAssetBundleEntitys.Count;i++){
//			ABBuildEntity abEntity = allAssetBundleEntitys[i];
//			if (!versionDic.ContainsKey (abEntity.abName)) {
//				VersionCSV v = new VersionCSV ();
//				v.FileName = abEntity.abName;
//				v.FileSize = (int)abEntity.realLength;
//				v.HashCode = abEntity.abHash;
//				v.IsCSV = abEntity.abName == "csv.assetbundle" ? 1 : 0;
//				versionDic.Add (abEntity.abName, v);
//			} else {
//				//VersionCSV v = versionDic [abEntity.abName];
//				VersionCSV v = new VersionCSV ();
//				v.FileName = abEntity.abName;
//				v.FileSize = (int)abEntity.realLength;
//				v.HashCode = abEntity.abHash;
//				v.IsCSV = abEntity.abName == "csv.assetbundle" ? 1 : 0;
//				if (v.HashCode!=versionDic [abEntity.abName].HashCode) {
//					versionDic [abEntity.abName] = v;
//				}
//			}
//		}
//		StringBuilder result = new StringBuilder ();
//		string title = "FileName,FileSize,IsCSV,HashCode";
//		result.AppendLine (title);
//		foreach(string key in versionDic.Keys){
//			VersionCSV version = versionDic [key];
//			result.AppendLine (CreateLine (version.FileName, version.FileSize, version.IsCSV, version.HashCode).ToString());
//		}
//		FileManager.WriteString (fullServerCSVPath, result.ToString ());
		AssetDatabase.Refresh ();

//		List<VersionCSV> versions = VersionCSV.LoadDatas (serverCSV);
//		Dictionary<string,VersionCSV> versionDic = new Dictionary<string, VersionCSV> ();
//		foreach(VersionCSV version in versions){
//		if(version.FileName.Trim()==""){
//				continue;
//		}
//			versionDic.Add (version.FileName, version);
//		}
//		bool isChanged = false;
//		foreach (ABBuildEntity abEntity in allAssetBundleEntitys) {
//			if (abEntity.isSelected) {
//				if (!versionDic.ContainsKey (abEntity.abName)) {
//					VersionCSV v = new VersionCSV ();
//					v.FileName = abEntity.abName;
//					v.FileSize = (int)abEntity.realLength;
//					v.HashCode = abEntity.abHash;
//					v.IsCSV = abEntity.abName == "csv.assetbundle" ? 1 : 0;
//					versionDic.Add (abEntity.abName, v);
//					isChanged = true;
//				} else {
//					//VersionCSV v = versionDic [abEntity.abName];
//					VersionCSV v = new VersionCSV ();
//					v.FileName = abEntity.abName;
//					v.FileSize = (int)abEntity.realLength;
//					v.HashCode = abEntity.abHash;
//					v.IsCSV = abEntity.abName == "csv.assetbundle" ? 1 : 0;
//					if (!CompareVersionCSV (v, versionDic [abEntity.abName])) {
//						versionDic [abEntity.abName] = v;
//						isChanged = true;
//					}
//				}
//			}
//		}
//		if (!isChanged)
//			return;
//		StringBuilder result = new StringBuilder ();
//		string title = "FileName,FileSize,IsCSV,HashCode";
//		result.AppendLine (title);
//		foreach(string key in versionDic.Keys){
//			VersionCSV version = versionDic [key];
//			result.AppendLine (CreateLine (version.FileName, version.FileSize, version.IsCSV, version.HashCode).ToString());
//		}
//		FileManager.WriteString (fullServerCSVPath, result.ToString ());
//		AssetDatabase.Refresh ();
//		Debug.Log ("CreateVersion successfully");
	}

	static bool CompareVersionCSV (VersionCSV originVersion, VersionCSV newVersion)
	{
		if (originVersion.FileName != newVersion.FileName || originVersion.FileSize != newVersion.FileSize || originVersion.HashCode != newVersion.HashCode || originVersion.IsCSV != newVersion.IsCSV) {
			Debug.Log (originVersion.FileName + ":" + newVersion.FileName);	
			Debug.Log (originVersion.FileSize + ":" + newVersion.FileSize);	
			Debug.Log (originVersion.HashCode + ":" + newVersion.HashCode);	
			Debug.Log (originVersion.IsCSV + ":" + newVersion.IsCSV);	
			return false;
		}
		return true;
	}

	static void CreateVersion ()
	{
		Caching.CleanCache ();
		StringBuilder result = new StringBuilder ();
		string title = "FileName,FileSize,IsCSV,HashCode";
		result.AppendLine (title);
		foreach (ABBuildEntity abEntity in allAssetBundleEntitys) {
//			if (abEntity.isSelected) {
			if (abEntity.abHash != null && abEntity.abHash.Trim () != "") {
				result.AppendLine (CreateLine (abEntity.abName, new FileInfo (fullTmpOutputPath + abEntity.abName).Length, abEntity.abName == "csv.assetbundle" ? 1 : 0, abEntity.abHash).ToString ());
			}
//			}
		}
//		result.Append (GetMovieHash ());
		FileManager.WriteString (fullServerCSVPath, result.ToString ());
		serverHash = FileManager.GetFileHash (fullServerCSVPath);
		AssetDatabase.Refresh ();
		Debug.Log ("CreateVersion successfully");
	}

	private const string comma = ",";

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
	}
}

public class ABBuildEntity
{
	public string abName;
	public string abHash;
	public long realLength;
	public string length;
	public bool isSelected;
}

