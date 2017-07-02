	using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BaseLib;
using DataCenter;

namespace DataCenter
{
	public class DataTask: DataModule
	{

		private List<AreaInfo> areaInfoList = new List<AreaInfo>();
		private List<MapInfo> mapInfoList = new List<MapInfo>();
		private List<BattleInfo> battleInfoList = new List<BattleInfo>();
		private List<BattleInfo> chaosInfoList = new List<BattleInfo>();
		private List<FieldInfo> fieldInfoList = new List<FieldInfo>();
		private List<EnemyInfo> enemyInfoList = new List<EnemyInfo>();
		private List<FieldEnemy> fieldEnemyList = new List<FieldEnemy>();
		private Dictionary<int,AttackQueue> attackQueues = new Dictionary<int, AttackQueue>();

		public int currAreaID;
		public int currMapID; 
		public int currBattleID = 30002; 
		public int currFieldID = 50001; 
		public List<uint> firstFieldList = new List<uint>();

		public DataTask()
		{
			/*getAreaData();
			getMapData();
			getBattleData();
			getFieldData();
			getEnemyData();*/
			//EventSystem.register((int)EVENT_MAINUI.battleTask,onTaskInfo,(int)EVENT_GROUP.mainUI);
		}

		public override bool init()
		{

			return true;
		}

		public override void release()
		{

		}

		public List<AreaInfo> getAreaInfoList()
		{
			return areaInfoList;
		}

		public List<MapInfo> getMapInfoList()
		{
			return mapInfoList;
		}

		public List<BattleInfo> getBattleInfoList()
		{
			return battleInfoList;
		}

		public List<BattleInfo> getChaosInfoList()
		{
			return chaosInfoList;
		}

		public List<FieldEnemy> getEnemyInfoListByFieldID()	
		{
			//fieldEnemyList = getEnemyInfoList(fieldID);
			return fieldEnemyList;
		}	


		public void getTaskInfo()
		{
			Debug.Log("getTaskInfo........................................................");

			getAreaData();
			getMapData();
			getBattleData();
			getChaosData();
			getFieldData();
			getEnemyData();
		}



		public void getAttackQueueData()
		{
			attackQueues.Clear();
			ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.ATTACKPART);
			if(table!=null)
			{

			}
		}

		public void getAreaData()
		{
			areaInfoList.Clear();
			Debug.Log("getAreaData........................................................");
			ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_AREA);
			if (table != null)
			{
				ConfigRow[] rows = table.rows;
				for (int i = 0; i < rows.Length; i ++)
				{

					uint fieldid = (uint)rows[i].getIntValue(DICT_AREA.PRE_FIELD_ID);
					BattleTask taskState = DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).getTaskInfo(fieldid);
					if (fieldid == 0 || taskState != null)
					{
						AreaInfo areaInfo = new AreaInfo();
						areaInfo.AREA_ID = rows[i].getIntValue(DICT_AREA.AREA_ID);
						areaInfo.DESC = BaseLib.LanguageMgr.getString(rows[i].getIntValue(DICT_AREA.DESC_ID));
						areaInfo.NAME = BaseLib.LanguageMgr.getString(rows[i].getIntValue(DICT_AREA.NAME_ID));											
						areaInfo.PRE_FIELD_ID = rows[i].getIntValue(DICT_AREA.PRE_FIELD_ID);
						areaInfoList.Add(areaInfo);
					}
				}
			}




			/*for (int i = 0; i < 2; i++)
			{
				AreaInfo areaInfo = new AreaInfo();
				areaInfo.AREA_ID = 1000;
				areaInfo.DESC_ID = 100;
				areaInfo.NAME_ID = 200 + i;
				areaInfo.PRE_FIELD_ID = 0;
				areaInfoList.Add(areaInfo);
			}*/			
		}

		public void getMapData()
		{
			mapInfoList.Clear();

			ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_MAP);
			if (table != null)
			{
				ConfigRow[] rows = table.rows;
				for (int i = 0; i < rows.Length; i ++)
				{
					uint fieldid = (uint)rows[i].getIntValue(DICT_MAP.PRE_FIELD_ID);
					BattleTask taskState = DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).getTaskInfo(fieldid);
					if (fieldid == 0 || taskState != null)
					{
						MapInfo mapInfo = new MapInfo();
						mapInfo.AREA_ID = rows[i].getIntValue(DICT_MAP.AREA_ID);
						mapInfo.MAP_ID = rows[i].getIntValue(DICT_MAP.MAP_ID);
						mapInfo.PRE_FIELD_ID = rows[i].getIntValue(DICT_MAP.PRE_FIELD_ID);
						mapInfo.DESC = BaseLib.LanguageMgr.getString(rows[i].getIntValue(DICT_MAP.DESC_ID));
						mapInfo.NAME = BaseLib.LanguageMgr.getString(rows[i].getIntValue(DICT_MAP.NAME_ID));
						mapInfoList.Add(mapInfo);
					}
				}
			}					
			/*for (int i = 0; i < 3; i ++)
			{
				MapInfo mapInfo = new MapInfo();
				mapInfo.AREA_ID = 100;
				mapInfo.MAP_ID = 200;
				mapInfo.NAME_ID = 300;
				mapInfo.PRE_FIELD_ID = 1;
				mapInfo.DESC_ID = 22;
				mapInfoList.Add(mapInfo);
			}*/
		}

		public MapInfo getMapdataByAreaID(int areaID)
		{
			currAreaID = areaID;
			MapInfo mapInfo = new MapInfo();
			if (mapInfoList.Count > 0)
			{
				for (int i = 0; i < mapInfoList.Count; i++)
				{
					if (mapInfoList[i].AREA_ID == areaID)
					{
						mapInfo = mapInfoList[i];
						break;
					}
				}
			}
			return mapInfo;
		}

		public void getBattleData()
		{
			List<string> sceneList = new List<string>();
			battleInfoList.Clear();
			ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_BATTLE);
			if (table != null)
			{
				ConfigRow[] rows = table.rows;
				for (int i = 0; i < rows.Length; i ++)
				{
					uint fieldid = (uint)rows[i].getIntValue(DICT_BATTLE.PRE_FIELD_ID);
					int style = rows[i].getIntValue(DICT_BATTLE.STYLE);
					BattleTask taskState = DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).getTaskInfo(fieldid);
					//普通副本，无前置关卡，前置关卡已通关
					if ((style == 1) && (fieldid == 0 || taskState != null))
					{
						BattleInfo battleInfo = new BattleInfo();
						battleInfo.BATTLE_ID = rows[i].getIntValue(DICT_BATTLE.BATTLE_ID);
						battleInfo.DESC = BaseLib.LanguageMgr.getString(rows[i].getIntValue(DICT_BATTLE.DESC_ID));
						battleInfo.NAME = BaseLib.LanguageMgr.getString(rows[i].getIntValue(DICT_BATTLE.NAME_ID));
						battleInfo.STYLE = rows[i].getIntValue(DICT_BATTLE.STYLE);
						battleInfo.COND = rows[i].getIntValue(DICT_BATTLE.COND);
						battleInfo.COND_PARAM1 = rows[i].getIntValue(DICT_BATTLE.COND_PARAM1);
						battleInfo.COND_PARAM2 = rows[i].getIntValue(DICT_BATTLE.COND_PARAM2);
						battleInfo.PRE_FIELD_ID = rows[i].getIntValue(DICT_BATTLE.PRE_FIELD_ID);
						battleInfo.MAP_ID = rows[i].getIntValue(DICT_BATTLE.MAP_ID);
						battleInfo.POSX = rows[i].getFloatValue(DICT_BATTLE.POSX);
						battleInfo.POSY = rows[i].getFloatValue(DICT_BATTLE.POSY);
						battleInfo.REWARD_RULE = rows[i].getIntValue(DICT_BATTLE.REWARD_RULE);
						battleInfo.SCENE = rows[i].getStringValue(DICT_BATTLE.SCENE);
						battleInfo.SCENETYPE = (SceneType)rows[i].getIntValue(DICT_BATTLE.SCENETYPE);
						//battleInfo.PRE_FIELD_ID = rows[i].getIntValue(DICT_BATTLE.PRE_FIELD_ID);
						battleInfoList.Add(battleInfo);
							
						sceneList.Add(battleInfo.SCENE);

					}
				}

				AssetBundleMgr.SingleTon().CacheOrDownloadScenes(sceneList,null);
			}

		}


		public void getChaosData()
		{
			List<string> sceneList = new List<string>();
			chaosInfoList.Clear();
			ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_BATTLE);
			if (table != null)
			{
				ConfigRow[] rows = table.rows;
				for (int i = 0; i < rows.Length; i ++)
				{
					uint fieldid = (uint)rows[i].getIntValue(DICT_BATTLE.PRE_FIELD_ID);
					int style = rows[i].getIntValue(DICT_BATTLE.STYLE);
					BattleTask taskState = DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).getTaskInfo(fieldid);
					//普通副本，无前置关卡，前置关卡已通关
					if (style == 2 || style == 3)
					{
						BattleInfo battleInfo = new BattleInfo();
						battleInfo.BATTLE_ID = rows[i].getIntValue(DICT_BATTLE.BATTLE_ID);
						battleInfo.DESC = BaseLib.LanguageMgr.getString(rows[i].getIntValue(DICT_BATTLE.DESC_ID));
						battleInfo.NAME = BaseLib.LanguageMgr.getString(rows[i].getIntValue(DICT_BATTLE.NAME_ID));
						battleInfo.STYLE = rows[i].getIntValue(DICT_BATTLE.STYLE);
						battleInfo.COND = rows[i].getIntValue(DICT_BATTLE.COND);
						battleInfo.COND_PARAM1 = rows[i].getIntValue(DICT_BATTLE.COND_PARAM1);
						battleInfo.COND_PARAM2 = rows[i].getIntValue(DICT_BATTLE.COND_PARAM2);
						battleInfo.PRE_FIELD_ID = rows[i].getIntValue(DICT_BATTLE.PRE_FIELD_ID);
						battleInfo.MAP_ID = rows[i].getIntValue(DICT_BATTLE.MAP_ID);
						battleInfo.POSX = rows[i].getFloatValue(DICT_BATTLE.POSX);
						battleInfo.POSY = rows[i].getFloatValue(DICT_BATTLE.POSY);
						battleInfo.REWARD_RULE = rows[i].getIntValue(DICT_BATTLE.REWARD_RULE);
						battleInfo.SCENE = rows[i].getStringValue(DICT_BATTLE.SCENE);
						battleInfo.SCENETYPE = (SceneType)rows[i].getIntValue(DICT_BATTLE.SCENETYPE);
						//battleInfo.PRE_FIELD_ID = rows[i].getIntValue(DICT_BATTLE.PRE_FIELD_ID);
						chaosInfoList.Add(battleInfo);
						
						sceneList.Add(battleInfo.SCENE);
						
					}
				}
				
				AssetBundleMgr.SingleTon().CacheOrDownloadScenes(sceneList,null);
			}
			
		}


		public List<BattleInfo> getBattledataByMapID(int mapID)
		{
			currMapID = mapID;
			List<BattleInfo> battleList = new List<BattleInfo>();
			if (battleInfoList.Count > 0)
			{
				for (int i = 0; i < battleInfoList.Count; i++)
				{
					if (battleInfoList[i].MAP_ID == mapID)
					{
						battleList.Add(battleInfoList[i]);
						//battleInfo = battleInfoList[i];
						//break;
					}
				}
			}
			return battleList;
		}

		public BattleInfo getBattledatabyBattleID(int battleID)
		{
			BattleInfo battleInfo = new BattleInfo();
			battleInfo.SCENE = "Desert1";
			battleInfo.SCENETYPE = SceneType.scroll;
			if (battleInfoList.Count > 0)
			{
				for (int i = 0; i < battleInfoList.Count; i++)
				{
					if (battleInfoList[i].BATTLE_ID == battleID)
					{
						battleInfo = battleInfoList[i];
						break;
					}
				}
			}
			return battleInfo;
		}


		public void getFieldData()
		{
			fieldInfoList.Clear();
			firstFieldList.Clear();
			ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_FIELD);
			if (table != null)
			{
				ConfigRow[] rows = table.rows;
				for (int i = 0; i < rows.Length; i++)
				{
					uint currfieldid = (uint)rows[i].getIntValue(DICT_FIELD.FIELD_ID);
					uint prefieldid = (uint)rows[i].getIntValue(DICT_FIELD.PRE_FIELD_ID);
					BattleTask currfieldidState = DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).getTaskInfo(currfieldid);
					BattleTask prefieldidState = DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).getTaskInfo(prefieldid);
					if (currfieldidState != null || prefieldidState != null || prefieldid == 0)
					{
						FieldInfo fieldInfo = new FieldInfo();
						fieldInfo.FIELD_ID = rows[i].getIntValue(DICT_FIELD.FIELD_ID);
						fieldInfo.BATTLE_ID = rows[i].getIntValue(DICT_FIELD.BATTLE_ID);
						fieldInfo.NAME = BaseLib.LanguageMgr.getString(rows[i].getIntValue(DICT_FIELD.NAME_ID));
						fieldInfo.DESC = BaseLib.LanguageMgr.getString(rows[i].getIntValue(DICT_FIELD.DESC_ID));
						fieldInfo.PRE_FIELD_ID = rows[i].getIntValue(DICT_FIELD.PRE_FIELD_ID);
						fieldInfo.NEXT_FIELD_ID = rows[i].getIntValue(DICT_FIELD.NEXT_FIELD_ID);
						fieldInfo.EXP = rows[i].getIntValue(DICT_FIELD.EXP);	
						fieldInfo.COST_ENARGY = rows[i].getIntValue(DICT_FIELD.COST_ENARGY);
						fieldInfo.MAX_STEP = rows[i].getIntValue(DICT_FIELD.MAX_STEP);
						fieldInfo.ENEMY1_ID = rows[i].getIntValue(DICT_FIELD.ENEMY1_ID);
						fieldInfo.ENEMY1_RATE = rows[i].getIntValue(DICT_FIELD.ENEMY1_RATE);
						fieldInfo.ENEMY2_ID = rows[i].getIntValue(DICT_FIELD.ENEMY2_ID);
						fieldInfo.ENEMY2_RATE = rows[i].getIntValue(DICT_FIELD.ENEMY2_RATE);
						fieldInfo.ENEMY3_ID = rows[i].getIntValue(DICT_FIELD.ENEMY3_ID);
						fieldInfo.ENEMY2_RATE = rows[i].getIntValue(DICT_FIELD.ENEMY2_RATE);
						fieldInfo.ENEMY4_ID = rows[i].getIntValue(DICT_FIELD.ENEMY4_ID);
						fieldInfo.ENEMY4_RATE = rows[i].getIntValue(DICT_FIELD.ENEMY4_RATE);
						fieldInfo.ENEMY5_ID = rows[i].getIntValue(DICT_FIELD.ENEMY5_ID);
						fieldInfo.ENEMY5_RATE = rows[i].getIntValue(DICT_FIELD.ENEMY5_RATE);
						fieldInfo.ENEMY6_ID = rows[i].getIntValue(DICT_FIELD.ENEMY6_ID);
						fieldInfo.ENEMY6_RATE = rows[i].getIntValue(DICT_FIELD.ENEMY6_RATE);
						fieldInfo.MESS_STEP_SUM = rows[i].getIntValue(DICT_FIELD.MESS_STEP_SUM);
						fieldInfo.FINAL_BOSS_ENEMY_ID = rows[i].getIntValue(DICT_FIELD.FINAL_BOSS_ENEMY_ID);
						fieldInfo.CHESTS_ID = rows[i].getIntValue(DICT_FIELD.CHESTS_ID);
						fieldInfo.CHESTS_RATE = rows[i].getIntValue(DICT_FIELD.CHESTS_RATE);
						fieldInfo.MONSTER_DROP_COIN_MAX = rows[i].getIntValue(DICT_FIELD.MONSTER_DROP_COIN_MAX);
						fieldInfo.MONSTER_DROP_SOUL_MAX = rows[i].getIntValue(DICT_FIELD.MONSTER_DROP_SOUL_MAX);

						if (prefieldid == 0)
						{
							firstFieldList.Add(currfieldid);
						}

						if (currfieldidState != null)
							fieldInfo.STATE = currfieldidState.state;
						else
						if (prefieldidState != null || prefieldid == 0)
							fieldInfo.STATE = TASK_STATE.NEW;

						fieldInfoList.Add(fieldInfo);

						for (int j = 0; j < firstFieldList.Count; j ++)
						{
							if (firstFieldList[j] == prefieldid)
							{
								for (int k = 0; k < fieldInfoList.Count; k ++)
								{
									if (fieldInfoList[k].FIELD_ID == prefieldid && fieldInfo.STATE == TASK_STATE.NEW)
									{
										FieldInfo fieldinfo = fieldInfoList[k];
										fieldinfo.STATE = TASK_STATE.PASS;
										fieldInfoList[k] = fieldinfo;
										break;
									}
								}
								break;
							}
						}

					}
				}
			}
		}

		public List<FieldInfo> getFieldDataByBattleID(int battleID)
		{
			currBattleID = battleID;
			List<FieldInfo> fieldList = new List<FieldInfo>();
			if (fieldInfoList.Count > 0)
			{
				for (int i = 0; i < fieldInfoList.Count; i++)
				{
					if (fieldInfoList[i].BATTLE_ID == battleID)
					{
						fieldList.Add(fieldInfoList[i]);
					}
				}
			}
			return fieldList;
		}

		public FieldInfo getFieldDataByFieldID(int fieldID)
		{
			FieldInfo fieldInfo = new FieldInfo();
			if (fieldInfoList.Count > 0)
			{
				for (int i = 0; i < fieldInfoList.Count; i++)
				{
					if (fieldInfoList[i].FIELD_ID == fieldID)
					{
						fieldInfo = fieldInfoList[i];
						break;
					}
				}
			}
			return fieldInfo;
		}

		public void getEnemyData()
		{
			enemyInfoList.Clear();

			ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ENEMY);
			if (table != null)
			{
				ConfigRow[] rows = table.rows;
				for (int i = 0; i < rows.Length; i++)
				{
					EnemyInfo enemyInfo = new EnemyInfo();
					enemyInfo.ENEMY_ID = rows[i].getIntValue(DICT_ENEMY.ENEMY_ID);
					enemyInfo.MONSTER_TYPEID = rows[i].getIntValue(DICT_ENEMY.MONSTER_TYPEID);
					enemyInfo.MONSTER_LOCATION = rows[i].getIntValue(DICT_ENEMY.MONSTER_LOCATION);
					enemyInfoList.Add(enemyInfo);
				}
			}
		}


		public void getEnemyInfoList(int fieldID)		
		{
			currFieldID = fieldID;
			FieldInfo fieldInfo = new FieldInfo();

			fieldEnemyList.Clear();
			//List<FieldEnemy> fieldEnemyList = new List<FieldEnemy>();
			//List<List<EnemyInfoList>> enemyInfoListOfList = new List<List<EnemyInfoList>>();



			if (fieldInfoList.Count > 0)
			{
				for (int i = 0; i < fieldInfoList.Count; i++)
				{
					if (fieldInfoList[i].FIELD_ID == fieldID)
					{
						fieldInfo = fieldInfoList[i];
						break;
					}
				}
			}


			for (int j = 1; j <= fieldInfo.MAX_STEP; j ++)
			{
				int enemyID = getEnemyID(j,fieldInfo);
				FieldEnemy fieldEnemy = new FieldEnemy();
				fieldEnemy.FIELD_ID = fieldID;
				fieldEnemy.STEP = j;
				List<EnemyInfo> fieldEnemyInfoList = new List<EnemyInfo>();
				fieldEnemy.EnemyInfo = fieldEnemyInfoList; 
				for (int k = 0; k < enemyInfoList.Count; k ++)
				{
					if (enemyInfoList[k].ENEMY_ID == enemyID)
					{
						//FieldEnemyInfo fieldEnemyInfo = new FieldEnemyInfo();
						//fieldEnemyInfo.FIELD_ID = fieldID;
						//fieldEnemyInfo.STEP = j;

						EnemyInfo enemyInfo = new EnemyInfo();
						enemyInfo.MONSTER_TYPEID = enemyInfoList[k].MONSTER_TYPEID;
						enemyInfo.MONSTER_LOCATION = enemyInfoList[k].MONSTER_LOCATION;
						fieldEnemyInfoList.Add(enemyInfo);					
					}

				}

				fieldEnemyList.Add(fieldEnemy);

			}
			//return fieldEnemyList;
		}

		public int getEnemyID(int step,FieldInfo fieldInfo)
		{
			int enemyID = 0;
			if (step == 1)
			{
				enemyID = fieldInfo.ENEMY1_ID;
			}
			else
			if (step == 2)
			{
				enemyID = fieldInfo.ENEMY2_ID;
			}
			else
			if (step == 3)
			{
				enemyID = fieldInfo.ENEMY3_ID;
			}
			else
			if (step == 4)
			{
				enemyID = fieldInfo.ENEMY4_ID;
			}
			else
			if (step == 5)
			{
				enemyID = fieldInfo.ENEMY5_ID;
			}
			else
			{
				enemyID = fieldInfo.ENEMY5_ID;
			}
			return enemyID;
		}

		public HeroInfo GetMonsterInfo(int typeID){
			HeroInfo heroInfo = new HeroInfo ();
			ConfigTable monsterTable = ConfigMgr.getConfig (CONFIG_MODULE.DICT_MONSTER);
			ConfigTable heroTable = ConfigMgr.getConfig (CONFIG_MODULE.DICT_HERO);
			if (monsterTable.rows.Length > 0) {
				ConfigRow row = monsterTable.getRow(DICT_MONSTER.MONSTER_TYPEID,typeID);
				if(row != null)
				{
					//					Debug.Log(row.getIntValue(DICT_MONSTER.MONSTER_TYPEID));
					heroInfo.initAtk = row.getIntValue(DICT_MONSTER.ATK);
					heroInfo.initDef = row.getIntValue(DICT_MONSTER.DEF);
					heroInfo.initRecover = row.getIntValue(DICT_MONSTER.RECOVER);
					heroInfo.initHP = row.getIntValue(DICT_MONSTER.HP);
					heroInfo.initViolence = row.getIntValue(DICT_MONSTER.VIOLENCE);
					heroInfo.coinPerDrop = row.getIntValue(DICT_MONSTER.SPARK_COIN);
					heroInfo.soulPerDrop = row.getIntValue(DICT_MONSTER.SPARK_SOUL);
					heroInfo.isBoss = row.getIntValue(DICT_MONSTER.IS_BOSS);
					heroInfo.id = row.getIntValue(DICT_MONSTER.CATCH_ID);
					Debug.Log("heroInfo.id:" + heroInfo.id );
					int catchId = row.getIntValue(DICT_MONSTER.CATCH_ID);
					ConfigRow heroRow = heroTable.getRow(DICT_HERO.HERO_TYPEID,catchId);
					heroInfo.name = BaseLib.LanguageMgr.getString(heroRow.getIntValue(DICT_HERO.NAME_ID));
					heroInfo.fbxFile = heroRow.getStringValue(DICT_HERO.FBX_FILE);
					heroInfo.series = row.getIntValue(DICT_MONSTER.CLASS);//TODO the CLASS is nessasery? will it conflect with model view?
					heroInfo.moveSpeed = heroRow.getIntValue(DICT_HERO.MOVE_SPEED);
					heroInfo.movable = heroRow.getIntValue(DICT_HERO.MOVABLE);
				}
			}
			return heroInfo;
		}

		[System.Obsolete("Use GetMonsterInfo to instaed")]
		public HeroInfo getMonsterInfo(int typeID)
		{
			//HeroAttr heroAttr = new HeroAttr();
			HeroInfo heroInfo = new HeroInfo();
			ConfigTable monstertable = ConfigMgr.getConfig(CONFIG_MODULE.DICT_MONSTER);
			ConfigTable herotable = ConfigMgr.getConfig(CONFIG_MODULE.DICT_HERO);

			if (monstertable.rows.Length  > 0)
			{
				ConfigRow row = monstertable.getRow(DICT_MONSTER.MONSTER_TYPEID,typeID);			

				if (row != null)
				{
					/*heroAttr.BaseATK = row.getIntValue(DICT_MONSTER.ATK);
					heroAttr.BaseDEF = row.getIntValue(DICT_MONSTER.DEF);
					heroAttr.BaseRevert = row.getIntValue(DICT_MONSTER.RECOVER);
					heroAttr.HP = row.getIntValue(DICT_MONSTER.HP);
					heroAttr.Energy = 5;

					int catchid = row.getIntValue(DICT_MONSTER.CATCH_ID);
					ConfigRow herorow = herotable.getRow(DICT_HERO.HERO_TYPEID,catchid);

					heroAttr.Name = BaseLib.LanguageMgr.getString(herorow.getIntValue(DICT_HERO.NAME_ID));
					//heroAttr.BodyPrefabName = "Knight3";
					heroAttr.BodyPrefabName = herorow.getStringValue(DICT_HERO.FBX_FILE);
					//heroAttr.HeroType = _ElementType.Fire;

					heroAttr.HeroType = (_ElementType)row.getIntValue(DICT_MONSTER.CLASS);
					//heroAttr.Location = 0;			
					heroAttr.Speed = herorow.getIntValue(DICT_HERO.MOVE_SPEED);
					heroAttr.ATKSound = "bf218_se_saint1";
					heroAttr.ATKQueue = new float[]{0.63f,0.1f,0.1f,0.1f,0.1f,0.1f,0.1f,0.1f,0.1f};*/



					int catchid = row.getIntValue(DICT_MONSTER.CATCH_ID);
					ConfigRow herorow = herotable.getRow(DICT_HERO.HERO_TYPEID,catchid);

					heroInfo.id = catchid;
					heroInfo.baseAtk = row.getIntValue(DICT_MONSTER.ATK);
					heroInfo.baseDef = row.getIntValue(DICT_MONSTER.DEF);
					heroInfo.baseRecover = row.getIntValue(DICT_MONSTER.RECOVER); 
					heroInfo.initHP = row.getIntValue(DICT_MONSTER.HP);
					heroInfo.name = BaseLib.LanguageMgr.getString(herorow.getIntValue(DICT_MONSTER.NAME));
					heroInfo.spriteName = row.getIntValue(DICT_HERO.ICON_SPRITE_NAME).ToString();
					heroInfo.fbxFile = herorow.getStringValue(DICT_HERO.FBX_FILE);
					heroInfo.series = row.getIntValue(DICT_MONSTER.CLASS);
					heroInfo.moveSpeed = herorow.getIntValue(DICT_HERO.MOVE_SPEED);
					//heroInfo.ATKSound  = "bf218_se_saint1";
					//heroInfo.ATKQueue = new float[]{0.63f,0.1f,0.1f,0.1f,0.1f,0.1f,0.1f,0.1f,0.1f};


				}

			}
			return heroInfo;
		}

		public void SendHeroGroupMsg()
		{
			MSG_HERO_FORMATION_REQUEST msg = new MSG_HERO_FORMATION_REQUEST();
			msg.idFightTroop = (uint)DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).nCurTeamId + 1;					
			NetworkMgr.sendData(msg);
		}


	}


}