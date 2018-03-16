using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using BattleFramework.Data;
using CSV;

public class CSVToJsonWin : EditorWindow
{
	string csvDirection = @"Assets/CSV";

	string jsonFilePath = @"Assets/Json";

	//	static CSVToJson csvWin;
//	[MenuItem ("Tools/Csv --> Json")]
	static void AddWindow ()
	{
		//CreateClassFromCSV csvWin = EditorWindow.GetWindowWithRect<CreateClassFromCSV>(new Rect(new Vector2(Screen.width/2 - 400,Screen.height/2 - 300) , new Vector2(800,600)),false,"CSV To Class",true);
		CSVToJsonWin csvWin = EditorWindow.GetWindowWithRect<CSVToJsonWin> (new Rect (0, 0, 800, 600), true, "Csv --> Json", true);
		csvWin.Init ();
	}

	Vector2 srollPos;

	void OnGUI ()
	{
		GUILayout.BeginHorizontal ();
		GUI.color = Color.green;
		GUILayout.Label ("Csv Path:" + csvDirection);
		GUI.color = Color.white;
		if (GUILayout.Button ("Select", GUILayout.Width (100))) {
			Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object> (csvDirection);
		}
		GUILayout.FlexibleSpace (); 
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUI.color = Color.green;
		GUILayout.Label ("Json Path:" + jsonFilePath);
		GUI.color = Color.white;
		if (GUILayout.Button ("Select", GUILayout.Width (100))) {
			Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object> (jsonFilePath);
		}
		GUILayout.FlexibleSpace (); 
		GUILayout.EndHorizontal ();
		GUILayout.Space (20);

		srollPos = EditorGUILayout.BeginScrollView (srollPos, GUILayout.Height (450));
		for (int i = 0; i < mCSVDatas.Count; i++) {
			GUILayout.BeginHorizontal ();
			mCSVDatas [i].isSelect = GUILayout.Toggle (mCSVDatas [i].isSelect, "");
			GUILayout.Label (mCSVDatas [i].path);
			GUILayout.FlexibleSpace (); 
			GUILayout.EndHorizontal ();
		}
		EditorGUILayout.EndScrollView ();
		GUILayout.Space (20);

		GUI.color = Color.white;
		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("CoverToJson", GUILayout.Width (100))) {
			int count = 0;
			foreach (CSVData data in mCSVDatas) {
				if (data.isSelect) {
					CoverToJson (data.path);
					count++;
				}
			}
			SetJsonAB ();
			Debug.Log ("==================================");
			Debug.Log (count + " class has been created!");
			Debug.Log ("==================================");
//			csvWin.Close ();
		}
		if (GUILayout.Button ("SelectAll", GUILayout.Width (100))) {
			foreach (CSVData data in mCSVDatas) {
				data.isSelect = true;
			}
		}
		if (GUILayout.Button ("UnSelectAll", GUILayout.Width (100))) {
			foreach (CSVData data in mCSVDatas) {
				data.isSelect = false;
			}
		}
//		if (GUILayout.Button ("Test",GUILayout.Width(100))) {
//			string csvDirectionPath = Application.dataPath.Replace ("Assets","") + csvDirection;
//			csvDirectionPath += "/m_convention.csv";
//			Debug.Log (csvDirectionPath);
//			CoverToJson1 (csvDirectionPath);
//		}
		GUILayout.EndHorizontal ();
	}

	void SetJsonAB ()
	{
		string jsonPath = Application.dataPath + "/Json";
		Debug.Log (jsonPath);
		string[] files = FileManager.GetFiles (jsonPath, "*", SearchOption.TopDirectoryOnly);
		Debug.Log (files.Length);
		foreach (string file in files) {
			string subDir1 = file.Substring (file.LastIndexOf ("/Assets/") + 1);
			if (file.LastIndexOf (".meta") == -1) {
				AssetImporter assetImporter = AssetImporter.GetAtPath (subDir1);
				if (assetImporter != null) {
					assetImporter.SetAssetBundleNameAndVariant ("json", "assetbundle");
				}
			}
		}
	}


	string[] mCsvFiles;
	List<CSVData> mCSVDatas;

	void Init ()
	{
		mCSVDatas = new List<CSVData> ();
		string csvDirectionPath = Application.dataPath.Replace ("Assets", "") + csvDirection;
		//Debug.Log (csvDirectionPath);
		mCsvFiles = Directory.GetFiles (csvDirectionPath, "*.csv", SearchOption.AllDirectories);

		for (int i = 0; i < mCsvFiles.Length; i++) {

			if (mCsvFiles [i].IndexOf ("m_convention") != -1 || mCsvFiles [i].IndexOf ("m_help") != -1) {
				continue;
			} else {
				int index = mCsvFiles [i].IndexOf ("Assets");
				CSVData data = new CSVData ();
				data.path = mCsvFiles [i].Substring (index);
				data.isSelect = false;
				mCSVDatas.Add (data);
			}
		}

	}

	public static void CoverAnimationJson (string path,string outName)
	{
//		int index = path.LastIndexOf("/");
//		string realName = path.Replace (".csv","");
//		if(index!=-1){
//			realName = path.Substring(index + 1).Replace (".csv","");
//		}
		List<Dictionary<string,object>> datas = CSVFileReader.Read (@path);
		Debug.Log (datas.Count);
//		string fileName = path.Replace (".csv", ".json");
		if (File.Exists (outName)) {
			Debug.Log ("Delete " + outName);
			File.Delete (outName);
		}
		StreamWriter file = new StreamWriter (outName, false);
		file.WriteLine ("{\"data\":[");
		int j = 0;
		foreach (Dictionary<string,object> data in datas) {
			file.Write ("{");
			int i = 0;
			foreach (string key in data.Keys) {
				file.Write ("\"" + key.Trim() + "\"" + ":" + "\"" + data [key] + "\"");
				i++;
				if (i < data.Keys.Count)
					file.Write (",");
			}
			j++;
			if (j < datas.Count) {
				file.Write ("}");
				file.WriteLine (",");
			} else {
				file.WriteLine ("}");
			}
		}
		file.WriteLine ("]}");
		file.Flush ();
		file.Close ();
//		AssetDatabase.ImportAsset (fileName);
	}

	void CoverToJson (string path)
	{
		int index = path.LastIndexOf ("/");
		string realName = path.Replace (".csv", "");
		if (index != -1) {
			realName = path.Substring (index + 1).Replace (".csv", "");
		}
		List<Dictionary<string,object>> datas = CSVFileReader.Read (path);
		string fileName = jsonFilePath + "/" + realName + ".json";
		if (!Directory.Exists (jsonFilePath)) {
			Directory.CreateDirectory (jsonFilePath);
		}
		if (File.Exists (fileName)) {
			Debug.Log ("Delete " + fileName);
			File.Delete (fileName);
		}
		StreamWriter file = new StreamWriter (fileName, false);
		file.WriteLine ("{\"data\":[");
		int j = 0;
		foreach (Dictionary<string,object> data in datas) {

			file.Write ("{");
			int i = 0;
			foreach (string key in data.Keys) {
				file.Write ("\"" + key + "\"" + ":" + "\"" + data [key] + "\"");
				i++;
				if (i < data.Keys.Count)
					file.Write (",");
			}
			j++;
			if (j < datas.Count) {
				file.Write ("}");
				file.WriteLine (",");
			} else {
				file.WriteLine ("}");
			}
		}
		file.WriteLine ("]}");
		file.Flush ();
		file.Close ();
		AssetDatabase.ImportAsset (fileName);
	}
	//
	//	void CoverToJson1(string path){
	//		CsvContext csv = new CsvContext ();
	//		IEnumerable<ConventionCSVStructure> datas = csv.Read<ConventionCSVStructure> (path);
	//
	//		int index = path.LastIndexOf("/");
	//		string realName = path.Replace (".csv","");
	//		if(index!=-1){
	//			realName = path.Substring(index + 1).Replace (".csv","");
	//		}
	////		List<Dictionary<string,object>> datas = CSVFileReader.Read (path);
	//		string fileName = jsonFilePath + "/" + realName + ".json";
	//		if (!Directory.Exists (jsonFilePath)) {
	//			Directory.CreateDirectory (jsonFilePath);
	//		}
	//		if (File.Exists (fileName)) {
	//			Debug.Log ("Delete " + fileName);
	//			File.Delete (fileName);
	//		}
	//		StreamWriter file = new StreamWriter (fileName, false);
	//		ConventionJson json = new ConventionJson();
	//		List <ConventionCSVStructure> ccs = new List<ConventionCSVStructure> (datas);
	//
	//		json.data = ccs.ToArray();
	//
	//		foreach(ConventionCSVStructure cc in ccs){
	//			Debug.Log (cc.description);
	//		}
	//
	//		string jsonStr = JsonUtility.ToJson(json);
	//		Debug.Log (jsonStr);
	//
	//
	//
	//		file.WriteLine ("{data:[");
	//
	//		for(int j=0;j < json.data.Length;j++){
	//			file.Write ("{");
	//			int i = 0;
	//			foreach(string key in data.Keys){
	//				file.Write ("\"" + key + "\"" + ":" + "\"" + data[key] + "\"");
	//				i++;
	//				if(i<data.Keys.Count)
	//					file.Write (",");
	//			}
	//			j++;
	//			if (j < datas.Count) {
	//				file.Write ("}");
	//				file.WriteLine (",");
	//			} else {
	//				file.WriteLine ("}");
	//			}
	//		}
	//
	//		foreach(ConventionCSVStructure data in datas){
	//
	//			file.Write ("{");
	//			int i = 0;
	//			foreach(string key in data.Keys){
	//				file.Write ("\"" + key + "\"" + ":" + "\"" + data[key] + "\"");
	//				i++;
	//				if(i<data.Keys.Count)
	//					file.Write (",");
	//			}
	//			j++;
	//			if (j < datas.Count) {
	//				file.Write ("}");
	//				file.WriteLine (",");
	//			} else {
	//				file.WriteLine ("}");
	//			}
	//		}
	//		file.WriteLine ("]}");
	//		file.Flush ();
	//		file.Close ();
	//		AssetDatabase.ImportAsset (fileName);
	//	}

	class CSVData
	{
		public bool isSelect;
		public string path;
	}
}
