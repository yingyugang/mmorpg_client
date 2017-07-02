using System;
using System.Collections.Generic;
using BaseLib;

namespace DataCenter
{
    public enum SUMMON_QUALITY
    {
        SUMMON_QUALITY_WHITE    = 0,
        SUMMON_QUALITY_GREEN    = 1,
        SUMMON_QUALITY_BLUE     = 2,
        SUMMON_QUALITY_PURPLE   = 3,
    }

    public class SummonInfo
    {
        /*
        public static UInt32 ID_ITEM_WATER_CHIP = 44001;
        public static UInt32 ID_ITEM_FIRE_CHIP = 44002;
        public static UInt32 ID_ITEM_WOOD_CHIP = 44003;
        public static UInt32 ID_ITEM_WIND_CHIP = 44004;
        public static UInt32 ID_ITEM_LIGHT_CHIP = 44005;
        public static UInt32 ID_ITEM_DARK_CHIP = 44006;
        public static UInt32 ID_ITEM_MYTH_CHIP = 44007;
        */

        public UInt32 id;
        public UInt32 type;
        public UInt16 level;
        public UInt32 curSoul;
        public int quality;


        public string name;
        public string desc;
        public string iconFile;
        public string iconSprite;
        public string fbxFile;
        public string portarait;
        public int star;
        public int series;
        public int skillIn;
        public string skillInName;
        public string skillInDesc;
        public int skillOut;
        public string skillOutName;
        public string skillOutDesc;
        public int skillBase;
        public string skillBaseName;
        public string skillBaseDesc;
        public int skillPassive;
        public string skillPassiveName;
        public string skillPassiveDesc;
        public int energy;
        public int copyId;
        public int soul;
        public int chip;
        public int initHp;
        public int initAtk;
        public int initDef;
        public int initRecover;
        public int initViolence;
        public int inherit;
        public int frequency;
        public int skillAp;
        public int atkAp;
        public int defAp;
        
        public int baseHp;
        public int baseAtk;
        public int baseDef;
        public int baseRecover;
        public int baseViolence;

        public int type4;
        public int NO;
        public int maxLv;
        public int hp;
        public int recover;
        public int atk;
        public int def;
        public int violence;
        public int chipType;

        public void init(SUMMON_INFO info)
        {
            id = info.idSummon;
            type = info.idSummonType;
            level = info.wLevel;
            curSoul = info.dwSoul;
            quality = info.btQuality;


            ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_SUMMON);
            ConfigRow row = table.getRow(DICT_SUMMON.SUMMON_TYPEID, (int)type);

            name = row.getStringValue(DICT_SUMMON.NAME_ID);
            name = BaseLib.LanguageMgr.getString(name);

            desc = row.getStringValue(DICT_SUMMON.DESC_ID);

            iconFile = row.getStringValue(DICT_SUMMON.ICON_FILE);
            iconSprite = row.getStringValue(DICT_SUMMON.ICON_SPRITE_NAME);
            fbxFile = row.getStringValue(DICT_SUMMON.FBX_FILE);
            portarait = row.getStringValue(DICT_SUMMON.PORTARAIT);
            star = row.getIntValue(DICT_SUMMON.STAR);
            series = row.getIntValue(DICT_SUMMON.SERIES);

            skillIn = row.getIntValue(DICT_SUMMON.SKILL_IN);
            skillInName = row.getStringValue(DICT_SUMMON.SKILL_IN_NAME);
            skillInDesc = row.getStringValue(DICT_SUMMON.SKILL_IN_DESC);

            skillOut = row.getIntValue(DICT_SUMMON.SKILL_OUT);
            skillOutName = row.getStringValue(DICT_SUMMON.SKILL_OUT_NAME);
            skillOutDesc = row.getStringValue(DICT_SUMMON.SKILL_OUT_DESC);

            skillBase = row.getIntValue(DICT_SUMMON.SKILL_BASE);
            skillBaseName = row.getStringValue(DICT_SUMMON.SKILL_BASE_NAME);
            skillBaseDesc = row.getStringValue(DICT_SUMMON.SKILL_BASE_DESC);

            skillPassive = row.getIntValue(DICT_SUMMON.SKILL_PASSIVE);
            skillPassiveName = row.getStringValue(DICT_SUMMON.SKILL_PASSIVE_NAME);
            skillPassiveDesc = row.getStringValue(DICT_SUMMON.SKILL_PASSIVE_DESC);

            energy = row.getIntValue(DICT_SUMMON.ENERGY);
            //copyId = row.getIntValue(DICT_SUMMON.COPY_ID);
            soul = row.getIntValue(DICT_SUMMON.SOUL);
            //chip = row.getIntValue(DICT_SUMMON.CHIP);

            chip = SummonInfo.GetNeedChipCount((int)type);

            initHp = row.getIntValue(DICT_SUMMON.INIT_HP);
            initAtk = row.getIntValue(DICT_SUMMON.INIT_ATK);
            initDef = row.getIntValue(DICT_SUMMON.INIT_DEF);
            initRecover = row.getIntValue(DICT_SUMMON.INIT_RECOVER);
            initViolence = row.getIntValue(DICT_SUMMON.INIT_VIOLENCE);
            inherit = row.getIntValue(DICT_SUMMON.INHERIT);
            frequency = row.getIntValue(DICT_SUMMON.FREQUENCY);
            skillAp = row.getIntValue(DICT_SUMMON.SKILL_AP);
            atkAp = row.getIntValue(DICT_SUMMON.ATK_AP);
            defAp = row.getIntValue(DICT_SUMMON.DEF_AP);
            baseHp = row.getIntValue(DICT_SUMMON.BASE_HP);
            baseAtk = row.getIntValue(DICT_SUMMON.BASE_ATK);
            baseDef = row.getIntValue(DICT_SUMMON.BASE_DEF);
            baseRecover = row.getIntValue(DICT_SUMMON.BASE_RECOVER);
            baseViolence = row.getIntValue(DICT_SUMMON.BASE_VIOLENCE);

            //需要碎片类型
            switch (series)
            {
                case 1:
                    chipType = 44001;
                    break;
                case 2:
                    chipType = 44002;
                    break;
                case 3:
                    chipType = 44003;
                    break;
                case 4:
                    chipType = 44004;
                    break;
                case 5:
                    chipType = 44005;
                    break;
                case 6:
                    chipType = 44006;
                    break;
                case 7:
                    chipType = 44007;
                    break;
            }

            //最大等级
            switch(star)
            {
                case 1:
                    maxLv = 10;
                    break;
                case 2:
                    maxLv = 15;
                    break;
                case 3:
                    maxLv = 20;
                    break;
                case 4:
                    maxLv = 25;
                    break;
            }

            string strTemp = type.ToString();
            strTemp = strTemp.Substring(0, 4);
            type4 = int.Parse(strTemp);

            strTemp = strTemp.Substring(3, 1);
            NO = int.Parse(strTemp) + 1;

            InitAbility();
        }

        void InitAbility()
        {
            this.hp = this.initHp + this.baseHp * (this.level - 1);
            this.atk = this.initAtk + this.baseAtk * (this.level - 1);
            this.def = this.initDef + this.baseDef * (this.level - 1);
            this.recover = this.initRecover + this.baseRecover * (this.level - 1);
            this.violence = this.initViolence + this.baseViolence * (this.level - 1);
        }

        public static int GetChipType(int nSeries)
        {
            switch (nSeries)
            {
                case 1:
                    return 44001;
                case 2:
                    return 44002;
                case 3:
                    return 44003;
                case 4:
                    return 44004;
                case 5:
                    return 44005;
                case 6:
                    return 44006;
                case 7:
                    return 44007;
            }


            return 0;
        }


        public static int GetNeedChipCount(int nType)
        {
            int chip = 0;
            string strTemp = nType.ToString().Substring(0, 4);
            string qualityType = nType.ToString().Substring(4, 1);
            int nQuality = int.Parse(qualityType);
            
            ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_SUMMON_CFG);
            ConfigRow row = table.getRow(DICT_SUMMON_CFG.SUMMON_TYPE, int.Parse(strTemp));
            if (row != null)
            {

                switch (nQuality)
                {
                    case 1:
                        chip = row.getIntValue(DICT_SUMMON_CFG.WHITE);
                        break;
                    case 2:
                        chip = row.getIntValue(DICT_SUMMON_CFG.GREEN);
                        break;
                    case 3:
                        chip = row.getIntValue(DICT_SUMMON_CFG.BLUE);
                        break;
                    case 4:
                        chip = row.getIntValue(DICT_SUMMON_CFG.PURPLE);
                        break;
                }
            }

            return chip;
        }
    }
}
