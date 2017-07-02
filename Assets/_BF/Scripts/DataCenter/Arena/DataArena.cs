using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BaseLib;

namespace DataCenter
{
    public class DataArena : DataModule
    {
        public uint totalWin;
        public uint totalLose;
        public uint arenaPoint;

        public DataArena()
        {
        }

        public override void release()
        {
        }

        public override bool init()
        {
            totalWin = 0;
            totalLose = 0;

            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_ARENA_BRIEF_INFO_EVENT, onBaseArena, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_ARENA_TARGET, onRecvTarget, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_ARENA_TARGET_HERO_INFO, onRecvTargetHero, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_ARENA_PVP_START, onStart, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_ARENA_PVP_REWARD, onReward, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_ARENA_FRIEND_RANKING, onRecvFriendRank, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_ARENA_TOTAL_RANKING, onRecvTotalRank, (int)DataCenter.EVENT_GROUP.packet);

            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_HERO_FORMATION_ARENA, onSetFormationArena, (int)DataCenter.EVENT_GROUP.packet);
            
            return true;
        }

        void onBaseArena(int nEvent, System.Object param)
        {
            MSG_CLIENT_ARENA_BRIEF_INFO_EVENT msg = (MSG_CLIENT_ARENA_BRIEF_INFO_EVENT)param;
            this.totalWin = msg.u32TotalWin;
            this.totalLose = msg.u32TotalLose;
            this.arenaPoint = msg.u32ArenaPoint;
        }


        public List<ArenaTargetInfo> targetList = new List<ArenaTargetInfo>();
        int nTargetCount = 0;

        void onRecvTarget(int nEvent, System.Object param)
        {
            MSG_CLIENT_ARENA_TARGET_RESPONSE msg = (MSG_CLIENT_ARENA_TARGET_RESPONSE)param;
            targetList.Clear();
            targetList.Capacity = msg.usCnt;
            nTargetCount = msg.usCnt;
            foreach(ARENA_TARGET_TMP item in msg.lst)
            {
                ArenaTargetInfo info = new ArenaTargetInfo();
                info.init(item);
                targetList.Add(info);
            }
            EventSystem.sendEvent((int)EVENT_MAINUI.arenaRecvTargetList, targetList.ToArray(), (int)EVENT_GROUP.mainUI);
        }

        int nTargetIndex = 0;
        public ArenaTargetInfo GetTargetInfo()
        {
            ArenaTargetInfo ati = null;

            if (nTargetIndex >= nTargetCount)
            {
                nTargetIndex = 0;
            }

            ati = targetList[nTargetIndex];

            if (nTargetIndex == nTargetCount - 1)
            {
                SendArenaTargetMsg();
            }


            ++nTargetIndex;

            return ati;
        }


        void onStart(int nEvent, System.Object param)
        {
            MSG_CLIENT_ARENA_PVP_START_RESPONSE msg = (MSG_CLIENT_ARENA_PVP_START_RESPONSE)param;
            EventSystem.sendEvent((int)EVENT_MAINUI.arenaStart, null, (int)EVENT_GROUP.mainUI);
        }

        void onReward(int nEvent, System.Object param)
        {
            MSG_CLIENT_ARENA_PVP_REWARD_EVENT msg = (MSG_CLIENT_ARENA_PVP_REWARD_EVENT)param;

            ArenaReward reward = new ArenaReward();
            reward.init(msg);
            EventSystem.sendEvent((int)EVENT_MAINUI.arenaReward, reward, (int)EVENT_GROUP.mainUI);
        }


        public ArenaTargetHeroList targetHeroList = new ArenaTargetHeroList();
        void onRecvTargetHero(int nEvent, System.Object param)
        {
            targetHeroList.heroList.Clear();
            MSG_CLIENT_ARENA_TARGET_HERO_INFO_EVENT msg = (MSG_CLIENT_ARENA_TARGET_HERO_INFO_EVENT)param;

            targetHeroList.useid = msg.idUser;
            foreach (ARENA_HERO_INFO_TMP item in msg.lst)
            {
                if (item.idHeroType != 0)
                {
                    ArenaTargetHero hero = new ArenaTargetHero();
                    hero.init(item);
                    targetHeroList.heroList.Add(hero);
                }
            }
            EventSystem.sendEvent((int)EVENT_MAINUI.arenaRecvHeroList, targetHeroList, (int)EVENT_GROUP.mainUI);
        } 

        public List<ArenaRankUser> friendRankList = new List<ArenaRankUser>();
        void onRecvFriendRank(int nEvent, System.Object param)
        {
            MSG_CLIENT_ARENA_FRIEND_RANKING_RESPONSE msg = (MSG_CLIENT_ARENA_FRIEND_RANKING_RESPONSE)param;

            if (msg.cbBeginFlag == 1)
            {
                friendRankList.Clear();
            }

            foreach(ARENA_RANKING_TMP item in msg.lst)
            {
                ArenaRankUser user = new ArenaRankUser();
                user.init(item);
                friendRankList.Add(user);
            }
            EventSystem.sendEvent((int)EVENT_MAINUI.arenaFriendRank, friendRankList, (int)EVENT_GROUP.mainUI);


            if (msg.cbEndFlag == 1)
            {
                ShowFriendRank();
            }
        }

        public ArenaRankUserList totalRankList = new ArenaRankUserList();
        void onRecvTotalRank(int nEvent, System.Object param)
        {
            MSG_CLIENT_ARENA_TOTAL_RANKING_RESPONSE msg = (MSG_CLIENT_ARENA_TOTAL_RANKING_RESPONSE)param;

            if (msg.cbBeginFlag == 1)
            {
                totalRankList.theList.Clear();
            }

            totalRankList.selfRank = (int)msg.u32UserRanking;
            foreach (ARENA_RANKING_TMP item in msg.lst)
            {
                ArenaRankUser user = new ArenaRankUser();
                user.init(item);
                totalRankList.theList.Add(user);
            }
            EventSystem.sendEvent((int)EVENT_MAINUI.arenaTotalRank, totalRankList, (int)EVENT_GROUP.mainUI);

            if (msg.cbEndFlag == 1)
            {
                ShowTotalRank();
            }
        }

        public void SendArenaTargetMsg()
        {
            MSG_CLIENT_ARENA_TARGET_REQUEST msg = new MSG_CLIENT_ARENA_TARGET_REQUEST();
            msg.idUser = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.id;

            NetworkMgr.sendData(msg);
        }

        public void SendArenaPVPStartMsg(ulong targetId, bool isRobot)
        {
            MSG_CLIENT_ARENA_PVP_START_REQUEST msg = new MSG_CLIENT_ARENA_PVP_START_REQUEST();
            msg.idUser = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.id;
            msg.idTarget = targetId;

            if (isRobot)
            {
                msg.cbIsRobot = 1;
            }
            else
            {
                msg.cbIsRobot = 0;
            }

            NetworkMgr.sendData(msg);
        }

        
        
        public void SendArenaEndMsg(ArenaEndInfo aei)
        {
            MSG_CLIENT_ARENA_PVP_END_REQUEST msg = new MSG_CLIENT_ARENA_PVP_END_REQUEST();
            msg.u32SelfTotalHP = aei.uSelfTotalHP;
            msg.u32TargetTotalHP = aei.uTargetTotalHP;
            msg.u32SelfTotalHPRemain = aei.uSelfTotalHPRemain;
            msg.u32TargetTotalHPRemain = aei.uTargetTotalHPRemain;
            msg.u32SelfHeroAlive = aei.uSelfHeroAlive;
            msg.u32TargetHeroAlive = aei.uTargetHeroAlive;
            msg.u32SelfTotalDamage = aei.uSelfTotalDamage;
            msg.u32TargetTotalDamage = aei.uTargetTotalDamage;


            NetworkMgr.sendData(msg);
        }

        public void SendArenaFriendRankingMsg()
        {
            MSG_CLIENT_ARENA_FRIEND_RANKING_REQUEST msg = new MSG_CLIENT_ARENA_FRIEND_RANKING_REQUEST();
            msg.idUser = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.id;
            NetworkMgr.sendData(msg);
        }

        public void SendArenaTotalRankingMsg()
        {
            MSG_CLIENT_ARENA_TOTAL_RANKING_REQUEST msg = new MSG_CLIENT_ARENA_TOTAL_RANKING_REQUEST();
            msg.idUser = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.id;
            NetworkMgr.sendData(msg);
        }

        //竞技场队伍
        public void SendHeroFormationArenaMsg(int idTeam)
        {
            MSG_CLIENT_ARENA_HERO_FORMATION_REQUEST msg = new MSG_CLIENT_ARENA_HERO_FORMATION_REQUEST();

            if (idTeam == 0)
            {
                msg.idFightTroop = (uint)nPveTeamId;
                nPvpTeamId = 1;
            }
            else
            {
                msg.idFightTroop = 14;
                nPvpTeamId = 2;
            }
            
            List<TROOP> teamList = new List<TROOP>();
            msg.lst = null;

            TeamInfo teamPve = dicArenaTeams[1];
            TROOP newTeam = new TROOP();

            newTeam.idTroop = (byte)nPveTeamId;
            newTeam.btLeaderPos = (byte)teamPve.leaderPos;
            newTeam.btFriendPos = (byte)teamPve.friendPos;
            newTeam.idPos1 = (uint)teamPve.pos1HeroId;
            newTeam.idPos2 = (uint)teamPve.pos2HeroId;
            newTeam.idPos3 = (uint)teamPve.pos3HeroId;
            newTeam.idPos4 = (uint)teamPve.pos4HeroId;
            newTeam.idPos5 = (uint)teamPve.pos5HeroId;
            newTeam.idPos6 = (uint)teamPve.pos6HeroId;

            if (newTeam.btLeaderPos != 0)
            {
                teamList.Add(newTeam);
            }

            TeamInfo teamPvp = dicArenaTeams[2];
            TROOP pvpTeam = new TROOP();

            pvpTeam.idTroop = 14;
            pvpTeam.btLeaderPos = (byte)teamPvp.leaderPos;
            pvpTeam.btFriendPos = (byte)teamPvp.friendPos;
            pvpTeam.idPos1 = (uint)teamPvp.pos1HeroId;
            pvpTeam.idPos2 = (uint)teamPvp.pos2HeroId;
            pvpTeam.idPos3 = (uint)teamPvp.pos3HeroId;
            pvpTeam.idPos4 = (uint)teamPvp.pos4HeroId;
            pvpTeam.idPos5 = (uint)teamPvp.pos5HeroId;
            pvpTeam.idPos6 = (uint)teamPvp.pos6HeroId;

            if (pvpTeam.btLeaderPos != 0)
            {
                teamList.Add(pvpTeam);
            }

            msg.usCnt = (ushort)teamList.Count;
            msg.lst = new TROOP[msg.usCnt];
            msg.lst = teamList.ToArray();


            if (msg.usCnt == 0)
            {
                return;
            }

            NetworkMgr.sendData(msg);
        }

        void onSetFormationArena(int nEvent, System.Object param)
        {
            MSG_CLIENT_ARENA_HERO_FORMATION_RESPONSE msg = (MSG_CLIENT_ARENA_HERO_FORMATION_RESPONSE)param;

            if (msg.err == 100000)
            {
                Debug.Log("ArenaFormationSuc");
            }
        }


        int nPveTeamId = 0;
        public int nPvpTeamId = 0; 
        public Dictionary<int, TeamInfo> dicArenaTeams = new Dictionary<int, TeamInfo>();

        public void InitTeam()
        {
            dicArenaTeams.Clear();
            nPveTeamId = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).nCurTeamId + 1;

//             if (!DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams.ContainsKey(nPveTeamId))
//             {
//                 return;
//             }

            TeamInfo pveTeam = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams[nPveTeamId];
            dicArenaTeams.Add(1, pveTeam);

            TeamInfo pvpTeam = null;
            if (!DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams.ContainsKey(14))
            {
                pvpTeam = new TeamInfo();
                pvpTeam.friendPos = 6;
                dicArenaTeams.Add(2, pvpTeam);
            }
            else
            {
                pvpTeam = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams[14];
                dicArenaTeams.Add(2, pvpTeam);
                DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams.Remove(14);
            }
        }

        public void ChangeTeamerById(int idGround, int idHero, int idTroop = 0)
        {
            int beforeId = 0;

            switch (idGround)
            {
                case 0:
                    beforeId = dicArenaTeams[idTroop + 1].pos1HeroId;
                    dicArenaTeams[idTroop + 1].pos1HeroId = idHero;
                    break;
                case 1:
                    beforeId = dicArenaTeams[idTroop + 1].pos2HeroId;
                    dicArenaTeams[idTroop + 1].pos2HeroId = idHero;
                    break;
                case 2:
                    beforeId = dicArenaTeams[idTroop + 1].pos3HeroId;
                    dicArenaTeams[idTroop + 1].pos3HeroId = idHero;
                    break;
                case 3:
                    beforeId = dicArenaTeams[idTroop + 1].pos4HeroId;
                    dicArenaTeams[idTroop + 1].pos4HeroId = idHero;
                    break;
                case 4:
                    beforeId = dicArenaTeams[idTroop + 1].pos5HeroId;
                    dicArenaTeams[idTroop + 1].pos5HeroId = idHero;
                    break;
                case 5:
                    beforeId = dicArenaTeams[idTroop + 1].pos6HeroId;
                    dicArenaTeams[idTroop + 1].pos6HeroId = idHero;
                    break;
            }

            if (idHero == 0)
            {
                dicArenaTeams[idTroop + 1].friendPos = idGround + 1;
            }
        }

        public void DelTeamerById(int idGround, int idHero, int idTroop = 0)
        {
            switch (idGround)
            {
                case 0:
                    dicArenaTeams[idTroop + 1].pos1HeroId = 0;
                    break;
                case 1:
                    dicArenaTeams[idTroop + 1].pos2HeroId = 0;
                    break;
                case 2:
                    dicArenaTeams[idTroop + 1].pos3HeroId = 0;
                    break;
                case 3:
                    dicArenaTeams[idTroop + 1].pos4HeroId = 0;
                    break;
                case 4:
                    dicArenaTeams[idTroop + 1].pos5HeroId = 0;
                    break;
                case 5:
                    dicArenaTeams[idTroop + 1].pos6HeroId = 0;
                    break;

            }

        }

        //升级所需点
        public int GetNextLevelPoint(uint curPoint)
        {
            ConfigTable arenarank = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ARENA_RANK);
            if (arenarank == null)
                return 0;
            ConfigRow cur = null;
            foreach (ConfigRow row in arenarank.rows)//递增
            {
                if (row.getIntValue(DICT_ARENA_RANK.ARENA_POINT) > curPoint)
                {
                    cur = row;
                    return row.getIntValue(DICT_ARENA_RANK.ARENA_POINT);
                }
            }

            return 0;
        }

        //下段奖励
        public string GetNextAward(uint curPoint)
        {
            string strTemp = "";

            ConfigTable arenarank = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ARENA_RANK);
            if (arenarank == null)
                return strTemp;
            ConfigRow cur = null;
            foreach (ConfigRow row in arenarank.rows)//递增
            {
                if (row.getIntValue(DICT_ARENA_RANK.ARENA_POINT) > curPoint)
                {
                    cur = row;
                    break;
                }
            }

            if (cur == null)
                return strTemp;

            if (cur.getIntValue(DICT_ARENA_RANK.DIAMOND) == 0)
            {
                int idType = cur.getIntValue(DICT_ARENA_RANK.ITEMTYPE_ID);
                ItemInfo item = new ItemInfo();
                item.init(idType);
                strTemp = item.icon;
            }
            else
            {
                strTemp = "FgtIcon_diamond";
            }

            return strTemp;
        }
        
        //UI界面
        public ArenaMain arenaMain;

        public void ShowArenaMain()
        {
            nPveTeamId = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).nCurTeamId + 1;
            TeamInfo pveTeam = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams[nPveTeamId];
            dicArenaTeams[1] = pveTeam;

            arenaMain.gameObject.SetActive(true);
            arenaMain.step1.SetActive(true);
            SendArenaTargetMsg();
        }

        public void ShowStep1()
        {
            arenaMain.ShowPageByIndex(0);
        }

        public void ShowStep2()
        {
            arenaMain.ShowPageByIndex(1);
        }

        public void ShowTotalRank()
        {
            arenaMain.ShowPageByIndex(2);
            ArenaRankList arl = arenaMain.ArenaPages[2].GetComponent<ArenaRankList>();
            arl.ShowRankList();

        }

        public void ShowFriendRank()
        {
            arenaMain.ShowPageByIndex(3);

            FriendRankList frl = arenaMain.ArenaPages[3].GetComponent<FriendRankList>();
            frl.ShowRankList();
        }

        public void ShowArenaLevel(int nIndex)
        {
            arenaMain.ShowPageByIndex(4);
            ArenaRank ar = arenaMain.ArenaPages[4].GetComponent<ArenaRank>();
            ar.pageIndex = nIndex;
        }

        //队伍编组使用
        public void ShowArenaTeam()
        {
            nPveTeamId = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).nCurTeamId + 1;
            TeamInfo pveTeam = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroTeams[nPveTeamId];
            dicArenaTeams[1] = pveTeam;
            
            arenaMain.gameObject.SetActive(true);
            arenaMain.ShowPageByIndex(5);
            ArenaTeam at = arenaMain.ArenaPages[5].GetComponent<ArenaTeam>();
            at.InitShowTeam();
            at.pageIndex = 10;
        }

    }
}