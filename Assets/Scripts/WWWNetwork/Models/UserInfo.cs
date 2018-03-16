using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UserInfo {
	public int id;//用户ID;
	public string device_id;
	public string user_name; //名称: name;
	public int is_npc;//
	
	public int town_hall_level;//TID_BUILDING_PALACE主建筑物级别;
	public int laboratory_level;//TID_BUILDING_LABORATORY实验室级别;
	public int gunship_level;//TID_BUILDING_GUNSHIP炮舰等级;
	public int vault_level;//TID_BUILDING_VAULT地下仓库;

	public int common_piece;//'三角水晶',
	public int rare_piece;//'4角水晶',
	public int epic_piece;//'6角水晶',

	public int common_piece_ice;//'三角水晶',
	public int rare_piece_ice;//'4角水晶',
	public int epic_piece_ice;//'6角水晶',

	public int common_piece_fire;//'三角水晶',
	public int rare_piece_fire;//'4角水晶',
	public int epic_piece_fire;//'6角水晶',

	public int common_piece_dark;//'三角水晶',
	public int rare_piece_dark;//'4角水晶',
	public int epic_piece_dark;//'6角水晶',

	public int gold_count; //当前金币数;
	public int wood_count; //当前木材数;	
	public int stone_count; //当前石头数;
	public int iron_count; //当前钢材数;
	public int diamond_count; //当前宝石数;	
	
	public int max_gold_count; //金币最大容量;
	public int max_wood_count; //木材最大容量	;
	public int max_stone_count; //石头最大容量;
	public int max_iron_count; //钢材最大容量;
	
	public int reward_count; //当前奖杯数	reward_count
	public int exp_count; //当前绿星标数  exp_count
	public int exp_level; //当前绿星标级别  exp_level
	
	public int worker_count; //工人数	worker_count	
	//public int shield_expire; //盾牌到期时间 shield_expire	
	
	//public int max_troops_count; //兵营最大容量 max_troops_count	
	//public int max_spells_count; //法术最大容量 max_spells_count	

}
