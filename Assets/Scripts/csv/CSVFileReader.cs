using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

namespace BattleFramework.Data
{
	//Core of csv reader;
	public class CSVFileReader
	{
		
		private List<Row> m_mapInfo = null;
		private List<string> m_listColumns = null;
		private List<string> m_listTypes = null;
		private List<string> m_listColumnComments = null;

		public List<Row> mapData{ get { return m_mapInfo; } }
//csv datas,read only;
		public int rowAmount{ get { return m_mapInfo.Count; } }
//csv row count,read only;
		public List<string> listColumnName{ get { return m_listColumns; } }
//csv column names,read only;
		public List<string> listColumnComment{ get { return m_listColumnComments; } }
//csv column comment,read only;
		public List<string> listColumnType { get { return m_listTypes; } }
//csv column comment,read only;

		public int columnAmount{ get { return m_listColumns.Count; } }
//csv column count,read only;
		public const char mSplitMark = ',';

		public string PathFile { get; set; }
//csv path,base on Resources folder;


		public CSVFileReader ()
		{
			_Initialize ();
		}

		void _Initialize ()
		{
			m_mapInfo = new List<Row> ();
			m_listColumns = new List<string> ();
			m_listColumnComments = new List<string> ();
			m_listTypes = new List<string> ();
		}

		public void Clear ()
		{
			mapData.Clear ();
			listColumnName.Clear ();
		}

		public void Open (TextAsset textFile)
		{
			string text = textFile.text;
			Open (text);
		}

		public void Open (string text)
		{
			string[] lineArray = text.Split ("\n" [0]);
			m_listColumns.AddRange (lineArray [0].Trim ().Replace ("\"", "").Split ("," [0]));
			m_listTypes.AddRange (lineArray [1].Trim ().Replace ("\"", "").ToLower ().Split ("," [0]));

			for (int i = 2; i < lineArray.Length; i++) {
				Row trunk = new Row ();
				StringBuilder sb = new StringBuilder ();
				string strLine = lineArray [i].Trim ();
				if (strLine.Length == 0)
					continue;
				bool isJoin = false;
				string[] data = new string[m_listColumns.Count];
				int index = 0;
				for (int j = 0; j < strLine.Length; j++) {
					if (strLine [j] == '"') {
						if (isJoin) {
							isJoin = false;
						} else {
							isJoin = true;
						}
						continue;
					}
					if (isJoin && strLine [j] == ',') {
						sb.Append (',');
					} else if (strLine [j] == ',') {
						data [index] = sb.ToString ();
						index++;
						sb = new StringBuilder ();
					} else {
						sb.Append (strLine [j]);
					}
				}
				data [index] = sb.ToString ();
				trunk.data.AddRange (data);
				m_mapInfo.Add (trunk);
			}
		}

		static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
		static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
		static char[] TRIM_CHARS = { '\"', '"' };

		public static List<Dictionary<string, object>> Read (string path)
		{
			var list = new List<Dictionary<string, object>> ();
			//string path = Application.persistentDataPath + "/Resources/" + file + ".csv";
			string fileContents = "";
			//path = path.Replace ("\\","/");
			//Debug.Log("Read CSV: " + path);
//			#if READ_LOCAL_CSV
//			TextAsset data = Resources.Load (file) as TextAsset;
//			fileContents = data.text;
//			#else
//			if (File.Exists(path))
//			{
//				Debug.Log("Read SDcard CSV: " + file);
//				var sourse = new StreamReader(path);
//				fileContents = sourse.ReadToEnd();
//				sourse.Close();
//			}
//			else
//			{
//				//Debug.Log("Read Asset CSV: " + file);
//				TextAsset data = Resources.Load (file) as TextAsset;
//				fileContents = data.text;
//			}
//			#endif

			if (File.Exists (path)) {
				var sourse = new StreamReader (path);
				fileContents = sourse.ReadToEnd ();
				sourse.Close ();
			}
			bool isMaohaoBegin = false;
			for (int i = 0; i < fileContents.Length; i++) {
				if (fileContents [i] == '\"') {
					Debug.Log ("isMaohaoBegin");
					isMaohaoBegin = !isMaohaoBegin;
				}
				if (fileContents [i] == '\n') {
					if (isMaohaoBegin) {
						fileContents = fileContents.Remove (i, 1);
						fileContents = fileContents.Insert (i, "|");
					}
				}
			}
			var lines = Regex.Split (fileContents, LINE_SPLIT_RE);
			if (lines.Length <= 1)
				return list;
			var header = Regex.Split (lines [0], SPLIT_RE);
			for (var j = 0; j < header.Length; j++) {
				string value = header [j];
				value = value.TrimStart (TRIM_CHARS).TrimEnd (TRIM_CHARS).Replace ("\\", "");
				header [j] = value;
				//Debug.Log (value);
			}

			for (var i = 1; i < lines.Length; i++) {
				var values = Regex.Split (lines [i], SPLIT_RE);
				if (values.Length == 0 || values [0] == "")
					continue;
				var entry = new Dictionary<string, object> ();
				for (var j = 0; j < header.Length && j < values.Length; j++) {
					string value = values [j];
					value = value.TrimStart (TRIM_CHARS).TrimEnd (TRIM_CHARS).Replace ("\\", "").Replace("/n", "\\n");

					//				Debug.Log (value);
					object finalvalue = value;
					int n;
					float f;
					if (int.TryParse (value, out n)) {
						finalvalue = n;
					} else if (float.TryParse (value, out f)) {
						finalvalue = f;
					}
					entry [header [j]] = finalvalue;
				}
				list.Add (entry);
			}
			return list;
		}
	}

	public class Row
	{
		public List<string> data = new List<string> ();

		public Dictionary<string,string> dicData = new Dictionary<string, string> ();

		int GetIndex (System.Enum key)
		{
			if (data == null)
				return -1;
			int value = Convert.ToInt32 (key);
			if (value < 0 || value >= data.Count)
				return -1;
			return value;
		}

		public bool GetIntValue (System.Enum key, out int value)
		{
			int index = GetIndex (key);
			if (index != -1) {
				if (System.Int32.TryParse (data [index], out value))
					return true;
				float fValue;
				if (GetFloatValue (key, out fValue))
					value = (int)fValue;
			}
			Debug.LogWarning ("GetIntValue " + key + " false!");
			value = -1;
			return false;
		}

		public bool GetFloatValue (System.Enum key, out float value)
		{
			int index = GetIndex (key);
			if (index != -1) {
				if (System.Single.TryParse (data [index], out value)) {
					return true;
				}
			}
			Debug.LogWarning ("GetFloatValue " + key + " false!");
			value = -1;
			return false;
		}

		public bool GetBoolValue (System.Enum key, out bool value)
		{
			int index = GetIndex (key);
			if (index != -1) {
				if (System.Boolean.TryParse (data [index], out value))
					return true;
			}
			Debug.LogWarning ("GetBoolValue " + key + " false!");
			value = false;
			return false;  
		}

		public bool GetStringValue (System.Enum key, out string value)
		{
			int index = GetIndex (key);
			if (index != -1) {
				value = data [index];
				return true;
			}
			Debug.LogWarning ("GetStringValue " + key + " false!");
			value = "";
			return false;
		}

	}

}
