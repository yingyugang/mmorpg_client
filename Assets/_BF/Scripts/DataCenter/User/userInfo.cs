using System;
using System.Collections.Generic;
using BaseLib;

namespace DataCenter
{
	public enum TAG_SORT
	{
		item =0, //0物品
		hero = 1,//1英雄
		friendPoints = 2,//2友情点
		coin = 3, //3金币
		soul = 4, //魂
		diamond = 5 //钻石
	}

    public enum USER_ATTR
    {
        USER_ATTR_INVALID = -1,
        //服务端和客户端属性同步索引
        USER_ATTR_LEVEL = 1,
        USER_ATTR_EXP = 2,
        USER_ATTR_POWER = 3,  // 当前体力值
        USER_ATTR_VIP = 4,
        USER_ATTR_COIN = 5,
        USER_ATTR_SOUL = 6,  // 魂数量 用于升级村庄建筑
        USER_ATTR_DIAMOND = 7,
        USER_ATTR_ARENA_DIAMOND = 8,  // 竞技场宝珠
        USER_ATTR_ARENA_POINT = 9,  // 竞技场点数
        USER_ATTR_GAY_POINT = 10, // 友情召唤伙伴点数
        USER_ATTR_KEY_GOLD = 11, // 金钱副本开启钥匙数量
        USER_ATTR_KEY_SILVER = 12, // 经验副本开启钥匙数量
        USER_ATTR_FIGHT_TROOP = 13, // 当前队伍编号
        USER_ATTR_MAX_HERO = 14, // 最大携带宠物数量
        USER_ATTR_MAX_ITEM = 15, // 最大携带物品数量
        USER_ATTR_HERO_LIST = 16, // 英雄图鉴
        USER_ATTR_ITEM_LIST = 17, // 道具图鉴
        USER_ATTR_POWER_RECOVER_TIME = 18, //上次体力恢复时间     
        USER_ATTR_ARENA_RECOVER_TIME = 19,  // 上次竞技点恢复时间
        USER_ATTR_LOGINED_TIME = 20,  // 最近一次登录时间
        USER_ATTR_ACTION_TIME = 21,  // 最近一次行动时间
        USER_ATTR_GIFT_TYPEID1 = 22,
        USER_ATTR_GIFT_TYPEID2 = 23,
        USER_ATTR_GIFT_TYPEID3 = 24,
        USER_ATTR_USE_ITEM_ID1               = 25, // 战斗使用物品类型ID1
        USER_ATTR_USE_ITEM_AMOUNT1           = 26, // 战斗使用物品数量1
        USER_ATTR_USE_ITEM_ID2               = 27, // 战斗使用物品类型ID2
        USER_ATTR_USE_ITEM_AMOUNT2           = 28, // 战斗使用物品数量2
        USER_ATTR_USE_ITEM_ID3               = 29, // 战斗使用物品类型ID3
        USER_ATTR_USE_ITEM_AMOUNT3           = 30, // 战斗使用物品数量3
        USER_ATTR_USE_ITEM_ID4               = 31, // 战斗使用物品类型ID4
        USER_ATTR_USE_ITEM_AMOUNT4           = 32, // 战斗使用物品数量4
        USER_ATTR_USE_ITEM_ID5               = 33, // 战斗使用物品类型ID5
        USER_ATTR_USE_ITEM_AMOUNT5           = 34, // 战斗使用物品数量5
        USER_ATTR_LAST_GET_KEY_TIME          = 35, // 上次获取钥匙的时间
        USER_ATTR_LAST_GET_PRESENT_TIME = 36, // 上次获取每日登录奖励时间
        USER_ATTR_PRESENT_SEQ = 37, // 上次领取每日登录奖励顺位
        USER_ATTR_ARENA_TROOP = 38, // 竞技场队伍出战阵型
    };

    public class UserLevelUpdate
    {
        public uint newLevel; //最新等级
        public uint addPower;//增加体力
        public uint addLeader;//增加领导力
        public uint addFriend;//新增好友数
    }

	public class UserInfo
    {
        public UInt64 id { get; set; }
        //名称
        public string name { get; set; }

        //等级
        public UInt32 level { get; set; }

        //经验
        public UInt32 curexp { get; set; }
        public UInt32 maxexp { get; set; }

        //体力
        public UInt32 curpower { get; set; }
        public UInt32 maxpower { get; set; }

        public UInt32[] heroLst = new UInt32[32];  // 伙伴图鉴 十六进制字符串
        public UInt32[] itemLst = new UInt32[16];   // 物品图鉴 十六进制字符串

        //体力更新剩余时间
        public UInt32 powerTime { get; set; }

        //最大好友数
        public UInt32 maxFriend { get; set; }

        //最大领导力
        public UInt32 maxLeader { get; set; }

        //最大英雄树
        public UInt32 maxHero { get; set; }

        //最大格子数
        public UInt32 maxItem { get; set; }

        // 当前队伍编号FIGHT_TROOP
        public UInt32 fightTroop { get; set; }

        // 上次体力恢复时间
        public UInt64 PowerRecoverTime { get; set; }

        // 上次竞技点恢复时间
        public UInt64 ArenaRecoverTime { get; set; }

        // 最近一次登录时间
        public UInt64 LoginedTime { get; set; }

        //最近一次行动时间
        public UInt64 ActionTime { get; set; }

        public UInt32 idGiftType1 { get; set; }
        public UInt32 idGiftType2 { get; set; }
        public UInt32 idGiftType3 { get; set; }
        // 上次领取钥匙时间
        public UInt64 LastGetKeyTime { get; set; }
        //钻石
        public UInt32 diamond { get; set; }

        //金币
        public UInt64 goldCoin { get; set; }
        //魂
        public UInt64 soul { get; set; }

        //竞技点
        public UInt32 arenaPoint { get; set; }

        //竞技经验 
        public UInt32 arenaExp { get; set; }
        public string arenaHonor { get; set; }

        //友情点
        public UInt32 friendPt { get; set; }

        //金钱副本钥匙数
        public UInt32 keyCoin { get; set; }

        //经验副本钥匙数
        public UInt32 keyExp { get; set; }

        // 上次领取登录奖励的时间
        public UInt64 lastGetPresentTime{ get; set; }
        // 上次领取登录奖励的顺位
        public int presentSeq { get; set; }

        //竞技场出战阵型id
        public int idArenaTroop { get; set; }

        public UserInfo()
        {
            initTest();
        }

        //初始化
        public void init(MSG_CLIENT_USER_INFO_EVENT info)
        {
            name = info.szName;
            setLevel((uint)info.wLevel);
            curexp = info.dwExp;
            curpower = info.wPower;
            goldCoin = info.dwCoin;
            soul = info.dwSoul;
            diamond = info.dwDiamond;
            arenaPoint = info.btArenaDiamond;
            arenaExp = info.dwArenaPt;
            friendPt = info.dwGayPt;
            keyCoin = info.wKeyGold;
            keyExp = info.wKeySilver;
            maxHero = info.wMaxPet;
            maxItem = info.wMaxItem;
            fightTroop = info.btFightTroop;
            PowerRecoverTime = info.unPowerRecoverTime;
            ArenaRecoverTime = info.unArenaRecoverTime;
            LoginedTime = info.unLoginedTime;
            ActionTime = info.unActionTime;
            idGiftType1 = info.idGiftType1;
            idGiftType2 = info.idGiftType2;
            idGiftType3 = info.idGiftType3;
            LastGetKeyTime = info.unLastGetKeyTime;
            lastGetPresentTime = info.i64LastGetPresentTime;
            presentSeq = info.btPresentSeq;
            idArenaTroop = info.idArenaTroop;


            for (int index = 0; index < 32; index++)
                this.heroLst[index] = info.heroLst[index];
            for (int index = 0; index < 16; index++)
                this.itemLst[index] = info.itemLst[index];
        }

        public void setLevel(uint newLevel)
        {
            ConfigTable userLevel = ConfigMgr.getConfig(CONFIG_MODULE.DICT_USER_LEVEL);
            if (userLevel != null)
            {
                //获得当前用户等级信息
                ConfigRow row = userLevel.getRow(DICT_USER_LEVEL.LEVEL, (int)newLevel);
                if (row != null)
                {
                    maxexp = (uint)row.getIntValue(DICT_USER_LEVEL.EXP);
                    maxpower = (uint)row.getIntValue(DICT_USER_LEVEL.MAX_POWER);
                    maxFriend = (uint)row.getIntValue(DICT_USER_LEVEL.MAX_FRIEND);
                    maxLeader = (uint)row.getIntValue(DICT_USER_LEVEL.MAX_LEADER);
                }
                                
                if (level > 0 && newLevel > level)
                {
                    //获得前一级用户等级信息
                    ConfigRow prerow = userLevel.getRow(DICT_USER_LEVEL.LEVEL, (int)(newLevel-1));
                    if (prerow != null)
                    {
                        uint preMaxpower = (uint)prerow.getIntValue(DICT_USER_LEVEL.MAX_POWER);
                        uint preMaxFriend = (uint)prerow.getIntValue(DICT_USER_LEVEL.MAX_FRIEND);
                        uint preMaxLeader = (uint)prerow.getIntValue(DICT_USER_LEVEL.MAX_LEADER);

                        UserLevelUpdate update = new UserLevelUpdate();
                        update.addFriend = this.maxFriend - preMaxFriend;
                        update.addLeader = this.maxLeader - preMaxLeader;
                        update.addPower = this.maxpower - preMaxpower;
                        EventSystem.sendEvent((int)EVENT_MAINUI.userLevelUpdate, update, (int)EVENT_GROUP.mainUI);
                    }     
                }           
            }
            level = newLevel;
        }

        public void initTest()
        {
            id = 100;
            name = "死神";
            curexp = 15932;
            maxexp = 50000;
            curpower = 45;
            maxpower = 60;
            powerTime = 600;
            diamond = 250;
            goldCoin = 1234551;
            soul = 104234;
            arenaPoint = 2;

            setArenaExp(1240);
            setLevel(59);
        }

        public static string getArenaHonor(UInt32 value)
        {
            ConfigTable arenarank = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ARENA_RANK);
            if (arenarank == null)
                return "";
            ConfigRow cur = null;
            foreach (ConfigRow row in arenarank.rows)//递增
            {
                if (row.getIntValue(DICT_ARENA_RANK.ARENA_POINT) < value)
                {
                    cur = row;
                    continue;
                }
                else
                    break;
            }
            if (cur != null)
                return LanguageMgr.getString(cur.getIntValue(DICT_ARENA_RANK.NAME_ID));
            return "";
        }


        public void setArenaExp(UInt32 value)
        {
            this.arenaExp = value;
            this.arenaHonor = getArenaHonor(value);
        }

        public bool heroIsOpen(uint id)
        {
            return this.isOpen(this.heroLst, id);
        }

        public bool itemIsOpen(uint id)
        {
            return this.isOpen(this.itemLst,id);
        }

        public void setOpenHeroLib(uint id)
        {
            this.setOpen(this.heroLst, id);
        }

        public void setOpenItemLib(uint id)
        {
            this.setOpen(this.itemLst, id);
        }

        void setOpen(UInt32[] data,uint id)
        {
            if (data == null)
                return;
            if (id > data.Length * 32 || id < 1)
                return;

            uint offset = (id - 1) % 32;
            uint index = (id - 1) / 32;
            uint res = (uint)(1 << (int)offset);
            data[index] |= res;
        }

        bool isOpen(UInt32[] data, uint id)
        {
            if (data == null)
                return false;
            if (id > data.Length * 32 || id < 1)
                return false;
            uint offset = (id - 1) % 32;
            uint index = (id - 1) / 32;
			uint res = (uint)(1 << (int)offset);
			if((data[index] & res)!= res)
				return false;
			return true;
        }
	}
}
