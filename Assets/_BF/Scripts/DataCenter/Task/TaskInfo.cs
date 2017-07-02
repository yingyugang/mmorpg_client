using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace DataCenter
{

	public enum SceneType
	{
		Single = 1,
		scroll = 2
	}

	public struct AreaInfo
	{
		public int AREA_ID;
		public string NAME;
		public string DESC;
		public int PRE_FIELD_ID;
	}

	public struct MapInfo
	{
		public int MAP_ID;
		public int AREA_ID;
		public string NAME;
		public string DESC;
		public int PRE_FIELD_ID;
	}
		
	public struct BattleInfo
	{
		public int BATTLE_ID;
		public string NAME;
		public string DESC;
		public int STYLE;
		public int COND;
		public int COND_PARAM1;
		public int COND_PARAM2;
		public int PRE_FIELD_ID;
		public int MAP_ID;
		public float POSX; 
		public float POSY;
		public int REWARD_RULE;
		public string SCENE;
		public SceneType SCENETYPE;
	}

	public struct dict_field_smallboss
	{
		public int FIELD_ID;
		public int STEP;
		public int ENEMY_ID;
	}

	public struct EnemyInfo
	{
		public int ENEMY_ID;
		public int MONSTER_TYPEID;
		public int MONSTER_LOCATION;
	}

	public struct dict_enemy_mess
	{
		public int ENEMY_ID;
		public int RATE;
		public int USER_LEVEL;
	}

	public struct FieldInfo
	{
		public int BATTLE_ID;
		public int FIELD_ID;
		public string NAME;
		public string DESC;
		public int PRE_FIELD_ID;
		public int NEXT_FIELD_ID;
		public int EXP;
		public int COST_ENARGY;
		public int MAX_STEP;
		public int ENEMY1_ID;
		public int ENEMY1_RATE;
		public int ENEMY2_ID;
		public int ENEMY2_RATE;
		public int ENEMY3_ID;
		public int ENEMY3_RATE;
		public int ENEMY4_ID;
		public int ENEMY4_RATE;
		public int ENEMY5_ID;
		public int ENEMY5_RATE;
		public int ENEMY6_ID;
		public int ENEMY6_RATE;
		public int MESS_STEP_SUM;
		public int FINAL_BOSS_ENEMY_ID;
		public int CHESTS_ID;
		public int CHESTS_RATE;
		public int MONSTER_DROP_COIN_MAX;
		public int MONSTER_DROP_SOUL_MAX;
		public TASK_STATE STATE;
	}



	public struct FieldEnemy
	{
		public int FIELD_ID;
		public int STEP;
		public List<EnemyInfo> EnemyInfo;
	}

}
