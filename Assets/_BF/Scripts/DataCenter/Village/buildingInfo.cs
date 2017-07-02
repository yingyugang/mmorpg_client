using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;

namespace DataCenter
{
    public enum BUILD_TYPE
    {
        BUILD_UNKOWN =0,
        BUILD_STONE = 15002, // 宝玉屋
        BUILD_SYNTHETIZE = 15003, // 调和屋
        BUILD_ITEM_STORAGE = 15004, // 道具仓库
        BUILD_FARMLAND = 15005,
        BUILD_MOUNTAIN = 15006,
        BUILD_RIVERS = 15007,
        BUILD_TREE = 15008,
        BUILD_MUSIC = 15009,
    }
	public class buildingInfo
	{
        BUILD_TYPE _type;
        uint _curlevel;
        uint _maxLevel;
        uint _curSoul;
        uint _maxSoul;
        uint _lastLevy;
        uint _lastLevyTimes;
        uint _id;
        //效果

        public uint level
        {
            get { return _curlevel; }
        }

        void setLevel(uint value)
        {
            _curlevel = value;
            ConfigTable build = ConfigMgr.getConfig(CONFIG_MODULE.DICT_BUILDING_LEVEL);
            if (build != null)
            {
				ConfigRow row = build.getRow(DICT_BUILDING_LEVEL.BUILDING_TYPEID, (int)_type, DICT_BUILDING_LEVEL.LEVEL, (int)(_curlevel+1));
                if (row != null)
                {
                    _maxSoul = (uint)row.getIntValue(DICT_BUILDING_LEVEL.COST_SOUL);
                }
            }
        }

        public uint id { get { return _id; } }
        public uint lastLevy { get { return _lastLevy; } }
        public uint lastLevyTimes { get { return _lastLevyTimes; } }

        public uint maxSoul
        {
            get { return _maxSoul; }
        }

        public uint curSoul
        {
            get { return _curSoul; }
        }

        public BUILD_TYPE type
        {
            get { return _type; }
        }

        public bool isMax
        {
            get { return true; }
        }

        public bool init(BUILDING_INFO info)
        {
            this._id = info.idBuilding;
            this._type = getType(info.idBuildingType);
            this._curSoul = info.unSoul;
            this._lastLevy = info.u32LastLevy;
            this._lastLevyTimes = info.u32LevyTimes;
            this.setLevel(info.cbLev);
            return true;
        }

        public void update(MSG_BUILDING_UPDATE_EVENT info)
        {
            if (this._id != info.idBuilding)
                return;
            this.setLevel(info.cbLev);
            this._curSoul = info.unSoul;
            this._lastLevy = info.u32LastLevy;
            this._lastLevyTimes = info.u32LevyTimes;
        }

		/*
		public void update(MSG_BUILDING_UPLEV_RESPONSE info)
		{
			if (this._id != info.idBuilding)
				return;
			this.setLevel(info.cbLev);
			this._curSoul = info.unSoul;
			this._lastLevy = info.u32LastLevy;
			this._lastLevyTimes = info.u32LevyTimes;
		}
*/
        public static BUILD_TYPE getType(uint type)
        {
            return Tools.getEnumValue<BUILD_TYPE>((int)type, BUILD_TYPE.BUILD_UNKOWN);
        }
	}
}
