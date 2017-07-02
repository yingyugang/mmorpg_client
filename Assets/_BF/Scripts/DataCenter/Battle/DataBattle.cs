using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BaseLib;

namespace DataCenter
{
    public class DataBattle : DataModule
    {
        BattleResultInfo _battleData = new BattleResultInfo();

        //战斗好友队列
        List<BattleFirend> _friendList = new List<BattleFirend>();
        //常规任务列表
        Dictionary<uint, BattleTask> _taskList = new Dictionary<uint, BattleTask>();
        //活动任务列表
        Dictionary<uint, BattleActivityTask> _activityTaskList = new Dictionary<uint, BattleActivityTask>();
        //
        public BattleRecvData curBattleInfo = null;

        public DataBattle()
        {
        }

        public override bool init()
        {
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_BATTLE_PVE_START, onStart, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_BATTLE_PVE_ACTIVITY_BATTLE, onGetActivityTask, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_BATTLE_PVE_NORMAL_BATTLE, onGetTask, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_BATTLE_PVE_USER_HELP, onGetFriendHelp, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_BATTLE_PVE_FIELD_DROP_CHESTS, onDropChests, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_BATTLE_PVE_FIELD_DROP_ITEM, onDropItem, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_BATTLE_PVE_FIELD_MONSTER_CATCH, onCatchHero, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_BATTLE_PVE_FIELD_ENEMY, onGetEnemyInfo, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_BATTLE_PVE_REWARD, onReward, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLEINT_USE_BATTLE_IEM, onBattleItem, (int)DataCenter.EVENT_GROUP.packet);
            return true;
        }

        public override void release()
        {

        }

        public BattleFirend[] friendList
        {
            get { return this._friendList.ToArray(); }
        }

        public void SetBattleResult(BattleResultInfo info)
        {
            _battleData = info;
            isShowResult = true;
        }

        public BattleResultInfo GetBattleResult()
        {
//			curBattleInfo.reward
			_battleData.exp = (int)curBattleInfo.reward.exp;
            return _battleData;
        }

        public bool isShowResult = false;

        //战斗奖励
        void onReward(int nEvent, System.Object param)
        {
            MSG_CLIENT_BATTLE_PVE_REWARD_EVENT msg = (MSG_CLIENT_BATTLE_PVE_REWARD_EVENT)param;
            if (!checkField(msg.idField))
                return;

            curBattleInfo.reward.win = (msg.cbResult==1);
            curBattleInfo.reward.passAll = (msg.cbBattleAllPass == 1);
            curBattleInfo.reward.diamond = msg.u32Diamond;
            curBattleInfo.reward.exp = msg.u32Exp;
            curBattleInfo.reward.maxStar = msg.cbMaxStar;

            BattleTask task = this.getTaskInfo(msg.idField);
            if (task != null)
            {
                if (msg.cbMaxStar >= 2)
                    task.state = TASK_STATE.PERFECT;
                else
                    task.state = TASK_STATE.PASS;
            }

            BattleActivityTask attivetask = this.getActivityTaskInfo(msg.idField);
            if (attivetask != null)
            {
                if (msg.cbMaxStar >= 2)
                    attivetask.state = TASK_STATE.PERFECT;
                else
                    attivetask.state = TASK_STATE.PASS;
            }

            EventSystem.sendEvent((int)EVENT_MAINUI.battleReward, null, (int)EVENT_GROUP.mainUI);
        }

        //战斗掉落道具
        void onDropItem(int nEvent, System.Object param)
        {
            MSG_CLIENT_BATTLE_PVE_FIELD_DROP_ITEM_EVENT msg = (MSG_CLIENT_BATTLE_PVE_FIELD_DROP_ITEM_EVENT)param;
            if (!checkField(msg.idField))
                return;
            curBattleInfo.setCatchItem(msg.lst);
        }

        //宝箱怪掉落
        void onDropChests(int nEvent, System.Object param)
        {
            MSG_CLIENT_BATTLE_PVE_FIELD_DROP_CHESTS_EVENT msg = (MSG_CLIENT_BATTLE_PVE_FIELD_DROP_CHESTS_EVENT)param;
            if (!checkField(msg.idField))
                return;

            curBattleInfo.setChests(msg.lst);
        }

        //战斗捕获英雄
        void onCatchHero(int nEvent, System.Object param)
        {
            MSG_CLIENT_BATTLE_PVE_FIELD_MONSTER_CATCH_EVENT msg = (MSG_CLIENT_BATTLE_PVE_FIELD_MONSTER_CATCH_EVENT)param;
            if (!checkField(msg.idField))
                return;

            foreach(MONSTER_CATCH_INFO it in msg.lst)
            {
                BattleStep step = curBattleInfo.getStep(it.cbCurStep);
                if (step != null)
                    step.addCatchInfo(it);
            }
        }

        void onGetEnemyInfo(int nEvent, System.Object param)
        {
            MSG_CLIENT_BATTLE_PVE_FIELD_ENEMY_EVENT msg = (MSG_CLIENT_BATTLE_PVE_FIELD_ENEMY_EVENT)param;
            if (!checkField(msg.idField))
                return;
            foreach (ENEMY_INFO it in msg.lst)
			{
                this.curBattleInfo.addStep(it.cbCurStep, it.idEnemy);
			}
			AssetBundleMgr.SingleTon().CacheOrDownloadHeros(this.curBattleInfo.fbxStrings);
			
            if (!checkField(msg.idField))
                return;
        }

        //请求朋友帮忙
        public void getFriendHelp()
        {
            NetworkMgr.sendData(new MSG_CLIENT_BATTLE_PVE_USER_HELP_REQUEST());
        }

        void onGetFriendHelp(int nEvent, System.Object param)
        {
            MSG_CLIENT_BATTLE_PVE_USER_HELP_RESPONSE msg = (MSG_CLIENT_BATTLE_PVE_USER_HELP_RESPONSE)param;
            //
            _friendList.Clear();
            foreach (USER_HELP_INFO it in msg.lst)
            {
                BattleFirend friend = new BattleFirend();
                friend.init(it);
                _friendList.Add(friend);
            }

            EventSystem.sendEvent((int)EVENT_MAINUI.battleFriend, null, (int)EVENT_GROUP.mainUI);
        }

        //获取关卡信息
        public void getAllTaskInfo()
        {
            MSG_CLIENT_BATTLE_PVE_NORMAL_BATTLE_REQUEST msg = new MSG_CLIENT_BATTLE_PVE_NORMAL_BATTLE_REQUEST();
            NetworkMgr.sendData(msg);
        }
        
        void onGetTask(int nEvent, System.Object param)
        {
            this._taskList.Clear();
            MSG_CLIENT_BATTLE_PVE_NORMAL_BATTLE_RESPONSE msg = (MSG_CLIENT_BATTLE_PVE_NORMAL_BATTLE_RESPONSE)param;
            foreach(BATTLE_PVE_NORMAL_BATTLE it in msg.lst)
            {
                BattleTask task = new BattleTask();
                task.init(it.idField, it.cbStatus);
                this._taskList[task.field] = task;
            }
			//BattleTask task1 = new BattleTask();
			//task1.field = 50001;
			//task1.state = TASK_STATE.NEW;
			//this._taskList[task1.field] = task1;

            EventSystem.sendEvent((int)EVENT_MAINUI.battleTask, null, (int)EVENT_GROUP.mainUI);
        }

        public BattleTask getTaskInfo(uint key)
        {
            if(this._taskList.ContainsKey(key))
                return this._taskList[key];
            return null;
        }

        //获取活动关卡信息
        public void getAllActivityTaskInfo()
        {
            MSG_CLIENT_BATTLE_PVE_ACTIVITY_BATTLE_REQUEST msg = new MSG_CLIENT_BATTLE_PVE_ACTIVITY_BATTLE_REQUEST();
            NetworkMgr.sendData(msg);
        }

        void onGetActivityTask(int nEvent, System.Object param)
        {
            this._taskList.Clear();
            MSG_CLIENT_BATTLE_PVE_ACTIVITY_BATTLE_RESPONSE msg = (MSG_CLIENT_BATTLE_PVE_ACTIVITY_BATTLE_RESPONSE)param;
            foreach (BATTLE_PVE_ACTIVITY_BATTLE it in msg.lst)
            {
                BattleActivityTask task = new BattleActivityTask();
                task.init(it.idField, it.cbStatus, it.u32DueTime);
                this._activityTaskList[task.field] = task;
            }
            EventSystem.sendEvent((int)EVENT_MAINUI.battleActivityTask, null, (int)EVENT_GROUP.mainUI);
        }

        public BattleActivityTask getActivityTaskInfo(uint key)
        {
            if (this._activityTaskList.ContainsKey(key))
                return this._activityTaskList[key];
            return null;
        }

        //战斗开始
		public void battleStart(uint field,uint hpTotal,uint idHelpUser)
        {
            MSG_CLIENT_BATTLE_PVE_START_REQUEST msg = new MSG_CLIENT_BATTLE_PVE_START_REQUEST();
            msg.idField = field;
            msg.u32TotalHP = hpTotal;
			msg.idHelpUser = idHelpUser;
            this.curBattleInfo = new BattleRecvData();
            this.curBattleInfo.field = field;

            NetworkMgr.sendData(msg);
        }

        void onStart(int nEvent, System.Object param)
        {
            MSG_CLIENT_BATTLE_PVE_START_RESPONSE msg = (MSG_CLIENT_BATTLE_PVE_START_RESPONSE)param;

            if (!checkField(msg.idField))
                return;
            if (msg.isSucc((int)msg.u32ErrCode))
                EventSystem.sendEvent((int)EVENT_MAINUI.battleStart, null, (int)EVENT_GROUP.mainUI);
        }

        bool checkField(uint field)
        {
            if (this.curBattleInfo != null && this.curBattleInfo.field == field)
                return true;
            return false;
        }

        //战斗结束请求
        public void battleEnd(uint field,uint hp,uint coin,uint soul)
        {
            MSG_CLIENT_BATTLE_PVE_END_REQUEST msg = new MSG_CLIENT_BATTLE_PVE_END_REQUEST();
            msg.idField = field;
            msg.u32Coin = coin;
            msg.u32Soul = soul;
            msg.u32TotalHPRemain = hp;
            NetworkMgr.sendData(msg);
        }

        //战斗消耗
        public void setBattleUseItem(BattleItemUsed[] lst)
        {
            if (lst == null)
                return;
            MSG_USE_BATTLE_IEM_REQUEST msg = new MSG_USE_BATTLE_IEM_REQUEST();
            foreach (BattleItemUsed item in lst)
            {
                USE_BATTLE_IEM_REQ_INFO info = new USE_BATTLE_IEM_REQ_INFO();
                info.idItemType = item.itemType;
                info.unAmount = item.count;
            }
            NetworkMgr.sendData(msg);
        }

        void onBattleItem(int nEvent, System.Object param)
        {
            //MSG_USE_BATTLE_IEM_RESPONSE msg = (MSG_USE_BATTLE_IEM_RESPONSE)param;
        }
    }
}