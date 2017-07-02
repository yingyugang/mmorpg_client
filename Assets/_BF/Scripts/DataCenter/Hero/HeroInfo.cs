using UnityEngine;
using System;
using System.Collections.Generic;
using BaseLib;

namespace DataCenter
{
	public class HeroInfo
    {
        public int id;

        //类型
        public int type;
        
        //名称
        public string name;

        //描述
        public string desc;

        //图集文件
        public string iconFile;

        //图标名称
        public string spriteName;

        //英雄模型文件
        public string fbxFile;

        //头像文件
        public string portarait;

        //攻击方式
        public int atkMethod;

        //移动速度
        public int moveSpeed;

        //升级方式
        public int lvlupMethod;

        //系
        public int series;

        //星级
        public int star;

        //领导力
        public int leader;

        //图鉴
        public int library;

        //队长技能
        public int skillCaptian;

        //技能
        public int skillBase;

        //作为强化材料获得的经验
        public int sourceExp;

        //售卖金币
        public int coin;

        //初始攻击点
        public int initAtk;

        //初始防御
        public int initDef;

        //初始HP
        public int initHP;

        //初始回复
        public int initRecover;

        //初始暴击率 概率值 千分比
        public int initViolence;

        //攻击加点
        public int baseAtk;

        //防御加点
        public int baseDef;

        //HP加点
        public int baseHP;

        //回复加点
        public int baseRecover;

        //暴击率加成
        public int baseViolence;


		//是否远程攻击
		public int movable; //
        
        //等级
        public int level; 

        //是否参战
        public bool fight = false;

        //当前经验
        public int exp;

        //装备
        public int equipId;

		public int coinPerDrop;
		public int soulPerDrop;
		public int isBoss;
		//站位
		public int location;


        public int hp;
        public int atk;
        public int def;
        public int recover;
        public int violence;

        public string skillCaptianName;
        public string skillCaptianDesc;

        public string skillBaseName;
        public string skillBaseDesc;

        public int btGrowup;
        public int btCollected;
        public int skillLevel;

		public float defaultSkillEffectDelay;
		public float skillEffectDelay;

        public void InitDict(int type)
        {
            ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_HERO);
            if (table == null)
                return;
            this.type = type;
            ConfigRow row = table.getRow(DICT_HERO.HERO_TYPEID, this.type);
            if (row == null)
                return;

            string strTemp = "";
            strTemp = row.getIntValue(DICT_HERO.NAME_ID).ToString();
            this.name = BaseLib.LanguageMgr.getString(strTemp);

            strTemp = row.getIntValue(DICT_HERO.DESC_ID).ToString();
            this.desc = BaseLib.LanguageMgr.getString(strTemp);
            this.iconFile = row.getStringValue(DICT_HERO.ICON_FILE);
            this.spriteName = row.getIntValue(DICT_HERO.ICON_SPRITE_NAME).ToString();
            this.fbxFile = row.getIntValue(DICT_HERO.FBX_FILE).ToString();
            this.portarait = row.getIntValue(DICT_HERO.PORTARAIT).ToString();
            this.atkMethod = row.getIntValue(DICT_HERO.ATK_METHOD);
            this.moveSpeed = row.getIntValue(DICT_HERO.MOVE_SPEED);
            this.lvlupMethod = row.getIntValue(DICT_HERO.LVLUP_METHOD);
            this.series = row.getIntValue(DICT_HERO.SERIES);
            this.star = row.getIntValue(DICT_HERO.STAR);

            if (lvlupMethod > 3)
            {
                level = getMaxLevel();
            }
            this.leader = row.getIntValue(DICT_HERO.LEADER);
            this.library = row.getIntValue(DICT_HERO.LIBRARY);
            this.skillCaptian = row.getIntValue(DICT_HERO.SKILL_CAPTAIN);

            int id = row.getIntValue(DICT_HERO.CAPTAIN_SKILL_NAME);
            this.skillCaptianName = BaseLib.LanguageMgr.getString(id);
            id = row.getIntValue(DICT_HERO.CAPTAIN_SKILL_DESC);
            this.skillCaptianDesc = BaseLib.LanguageMgr.getString(id);
           
            this.skillBase = row.getIntValue(DICT_HERO.SKILL_BASE);
            this.skillBaseName = BaseLib.LanguageMgr.getString(row.getIntValue(DICT_HERO.BASE_SKILL_NAME));
            this.skillBaseDesc = BaseLib.LanguageMgr.getString(row.getIntValue(DICT_HERO.BASE_SKILL_DESC));
            this.sourceExp = row.getIntValue(DICT_HERO.EXP);
            this.coin = row.getIntValue(DICT_HERO.COIN);

            this.initHP = row.getIntValue(DICT_HERO.INIT_HP);
            this.initAtk = row.getIntValue(DICT_HERO.INIT_ATK);
            this.initDef = row.getIntValue(DICT_HERO.INIT_DEF);
            this.initRecover = row.getIntValue(DICT_HERO.INIT_RECOVER);
            this.initViolence = row.getIntValue(DICT_HERO.INIT_VIOLENCE);
            this.baseHP = row.getIntValue(DICT_HERO.BASE_HP);
            this.baseAtk = row.getIntValue(DICT_HERO.BASE_ATK);
            this.baseDef = row.getIntValue(DICT_HERO.BASE_DEF);
            this.baseRecover = row.getIntValue(DICT_HERO.BASE_RECOVER);
            this.baseViolence = row.getIntValue(DICT_HERO.BASE_VIOLENCE);
            this.movable = row.getIntValue(DICT_HERO.MOVABLE);


			this.defaultSkillEffectDelay= row.getIntValue(DICT_HERO.DEFAULT_SKILL_EFFECT_DELAY);
			this.skillEffectDelay= row.getIntValue(DICT_HERO.SKILL_EFFECT_DELAY);

            InitAbility();
            SetIcon();
        }

        void InitAbility()
        {

            this.hp = this.initHP + this.baseHP * (this.level - 1);
            this.atk = this.initAtk + this.baseAtk * (this.level - 1);
            this.def = this.initDef + this.baseDef * (this.level - 1);
            this.recover = this.initRecover + this.baseRecover * (this.level - 1);
            this.violence = this.initViolence + this.baseViolence * (this.level - 1);

            switch (btGrowup)
            {
                case 1:
                    this.atk += this.atk / 10;
                    break;
                case 2:
                    this.def += this.def / 10;
                    break;
                case 3:
                    this.hp += this.hp / 10;
                    break;
                case 4:
                    this.recover += this.recover / 10;
                    break;
                case 5:
                    this.atk += this.atk / 10;
                    this.def += this.def / 10;
                    this.hp += this.hp / 10;
                    this.recover += this.recover / 10;
                    break;
            }
        }

        void SetIcon()
        {
            coin = 10 * level * level - 20 * level + 10;

            switch(star)
            {
                case 1:
                    coin = coin * 1 + 100;
                    break;
                case 2:
                    coin = coin * 1 + 300;

                    if (lvlupMethod == 5)
                    {
                        coin += 6300;
                    }

                    break;
                case 3:
                    coin = (int)(coin * 2.5f) + 1000;

                    if (lvlupMethod == 5)
                    {
                        coin += 19000;
                    }

                    break;
                case 4:
                    coin = (int)(coin * 4.2f) + 2500;


                    if (lvlupMethod == 5)
                    {
                        coin += 69000;
                    }

                    break;
                case 5:
                    coin = (int)(coin * 6.2f) + 5000;
                    break;
            }
        }

        public int getMaxLevel()
        {
            switch (star)
            {
                case 1:
                    return 10;
                case 2:
                    int nMaxLevel = (atkMethod == 3) ? 12 : 30;
                    return nMaxLevel;
                case 3:
                    return 40;
                case 4:
                    return 60;
                case 5:
                    return 80;
                case 6:
                    return 100;
            }

            return 0;
        }

        public bool isMaxLevel()
        {
            switch(star)
            {
                case 1:
                    if (level >= 10)
                    {
                        return true;
                    }
                    break;

                case 2:
                    int nMaxLevel = (atkMethod == 3) ? 12 : 30;
                    if (level >= nMaxLevel)
                    {
                        return true;
                    }
                    break;

                case 3:
                    if (level >= 40)
                    {
                        return true;
                    }
                    break;

                case 4:
                    if (level >= 60)
                    {
                        return true;
                    }
                    break;

                case 5:
                    if (level >= 80)
                    {
                        return true;
                    }
                    break;

                case 6:
                    if (level >= 100)
                    {
                        return true;
                    }
                    break;

            }
            
            return false;
        }


        public int GetSumExp()
        {
            int nExp = 0;
            float fLvlMethod = 1.0f;

            switch (this.lvlupMethod)
            {
                case 1:
                    fLvlMethod = 1.33f;
                    break;
                case 2:
                    fLvlMethod = 2.0f;
                    break;
            }

            ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_HERO_LEVEL);

            for (int i = 1; i <= this.level; ++i)
            {
                ConfigRow row = table.getRow(DICT_HERO_LEVEL.LEVEL, i);

                nExp += (int)(row.getIntValue(DICT_HERO_LEVEL.EXP) * fLvlMethod);
            }

            nExp += this.exp;

            return nExp;
        }

        public int GetLvlupExp()
        {
            int nExp = 0;
            float fLvlMethod = 1.0f;

            switch (this.lvlupMethod)
            {
                case 1:
                    fLvlMethod = 1.33f;
                    break;
                case 2:
                    fLvlMethod = 2.0f;
                    break;
            }

            ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_HERO_LEVEL);
            ConfigRow configRow = table.getRow(DICT_HERO_LEVEL.LEVEL, this.level + 1);
            if (configRow != null)
            {
                nExp = configRow.getIntValue(DICT_HERO_LEVEL.EXP);
                nExp = (int)(nExp * fLvlMethod);
            }

            return nExp;
        }

        public static void ChangeModel(GameObject go, float fscale)
        {
            ParticleSystem[] pss = go.transform.GetComponentsInChildren<ParticleSystem>();

            for (int i = 0; i < pss.Length; ++i)
            {
                pss[i].Stop();
                pss[i].gameObject.SetActive(false);
            }

            Xft.XffectComponent[] xcs = go.transform.GetComponentsInChildren<Xft.XffectComponent>();

            for (int i = 0; i < xcs.Length; ++i)
            {
                xcs[i].StopEmit();
                xcs[i].gameObject.SetActive(false);
            }

            ParticleRenderer[] prs = go.transform.GetComponentsInChildren<ParticleRenderer>();

            for (int i = 0; i < prs.Length; ++i)
            {
                prs[i].enabled = false;
                prs[i].gameObject.SetActive(false);
            }

            SpriteRenderer[] srs = go.transform.GetComponentsInChildren<SpriteRenderer>();

            if (srs.Length != 0)
            {
                if (srs[0].sortingOrder > 100)
                {
                    return;
                }

                for (int i = 0; i < srs.Length; ++i)
                {
                    srs[i].sortingOrder += 20;
                }
            }
            

            SkinnedMeshRenderer[] smrs = go.transform.GetComponentsInChildren<SkinnedMeshRenderer>();

            for (int i = 0; i < smrs.Length; ++i)
            {
                smrs[i].sortingOrder += 20;
            }
        }

        public static void ChangeWhiteModel(GameObject go, Material heroMaterial)
        {
            SkinnedMeshRenderer[] smrs = go.GetComponentsInChildren<SkinnedMeshRenderer>();
            SpriteRenderer[] srs = go.GetComponentsInChildren<SpriteRenderer>();

			for (int i = 0; i < smrs.Length; ++i)
			{
                smrs[i].sharedMaterial = heroMaterial;
                smrs[i].sharedMaterial.SetColor("_Color", Color.white);	
			}

			for (int i = 0; i < srs.Length; ++i)
			{
                srs[i].material = heroMaterial;
                srs[i].sharedMaterial.SetColor("_Color", Color.white);	
    		}
        }
	}


    public class EquipInfo
    {
        public int id;

        //名称
        public string name;

        public string effect;
        public string icon;
        public int heroId;
 
    }

}
