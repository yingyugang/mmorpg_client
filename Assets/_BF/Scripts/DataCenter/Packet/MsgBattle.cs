using System;
using System.IO;
using System.Runtime.InteropServices;
using BaseLib;

namespace DataCenter
{

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_BATTLE_PVE_NORMAL_BATTLE_REQUEST    
    public class MSG_CLIENT_BATTLE_PVE_NORMAL_BATTLE_REQUEST : PacketBase
    {
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_BATTLE_PVE_NORMAL_BATTLE_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BATTLE_PVE_NORMAL_BATTLE
    {
        public uint      idField;
        public byte      cbStatus;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_CLIENT_BATTLE_PVE_NORMAL_BATTLE_RESPONSE : UnPacketBase
    {
        public ushort usCnt;
        public BATTLE_PVE_NORMAL_BATTLE[] lst;

        protected override bool unpackBody()
        {
            usCnt = br.ReadUInt16();
            lst = new BATTLE_PVE_NORMAL_BATTLE[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idField = br.ReadUInt32();
                lst[i].cbStatus = br.ReadByte();
            }
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_BATTLE_PVE_ACTIVITY_BATTLE_REQUEST    
    public class MSG_CLIENT_BATTLE_PVE_ACTIVITY_BATTLE_REQUEST : PacketBase
    {  
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_BATTLE_PVE_ACTIVITY_BATTLE_RESPONSE    
    public struct BATTLE_PVE_ACTIVITY_BATTLE
    {
        public uint      idField;
        public byte      cbStatus;
        public uint      u32DueTime;
    }

    public class MSG_CLIENT_BATTLE_PVE_ACTIVITY_BATTLE_RESPONSE : UnPacketBase
    {
        public ushort usCnt;
        public BATTLE_PVE_ACTIVITY_BATTLE[] lst;

        protected override bool unpackBody()
        {
            usCnt = br.ReadUInt16();
            lst = new BATTLE_PVE_ACTIVITY_BATTLE[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idField = br.ReadUInt32();
                lst[i].cbStatus = br.ReadByte();
                lst[i].u32DueTime = br.ReadUInt32();
            }
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_BATTLE_PVE_START_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_CLIENT_BATTLE_PVE_START_REQUEST : PacketBase
    {
        public uint   idHelpUser;
        public uint   idField;
        public uint   u32TotalHP;

        protected override bool packetBody()
        {
            bw.Write(idHelpUser);
            bw.Write(idField);
            bw.Write(u32TotalHP);
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_BATTLE_PVE_START_RESPONSE    
    public class MSG_CLIENT_BATTLE_PVE_START_RESPONSE : UnPacketBase
    {
        public uint   idField;
        public uint   u32ErrCode;

        protected override bool unpackBody()
        {
            idField = br.ReadUInt32();
            u32ErrCode = br.ReadUInt32();
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_BATTLE_PVE_FIELD_ENEMY_EVENT    

    public struct ENEMY_INFO
    {
        public byte      cbCurStep;
        public uint      idEnemy;
    }

    public class MSG_CLIENT_BATTLE_PVE_FIELD_ENEMY_EVENT : UnPacketBase
    {
        public uint   idField;
        public ushort usCnt;
        public ENEMY_INFO[] lst;

        protected override bool unpackBody()
        {
            idField = br.ReadUInt32();
            usCnt = br.ReadUInt16();
            lst = new ENEMY_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].cbCurStep = br.ReadByte();
                lst[i].idEnemy = br.ReadUInt32();
            }
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_BATTLE_PVE_FIELD_MONSTER_CATCH_EVENT    
    public struct MONSTER_CATCH_INFO
    {
        public byte      cbCurStep;
        public uint      idMonsterType;
        public uint      idHeroType;
        public byte      cbGrowup;
        public ushort    wLevel;
    }

    public class MSG_CLIENT_BATTLE_PVE_FIELD_MONSTER_CATCH_EVENT : UnPacketBase
    {
        public uint   idField;
        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        public MONSTER_CATCH_INFO[] lst;

        protected override bool unpackBody()
        {
            idField = br.ReadUInt32();

            usCnt = br.ReadUInt16();
            lst = new MONSTER_CATCH_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].cbCurStep = br.ReadByte();
                lst[i].idMonsterType = br.ReadUInt32();
                lst[i].idHeroType = br.ReadUInt32();
                lst[i].cbGrowup = br.ReadByte();
                lst[i].wLevel = br.ReadUInt16();
            }
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_BATTLE_PVE_FIELD_DROP_CHESTS_EVENT    
    public struct DROP_CHESTS_INFO
    {
        public uint      idChests;
        public uint      u32Coin;
        public uint      u32Soul;
        public uint      idItemType;
        public uint      idMonsterType;
        public byte      cbGrowup;
        public ushort    wLevel;
    }

    public class MSG_CLIENT_BATTLE_PVE_FIELD_DROP_CHESTS_EVENT : UnPacketBase
    {
        public ushort wSize;
        public ushort wType;
        public uint   idField;

        public ushort usCnt;
        public DROP_CHESTS_INFO[] lst;

        protected override bool unpackBody()
        {
            idField = br.ReadUInt32();
            usCnt = br.ReadUInt16();
            lst = new DROP_CHESTS_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idChests = br.ReadUInt32();
                lst[i].u32Coin = br.ReadUInt32();
                lst[i].u32Soul = br.ReadUInt32();
                lst[i].idItemType = br.ReadUInt32();
                lst[i].idMonsterType = br.ReadUInt32();
                lst[i].cbGrowup = br.ReadByte();
                lst[i].wLevel = br.ReadUInt16();
            }
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_BATTLE_PVE_FIELD_DROP_ITEM_EVENT    
    public struct DROP_ITEM_INFO
    {
        public uint      idItemType;
        public uint      u32Amount;
    }

    public class MSG_CLIENT_BATTLE_PVE_FIELD_DROP_ITEM_EVENT : UnPacketBase
    {
        public uint   idField;
        public ushort usCnt;
        public DROP_ITEM_INFO[] lst;

        protected override bool unpackBody()
        {
            idField = br.ReadUInt32();
            usCnt = br.ReadUInt16();
            lst = new DROP_ITEM_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idItemType = br.ReadUInt32();
                lst[i].u32Amount = br.ReadUInt32();
            }
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_BATTLE_PVE_END_REQUEST    

    public class MSG_CLIENT_BATTLE_PVE_END_REQUEST : PacketBase
    {
        public uint   idField;
        public uint   u32TotalHPRemain;
        public uint   u32Coin;
        public uint   u32Soul;

        protected override bool packetBody()
        {
            bw.Write(idField);
            bw.Write(u32TotalHPRemain);
            bw.Write(u32Coin);
            bw.Write(u32Soul);
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_BATTLE_PVE_REWARD_EVENT    
    public class MSG_CLIENT_BATTLE_PVE_REWARD_EVENT : UnPacketBase
    {
        public uint   idField;
        public byte   cbResult;
        public byte   cbBattleAllPass;
        public byte   cbMaxStar;
        public uint   u32Exp;
        public uint   u32Diamond;

        protected override bool unpackBody()
        {
            idField = br.ReadUInt32();
            cbResult = br.ReadByte();
            cbBattleAllPass = br.ReadByte();
            cbMaxStar = br.ReadByte();
            u32Exp = br.ReadUInt32();
            u32Diamond = br.ReadUInt32();
            return true;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_BATTLE_PVE_USER_HELP_REQUEST    

    public class MSG_CLIENT_BATTLE_PVE_USER_HELP_REQUEST : PacketBase
    {
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_BATTLE_PVE_USER_HELP_RESPONSE    

    public struct USER_HELP_INFO
    {
        public uint      idUser;
        public string szName;            // 玩家名字
        public ushort    wUserLevel;            // idUser的等级
        public byte      cbIsFriend;            // idUser与请求者是否好友
        public byte      cbFriendPoint;            // 若为好友，显示好友点数
        public uint      idHeroType;            // 好友出战队列的队长
        public byte      cbGrowup;            // idHeroType对应的成长类型
        public ushort    wLevel;            // idHeroType的等级
        public ushort    wSkillLevel;            // idHeroType的技能等级
        public uint      dwEquipID;            // idHeroType的装备类型ID
    }

    public class MSG_CLIENT_BATTLE_PVE_USER_HELP_RESPONSE : UnPacketBase
    {
        public uint   u32ErrCode;
        public ushort usCnt;
        public USER_HELP_INFO[] lst;

        protected override bool unpackBody()
        {
            u32ErrCode = br.ReadUInt32();
            usCnt = br.ReadUInt16();
            lst = new USER_HELP_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idUser = br.ReadUInt32();
                lst[i].szName = getString(PACKET_LEN.MAX_NAME);
                lst[i].wUserLevel = br.ReadUInt16();
                lst[i].cbIsFriend = br.ReadByte();
                lst[i].cbFriendPoint = br.ReadByte();
                lst[i].idHeroType = br.ReadUInt32();
                lst[i].cbGrowup = br.ReadByte();
                lst[i].wLevel = br.ReadUInt16();
                lst[i].wSkillLevel = br.ReadUInt16();
                lst[i].dwEquipID = br.ReadUInt32();
            }
            return true;
        }
    }  // end struct
}

