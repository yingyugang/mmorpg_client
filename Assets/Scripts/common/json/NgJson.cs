using UnityEngine;
using System.Collections;

[System.Serializable]
public class NgJson {
	public NgData[] data;

	[System.Serializable]
	public class NgData:BaseData{
		public string name;
	}
}