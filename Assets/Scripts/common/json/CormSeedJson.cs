using UnityEngine;
using System.Collections;

[System.Serializable]
public class CormSeedJson {
	public CormSeedData[] data;

	[System.Serializable]
	public class CormSeedData:BaseData{
		public int m_seed_id;
		public int m_seeding_id;
		public int probability;
	}
}

