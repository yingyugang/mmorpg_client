using System;
using System.IO;
using System.Runtime.InteropServices;
using BaseLib;

namespace DataCenter
{
    public struct SUMMON_INFO
    {
        public UInt32 idSummon;
        public UInt32 idSummonType;
        public UInt16 wLevel;
        public UInt32 dwSoul;
        public byte btQuality;
        public byte btAtkSkill;
        public byte btInSkill;
        public byte btPassiveSkill;
        public byte btOutSkill;
    }

    public class MSG_CLIENT_SUMMON_LST_EVENT : UnPacketBase
    {
        UInt16 cnt;
        public SUMMON_INFO[] lst = null;

        protected override bool unpackBody()
        {
            cnt = br.ReadUInt16();
            lst = new SUMMON_INFO[cnt];

            for (int n = 0; n < cnt;n++ )
            {
                lst[n].idSummon = br.ReadUInt32();
                lst[n].idSummonType = br.ReadUInt32();
                lst[n].wLevel = br.ReadUInt16();
                lst[n].dwSoul = br.ReadUInt32();
                lst[n].btQuality = br.ReadByte();
                lst[n].btAtkSkill = br.ReadByte();
                lst[n].btInSkill = br.ReadByte();
                lst[n].btPassiveSkill = br.ReadByte();
                lst[n].btOutSkill = br.ReadByte();
            }
            return true;
        }
    }

    // 召唤兽进化
    public class MSG_CLIENT_SUMMON_EVOLVE_REQUEST : PacketBase
    {
        public UInt32 idSummon;
        public UInt16 usCnt;
        public UInt32[] lst = null;

        protected override bool packetBody()
        {
            bw.Write(idSummon);
            bw.Write(usCnt);
            for (int n=0; n < 5;n++)
            {
                if (n < usCnt && lst != null)
                    bw.Write(lst[n]);
                else
                    bw.Write((UInt32)0);
            }
            return true;
        }
    }

    public class MSG_CLIENT_SUMMON_EVOLVE_RESPONSE : UnPacketBase
    {
        public UInt32 idEvolveSummon;
        public UInt32 errCode;

        protected override bool unpackBody()
        {
            idEvolveSummon = br.ReadUInt32();
            errCode = br.ReadUInt32();
            return true;
        }
    }

    // 召唤兽增减事件
    public class MSG_CLIENT_SUMMON_CHANGE_EVENT : UnPacketBase
    {
        public byte tag;
        public UInt32 idSummon;
        public UInt32 idSummonType;

        protected override bool unpackBody()
        {
            tag = br.ReadByte();
            idSummon = br.ReadUInt32();
            idSummonType = br.ReadUInt32();
            return true;
        }
    }

    //召唤兽升级
    public class MSG_CLIENT_SUMMON_UPLVL_REQUEST : PacketBase
    {
        public UInt32 idSummon;
        public UInt32 unSoul;

        protected override bool packetBody()
        {
            bw.Write(idSummon);
            bw.Write(unSoul);
            return true;
        }
    }

    public class MSG_CLIENT_SUMMON_UPLVL_RESPONSE : UnPacketBase
    {
        public UInt32 errCode;           // 错误码 成功为100000
        public UInt32 idSummon;
        public UInt32 unLvl;         // 当前等级
        public UInt32 unSoul;        // 当前等级下 魂的数量

        protected override bool unpackBody()
        {
            errCode = br.ReadUInt32();
            idSummon = br.ReadUInt32();
            unLvl = br.ReadUInt32();
            unSoul = br.ReadUInt32();
            return true;
        }
    }

    // 召唤兽品质提升
    public class MSG_CLIENT_SUMMON_QUALITY_REQUEST : PacketBase
    {
        public UInt32 idSummon;
        protected override bool packetBody()
        {
            bw.Write(idSummon);
            return true;
        }
    }

    public class MSG_CLIENT_SUMMON_QUALITY_RESPONSE : UnPacketBase
    {
        public UInt32 errCode;
        public UInt32 idSummon;
        public byte btQuality;
        protected override bool unpackBody()
        {
            errCode = br.ReadUInt32();
            idSummon = br.ReadUInt32();
            btQuality = br.ReadByte();
            return true;
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // 召唤兽技能升级

    public class MSG_CLIENT_SUMMON_SKILL_UPLVL_REQUEST : PacketBase 
    {
        public UInt32 u32SeqId;
        public UInt32 idSummon;
        public byte btTag; // 0:攻击技能 1:入场技能 2:被动技能 3:离场技能
    };

    //////////////////////////////////////////////////////////////////////////
    // 召唤兽解锁
    public class MSG_CLIENT_SUMMON_UNLOCK_REQUEST : PacketBase
    {
        public UInt32 idSummonType;
        protected override bool packetBody()
        {
            bw.Write(idSummonType);
            return true;
        }
    }

    public class MSG_CLIENT_SUMMON_UNLOCK_RESPONSE : UnPacketBase
    {
        public UInt32 errCode;

        protected override bool unpackBody()
        {
            errCode = br.ReadUInt32();
            return true;
        }
    }
}
