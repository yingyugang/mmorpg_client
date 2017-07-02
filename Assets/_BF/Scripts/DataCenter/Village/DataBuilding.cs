using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;

namespace DataCenter
{
    class DataBuilding : DataModule
	{
        private Dictionary<BUILD_TYPE, buildingInfo> _buildList = new Dictionary<BUILD_TYPE, buildingInfo>();

        public override bool init()
        {
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_BUILDING_LIST, onRecvBuildingList, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_BUILDING_UPDATE, onBuildingUpdate, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_BUILDING_LEVY, onBuildingLevy, (int)DataCenter.EVENT_GROUP.packet);
            return true;
        }

        public override void release()
        {

        }

        void onRecvBuildingList(int nEvent, System.Object param)
        {
            MSG_BUILDING_LIST_EVENT msg = (MSG_BUILDING_LIST_EVENT)param;
 //           _buildList.Clear();
            foreach (BUILDING_INFO item in msg.lst)
            {
                buildingInfo newObj = new buildingInfo();
                if (newObj.init(item))
                    _buildList[newObj.type] = newObj;
            }
        }

        void onBuildingUpdate(int nEvent, System.Object param)
        {
            MSG_BUILDING_UPDATE_EVENT msg = (MSG_BUILDING_UPDATE_EVENT)param;
			BUILD_TYPE type = buildingInfo.getType(msg.idBuildingType);

            buildingInfo build = this.getBuilding(type);
            if (build == null)
                return;
            build.update(msg);

            EventSystem.sendEvent((int)EVENT_MAINUI.buildUpdate, type, (int)EVENT_GROUP.mainUI);
        }

        //征收
        public void buildingLevy(uint buildid,int count)
        {
            MSG_BUILDING_LEVY_REQUEST msg = new MSG_BUILDING_LEVY_REQUEST();
            msg.idBuilding = buildid;
            msg.count = (byte)count;
            NetworkMgr.sendData(msg);
        }

        void onBuildingLevy(int nEvent,System.Object param)
        {
            MSG_BUILDING_LEVY_RESPONSE msg = (MSG_BUILDING_LEVY_RESPONSE)param;
            EventSystem.sendEvent((int)EVENT_MAINUI.buildLevy, param, (int)EVENT_GROUP.mainUI);
        }

        public buildingInfo getBuilding(BUILD_TYPE type)
        {
            if (_buildList.ContainsKey(type))
                return _buildList[type];
            return null;
        }

        //建筑升级
        public void updateBuilding(uint id, uint soulCount)
        {
            MSG_BUILDING_UPLEV_REQUEST msg = new MSG_BUILDING_UPLEV_REQUEST();
            msg.idBuilding = id;
            msg.unCostSoul = soulCount;
            NetworkMgr.sendData(msg);
        }
	}
}
