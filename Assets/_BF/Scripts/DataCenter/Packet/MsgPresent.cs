using System;
using System.IO;
using System.Runtime.InteropServices;
using BaseLib;

namespace DataCenter
{
    public struct PRESENT_INFO
    {
        public UInt32 idPresent;
        public byte btChannel;      // 礼品获取渠道
        public byte btTag;          // 礼品类型
        public UInt32 unPara1;
        public UInt32 unPara2;
        public UInt64 i64GetTime;     // 礼品获取时间
    }

    public class MSG_CLIENT_PRESENT_LST_EVENT : UnPacketBase
    {
        public UInt16 cnt;
        public PRESENT_INFO[] lst;

        protected override bool unpackBody()
        {
            cnt = br.ReadUInt16();
            lst = new PRESENT_INFO[cnt];
            for (int index = 0; index < cnt; index++)
            {
                lst[index].idPresent = br.ReadUInt32();
                lst[index].btChannel = br.ReadByte();
                lst[index].btTag = br.ReadByte();
                lst[index].unPara1 = br.ReadUInt32();
                lst[index].unPara2 = br.ReadUInt32();
                lst[index].i64GetTime = br.ReadUInt64(); 
            }
            return true;
        }
    }

    public struct PRESENT_CHANGE 
    {
        public UInt32  idPresent;
        public byte btTag;        // 0:新增 1:删除 2:无法领取
    }

    public class MSG_CLIENT_PRESENT_CHANGE_EVENT : UnPacketBase
    {
        public UInt16 cnt;
        public PRESENT_CHANGE[] lst;

        protected override bool unpackBody()
        {
            cnt = br.ReadUInt16();
            lst = new PRESENT_CHANGE[cnt];
            for (int index = 0; index < cnt; index++)
            {
                lst[index].idPresent = br.ReadUInt16();
                lst[index].btTag = br.ReadByte();
            }
            return true;
        }
    }

    // 领取礼品
    public class MSG_CLIENT_GET_PRESENT_REQUEST: PacketBase
    {
        public UInt32 idPresent; // 0:领取所有礼品  >0:领取某个礼品

        protected override bool packetBody()
        {
            bw.Write(idPresent);
            return true;
        }
    }

    public struct PRESENT_RES
    {
        public UInt32 idPresent;
        public byte res;// 0:成功领取    1:无法领取    2:暂时无法领取
    }

    public class MSG_CLIENT_GET_PRESENT_RESPONSE: UnPacketBase
    {
        public UInt16 cnt;
        public PRESENT_RES[] lst;
        
        protected override bool unpackBody()
        {
            cnt = br.ReadUInt16();
            lst = new PRESENT_RES[cnt];
            for (int index = 0; index < cnt; index++)
            {
                lst[index].idPresent = br.ReadUInt32();
                lst[index].res = br.ReadByte();
            }
            return true;
        }
    }
}