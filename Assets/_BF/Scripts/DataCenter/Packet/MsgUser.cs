using System;
using System.IO;
using System.Runtime.InteropServices;
using BaseLib;

namespace DataCenter
{
    public class MSG_CLIENT_USER_INFO_EVENT : UnPacketBase
    {
        public string szName;// 玩家名字
        public ushort wLevel;
        public uint dwExp;
        public ushort wPower;                        // 当前体力值
        public byte btVip;
        public uint dwCoin;
        public uint dwSoul;                        // 魂数量 用于升级村庄建筑
        public uint dwDiamond;
        public byte btArenaDiamond;                // 竞技场宝珠
        public uint dwArenaPt;                     // 竞技场点数
        public uint dwGayPt;                       // 友情召唤伙伴点数
        public ushort wKeyGold;                      // 金钱副本开启钥匙数量
        public ushort wKeySilver;                    // 经验副本开启钥匙数量
        public byte btFightTroop;
        public ushort wMaxPet;                       // 伙伴数量上限
        public ushort wMaxItem;                      // 物品数量上限
        public UInt32[] heroLst = new UInt32[32];  // 伙伴图鉴 十六进制字符串
        public UInt32[] itemLst = new UInt32[16];   // 物品图鉴 十六进制字符串
        public UInt64 unPowerRecoverTime;            // 上次体力恢复时间
        public UInt64 unArenaRecoverTime;            // 上次竞技点恢复时间
        public UInt64 unLoginedTime;                 // 最近一次登录时间
        public UInt64 unActionTime;                  // 最近一次行动时间 
        public UInt32 idGiftType1;                  //索求礼物类型
        public UInt32 idGiftType2;
        public UInt32 idGiftType3;

        public uint idUseItemType1;                // 战斗使用物品类型ID1
        public uint unUseItemAmount1;              // 战斗使用物品数量1
        public uint idUseItemType2;                // 战斗使用物品类型ID2
        public uint unUseItemAmount2;              // 战斗使用物品数量2
        public uint idUseItemType3;                // 战斗使用物品类型ID3
        public uint unUseItemAmount3;              // 战斗使用物品数量3
        public uint idUseItemType4;                // 战斗使用物品类型ID4
        public uint unUseItemAmount4;              // 战斗使用物品数量4
        public uint idUseItemType5;                // 战斗使用物品类型ID5
        public uint unUseItemAmount5;              // 战斗使用物品数量5
        public UInt64 unLastGetKeyTime;             // 上次领取钥匙时间
        public UInt64 i64LastGetPresentTime;           // 上次领取登录奖励的时间
        public byte btPresentSeq;                      // 上次领取登录奖励的顺位
        public byte idArenaTroop;                  //竞技场出战阵型id

        protected override bool unpackBody()
        {
            szName = this.getString(PACKET_LEN.MAX_NAME);
            wLevel = br.ReadUInt16();
            dwExp = br.ReadUInt32();
            wPower = br.ReadUInt16();
            btVip = br.ReadByte();
            dwCoin = br.ReadUInt32();
            dwSoul = br.ReadUInt32();
            dwDiamond = br.ReadUInt32();
            btArenaDiamond = br.ReadByte();
            dwArenaPt = br.ReadUInt32();
            dwGayPt = br.ReadUInt32();
            wKeyGold = br.ReadUInt16();
            wKeySilver = br.ReadUInt16();
            btFightTroop = br.ReadByte();
            wMaxPet = br.ReadUInt16();
            wMaxItem = br.ReadUInt16();
            for (int n = 0; n < 32;n++ )
                heroLst[n] = br.ReadUInt32();
            for (int n = 0; n < 16; n++)
                itemLst[n] = br.ReadUInt32();
            unPowerRecoverTime = br.ReadUInt64();
            unArenaRecoverTime = br.ReadUInt64();
            unLoginedTime = br.ReadUInt64();
            unActionTime = br.ReadUInt64();
            idGiftType1 = br.ReadUInt32();
            idGiftType2 = br.ReadUInt32();
            idGiftType3 = br.ReadUInt32();

            idUseItemType1 = br.ReadUInt32();
            unUseItemAmount1 = br.ReadUInt32();
            idUseItemType2 = br.ReadUInt32();
            unUseItemAmount2 = br.ReadUInt32();
            idUseItemType3 = br.ReadUInt32();
            unUseItemAmount3 = br.ReadUInt32();
            idUseItemType4 = br.ReadUInt32();
            unUseItemAmount4 = br.ReadUInt32();
            idUseItemType5 = br.ReadUInt32();
            unUseItemAmount5 = br.ReadUInt32();
            unLastGetKeyTime = br.ReadUInt64();
            i64LastGetPresentTime = br.ReadUInt64();
            btPresentSeq = br.ReadByte();
            idArenaTroop = br.ReadByte();
            return true;
        }
    }

    public class MSG_SEL_MAIN_HERO_REQUEST : PacketBase
    {
        public byte btJob;                         // 主角职业

        protected override bool packetBody()
        {
            bw.Write(btJob);
            return true;
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // MSG_CLIENT_USER_ATTR_EVENT    玩家属性信息

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_USER_ATTR_INFO
    {
        public ushort wUserAttrIndex;
        public uint dwUserAttrData;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_CLIENT_USER_ATTR_EVENT : UnPacketBase
    {
        public ushort usCnt;
        public MSG_CLIENT_USER_ATTR_INFO[] lst;

        protected override bool unpackBody()
        {
            usCnt = br.ReadUInt16();
            lst = new MSG_CLIENT_USER_ATTR_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].wUserAttrIndex = br.ReadUInt16();
                lst[i].dwUserAttrData = br.ReadUInt32();
            }
            return true;
        }
    }

    //购买英雄格子
    public class MSG_BUY_HERO_SIZE_REQUEST : PacketBase
    {
    }

    public class MSG_BUY_HERO_SIZE_RESPONSE : UnPacketBase
    {
        public uint err;                           // 错误码 成功为100000

        protected override bool unpackBody()
        {
            err = br.ReadUInt32();
            return true;
        }
    }

    //恢复体力
    public class MSG_BUY_POWER_REQUEST : PacketBase
    {
    }

    public class MSG_CLIENT_GET_KEY_REQUEST : PacketBase
    {
    }
}
