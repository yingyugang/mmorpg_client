using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class SummonsTeam : MonoBehaviour
{
    GameObject returnBtn;
    GameObject enterBtn;


    GameObject editBtn1;
    GameObject editBtn2;
    GameObject editBtn3;

    GameObject backupBtn1;
    GameObject backupBtn2;
    GameObject backupBtn3;

    GameObject team1;
    GameObject team2;
    GameObject team3;
    List<GameObject> teamList = new List<GameObject>();

    // Use this for initialization
    void Awake()
    {

        returnBtn = PanelTools.findChild(gameObject, "Title/btnBack");

        if (returnBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onReturn;
        }

        enterBtn = PanelTools.findChild(gameObject, "enterButton");

        if (enterBtn != null)
        {
            UIEventListener.Get(enterBtn).onClick = onEnter;
        }

        editBtn1 = PanelTools.findChild(gameObject, "Team1/editBtn");
        if (editBtn1 != null)
            UIEventListener.Get(editBtn1).onClick = onEdit1;

        editBtn2 = PanelTools.findChild(gameObject, "Team2/editBtn");
        if (editBtn2 != null)
            UIEventListener.Get(editBtn2).onClick = onEdit2;

        editBtn3 = PanelTools.findChild(gameObject, "Team3/editBtn");
        if (editBtn3 != null)
            UIEventListener.Get(editBtn3).onClick = onEdit3;



        backupBtn1 = PanelTools.findChild(gameObject, "Team1/backupBtn");
        if (backupBtn1 != null)
            UIEventListener.Get(backupBtn1).onClick = onBackup1;

        backupBtn2 = PanelTools.findChild(gameObject, "Team2/backupBtn");
        if (backupBtn2 != null)
            UIEventListener.Get(backupBtn2).onClick = onBackup2;

        backupBtn3 = PanelTools.findChild(gameObject, "Team3/backupBtn");
        if (backupBtn3 != null)
            UIEventListener.Get(backupBtn3).onClick = onBackup3;

        team1 = PanelTools.findChild(gameObject, "Team1");
        team2 = PanelTools.findChild(gameObject, "Team2");
        team3 = PanelTools.findChild(gameObject, "Team3");
        teamList.Add(team1);
        teamList.Add(team2);
        teamList.Add(team3);
	}

    void onReturn(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowChallengePage();
    }

    void onEnter(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowReadyChallenge();
    }

    void onEdit1(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowChanllengeTeam(0);
    }

    void onEdit2(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowChanllengeTeam(1);
    }

    void onEdit3(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowChanllengeTeam(2);
    }

    void onBackup1(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowSelectAssistant();
    }

    void onBackup2(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowSelectAssistant();
    }

    void onBackup3(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowSelectAssistant();
    }

    void OnEnable()
    {
        ShowTeams();
    }

    public void ShowTeams()
    {
        int nCount = DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).dicHeroTeams.Count;

        for (int i = 0; i < nCount; ++i)
        {
            TeamInfo info = DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).dicHeroTeams[i + 1];
            info.OrderTeamer();

            GameObject hero1 = PanelTools.findChild(teamList[i], "hero1");
            GameObject hero2 = PanelTools.findChild(teamList[i], "hero2");
            GameObject hero3 = PanelTools.findChild(teamList[i], "hero3");
            GameObject hero4 = PanelTools.findChild(teamList[i], "hero4");
            GameObject hero5 = PanelTools.findChild(teamList[i], "hero5");
            GameObject hero6 = PanelTools.findChild(teamList[i], "hero6");
            List<GameObject> heroList = new List<GameObject>();
            heroList.Add(hero1);
            heroList.Add(hero2);
            heroList.Add(hero3);
            heroList.Add(hero4);
            heroList.Add(hero5);


            for (int j = 0; j < 5; ++j)
            {
                HeroInfo heroInfo = info.GetOrderTeamerByIndex(j + 1);

                if (heroInfo != null)
                {

                    UILabel level = PanelTools.findChild<UILabel>(heroList[j], "levelLabel");
                    level.text = "LV." + heroInfo.level.ToString();
                    UISprite heroIcon = PanelTools.findChild<UISprite>(heroList[j], "heroIcon");
                    heroIcon.spriteName = heroInfo.portarait;
                    UISprite frame = PanelTools.findChild<UISprite>(heroList[j], "frame");

                    heroList[j].SetActive(true);
                }
                else
                {
                    heroList[j].SetActive(false);
                }

            }

            //更新好友英雄
            if (info.friendHero != null)
            {
                UILabel level = PanelTools.findChild<UILabel>(hero6, "levelLabel");
                level.text = "LV." + info.friendHero.level.ToString();
                UISprite heroIcon = PanelTools.findChild<UISprite>(hero6, "heroIcon");
                heroIcon.spriteName = info.friendHero.portarait;
                UISprite frame = PanelTools.findChild<UISprite>(hero6, "frame");
                
                hero6.SetActive(true);
            }
            else
            {
                hero6.SetActive(false);
            }
        }



    }
}
