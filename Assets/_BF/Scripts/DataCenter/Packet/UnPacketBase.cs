using System;
using System.Collections.Generic;
using BaseLib;
using System.IO;

namespace DataCenter
{
    public class UnPacketBase:IUnPacket
    {
        //流水号
        protected uint _sequence;

        public UInt32 sequence { get { return this._sequence; } }
        public int msgid {get;set;}
        
        MemoryStream _ms = null;
        protected BinaryReader _br = null;
        protected BinaryReader br { get { return _br; } }
        
        public UnPacketBase()
        {
        }

        public bool unpack(byte[] msg,int len)
        {
            try
            {
                _ms = new MemoryStream(MSG.Truncate(msg));
                _br = new BinaryReader(_ms);
                //长度
                _br.ReadUInt16();
                //类型
                _br.ReadUInt16();

                bool ret = unpackBody();
                _br.Close();
                _ms.Close();
                return ret;
            }

            catch(Exception ex)
            {
                UnityEngine.Debug.Log("Unpack Error:" + ex.Message);
                return false;
            }
        }

        protected virtual bool unpackBody()
        {
            return true;
        }

        protected string getString(PACKET_LEN len)
        {
            return getString((int)len);
        }

        protected string getString(int len)
        {
            byte[] data = _br.ReadBytes(len);
            int realLen = 0;
            while (realLen < len)
            {
                if (data[realLen] == 0)
                    break;
                else
                    realLen++;
            }
            return MSG.HexStringToString(BitConverter.ToString(data,0,realLen));
        }

        public void release()
        {
        }

        public void initMsgId()
        {
            this.msgid = UnPacketMgr.me.getPacketId(this);
        }

        public bool isSucc(int code)
        {
            if (code == 100000)
                return true;
            UnityEngine.Debug.LogError("errcode = " + code);
            return false;
        }
        
        public bool isSucc(uint code)
        {
            if (code == 100000)
                return true;
            UnityEngine.Debug.LogError("errcode = " + code);
            return false;
        }

        public string getErrMsg(int code)
        {
            return BaseLib.LanguageMgr.getString(code);
        }
    }
}
