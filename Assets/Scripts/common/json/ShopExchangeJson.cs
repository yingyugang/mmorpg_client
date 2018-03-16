using UnityEngine;
using System.Collections;

[System.Serializable]
public class ShopExchangeJson {
	public ShopExchangeData[]  data;

	[System.Serializable]
	public class ShopExchangeData:BaseData{
		public int m_item_id;
		public int item_num;
		public int need_sol_num;
		public int is_kit;
		public int limit_count;
		public string kit_items;
	}
}