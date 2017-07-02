using System;
using System.IO;
using System.Runtime.InteropServices;
using BaseLib;

namespace DataCenter
{
    public class MSG_CLIENT_ARENA_BRIEF_INFO_EVENT : UnPacketBase
    {
        public UInt32 u32ArenaPoint;
        public UInt32 u32TotalWin;
        public UInt32 u32TotalLose;

        protected override bool unpackBody()
        {
            u32ArenaPoint = br.ReadUInt32();
            u32TotalWin = br.ReadUInt32();
            u32TotalLose = br.ReadUInt32();
            return true;
        }
    }

    public class MSG_CLIENT_ARENA_TARGET_REQUEST : PacketBase
    {
        public UInt64 idUser;
        protected override bool packetBody()
        {
            bw.Write(idUser);
            return true;
        }
    }

    public struct ARENA_TARGET_TMP
    {
        public UInt64 idTarget;
        public string szName; // 玩家名字
        public byte cbIsRobot; //0非机器人; 1机器人
        public UInt16 wUserLevel;
        public UInt16 wArenaRank;
        public UInt32 u32TotalWin;
        public UInt32 u32TotalLose;
        public UInt32 idLeaderHeroType;
    }

    public class MSG_CLIENT_ARENA_TARGET_RESPONSE : UnPacketBase
    {
        public UInt64 idUser;
        public UInt16 usCnt;
        public ARENA_TARGET_TMP[] lst;

        protected override bool unpackBody()
        {
            idUser = br.ReadUInt64();
            usCnt = br.ReadUInt16();
            lst = new ARENA_TARGET_TMP[usCnt];
            for (int index = 0;index < usCnt ;index++ )
            {
                lst[index].idTarget = br.ReadUInt64();
                lst[index].szName = this.getString(PACKET_LEN.MAX_NAME);
                lst[index].cbIsRobot = br.ReadByte();
                lst[index].wUserLevel = br.ReadUInt16();
                lst[index].wArenaRank = br.ReadUInt16();
                lst[index].u32TotalWin = br.ReadUInt32();
                lst[index].u32TotalLose = br.ReadUInt32();
                lst[index].idLeaderHeroType = br.ReadUInt32();
            }
            return true;
        }
    }

    public struct ARENA_HERO_INFO_TMP
    {
        public UInt32 idHeroType;
        public byte cbGrowup;
        public UInt16 wLevel;
        public UInt16 wSkillLevel;
        public UInt32 dwEquipTypeID; //英雄装备类型ID 
        public byte cbLeader; //0非队长; 1队长
        public byte cbPos; //站位
    }

    public class MSG_CLIENT_ARENA_TARGET_HERO_INFO_EVENT : UnPacketBase
    {
        public UInt64 idUser;
        public UInt16 usCnt;
        public ARENA_HERO_INFO_TMP[] lst;

        protected override bool unpackBody()
        {
            idUser = br.ReadUInt64();
            usCnt = br.ReadUInt16();
            lst = new ARENA_HERO_INFO_TMP[usCnt];
            for (int index = 0; index < usCnt;index++ )
            {
                lst[index].idHeroType = br.ReadUInt32();
                lst[index].cbGrowup = br.ReadByte();
                lst[index].wLevel = br.ReadUInt16();
                lst[index].wSkillLevel = br.ReadUInt16();
                lst[index].dwEquipTypeID = br.ReadUInt32();
                lst[index].cbLeader = br.ReadByte();
                lst[index].cbPos = br.ReadByte();
            }
            return true;
        }
    }

    public class MSG_CLIENT_ARENA_PVP_START_REQUEST : PacketBase
    {
        public UInt64 idUser;
        public UInt64 idTarget;//对手ID,玩家ID或者机器人ID
        public byte cbIsRobot;//0非机器人; 1机器人

        protected override bool packetBody()
        {
            bw.Write(idUser);
            bw.Write(idTarget);
            bw.Write(cbIsRobot);
            return true;
        }
    }

    public class MSG_CLIENT_ARENA_PVP_START_RESPONSE : UnPacketBase
    {
        public UInt32 errcode;

        protected override bool unpackBody()
        {
            errcode = br.ReadUInt32();
            return true;
        }
    }

    public class MSG_CLIENT_ARENA_PVP_END_REQUEST : PacketBase
    {
        //请求竞技场战斗结束
        public UInt32 u32SelfTotalHP;
        public UInt32 u32TargetTotalHP;
        public UInt32 u32SelfTotalHPRemain;
        public UInt32 u32TargetTotalHPRemain;
        public UInt32 u32SelfHeroAlive;
        public UInt32 u32TargetHeroAlive;
        public UInt32 u32SelfTotalDamage;
        public UInt32 u32TargetTotalDamage;

        protected override bool packetBody()
        {
            bw.Write(u32SelfTotalHP);
            bw.Write(u32TargetTotalHP);
            bw.Write(u32SelfTotalHPRemain);
            bw.Write(u32TargetTotalHPRemain);
            bw.Write(u32SelfHeroAlive);
            bw.Write(u32TargetHeroAlive);
            bw.Write(u32SelfTotalDamage);
            bw.Write(u32TargetTotalDamage);
            return true;
        }
    }

    public class MSG_CLIENT_ARENA_PVP_REWARD_EVENT : UnPacketBase
    {
        public byte cbResult; //ARENA_RESULT(1输; 2赢; 3和)
        public UInt16 n16AddPoint; //获得竞技点，有正负数
        public UInt16 wArenaRank;
        public UInt32 u32ArenaPoint;
        public UInt32 u32Diamond;
        public UInt32 idItemType;

        protected override bool unpackBody()
        {
            cbResult = br.ReadByte();
            n16AddPoint = br.ReadUInt16();
            wArenaRank = br.ReadUInt16();
            u32ArenaPoint = br.ReadUInt32();
            u32Diamond = br.ReadUInt32();
            idItemType = br.ReadUInt32();
            return true;
        }
    }

    public class MSG_CLIENT_ARENA_FRIEND_RANKING_REQUEST : PacketBase
    {
        public UInt64 idUser;

        protected override bool packetBody()
        {
            bw.Write(idUser);
            return true;
        }
    }

    public struct ARENA_RANKING_TMP
    {
        public UInt64 idUser;
        public string szName; // 玩家名字
        public UInt32 idLeaderHeroType;
        public UInt16 wHeroLevel;
        public UInt16 wArenaRank;
        public UInt32 u32ArenaPoint;
        public UInt32 u32TotalWin;
        public UInt32 u32TotalLose;
    }

    public class MSG_CLIENT_ARENA_FRIEND_RANKING_RESPONSE : UnPacketBase
    {
        public UInt64 idUser;
        public byte cbBeginFlag; //开始标志
        public byte cbEndFlag; //结束标志
        public UInt16 usCnt;
        public ARENA_RANKING_TMP[] lst;

        protected override bool unpackBody()
        {
            idUser = br.ReadUInt64();
            cbBeginFlag = br.ReadByte();
            cbEndFlag = br.ReadByte();
            usCnt = br.ReadUInt16();
            lst = new ARENA_RANKING_TMP[usCnt];

            for (int index=0; index< usCnt;index++ )
            {
                lst[index].idUser = br.ReadUInt64();
                lst[index].szName = getString(PACKET_LEN.MAX_NAME);
                lst[index].idLeaderHeroType = br.ReadUInt32();
                lst[index].wHeroLevel = br.ReadUInt16();
                lst[index].wArenaRank = br.ReadUInt16();
                lst[index].u32ArenaPoint = br.ReadUInt32();
                lst[index].u32TotalWin = br.ReadUInt32();
                lst[index].u32TotalLose = br.ReadUInt32();
            }
            return true;
        }
    }

    public class MSG_CLIENT_ARENA_TOTAL_RANKING_REQUEST : PacketBase
    {
        public UInt64 idUser;

        protected override bool packetBody()
        {
            bw.Write(idUser);
            return true;
        }
    }

    public class MSG_CLIENT_ARENA_TOTAL_RANKING_RESPONSE : UnPacketBase
    {
        public UInt64 idUser;
        public UInt32 u32UserRanking; //玩家当前的排名，如果在30名内，该字段无效，在30名外才有效
        public byte cbBeginFlag; //开始标志
        public byte cbEndFlag; //结束标志
        public UInt16 usCnt;
        public ARENA_RANKING_TMP[] lst;

        protected override bool unpackBody()
        {
            idUser = br.ReadUInt64();
            u32UserRanking = br.ReadUInt32();
            cbBeginFlag = br.ReadByte();
            cbEndFlag = br.ReadByte();
            usCnt = br.ReadUInt16();
            lst = new ARENA_RANKING_TMP[usCnt];

            for (int index = 0; index < usCnt; index++)
            {
                lst[index].idUser = br.ReadUInt64();
                lst[index].szName = getString(PACKET_LEN.MAX_NAME);
                lst[index].idLeaderHeroType = br.ReadUInt32();
                lst[index].wHeroLevel = br.ReadUInt16();
                lst[index].wArenaRank = br.ReadUInt16();
                lst[index].u32ArenaPoint = br.ReadUInt32();
                lst[index].u32TotalWin = br.ReadUInt32();
                lst[index].u32TotalLose = br.ReadUInt32();
            }
            return true;
        }
    }

    public class MSG_CLIENT_ARENA_HERO_FORMATION_REQUEST : PacketBase
    {
        public ushort usCnt;
        public TROOP[] lst;
        public UInt32 idFightTroop;

        protected override bool packetBody()
        {
            usCnt = (ushort)lst.Length;
            bw.Write(usCnt);
            bw.Write(idFightTroop);
            for (int i = 0; i < usCnt; ++i)
            {
                bw.Write(lst[i].idTroop);
                bw.Write(lst[i].btLeaderPos);
                bw.Write(lst[i].btFriendPos);
                bw.Write(lst[i].idPos1);
                bw.Write(lst[i].idPos2);
                bw.Write(lst[i].idPos3);
                bw.Write(lst[i].idPos4);
                bw.Write(lst[i].idPos5);
                bw.Write(lst[i].idPos6);
                bw.Write(lst[i].idLeader);
            }
            return true;
        }
    }

    public class MSG_CLIENT_ARENA_HERO_FORMATION_RESPONSE : UnPacketBase
    {
        public uint err;                           // 错误码 成功为100000

        protected override bool unpackBody()
        {
            err = br.ReadUInt32();
            return true;
        }
    }
}
