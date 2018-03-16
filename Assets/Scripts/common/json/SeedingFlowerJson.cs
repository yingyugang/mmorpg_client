using UnityEngine;
using System.Collections;

[System.Serializable]
public class SeedingFlowerJson {
	public SeedingFlowerData[]  data;

	[System.Serializable]
	public class SeedingFlowerData:BaseData{
		public int m_seeding_id;
		public int m_flower_id;
		public int probability;
	}
}