using UnityEngine;
using System.Collections;

[System.Serializable]
public class ShopOtherJson {
	public ShopOtherData[]  data;

	[System.Serializable]
	public class ShopOtherData:BaseData{
		public int target_type;
		public int lv;
		public int need_sol_num;
		public string base_title;
		public string base_description;
		public string popup_title;
		public string popup_description;
		public string popup_confirm;
	}
}