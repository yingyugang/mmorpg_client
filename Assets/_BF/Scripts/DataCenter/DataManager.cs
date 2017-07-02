using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BaseLib;

namespace DataCenter
{
    public enum DATA_MODULE
    {
        Data_ServTime = 0,
        Data_User,
        Data_Hero,
		Data_Task,
		Data_Village,
        Data_Battle,
        Data_Building,//建筑管理
        Data_Item,//道具管理
		Data_Skill,
        Data_Summons, //召唤兽
        Data_Arena,//竞技场
        Data_Friend,//好友
        Data_Post,//公告
        Data_chat,//聊天
        Data_Shop,//商店
        Data_Present,//礼物
        Data_Story,//剧情
    }

	public class DataManager : SingletonMonoBehaviour<DataManager>
    {
        Dictionary<DATA_MODULE, DataModule> _moduleList = new Dictionary<DATA_MODULE, DataModule>();

		public int CurrentPanelIndex;

        protected override void Init()
        {
            PacketData.init();
            onInit();
        }

        void onInit()
        {
            this._moduleList.Clear();
            addModule<DataServTime>(DATA_MODULE.Data_ServTime);
            addModule<DataUser>(DATA_MODULE.Data_User);
            addModule<DataHero>(DATA_MODULE.Data_Hero);
            addModule<DataBuilding>(DATA_MODULE.Data_Building);
            addModule<DataItem>(DATA_MODULE.Data_Item);
            addModule<DataSummons>(DATA_MODULE.Data_Summons);
            addModule<DataArena>(DATA_MODULE.Data_Arena);
            addModule<DataFriend>(DATA_MODULE.Data_Friend);
            addModule<DataPost>(DATA_MODULE.Data_Post);
            addModule<DataShop>(DATA_MODULE.Data_Shop);
            addModule<DataPresent>(DATA_MODULE.Data_Present);
            addModule<DataStory>(DATA_MODULE.Data_Story);
        }

        void Update()
        {
            DataUser userdata = DataManager.getModule<DataUser>(DATA_MODULE.Data_User);
            if (userdata != null)
                userdata.sendActivePack();
        }

        T addModule<T>(DATA_MODULE key) where T:DataModule,new()
        {
            T newObj = new T();
            newObj.init();
            _moduleList[key] = newObj;
            return newObj;
        }

        static public T getModule<T>(DATA_MODULE key) where T : DataModule,new()
        {
            if (DataManager.me == null)
                return null;
            DataModule temp;
            lock (DataManager.me._moduleList)
            {
                if (!DataManager.me._moduleList.TryGetValue(key, out temp))
                {
                    temp = DataManager.me.addModule<T>(key);
                }
            }
            return temp as T;
        }

        static public void releaseModule(DATA_MODULE key)
        {
            DataModule temp;
            if (!DataManager.me._moduleList.TryGetValue(key, out temp))
            {
                temp.release();
                DataManager.me._moduleList.Remove(key);
            }
        }

        static public void release()
        {
            foreach (KeyValuePair<DATA_MODULE, DataModule> module in DataManager.me._moduleList)
            {
                if (module.Value != null)
                    module.Value.release();
            }
            DataManager.me.onInit();
        }

    }
}