using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace DataCenter
{
    //客户端向登录服务器请求登录消息(CLIENT->L)
    public class MSG_CLIENT_LOGINT_2_LS:PacketBase
    {
        public UInt64 userid;
        public UInt32 version = 1;
        public string sessionKey = "000000000";
     
        public MSG_CLIENT_LOGINT_2_LS()
        {
            this.haveSequence = false;
        }

        protected override bool packetBody()
        {
            this._bw.Write(userid);
            this._bw.Write(version);
            this.addString(sessionKey,PACKET_LEN.SESSIONKEY_LENGTH);
            return true;
        }
    }

    public enum LOGIN_RESULT
    {
        SUCC,
        ERROR_GAMESERV,//游戏服务器故障
        ERROR_MAXCONN,//最大连接数
        ERROR_USERSERV,//用户服务器故障
        ERROR_DBSERV,//数据库服务器故障
        ERROR_DATA,//数据故障
        ERROR_USERDISABLE,//账号限制
        ERROR_IPDISABLE,//IP限制
        ERROR_VERDIASBLE,//版本限制
        ERROR_UNKNOWN //未知错误
    }

    public class MSG_CLIENT_LOGIN_LS_VALIDATE : UnPacketBase
    {
        public UInt16 port;
        public UInt64 userid;
        public string servip = "";
        public string checkCode = "";

        protected override bool unpackBody()
        {
            this.port = _br.ReadUInt16();
            this.userid = _br.ReadUInt64();
            this.servip = this.getString(PACKET_LEN.MAX_NAME);
            this.checkCode = this.getString(PACKET_LEN.VALIDATECODE_LENTH);
            return true;
        }
    }

    public class MSG_CLIENT_LOGIN_2_GS : PacketBase
    {
        public UInt64 userid;
        public string checkCode = "";
        public string sessionKey = "";
        public UInt32 version = 1;
        public UInt32 gameid = 1;
        public UInt32 clientSign = 1;
        public UInt32 clientSign2 = 1;
        int webFlag = 0;

        public MSG_CLIENT_LOGIN_2_GS()
        {
            this.haveSequence = false;
        }

        protected override bool packetBody()
        {
            _bw.Write(userid);
            this.addString(this.checkCode, PACKET_LEN.VALIDATECODE_LENTH);
            this.addString(this.sessionKey, PACKET_LEN.SESSIONKEY_LENGTH);
            _bw.Write(version);
            _bw.Write(gameid);
            _bw.Write(clientSign);
            _bw.Write(clientSign2);
            _bw.Write(webFlag);
            return true;
        }
    }

    public class MSG_SERVER_LOGIN_RESPONSE : UnPacketBase
    {
        public LOGIN_RESULT ret;
        protected override bool unpackBody()
        {
            int ret = (int)_br.ReadByte();
            this.ret = BaseLib.Tools.getEnumValue<LOGIN_RESULT>(ret, LOGIN_RESULT.ERROR_UNKNOWN);
            return true;
        }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MSG_CLIENT_USERINFO_REQUEST : PacketBase
    {
        public UInt64 userid;
        protected override bool packetBody()
        {
            _bw.Write(userid);
            return true;
        }
    }

    public class MSG_CLIENT_USERINFO_STAR : UnPacketBase
    {
    }

    public class MSG_CLIENT_USERINFO_FINISH : UnPacketBase
    {
    }

    public class MSG_CLIENT_CLIENTINITOVER_REQUEST : PacketBase
    {
    }

    //心跳包
    public class MSG_CLIENT_ACTIVE_REQUEST : PacketBase
    {
    }

    public class MSG_CLIENT_ACTIVE_RESPONSE : UnPacketBase
    {
        public UInt64 u64ServerTime;//服务器时间

        protected override bool unpackBody()
        {
            u64ServerTime = br.ReadUInt64();
            return true;
        }
    }

    //时间
    public class MSG_CLIENT_SERVER_TIME_EVENT : UnPacketBase
    {
        public UInt64 serverTime;

        protected override bool unpackBody()
        {
            serverTime = _br.ReadUInt64();
            return true;
        }
    }
}
