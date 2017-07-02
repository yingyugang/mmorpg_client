using UnityEngine;
using System.Collections;
using BaseLib;

namespace DataCenter
{
    public class DataUser : DataModule
    {
        UserInfo _mainUser = new UserInfo();
        float sendActiveTime = 0;

        public bool isLogin { get; set; }
         
        public DataUser()
        {
            isLogin = false;
        }

        public UserInfo mainUser { get { return this._mainUser; } }

        public override bool init()
        { 
            this._mainUser.initTest();

            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_USER_INFO_EVENT, onRecvUserInfo, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_USER_ATTR_EVENT, onRecvUserAtrr, (int)DataCenter.EVENT_GROUP.packet);
            isLogin = false;
            return true;
        }

        public override void release()
        {
            isLogin = false;
        }

        //发送心跳包
        public void sendActivePack()
        {
            if (!isLogin || Time.time - sendActiveTime < 60) //60秒发送一次
                return;
            MSG_CLIENT_ACTIVE_REQUEST request = new MSG_CLIENT_ACTIVE_REQUEST();
            NetworkMgr.sendData(request);
            sendActiveTime = Time.time;
        }

        //用户属性变更
        void onRecvUserAtrr(int nEvent, System.Object param)
        {
            MSG_CLIENT_USER_ATTR_EVENT msg = (MSG_CLIENT_USER_ATTR_EVENT)param;
            foreach (MSG_CLIENT_USER_ATTR_INFO it in msg.lst)
            {
                USER_ATTR type = Tools.getEnumValue<USER_ATTR>((int)it.wUserAttrIndex, USER_ATTR.USER_ATTR_INVALID);
                switch (type)
                {
                    case USER_ATTR.USER_ATTR_LEVEL:
                        mainUser.setLevel(it.dwUserAttrData);
                        break;
                    case USER_ATTR.USER_ATTR_EXP:
                        mainUser.curexp = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_POWER:
                        mainUser.curpower = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_VIP:
                        break;
                    case USER_ATTR.USER_ATTR_COIN:
                        mainUser.goldCoin = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_SOUL:
                        mainUser.soul = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_DIAMOND:
                        mainUser.diamond = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_ARENA_DIAMOND:
                        mainUser.arenaPoint = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_ARENA_POINT:
                        mainUser.setArenaExp(it.dwUserAttrData);
                        break;
                    case USER_ATTR.USER_ATTR_GAY_POINT:
                        mainUser.friendPt = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_KEY_GOLD:
                        mainUser.keyCoin = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_KEY_SILVER:
                        mainUser.keyExp = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_FIGHT_TROOP:
                        mainUser.fightTroop = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_MAX_HERO:
                        mainUser.maxHero = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_MAX_ITEM:
                        mainUser.maxItem = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_POWER_RECOVER_TIME:
                        mainUser.PowerRecoverTime = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_ARENA_RECOVER_TIME:
                        mainUser.ArenaRecoverTime = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_LOGINED_TIME:
                        mainUser.LoginedTime = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_ACTION_TIME:
                        mainUser.ActionTime = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_GIFT_TYPEID1:
                        mainUser.idGiftType1 = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_GIFT_TYPEID2:
                        mainUser.idGiftType2 = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_GIFT_TYPEID3:
                        mainUser.idGiftType3 = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_USE_ITEM_ID1:
                        onSetItemType(0, it.dwUserAttrData);
                        break;
                    case USER_ATTR.USER_ATTR_USE_ITEM_AMOUNT1:
                        onSetItemCount(0, it.dwUserAttrData);
                        break;
                    case USER_ATTR.USER_ATTR_USE_ITEM_ID2:
                        onSetItemType(1, it.dwUserAttrData);
                        break;
                    case USER_ATTR.USER_ATTR_USE_ITEM_AMOUNT2:
                        onSetItemCount(1,it.dwUserAttrData);
                        break;
                    case USER_ATTR.USER_ATTR_USE_ITEM_ID3:
                        onSetItemType(2, it.dwUserAttrData);
                        break;
                    case USER_ATTR.USER_ATTR_USE_ITEM_AMOUNT3:
                        onSetItemCount(2, it.dwUserAttrData);
                        break;
                    case USER_ATTR.USER_ATTR_USE_ITEM_ID4:
                        onSetItemType(3, it.dwUserAttrData);
                        break;
                    case USER_ATTR.USER_ATTR_USE_ITEM_AMOUNT4:
                        onSetItemCount(3, it.dwUserAttrData);
                        break;
                    case USER_ATTR.USER_ATTR_USE_ITEM_ID5:
                        onSetItemType(4,it.dwUserAttrData);
                        break;
                    case USER_ATTR.USER_ATTR_USE_ITEM_AMOUNT5:
                        onSetItemCount(4,it.dwUserAttrData);
                        break;
                    case USER_ATTR.USER_ATTR_LAST_GET_KEY_TIME:
                        mainUser.LastGetKeyTime = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_LAST_GET_PRESENT_TIME:
                        mainUser.lastGetPresentTime = it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_PRESENT_SEQ:
                        mainUser.presentSeq = (int)it.dwUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_ARENA_TROOP:
                        mainUser.idArenaTroop = (int)it.dwUserAttrData;
                        break;
                }
                EventSystem.sendEvent((int)EVENT_MAINUI.userUpdate, type, (int)EVENT_GROUP.mainUI);
            }
        }

        void onSetItemType(uint index,uint typeid)
        {
            DataItem itemdata = DataManager.getModule<DataItem>(DATA_MODULE.Data_Item);
            if (itemdata != null)
            {
                ItemInfo item = itemdata.getBattleItem(index);
                if (item != null)
                    itemdata.recvBattleItem(index, typeid, item.amount);
            }
        }

        void onSetItemCount(uint index, uint count)
        {
            DataItem itemdata = DataManager.getModule<DataItem>(DATA_MODULE.Data_Item);
            if (itemdata != null)
            {
                ItemInfo item = itemdata.getBattleItem(index);
                if(item!=null)
                    item.amount = count;
            }
        }

        //用户信息初始化
        void onRecvUserInfo(int nEvent, System.Object param)
        {
            MSG_CLIENT_USER_INFO_EVENT msg = (MSG_CLIENT_USER_INFO_EVENT)param;
            _mainUser.init(msg);
            DataItem itemdata = DataManager.getModule<DataItem>(DATA_MODULE.Data_Item);
            if (itemdata != null)
            {
                itemdata.recvBattleItem(0,msg.idUseItemType1,msg.unUseItemAmount1);
                itemdata.recvBattleItem(1, msg.idUseItemType2, msg.unUseItemAmount2);
                itemdata.recvBattleItem(2, msg.idUseItemType3, msg.unUseItemAmount3);
                itemdata.recvBattleItem(3, msg.idUseItemType4, msg.unUseItemAmount4);
                itemdata.recvBattleItem(4, msg.idUseItemType5, msg.unUseItemAmount5);
            }
            isLogin = true;
        }	

		public void BuyPower()
		{
			MSG_BUY_POWER_REQUEST msg = new MSG_BUY_POWER_REQUEST();
			NetworkMgr.sendData(msg);
			Debug.Log("onBuyPower");
		}

        public void getKey()
        {
            MSG_CLIENT_GET_KEY_REQUEST msg = new MSG_CLIENT_GET_KEY_REQUEST();
            NetworkMgr.sendData(msg);
        }
    }
}