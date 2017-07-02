using System;
using System.Collections.Generic;
using BaseLib;

namespace DataCenter
{
    //脱机模式
    class DataOffline : Singleton<DataOffline>
    {
        public UInt64 iggid;

        Dictionary<MSG_TYPE, List<UnPacketBase>> _msgList = new Dictionary<MSG_TYPE, List<UnPacketBase>>();
        private DataOffline()
        {
            BaseLib.EventSystem.register((int)DataCenter.EVENT_GLOBAL.net_sendData,sendData);
        }

        public void init()
        {
            initLogin();
        }

        void sendData(int nEvent, object param)
        {
            IPacket packet = (IPacket)param;
            if (packet == null)
                return;
            packet.pack();
            UnityEngine.Debug.Log(string.Format("send pack:{0}:{1}", PacketMgr.me.getPacketId(packet), packet.length));

            List<UnPacketBase> outPacket;
            MSG_TYPE msgtype = Tools.getEnumValue<MSG_TYPE>(packet.msgid, MSG_TYPE._MSG_CLIENT_ERROR);
            if (_msgList.TryGetValue(msgtype, out outPacket))
            {
                if (outPacket != null)
                {
                    foreach (UnPacketBase item in outPacket)
                    {
                        item.initMsgId();
                        EventSystem.sendEvent((int)item.msgid, item, (int)DataCenter.EVENT_GROUP.packet);
                    }
                }
            }
        }

        void addPacket(MSG_TYPE type, UnPacketBase data)
        {
            List<UnPacketBase> listData;
            if (this._msgList.TryGetValue(type, out listData))
            {
                listData.Add(data);
            }
            else
            {
                listData = new List<UnPacketBase>();
                this._msgList[type] = listData;
            }
            if (listData != null)
                listData.Add(data);
        }

        void initLogin()
        {
            MSG_CLIENT_LOGIN_LS_VALIDATE msg1 = new MSG_CLIENT_LOGIN_LS_VALIDATE();
            msg1.userid = this.iggid;
            addPacket(MSG_TYPE._MSG_CLIENT_LOGIN_2_LS, msg1);
            
            MSG_SERVER_LOGIN_RESPONSE msg2 = new MSG_SERVER_LOGIN_RESPONSE();
            msg2.ret = LOGIN_RESULT.SUCC;
            addPacket(MSG_TYPE._MSG_CLIENT_LOGIN_2_GS, msg2);

            //登陆后消息
            //角色信息
            MSG_CLIENT_USER_INFO_EVENT user = new MSG_CLIENT_USER_INFO_EVENT();
            user.wLevel = 30;
            addPacket(MSG_TYPE._MSG_CLIENT_USERINFO_REQUEST, user);
            
            //英雄列表
            MSG_CLIENT_HERO_LST_EVENT heroList = new MSG_CLIENT_HERO_LST_EVENT();
            addPacket(MSG_TYPE._MSG_CLIENT_USERINFO_REQUEST, user);

            //道具列表
            MSG_ITEM_LIST_EVENT itemList = new MSG_ITEM_LIST_EVENT();
            addPacket(MSG_TYPE._MSG_CLIENT_USERINFO_REQUEST, user);
            
            //登陆结束
            addPacket(MSG_TYPE._MSG_CLIENT_USERINFO_REQUEST, new MSG_CLIENT_USERINFO_FINISH());
        }
    }
}
