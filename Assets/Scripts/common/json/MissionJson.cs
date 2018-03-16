using UnityEngine;
using System.Collections;

[System.Serializable]
public class MissionJson {
	public MissionData[]  data;

	[System.Serializable]
	public class MissionData:BaseData{
		public string name;
		public string image_resource;
		public int difficulty;
		public int reward_type;
		public int reward_id;
		public int num;
		public int mission_type;
		public int action_type;
		public int value;
	}
}