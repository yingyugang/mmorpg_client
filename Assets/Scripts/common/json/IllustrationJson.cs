using UnityEngine;
using System.Collections;

[System.Serializable]
public class IllustrationJson {
	public IllustrationData[]  data;

	[System.Serializable]
	public class IllustrationData:BaseData{
		public int target_type;
		public string target_id;
		public string name;
		public string tips;
		public int rarity;
		public int action_type;
		public int whiteout;
		public string voice_id;
		public string description;
		public string assetbundle_name;
		public int get_at;
	}
}