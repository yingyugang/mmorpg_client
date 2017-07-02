using System;
using System.IO;
using System.Runtime.InteropServices;
using BaseLib;

namespace DataCenter
{
    public struct STORY_INFO
    {
        public UInt32 idBattle;
    }

    public class MSG_CLIENT_STORY_LIST : UnPacketBase
    {
        public UInt16 usCnt;
        public STORY_INFO[] lst;

        protected override bool unpackBody()
        {
            usCnt = br.ReadUInt16();
            lst = new STORY_INFO[usCnt];
            for (int n = 0; n < usCnt; n++)
            {
                lst[n].idBattle = br.ReadUInt32();
            }
            return true;
        }
    }

    public class MSG_CLIENT_STORY_UPDAE : UnPacketBase
    {
        public STORY_INFO stInfo;
        protected override bool unpackBody()
        {
            stInfo.idBattle = br.ReadUInt32();
            return true;
        }
    }

    public class MSG_CLIENT_STORY_ACTIVATE_REQUEST : PacketBase
    {
        public UInt32 idBattle;//战役ID

        protected override bool packetBody()
        {
            bw.Write(idBattle);
            return true;
        }
    }

    public class MSG_CLIENT_STORY_ACTIVATE_RESPONSE : UnPacketBase
    {
        public UInt32 idBattle;//战役ID
        public UInt32 errcode;//战役ID
        
        protected override bool unpackBody()
        {
            idBattle = br.ReadUInt32();
            errcode = br.ReadUInt32();
            return true;
        }
    }
}
