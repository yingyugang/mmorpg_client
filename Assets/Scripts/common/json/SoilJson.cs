using UnityEngine;
using System.Collections;

[System.Serializable]
public class SoilJson {
	public SoilData[]  data;

	[System.Serializable]
	public class SoilData:BaseData{
		public string title;
		public int need_num;
		public int evolution_min;
		public int evolution_max;
	}
}