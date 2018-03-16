using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SeedingJson {
	public List<SeedingData>  data;

	[System.Serializable]
	public class SeedingData:BaseSeedSeedingFlowerData{

	}
}