using System;
using System.Collections.Generic;
using BaseLib;

namespace DataCenter
{
    public enum FRIEND_STATUS
    {
        FRIEND_STATUS_APPLY = 0,//申请中
        FRIEND_STATUS_BE_APPLIED = 1,//被申请
        FRIEND_STATUS_FRIEND = 2,//好友
        FRIEND_STATUS_DEL = 3,//删除
    }

    public class FriendInfo
    { 
        public UInt64 userid;   //
        public string username; //好友昵称
        public bool   bCollect; //是否收藏标志
        public UInt32 giveGiftTime;//给礼物时间
        public UInt32 applyTime; //申请时间
        public FRIEND_STATUS status;//好友状态
        public UInt32 level;//等级
        public UInt32 actionTime;//最近活动时间
        public UInt32 gift1;//领取礼物类型ID1
        public UInt32 gift2;//领取礼物类型ID2
        public UInt32 gift3;//领取礼物类型ID3
        public UInt32 arenaPoint;//竞技点
        public string arenaHonor;//竞技等级名
        public HeroInfo   hero = new HeroInfo();

        public void init(FRIEND_INFO_TMP info)
        {
            userid = info.idFriendUser;
            username = info.szNick;
            bCollect = (info.un8Collected==1);
            giveGiftTime = info.un32GiftGiveTime;
            applyTime = info.un32ApplyTime;
            status = (FRIEND_STATUS)info.un8Status;
            level = info.un16Level;
            actionTime = info.un32ActionTime;
            gift1 = info.idGiftType1;
            gift2 = info.idGiftType2;
            gift3 = info.idGiftType3;
            arenaPoint = info.un32ArenaPt;
            hero.InitDict((int)info.idHeroLeaderType);
            hero.skillLevel = info.un16HeroLeaderSkillLevel;
            hero.level = (int)info.un16HeroLeaderLevel;
            hero.btGrowup = info.un16HeroLeaderGrowup;
            hero.equipId = (int)info.idHeroLeaderEquipType;
            arenaHonor = UserInfo.getArenaHonor(arenaPoint);
        }

        public bool needGift(int gift)
        {
            if (gift == this.gift1 || gift == gift2 || gift == gift3)
                return true;
            return false;
        }
    }

    public class FriendGift
    {
        public FriendInfo friend;
        public ItemInfo giftItem;
        public UInt32 giftid;
        public UInt32 gifttype;
        public int giftClassify;
        public int itemType;
        public int amount;
        public string giftName;
        public string giftIcon;

        public void init(FRIEND_GIFT_INFO_TMP info)
        {
            giftid = info.giftid;
            gifttype = info.gifttype;
            friend = new FriendInfo();
            friend.userid = info.friendid;
            friend.username = info.freindName;
            friend.hero.InitDict((int)info.heroType);
            friend.hero.level = info.hreoLevel;

            iniGift((int)gifttype);
        }

        public void iniGift(int giftType)
        {
            
            ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_FRIEND_GIFT);
            ConfigRow row = table.getRow(DICT_FRIEND_GIFT.GIFT_TYPEID, giftType);

            if (row == null)
            {
                return;
            }
            
            amount = row.getIntValue(DICT_FRIEND_GIFT.AMOUNT);
            giftClassify = row.getIntValue(DICT_FRIEND_GIFT.CLASSIFY);

            if (giftClassify == 4)
            {
                itemType = row.getIntValue(DICT_FRIEND_GIFT.ITEM_TYPEID);

                giftItem = new ItemInfo();
                giftItem.init((int)itemType);

                giftName = giftItem.name;
                giftIcon = giftItem.icon;
            }

            switch (giftClassify)
            {
                case 1:
                    giftName = "金币";
                    giftIcon = "Star";
                    break;
                case 2:
                    giftName = "魂";
                    giftIcon = "Star";
                    break;
                case 3:
                    giftName = "友情点";
                    giftIcon = "Star";
                    break;
            }
        }
    }
}
