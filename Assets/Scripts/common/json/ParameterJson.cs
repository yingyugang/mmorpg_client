using UnityEngine;
using System.Collections;

[System.Serializable]
public class ParameterJson {
	public ParameterData[]  data;

	[System.Serializable]
	public class ParameterData:BaseData{
		public string key;
		public string value;
		public string description;
	}
}