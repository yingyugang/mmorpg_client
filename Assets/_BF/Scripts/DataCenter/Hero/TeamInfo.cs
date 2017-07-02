using System;
using System.Collections.Generic;
using BaseLib;

namespace DataCenter
{
    //阵容信息
    public class TeamInfo
    {
        const int MAX_HERO = 5;
        
        
        public int troopId;    //队伍序号
        public int userId;
        public int leaderPos;  //队长位置
        public int friendPos;  //好友位置
        public int pos1HeroId;
        public int pos2HeroId;
        public int pos3HeroId;
        public int pos4HeroId;
        public int pos5HeroId;
        public int pos6HeroId;


        public int GetHeroCount()
        {
            int nCount = 0;

            if (pos1HeroId != 0)
            {
                ++nCount;
            }

            if (pos2HeroId != 0)
            {
                ++nCount;
            }

            if (pos3HeroId != 0)
            {
                ++nCount;
            }

            if (pos4HeroId != 0)
            {
                ++nCount;
            }

            if (pos5HeroId != 0)
            {
                ++nCount;
            }

            if (pos6HeroId != 0)
            {
                ++nCount;
            }

            return nCount;
        }

        public bool isInTeam(int idHero)
        {
            if (idHero == pos1HeroId)
            {
                return true;
            }

            if (idHero == pos2HeroId)
            {
                return true;
            }

            if (idHero == pos3HeroId)
            {
                return true;
            }

            if (idHero == pos4HeroId)
            {
                return true;
            }

            if (idHero == pos5HeroId)
            {
                return true;
            }

            if (idHero == pos6HeroId)
            {
                return true;
            }

            return false;
        }

        public int GetTeamLeader()
        {
            int nLeader = 0;

            HeroInfo hi = null;

            if (GetHeroCount() == 0)
            {
                return nLeader;
            }

            if (pos1HeroId != 0)
            {
                hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(pos1HeroId);

                if (hi != null)
                {
                    nLeader += hi.leader;
                }

            }

            if (pos2HeroId != 0)
            {
                hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(pos2HeroId);

                if (hi != null)
                {
                    nLeader += hi.leader;
                }
            }

            if (pos3HeroId != 0)
            {
                hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(pos3HeroId);

                if (hi != null)
                {
                    nLeader += hi.leader;
                }

            }

            if (pos4HeroId != 0)
            {
                hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(pos4HeroId);

                if (hi != null)
                {
                    nLeader += hi.leader;
                }
            }

            if (pos5HeroId != 0)
            {
                hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(pos5HeroId);

                if (hi != null)
                {
                    nLeader += hi.leader;
                }
            }

            if (pos6HeroId != 0)
            {
                hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(pos6HeroId);

                if (hi != null)
                {
                    nLeader += hi.leader;
                }
            }

            return nLeader;
        }

        public HeroInfo GetLeaderHero()
        {
            HeroInfo hi = null;

            switch (leaderPos)
            {
                case 1:
                    hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(pos1HeroId);
                    break;
                case 2:
                    hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(pos2HeroId);
                    break;
                case 3:
                    hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(pos3HeroId);
                    break;
                case 4:
                    hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(pos4HeroId);
                    break;
                case 5:
                    hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(pos5HeroId);
                    break;
                case 6:
                    hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(pos6HeroId);
                    break;
            }
            

            return hi;
        }

        public Dictionary<int, HeroInfo> dicOrderTeamers = new Dictionary<int, HeroInfo>();
        public List<HeroInfo> teamerList = new List<HeroInfo>();

        public void OrderTeamer()
        {
            dicOrderTeamers.Clear();
            teamerList.Clear();
            ChangeLeader();
            int index = 0;

            HeroInfo hero1 = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(pos1HeroId);
            HeroInfo hero2 = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(pos2HeroId);
            HeroInfo hero3 = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(pos3HeroId);
            HeroInfo hero4 = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(pos4HeroId);
            HeroInfo hero5 = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(pos5HeroId);
            HeroInfo hero6 = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(pos6HeroId);

            teamerList.Add(hero1);
            teamerList.Add(hero2);
            teamerList.Add(hero3);
            teamerList.Add(hero4);
            teamerList.Add(hero5);
            teamerList.Add(hero6);


            for (int i = 0; i < 6; ++i)
            {
                if (leaderPos == i + 1)
                {
                    dicOrderTeamers.Add(1, teamerList[i]);
                }
                else if (friendPos == i + 1)
                {
                    dicOrderTeamers.Add(6, teamerList[i]);
                }
                else
                {
                    if (teamerList[i] != null)
                    {
                        dicOrderTeamers.Add(2 + index, teamerList[i]);
                        ++index;
                    }
                }
            }
        }
        
        public HeroInfo GetOrderTeamerByIndex(int nIndex)
        {
            HeroInfo hi = null;
            
            if (dicOrderTeamers.ContainsKey(nIndex))
            {
                dicOrderTeamers.TryGetValue(nIndex, out hi);
            }

            return hi;
        }


        public HeroInfo friendHero = null;
        
        public void AddFriendHero(HeroInfo fHero)
        {
            friendHero = fHero;
        }

        public void ChangeLeader()
        {
            List<int> posHeroId = new List<int>();
            posHeroId.Add(pos1HeroId);
            posHeroId.Add(pos2HeroId);
            posHeroId.Add(pos3HeroId);
            posHeroId.Add(pos4HeroId);
            posHeroId.Add(pos5HeroId);
            posHeroId.Add(pos6HeroId);
            
            
            HeroInfo hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(posHeroId[leaderPos - 1]);

            if (hi != null)
            {
                return;
            }
            
            for (int i = 0; i < posHeroId.Count; ++i)
            {
                if (posHeroId[i] != 0)
                {
                    hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(posHeroId[i]);

                    if (hi != null)
                    {
                        leaderPos = i + 1;
                    }

                }
            }
        }
    }
}
