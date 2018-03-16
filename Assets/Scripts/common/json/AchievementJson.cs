using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class AchievementJson  {

	public AchievementData[]  data;

	[System.Serializable]
	public class AchievementData:BaseData{
		public string name;
		public string description;
		public int need_resource_type;
		public int need_resource_num;
		public int reward_item_id;
		public int reward_item_num;
		public int start_at;
		public int end_at;
		public int target_type;
		public int target_id;
	}

}
