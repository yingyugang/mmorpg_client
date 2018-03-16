using UnityEngine;
using System.Collections;

[System.Serializable]
public class VoiceJson {
	public VoiceData[]  data;

	[System.Serializable]
	public class VoiceData:BaseData{
		public int m_flower_id;
		public int target_type;
		public string target_id;
		public string name;
		public string tips;
		public int rarity;
		public int is_se;
		public int is_bgm;
		public int is_memorial;
		public int action_type;
		public string description;
		public string assetbundle_name;
		public int get_at;
	}
}