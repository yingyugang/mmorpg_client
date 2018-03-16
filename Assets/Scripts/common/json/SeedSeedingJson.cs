using UnityEngine;
using System.Collections;

[System.Serializable]
public class SeedSeedingJson {
	public SeedSeedingData[]  data;

	[System.Serializable]
	public class SeedSeedingData:BaseData{
		public string name;
		public int price;
		public string description;
		public string image_resource;
	}
}

