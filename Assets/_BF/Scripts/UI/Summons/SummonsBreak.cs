using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class SummonsBreak : MonoBehaviour
{
    GameObject returnBtn;
    GameObject breakBtn;

    // Use this for initialization
	void Start () {

        returnBtn = PanelTools.findChild(gameObject, "Title/btnBack");

        if (returnBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onReturn;
        }

        breakBtn = PanelTools.findChild(gameObject, "breakButton");

        if (breakBtn != null)
        {
            UIEventListener.Get(breakBtn).onClick = onBreak;
        }
	}

    void onReturn(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowSelectBreak();
    }

    void onBreak(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowBreakResult();
    }

    public void ShowInfo()
    {
        SummonInfo si = DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).GetSelectSummon();

        UILabel lvLabel = PanelTools.findChild<UILabel>(gameObject, "current/lvLabel");
        lvLabel.text = si.level.ToString();

        UILabel inheritLabel = PanelTools.findChild<UILabel>(gameObject, "current/inheritLabel");
        inheritLabel.text = si.inherit.ToString() + "%";

        UILabel hpLabel = PanelTools.findChild<UILabel>(gameObject, "current/hpLabel");
        hpLabel.text = si.hp.ToString();

        UILabel atkLabel = PanelTools.findChild<UILabel>(gameObject, "current/atkLabel");
        atkLabel.text = si.atk.ToString();

        UILabel reLabel = PanelTools.findChild<UILabel>(gameObject, "current/reLabel");
        reLabel.text = si.recover.ToString();

        UILabel defLabel = PanelTools.findChild<UILabel>(gameObject, "current/defLabel");
        defLabel.text = si.def.ToString();


        //进化后信息
        //配置表
        ConfigTable breakTable = ConfigMgr.getConfig(CONFIG_MODULE.DICT_SUMMON_EVOLVE);
        ConfigRow breakRow = breakTable.getRow(DICT_SUMMON_EVOLVE.SUMMON_TYPEID, (int)si.type);
        int nBreakType = breakRow.getIntValue(DICT_SUMMON_EVOLVE.EVOLVE_TYPEID);

        ConfigTable summonTable = ConfigMgr.getConfig(CONFIG_MODULE.DICT_SUMMON);
        ConfigRow summonRow = summonTable.getRow(DICT_SUMMON.SUMMON_TYPEID, nBreakType);

        UILabel lvBLabel = PanelTools.findChild<UILabel>(gameObject, "next/lvLabel");
        lvBLabel.text = "1";

        UILabel ihBLabel = PanelTools.findChild<UILabel>(gameObject, "next/inheritLabel");
        ihBLabel.text = summonRow.getStringValue(DICT_SUMMON.INHERIT) + "%";

        UILabel hpBLabel = PanelTools.findChild<UILabel>(gameObject, "next/hpLabel");
        hpBLabel.text = summonRow.getStringValue(DICT_SUMMON.INIT_HP);

        UILabel atkBLabel = PanelTools.findChild<UILabel>(gameObject, "next/atkLabel");
        atkBLabel.text = summonRow.getStringValue(DICT_SUMMON.INIT_ATK);

        UILabel reBLabel = PanelTools.findChild<UILabel>(gameObject, "next/reLabel");
        reBLabel.text = summonRow.getStringValue(DICT_SUMMON.INIT_RECOVER);

        UILabel defBLabel = PanelTools.findChild<UILabel>(gameObject, "next/defLabel");
        defBLabel.text = summonRow.getStringValue(DICT_SUMMON.INIT_DEF);



        //消耗
        UILabel costLabel = PanelTools.findChild<UILabel>(gameObject, "source/costLabel");
        costLabel.text = breakRow.getStringValue(DICT_SUMMON_EVOLVE.SOUL);

        GameObject item1 = PanelTools.findChild(gameObject, "source/item1");
        GameObject item2 = PanelTools.findChild(gameObject, "source/item2");
        GameObject item3 = PanelTools.findChild(gameObject, "source/item3");
        GameObject item4 = PanelTools.findChild(gameObject, "source/item4");
        GameObject item5 = PanelTools.findChild(gameObject, "source/item5");
        
        List<GameObject> items = new List<GameObject>();
        items.Add(item1);
        items.Add(item2);
        items.Add(item3);
        items.Add(item4);
        items.Add(item5);

        List<int> idHeroList = new List<int>();
        idHeroList.Add(breakRow.getIntValue(DICT_SUMMON_EVOLVE.COST_HERO_1));
        idHeroList.Add(breakRow.getIntValue(DICT_SUMMON_EVOLVE.COST_HERO_2));
        idHeroList.Add(breakRow.getIntValue(DICT_SUMMON_EVOLVE.COST_HERO_3));
        idHeroList.Add(breakRow.getIntValue(DICT_SUMMON_EVOLVE.COST_HERO_4));
        idHeroList.Add(breakRow.getIntValue(DICT_SUMMON_EVOLVE.COST_HERO_5));


        for (int i = 0; i < 5; ++i)
        {
            if (idHeroList[i] == 0)
            {
                items[i].SetActive(false);
            }
            else
            {
                items[i].SetActive(true);

                HeroInfo hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoByTypeId(idHeroList[i]);

                UISprite heroIcon = PanelTools.findChild<UISprite>(items[i], "heroIcon");
                heroIcon.spriteName = hi.portarait;


            }
           

        }
    }
}
