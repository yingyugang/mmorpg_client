using System;
using System.Collections.Generic;

namespace WWWNetwork
{
	[System.Serializable]
	public class Model
	{
		//public Notice update_done_list;//升级完成的通知（在home场景的时候）
		public BuildingModel create_building;
		public UserInfo user_info;
		public UserInfo enemy_info;
		public List<BuildingModel> building_list;
		public List<TroopModel> troop_list;
		public List<RegionModel> region_list;
		public List<TechnologyModel> technology_list;
		public List<HeroModel> hero_list;
	}


}

