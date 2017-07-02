using System;
using System.Collections.Generic;

namespace DataCenter
{
    // 礼品获得渠道
    public enum PRESENT_CHANNEL
    {
        PRESENT_CHANNEL_DAILY_LOGIN = 0,    // 每日登录奖励
        PRESENT_CHANNEL_WEEK_CARD = 1,    // 周卡
        PRESENT_CHANNEL_MONTH_CARD = 2,    // 月卡
    }

    // 礼品类型
    public enum PRESENT_TYPE
    {
        PRESENT_TYPE_BEGIN = -1,
        PRESENT_TYPE_ITEM,          // 物品
        PRESENT_TYPE_HERO,          // 英雄
        PRESENT_TYPE_GAY_PT,        // 友情点
        PRESENT_TYPE_COIN,          // 金币
        PRESENT_TYPE_SOUL,          // 魂
        PRESENT_TYPE_DIAMOND,       // 钻石
        PRESENT_TYPE_END,
    }

    public class PresentInfo
    {
        public UInt32 id;
        public PRESENT_CHANNEL channel;      // 礼品获取渠道
        public PRESENT_TYPE type;          // 礼品类型
        public UInt32 para1;
        public UInt32 para2;
        public UInt64 getTime;     // 礼品获取时间

        public string name;
        public string icon;
        public string desc;
        public string atlas;

        public void init(PRESENT_INFO info)
        {
            id = info.idPresent;
            channel = (PRESENT_CHANNEL)info.btChannel;
            type = (PRESENT_TYPE)info.btTag;
            para1 = info.unPara1;
            para2 = info.unPara2;
            getTime = info.i64GetTime;

            switch (type)
            {
                case PRESENT_TYPE.PRESENT_TYPE_ITEM:
                    ItemInfo item = new ItemInfo();
                    item.init((int)para1);
                    name = item.name + "X" + para2.ToString();;
                    icon = item.icon;
                    break;

                case PRESENT_TYPE.PRESENT_TYPE_HERO:
                    HeroInfo hero = new HeroInfo();
                    hero.InitDict((int)para1);
                    name = hero.name + "X" + para2.ToString();
                    icon = hero.portarait;
                    break;
                case PRESENT_TYPE.PRESENT_TYPE_GAY_PT:
                    name = "友情点" + "X" + para1.ToString();
                    icon = "";
                    break;
                case PRESENT_TYPE.PRESENT_TYPE_COIN:
                    name = "金币" + "X" + para1.ToString();
                    icon = "";
                    break;
                case PRESENT_TYPE.PRESENT_TYPE_SOUL:
                    name = "魂" + "X" + para1.ToString();
                    icon = "";
                    break;
                case PRESENT_TYPE.PRESENT_TYPE_DIAMOND:
                    name = "砖石" + "X" + para1.ToString();
                    icon = "";
                    break;
            }


            switch (channel)
            {
                case PRESENT_CHANNEL.PRESENT_CHANNEL_DAILY_LOGIN:
                    desc = "每日登录奖励";
                    break;
                case PRESENT_CHANNEL.PRESENT_CHANNEL_WEEK_CARD:
                    desc = "周卡";
                    break;
                case PRESENT_CHANNEL.PRESENT_CHANNEL_MONTH_CARD:
                    desc = "月卡";
                    break;
            }
        }
    }
}
