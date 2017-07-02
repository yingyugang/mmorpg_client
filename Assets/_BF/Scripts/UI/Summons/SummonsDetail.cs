using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class SummonsDetail : MonoBehaviour {

    public GameObject backGO;
    
    GameObject returnBtn;
    GameObject backgroundPage;
    GameObject skillPage;

    // Use this for initialization
	void Start () {

        returnBtn = PanelTools.findChild(gameObject, "Title/btnBack");

        if (returnBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onReturn;
        }

        backgroundPage = PanelTools.findChild(gameObject, "Introduction");
        skillPage = PanelTools.findChild(gameObject, "Skill");
	}

    void OnDisable()
    {
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, true, (int)EVENT_GROUP.mainUI);
    }

    void OnEnable()
    {
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, false, (int)EVENT_GROUP.mainUI);
    }

    void onReturn(GameObject go)
    {
        if (backGO != null)
        {
            backGO.SetActive(true);
        }

        gameObject.SetActive(false);
    }

    void onBackground(GameObject go)
    {
        backgroundPage.SetActive(true);
        skillPage.SetActive(false);
    }

    void onSkill(GameObject go)
    {
        backgroundPage.SetActive(false);
        skillPage.SetActive(true);
    }

    public void ShowSummon()
    {
        SummonInfo si = DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).GetSelectSummon();

        UILabel NOLabel = PanelTools.findChild<UILabel>(gameObject, "info/NO");
        NOLabel.text = "NO." + si.NO.ToString();

        UISprite series = PanelTools.findChild<UISprite>(gameObject, "info/series");
        series.spriteName = "series" + si.series.ToString();

        UILabel nameLabel = PanelTools.findChild<UILabel>(gameObject, "info/name");
        nameLabel.text = si.name;

        UILabel energyLabel = PanelTools.findChild<UILabel>(gameObject, "info/energy");
        energyLabel.text = "LV." + si.energy.ToString();

        UILabel lvLabel = PanelTools.findChild<UILabel>(gameObject, "info/level");
        lvLabel.text = si.level.ToString();

        UILabel maxLvLabel = PanelTools.findChild<UILabel>(gameObject, "info/maxlevel");
        maxLvLabel.text = si.maxLv.ToString();


        UILabel hpLabel = PanelTools.findChild<UILabel>(gameObject, "info/hp");
        hpLabel.text = si.hp.ToString();

        UILabel reLabel = PanelTools.findChild<UILabel>(gameObject, "info/re");
        reLabel.text = si.recover.ToString();

        UILabel atkLabel = PanelTools.findChild<UILabel>(gameObject, "info/atk");
        atkLabel.text = si.atk.ToString();

        UILabel defLabel = PanelTools.findChild<UILabel>(gameObject, "info/def");
        defLabel.text = si.def.ToString();

        UILabel inheritLabel = PanelTools.findChild<UILabel>(gameObject, "info/inherit");
        inheritLabel.text = si.inherit.ToString() + "%";

        UILabel breakLabel = PanelTools.findChild<UILabel>(gameObject, "info/break");


        UILabel entryLabel = PanelTools.findChild<UILabel>(gameObject, "Skill/entryLabel");
        entryLabel.text = BaseLib.LanguageMgr.getString(si.skillOutName);

        UILabel attackLabel = PanelTools.findChild<UILabel>(gameObject, "Skill/attackLabel");
        attackLabel.text = BaseLib.LanguageMgr.getString(si.skillBaseName);

        UILabel passiveLabel = PanelTools.findChild<UILabel>(gameObject, "Skill/passiveLabel");
        passiveLabel.text = BaseLib.LanguageMgr.getString(si.skillPassiveName);

        UILabel departureLabel = PanelTools.findChild<UILabel>(gameObject, "Skill/departureLabel");
        departureLabel.text = BaseLib.LanguageMgr.getString(si.skillOutName);

        GameObject bgBtn = PanelTools.findChild(gameObject, "Skill/turnBtn");
        UIEventListener.Get(bgBtn).onClick = onBackground;



        UILabel bgLabel = PanelTools.findChild<UILabel>(gameObject, "Introduction/bgLabel");
        bgLabel.text = BaseLib.LanguageMgr.getString(si.desc);

        GameObject skillBtn = PanelTools.findChild(gameObject, "Introduction/turnBtn");
        UIEventListener.Get(bgBtn).onClick = onSkill;
    }
}
