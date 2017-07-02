using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class ReadyChallenge : MonoBehaviour
{
    GameObject returnBtn;
    GameObject enterBtn;

    GameObject Item1;
    GameObject Item2;
    GameObject Item3;

    List<GameObject> teams = new List<GameObject>();

    // Use this for initialization
	void Start () {

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
	}

    void OnEnable()
    {
        ShowTeam();
    }

    void onReturn(GameObject go)
    {

        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowChallengePage();
    }

    void onEnter(GameObject go)
    {
        //DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowBreakResult();
    }

    void onEdit(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowOrgTeam();
    }

    void onBackup(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowSelectAssistant();
    }

    void InitUI()
    {
        Item1 = PanelTools.findChild(gameObject, "Scroll View/Grid/Item1");
        Item2 = PanelTools.findChild(gameObject, "Scroll View/Grid/Item2");
        Item3 = PanelTools.findChild(gameObject, "Scroll View/Grid/Item3");

        teams.Add(Item1);
        teams.Add(Item2);
        teams.Add(Item3);
    }


    public void ShowTeam()
    {
        InitUI();

        int nTeam = 1;

        foreach(GameObject go in teams)
        {
            GameObject editBtn = PanelTools.findChild(go, "Team/editBtn");
            UIEventListener.Get(editBtn).onClick = onEdit;

            GameObject backupBtn = PanelTools.findChild(go, "Team/backupBtn");
            UIEventListener.Get(backupBtn).onClick = onBackup;

            TeamInfo info = DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).dicHeroTeams[nTeam];
            info.OrderTeamer();

            GameObject hero1 = PanelTools.findChild(go, "Team/hero1");
            GameObject hero2 = PanelTools.findChild(go, "Team/hero2");
            GameObject hero3 = PanelTools.findChild(go, "Team/hero3");
            GameObject hero4 = PanelTools.findChild(go, "Team/hero4");
            GameObject hero5 = PanelTools.findChild(go, "Team/hero5");
            GameObject hero6 = PanelTools.findChild(go, "Team/hero6");
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



            ++nTeam;
        }
    }
}
