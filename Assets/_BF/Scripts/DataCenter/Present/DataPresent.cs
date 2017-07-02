using UnityEngine;
using System.Collections.Generic;
using BaseLib;
using System;

namespace DataCenter
{
    class DataPresent : DataModule
    {
        Dictionary<uint, PresentInfo> _presentList = new Dictionary<uint, PresentInfo>();

        public override bool init()
        {
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_PRESENT_LST, onRecvList, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_GET_PRESENT, onGetPresent, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_PRESENT_CHANGE_EVENT, onUpdatePresent, (int)DataCenter.EVENT_GROUP.packet);
            return true;
        }

        public override void release()
        {

        }

        void onRecvList(int nEvent, System.Object param)
        {
            _presentList.Clear();
            MSG_CLIENT_PRESENT_LST_EVENT msg = (MSG_CLIENT_PRESENT_LST_EVENT)param;
            if (msg == null)
                return;
            foreach (PRESENT_INFO item in msg.lst)
            {
                PresentInfo info = new PresentInfo();
                info.init(item);
                _presentList[info.id] = info;
            }
            EventSystem.sendEvent((int)EVENT_MAINUI.presentRecvList, 0, (int)EVENT_GROUP.mainUI);
        }

        public PresentInfo[] getPresentList()
        {
            List<PresentInfo> theList = new List<PresentInfo>();
            foreach (KeyValuePair<uint, PresentInfo> item in this._presentList)
                theList.Add(item.Value);
            return theList.ToArray();
        }

        public PresentInfo getPresentInfo(uint id)
        {
            if(this._presentList.ContainsKey(id))
                return _presentList[id];
            return null;
        }

        public void getPresent(UInt32 id)// 0:领取所有礼品  >0:领取某个礼品
        {
            MSG_CLIENT_GET_PRESENT_REQUEST request = new MSG_CLIENT_GET_PRESENT_REQUEST();
            request.idPresent = id;
            NetworkMgr.sendData(request);
        }

        void onGetPresent(int nEvent, System.Object param)
        {
            MSG_CLIENT_GET_PRESENT_RESPONSE msg = (MSG_CLIENT_GET_PRESENT_RESPONSE)param;
            foreach (PRESENT_RES item in msg.lst)
            {
                if (item.res == 0)//领取成功
                {
                    EventSystem.sendEvent((int)EVENT_MAINUI.presentCantGet,0, (int)EVENT_GROUP.mainUI);

                    this._presentList.Remove(item.idPresent);
                }
                else//领取失败
                {

                }
            }

            EventSystem.sendEvent((int)EVENT_MAINUI.presentGetSucc, 0, (int)EVENT_GROUP.mainUI);

            DataManager.getModule<DataPost>(DATA_MODULE.Data_Post).ShowPost();
        }

        void onUpdatePresent(int nEvent, System.Object param)
        {
            MSG_CLIENT_PRESENT_CHANGE_EVENT msg = (MSG_CLIENT_PRESENT_CHANGE_EVENT)param;
            foreach (PRESENT_CHANGE item in msg.lst)
            {
                if (item.btTag == 0)//新增(暂时服务端未提供)
                {
                }
                else if(item.btTag == 1)//删除
                {
                    this._presentList.Remove(item.idPresent);
                }
            }
            EventSystem.sendEvent((int)EVENT_MAINUI.presentUpdate, 0, (int)EVENT_GROUP.mainUI);


            DataManager.getModule<DataPost>(DATA_MODULE.Data_Post).ShowPost();
        }



    
    }
}
