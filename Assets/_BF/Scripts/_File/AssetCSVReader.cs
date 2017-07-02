using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class AssetCSVReader : MonoBehaviour
{
	public void Read (AssetBundle assetBundle)
	{
		UnityEngine.Object[] objs = assetBundle.LoadAllAssets (typeof(SoCsv));
		int length = objs.Length;
		for (int i = 0; i < length; i++) {
			UnityEngine.Object obj = objs [i];
			SoCsv csv = obj as SoCsv;
			MemoryStream memoryStream = new MemoryStream (csv.content);
			StreamReader streamReader = new StreamReader (memoryStream);
			FileManager.WriteString (PathConstant.CLIENT_CSV_PATH + csv.fileName + ".csv", streamReader.ReadToEnd ());
		}
	}
}
