using UnityEngine;
using System.Collections;

public class RearTextJson {

	public RearTextData[]  data;

	[System.Serializable]
	public class RearTextData:BaseData{
		public int action_type;
		public int result_type;
		public string text;
		public int target_type;
		public int target_id;
	}
}
