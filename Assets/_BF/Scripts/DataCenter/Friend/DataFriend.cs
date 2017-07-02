using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BaseLib;
using System;

namespace DataCenter
{
    public class DataFriend : DataModule
    {
        public DataFriend()
        {
        }

        public override void release()
        {
        }

        Dictionary<UInt64, FriendInfo> _friendList = new Dictionary<UInt64, FriendInfo>();
        Dictionary<UInt32, FriendGift> _giftList = new Dictionary<uint, FriendGift>();

        public override bool init()
        {
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_FRIEND_LIST, onRecvFriendList, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_FRIEND_UPDATE, onFriendUpdate, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_FRIEND_APPLY, onApply, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_FRIEND_REFUSE, onRefuse, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_FRIEND_ACCEPT, onAccept, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_FRIEND_DEL, onDelFriend, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_SEARCH_USER, onSearchFriend, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_FRIEND_GIVE_GIFT, onGivegift, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_FRIEND_AWARD_GIFT, onAwardGift, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_FRIEND_GIFT_LIST, onRecvGiftList, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_FRIEND_GIFT_DEL, onGiftDel, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_FRIEND_GIFT_SET, onSetGift, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_FRIEND_CANCEL_APPLY, onCancelApply, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_FRIEND_COLLECT, onCollect, (int)DataCenter.EVENT_GROUP.packet);


            InitGiftType();
            return true;
        }

        public void initFriendList()
        {
            MSG_CLIENT_FRIEND_LIST_REQUEST request = new MSG_CLIENT_FRIEND_LIST_REQUEST();
            request.idUser = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.id;
            NetworkMgr.sendData(request);
        }

        void onRecvFriendList(int nEvent, System.Object param)
        {
            MSG_CLIENT_FRIEND_LIST msg = (MSG_CLIENT_FRIEND_LIST)param;
            if((msg.un8Flag &0x01) == 0x1)
                _friendList.Clear();
            foreach (FRIEND_INFO_TMP item in msg.lst)
            {
                FriendInfo friend = new FriendInfo();
                friend.init(item);
                _friendList[friend.userid] = friend;
            }
            if ((msg.un8Flag & 0x10) == 0x10)
                EventSystem.sendEvent((int)EVENT_MAINUI.friendGetList, null, (int)EVENT_GROUP.mainUI);
        }
        
        void onFriendUpdate(int nEvent, System.Object param)
        {
            MSG_CLIENT_FRIEND_UPDATE msg = (MSG_CLIENT_FRIEND_UPDATE)param;
            if (msg == null)
                return;
            foreach (FRIEND_UPDATE_INFO_TEMP item in msg.lst)
            {
                FriendInfo info = this.getFriend(item.idFriendUser);
                if (info != null)
                {
                    info.bCollect = (item.un8Collected==1);
                    info.giveGiftTime = item.un32GiftGiveTime;
                    info.applyTime = item.un32ApplyTime;
                    info.status = (FRIEND_STATUS)item.un8Status;
                }
                EventSystem.sendEvent((int)EVENT_MAINUI.friendUpdate,item.idFriendUser, (int)EVENT_GROUP.mainUI);
            }
        }

        //申请加好友
        public void apply(UInt64 target)
        {
            MSG_CLIENT_FRIEND_APPLY_REQUEST request = new MSG_CLIENT_FRIEND_APPLY_REQUEST();
            request.userid = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.id;
            request.idTarget = target;
            NetworkMgr.sendData(request);
        }

        void onApply(int nEvent, System.Object param)
        {
            MSG_CLIENT_FRIEND_APPLY_RESPONSE msg = (MSG_CLIENT_FRIEND_APPLY_RESPONSE)param;
            if (msg == null)
                return;

            EventSystem.sendEvent((int)EVENT_MAINUI.friendApply, msg.isSucc(msg.wErrCode), (int)EVENT_GROUP.mainUI);

            DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowPanelFriends();
        }

        //取消申请加好友
        public void cancelApply(UInt64 target)
        {
            MSG_CLIENT_FRIEND_CANCEL_APPLY_REQUEST request = new MSG_CLIENT_FRIEND_CANCEL_APPLY_REQUEST();
            request.userid = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.id;
            request.idTarget = target;
            NetworkMgr.sendData(request);
        }

        void onCancelApply(int nEvent, System.Object param)
        {
            MSG_CLIENT_FRIEND_CANCEL_APPLY_RESPONSE msg = (MSG_CLIENT_FRIEND_CANCEL_APPLY_RESPONSE)param;
            if (msg == null)
                return;
            EventSystem.sendEvent((int)EVENT_MAINUI.friendCancelApply, msg.isSucc(msg.wErrCode), (int)EVENT_GROUP.mainUI);


        }

        //收藏
        public void Collect(UInt64 target)
        {
            MSG_CLIENT_FRIEND_COLLECT_REQUEST request = new MSG_CLIENT_FRIEND_COLLECT_REQUEST();
            request.userid = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.id;
            request.idTarget = target;
            NetworkMgr.sendData(request);
        }

        void onCollect(int nEvent, System.Object param)
        {
            MSG_CLIENT_FRIEND_CANCEL_APPLY_RESPONSE msg = (MSG_CLIENT_FRIEND_CANCEL_APPLY_RESPONSE)param;
            if (msg == null)
                return;
            EventSystem.sendEvent((int)EVENT_MAINUI.friendCollect, msg.isSucc(msg.wErrCode), (int)EVENT_GROUP.mainUI);
        }

        //拒绝加好友
        public void refuse(UInt64 target)
        {
            MSG_CLIENT_FRIEND_REFUSE_REQUEST request = new MSG_CLIENT_FRIEND_REFUSE_REQUEST();
            request.userid = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.id;
            request.idTarget = target;
            NetworkMgr.sendData(request);
        }

        void onRefuse(int nEvent, System.Object param)
        {
            MSG_CLIENT_FRIEND_REFUSE_RESPONSE msg = (MSG_CLIENT_FRIEND_REFUSE_RESPONSE)param;
            if (msg == null)
                return;
            EventSystem.sendEvent((int)EVENT_MAINUI.friendRefuse, msg.isSucc(msg.wErrCode), (int)EVENT_GROUP.mainUI);
        }

        //同意加好友
        public void accept(UInt64 target)
        {
            MSG_CLIENT_FRIEND_ACCEPT_REQUEST request = new MSG_CLIENT_FRIEND_ACCEPT_REQUEST();
            request.userid = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.id;
            request.idTarget = target;
            NetworkMgr.sendData(request);
        }

        void onAccept(int nEvent, System.Object param)
        {
            MSG_CLIENT_FRIEND_ACCEPT_RESPONSE msg = (MSG_CLIENT_FRIEND_ACCEPT_RESPONSE)param;
            if (msg == null)
                return;
            EventSystem.sendEvent((int)EVENT_MAINUI.friendAccept, msg.isSucc(msg.wErrCode), (int)EVENT_GROUP.mainUI);
        }

        //删除好友
        public void delFriend(UInt64 target)
        {
            MSG_CLIENT_FRIEND_DEL_REQUEST request = new MSG_CLIENT_FRIEND_DEL_REQUEST();
            request.userid = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.id;
            request.idTarget = target;
            NetworkMgr.sendData(request);
        }

        void onDelFriend(int nEvent, System.Object param)
        {
            MSG_CLIENT_FRIEND_DEL_RESPONSE msg = (MSG_CLIENT_FRIEND_DEL_RESPONSE)param;
            if (msg == null)
                return;
            EventSystem.sendEvent((int)EVENT_MAINUI.friendDelete, msg.isSucc(msg.wErrCode), (int)EVENT_GROUP.mainUI);
        }

        //查找好友
        public void search(UInt64 target)
        {
            MSG_CLIENT_SEARCH_USER_REQUEST request = new MSG_CLIENT_SEARCH_USER_REQUEST();
            request.userid = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.id;
            request.idTarget = target;
            NetworkMgr.sendData(request);
        }

        void onSearchFriend(int nEvent, System.Object param)
        {
            MSG_CLIENT_SEARCH_USER_RESPONSE msg = (MSG_CLIENT_SEARCH_USER_RESPONSE)param;
            if (msg == null)
                return;
            FriendInfo friend = null;
            if(msg.isSucc(msg.wErrCode))
            {
                friend = new FriendInfo();
                friend.userid = msg.targetid;
                friend.username = msg.strName;
                friend.level = msg.level;
                friend.arenaPoint = msg.arenaPoint;
                friend.arenaHonor = UserInfo.getArenaHonor(msg.arenaPoint);
                friend.hero.InitDict((int)msg.heroType);
                friend.hero.level = (int)msg.heroLevel;
            }
            EventSystem.sendEvent((int)EVENT_MAINUI.frinedSearch, friend, (int)EVENT_GROUP.mainUI);

            DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowFindUser(friend);
        }

        //送礼物
        public void sendGift(UInt32 gifttype,UInt64[] target)
        {
            MSG_CLIENT_FRIEND_GIVE_GIFT_REQUEST request = new MSG_CLIENT_FRIEND_GIVE_GIFT_REQUEST();
            request.userid = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.id;
            request.idGiftType = gifttype;
            request.usCnt = (UInt16)target.Length;
            request.idTarget = target;

            NetworkMgr.sendData(request);
        }

        void onGivegift(int nEvent, System.Object param)
        {
            MSG_CLIENT_FRIEND_GIVE_GIFT_RESPONSE msg = (MSG_CLIENT_FRIEND_GIVE_GIFT_RESPONSE)param;
            if (msg == null)
                return;
            EventSystem.sendEvent((int)EVENT_MAINUI.friendSendGift, msg.isSucc(msg.wErrCode), (int)EVENT_GROUP.mainUI);
        }

        //接受礼物
        public void awardGift(UInt64 target,UInt32 gift)
        {
            MSG_CLIENT_FRIEND_AWARD_GIFT_REQUEST request = new MSG_CLIENT_FRIEND_AWARD_GIFT_REQUEST();
            request.userid = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.id;
            request.idTarget = target;
            request.idGift = gift;
            NetworkMgr.sendData(request);
        }

        void onAwardGift(int nEvent, System.Object param)
        {
            MSG_CLIENT_FRIEND_AWARD_GIFT_RESPONSE msg = (MSG_CLIENT_FRIEND_AWARD_GIFT_RESPONSE)param;
            if (msg == null)
                return;
            EventSystem.sendEvent((int)EVENT_MAINUI.friendAwardGift, msg.isSucc(msg.wErrCode), (int)EVENT_GROUP.mainUI);


            //panelFriends.FriendPages[4].GetComponent<FriendGiftPage>().ShowFriend();
        }


        int nSetGiftIndex = 0;
        uint uGifttype = 0;
        //设置期望礼物列表
        public void setGift(int index,UInt32 gifttype)
        {
            nSetGiftIndex = index;
            uGifttype = gifttype;

            MSG_CLIENT_FRIEND_GIFT_SET_REQUEST request = new MSG_CLIENT_FRIEND_GIFT_SET_REQUEST();
            request.index = (byte)index;
            request.gifttype = gifttype;
            NetworkMgr.sendData(request);
        }

        void onSetGift(int nEvent, System.Object param)
        {
            MSG_CLIENT_FRIEND_GIFT_SET_RESPONSE msg = (MSG_CLIENT_FRIEND_GIFT_SET_RESPONSE)param;
            if (msg == null)
                return;
            EventSystem.sendEvent((int)EVENT_MAINUI.friendSetGift, msg.isSucc(msg.errcode), (int)EVENT_GROUP.mainUI);

            UpdateSelfNeedGift(nSetGiftIndex, (int)uGifttype);
        }

        //请求礼物列表
        public void RequestGiftList()
        {
            MSG_CLIENT_FRIEND_GIFT_LIST_REQUEST request = new MSG_CLIENT_FRIEND_GIFT_LIST_REQUEST();
            request.userid = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.id;
            NetworkMgr.sendData(request);
        }
        
        void onRecvGiftList(int nEvent, System.Object param)
        {
            MSG_CLIENT_FRIEND_GIFT_LIST msg = (MSG_CLIENT_FRIEND_GIFT_LIST)param;
            if(msg==null)
                return;
            _giftList.Clear();
            foreach (FRIEND_GIFT_INFO_TMP item in msg.lst)
            {
                FriendGift gift = new FriendGift();
                gift.init(item);
                _giftList[gift.giftid] = gift;
            }
            EventSystem.sendEvent((int)EVENT_MAINUI.friendGetGift, null, (int)EVENT_GROUP.mainUI);


            panelFriends.FriendPages[4].GetComponent<FriendGiftPage>().ShowFriend();
        }

        void onGiftDel(int nEvent, System.Object param)
        {
            MSG_CLIENT_FRIEND_GIFT_DEL msg = (MSG_CLIENT_FRIEND_GIFT_DEL)param;
            if (msg == null)
                return;
            _giftList.Remove(msg.idGift);
            EventSystem.sendEvent((int)EVENT_MAINUI.friendDelGift, msg.idGift, (int)EVENT_GROUP.mainUI);

            panelFriends.FriendPages[4].GetComponent<FriendGiftPage>().ShowFriend();
        }

        public FriendInfo[] getFriendList(uint giftid = 0)
        {
            List<FriendInfo> theList = new List<FriendInfo>();
            foreach (KeyValuePair<UInt64, FriendInfo> item in this._friendList)
            {
                if (giftid == 0 || item.Value.needGift((int)giftid))
                    theList.Add(item.Value);
            }
            return theList.ToArray();
        }

        public FriendInfo getFriend(UInt64 id)
        {
            if (this._friendList.ContainsKey(id))
                return _friendList[id];
            return null;
        }

        public FriendGift[] getGiftList()
        {
            List<FriendGift> theList = new List<FriendGift>();
            foreach (KeyValuePair<UInt32, FriendGift> item in this._giftList)
                theList.Add(item.Value);

            return theList.ToArray();
        }


        //礼物配置
        public List<FriendGift> giftTypeList = new List<FriendGift>();
        protected void InitGiftType()
        {
            giftTypeList.Clear();

            ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_FRIEND_GIFT);

            int nCount = table.rows.Length;

            for (int i = 0; i < nCount; ++i)
            {
                int nType = table.rows[i].getIntValue(DICT_FRIEND_GIFT.GIFT_TYPEID);

                FriendGift fGift = new FriendGift();

                fGift.iniGift(nType);

                giftTypeList.Add(fGift);
            }

        }

        //好友是否收藏

        //UI界面
        public PanelFriends panelFriends;

        public void ShowPanelFriends()
        {
            panelFriends.InitUI();
            
            panelFriends.gameObject.SetActive(true);
        }

        public void ShowFriendsList()
        {
            panelFriends.ShowPageByIndex(1);
        }

        public void ShowFriendApply()
        {
            panelFriends.ShowPageByIndex(2);
        }

        public void ShowFriendFind()
        {
            panelFriends.ShowPageByIndex(3);
        }

        public void ShowFriendGift()
        {
            RequestGiftList();
            panelFriends.ShowPageByIndex(4);
        }

        public void ShowFriendPresent()
        {
            panelFriends.ShowPageByIndex(5);
        }

        public void ShowFriendDetail(UInt64 FriendID)
        {
            panelFriends.ShowPageByIndex(6);

            GameObject detail = panelFriends.FriendPages[6];
            FriendDetail fd = detail.GetComponent<FriendDetail>();

            fd.ShowInfo(_friendList[FriendID]);

        }

        public void ShowFindUser(FriendInfo fi)
        {
            panelFriends.ShowPageByIndex(7);

            FindUser fu = panelFriends.FriendPages[7].GetComponent<FindUser>();

            fu.ShowInfo(fi);
        }

        public void SelectNeedGift(int nIndex)
        {
            SelectNeedGift sng = panelFriends.FriendPages[8].GetComponent<SelectNeedGift>();
            sng.nIndex = nIndex;

            panelFriends.ShowPageByIndex(8);
        }

        public void UpdateSelfNeedGift(int nIndex, int type)
        {
            ShowFriendGift();
            //panelFriends.FriendPages[4].GetComponent<FriendGiftPage>().UpdateNeedGift(nIndex, type);
        }
    }
}