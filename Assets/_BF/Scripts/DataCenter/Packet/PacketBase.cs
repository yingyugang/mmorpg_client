using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using BaseLib;

namespace DataCenter
{
    public enum PACKET_LEN
    {
        MAX_MSGSIZE = 1024, // 消息包长度 
        SESSIONKEY_LENGTH = 128,
        VALIDATECODE_LENTH = 25, //校验码长度
        GAMESERVERNAME_LENGTH = 32,
        ACCOUNTS_LENGTH = 32,
        PASSWORD_LENGTH = 32,
        MAX_NAME = 32,
        MAX_IP_LEN = 16,
        CHAT_MESSAGE = 64, //聊天最大长度
        MAX_MEMO = 256, //文本框的长度
    }

    public abstract class PacketBase : IPacket
    {
        protected bool haveSequence = true;
        protected uint sequenceOffset = 4;

        //消息类型
        MSG_TYPE msgtype;
        MemoryStream _ms = new MemoryStream();
        protected BinaryWriter _bw = null;
        protected BinaryWriter bw { get { return _bw; } }

        public int msgid 
        { 
            get{return (int)msgtype;} 
            set{
                this.msgtype = Tools.getEnumValue<MSG_TYPE>(value, MSG_TYPE._MSG_CLIENT_ERROR);
            }
        }


        public PacketBase()
        {
        }

        //temp
        public object pack(ref byte[] bt)
        {
            return null;
        }

        public bool pack()
        {
            try
            {
                _bw = new BinaryWriter(_ms);
                this.msgtype = Tools.getEnumValue<MSG_TYPE>(PacketMgr.me.getPacketId(this), MSG_TYPE._MSG_CLIENT_ERROR);
                if (this.msgtype == MSG_TYPE._MSG_CLIENT_ERROR)
                    return false;
                packetHead();
                packetBody();
                packetTail();
                return true;
            }
            catch(Exception ex)
            {
                UnityEngine.Debug.Log(string.Format("pack error:{0}:{1}",this.msgtype,ex.Message));
                return false;
            }
        }

        public byte[] data
        {
            get { return this._ms.GetBuffer();}
        }

        public UInt32 length { get; set; }

        protected virtual bool packetBody()
        {
            return true;
        }

        //报文头
        void packetHead()
        {
            _bw.Write((ushort)0);
            _bw.Write((ushort)msgtype);
            //预留Sequence位置
            if (this.haveSequence)
                _bw.Write((UInt32)0);
        }

        public void addSequence()
        {
            if (this.haveSequence)
            {
                this._ms.Position = sequenceOffset;
                UInt32 seq = PacketMgr.me.getSequence();
                _bw.Write(seq);
            }
        }

        public void release()
        {
            _bw.Close();
            _ms.Close();
        }
        //添加报文长度字段
        void packetTail()
        {
            this.length = (ushort)_ms.Position;
            _ms.Position = 0;
            _bw.Write((ushort)this.length);
            _ms.Capacity  = (int)this.length;
        }

        protected void addString(string value, PACKET_LEN len)
        {
            this.addString(value, (uint)len);
        }

        protected void addString(string value, uint len)
        {
            if (value == null)
                return;
            if(value.Length >=len)
                _bw.Write(value.ToCharArray(0,(int)len),0,(int)len);
            else
            {
                _bw.Write(value.ToCharArray(),0,(int)value.Length);
                //补0
                for (int index = value.Length; index < len; index++)
                    _bw.Write((byte)0);
            }
        }
    }
}
