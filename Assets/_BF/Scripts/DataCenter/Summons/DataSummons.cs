using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BaseLib;

namespace DataCenter
{
    public class DataSummons : DataModule
    {
        Dictionary<uint, SummonInfo> _summonsList = new Dictionary<uint, SummonInfo>();
        
        public DataSummons()
        {
        }

        public override void release()
        {
        }

        public override bool init()
        {
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_SUMMON_LST, onRecvList, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_SUMMON_EVOLVE, onEvolve, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_SUMMON_CHANGE_EVENT, onChageEvent, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_SUMMON_UPLVL, onUplv, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_SUMMON_QUALITY, onQuality, (int)DataCenter.EVENT_GROUP.packet);

            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_SUMMON_UNLOCK, onUnlock, (int)DataCenter.EVENT_GROUP.packet);

            //英雄阵型
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_HERO_FORMATION_TRIAL, onSetFormationTrial, (int)DataCenter.EVENT_GROUP.packet);

            return true;
        }

        void onRecvList(int nEvent, System.Object param)
        {
            _summonsList.Clear();
            MSG_CLIENT_SUMMON_LST_EVENT msg = (MSG_CLIENT_SUMMON_LST_EVENT)param;
            foreach (SUMMON_INFO item in msg.lst)
            {
                SummonInfo info = new SummonInfo();
                info.init(item);
                this._summonsList[info.type] = info;
            }
            EventSystem.sendEvent((int)EVENT_MAINUI.summonRecvList, null, (int)EVENT_GROUP.mainUI);
        }

        public SummonInfo[] getSummonList()
        {
            List<SummonInfo> theList = new List<SummonInfo>();
            foreach (KeyValuePair<uint, SummonInfo> item in this._summonsList)
                theList.Add(item.Value);
            return theList.ToArray();
        }

//         public SummonInfo getSummon(uint id)
//         {
//             if (this._summonsList.ContainsKey(id))
//                 return this._summonsList[id];
//             return null;
//         }

        public SummonInfo getSummonById(uint id)
        {
            foreach (KeyValuePair<uint, SummonInfo> si in this._summonsList)
            {
                if (si.Value.id == id)
                {
                    return si.Value;
                }
            }

            return null;
        }

        public uint SelectSummonId = 0;
        public SummonInfo GetSelectSummon()
        {
            if (SelectSummonId != 0 && this._summonsList.ContainsKey(SelectSummonId))
            {
                return this._summonsList[SelectSummonId];
            }

            return null;
        }


        public void evolve(uint summonid, uint[] heroList)
        {
            MSG_CLIENT_SUMMON_EVOLVE_REQUEST request = new MSG_CLIENT_SUMMON_EVOLVE_REQUEST();
            request.idSummon = summonid;
            request.usCnt = (ushort)heroList.Length;
            request.lst = heroList;
            NetworkMgr.sendData(request);
        }

        void onEvolve(int nEvent, System.Object param)
        {
            MSG_CLIENT_SUMMON_EVOLVE_RESPONSE msg = (MSG_CLIENT_SUMMON_EVOLVE_RESPONSE)param;
            if (msg.isSucc(msg.errCode))
                EventSystem.sendEvent((int)EVENT_MAINUI.summonEvolve, msg.idEvolveSummon, (int)EVENT_GROUP.mainUI);
            else
                EventSystem.sendEvent((int)EVENT_MAINUI.summonEvolve, 0, (int)EVENT_GROUP.mainUI);
        }


        public SummonInfo newSummon = null;
        void onChageEvent(int nEvent, System.Object param)
        {
            MSG_CLIENT_SUMMON_CHANGE_EVENT msg = (MSG_CLIENT_SUMMON_CHANGE_EVENT)param;
            if (msg.tag == 1)// 0:新增	1:删除
                this._summonsList.Remove(msg.idSummon);
            else
            {
                SummonInfo info = new SummonInfo();
                info.id = msg.idSummon;
                info.type = msg.idSummonType;
                this._summonsList[info.id] = info;


                SUMMON_INFO si = new SUMMON_INFO();
                si.idSummon = msg.idSummon;
                si.idSummonType = msg.idSummonType;
                si.wLevel = 1;
                info.init(si);

                newSummon = info;
            }
        }

        public void updateSummon(uint costSoul)
        {
            MSG_CLIENT_SUMMON_UPLVL_REQUEST request = new MSG_CLIENT_SUMMON_UPLVL_REQUEST();
            request.unSoul = costSoul;
            request.idSummon = GetSelectSummon().id;
            NetworkMgr.sendData(request);
        }

        void onUplv(int nEvent, System.Object param)
        {
            MSG_CLIENT_SUMMON_UPLVL_RESPONSE msg = (MSG_CLIENT_SUMMON_UPLVL_RESPONSE)param;
            if (msg.isSucc((int)msg.errCode))
            {
                SummonInfo info = this.getSummonById(msg.idSummon);
                if (info != null)
                {
                    info.level = (ushort)msg.unLvl;
                    info.curSoul = msg.unSoul;
                }
            }
            EventSystem.sendEvent((int)EVENT_MAINUI.summonUpLv, msg.idSummon, (int)EVENT_GROUP.mainUI);
        }

        public void updateQuality(uint id)
        {
            MSG_CLIENT_SUMMON_QUALITY_REQUEST request = new MSG_CLIENT_SUMMON_QUALITY_REQUEST();
            request.idSummon = id;
            NetworkMgr.sendData(request);
        }

        void onQuality(int nEvent, System.Object param)
        {
            MSG_CLIENT_SUMMON_QUALITY_RESPONSE msg = (MSG_CLIENT_SUMMON_QUALITY_RESPONSE)param;
            if (msg.isSucc(msg.errCode))
            {
                SummonInfo info = this.getSummonById(msg.idSummon);
                if (info != null)
                {
                    
                    info.quality = msg.btQuality;
                }
            }
            EventSystem.sendEvent((int)EVENT_MAINUI.summonQuality, msg.idSummon, (int)EVENT_GROUP.mainUI);
        }

        public ulong GetNeedSoulByLevel(int nLevel)
        {
            ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_SUMMON_LEVEL);
            ConfigRow row = table.getRow(DICT_SUMMON_LEVEL.LEVEL, nLevel + 1);

            int soul = row.getIntValue(DICT_SUMMON_LEVEL.SOUL);

            return (ulong)soul;
        }


        public void SendSkillUplevel(byte btTag)
        {
            MSG_CLIENT_SUMMON_SKILL_UPLVL_REQUEST msg = new MSG_CLIENT_SUMMON_SKILL_UPLVL_REQUEST();
            msg.idSummon = SelectSummonId;
            msg.btTag = btTag;
            NetworkMgr.sendData(msg);
        }


        public void SendUnlockMsg()
        {
            MSG_CLIENT_SUMMON_UNLOCK_REQUEST msg = new MSG_CLIENT_SUMMON_UNLOCK_REQUEST();
            msg.idSummonType = SelectSummonId;
            NetworkMgr.sendData(msg);
        }

        void onUnlock(int nEvent, System.Object param)
        {
            MSG_CLIENT_SUMMON_UNLOCK_RESPONSE msg = (MSG_CLIENT_SUMMON_UNLOCK_RESPONSE)param;

            if (msg.isSucc(msg.errCode))
            {
                
            }
        }

        //挑战召唤兽队伍
        public void SendHeroFormationTrialMsg()
        {
            MSG_HERO_FORMATION_TRIAL_REQUEST msg = new MSG_HERO_FORMATION_TRIAL_REQUEST();

            List<TROOP> teamList = new List<TROOP>();
            msg.lst = null;

            for (int i = 0; i < dicHeroTeams.Count; ++i)
            {
                TeamInfo team = dicHeroTeams[i + 1];
                TROOP newTeam = new TROOP();

                newTeam.idTroop = (byte)(i + 11);

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

                if (newTeam.idPos1 == 0 && newTeam.idPos2 == 0
                    && newTeam.idPos3 == 0 && newTeam.idPos4 == 0
                    && newTeam.idPos5 == 0 && newTeam.idPos6 == 0)
                {
                    continue;
                }


                teamList.Add(newTeam);
            }
            msg.usCnt = (ushort)teamList.Count;
            msg.lst = new TROOP[msg.usCnt];
            msg.lst = teamList.ToArray();

            NetworkMgr.sendData(msg);
        }

        void onSetFormationTrial(int nEvent, System.Object param)
        {
            MSG_HERO_FORMATION_TRIAL_RESPONSE msg = (MSG_HERO_FORMATION_TRIAL_RESPONSE)param;

            if (msg.err == 100000)
            {
                Debug.Log("HeroFormationSuc");
            }

        }

        public Dictionary<int, TeamInfo> dicHeroTeams = new Dictionary<int, TeamInfo>();
        public int nMaxLeader = 200;

        public void InitTeamData()
        {
            nMaxLeader = (int)DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.maxLeader * 2;
            
            dicHeroTeams.Clear();

            for (int i = 11; i < 14; ++i)
            {
                if (DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams.ContainsKey(i))
                {
                    TeamInfo ti = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams[i];
                    ti.OrderTeamer();
                    dicHeroTeams.Add(i - 10, ti);
                    DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams.Remove(i);
                }
                else
                {
                    TeamInfo ti = new TeamInfo();
                    ti.friendPos = 6;
                    dicHeroTeams.Add(i - 10, ti);
                }
                
            }
        }


        public bool IsOverLeader(int curId, int teamId,int befId)
        {
            nMaxLeader = (int)DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.maxLeader * 2;
            int leader = 0;

            HeroInfo curHero = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(curId);

            if (curHero != null)
            {
                leader = curHero.leader;
            }

            int nCurLeader = 0;

            for (int i = 1; i < 4; ++i)
            {
                nCurLeader += dicHeroTeams[i].GetTeamLeader();
            }

            int nBefleader = 0;

            HeroInfo befHero = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(befId);

            if (befHero != null)
            {
                nBefleader = befHero.leader;
            }

            if (leader + nCurLeader - nBefleader > nMaxLeader)
            {
                return true;
            }
            
            return false;
        }

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
                    dicHeroTeams[idTroop + 1].pos1HeroId = 0;
                    break;
                case 1:
                    dicHeroTeams[idTroop + 1].pos2HeroId = 0;
                    break;
                case 2:
                    dicHeroTeams[idTroop + 1].pos3HeroId = 0;
                    break;
                case 3:
                    dicHeroTeams[idTroop + 1].pos4HeroId = 0;
                    break;
                case 4:
                    dicHeroTeams[idTroop + 1].pos5HeroId = 0;
                    break;
                case 5:
                    dicHeroTeams[idTroop + 1].pos6HeroId = 0;
                    break;

            }

        }

        //是否已上场
        public bool isOnGround(int idHero)
        {
            for (int i = 1; i < 4; ++i)
            {
                if (dicHeroTeams[i].pos1HeroId == idHero)
                    return true;
                if (dicHeroTeams[i].pos2HeroId == idHero)
                    return true;
                if (dicHeroTeams[i].pos3HeroId == idHero)
                    return true;
                if (dicHeroTeams[i].pos4HeroId == idHero)
                    return true;
                if (dicHeroTeams[i].pos5HeroId == idHero)
                    return true;
                if (dicHeroTeams[i].pos6HeroId == idHero)
                    return true;
            }
            
            
            return false;
        }

        //添加好友英雄
        public void AddfriendHero(int team, int userId)
        {
            foreach (HelpHeroInfo hhi in heroList)
            {
                if (hhi.userid == userId)
                {
                    dicHeroTeams[team].AddFriendHero(hhi.hero);
                }
            }
        }

        public void ClearFriendHero()
        {
            for (int i = 1; i < 4; ++i)
            {
                dicHeroTeams[i].friendHero = null;
            }
        }

        //测试好友数据
        public List<HelpHeroInfo> heroList = new List<HelpHeroInfo>();

        public class HelpHeroInfo
        {
            public int userid;
            public string userName;
            public int userLevel;
            public int isFriend;

            public HeroInfo hero = new HeroInfo();
        }

        private void TextData()
        {
            heroList.Clear();
            //BattleFirend[] battleFriend = DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).friendList;

            for (int i = 0; i < 10; ++i)
            {
                HelpHeroInfo hero = new HelpHeroInfo();

                hero.userid = 1000 + i;
                hero.userName = "好友" + i.ToString();
                hero.userLevel = i;

                hero.hero.id = 100 + i;
                hero.hero.type = 10007 + i % 6;
                hero.hero.btGrowup = 1 + (i % 4);
                hero.hero.level = 10 + i;
                hero.hero.skillLevel = i;
                //hero.hero.equipId = battleFriend[i].friendHero.equipId;
                hero.hero.equipId = 0;
                hero.hero.InitDict(hero.hero.type);

                heroList.Add(hero);
            }
        }


        //UI界面
        public PanelSummons panelSummons;

        public void ShowPanelSummons()
        {
            panelSummons.InitUI();
            panelSummons.gameObject.SetActive(true);
        }

        public void ShowSummonsBrowse()
        {
            panelSummons.ShowPageByIndex(1);
            SummonsBrowse sb = panelSummons.SummonsPages[1].GetComponent<SummonsBrowse>();
            sb.nState = 1;
        }

        public void ShowSummonsDetail(GameObject backPage)
        {
            panelSummons.ShowPageByIndex(2);
            SummonsDetail sd = panelSummons.SummonsPages[2].GetComponent<SummonsDetail>();
            sd.backGO = backPage;
            sd.ShowSummon();
        }

        public void ShowSelectStrengthen()
        {
            panelSummons.ShowPageByIndex(1);
            SummonsBrowse sb = panelSummons.SummonsPages[1].GetComponent<SummonsBrowse>();
            sb.nState = 2;
        }

        public void ShowSelectBreak()
        {
            panelSummons.ShowPageByIndex(1);
            SummonsBrowse sb = panelSummons.SummonsPages[1].GetComponent<SummonsBrowse>();
            sb.nState = 3;
        }

        public void ShowStrengthen()
        {
            panelSummons.ShowPageByIndex(3);
        }

        public void ShowResult()
        {
            GameObject resultPage = panelSummons.SummonsPages[4];
            resultPage.SetActive(true);
        }

        public void ShowBreak()
        {
            panelSummons.ShowPageByIndex(5);
        }

        public void ShowBreakResult()
        {
            GameObject resultPage = panelSummons.SummonsPages[6];
            resultPage.SetActive(true);
        }

        public void ShowChallengePage()
        {
            panelSummons.ShowPageByIndex(7);

            TextData();//临时好友数据
        }

        public void ShowOrgTeam()
        {
            //InitTeamData();//初始化召唤院队伍数据
            panelSummons.ShowPageByIndex(8);
        }

        public void ShowReadyChallenge()
        {
            panelSummons.ShowPageByIndex(9);
        }

        public void ShowChanllengeTeam(int nTeam)
        {
            panelSummons.ShowPageByIndex(10);

            ChanllengeTeam ct = panelSummons.SummonsPages[10].GetComponent<ChanllengeTeam>();
            ct.InitShowTeam(nTeam);
        }

        public void ShowSelectAssistant()
        {
            panelSummons.ShowPageByIndex(11);
        }

        public void ShowUnlock()
        {
            panelSummons.ShowPageByIndex(12);
        }

        public void ShowStrengthenMenu()
        {
            panelSummons.ShowPageByIndex(13);
        }

        public void ShowSelectQualityStrengthen()
        {
            panelSummons.ShowPageByIndex(1);
            SummonsBrowse sb = panelSummons.SummonsPages[1].GetComponent<SummonsBrowse>();
            sb.nState = 4;
        }

        public void ShowQualityStrengthen()
        {
            panelSummons.ShowPageByIndex(14);
        }
    }
}