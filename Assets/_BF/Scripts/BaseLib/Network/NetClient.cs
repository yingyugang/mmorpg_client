using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace BaseLib
{
	class NetClient
	{
        TcpClient _client;

        public NetClient()
        {
            this.status = NET_STATE.NOCONNT;
        }

        public servInfo server { get; set; }
        public NET_STATE status  { get; set; }

        public void connect(servInfo newInfo)
        {
            if (this.isConn && this.server.equals(newInfo))
            {
                NetResult result = new NetResult();
                result.server = this.server;
                UnityEngine.Debug.Log(string.Format("connect server {0}:{1} SUCC",this.server.strIp,this.server.nPort.ToString()));
                EventSystem.sendEvent((int)DataCenter.EVENT_GLOBAL.net_connSucc, result);
                return;
            }
            if (this.status == NET_STATE.CONNECTING)
                return;
            this.server = newInfo;
            this.status = NET_STATE.CONNECTING;
            try
            {
                _client = new TcpClient();
                _client.BeginConnect(this.server.strIp, this.server.nPort, new AsyncCallback(this.OnConnect),this._client);
            }
            catch (Exception ex)
            {
                this.status = NET_STATE.CONNECTFAILED;
                UnityEngine.Debug.Log(ex.Message);
            }
        }

        void OnConnect(IAsyncResult ar)
        {
            NetResult result = new NetResult();
            result.server = this.server;
            try
            {
                TcpClient client = ar.AsyncState as TcpClient;
                client.EndConnect(ar);
                //连接成功
                result.strErrMsg = "SUCC";
                this.status = NET_STATE.ESTABLISHED;
                UnityEngine.Debug.Log(string.Format("connect server {0}:{1} SUCC", this.server.strIp, this.server.nPort.ToString()));
                EventSystem.sendEvent((int)DataCenter.EVENT_GLOBAL.net_connSucc, result);
            }
            catch (Exception e)
            {
                //连接失败
                this.status = NET_STATE.CONNECTFAILED;
                result.strErrMsg = e.Message;
                UnityEngine.Debug.Log(string.Format("connect server {0}:{1} Failed: {2}",this.server.strIp,this.server.nPort.ToString(),result.strErrMsg));
                EventSystem.sendEvent((int)DataCenter.EVENT_GLOBAL.net_connfailed, result);
            }
        }

        public bool isConn
        {
            get
            {
                if (this._client == null)
                    return false;
                if (this.status == NET_STATE.ESTABLISHED || this.status == NET_STATE.CONNECTING)
                    return true;
                return false;
            }
        }

        public bool canWrite
        {
            get
            {
                if (isConn && this._client.GetStream().CanWrite)
                    return true;
                return false;
            }
        }
        
        public bool canRead
        {
            get
            {
                if (isConn && this._client.GetStream().CanRead)
                    return true;
                return false;
            }
        }

        public void close()
        {
            if (this._client != null)
            {
                this._client.Close();
                this._client = null;
            }
            this.status = NET_STATE.CLOSED;
        }

        //以下为发送部分
        public bool isSending
        {
            get;
            set;
        }

        public bool sendData(byte[] data,uint dataLen)
        {
//             if (!this.canWrite)
//                 return false;
            try
            {
                this.isSending = true;
                this._client.GetStream().BeginWrite(data,0,(int)dataLen,OnSend,this._client.GetStream());
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
                return false;
            }
            return true;
        }

        void OnSend(IAsyncResult ar)
        {
            try
            {
                UnityEngine.Debug.Log("send Succ");
                this._client.GetStream().EndWrite(ar);
                this.isSending = false;
            }
            catch (System.Exception ex)
            {
            	//断开
                onDisconnect();
                UnityEngine.Debug.Log(ex.Message);
            }
        }

        //以下为读取部分
        const int LEN_PACKLEN = 2;
        const int LEN_MSGID = 2;
        RecvPacket _recvPacket = null;
        //接收
        public void recvData()
        {
            if (_recvPacket != null)
                return;
            if (!this.canRead)
                return;
            _recvPacket = new RecvPacket();
            readPack(LEN_PACKLEN);
        }

        void readPack(int nLen)
        {
            try
            {
                this._client.GetStream().BeginRead(_recvPacket.data, _recvPacket.nOffset, nLen, onRecv, this._client.GetStream());
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
                this._recvPacket = null;
            }
        }

        void onRecv(IAsyncResult ar)
        {
            try
            {
                NetworkStream stream = (NetworkStream)ar.AsyncState;
                int nRecvLen = stream.EndRead(ar);
                if (nRecvLen == 0 && this._recvPacket.nOffset==0)
                {
                    //断开
                    onDisconnect();
                    return;
                }
                this._recvPacket.nOffset += nRecvLen;
                if (this._recvPacket.nOffset == LEN_PACKLEN)
                {
                    //报文长度
                    _recvPacket.nLen = BitConverter.ToUInt16(_recvPacket.data, 0);
                    //消息
                    UnityEngine.Debug.Log("recv Packet len:" + _recvPacket.nLen);
                    readPack(LEN_MSGID);
                }
                else if(this._recvPacket.nOffset == LEN_PACKLEN+LEN_MSGID)//消息类型
                {
                    _recvPacket.nMsgId = BitConverter.ToUInt16(_recvPacket.data, LEN_PACKLEN);
                    UnityEngine.Debug.Log("recvMsg id:" + _recvPacket.nMsgId);

                    int nBodyLen = _recvPacket.nLen - LEN_PACKLEN - LEN_MSGID;
                    if (nBodyLen > 0)
                        readPack(nBodyLen);
                    else //接收完毕，加入接收队列
                    {
                        NetworkMgr.recvData(this._recvPacket, this.server.nType);
                        this._recvPacket = null;
                    }
                }
                else if (this._recvPacket.nOffset > LEN_PACKLEN + LEN_MSGID) //包体
                {
                    if (this._recvPacket.nLen > this._recvPacket.nOffset)
                        readPack(this._recvPacket.nLen - this._recvPacket.nOffset);
                    else //接收完毕，加入接收队列
                    {
                        //插入接收队列
                        NetworkMgr.recvData(this._recvPacket,this.server.nType);
                        this._recvPacket = null;
                    }
                }
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
                onDisconnect();
            }
        }

        void onDisconnect()
        {
            UnityEngine.Debug.Log("server:" + this.server.strIp + ":" + this.server.nPort + "  Disconnect");
            this._recvPacket = null;
            this.status = NET_STATE.CLOSED;
            EventSystem.sendEvent((int)DataCenter.EVENT_GLOBAL.net_disconnt, this.server);
        }
	}
}
