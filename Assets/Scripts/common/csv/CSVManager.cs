using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BattleFramework.Data;
using CSV;
using System.IO;

namespace MMO
{
	public class CSVManager 
	{
		private const string CSV_EFFECT = "m_effect";
		private static CsvContext mCsvContext;
		public static Dictionary<int,MEffect> effectDic;
		public static List<MEffect> mEffectList;
		public const string EFFECT_FILE_TITLE = "id,effect_name,assetbundle,description";

		public static string GetCSVPath(string fileName){
			string direct = Application.dataPath + "/Resources/CSV/" ;
			string filePath = direct + fileName + ".csv";
			return filePath;
		}

		static byte[] GetCSV (string fileName)
		{
			//#if UNITY_EDITOR
			//TODO 因为时间关系暂时用Resources，放到固定的文件夹下面，可以编辑最佳。
			string direct = Application.dataPath + "/Resources/CSV/" ;
			string filePath = direct + fileName + ".csv";

			Debug.Log ("filePath:" + filePath);
			if(!FileManager.DirectoryExists(direct)){
				FileManager.CreateDirectory (direct);
			}
			if(!FileManager.FileExists(filePath)){
				FileManager.CreateFile (filePath);
				FileManager.AppendAllText (filePath,EFFECT_FILE_TITLE);
			}
			return File.ReadAllBytes (filePath);
			//#else
			//return ResourcesManager.Ins.GetCSV (fileName);
			//#endif
		}

		public static void StartLoading ()
		{
			mCsvContext = new CsvContext ();
			LoadEffect ();
		}

		public static void LoadEffect(){
			mCsvContext = new CsvContext ();
			mEffectList = CreateCSVList<MEffect> (CSV_EFFECT);
			effectDic = GetDictionary<MEffect> (mEffectList);
		}

		static List<T> CreateCSVList<T> (string csvname) where T:BaseCSVStructure, new()
		{
			var stream = new MemoryStream (GetCSV (csvname));
			var reader = new StreamReader (stream);
			IEnumerable<T> list = mCsvContext.Read<T> (reader);
			return new List<T> (list);
		}

		static Dictionary<int,T> GetDictionary<T> (List<T> list) where T : BaseCSVStructure
		{
			Dictionary<int,T> dic = new Dictionary<int, T> ();
			foreach (T t in list) {
				if (!dic.ContainsKey (t.id))
					dic.Add (t.id, t);
			}
			return dic;
		}
	}
}
