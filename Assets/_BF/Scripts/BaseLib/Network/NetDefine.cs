using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLib
{
    struct servInfo
    {
        public string strIp;
        public int nPort;
        public SEVER_TYPE nType;
        public bool autoReconn;//自动重连接

        public bool equals(servInfo other)
        {
            if (this.strIp == other.strIp && this.nPort == other.nPort)
                return true;
            return false;
        }
    }

    public class RecvPacket
    {
        public int nLen;
        public int nMsgId;
        public byte[] data;
        public int nOffset;
        const int MAX_LEN = 1024;

        public RecvPacket()
        {
            data = new byte[MAX_LEN];
            nOffset = 0;
        }
    }

    struct NetResult
    {
        public servInfo server;
        public string strErrMsg;
    }

    public enum NET_STATE
    {
        NOCONNT,
        CONNECTING,
        CONNECTFAILED,
        ESTABLISHED,
        CLOSED
    }
}
