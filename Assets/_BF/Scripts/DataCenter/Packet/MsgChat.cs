using System;
using System.IO;
using System.Runtime.InteropServices;
using BaseLib;

namespace DataCenter
{
    public enum CHAT_TYPE
    {
        CHAT_WORLD = 0,
        CHAT_USER = 1,
    }

    public class MSG_CLIENT_CHAT_REQUEST : PacketBase
    {
        public UInt64 userid;
        public UInt64 targetid;
        public CHAT_TYPE chatType;
        public string chatContent;

        protected override bool packetBody()
        {
            bw.Write(userid);
            bw.Write(targetid);
            bw.Write((byte)chatType);
            this.addString(chatContent,PACKET_LEN.CHAT_MESSAGE);
            return true;
        }
    }

    public class MSG_CLIENT_CHAT_RESPONSE : UnPacketBase
    {
        public UInt32 errcode;
        protected override bool unpackBody()
        {
            errcode = br.ReadUInt32();
            return true;
        }
    }

    public class MSG_CLIENT_CHAT_INFO : UnPacketBase
    {
        public UInt64 userid;
        public UInt64 targetid;
        public byte type;
        public byte servid;
        public UInt16 level;
        public UInt16 arenaRank;
        public UInt32 heroType;
        public UInt16 heroLevel;
        public string strName;
        public string strContent;
                
        protected override bool unpackBody()
        {
            userid = br.ReadUInt64();
            targetid = br.ReadUInt64();
            type = br.ReadByte();
            servid = br.ReadByte();
            level = br.ReadUInt16();
            arenaRank = br.ReadUInt16();
            heroType = br.ReadUInt32();
            heroLevel = br.ReadUInt16();
            strName = this.getString(PACKET_LEN.MAX_NAME);
            strContent = this.getString(PACKET_LEN.CHAT_MESSAGE);
            return true;
        }
    }

    public class MSG_CLIENT_GET_WORLDCHAT_REQUEST : PacketBase
    {
    }

    public struct WORLDCHAT_TMP
    {
        public UInt64 userid;
        public UInt32 chatTime;
        public UInt16 userlevel;
        public UInt16 arenaRank;
        public UInt32 heroType;
        public UInt16 heroLevel;
        public string strName;
        public string strContent;
    }

    public class MSG_CLIENT_GET_WORLDCHAT_RESPONSE : UnPacketBase
    {
        public UInt16 cnt;
        public WORLDCHAT_TMP[] lst;

        protected override bool unpackBody()
        {
            cnt = br.ReadUInt16();
            lst = new WORLDCHAT_TMP[cnt];

            for (int n = 0; n< cnt;n++ )
            {
                lst[n].userid = br.ReadUInt64();
                lst[n].chatTime = br.ReadUInt32();
                lst[n].userlevel = br.ReadUInt16();
                lst[n].arenaRank = br.ReadUInt16();
                lst[n].heroType = br.ReadUInt32();
                lst[n].heroLevel = br.ReadUInt16();
                lst[n].strName = this.getString(PACKET_LEN.MAX_NAME);
                lst[n].strContent = this.getString(PACKET_LEN.CHAT_MESSAGE);
            }
            return true;
        }
    }
}
