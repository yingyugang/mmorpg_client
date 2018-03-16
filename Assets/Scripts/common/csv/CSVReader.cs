using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader
{
	static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
	static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
	static char[] TRIM_CHARS = { '\"', '"' };

	public static List<Dictionary<string, object>> Read(string file)
	{
		

		var list = new List<Dictionary<string, object>>();

		string path = Application.persistentDataPath + "/Resources/" + file + ".csv";
		string fileContents = "";
		path = path.Replace ("\\","/");
		//Debug.Log("Read CSV: " + path);

		#if READ_LOCAL_CSV
		TextAsset data = Resources.Load (file) as TextAsset;
		fileContents = data.text;
		#else
		if (File.Exists(path))
		{
		Debug.Log("Read SDcard CSV: " + file);
		var sourse = new StreamReader(path);
		fileContents = sourse.ReadToEnd();
		sourse.Close();
		}
		else
		{
		//Debug.Log("Read Asset CSV: " + file);
		TextAsset data = Resources.Load (file) as TextAsset;
		fileContents = data.text;
		}
		#endif


		var lines = Regex.Split (fileContents, LINE_SPLIT_RE);

		if(lines.Length <= 1) return list;

		var header = Regex.Split(lines[0], SPLIT_RE);
		for (var j = 0; j < header.Length; j++) {
			string value = header[j];
			value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
			header [j] = value;
			//Debug.Log (value);
		}

		for(var i=1; i < lines.Length; i++) {

			var values = Regex.Split(lines[i], SPLIT_RE);
			if(values.Length == 0 ||values[0] == "") continue;

			var entry = new Dictionary<string, object>();
			for(var j=0; j < header.Length && j < values.Length; j++ ) {
				string value = values[j];
				value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
//				Debug.Log (value);
				object finalvalue = value;
				int n;
				float f;
				if(int.TryParse(value, out n)) {
					finalvalue = n;
				} else if (float.TryParse(value, out f)) {
					finalvalue = f;
				}
				entry[header[j]] = finalvalue;
			}
			list.Add (entry);
		}
		return list;
	}
}