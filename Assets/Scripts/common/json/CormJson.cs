using UnityEngine;
using System.Collections;
using CSV;

[System.Serializable]
public class CormJson {
	public CormData[]  data;

	[System.Serializable]
	public class CormData : BaseData{
		
		public string title;
		public int waiting_time;
		public int num;
		public int get_at;
	}
}

[System.Serializable]
public class BaseData{
	[CsvColumn (CanBeNull = true)]
	public int id;
}
