using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BaseLib;

namespace DataCenter
{
    public enum HERO_SORT
    {
        SORT_DEFAULT,
        SORT_HP,
        SORT_STAR,
        SORT_DEF,
        SORT_CANSEL,
    }
    
    public class DataHero: DataModule
    {
        //英雄
        public List<HeroInfo> heroInfoList = new List<HeroInfo>();
        
        public DataHero()
        {
        }

        public override void release()
        {
        }

        public override bool init()
        {
            //TextDate();
            //equipDate();

            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_HERO_LST_EVENT, onRecvHeroLst, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_HERO_CHANGE_EVENT, onChangeHero, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_HERO_FORMATION_EVENT, onHeroFormation, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_HERO_FORMATION, onSetFormation, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_DIAMOND_ACQUIRE_HERO, onDiamondHero, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_FRIEND_ACQUIRE_HERO, onFriendHero, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_ACQUIRE_EXP_FAT, onExpHero, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_HERO_ATTR_EVENT, onAttrHero, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_HERO_ENHANCE, onEnhanceHero, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_HERO_EVOLVE, onEvolveHero, (int)DataCenter.EVENT_GROUP.packet);

            return true;
        }

        public Dictionary<string, GameObject> dicHeroPrefabs = new Dictionary<string, GameObject>();
        List<string> fbxStrings = new List<string>();
        
        void onRecvHeroLst(int nEvent, System.Object param)
        {
            Debug.Log("onRecvHeroLst");
            MSG_CLIENT_HERO_LST_EVENT msg = (MSG_CLIENT_HERO_LST_EVENT)param;

            int nCount = msg.lst.Length;
            fbxStrings.Clear();

            for (int i = 0; i < nCount; ++i)
            {
                HeroInfo hero = new HeroInfo();

                hero.id = (int)msg.lst[i].idHero;
                hero.type = (int)msg.lst[i].idType;
                hero.btGrowup = (int)msg.lst[i].btGrowup;
                hero.level = (int)msg.lst[i].wLevel;
                hero.skillLevel = (int)msg.lst[i].wSkillLevel;
                hero.exp = (int)msg.lst[i].dwExp;
                hero.equipId = (int)msg.lst[i].dwEquipID;
                hero.btCollected = (int)msg.lst[i].btCollected;

                hero.InitDict(hero.type);
                fbxStrings.Add(hero.fbxFile);
                fbxStrings.Add(GetEvolutionFBX(hero.type));
                heroInfoList.Add(hero);

            }

            InitHeroPrefabs();
        }

        protected void InitHeroPrefabs()
        {
            AssetBundleMgr.SingleTon().CacheOrDownloadHeros(fbxStrings, SetHeroPrefabs);
        }

        protected void InitChangHeroFBX()
        {
            AssetBundleMgr.SingleTon().CacheOrDownloadHeros(fbxStrings, SetChangeHeroPrefabs);
        }

        public string GetEvolutionFBX(int heroType)
        {
            string strTemp = "";

            ConfigTable evolveTable = ConfigMgr.getConfig(CONFIG_MODULE.DICT_HERO_EVOLVE);
            ConfigRow evolveRow = evolveTable.getRow(DICT_HERO_EVOLVE.HERO_TYPEID, heroType);

            if (evolveRow != null)
            {
                int nEvolveTypeId = evolveRow.getIntValue(DICT_HERO_EVOLVE.EVOLVE_TYPEID);
                ConfigTable heroTable = ConfigMgr.getConfig(CONFIG_MODULE.DICT_HERO);
                ConfigRow heroRow = heroTable.getRow(DICT_HERO.HERO_TYPEID, nEvolveTypeId);

				if (heroRow != null)
                	strTemp = heroRow.getStringValue(DICT_HERO.PORTARAIT);
            }


            return strTemp;
        }

        protected void SetHeroPrefabs(Dictionary<_AssetBundleType, Dictionary<string, GameObject>> allPrefabs)
        {
            dicHeroPrefabs = allPrefabs[_AssetBundleType.Hero];
        }

        protected void SetChangeHeroPrefabs(Dictionary<_AssetBundleType, Dictionary<string, GameObject>> allPrefabs)
        {
            dicHeroPrefabs = allPrefabs[_AssetBundleType.Hero];

            if (isShowCallHeroEffect)
            {
                SummonShowHeroDetailById(changeHeroList[0].id);
                ShowCallHeroEffect();
            }
            
        }

        public GameObject GetHeroModel(string fbxName)
        {
            if (AssetBundleMgr.SingleTon().allCachePrefabs[_AssetBundleType.Hero].ContainsKey(fbxName))
            {
                return AssetBundleMgr.SingleTon().allCachePrefabs[_AssetBundleType.Hero][fbxName];
            }

            if (dicHeroPrefabs.ContainsKey(fbxName))
            {
                return dicHeroPrefabs[fbxName];
            }

            return null;
        }

        //改变的英雄
        public List<HeroInfo> changeHeroList = new List<HeroInfo>();

        void onChangeHero(int nEvent, System.Object param)
        {
            if (param == null)
                return;
            changeHeroList.Clear();
            fbxStrings.Clear();
            MSG_HERO_CHANGE_EVENT msg = (MSG_HERO_CHANGE_EVENT)param;
            if (msg.tag == 0)//新增
            {
                foreach (HERO_CHANGE item in msg.lst)
                {
                    HeroInfo newhero = new HeroInfo();
                    newhero.id = (int)item.idHero;
                    newhero.type = (int)item.idHeroType;
                    newhero.btGrowup = (int)item.btGrowup;
                    newhero.level = (int)item.wLevel;
                    newhero.skillLevel = (int)item.btSkillLv1;
                    newhero.exp = 0;
                    newhero.equipId = 0;
                    newhero.btCollected = 0;

                    newhero.InitDict(newhero.type);
                    heroInfoList.Add(newhero);
                    changeHeroList.Add(newhero);
                    fbxStrings.Add(newhero.fbxFile);
                    fbxStrings.Add(GetEvolutionFBX(newhero.type));
                    //发送事件,通知UI
                    //EventSystem.sendEvent();
                    UserInfo info = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser;
                    if (info != null)
                        info.setOpenHeroLib((uint)newhero.library);
                }

                InitChangHeroFBX();
            }
            else //删除
            {
                foreach (HERO_CHANGE item in msg.lst)
                {
                    DelHeroById((int)item.idHero);
                }
            }
        }

        void onHeroFormation(int nEvent, System.Object param)
        {
            MSG_HERO_FORMATION_EVENT msg = (MSG_HERO_FORMATION_EVENT)param;
            int nCount = msg.lst.Length;
            dicHeroTeams.Clear();
            nCurTeamId = (int)DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.fightTroop - 1;

            Debug.Log("onRecvHeroFormation " + nCount.ToString());

            for (int i = 0; i < nCount; ++i)
            {
                TeamInfo ti = new TeamInfo();
                int idTroop = (int)msg.lst[i].idTroop;

                ti.leaderPos = msg.lst[i].btLeaderPos;
                ti.friendPos = msg.lst[i].btFriendPos;
                ti.pos1HeroId = (int)msg.lst[i].idPos1;

                if (ti.pos1HeroId != 0)
                {
                    SetInFight(ti.pos1HeroId);
                }

                ti.pos2HeroId = (int)msg.lst[i].idPos2;

                if (ti.pos2HeroId != 0)
                {
                    SetInFight(ti.pos2HeroId);
                }

                ti.pos3HeroId = (int)msg.lst[i].idPos3;

                if (ti.pos3HeroId != 0)
                {
                    SetInFight(ti.pos3HeroId);
                }

                ti.pos4HeroId = (int)msg.lst[i].idPos4;

                if (ti.pos4HeroId != 0)
                {
                    SetInFight(ti.pos4HeroId);
                }

                ti.pos5HeroId = (int)msg.lst[i].idPos5;

                if (ti.pos5HeroId != 0)
                {
                    SetInFight(ti.pos5HeroId);
                }

                ti.pos6HeroId = (int)msg.lst[i].idPos6;

                if (ti.pos6HeroId != 0)
                {
                    SetInFight(ti.pos6HeroId);
                }

                dicHeroTeams.Add(idTroop, ti);
            }
            
            //初始化召唤院队伍数据
            DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).InitTeamData();
            InitTeamData();


            //添加新队伍
            addNewTeam();
            
            //竞技场队伍


            if (DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.idArenaTroop == 14)
            {
                DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).nPvpTeamId = 2;
            }
            else
            {
                DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).nPvpTeamId = 1;
            }
                   
            DataManager.getModule<DataArena>(DATA_MODULE.Data_Arena).InitTeam();

        }

        public void InitTeamData()
        {
            List<int> removeId = new List<int>();
            foreach (KeyValuePair<int, TeamInfo> item in dicHeroTeams)
            {
                if (item.Value.leaderPos == 0 ||
                    item.Value.friendPos == 0)

                {
                    removeId.Add(item.Key);
                }
                
            }

            foreach (int id in removeId)
            {
                dicHeroTeams.Remove(id);
            }
        }


        void onSetFormation(int nEvent, System.Object param)
        {
            MSG_HERO_FORMATION_RESPONSE msg = (MSG_HERO_FORMATION_RESPONSE)param;

            if (msg.err == 100000)
            {
                Debug.Log("HeroFormationSuc");
            }
            
        }

        public void SendHeroFormationMsg()
        {
            MSG_HERO_FORMATION_REQUEST msg = new MSG_HERO_FORMATION_REQUEST();
            msg.idFightTroop = (uint)nCurTeamId + 1;

            List<TROOP> teamList = new List<TROOP>();
            msg.lst = null;

            for (int i = 0; i < 10; ++i)
            {
                if (!dicHeroTeams.ContainsKey(i + 1))
                {
                    continue;
                }
                
                TeamInfo team = dicHeroTeams[i + 1];
                TROOP newTeam = new TROOP();

                newTeam.idTroop = (byte)(i + 1);

                if (team.leaderPos == 0 || team.friendPos == 0)
                {
                    continue;
                }

                newTeam.btLeaderPos = (byte)team.leaderPos;
                newTeam.btFriendPos = (byte)team.friendPos;
                newTeam.idPos1 = (uint)team.pos1HeroId;
                newTeam.idPos2 = (uint)team.pos2HeroId;
                newTeam.idPos3 = (uint)team.pos3HeroId;
                newTeam.idPos4 = (uint)team.pos4HeroId;
                newTeam.idPos5 = (uint)team.pos5HeroId;
                newTeam.idPos6 = (uint)team.pos6HeroId;

                teamList.Add(newTeam);
            }
            msg.usCnt = (ushort)teamList.Count;
            msg.lst = new TROOP[msg.usCnt];
            msg.lst = teamList.ToArray();

            NetworkMgr.sendData(msg);
        }

        //钻石抽卡
        public void getNewCardDiamond(DiamondNewCard type = DiamondNewCard.ONCE)
        {
            MSG_DIAMOND_ACQUIRE_HERO_REQUEST msg = new MSG_DIAMOND_ACQUIRE_HERO_REQUEST();
            if (type == DiamondNewCard.TENS)
                msg.method = 1;
            else
                msg.method = 0;

            NetworkMgr.sendData(msg);
        }

        //友情抽卡
        public void getNewCard(int nMethod = 0)
        {
            MSG_FRIEND_ACQUIRE_HERO_REQUEST msg = new MSG_FRIEND_ACQUIRE_HERO_REQUEST();
            msg.method = (byte)nMethod;
            NetworkMgr.sendData(msg);
        }

        //钻石抽经验怪
        public void getNewExpCard(int nMethod = 0)
        {
            MSG_ACQUIRE_EXP_FAT_REQUEST msg = new MSG_ACQUIRE_EXP_FAT_REQUEST();
            msg.method = (byte)nMethod;
            NetworkMgr.sendData(msg);
        }

        bool isShowCallHeroEffect = false;
        void onDiamondHero(int nEvent, System.Object param)
        {
            MSG_DIAMOND_ACQUIRE_HERO_RESPONSE msg = (MSG_DIAMOND_ACQUIRE_HERO_RESPONSE)param;

            if (msg.err == 100000)
            {
                Debug.Log("DiamondHeroSuc");
               
                if (1 == changeHeroList.Count)
                {
                    isShowCallHeroEffect = true;
                }
                else
                {
                    isShowCallHeroEffect = false;
                    ShowMultiSummon();
                }

                summon.UpdateDiamond();
            }
        }

        void onFriendHero(int nEvent, System.Object param)
        {
            MSG_FRIEND_ACQUIRE_HERO_RESPONSE msg = (MSG_FRIEND_ACQUIRE_HERO_RESPONSE)param;

            if (msg.err == 100000)
            {
                Debug.Log("FriendHeroSuc");

                if (1 == changeHeroList.Count)
                {
                    isShowCallHeroEffect = true;
                }
                else
                {
                    isShowCallHeroEffect = false;
                    ShowMultiSummon();
                }

                summon.UpdateFriendship();
            }
        }

        void onExpHero(int nEvent, System.Object param)
        {
            MSG_ACQUIRE_EXP_FAT_RESPONSE msg = (MSG_ACQUIRE_EXP_FAT_RESPONSE)param;

            if (msg.err == 100000)
            {
                Debug.Log("ExpHeroSuc");

                if (1 == changeHeroList.Count)
                {
                    isShowCallHeroEffect = true;
                }
                else
                {
                    isShowCallHeroEffect = false;
                    ShowMultiSummon();
                }

                summon.UpdateExpDiamond();
            }
        }

        int nEnhanceExp, nEnhanceResult, nEnhanceSkillLv;
        void onEnhanceHero(int nEvent, System.Object param)
        {
            MSG_HERO_ENHANCE_RESPONSE msg = (MSG_HERO_ENHANCE_RESPONSE)param;

            if (msg.res == 100000)
            {
                Debug.Log("EnhanceHeroSuc");

                nEnhanceExp = (int)msg.unExp;
                nEnhanceResult = (int)msg.btTag;
                nEnhanceSkillLv = (int)msg.btSkillLvl;

                ShowStrengthenEffect();
            }
        }

        void onEvolveHero(int nEvent, System.Object param)
        {
            MSG_HERO_EVOLVE_RESPONSE msg = (MSG_HERO_EVOLVE_RESPONSE)param;

            if (msg.err == 100000)
            {
                Debug.Log("EvolveHeroSuc");

                DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).ShowEvolutionEffect();
                EvolveShowDetail((int)msg.idEvolveHero);
            }
        }

        void onAttrHero(int nEvent, System.Object param)
        {
            MSG_HERO_ATTR_EVENT msg = (MSG_HERO_ATTR_EVENT)param;

            foreach (HeroInfo hero in heroInfoList)
            {
                if (hero.id == msg.idHero)
                {
                    for (int i = 0; i < msg.lst.Length; ++i)
                    {
                        switch ((HERO_ATTR)msg.lst[i].wAttr)
                        {
                            case HERO_ATTR.HERO_ATTR_TYPE_ID:
                                hero.type = (int)msg.lst[i].unVal;
                                break;
                            case HERO_ATTR.HERO_ATTR_GROWUP:
                                hero.btGrowup = (int)msg.lst[i].unVal;
                                break;
                            case HERO_ATTR.HERO_ATTR_LEVEL:
                                hero.level = (int)msg.lst[i].unVal;
                                break;
                            case HERO_ATTR.HERO_ATTR_SKILL_LEVEL:
                                hero.skillLevel = (int)msg.lst[i].unVal;
                                break;
                            case HERO_ATTR.HERO_ATTR_EXP:
                                hero.exp = (int)msg.lst[i].unVal;
                                break;
                            case HERO_ATTR.HERO_ATTR_EQUIP_ID:
                                hero.equipId = (int)msg.lst[i].unVal;
                                break;
                            case HERO_ATTR.HERO_ATTR_COLLECTED:
                                hero.btCollected = (int)msg.lst[i].unVal;
                                break;
                        }
                    }

                    int nlevel = hero.level;
                    int nExp = hero.level;
                    int id = hero.id;

                }
            }
        }

        //出售英雄
        public void sellHero(uint[] heroList)
        {
            MSG_HERO_SELL_REQUEST msg = new MSG_HERO_SELL_REQUEST();
            List<HERO_ID> selList = new List<HERO_ID>();
            foreach (uint item in heroList)
            {
                HERO_ID hero = new HERO_ID();
                hero.idHero = item;
                selList.Add(hero);
            }
            msg.lst = selList.ToArray();
            NetworkMgr.sendData(msg);
        }

        public HeroInfo destEnhanceHero = new HeroInfo();
        public List<string> srcHeroList = new List<string>();
        //强化英雄
        public void enhanceHero(uint destHeroId, uint[] srcHero)
        {
            MSG_HERO_ENHANCE_REQUEST msg = new MSG_HERO_ENHANCE_REQUEST();
            destEnhanceHero = GetCopyHero((int)destHeroId);
            msg.idMajorHero = destHeroId;
            List<HERO_ID> selList = new List<HERO_ID>();
            foreach (uint item in srcHero)
            {
                HERO_ID hero = new HERO_ID();
                hero.idHero = item;
                selList.Add(hero);
            }
            msg.lst = selList.ToArray();
            NetworkMgr.sendData(msg);
        }



        //进化英雄
        public void evolveHero(uint destHero, uint[] srcHero)
        {
            MSG_HERO_EVOLVE_REQUEST msg = new MSG_HERO_EVOLVE_REQUEST();
            msg.idMajorHero = destHero;
            List<HERO_ID> selList = new List<HERO_ID>();
            foreach (uint item in srcHero)
            {
                HERO_ID hero = new HERO_ID();
                hero.idHero = item;
                selList.Add(hero);
            }
            msg.lst = selList.ToArray();
            NetworkMgr.sendData(msg);
        }

        //收藏英雄
        public void SendCollectedHeroMsg(int id, int isCol)
        {
            MSG_HERO_COLLECT_REQUEST msg = new MSG_HERO_COLLECT_REQUEST();
            msg.idHero = (uint)id;
            msg.tag = (byte)isCol;
            NetworkMgr.sendData(msg);
        }

        public void TextDate()
        {
            heroInfoList.Clear();
            dicHeroTeams.Clear();
            ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_HERO);
            
            for (int i = 0; i < 200; ++i)
            {
                HeroInfo hero = new HeroInfo();

                hero.id = 100 + i;
                hero.type = 10007 + i%6;
                hero.btGrowup = 1 + (i % 4); 

                ConfigRow row = table.getRow(DICT_HERO.HERO_TYPEID, hero.type);

                string strTemp = "";
                strTemp = row.getIntValue(DICT_HERO.NAME_ID).ToString();
                hero.name = BaseLib.LanguageMgr.getString(strTemp);

                strTemp = row.getIntValue(DICT_HERO.DESC_ID).ToString();
                hero.desc = BaseLib.LanguageMgr.getString(strTemp);
                hero.iconFile = row.getStringValue(DICT_HERO.ICON_FILE);
                hero.spriteName = row.getIntValue(DICT_HERO.ICON_SPRITE_NAME).ToString();
                hero.fbxFile = row.getIntValue(DICT_HERO.FBX_FILE).ToString();
                hero.portarait = row.getIntValue(DICT_HERO.PORTARAIT).ToString();

                hero.series = row.getIntValue(DICT_HERO.SERIES);
                hero.star = row.getIntValue(DICT_HERO.STAR);
                hero.leader = row.getIntValue(DICT_HERO.LEADER);
                hero.library = row.getIntValue(DICT_HERO.LIBRARY);
                hero.skillCaptian = row.getIntValue(DICT_HERO.SKILL_CAPTAIN);
                hero.skillBase = row.getIntValue(DICT_HERO.SKILL_BASE);
                hero.sourceExp = row.getIntValue(DICT_HERO.EXP);
                hero.coin = row.getIntValue(DICT_HERO.COIN);

                hero.initHP = row.getIntValue(DICT_HERO.INIT_HP);
                hero.initAtk = row.getIntValue(DICT_HERO.INIT_ATK);
                hero.initDef = row.getIntValue(DICT_HERO.INIT_DEF);
                hero.initRecover = row.getIntValue(DICT_HERO.INIT_RECOVER);
                hero.initViolence = row.getIntValue(DICT_HERO.INIT_VIOLENCE);
                hero.baseHP = row.getIntValue(DICT_HERO.BASE_HP);
                hero.baseAtk = row.getIntValue(DICT_HERO.BASE_ATK);
                hero.baseDef = row.getIntValue(DICT_HERO.BASE_DEF);
                hero.baseRecover = row.getIntValue(DICT_HERO.BASE_RECOVER);
                hero.baseViolence = row.getIntValue(DICT_HERO.BASE_VIOLENCE);
                hero.moveSpeed = row.getIntValue(DICT_HERO.MOVE_SPEED);
				hero.movable = row.getIntValue(DICT_HERO.MOVABLE);

                hero.exp = 100 + i;
                hero.level = i;

                hero.hp = hero.initHP + hero.baseHP * hero.level;
                hero.atk = hero.initAtk + hero.baseAtk * hero.level;
                hero.def = hero.initDef + hero.baseDef * hero.level;
                hero.recover = hero.initRecover + hero.baseRecover * hero.level;

                hero.skillBaseName = "神之爆破";
                hero.skillBaseDesc = "对群体敌人进行超强光属性12连击";
                hero.skillCaptianName = "五光神之力";
                hero.skillCaptianDesc = "角色超过五个属性攻击力上升50%";
                hero.skillLevel = 1;
                

                if (i < 3)
                {
                    hero.equipId = 43001 + i;
                }

                if (i < 5)
                {
                    hero.fight = true;
                }

                heroInfoList.Add(hero);
            }

            //队伍信息
            for(int i = 0; i < 10; ++i)
            {
                TeamInfo ti = new TeamInfo();

                ti.troopId = i;
                ti.leaderPos = 0;
                ti.friendPos = 5;
                ti.pos1HeroId = 107;
                ti.pos2HeroId = 108;
                ti.pos3HeroId = 109;
                ti.pos4HeroId = 110;
                ti.pos5HeroId = 111;
                ti.pos6HeroId = 0;

                dicHeroTeams.Add(i, ti);
            }

        }

        public string GetPortaraitByTypeId(int id)
        {
            string strTemp = "";

            ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_HERO);
            if (table != null)
            {
                ConfigRow row = table.getRow(DICT_HERO.HERO_TYPEID, id);
                strTemp = row.getStringValue(DICT_HERO.PORTARAIT);
            }

            return strTemp;
        }

        //排序
        public void SortHeroList(int nSortFun)
        {
            switch (nSortFun)
            {
                case 0:
                    heroInfoList.Sort(SortByLevel);
                    break;
                case 1:
                    heroInfoList.Sort(SortByStar);
                    break;
            }
        }

        protected int SortByStar(HeroInfo a, HeroInfo b)
        {
            int astar = a.star;
            int bstar = b.star;
            int result = astar.CompareTo(bstar);

            if (result > 0)
            {
                result = -1;
            }
            else if (result < 0)
            {
                result = 1;
            }
            else
            {
                result = a.id.CompareTo(b.id);
            }

            return result;
        }

        protected int SortByLevel(HeroInfo a, HeroInfo b)
        {
            int result = a.level.CompareTo(b.level);

            if (result > 0)
            {
                result = -1;
            }
            else if (result < 0)
            {
                result = 1;
            }
            else
            {
                result = a.id.CompareTo(b.id);
            }

            return result;
        }

        public HeroInfo GetHeroInfoById(int id)
        {
            foreach (HeroInfo info in heroInfoList)
            {
                if (info.id == id)
                {
                    return info;
                }
            }

            return null;
        }

        public HeroInfo GetCopyHero(int id)
        {
            HeroInfo hi = new HeroInfo();

            foreach (HeroInfo info in heroInfoList)
            {
                if (info.id == id)
                {
                    hi.id = info.id;
                    hi.type = info.type;
                    hi.btGrowup = info.btGrowup;
                    hi.level = info.level;
                    hi.skillLevel = info.skillLevel;
                    hi.exp = info.exp;
                    hi.equipId = info.equipId;
                    hi.btCollected = info.btCollected;

                    hi.InitDict(hi.type);
                }
            }

            return hi;
        }

        public List<int> evlSrcList = new List<int>();

        protected bool _isInEvlSrcList(int id)
        {
            foreach (int idhero in evlSrcList)
            {
                if (id == idhero)
                {
                    return true;
                }
            }
            
            return false;
        }

        public HeroInfo GetHeroInfoByTypeId(int type)
        {
            foreach (HeroInfo info in heroInfoList)
            {
                if (_isInEvlSrcList(info.id))
                {
                    continue;
                }
                    
                if (info.type == type)
                {
                    return info;
                }
            }

            return null;
        }

        public bool DelHeroById(int heroId)
        {
            foreach (HeroInfo info in heroInfoList)
            {
                if (info.id == heroId)
                {
                    heroInfoList.Remove(info);
                    return true;
                }
            }

            return false;
        }

        //成长类型
        public string GetGrowupStr(int nGrowup)
        {
            string strGrowup = "";

            switch (nGrowup)
            {
                case 1: //攻击型
                    strGrowup = BaseLib.LanguageMgr.getString("66361103");
                    break;
                case 2://防御型
                    strGrowup = BaseLib.LanguageMgr.getString("66361104");
                    break;
                case 3://体力型
                    strGrowup = BaseLib.LanguageMgr.getString("66361105");
                    break;
                case 4://回复型
                    strGrowup = BaseLib.LanguageMgr.getString("66361106");
                    break;
                case 5://全能型
                    strGrowup = BaseLib.LanguageMgr.getString("66361107");
                    break;

            }

            return strGrowup;
        }

        //队伍
        public int nCurTeamId = 0;
        public Dictionary<int, TeamInfo> dicHeroTeams = new Dictionary<int, TeamInfo>();
        public Dictionary<int, HeroInfo> dicCurHeros = new Dictionary<int, HeroInfo>();
        public Dictionary<int, HeroInfo> dicFightHeros = new Dictionary<int, HeroInfo>();

        public void ChangeTeamerById(int idGround, int idHero, int idTroop = 0)
        {
            int beforeId = 0;

            switch (idGround)
            {
                case 0:
                    beforeId = dicHeroTeams[idTroop + 1].pos1HeroId;
                    dicHeroTeams[idTroop + 1].pos1HeroId = idHero;
                    break;
                case 1:
                    beforeId = dicHeroTeams[idTroop + 1].pos2HeroId;
                    dicHeroTeams[idTroop + 1].pos2HeroId = idHero;
                    break;
                case 2:
                    beforeId = dicHeroTeams[idTroop + 1].pos3HeroId;
                    dicHeroTeams[idTroop + 1].pos3HeroId = idHero;
                    break;
                case 3:
                    beforeId = dicHeroTeams[idTroop + 1].pos4HeroId;
                    dicHeroTeams[idTroop + 1].pos4HeroId = idHero;
                    break;
                case 4:
                    beforeId = dicHeroTeams[idTroop + 1].pos5HeroId;
                    dicHeroTeams[idTroop + 1].pos5HeroId = idHero;
                    break;
                case 5:
                    beforeId = dicHeroTeams[idTroop + 1].pos6HeroId;
                    dicHeroTeams[idTroop + 1].pos6HeroId = idHero;
                    break;
            }

            SetOutFight(beforeId);
            SetInFight(idHero);

            if (idHero == 0)
            {
                dicHeroTeams[idTroop + 1].friendPos = idGround + 1;
            }
        }

        public void DelTeamerById(int idGround, int idHero, int idTroop = 0)
        {
            switch (idGround)
            {
                case 0:
                    DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams[idTroop + 1].pos1HeroId = 0;
                    break;
                case 1:
                    DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams[idTroop + 1].pos2HeroId = 0;
                    break;
                case 2:
                    DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams[idTroop + 1].pos3HeroId = 0;
                    break;
                case 3:
                    DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams[idTroop + 1].pos4HeroId = 0;
                    break;
                case 4:
                    DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams[idTroop + 1].pos5HeroId = 0;
                    break;
                case 5:
                    DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams[idTroop + 1].pos6HeroId = 0;
                    break;

            }

            SetOutFight(idHero);
        }

        public void SetInFight(int heroId)
        {
            foreach(HeroInfo hi in heroInfoList)
            {
                if (hi.id == heroId)
                {
                    hi.fight = true;
                }
            }
        }

        public void SetOutFight(int heroId)
        {
            foreach (HeroInfo hi in heroInfoList)
            {
                if (hi.id == heroId)
                {
                    hi.fight = false;
                }
            }
        }

        //领导力
        public int GetTeamLeader(int idTeam)
        {
            int leader = 0;

            if (!dicHeroTeams.ContainsKey(idTeam))
            {
                return leader;
            }


            HeroInfo hi = null;

            if (dicHeroTeams[idTeam].GetHeroCount() == 0)
            {
                return leader;
            }

            if (dicHeroTeams[idTeam].pos1HeroId != 0)
            {
                hi = GetHeroInfoById(dicHeroTeams[idTeam].pos1HeroId);
                
                if (hi != null)
                {
                    leader += hi.leader;
                }
                
            }

            if (dicHeroTeams[idTeam].pos2HeroId != 0)
            {
                hi = GetHeroInfoById(dicHeroTeams[idTeam].pos2HeroId);

                if (hi != null)
                {
                    leader += hi.leader;
                }
            }

            if (dicHeroTeams[idTeam].pos3HeroId != 0)
            {
                hi = GetHeroInfoById(dicHeroTeams[idTeam].pos3HeroId);

                if (hi != null)
                {
                    leader += hi.leader;
                }

            }

            if (dicHeroTeams[idTeam].pos4HeroId != 0)
            {
                hi = GetHeroInfoById(dicHeroTeams[idTeam].pos4HeroId);

                if (hi != null)
                {
                    leader += hi.leader;
                }
            }

            if (dicHeroTeams[idTeam].pos5HeroId != 0)
            {
                hi = GetHeroInfoById(dicHeroTeams[idTeam].pos5HeroId);

                if (hi != null)
                {
                    leader += hi.leader;
                }
            }

            if (dicHeroTeams[idTeam].pos6HeroId != 0)
            {
                hi = GetHeroInfoById(dicHeroTeams[idTeam].pos6HeroId);

                if (hi != null)
                {
                    leader += hi.leader;
                }
            }

            return leader;
        }

        public Dictionary<int, HeroInfo> GetCurHeros()
        {
            dicCurHeros.Clear();
            int nFriendPos = GetCurFriendPos();

            if (nFriendPos > 0)
            {
                HeroInfo hi1 = GetHeroInfoById(dicHeroTeams[nCurTeamId + 1].pos1HeroId);
                dicCurHeros.Add(0, hi1);
            }

            if (nFriendPos > 1)
            {
                HeroInfo hi2 = GetHeroInfoById(dicHeroTeams[nCurTeamId + 1].pos2HeroId);
                dicCurHeros.Add(1, hi2);
            }
            else if (nFriendPos < 1)
            {
                HeroInfo hi2 = GetHeroInfoById(dicHeroTeams[nCurTeamId + 1].pos2HeroId);
                dicCurHeros.Add(0, hi2);
            }

            if (nFriendPos > 2)
            {
                HeroInfo hi3 = GetHeroInfoById(dicHeroTeams[nCurTeamId + 1].pos3HeroId);
                dicCurHeros.Add(2, hi3);
            }
            else if (nFriendPos < 2)
            {
                HeroInfo hi3 = GetHeroInfoById(dicHeroTeams[nCurTeamId + 1].pos3HeroId);
                dicCurHeros.Add(1, hi3);
            }

            if (nFriendPos > 3)
            {
                HeroInfo hi4 = GetHeroInfoById(dicHeroTeams[nCurTeamId + 1].pos4HeroId);
                dicCurHeros.Add(3, hi4);
            }
            else if (nFriendPos < 3)
            {
                HeroInfo hi4 = GetHeroInfoById(dicHeroTeams[nCurTeamId + 1].pos4HeroId);
                dicCurHeros.Add(2, hi4);
            }

            if (nFriendPos > 4)
            {
                HeroInfo hi5 = GetHeroInfoById(dicHeroTeams[nCurTeamId + 1].pos5HeroId);
                dicCurHeros.Add(4, hi5);
            }
            else if (nFriendPos < 4)
            {
                HeroInfo hi5 = GetHeroInfoById(dicHeroTeams[nCurTeamId + 1].pos5HeroId);
                dicCurHeros.Add(3, hi5);
            }

            if (nFriendPos > 5)
            {
                HeroInfo hi6 = GetHeroInfoById(dicHeroTeams[nCurTeamId + 1].pos6HeroId);
                dicCurHeros.Add(5, hi6);
            }
            else if (nFriendPos < 5)
            {
                HeroInfo hi6 = GetHeroInfoById(dicHeroTeams[nCurTeamId + 1].pos6HeroId);
                dicCurHeros.Add(4, hi6);
            }

            
            return dicCurHeros;
        }

        HeroInfo InitTempHero(int heroId,int type,int btGrowup,int level,int skillLevel,int exp,int equipId,int btCollected)
        {
            HeroInfo hero = new HeroInfo();
            hero.id = heroId;
            hero.type = type;
            hero.btGrowup = btGrowup;
            hero.level = level;
            hero.skillLevel = skillLevel;
            hero.exp = exp;
            hero.equipId = equipId;
            hero.btCollected = btCollected;
            hero.InitDict(hero.type);
            return hero;
        }

        Dictionary<int,HeroInfo> dicFightHeros1 = new Dictionary<int, HeroInfo>();

        public Dictionary<int,HeroInfo> GetFightHeros1()
        {
            dicFightHeros1 = new Dictionary<int, HeroInfo>();
			dicFightHeros1.Add(0,InitTempHero(0,10325,1,10,1,100,0,0));//10325
			dicFightHeros1.Add(1,InitTempHero(1,10282,1,10,1,100,0,0));
//			dicFightHeros1.Add(2,InitTempHero(2,10325,1,10,1,100,0,0));
//			dicFightHeros1.Add(3,InitTempHero(3,10282,2,10,1,100,0,0));
//			dicFightHeros1.Add(4,InitTempHero(4,10282,2,10,1,100,0,0));
            return dicFightHeros1;
        }

		public Dictionary<int,HeroInfo> GetFightHeros2()
		{
			dicFightHeros1 = new Dictionary<int, HeroInfo>();
//			GameObject go = SpawnManager.SingleTon().heroPrefabs[Random.Range(0,SpawnManager.SingleTon().heroPrefabs.Count)];
			dicFightHeros1.Add(0,InitTempHero(0,10043,1,10,1,100,0,0));
//			go = SpawnManager.SingleTon().heroPrefabs[Random.Range(0,SpawnManager.SingleTon().heroPrefabs.Count)];
			dicFightHeros1.Add(1,InitTempHero(1,10044,1,10,1,100,0,0));
//			go = SpawnManager.SingleTon().heroPrefabs[Random.Range(0,SpawnManager.SingleTon().heroPrefabs.Count)];
			dicFightHeros1.Add(2,InitTempHero(2,10045,1,10,1,100,0,0));
//			go = SpawnManager.SingleTon().heroPrefabs[Random.Range(0,SpawnManager.SingleTon().heroPrefabs.Count)];
			dicFightHeros1.Add(3,InitTempHero(3,10046,1,10,1,100,0,0));
//			go = SpawnManager.SingleTon().heroPrefabs[Random.Range(0,SpawnManager.SingleTon().heroPrefabs.Count)];
			dicFightHeros1.Add(4,InitTempHero(4,10047,1,10,1,100,0,0));
			return dicFightHeros1;
		}

        public Dictionary<int, HeroInfo> GetFightHeros()
        {
            dicFightHeros.Clear();

            HeroInfo hi1 = GetHeroInfoById(dicHeroTeams[nCurTeamId + 1].pos1HeroId);
			if (hi1 != null)
            	dicFightHeros.Add(0, hi1);

            HeroInfo hi2 = GetHeroInfoById(dicHeroTeams[nCurTeamId + 1].pos2HeroId);
			if (hi2 != null)
				dicFightHeros.Add(1, hi2);

            HeroInfo hi3 = GetHeroInfoById(dicHeroTeams[nCurTeamId + 1].pos3HeroId);
			if (hi3 != null)
            	dicFightHeros.Add(2, hi3);

            HeroInfo hi4 = GetHeroInfoById(dicHeroTeams[nCurTeamId + 1].pos4HeroId);
			if (hi4 != null)
            	dicFightHeros.Add(3, hi4);

            HeroInfo hi5 = GetHeroInfoById(dicHeroTeams[nCurTeamId + 1].pos5HeroId);
			if (hi5 != null)
            	dicFightHeros.Add(4, hi5);

            HeroInfo hi6 = GetHeroInfoById(dicHeroTeams[nCurTeamId + 1].pos6HeroId);
			if (hi6 != null)
            	dicFightHeros.Add(5, hi6);

			int nPos = GetCurFriendPos();			
			dicFightHeros[nPos] = currfriendHero;

            return dicFightHeros;
        }

        public int GetCurLeaderPos()
        {
            return dicHeroTeams[nCurTeamId + 1].leaderPos;
        }

        public int GetCurFriendPos()
        {
            return dicHeroTeams[nCurTeamId + 1].friendPos - 1;
        }

		public HeroInfo currfriendHero = null;
		public int currUserID = 0;

		public void SetFriendHero(HeroInfo hi)
        {
         //   int nPos = GetCurFriendPos();

         //   dicFightHeros[nPos] = hi;

			currfriendHero = hi;
        }
			
		public HeroInfo getFriendHero()
		{
			return currfriendHero;
		}

		public void setFriendUserID(int userid)
		{		
			currUserID = userid;
		}
		
		public int getFriendUserID()
		{		
			return currUserID;
		}

        public void addNewTeam()
        {
            TeamInfo team = new TeamInfo();

            team.friendPos = 6;


            for (int i = 1; i < 11; ++i)
            {
                if (!dicHeroTeams.ContainsKey(i))
                {
                    dicHeroTeams.Add(i, team);

                    break; 
                }
            }
            
        }


        //加入,取消收藏
        public void CollectedHero(int heroId, bool isCollected)
        {
            int nCount = heroInfoList.Count;

            for (int i = 0; i < nCount; ++i)
            {
                if (heroInfoList[i].id == heroId)
                {

                    if (isCollected)
                    {
                        heroInfoList[i].btCollected = 1;
                        SendCollectedHeroMsg(heroId, 1);
                        return;
                    }
                    else
                    {
                        heroInfoList[i].btCollected = 0;
                        SendCollectedHeroMsg(heroId, 0);
                        return;
                    }
                }
            }
        }


        //是否可以进化
        public bool isEvolvability(int TypeId)
        {
            ConfigTable evolveTable = ConfigMgr.getConfig(CONFIG_MODULE.DICT_HERO_EVOLVE);
            ConfigRow evolveRow = evolveTable.getRow(DICT_HERO_EVOLVE.HERO_TYPEID, TypeId);

            if (evolveRow != null)
            {
                return true;
            }
            
            return false;
        }

        //某类型英雄数量
        public int CountHeroType(int nTypeId)
        {
            int nCount = 0;

            foreach (HeroInfo info in heroInfoList)
            {
                if (info.type == nTypeId)
                {
                    ++nCount;
                }
            }

            return nCount;
        }


        //装备
        public List<EquipInfo> equipList = new List<EquipInfo>();

        public void equipDate()
        {
            equipList.Clear();
            ItemInfo[] items = DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).getItemList(ITEM_SORT.stone);

            for (int i = 0; i < items.Length; ++i)
            {
                EquipInfo ei = new EquipInfo();

                ei.id = (int)items[i].id;
                ei.name = items[i].name;
                ei.icon = items[i].icon;
                ei.effect = items[i].desc;

                equipList.Add(ei);
            }

            foreach (HeroInfo hi in heroInfoList)
            {
                if (hi.equipId != 0)
                {
                    HeroEquipByid(hi.equipId, hi.id);
                }
            }
        }

        public void HeroEquipByid(int EquipId, int HeroId)
        {
            foreach(EquipInfo ei in equipList)
            {
                if (ei.id == EquipId)
                {
                    ei.heroId = HeroId;

                }
            }
        }

        public void HeroRemoveEquip(int EquipId, int HeroId)
        {
            foreach (EquipInfo ei in equipList)
            {
                if (ei.id == EquipId)
                {
                    ei.heroId = 0;

                }
            }
        }

        public string GetEquipNameById(int EquipId)
        {
            string strName = "";

            foreach (EquipInfo ei in equipList)
            {
                if (ei.id == EquipId)
                {
                    strName = ei.name;

                }
            }


            return strName;
        }

        public bool isEquipped(int EquipId)
        {
            foreach (EquipInfo ei in equipList)
            {
                if (ei.id == EquipId)
                {
                    if (ei.heroId > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }


        //召唤效果显示
        public void ShowCallHeroEffect()
        {
            UIPanelPartner.CallHeroBrushs.SetActive(true);
            Transform befModel = UIPanelPartner.CallHeroBrushs.transform.FindChild("Brushs").Find("model");

            if (befModel != null)
            {
                UnityEngine.Object.Destroy(befModel.gameObject);

            }

            GameObject modelPrefab = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroPrefabs[changeHeroList[0].fbxFile];
            GameObject model = NGUITools.AddChild(UIPanelPartner.CallHeroBrushs.transform.FindChild("Brushs").gameObject, modelPrefab);
            model.name = "model";
            model.transform.localPosition = new Vector3(-14.5f, 35f, -3.5f);
            model.transform.localScale = new UnityEngine.Vector3(0.9f, 0.9f, 1);
            model.SetActive(false);

        }

        public void HideCallHeroEffect()
        {
            Brush brush = UIPanelPartner.CallHeroBrushs.transform.FindChild("Brushs").GetComponent<Brush>();
            brush.Reset();

            UIPanelPartner.CallHeroBrushs.SetActive(false);
        }

        //强化效果显示
        public void ShowStrengthenEffect()
        {
            UIPanelPartner.StrengthenEffect.SetActive(true);

            StrengthenEffect se = UIPanelPartner.StrengthenEffect.GetComponent<StrengthenEffect>();
            se.successType = nEnhanceResult;
            se.StartPlay();
        }

        public void HideStrengthenEffect()
        {
            UIPanelPartner.StrengthenEffect.SetActive(false);
            StrengthenEffect se = UIPanelPartner.StrengthenEffect.GetComponent<StrengthenEffect>();
            se.Reset();

            ShowStrengthenProcess(nEnhanceExp, nEnhanceResult, nEnhanceSkillLv);
        }

        //进化效果显示
        public string strCurModel = "";
        public string strEvlModel = "";
        public void ShowEvolutionEffect()
        {
            UIPanelPartner.Evolution.SetActive(true);

            EvolutionEffect ee = UIPanelPartner.Evolution.GetComponent<EvolutionEffect>();
            ee.StartPlay(strCurModel, strEvlModel);
        }
        //UI界面
        public GameObject panelPartner;
        public PanelPartner UIPanelPartner;
        public PartnerMenu UIpartnerMenu;
        public PartnerDetailMain UIpartnerDetailMain;
        public Illustrations illustrations;
        public SummonPartner summon;
        public FriendPartner friendHero;
        public PartnerDetail UIpartnerDetail;
        public PartnerEdit UIpartnerEdit;

        public void MainShowHeroDetailById(int id)
        {

            panelPartner.SetActive(true);
            UIpartnerMenu.gameObject.SetActive(false);
            UIpartnerDetailMain.isMainUse = true;
            UIpartnerDetailMain.ShowHeroDetail(id);
            UIpartnerDetailMain.gameObject.SetActive(true);
        }

        public void SummonShowHeroDetailById(int id)
        {
            summon.gameObject.SetActive(false);
            UIpartnerDetail.gameObject.SetActive(true);
            UIpartnerDetail.isSummonUse = true;
            UIpartnerDetail.ShowHeroDetail(id, summon.gameObject);
        }

        public void ReturnSummon()
        {
            summon.gameObject.SetActive(true);
            UIpartnerDetail.gameObject.SetActive(false);
        }

        public void MainShowHeroMenu()
        {
            panelPartner.SetActive(true);
            PanelPartner pp = panelPartner.GetComponent<PanelPartner>();
            pp.InitUI();
            UIpartnerMenu.gameObject.SetActive(true);
        }

        public void ShowIllustrations()
        {
            panelPartner.SetActive(true);
            UIpartnerMenu.gameObject.SetActive(false);
            illustrations.gameObject.SetActive(true);
            illustrations.ShowHero();
        }

        public void ShowSummonPartner()
        {
            panelPartner.SetActive(true);
            PanelPartner pp = panelPartner.GetComponent<PanelPartner>();
            pp.InitUI();
            UIpartnerMenu.gameObject.SetActive(false);
            pp.partnerSummon.SetActive(true);
        }

        public void ShowMultiSummon()
        {
            panelPartner.SetActive(true);
            PanelPartner pp = panelPartner.GetComponent<PanelPartner>();
            pp.InitUI();
            UIpartnerMenu.gameObject.SetActive(false);


            GameObject multiSummon = pp.partnerUI[21];
            MultiSummon mSummon = multiSummon.GetComponent<MultiSummon>();
            mSummon.ShowHeros();
            multiSummon.SetActive(true);
        }

        public void ShowFriendPartner()
        {
            panelPartner.SetActive(true);
            PanelPartner pp = panelPartner.GetComponent<PanelPartner>();
            pp.InitUI();
            UIpartnerMenu.gameObject.SetActive(false);
            pp.friendPartner.SetActive(true);

            friendHero.ShowHero();
        }

        public void FightShowTeamEdit()
        {
            panelPartner.SetActive(true);
            PanelPartner pp = panelPartner.GetComponent<PanelPartner>();
            pp.InitUI();
            UIpartnerMenu.gameObject.SetActive(false);
            pp.partnerEdit.SetActive(true);

            UIpartnerEdit.isFightShow = true;
            UIpartnerEdit.InitShowTeam();
        }

        public void ArenaShowTeamEdit()
        {
            panelPartner.SetActive(true);
            PanelPartner pp = panelPartner.GetComponent<PanelPartner>();
            pp.InitUI();
            UIpartnerMenu.gameObject.SetActive(false);
            pp.partnerEdit.SetActive(true);

            UIpartnerEdit.isFightShow = false;
            UIpartnerEdit.InitShowTeam();
        }

        public void ShowStrengthenProcess(int nExp,int nResult,int nSkillLvl)
        {
            panelPartner.SetActive(true);
            PanelPartner pp = panelPartner.GetComponent<PanelPartner>();
            pp.InitUI();
            UIpartnerMenu.gameObject.SetActive(false);

            GameObject strengthenProcess = pp.partnerUI[19];
            strengthenProcess.SetActive(true);
            StrengthenProcess sp = strengthenProcess.GetComponent<StrengthenProcess>();
            sp.ShowStrengthen(destEnhanceHero, nExp, nResult, nSkillLvl);
        }

        public void EvolveShowDetail(int idHero)
        {
            panelPartner.SetActive(true);
            PanelPartner pp = panelPartner.GetComponent<PanelPartner>();
            pp.InitUI();
            UIpartnerMenu.gameObject.SetActive(false);

            GameObject selectEvolution = pp.partnerUI[13];
            SelectEvolution se = selectEvolution.GetComponent<SelectEvolution>();
            se.ShowHeroBrowse();

            UIpartnerDetail.ShowHeroDetail(idHero, selectEvolution);
            UIpartnerDetail.gameObject.SetActive(true);
        }

        public void UpdateEquip()
        {
            panelPartner.SetActive(true);
            PanelPartner pp = panelPartner.GetComponent<PanelPartner>();
            pp.InitUI();
            UIpartnerMenu.gameObject.SetActive(false);

            GameObject PartnerEquip = pp.partnerUI[7];
            PartnerEquip.SetActive(true);
            PartnerEquip pe = PartnerEquip.GetComponent<PartnerEquip>();
            pe.ShowEquip();
        }



        //确认框
        public void ShowComfirmPanel(string strText, UIEventListener.VoidDelegate func)
        {
            panelPartner.SetActive(true);
            PanelPartner pp = panelPartner.GetComponent<PanelPartner>();

            GameObject comfirmPanel = pp.partnerUI[22];
            comfirmPanel.SetActive(true);

            ComfirmPanel cp = comfirmPanel.GetComponent<ComfirmPanel>();
            cp.SetComfirmText(strText);
            cp.SetComfirmFunc(func);
        }
    }
}