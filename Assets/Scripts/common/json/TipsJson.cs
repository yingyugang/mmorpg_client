using UnityEngine;
using System.Collections;

[System.Serializable]
public class TipsJson  {

	public TipsData[]  data;

	[System.Serializable]
	public class TipsData:BaseData{
		public string message;
	}
}
