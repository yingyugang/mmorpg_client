using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;

namespace DataCenter
{
    public enum ITEM_SORT
    {
        unknown =0,
        item = 1,//1素材
        drug = 2,//2道具（药品）
        stone = 3, //3宝玉
        all
    }

    public class ItemSet
    {
        public uint index;
        public uint itemId;
        public uint count;
    }
    //道具
	public class ItemInfo
	{
        //服务端下发
        uint _id;
        uint _type;
        uint _hero;
        uint _amount;

        //字典表读取
        //类型
        ITEM_SORT _sortType;
        //图集
        string _strName;
        string _strDesc;
        string _strDescEx;
        string _strAtlas;
        string _strIcon;
        bool _stackAble;
        uint _salePrice;
        uint _maxUsed;
        //效果
        int _effect;
        int _library;

        public uint id { get { return _id; } }
        public uint type { get { return _type; } }
        public uint hero { get { return _hero; } set { _hero = value; } }
        public uint amount { get { return _amount; } set { _amount = value; } }
        public string name {get {return this._strName;}}
        public string desc {get {return this._strDesc;}}
        public string descEx {get {return this._strDescEx;}}
        public ITEM_SORT sortType { get { return this._sortType; } }
        public string atlas { get { return this._strAtlas; } }
        public string icon {get {return this._strIcon;}}
        public bool stack {get {return this._stackAble;}}
        public uint price{get {return this._salePrice;}}
        public uint maxUsed { get { return this._maxUsed; } }
        public int effect { get { return this._effect; } }
        public int library { get { return this._library; } }

        public ItemInfo()
        {
        }

        public void init(ITEM_INFO info)
        {
            this._id = info.idItem;
            this._type = info.idItemType;
            this._hero = info.idHeroEquip;
            this._amount = info.unAmount;
            init((int)this._type);
        }

        public void init(MSG_ITEM_UPDATE_EVENT info)
        {
            this._id = info.idItem;
            this._type = info.idItemType;
            this._hero = info.idHeroEquip;
            this._amount = info.unAmount;
            init((int)this._type);
        }

		public void init(int type)
		{
			this._type = (uint)type;

			ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ITEM);
			if (table == null)
				return;
			
			ConfigRow row = table.getRow(DICT_ITEM.ITEM_TYPEID,(int)this._type);
			if(row==null)
				return;
			
			this._sortType = row.getEnumValue<ITEM_SORT>(DICT_ITEM.SORT,ITEM_SORT.unknown);
			this._strName = BaseLib.LanguageMgr.getString(row.getIntValue(DICT_ITEM.NAME_ID));
			this._strDesc = BaseLib.LanguageMgr.getString(row.getIntValue(DICT_ITEM.DESC_ID));
			this._strDescEx = BaseLib.LanguageMgr.getString(row.getIntValue(DICT_ITEM.DESC_EX_ID));
			this._strAtlas = row.getStringValue(DICT_ITEM.ICON_FILE);
			this._strIcon = row.getStringValue(DICT_ITEM.ICON_SPRITE_NAME);
			this._stackAble = row.getBoolValue(DICT_ITEM.STACKABLE,false);
			this._salePrice = (uint)row.getIntValue(DICT_ITEM.PRICE);
			this._maxUsed = (uint)row.getIntValue(DICT_ITEM.MAX_USED);
			this._effect = row.getIntValue(DICT_ITEM.EFFECT_ID);
            this._library = row.getIntValue(DICT_ITEM.LIBRARY);
		}
	}
}
