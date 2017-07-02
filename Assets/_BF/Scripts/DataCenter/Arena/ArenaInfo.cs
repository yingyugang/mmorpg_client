using System;
using System.Collections.Generic;

namespace DataCenter
{
    public class ArenaTargetInfo
    {
        public UInt64 id;
        public int level;
        public string name;
        public bool isRobot;
        public int totalWin;
        public int totalLose;
        public int arenaPoint;
        public string arenaName;
        public HeroInfo leaderHero;

        public void init(ARENA_TARGET_TMP info)
        {
            id = info.idTarget;
            level = info.wUserLevel;
            name = info.szName;
            isRobot = (info.cbIsRobot==1);
            totalWin = (int)info.u32TotalWin;
            totalLose = (int)info.u32TotalLose;
            arenaPoint = (int)info.wArenaRank;
            arenaName = UserInfo.getArenaHonor(info.wArenaRank);
            HeroInfo hi = new HeroInfo();
            hi.InitDict((int)info.idLeaderHeroType);
            leaderHero = hi;

        }
    }

    public class ArenaTargetHeroList
    {
        public UInt64 useid;
        public List<ArenaTargetHero> heroList = new List<ArenaTargetHero>();
    }

    public class ArenaTargetHero
    {
        public HeroInfo hero;
        public int location;
        public bool isLeader;

        public void init(ARENA_HERO_INFO_TMP info)
        {
            hero = new HeroInfo();
            hero.InitDict((int)info.idHeroType);
            hero.level = info.wLevel;
            hero.btGrowup = (int)info.cbGrowup;
            hero.skillLevel = info.wSkillLevel;
            hero.equipId = (int)info.dwEquipTypeID;
            location = (int)info.cbPos;
            isLeader = (info.cbLeader == 1);
        }
    }

    public enum ARENA_RESULT
    {
        LOSE = 1,
        WIN = 2,
        DRAW
    }

    public class ArenaReward
    {
        public ARENA_RESULT result;
        public int addPoint;
        public int arenaPoint;
        public uint diamond;
        public ItemInfo recvItem;
        public string arenaName;

        public void init(MSG_CLIENT_ARENA_PVP_REWARD_EVENT info)
        {
            result = (ARENA_RESULT)info.cbResult;
            addPoint = info.n16AddPoint;
            arenaPoint = (int)info.u32ArenaPoint;
            diamond = info.u32Diamond;
            recvItem = new ItemInfo();
            recvItem.init((int)info.idItemType);
            arenaName = UserInfo.getArenaHonor((uint)arenaPoint);
        }
    }

    public class ArenaRankUser
    {
        public UInt64 id;
        public string name;
        public HeroInfo hero;
        public string arenaName;
        public int totalWin;
        public int totalLose;
        public int arenaPoint;
        public int arenaRank;

        public void init(ARENA_RANKING_TMP info)
        {
            id = info.idUser;
            name = info.szName;
            hero = new HeroInfo();
            hero.InitDict((int)info.idLeaderHeroType);
            hero.level = info.wHeroLevel;
            arenaName = UserInfo.getArenaHonor(info.u32ArenaPoint);
            totalLose = (int)info.u32TotalLose;
            totalWin = (int)info.u32TotalWin;
            arenaPoint = (int)info.u32ArenaPoint;
            arenaRank = info.wArenaRank;
        }
    }

    public class ArenaRankUserList
    {
        public int selfRank;
        public List<ArenaRankUser> theList = new List<ArenaRankUser>();
    }

    public class ArenaEndInfo
    {
        public uint uSelfTotalHP;
        public uint uTargetTotalHP;
        public uint uSelfTotalHPRemain;
        public uint uTargetTotalHPRemain;
        public uint uSelfHeroAlive;
        public uint uTargetHeroAlive;
        public uint uSelfTotalDamage;
        public uint uTargetTotalDamage;
    }
}
