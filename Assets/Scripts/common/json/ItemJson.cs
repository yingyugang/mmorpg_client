using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemJson {
	public ItemData[]  data;

	[System.Serializable]
	public class ItemData:BaseData{
		public string name;
		public int price;
		public string description;
		public string image_resource;
		public int num;
	}
}