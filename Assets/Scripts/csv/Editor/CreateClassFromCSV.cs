using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

namespace BattleFramework.Data
{
	//Support type: int,float,string,bool,List<int>,Vector3
	public class CreateClassFromCSV : EditorWindow
	{
		string csvDirection = @"Assets/CSV";

		string classFilePath = @"Assets/Scripts/common/csv/entity";

		string jsonFilePath = @"Assets/Json";

		//string tempatePath = @"Assets/Scripts/csv/Editor/CSVScriptTemplate";

		static CreateClassFromCSV csvWin;
		//[MenuItem("Tools/CSV Mananger")]
		static void AddWindow ()
		{
			//CreateClassFromCSV csvWin = EditorWindow.GetWindowWithRect<CreateClassFromCSV>(new Rect(new Vector2(Screen.width/2 - 400,Screen.height/2 - 300) , new Vector2(800,600)),false,"CSV To Class",true);
			csvWin = EditorWindow.GetWindow<CreateClassFromCSV>(true,"Csv To Class",true);
			csvWin.Init ();
		}

		string[] mCsvFiles;
		List<CSVData> mCSVDatas;

		void Init(){
			mCSVDatas = new List<CSVData> ();
			string csvDirectionPath = Application.dataPath.Replace ("Assets","") + csvDirection;
			//CustomDebug.Log (csvDirectionPath);
			mCsvFiles = Directory.GetFiles (csvDirectionPath,"*.csv", SearchOption.AllDirectories);

			for(int i=0;i<mCsvFiles.Length;i++){
				int index = mCsvFiles[i].IndexOf ("Assets");
				CSVData data = new CSVData ();
				data.path = mCsvFiles [i].Substring (index);
				data.isSelect = false;
				mCSVDatas.Add (data);
			}

		}

        //int top;
		//int height;
		//int index;
		//int width;
		//int offsetY = 5;
		//FileInfo[] filePaths;
		Vector2 srollPos;

		void OnGUI ()
		{
			//top = 20;
			//height = 30;
			//index = 0;
			//width = 200;
//			if(GUI.Button(new Rect(60,top + (height + 5) * index,width,height),"select CSV folder"))
//			{
//				csvDirection = EditorUtility.OpenFolderPanel("Please select folder" , csvDirection , "");
//				filePaths = null;
//				if(csvDirection!=null && csvDirection!= "")
//				{
//
//				}
//			}
//			index ++;
//			GUI.color = Color.green;
//			GUI.Label (new Rect(60,top + (height + offsetY) * index,300,height),"Csv Path:" + csvDirection);
//			GUI.color = Color.white;
//			if (GUI.Button ( new Rect(60 + 300 + 10,top + (height + offsetY) * index,100,height) ,"Select")) {
//				Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object> (csvDirection);
//			}
//			index++;
//			GUI.color = Color.green;
//			GUI.Label (new Rect(60,top + (height + offsetY) * index,300,height),"Class Path:" + classFilePath);
//			GUI.color = Color.white;
//			if (GUI.Button ( new Rect(60 + 300 + 10,top + (height + offsetY) * index,100,height) ,"Select")) {
//				Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object> (classFilePath);
//			}
//			index++;

			GUILayout.BeginHorizontal ();
			GUI.color = Color.green;
			GUILayout.Label ("Csv Path:" + csvDirection);
			GUI.color = Color.white;
			if (GUILayout.Button ("Select",GUILayout.Width(100))) {
				Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object> (csvDirection);
			}
			GUILayout.FlexibleSpace(); 
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUI.color = Color.green;
			GUILayout.Label ("Class Path:" + classFilePath);
			GUI.color = Color.white;
			if (GUILayout.Button ("Select",GUILayout.Width(100))) {
				Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object> (classFilePath);
			}
			GUILayout.FlexibleSpace(); 
			GUILayout.EndHorizontal ();
			GUILayout.Space (20);

			srollPos = EditorGUILayout.BeginScrollView(srollPos,GUILayout.Height(500));
			for(int i =0;i<mCSVDatas.Count;i++){
				GUILayout.BeginHorizontal ();
				mCSVDatas[i].isSelect = GUILayout.Toggle (mCSVDatas[i].isSelect,"");
				GUILayout.Label (mCSVDatas[i].path);
				GUILayout.FlexibleSpace(); 
				GUILayout.EndHorizontal ();
			}
			EditorGUILayout.EndScrollView ();
			GUILayout.Space (20);

			GUI.color = Color.white;
			GUILayout.BeginHorizontal ();
//			if (GUILayout.Button ("CreateClass",GUILayout.Width(100))) {
//				//DirectoryInfo dir = new DirectoryInfo (csvDirection);
//				//filePaths = dir.GetFiles ("*.csv");
//				//if (filePaths != null) {
//					//foreach (FileInfo file in filePaths) {
//					//	Create (file);
//					//}
//					//格式化生成DataCenter
//					//CreateDataCenter (filePaths);
//				//}
//				int count = 0;
//				foreach(CSVData data in mCSVDatas){
//					if (data.isSelect) {
//						Create (data.path);
//						count++;
//					}
//				}
//				CustomDebug.Log ("==================================");
//				CustomDebug.Log (count + " class has bean created!");
//				CustomDebug.Log ("==================================");
//				csvWin.Close ();
//			}
			if (GUILayout.Button ("CoverToJson",GUILayout.Width(100))) {
				//DirectoryInfo dir = new DirectoryInfo (csvDirection);
				//filePaths = dir.GetFiles ("*.csv");
				//if (filePaths != null) {
				//foreach (FileInfo file in filePaths) {
				//	Create (file);
				//}
				//CreateDataCenter (filePaths);
				//}
				int count = 0;
				foreach(CSVData data in mCSVDatas){
					if (data.isSelect) {
						CoverToJson (data.path);
						count++;
					}
				}
//				CustomDebug.Log ("==================================");
//				CustomDebug.Log (count + " class has bean created!");
//				CustomDebug.Log ("==================================");
				csvWin.Close ();
			}
			if (GUILayout.Button ("SelectAll",GUILayout.Width(100))) {
				foreach(CSVData data in mCSVDatas){
					data.isSelect = true;
				}
			}
			if (GUILayout.Button ("UnSelectAll",GUILayout.Width(100))) {
				foreach(CSVData data in mCSVDatas){
					data.isSelect = false;
				}
			}

			if (GUILayout.Button ("Test",GUILayout.Width(100))) {
				TestJson ();
			}


			GUILayout.EndHorizontal ();
		}

		string GetClassName(string path){
			int index = path.LastIndexOf("/");
			string className = path.Replace (".csv","");
			if(index!=-1){
				className = path.Substring(index + 1).Replace (".csv","");
			}
			string head = className.Substring (0, 1).ToUpper ();
			string body = className.Substring (1, className.Length - 1);
			className = head + body;
			return className;
		}

		void CoverToJson(string path){
			int index = path.LastIndexOf("/");
			string realName = path.Replace (".csv","");
			if(index!=-1){
				realName = path.Substring(index + 1).Replace (".csv","");
			}
			List<Dictionary<string,object>> datas = CSVFileReader.Read (path);
			string fileName = jsonFilePath + "/" + realName + ".json";
			if (!Directory.Exists (jsonFilePath)) {
				Directory.CreateDirectory (jsonFilePath);
			}
			if (File.Exists (fileName)) {
//				CustomDebug.Log ("Delete " + fileName);
				File.Delete (fileName);
			}
			StreamWriter file = new StreamWriter (fileName, false);
			file.WriteLine ("{\"data\":[");
			int j = 0;
			foreach(Dictionary<string,object> data in datas){

				file.Write ("{");
				int i = 0;
				foreach(string key in data.Keys){
					file.Write ("\"" + key + "\"" + ":" + "\"" + data[key] + "\"");
					i++;
					if(i<data.Keys.Count)
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

		void Create (string path)
		{

			string fileName = path;
//			int index = fileName.LastIndexOf("/");
			string className = GetClassName (path);
			fileName = classFilePath + "/" + className + ".cs";
//			CustomDebug.Log ("fileName: " + fileName + ";" + "className: " + className);
			if (!Directory.Exists (classFilePath)) {
				Directory.CreateDirectory (classFilePath);
			}
			if (File.Exists (fileName)) {
//				CustomDebug.Log ("Delete " + fileName);
				File.Delete (fileName);
			}
			CSVFileReader csvFile = new CSVFileReader ();
			csvFile.Open (AssetDatabase.LoadAssetAtPath<TextAsset>(path));
			for (int i = 0; i < csvFile.listColumnName.Count; i++)
			{
				csvFile.listColumnName[i] = Regex.Replace(csvFile.listColumnName[i], @"\s", "");
			}

			StreamWriter file = new StreamWriter (fileName, false);
			file.WriteLine ("using System;");
			file.WriteLine ("using System.Collections.Generic;");
			file.WriteLine ("using System.Collections;");
			file.WriteLine ("using UnityEngine;");
			file.WriteLine ("namespace BattleFramework.Data{");
			file.WriteLine ("    [System.Serializable]");
			file.WriteLine ("    public class " + className + " {");
            //string resourcesPath = "CSV/" + fileInfo.Name.Replace(".csv","");
			//file.WriteLine ("        public static string u = \"" + path.Replace(".csv","") + "\";");
			file.WriteLine ("        public static string[] columnNameArray = new string[" + csvFile.listColumnName.Count + "];");
			file.WriteLine ("        public static List<" + className + "> LoadDatas(string text){");
			file.WriteLine ("            CSVFileReader csvFile = new CSVFileReader();");
			//file.WriteLine ("            csvFile.Open(Resources.Load<TextAsset>(csvFilePath));");
			file.WriteLine ("            csvFile.Open(text);");
			file.WriteLine ("            List<" + className + "> dataList = new List<" + className + ">();");
			//file.WriteLine ("            string[] strs;");
			//file.WriteLine ("            string[] strsTwo;");
			//file.WriteLine ("            List<int> listChild;");
			file.WriteLine ("            columnNameArray = new string[" + csvFile.listColumnName.Count + "];");
			file.WriteLine ("            for(int i = 0;i < csvFile.mapData.Count;i ++){");
            file.WriteLine ("                if (csvFile.mapData[i].data.Count < columnNameArray.Length){");
            file.WriteLine ("                    CustomDebug.LogError(\"csvFile.mapData[i].data.Count :\" + csvFile.mapData[i].data.Count + \" columnNameArray.Length:\" + columnNameArray.Length);");
            file.WriteLine ("                    continue;");
            file.WriteLine ("                }");
            file.WriteLine ("                " + className + " data = new " + className + "();");
			List<string> fields = new List<string> ();
			string type;
			string fieldName;
                for (int i = 0; i < csvFile.listColumnName.Count; i ++) {
                fieldName = csvFile.listColumnName [i];
                //default csv column name is column name plus column type ,e.g NAME_STRING,ID_INT;
                //so we can know the column type and then generate class field;
				type = csvFile.listColumnType[i];
                /**
                if (columnName.LastIndexOf ("_") != -1) {
					type = columnName.Substring (columnName.LastIndexOf ("_") + 1, columnName.Length - columnName.LastIndexOf ("_") - 1).ToUpper ();
				} else {
					type = "";
					fieldName = columnName;
				}
				if (type.ToUpper () == "INT") {
					type = "int";
					fieldName = columnName.Substring (0, columnName.LastIndexOf ("_"));

				} else if (type.ToUpper () == "FLOAT") {
					type = "float";
					fieldName = columnName.Substring (0, columnName.LastIndexOf ("_"));
				} else if (type.ToUpper () == "BOOL") {
					type = "bool";
					fieldName = columnName.Substring (0, columnName.LastIndexOf ("_"));
				} else if (type.ToUpper () == "STRING") {
					type = "string";
					fieldName = columnName.Substring (0, columnName.LastIndexOf ("_"));
				} else if (type.ToUpper () == "LIST") {
					type = "List<int>";
					fieldName = columnName.Substring (0, columnName.LastIndexOf ("_"));
				} else if (type.ToUpper () == "VECTOR3") {
					type = "Vector3";
					fieldName = columnName.Substring (0, columnName.LastIndexOf ("_"));
				} else {
					type = "string";
					fieldName = columnName;
				}
                **/
                if (type == "String")
                {
                    type = "string";
                }
                else if (type == "boolean")
                {
                    type = "bool";
                }


				if (type == "string") {
					file.WriteLine ("                data." + fieldName + " = " + "csvFile.mapData[i].data[" + i + "];");
				} else if (type == "Vector3") {
					file.WriteLine ("                data." + fieldName + "= new Vector3();");
					file.WriteLine ("                strs = " + "csvFile.mapData[i].data[" + i + "].Split(new char[1]{\',\'});");
					file.WriteLine ("                    data." + fieldName + ".x = (float.Parse(strs[0]));");
					file.WriteLine ("                    data." + fieldName + ".y = (float.Parse(strs[1]));");
					file.WriteLine ("                    data." + fieldName + ".z = (float.Parse(strs[2]));");
				} else if (type == "List<int>") {
					file.WriteLine ("                data." + fieldName + "= new List<int>();");
//					file.WriteLine ("                strs = " + "csvFile.mapData[i].data[" + i + "].Split(new char[1]{\';\'});");
//					file.WriteLine ("                for(int j=0;j<strs.Length;j++){");
					file.WriteLine ("                      listChild = new List<int>();");
					file.WriteLine ("                      strs = csvFile.mapData[i].data[" + i + "].Split(new char[1]{\',\'});");
					file.WriteLine ("                      for(int m=0;m<strsTwo.Length;m++){");	
					file.WriteLine ("                            listChild.Add(int.Parse(strs[m]));");
					file.WriteLine ("                      }");			
					file.WriteLine ("                    data." + fieldName + ".Add(listChild);");
					file.WriteLine ("                }");
				} else {
					file.WriteLine ("                " + type + ".TryParse(" + "csvFile.mapData[i].data[" + i + "],out data." + fieldName + ")" + ";");
				}

				fields.Add ("        public " + type + " " + fieldName + ";");//+ csvFile.listColumnComment [i]
                //file.WriteLine ("                columnNameArray [" + i + "] = \"" + fieldName + "\";");
			}
			file.WriteLine ("                dataList.Add(data);");
			file.WriteLine ("            }");
			file.WriteLine ("            return dataList;");
			file.WriteLine ("        }");

            /**
			file.WriteLine ("  ");
			file.WriteLine ("        public static " + className + " GetByID (int id,List<" + className + "> data)");
			file.WriteLine ("        {");
			file.WriteLine ("            foreach (" + className + " item in " + "data) {");
			file.WriteLine ("                if (id == item.id) {");
			file.WriteLine ("                     return item;");
			file.WriteLine ("                }");
			file.WriteLine ("            }");
			file.WriteLine ("            return null;");
			file.WriteLine ("        }");
			file.WriteLine ("  ");
            **/

			for (int i=0; i<fields.Count; i++) {
				file.WriteLine (fields [i]);
			}

			file.WriteLine ("    }");

			file.WriteLine ("    [System.Serializable]");
			file.WriteLine ("    public class " + className + "_Josn" + " {");
			file.WriteLine ("        public List<" + className + ">  data;");
			file.WriteLine ("    }");

			file.WriteLine ("}");



			file.Flush ();
			file.Close ();
			AssetDatabase.ImportAsset (fileName);
//			CustomDebug.Log ("Create " + fileName);
		}
		/*
		void CreateDataCenter (FileInfo[] filePathsAll)
		{
			StreamWriter file = new StreamWriter (dataCenterFilePath, false);
			file.WriteLine ("using UnityEngine;");
			file.WriteLine ("using System.Collections;");
			file.WriteLine ("using System.Collections.Generic;");
			file.WriteLine (" ");
			
			file.WriteLine ("namespace BattleFramework.Data");
			file.WriteLine ("{");
			file.WriteLine ("    public class DataCenter : MonoBehaviour");
			file.WriteLine ("    {");
			file.WriteLine ("        static DataCenter instance;");
			file.WriteLine ("  ");

			//list<CSV>
			foreach (FileInfo nowfile in filePathsAll) {
				string fileName = nowfile.Name;
				string className = fileName.Replace (".csv", "");
				string head = className.Substring (0, 1).ToUpper ();
				string body = className.Substring (1, className.Length - 1);

				string classNameLower = "list_" + head + body;
				string classNameUpper = head + body;  
				file.WriteLine ("        public List<" + classNameUpper + "> list_" + classNameUpper + ";");
			}
			file.WriteLine ("  ");
			CustomDebug.Log ("list<CSV>");


			//SingleTon ()
			file.WriteLine ("        public static DataCenter SingleTon ()");
			file.WriteLine ("        {");
			file.WriteLine ("            if (instance == null) {");
			file.WriteLine ("                CustomDebug.Log (\"new _DataCenter\");");
			file.WriteLine ("                GameObject go = new GameObject (\"_DataCenter\");");
			file.WriteLine ("                DataCenter dataCenter = go.AddComponent<DataCenter> ();");
			file.WriteLine ("                dataCenter.LoadCSV ();");
			file.WriteLine ("                DontDestroyOnLoad (go);");
			file.WriteLine ("                instance = dataCenter;");
			file.WriteLine ("            }");
			file.WriteLine ("            return instance;");
			file.WriteLine ("        }");

			file.WriteLine ("   ");
			file.WriteLine ("   ");
			CustomDebug.Log ("//SingleTon ()");

			//LoadCSV
			file.WriteLine ("        public void LoadCSV ()");
			file.WriteLine ("        {");
			foreach (FileInfo nowfile in filePathsAll) {
				string fileName = nowfile.Name;
				string className = fileName.Replace (".csv", "");
				string head = className.Substring (0, 1).ToUpper ();
				string body = className.Substring (1, className.Length - 1);
				
				string classNameLowerList = "list_" + head + body;
				string classNameUpper = head + body;  
				file.WriteLine ("            " + classNameLowerList + " = " + classNameUpper + ".LoadDatas ();");
			}
			file.WriteLine ("        }");
			CustomDebug.Log ("//LoadCSV");

			//get id  ITEM
			file.WriteLine ("  ");
			file.WriteLine ("  ");
			file.WriteLine ("//*****************get id Item***********************************");
			foreach (FileInfo nowfile in filePathsAll) {
				string fileName = nowfile.Name;
				string className = fileName.Replace (".csv", "");
				string head = className.Substring (0, 1).ToUpper ();
				string body = className.Substring (1, className.Length - 1);
				
				string classNameLowerList = "list_" + head + body;
				string classNameUpper = head + body;  
				file.WriteLine ("  ");
				file.WriteLine ("//" + classNameUpper + "-----------------------------------------");
				file.WriteLine ("        public " + classNameUpper + " CSV_" + classNameUpper + " (int id)");
				file.WriteLine ("        {");
				file.WriteLine ("            foreach (" + classNameUpper + " item in " + classNameLowerList + ") {");
				file.WriteLine ("                if (id == item.id) {");
				file.WriteLine ("                return item;");
				file.WriteLine ("                }");
				file.WriteLine ("            }");
				file.WriteLine ("            return null;");
				file.WriteLine ("        }");
				file.Flush ();
			}
			file.WriteLine (" ");
			file.WriteLine (" ");
			file.WriteLine ("    }");
			file.WriteLine ("}");
			file.Flush ();
			file.Close ();
			CustomDebug.Log ("//get id  ITEM");
		}
		*/

		void TestJson(){
//			string realName = "m_flower";
//			string fileName = jsonFilePath + "/" + realName + ".json";
//			string str = FileManager.ReadString (fileName);
//			CustomDebug.Log (str);
//			FlowerJson json = JsonUtility.FromJson<FlowerJson> (str);
		}


	}


	 class CSVData{
		public bool isSelect;
		public string path;
	}
}
