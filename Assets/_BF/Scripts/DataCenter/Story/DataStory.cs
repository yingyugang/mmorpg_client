using UnityEngine;
using System.Collections.Generic;
using BaseLib;
using System;

namespace DataCenter
{
    public class DataStory : DataModule
    {
        List<UInt32> _batttleList = new List<uint>();

        public override void release()
        {
        }

        public override bool init()
        {
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_STORY_LIST, onRecvList, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_STORY_UPDAE, onUpdate, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_STORY_ACTIVATE, onActivate, (int)DataCenter.EVENT_GROUP.packet);
            return true;
        }

        void onRecvList(int nEvent, System.Object param)
        {
 //           _batttleList.Clear();

            MSG_CLIENT_STORY_LIST msg = (MSG_CLIENT_STORY_LIST)param;
            foreach(STORY_INFO item in msg.lst)
            {
                _batttleList.Add(item.idBattle);
            }
        }

        public bool isFinish(UInt32 idBattle)
        {
            foreach (UInt32 item in this._batttleList)
            {
                if (item == idBattle)
                    return true;
            }
            return false;
        }

        void onUpdate(int nEvent, System.Object param)
        {
            MSG_CLIENT_STORY_UPDAE msg = (MSG_CLIENT_STORY_UPDAE)param;
            _batttleList.Add(msg.stInfo.idBattle);
            EventSystem.sendEvent((int)EVENT_MAINUI.storyUpdate, 0, (int)EVENT_GROUP.mainUI);
        }

        public void activateStory(UInt32 idBattle)
        {
            MSG_CLIENT_STORY_ACTIVATE_REQUEST request = new MSG_CLIENT_STORY_ACTIVATE_REQUEST();
            request.idBattle = idBattle;
            NetworkMgr.sendData(request);
        }

        void onActivate(int nEvent, System.Object param)
        {
            MSG_CLIENT_STORY_ACTIVATE_RESPONSE msg = (MSG_CLIENT_STORY_ACTIVATE_RESPONSE)param;
            EventSystem.sendEvent((int)EVENT_MAINUI.storyActivaty, msg.idBattle, (int)EVENT_GROUP.mainUI);                
        }
    }
}
