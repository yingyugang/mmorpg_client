using System;
using System.IO;
using System.Runtime.InteropServices;
using BaseLib;

namespace DataCenter
{
    public struct HERO_INFO
    {
        public uint idHero;
        public uint idType;
        public byte btGrowup;            // 攻击、防御、体力、回复、平衡 服务器随机生成，作为英雄升级分配点数的依据
        public ushort wLevel;
        public ushort wSkillLevel;            // 在强化过程中有可能出现必杀技能升级的情况
        public uint dwExp;
        public uint dwEquipID;
        public byte btCollected;            // 收藏宠物不能被出售/强化 0：未收藏 1:  收藏
    }

    public class MSG_CLIENT_HERO_LST_EVENT : UnPacketBase
    {
        public ushort usCnt;
        public HERO_INFO[] lst;

        protected override bool unpackBody()
        {
            usCnt = br.ReadUInt16();
            lst = new HERO_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idHero = br.ReadUInt32();
                lst[i].idType = br.ReadUInt32();
                lst[i].btGrowup = br.ReadByte();
                lst[i].wLevel = br.ReadUInt16();
                lst[i].wSkillLevel = br.ReadUInt16();
                lst[i].dwExp = br.ReadUInt32();
                lst[i].dwEquipID = br.ReadUInt32();
                lst[i].btCollected = br.ReadByte();
            }
            return true;
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // MSG_DIAMOND_ACQUIRE_HERO_REQUEST    钻石抽卡
    public class MSG_DIAMOND_ACQUIRE_HERO_REQUEST : PacketBase
    {
        public byte method;                        // 0: 1次抽卡  1: 10次抽卡

        protected override bool packetBody()
        {
            bw.Write(method);
            return true;
        }
    }

    public class MSG_DIAMOND_ACQUIRE_HERO_RESPONSE : UnPacketBase
    {
        public uint err;                           // 错误码 成功为100000

        protected override bool unpackBody()
        {
            err = br.ReadUInt32();
            return true;
        }
    }

    //////////////////////////////////////////////////////////////////////////
    //友情抽卡
    public class MSG_FRIEND_ACQUIRE_HERO_REQUEST : PacketBase
    {
        public byte method;                        // 0: 1次抽卡  1: 8次抽卡

        protected override bool packetBody()
        {
            bw.Write(method);
            return true;
        }
    }

    // MSG_FRIEND_ACQUIRE_HERO_RESPONSE    友情抽卡
    public class MSG_FRIEND_ACQUIRE_HERO_RESPONSE : UnPacketBase
    {
        public uint err;                           // 错误码 成功为100000

        protected override bool unpackBody()
        {
            err = br.ReadUInt32();
            return true;
        }
    }
    //////////////////////////////////////////////////////////////////////////
    //付费抽取经验胖子
    public class MSG_ACQUIRE_EXP_FAT_REQUEST : PacketBase
    {
        public byte method;                        // 0: 1次抽卡  1: 5次抽卡

        protected override bool packetBody()
        {
            bw.Write(method);
            return true;
        }
    }


    public class MSG_ACQUIRE_EXP_FAT_RESPONSE : UnPacketBase
    {
        public uint err;                           // 错误码 成功为100000

        protected override bool unpackBody()
        {
            err = br.ReadUInt32();
            return true;
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // MSG_HERO_CHANGE_EVENT    英雄增减事件
    public struct HERO_CHANGE
    {
        public uint idHero;
        public uint idHeroType;
        public byte btGrowup;
        public ushort wLevel;
        public byte btSkillLv1;
    }

    public class MSG_HERO_CHANGE_EVENT : UnPacketBase
    {
        public byte tag;                           // 0:新增	1:删除
        public ushort usCnt;
        public HERO_CHANGE[] lst;

        protected override bool unpackBody()
        {
            tag = br.ReadByte();
            usCnt = br.ReadUInt16();
            lst = new HERO_CHANGE[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idHero = br.ReadUInt32();
                lst[i].idHeroType = br.ReadUInt32();
                lst[i].btGrowup = br.ReadByte();
                lst[i].wLevel = br.ReadUInt16();
                lst[i].btSkillLv1 = br.ReadByte();
            }
            return true;
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // MSG_HERO_SELL_REQUEST    出售英雄
    public struct HERO_ID
    {
        public uint idHero;
    }

    public class MSG_HERO_SELL_REQUEST : PacketBase
    {
        public ushort usCnt;
        public HERO_ID[] lst;

        protected override bool packetBody()
        {
            usCnt = (ushort)lst.Length;
            bw.Write(usCnt);
            for (int i = 0; i < usCnt; ++i)
            {
                bw.Write(lst[i].idHero);
            }
            return true;
        }
    }  // end struct

    //////////////////////////////////////////////////////////////////////////
    // MSG_HERO_EVOLVE_REQUEST    英雄进化
    public class MSG_HERO_EVOLVE_REQUEST : PacketBase
    {
        public uint idMajorHero;
        public ushort usCnt;
        public HERO_ID[] lst;

        protected override bool packetBody()
        {
            usCnt = (ushort)lst.Length;
            bw.Write(idMajorHero);
            bw.Write(usCnt);
            for (int i = 0; i < usCnt; ++i)
            {
                bw.Write(lst[i].idHero);
            }
            return true;
        }
    }  // end struct

    //////////////////////////////////////////////////////////////////////////
    // MSG_HERO_EVOLVE_RESPONSE    英雄进化

    public class MSG_HERO_EVOLVE_RESPONSE : UnPacketBase
    {
        public uint idEvolveHero;                  // 进化成功后的新英雄ID
        public uint err;                           // 错误码 成功为100000

        protected override bool unpackBody()
        {
            idEvolveHero = br.ReadUInt32();
            err = br.ReadUInt32();
            return true;
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // MSG_HERO_ENHANCE_REQUEST    英雄强化
    public class MSG_HERO_ENHANCE_REQUEST : PacketBase
    {
        public uint idMajorHero;
        public ushort usCnt;

        public HERO_ID[] lst;

        protected override bool packetBody()
        {
            usCnt = (ushort)lst.Length;
            bw.Write(idMajorHero);
            bw.Write(usCnt);
            for (int i = 0; i < usCnt; ++i)
            {
                bw.Write(lst[i].idHero);
            }
            return true;
        }
    }  // end struct

    //////////////////////////////////////////////////////////////////////////
    // MSG_HERO_ENHANCE_RESPONSE    
    public class MSG_HERO_ENHANCE_RESPONSE : UnPacketBase
    {
        public uint res;                           // 错误码 成功为100000
        public uint unExp;                         // 英雄强化后最终的EXP值
        public byte btTag;                         // res=100000该参数才有效 0:成功  1:大成功  2:超成功
        public byte btSkillLvl;                    // 技能等级

        protected override bool unpackBody()
        {
            res = br.ReadUInt32();
            unExp = br.ReadUInt32();
            btTag = br.ReadByte();
            btSkillLvl = br.ReadByte();
            return true;
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // MSG_HERO_COLLECT_REQUEST    英雄收藏
    public class MSG_HERO_COLLECT_REQUEST : PacketBase
    {
        public uint idHero;
        public byte tag;                           // 0:取消收藏		1:收藏

        protected override bool packetBody()
        {
            bw.Write(idHero);
            bw.Write(tag);
            return true;
        }
    }  // end struct

    //////////////////////////////////////////////////////////////////////////
    // MSG_HERO_ATTR_EVENT    英雄属性更新事件
    public struct HERO_ATTR_INFO
    {
        public ushort wAttr;            // 英雄属性索引
        public uint unVal;            // 英雄属性值
    }

    public class MSG_HERO_ATTR_EVENT : UnPacketBase
    {
        public uint idHero;
        public ushort usCnt;
        public HERO_ATTR_INFO[] lst;
        
        protected override bool unpackBody()
        {
            idHero = br.ReadUInt32();
            usCnt = br.ReadUInt16();
            lst = new HERO_ATTR_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].wAttr = br.ReadUInt16();
                lst[i].unVal = br.ReadUInt32();
            }
            return true;
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // MSG_HERO_FORMATION_REQUEST    英雄出战设置
    public struct TROOP
    {
        public uint idTroop;            // 队伍id
        public byte btLeaderPos;            // 队长位置
        public byte btFriendPos;            // 好友位置
        public uint idPos1;            // 位置1的英雄
        public uint idPos2;            // 位置2的英雄
        public uint idPos3;            // 位置3的英雄
        public uint idPos4;            // 位置4的英雄
        public uint idPos5;            // 位置5的英雄
        public uint idPos6;            // 位置6的英雄
        public uint idLeader;            // 队长英雄ID
    }

    public class MSG_HERO_FORMATION_REQUEST : PacketBase
    {
        public uint idFightTroop;                  // 出战队伍id

        public ushort usCnt;
        public TROOP[] lst;

        protected override bool packetBody()
        {
            bw.Write(idFightTroop);
            usCnt = (ushort)lst.Length;
            bw.Write(usCnt);
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

    //////////////////////////////////////////////////////////////////////////
    // MSG_HERO_FORMATION_RESPONSE    英雄出战设置
    public class MSG_HERO_FORMATION_RESPONSE : UnPacketBase
    {
        public uint err;                           // 错误码 成功为100000

        protected override bool unpackBody()
        {
            err = br.ReadUInt32();
            return true;
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // MSG_HERO_FORMATION_TRIAL_REQUEST    英雄召唤院出战设置
    public class MSG_HERO_FORMATION_TRIAL_REQUEST : PacketBase
    {

        public ushort usCnt;
        public TROOP[] lst;

        protected override bool packetBody()
        {
            usCnt = (ushort)lst.Length;
            bw.Write(usCnt);
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

    public class MSG_HERO_FORMATION_TRIAL_RESPONSE : UnPacketBase
    {
        public uint err;                           // 错误码 成功为100000

        protected override bool unpackBody()
        {
            err = br.ReadUInt32();
            return true;
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // MSG_HERO_FORMATION_EVENT    登录下发英雄阵型信息
    public class MSG_HERO_FORMATION_EVENT : UnPacketBase
    {
        public ushort usCnt;
        public TROOP[] lst;

        protected override bool unpackBody()
        {
            usCnt = br.ReadUInt16();
            lst = new TROOP[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idTroop = br.ReadUInt32();
                lst[i].btLeaderPos = br.ReadByte();
                lst[i].btFriendPos = br.ReadByte();
                lst[i].idPos1 = br.ReadUInt32();
                lst[i].idPos2 = br.ReadUInt32();
                lst[i].idPos3 = br.ReadUInt32();
                lst[i].idPos4 = br.ReadUInt32();
                lst[i].idPos5 = br.ReadUInt32();
                lst[i].idPos6 = br.ReadUInt32();
                lst[i].idLeader = br.ReadUInt32();
            }
            return true;
        }
    }  // end struct
}
