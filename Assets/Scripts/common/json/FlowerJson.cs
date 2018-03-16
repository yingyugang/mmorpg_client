using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FlowerJson
{
	public List<FlowerData> data;

	[System.Serializable]
	public class FlowerData:BaseSeedSeedingFlowerData
	{

	}
}