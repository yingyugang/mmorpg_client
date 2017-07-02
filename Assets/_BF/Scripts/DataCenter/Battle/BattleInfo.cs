using UnityEngine;
using System;
using System.Collections.Generic;
using BaseLib;

namespace DataCenter
{
    public enum TASK_STATE
    {
        NEW=0, //新关卡
        PASS=1, //通过
        PERFECT=2, //完美通过
    }

    //普通关卡
    public class BattleTask
    {
        public uint field;
        public TASK_STATE state;

        public void init(uint field,byte status)
        {
            this.field = field;
			this.state = Tools.getEnumValue<TASK_STATE>((int)status, TASK_STATE.NEW);
        }
    }


    //活动关卡
    public class BattleActivityTask
    {
        public uint field;
        public TASK_STATE state;
        public uint dueTIme;//剩余时间

        public void init(uint field, byte status, uint dueTIme)
        {
            this.field = field;
            this.state = Tools.getEnumValue<TASK_STATE>((int)state, TASK_STATE.NEW);
            this.dueTIme = dueTIme;
        }
    }

    //怪物信息
    public class BattleMonSter
    {

		public int MONSTER_TYPEID;
		public int MONSTER_LOCATION;
        public int typeid;
        public HeroInfo heroInfo = new HeroInfo();
		public string fbxFile;
        public void initHero(int type)
        {
            ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_MONSTER);
            ConfigTable heroTable = ConfigMgr.getConfig(CONFIG_MODULE.DICT_HERO);
            if (table == null || heroTable ==null)
                return;
            ConfigRow row = table.getRow(DICT_MONSTER.MONSTER_TYPEID, type);
            if (row != null)
            {
				heroInfo.type = row.getIntValue(DICT_MONSTER.CATCH_ID);;
                heroInfo.name = BaseLib.LanguageMgr.getString(row.getIntValue(DICT_MONSTER.NAME));
                heroInfo.initAtk = row.getIntValue(DICT_MONSTER.ATK);
                heroInfo.initDef = row.getIntValue(DICT_MONSTER.DEF);
                heroInfo.initRecover = row.getIntValue(DICT_MONSTER.RECOVER);
                heroInfo.initHP = row.getIntValue(DICT_MONSTER.HP);
                heroInfo.initViolence = row.getIntValue(DICT_MONSTER.VIOLENCE);
                heroInfo.coinPerDrop = row.getIntValue(DICT_MONSTER.SPARK_COIN);
                heroInfo.soulPerDrop = row.getIntValue(DICT_MONSTER.SPARK_SOUL);
                heroInfo.isBoss = row.getIntValue(DICT_MONSTER.IS_BOSS);
                heroInfo.id = row.getIntValue(DICT_MONSTER.CATCH_ID);
                int catchId = row.getIntValue(DICT_MONSTER.CATCH_ID);
                ConfigRow heroRow = heroTable.getRow(DICT_HERO.HERO_TYPEID, catchId);
                heroInfo.fbxFile = heroRow.getStringValue(DICT_HERO.FBX_FILE);
                heroInfo.series = row.getIntValue(DICT_MONSTER.CLASS);
                heroInfo.moveSpeed = heroRow.getIntValue(DICT_HERO.MOVE_SPEED);
                heroInfo.movable = heroRow.getIntValue(DICT_HERO.MOVABLE);


				fbxFile = heroInfo.fbxFile;

            }
        }
    }

	[System.Serializable]
    public class BattleCatchHero
    {
        public HERO_GROWUP grown;
        public uint level;
        public uint typeid;
		public bool isCatch;
    }

    public class BattleReward
    {
        public bool win;
        public bool passAll;
        public int maxStar;
        public uint exp;
        public uint diamond;
    }

    public class BattleStep
    {
        //public Dictionary<int, BattleMonSter> _monsterList = new Dictionary<int, BattleMonSter>();

		public List<BattleMonSter> _monsterList = new List<BattleMonSter>();
		public List<BattleCatchHero> _catchHeroList = new List<BattleCatchHero>();
		public List<string> fbxStrings = new List<string>();

		public BattleStep()
		{
			fbxStrings.Clear();
		}
        public void addCatchInfo(MONSTER_CATCH_INFO info)
        {
            BattleCatchHero catchInfo = new BattleCatchHero();
            catchInfo.level = info.wLevel;
            catchInfo.typeid = info.idHeroType;
            catchInfo.grown = Tools.getEnumValue<HERO_GROWUP>((int)info.cbGrowup, HERO_GROWUP.HERO_GROWUP_INVALID);
			catchInfo.isCatch = false;
            _catchHeroList.Add(catchInfo);
        }

        public BattleCatchHero[] catchHeroList()
        {
            return _catchHeroList.ToArray();
        }


        public void initEnemy(uint enemy)
        {
            ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ENEMY);
            if (table == null)
                return;
            ConfigRow[] rows = table.getRows(DICT_ENEMY.ENEMY_ID,(int)enemy);
            foreach(ConfigRow row in rows)
            {
                int typeid = row.getIntValue(DICT_ENEMY.MONSTER_TYPEID);
                BattleMonSter monster = new BattleMonSter();
                monster.initHero(typeid);
				fbxStrings.Add(monster.fbxFile);
				//AssetBundleMgr.SingleTon().CacheOrDownloadHeros(fbxStrings, SetHeroPrefabs);
				monster.MONSTER_TYPEID = row.getIntValue(DICT_ENEMY.MONSTER_TYPEID);
				//monster.MONSTER_TYPEID = location;
				monster.MONSTER_LOCATION = row.getIntValue(DICT_ENEMY.MONSTER_LOCATION);
				_monsterList.Add(monster);
				//同一波怪物，会有多个重复的MONSTER_TYPEID值，Dictionary类型无法保存重复的值
                //_monsterList[location] = monster;
            }
        }

        public BattleMonSter getMonSter(int location)
        {
            /*if (_monsterList.ContainsKey(location))
                return _monsterList[location];*/
            return null;
        }
    }

    public class BattleDropItem
    {
        public uint typeid;
        public uint count;
    }

    //宝箱类型
    public enum CHEST_TYPE
    {
        CHEST_ERROR,
        CHEST_COIN =1, //
        CHEST_SOUL,
        CHEST_ITEM,
        CHEST_MOSTER
    }

	[System.Serializable]
    public class BattleDropChest
    {
        public CHEST_TYPE type;
        public uint id;
        public uint value;
        public BattleCatchHero dropMonster;

        public void init(DROP_CHESTS_INFO info)
        {
            if (info.u32Coin > 0)
            {
                type = CHEST_TYPE.CHEST_COIN;
                value = info.u32Coin;
            }
            else if (info.u32Soul > 0)
            {
                type = CHEST_TYPE.CHEST_SOUL;
                value = info.u32Soul;
            }
            else if (info.idItemType > 0)
            {
                type = CHEST_TYPE.CHEST_ITEM;
                value = info.idItemType;
            }
            else if (info.idMonsterType > 0)
            {
                type = CHEST_TYPE.CHEST_MOSTER;
                dropMonster = new BattleCatchHero();
                dropMonster.level = info.wLevel;
                dropMonster.typeid = info.idMonsterType;
                dropMonster.grown = Tools.getEnumValue<HERO_GROWUP>((int)info.cbGrowup, HERO_GROWUP.HERO_GROWUP_INVALID);
				dropMonster.isCatch = false;
            }
            else
                type = CHEST_TYPE.CHEST_ERROR;
        }
    }

    public class BattleRecvData
    {
        //各轮信息
        public Dictionary<int, BattleStep> _stepList = new Dictionary<int, BattleStep>();
        //掉落素材
        List<BattleDropItem> _dropItem = new List<BattleDropItem>();
        //宝箱怪列表
        List<BattleDropChest> _dropChest = new List<BattleDropChest>();
        //
        public BattleReward reward = new BattleReward();

		public List<string> fbxStrings = new List<string>();

        public uint field { get; set; }

		public BattleRecvData()
		{
			fbxStrings.Clear();
		}

        public void addStep(int index, uint enemyid)
        {
            BattleStep step = new BattleStep();
            step.initEnemy(enemyid);
			fbxStrings.AddRange(step.fbxStrings);
            _stepList[index] = step;
        }

        public BattleStep getStep(int index)
        {
            if (_stepList.ContainsKey(index))
                return _stepList[index];
            return null;
        }

        public void clear()
        {
            this._stepList.Clear();
        }

        public void setCatchItem(DROP_ITEM_INFO[] lst)
        {
            _dropItem.Clear();
            _dropItem.Capacity = lst.Length;
            foreach(DROP_ITEM_INFO it in lst)
            {
                BattleDropItem item = new BattleDropItem();
                item.count = it.u32Amount;
                item.typeid = it.idItemType;
                _dropItem.Add(item);
            }
        }

        public BattleDropItem[] getDropitemList()
        {
            return _dropItem.ToArray();
        }

        public BattleDropChest[] getDropChestList()
        {
            return _dropChest.ToArray();
        }

        public void setChests(DROP_CHESTS_INFO[] lst)
        {
            _dropChest.Clear();
            _dropChest.Capacity = lst.Length;
            foreach(DROP_CHESTS_INFO it in lst)
            {
                BattleDropChest chest = new BattleDropChest();
                chest.init(it);
                _dropChest.Add(chest);
            }
        }
    }

    public class BattleItemUsed
    {
        public uint itemType;
        public uint count;
    }

    public class BattleFirend
    {
        //用户属性
        public uint uid;//用户id
        public string uname;//用户名
        public uint ulevel;//用户等级
        public bool isFriend;//是否好友
        public uint friendPt;//友情点

        //该用户共享队长属性
        public HeroInfo friendHero = new HeroInfo();

        public BattleFirend()
        {

        }

        public void init(USER_HELP_INFO info)
        {
            uid = info.idUser;
            uname = info.szName;
            ulevel = info.wUserLevel;
            isFriend = (info.cbIsFriend==1);
            friendPt = (uint)info.cbFriendPoint;
			friendHero.type = (int)info.idHeroType;
            friendHero.InitDict((int)info.idHeroType);
            friendHero.btGrowup = info.cbGrowup;
            friendHero.level = info.wLevel;
            friendHero.equipId = (int)info.dwEquipID;
            friendHero.skillLevel = info.wSkillLevel;

        }
    }
}
