using UnityEngine;
using System.Collections;

[System.Serializable]
public class UserJson  {

	public UserData[] seed_list;
	public UserData[] seeding_list; 
	public UserData[] flower_list;
	public UserData[] voice_list;
	public UserData[] illust_list;
	public UserData[] animation_list;

	[System.Serializable]
	public class UserData:BaseData{
		public int num;
		public int get_at;
	}
}
