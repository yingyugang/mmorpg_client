using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BaseLib;
using System;

namespace DataCenter
{
    class DataChat : DataModule
    {
        Dictionary<UInt64, List<chatInfo>> _chatList = new Dictionary<ulong, List<chatInfo>>();

        public override void release()
        {
        }

        public override bool init()
        {
            _chatList.Clear();
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_CHAT_REQUEST, onSendChat, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_CHAT_INFO, onRecvChat, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_GET_WORLDCHAT, onWorldChat, (int)DataCenter.EVENT_GROUP.packet);

            return true;
        }

        public void sendChat(CHAT_TYPE chatType,UInt64 target,string content)
        {
            MSG_CLIENT_CHAT_REQUEST request = new MSG_CLIENT_CHAT_REQUEST();
            request.userid = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.id;
            request.chatType = chatType;
            request.chatContent = content;

            NetworkMgr.sendData(request);
            //
        }

        public List<chatInfo> getUserChatList(UInt64 usefid)
        {
            if(!this._chatList.ContainsKey(usefid))
               _chatList[usefid] = new List<chatInfo>();
            return _chatList[usefid];
        }

        void onSendChat(int nEvent, System.Object param)
        {
            MSG_CLIENT_CHAT_RESPONSE msg = (MSG_CLIENT_CHAT_RESPONSE)param;
            
        }

        void onRecvChat(int nEvent, System.Object param)
        {
            MSG_CLIENT_CHAT_INFO msg = (MSG_CLIENT_CHAT_INFO)param;
            if (msg == null)
                return;
            
            chatInfo info = new chatInfo();
            info.init(msg);
            getUserChatList(msg.targetid).Add(info);
            EventSystem.sendEvent((int)EVENT_MAINUI.chatRecvMsg,info,(int)EVENT_GROUP.mainUI);
        }

        public void getWorldChat()
        {
            MSG_CLIENT_GET_WORLDCHAT_REQUEST request = new MSG_CLIENT_GET_WORLDCHAT_REQUEST();
            NetworkMgr.sendData(request);
        }

        void onWorldChat(int nEvent, System.Object param)
        {
            MSG_CLIENT_GET_WORLDCHAT_RESPONSE msg = (MSG_CLIENT_GET_WORLDCHAT_RESPONSE)param;
            if (msg == null)
                return;
            if(msg.cnt==0)
                return;
            List<chatInfo> chatList = getUserChatList(0);
            foreach(WORLDCHAT_TMP info in msg.lst)
            {
                chatInfo item = new chatInfo();
                item.init(info);
                chatList.Add(item);
            }
            EventSystem.sendEvent((int)EVENT_MAINUI.chatRecvWorld,null,(int)EVENT_GROUP.mainUI);
        }
    }
}
