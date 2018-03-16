using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WWWNetwork
{
	[System.Serializable]
	public class BuildingModel 
	{
		public int id;
		public int user_id;
		public int type;
		public int level;
		public int status;
		public int start_time;
		public int end_time;
		public int pos_x;
		public int pos_y;
	}
}
