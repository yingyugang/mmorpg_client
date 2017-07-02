using System;
using System.Collections.Generic;
using BaseLib;

namespace DataCenter
{

    public struct chatInfo
    {
        public UInt64 userid;
        public string name;
        public UInt32 chatTime;
        public UInt16 level;
        public string ArenaRank;
        public HeroInfo hero;
        public string chatContent;

        public void init(WORLDCHAT_TMP info)
        {
            userid = info.userid;
            name = info.strName;
            chatTime = info.chatTime;
            level = info.userlevel;
            ArenaRank = UserInfo.getArenaHonor(info.arenaRank);
            chatContent = info.strContent;
            initHero(info.heroType,info.heroLevel);
        }

        public void init(MSG_CLIENT_CHAT_INFO info)
        {
            userid = info.userid;
            name = info.strName;
            level = info.level;
            ArenaRank = UserInfo.getArenaHonor(info.arenaRank);
            chatContent = info.strContent;
            initHero(info.heroType, info.heroLevel);
        }

        void initHero(UInt32 heroType, UInt32 heroLevel)
        {
            hero = new HeroInfo();
            hero.InitDict((int)heroType);
            hero.level = (int)heroLevel;
        }
    }
}
