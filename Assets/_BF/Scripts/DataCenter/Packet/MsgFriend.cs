using System;
using System.IO;
using System.Runtime.InteropServices;
using BaseLib;

namespace DataCenter
{
    public class MSG_CLIENT_FRIEND_LIST_REQUEST : PacketBase
    {
        public UInt64 idUser;
        protected override bool packetBody()
        {
            bw.Write(idUser);
            return true;
        }
    }

    public struct FRIEND_INFO_TMP
    {
        public UInt64 idFriendUser;//好友ID
        public byte un8Collected; //是否收藏，0：否，1：收藏不能删除好友
        public UInt32 un32GiftGiveTime;//礼物赠送时间
        public UInt32 un32ApplyTime;//申请好友时间
        public byte un8Status;//状态
        public string  szNick;//名称
        public UInt16  un16Level;//等级
        public UInt32  un32ActionTime;//行动时间
        public UInt32  idGiftType1;//领取礼物类型ID1
        public UInt32  idGiftType2;//领取礼物类型ID2
        public UInt32  idGiftType3;//领取礼物类型ID3
        public UInt32  un32ArenaPt;//竞技场点数
        public UInt32  idHeroLeaderType;//英雄队长类型ID
        public UInt16  un16HeroLeaderLevel;//英雄队长等级
        public UInt32  idHeroLeaderEquipType;//英雄队长装备ID
        public UInt16  un16HeroLeaderGrowup;//英雄队长成长类型
        public UInt16  un16HeroLeaderSkillLevel;//英雄队长技能等级
    }

    public class MSG_CLIENT_FRIEND_LIST : UnPacketBase
    {
        public UInt64 idUser;
        public byte un8Flag;//开始结束标志，0：开始1，：结束
        public UInt16 usCnt;
        public FRIEND_INFO_TMP[] lst;

        protected override bool unpackBody()
        {
            idUser = br.ReadUInt64();
            un8Flag = br.ReadByte();
            usCnt = br.ReadUInt16();
            lst = new FRIEND_INFO_TMP[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idFriendUser = br.ReadUInt64();
                lst[i].un8Collected = br.ReadByte();
                lst[i].un32GiftGiveTime = br.ReadUInt32();
                lst[i].un32ApplyTime = br.ReadUInt32();
                lst[i].un8Status = br.ReadByte();
                lst[i].szNick = this.getString(PACKET_LEN.MAX_NAME);//名称
                lst[i].un16Level = br.ReadUInt16();//等级
                lst[i].un32ActionTime = br.ReadUInt32();//行动时间
                lst[i].idGiftType1 = br.ReadUInt32();//领取礼物类型ID1
                lst[i].idGiftType2 = br.ReadUInt32() ;//领取礼物类型ID2
                lst[i].idGiftType3 = br.ReadUInt32();//领取礼物类型ID3
                lst[i].un32ArenaPt = br.ReadUInt32();//竞技场点数
                lst[i].idHeroLeaderType = br.ReadUInt32();//英雄队长类型ID
                lst[i].un16HeroLeaderLevel = br.ReadUInt16();//英雄队长等级
                lst[i].idHeroLeaderEquipType = br.ReadUInt32();//英雄队长装备ID
                lst[i].un16HeroLeaderGrowup = br.ReadUInt16();//英雄队长成长类型
                lst[i].un16HeroLeaderSkillLevel = br.ReadUInt16();//英雄队长技能等级
            }
            return true;
        }
    }

    public struct FRIEND_UPDATE_INFO_TEMP
    {
        public UInt64 idFriendUser;//好友ID
        public byte un8Collected;//是否收藏，0：否，1：收藏不能删除好友
        public UInt32 un32GiftGiveTime;//礼物赠送时间
        public UInt32 un32ApplyTime;//申请好友时间
        public byte un8Status;//状态
    };

    public class MSG_CLIENT_FRIEND_UPDATE : UnPacketBase
    {
        public UInt64 userid;
        public UInt16 uscnt;
        public FRIEND_UPDATE_INFO_TEMP[] lst;

        protected override bool unpackBody()
        {
            userid = br.ReadUInt64();
            uscnt = br.ReadUInt16();
            lst = new FRIEND_UPDATE_INFO_TEMP[uscnt];
            for (int index = 0; index < uscnt; index++)
            {
                lst[index].idFriendUser = br.ReadUInt64();
                lst[index].un8Collected = br.ReadByte();
                lst[index].un32GiftGiveTime = br.ReadUInt32();
                lst[index].un32ApplyTime = br.ReadUInt32();
                lst[index].un8Status = br.ReadByte();
            }
            return true;
        }
    }

    public class MSG_CLIENT_FRIEND_APPLY_REQUEST : PacketBase
    {
        public UInt64 userid;
        public UInt64 idTarget;
        protected override bool packetBody()
        {
            bw.Write(this.userid);
            bw.Write(this.idTarget);
            return true;
        }
    }

    public class MSG_CLIENT_FRIEND_APPLY_RESPONSE : UnPacketBase
    {
        public UInt64 userid;
        public UInt32 wErrCode;

        protected override bool unpackBody()
        {
            userid = br.ReadUInt64();
            wErrCode = br.ReadUInt32();
            return true;
        }
    }

    public class MSG_CLIENT_FRIEND_REFUSE_REQUEST : PacketBase
    {
        public UInt64 userid;
        public UInt64 idTarget;

        protected override bool packetBody()
        {
            bw.Write(userid);
            bw.Write(idTarget);
            return true;
        }
    }

    public class MSG_CLIENT_FRIEND_REFUSE_RESPONSE : UnPacketBase
    {
        public UInt64 userid;
        public UInt32 wErrCode;
        protected override bool unpackBody()
        {
            userid = br.ReadUInt64();
            wErrCode = br.ReadUInt32();
            return true;
        }
    }

    public class MSG_CLIENT_FRIEND_ACCEPT_REQUEST : PacketBase
    {
        public UInt64 userid;
        public UInt64 idTarget;

        protected override bool packetBody()
        {
            bw.Write(userid);
            bw.Write(idTarget);
            return true;
        }
    }

    public class MSG_CLIENT_FRIEND_ACCEPT_RESPONSE : UnPacketBase
    {
        public UInt64 userid;
        public UInt32 wErrCode;

        protected override bool unpackBody()
        {
            userid = br.ReadUInt64();
            wErrCode = br.ReadUInt32();
            return true;
        }
    }

    public class MSG_CLIENT_FRIEND_DEL_REQUEST : PacketBase
    {
        public UInt64 userid;
        public UInt64 idTarget;

        protected override bool packetBody()
        {
            bw.Write(userid);
            bw.Write(idTarget);
            return true;
        }
    }

    public class MSG_CLIENT_FRIEND_DEL_RESPONSE : UnPacketBase
    {
        public UInt64 userid;
        public UInt32 wErrCode;

        protected override bool unpackBody()
        {
            userid = br.ReadUInt64();
            wErrCode = br.ReadUInt32();
            return true;
        }
    }

    public class MSG_CLIENT_SEARCH_USER_REQUEST : PacketBase
    {
        public UInt64 userid;
        public UInt64 idTarget;

        protected override bool packetBody()
        {
            bw.Write(userid);
            bw.Write(idTarget);
            return true;
        }
    }

    public class MSG_CLIENT_SEARCH_USER_RESPONSE : UnPacketBase
    {
        public UInt64 userid;
        public UInt64 targetid;
        public UInt32 wErrCode;
        public string strName;
        public UInt16 level;
        public UInt32 arenaPoint;
        public UInt32 heroType;
        public UInt16 heroLevel;

        protected override bool unpackBody()
        {
            userid = br.ReadUInt64();
            targetid = br.ReadUInt64();
            wErrCode = br.ReadUInt32();
            strName = this.getString(PACKET_LEN.MAX_NAME);
            level = br.ReadUInt16();
            arenaPoint = br.ReadUInt32();
            heroType = br.ReadUInt32();
            heroLevel = br.ReadUInt16();
            return true;
        }
    }

    public class MSG_CLIENT_FRIEND_GIVE_GIFT_REQUEST : PacketBase
    {
        public UInt64 userid;
        public UInt32 idGiftType;
        public UInt16 usCnt;
        public UInt64[] idTarget = null;

        public byte sendtype; //idTarget==0有效果，0=全部送出，1=赠送给需要当前礼物类型的所有好友

        protected override bool packetBody()
        {
            bw.Write(userid);
            bw.Write(idGiftType);
            bw.Write(usCnt);
            for (int index = 0; index < 100; index++)
            {
                if (index < usCnt && idTarget != null)
                    bw.Write(idTarget[index]);
                else
                    bw.Write((UInt64)0);
            }
            return true;
        }
    }

    public class MSG_CLIENT_FRIEND_GIVE_GIFT_RESPONSE : UnPacketBase
    {
        public UInt64 userid;
        public UInt32 wErrCode;

        protected override bool unpackBody()
        {
            userid = br.ReadUInt64();
            wErrCode = br.ReadUInt32();
            return true;
        }
    }

    public class MSG_CLIENT_FRIEND_AWARD_GIFT_REQUEST : PacketBase
    {
        public UInt64 userid;
        public UInt64 idTarget;
        public UInt32 idGift;

        protected override bool packetBody()
        {
            bw.Write(userid);
            bw.Write(idTarget);
            bw.Write(idGift);
            return true;
        }
    }

    public class MSG_CLIENT_FRIEND_AWARD_GIFT_RESPONSE : UnPacketBase
    {
        public UInt64 userid;
        public UInt32 wErrCode;

        protected override bool unpackBody()
        {
            userid = br.ReadUInt64();
            wErrCode = br.ReadUInt32();
            return true;
        }
    }

    public struct FRIEND_GIFT_INFO_TMP
    {
        public UInt64 friendid;
        public UInt32 giftid;
        public UInt32 gifttype;

        public string freindName;
        public UInt32 heroType;
        public UInt16 hreoLevel;
    }

    public class MSG_CLIENT_FRIEND_GIFT_LIST : UnPacketBase
    {
        public UInt64 userid;
        public UInt16 cnt;
        public byte un8Flag;
        public FRIEND_GIFT_INFO_TMP[] lst;

        protected override bool unpackBody()
        {
            userid = br.ReadUInt64();
            cnt = br.ReadUInt16();
            un8Flag = br.ReadByte();
            lst = new FRIEND_GIFT_INFO_TMP[cnt];
            for(int n=0;n<cnt;n++)
            {
                lst[n].friendid = br.ReadUInt64();
                lst[n].giftid = br.ReadUInt32();
                lst[n].gifttype = br.ReadUInt32();
                lst[n].freindName = this.getString(PACKET_LEN.MAX_NAME);
                lst[n].heroType = br.ReadUInt32();
                lst[n].hreoLevel = br.ReadUInt16();
            }
            return true;
        }
    }

    public class MSG_CLIENT_FRIEND_GIFT_DEL : UnPacketBase
    {
        public UInt64 userid;
        public UInt32 idGift;

        protected override bool unpackBody()
        {
            userid = br.ReadUInt64(); ;
            idGift = br.ReadUInt32();
            return true;
        }
    }

    public class MSG_CLIENT_FRIEND_GIFT_SET_REQUEST : PacketBase
    {
        public byte index;
        public UInt32 gifttype;

        protected override bool packetBody()
        {
            bw.Write(index);
            bw.Write(gifttype);
            return true;
        }
    }

    public class MSG_CLIENT_FRIEND_GIFT_SET_RESPONSE : UnPacketBase
    {
        public UInt32 errcode;

        protected override bool unpackBody()
        {
            errcode = br.ReadUInt32();
            return true;
        }
    }

    public class MSG_CLIENT_FRIEND_COLLECT_REQUEST : PacketBase
    {
        public UInt64 userid;
        public UInt64 idTarget;

        protected override bool packetBody()
        {
            bw.Write(userid);
            bw.Write(idTarget);
            return true;
        }
    }
    
    public class MSG_CLIENT_FRIEND_COLLECT_RESPONSE : UnPacketBase
    {
        public UInt64 idUser;
        public UInt32 wErrCode;

        protected override bool unpackBody()
        {
            idUser = br.ReadUInt64();
            wErrCode = br.ReadUInt32();
            return true;
        }
    }

    public class MSG_CLIENT_FRIEND_CANCEL_APPLY_REQUEST : PacketBase
    {
        public UInt64 userid;
        public UInt64 idTarget;

        protected override bool packetBody()
        {
            bw.Write(userid);
            bw.Write(idTarget);
            return true;
        }
    }

    public class MSG_CLIENT_FRIEND_CANCEL_APPLY_RESPONSE:UnPacketBase
    {
        public UInt64 idUser;
        public UInt32 wErrCode;

        protected override bool unpackBody()
        {
            idUser = br.ReadUInt64();
            wErrCode = br.ReadUInt32();
            return true;
        }
    }

    public class MSG_CLIENT_FRIEND_GIFT_LIST_REQUEST : PacketBase
    {
        public UInt64 userid;

        protected override bool packetBody()
        {
            bw.Write(userid);
            return true;
        }
    }
}
