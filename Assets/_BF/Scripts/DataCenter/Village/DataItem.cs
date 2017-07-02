using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;

namespace DataCenter
{
    //道具管理
    class DataItem : DataModule
	{
        const int MAX_BATTLE_ITEM = 5;
        private Dictionary<uint, ItemInfo> _itemList = new Dictionary<uint, ItemInfo>();
        private ItemInfo[] _battleItemList = new ItemInfo[MAX_BATTLE_ITEM];

        public override bool init()
        {
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_ITEM_LIST_EVENT, onItemList, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_ITEM_UPDATE_EVENT, onItemUpdate, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLEINT_MAKE_ITEM_BY_FORMULA, onMakeItem, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLEINT_UNEQUIP_ITEM, onUnEquip, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLEINT_EQUIP_ITEM, onEquip, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLEINT_ITEM_SELL, onSale, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLEINT_BUY_ITEM_SIZE, onBuySize, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLEINT_SET_BATTLE_IEM, onSetBattleItem, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_ITEM_DELETE_EVENT, onDeleteItem, (int)DataCenter.EVENT_GROUP.packet);
            return true;
        }

        void onItemList(int nEvent, System.Object param)
        {
//            _itemList.Clear();
            MSG_ITEM_LIST_EVENT msg = (MSG_ITEM_LIST_EVENT)param;
            foreach (ITEM_INFO item in msg.lst)
            {
                ItemInfo newItem = new ItemInfo();
                newItem.init(item);
                _itemList[newItem.id] = newItem;
            }
        }

        public void recvBattleItem(uint index,uint typeid,uint count)
        {
            if (index >= MAX_BATTLE_ITEM)
                return;
            ItemInfo item = new ItemInfo();
            item.init((int)typeid);
            item.amount = count;
            _battleItemList[index] = item;
        }

        public ItemInfo getBattleItem(uint index)
        {
            if (index >= MAX_BATTLE_ITEM)
                return null;
            return _battleItemList[index];
        }

        public ItemInfo[] getBattleItemList()
        {
            return _battleItemList;
        }

        public ItemInfo getItem(uint id)
        {
            if(_itemList.ContainsKey(id))
                return _itemList[id];
            return null;
        }

        public uint getItemCount()
        {
            return (uint)this._itemList.Count;
        }

        //获得指定类型道具数量
        public int getItemCountByType(uint typeid)
        {
            int count = 0;
            foreach (KeyValuePair<uint, ItemInfo> it in this._itemList)
            {
                if (it.Value.type == typeid)
                    count += (int)it.Value.amount;
            }
			UnityEngine.Debug.Log("getItemCountByType:  " + typeid + ":" + count);
            return count;
        }

        public ItemInfo[] getItemListByType(uint type)
        {
            List<ItemInfo> theList = new List<ItemInfo>();
            foreach (KeyValuePair<uint, ItemInfo> it in this._itemList)
            {
                if (type != it.Value.type)
                    continue;
                theList.Add(it.Value);
            }
            return theList.ToArray();
        }

        public ItemInfo[] getItemList(ITEM_SORT type)
        {
            List<ItemInfo> theList = new List<ItemInfo>();
            foreach (KeyValuePair<uint, ItemInfo> it in this._itemList)
            {
                if (type != ITEM_SORT.all && type != it.Value.sortType)
                    continue;
                theList.Add(it.Value);
            }
            return theList.ToArray();
        }

        public override void release()
        {
            _itemList.Clear();
        }

		void onSetBattleItem(int nEvent, System.Object param)
		{
			EventSystem.sendEvent((int)EVENT_MAINUI.itemSetBattle,null,(int)EVENT_GROUP.mainUI);
		}

        void onDeleteItem(int nEvent, System.Object param)
        {
            MSG_ITEM_DELETE_EVENT msg = (MSG_ITEM_DELETE_EVENT)param;
            this._itemList.Remove(msg.idItem);
			EventSystem.sendEvent((int)EVENT_MAINUI.itemDelete,msg.idItem,(int)EVENT_GROUP.mainUI);
        }

        //道具更新
        void onItemUpdate(int nEvent, System.Object param)
        {
            MSG_ITEM_UPDATE_EVENT msg = (MSG_ITEM_UPDATE_EVENT)param;
            ItemInfo item = this.getItem(msg.idItem);
            if (item == null)
            {
                item = new ItemInfo();
                this._itemList[msg.idItem] = item;

            }
            item.init(msg);
            UserInfo info = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser;
            if (info != null)
                info.setOpenItemLib((uint)item.library);

			EventSystem.sendEvent((int)EVENT_MAINUI.itemUpdate,msg.idItem,(int)EVENT_GROUP.mainUI);
		}

        //设置道具栏
        public void setBattleItem(ItemSet[] lst)
        {
            MSG_SET_BATTLE_IEM_REQUEST msg = new MSG_SET_BATTLE_IEM_REQUEST();

            List<BATTLE_ITEM_REQ_INFO> theList = new List<BATTLE_ITEM_REQ_INFO>();
            foreach (ItemSet it in lst)
            {
                BATTLE_ITEM_REQ_INFO item = new BATTLE_ITEM_REQ_INFO();
                item.idItem = it.itemId;
                item.unIndex = it.index;
                item.unAmount = it.count;
                theList.Add(item);
            }

            msg.lst = theList.ToArray();
            msg.usCnt = (ushort)theList.Count;
            NetworkMgr.sendData(msg);
        }

        //购买道具格子
        public void buySize()
        {
            NetworkMgr.sendData(new MSG_BUY_ITEM_SIZE_REQUEST());
        }

        public void onBuySize(int nEvent, System.Object param)
        {
            //MSG_BUY_ITEM_SIZE_RESPONSE msg = (MSG_BUY_ITEM_SIZE_RESPONSE)param;
        }

        //出售道具
        public void saleItem(uint itemid,uint count)
        {
            if (count == 0)
                return;
            MSG_ITEM_SELL_REQUEST msg = new MSG_ITEM_SELL_REQUEST();
            msg.idItem = itemid;
            msg.unAmount = count;
            NetworkMgr.sendData(msg);
        }

        void onSale(int nEvent, System.Object param)
        {
            //MSG_ITEM_SELL_RESPONSE msg = (MSG_ITEM_SELL_RESPONSE)param;
        }

        //装备物品
        public void equip(uint hero,uint item)
        {
            MSG_CLEINT_EQUIP_ITEM_REQUEST msg = new MSG_CLEINT_EQUIP_ITEM_REQUEST();
            msg.idHero = hero;
            msg.idItem = item;
            NetworkMgr.sendData(msg);
        }

        void onEquip(int nEvent, System.Object param)
        {
            MSG_CLEINT_EQUIP_ITEM_RESPONSE msg = (MSG_CLEINT_EQUIP_ITEM_RESPONSE)param;

            if (msg.wErrCode == 100000)
            {
                Debug.Log("HeroEquipSuc");
                DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).UpdateEquip();
            }
        }

        //解除装备
        public void unEquip(uint hero)
        {
            MSG_CLEINT_UNEQUIP_ITEM_REQUEST msg = new MSG_CLEINT_UNEQUIP_ITEM_REQUEST();
            msg.idHero = hero;
            NetworkMgr.sendData(msg);
        }

        void onUnEquip(int nEvent, System.Object param)
        {
            MSG_CLEINT_UNEQUIP_ITEM_RESPONSE msg = (MSG_CLEINT_UNEQUIP_ITEM_RESPONSE)param;

            if (msg.wErrCode == 100000)
            {
                Debug.Log("HeroUnEquipSuc");
                DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).UpdateEquip();
            }
        }

        //制作道具
        public void makeItem(uint itemType,uint count)
        {
            MSG_MAKE_ITEM_BY_FORMULA_REQUEST msg = new MSG_MAKE_ITEM_BY_FORMULA_REQUEST();
            msg.idItemType = itemType;
            msg.unAmount = count;
            NetworkMgr.sendData(msg);
        }

        void onMakeItem(int nEvent, System.Object param)
        {
            MSG_MAKE_ITEM_BY_FORMULA_RESPONSE msg = (MSG_MAKE_ITEM_BY_FORMULA_RESPONSE)param;
            if (msg.isSucc((int)msg.wErrCode))
                EventSystem.sendEvent((int)EVENT_MAINUI.itemMake,null,(int)EVENT_GROUP.mainUI);
        }
	}
}
