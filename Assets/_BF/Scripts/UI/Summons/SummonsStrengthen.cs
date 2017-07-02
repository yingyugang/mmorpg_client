using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class SummonsStrengthen : MonoBehaviour
{
    GameObject returnBtn;
    GameObject uplvBtn;
    GameObject upInfo;

    UISlider expBar;
    UILabel expLabel;

    SummonInfo m_Summon;

    bool pressState = false;
    ulong soulCost = 0;
    ulong nNeedExp = 10000;
    ulong curSoul = 0;
    uint upLevel = 0;
    // Use this for initialization
	void Start () {

        returnBtn = PanelTools.findChild(gameObject, "Title/btnBack");

        if (returnBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onReturn;
        }

        uplvBtn = PanelTools.findChild(gameObject, "upLv/uplvButton");

        if (uplvBtn != null)
        {
            UIEventListener.Get(uplvBtn).onPress = onUpLevel;
        }
	}

    void OnEnable()
    {
        ShowSummon();
    }

    void onReturn(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowSelectStrengthen();
    }

    void onUpLevel(GameObject go, bool isPressed)
    {
        pressState = isPressed;
        
        if (!isPressed)
        {
            Debug.Log("soulCost:" + soulCost);
            curSoul += soulCost;
            soulCost = 0;

            DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).updateSummon((uint)curSoul);
        }
    }

    public void Update()
    {
        if (pressState)
        {
            //soulCost += nNeedExp / 50;
            soulCost += 1;

            if (curSoul + soulCost >= nNeedExp)
            {
                soulCost = nNeedExp - curSoul;
                ShowUpLevel();
            }


            expLabel.text = (curSoul + soulCost).ToString() + "/" + nNeedExp.ToString();
            expBar.value = (float)(curSoul + soulCost) / (float)nNeedExp;
        }
    }

    void ShowUpLevel()
    {
        curSoul = 0;
        upInfo.SetActive(true);

        ++upLevel;
        
        UILabel upLv = PanelTools.findChild<UILabel>(gameObject, "upLv/upInfo/lvLabel");
        upLv.text = upLevel.ToString();


        long hp = m_Summon.initHp + m_Summon.baseHp * upLevel;
        UILabel upHp = PanelTools.findChild<UILabel>(gameObject, "upLv/upInfo/hpLabel");
        upHp.text = hp.ToString();

        long atk = m_Summon.initAtk + m_Summon.baseAtk * upLevel;
        UILabel upAtk = PanelTools.findChild<UILabel>(gameObject, "upLv/upInfo/atkLabel");
        upAtk.text = atk.ToString();

        long re = m_Summon.initRecover + m_Summon.baseRecover * upLevel;
        UILabel upRe = PanelTools.findChild<UILabel>(gameObject, "upLv/upInfo/reLabel");
        upRe.text = re.ToString();

        long def = m_Summon.initDef + m_Summon.baseDef * upLevel;
        UILabel upDef = PanelTools.findChild<UILabel>(gameObject, "upLv/upInfo/defLabel");
        upDef.text = def.ToString(); 

        nNeedExp = (uint)DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).GetNeedSoulByLevel(m_Summon.level + (int)upLevel);
    }


    void InitUI()
    {
        expBar = PanelTools.findChild<UISlider>(gameObject, "upLv/Progress Bar");
        expLabel = PanelTools.findChild<UILabel>(gameObject, "upLv/expLabel");
        upInfo = PanelTools.findChild(gameObject, "upLv/upInfo");
        upInfo.SetActive(false);
        upLevel = 0;
    }

    public void ShowSummon()
    {
        InitUI();

        curSoul = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.soul;
        
        m_Summon = DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).GetSelectSummon();
        nNeedExp = (uint)DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).GetNeedSoulByLevel(m_Summon.level);

        UILabel NoLabel = PanelTools.findChild<UILabel>(gameObject, "info/NOLabel");
        NoLabel.text = "NO." + m_Summon.id.ToString();

        UILabel nameLabel = PanelTools.findChild<UILabel>(gameObject, "info/nameLabel");
        nameLabel.text = BaseLib.LanguageMgr.getString(m_Summon.name);

        if (BaseLib.LanguageMgr.getString(m_Summon.name) == string.Empty)
        {
            nameLabel.text = m_Summon.name;
        }

        UILabel lvLabel = PanelTools.findChild<UILabel>(gameObject, "info/lvLabel");
        lvLabel.text = m_Summon.level.ToString();

        UILabel hpLabel = PanelTools.findChild<UILabel>(gameObject, "info/hpLabel");
        hpLabel.text = m_Summon.hp.ToString();

        UILabel atkLabel = PanelTools.findChild<UILabel>(gameObject, "info/atkLabel");
        atkLabel.text = m_Summon.atk.ToString();

        UILabel reLabel = PanelTools.findChild<UILabel>(gameObject, "info/reLabel");
        reLabel.text = m_Summon.recover.ToString();

        UILabel defLabel = PanelTools.findChild<UILabel>(gameObject, "info/defLabel");
        defLabel.text = m_Summon.def.ToString();

    }
}
