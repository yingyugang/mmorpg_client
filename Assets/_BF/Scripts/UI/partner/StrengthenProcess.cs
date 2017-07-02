using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class StrengthenProcess : MonoBehaviour
{

    public UIButton skipBtn;
    public UIButton returnBtn;
    public GameObject selectBase;
    public GameObject partnerStrengthen;
    public GameObject upLevelEffet;

    GameObject upLevel = null;
    GameObject upSkill = null;
    // Use this for initialization
	void Start () {

        if (skipBtn != null)
            UIEventListener.Get(skipBtn.gameObject).onClick = onSkip;
        if (returnBtn != null)
            UIEventListener.Get(returnBtn.gameObject).onClick = onReturn;
	}

    void OnEnable()
    {
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, false, (int)EVENT_GROUP.mainUI);
        upLevelEffet.SetActive(false);
    }

    void OnDisable()
    {
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, true, (int)EVENT_GROUP.mainUI);
    }

	// Update is called once per frame
	void Update () {
        if (isStart)
        {
            ShowProgress();
        }
        
	}


    void InitUI()
    {
        upLevel = PanelTools.findChild(gameObject, "upLevel");
        upSkill = PanelTools.findChild(gameObject, "upSkill");

        upLevel.SetActive(false);
        upSkill.SetActive(false);
        
        curExp = 0;
        curLevel = 0;
        curUpLevel = 0;
        getExp = 0;
        isStart = false;
        elapsedTime = 0;
        showExp = 0;
        nProgressTime = 2.0f;
        isShowResult = false;
    }

    void onReturn(GameObject go)
    {
        onSkip(go);
        
        gameObject.SetActive(false);

//         selectBase.SetActive(true);
//         SelectBase sb = selectBase.GetComponent<SelectBase>();
//         sb.ShowHeroBrowse();
        partnerStrengthen.SetActive(true);
        PartnerStrengthen ps = partnerStrengthen.GetComponent<PartnerStrengthen>();
        ps.ClearUI();
        ps.ShowBase(baseHero.id);
    }
    
    void onSkip(GameObject go)
    {
        if (isShowResult)
        {
            return;
        }
        
        nProgressTime = 0.01f;
        ShowResult();
    }

    HeroInfo baseHero = null;
    
    public void ShowStrengthen(HeroInfo destHero, int exp, int result, int skillLvl)
    {
        InitUI();
        
        baseHero = destHero;

        
        UILabel nameLabel = PanelTools.findChild<UILabel>(gameObject, "Info/nameLabel");
        nameLabel.text = baseHero.name;
        
        UILabel level = PanelTools.findChild<UILabel>(gameObject, "Info/level");
        level.text = baseHero.level.ToString();

        UILabel hp = PanelTools.findChild<UILabel>(gameObject, "Info/hp");
        hp.text = baseHero.hp.ToString();

        UILabel bblv = PanelTools.findChild<UILabel>(gameObject, "Info/BBLv");
        bblv.text = baseHero.skillLevel.ToString();

        UILabel atk = PanelTools.findChild<UILabel>(gameObject, "Info/atk");
        atk.text = baseHero.atk.ToString();

        UILabel def = PanelTools.findChild<UILabel>(gameObject, "Info/def");
        def.text = baseHero.def.ToString();

        UILabel re = PanelTools.findChild<UILabel>(gameObject, "Info/re");
        re.text = baseHero.recover.ToString();

        Transform info = gameObject.transform.FindChild("HeroModel");
        Transform modelOld = info.FindChild("model");
        if (modelOld != null)
        {
            Destroy(modelOld.gameObject);
        }

        if (DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroPrefabs.ContainsKey(baseHero.fbxFile))
        {
            GameObject modelPrefab = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroPrefabs[baseHero.fbxFile];
            if (modelPrefab != null)
            {
                GameObject model = NGUITools.AddChild(info.gameObject, modelPrefab);
                model.transform.localScale = new UnityEngine.Vector3(60, 60, 1);
                model.transform.localPosition = new UnityEngine.Vector3(10, -140, 0);
                model.name = "model";

                HeroInfo.ChangeModel(model, 1.0f/60.0f);
            }
        }

        int nMaxExp = baseHero.GetLvlupExp();
        
        UILabel leftExp = PanelTools.findChild<UILabel>(gameObject, "Info/toUp");
        leftExp.text = (nMaxExp - baseHero.exp).ToString();

        UIProgressBar pb = PanelTools.findChild<UIProgressBar>(gameObject, "Info/Progress Bar");
        pb.value = (float)baseHero.exp / (float)nMaxExp;
        curExp = baseHero.exp;
        curLevel = baseHero.level;
        getExp = exp;
        nGetResultExp = exp;
        nMaxProgressExp = baseHero.GetLvlupExp();
        StartCoroutine(_ShowProgress());


        if (baseHero.skillLevel != skillLvl)
        {
            ShowUpSkill(skillLvl);
        }
    }


    float nProgressTime = 2.0f;
    
    int curExp = 0;
    int curLevel = 0;
    int curUpLevel = 0;
    int getExp = 0;
    bool isStart = false;
    float elapsedTime = 0;
    float showExp = 0;
    int nMaxProgressExp = 0;

    IEnumerator _ShowProgress()
    {
        yield return new WaitForSeconds(0.5f);
        isStart = true;
    }

    protected void ShowProgress()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > nProgressTime || getExp + curExp - showExp < 0)
        {
            elapsedTime = 0;
            showExp = 0;
            isStart = false;
            return;
        }

        if (getExp + curExp > nMaxProgressExp)
        {
            showExp = Mathf.Lerp((float)curExp, (float)nMaxProgressExp, elapsedTime);

            if (showExp == nMaxProgressExp)
            {
                getExp -= (nMaxProgressExp - curExp);
                showExp = 0;
                curExp = 0;
                elapsedTime = 0;
                ++curLevel;
                ++curUpLevel;
                ShowUpLevel();
            }
        }
        else
        {
            showExp = Mathf.Lerp((float)curExp, (float)(getExp + curExp), elapsedTime);

            if (showExp == (float)(getExp + curExp))
            {
                HeroInfo hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(baseHero.id);
                hi.exp = getExp + curExp;
                isShowResult = true;
            }
        }

        int nLeftExp = nMaxProgressExp - (int)showExp;
        UILabel leftExpLabel = PanelTools.findChild<UILabel>(gameObject, "Info/toUp");
        leftExpLabel.text = nLeftExp.ToString();

        UIProgressBar pb = PanelTools.findChild<UIProgressBar>(gameObject, "Info/Progress Bar");
        pb.value = showExp / (float)nMaxProgressExp;

    }

    GameObject creatEffec = null;
    protected void ShowUpLevelEffect()
    {
        if (creatEffec != null)
        {
            NGUITools.Destroy(creatEffec);
        }
        
        creatEffec = NGUITools.AddChild(gameObject, upLevelEffet);
        creatEffec.SetActive(true);
    }
    
    protected void ShowUpLevel()
    {
        ShowUpLevelEffect();
        upLevel.SetActive(true);

        HeroInfo hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(baseHero.id);
        hi.level = curLevel;
        hi.InitDict(baseHero.type);
        nMaxProgressExp = hi.GetLvlupExp();
        
        UILabel hpLabel = PanelTools.findChild<UILabel>(upLevel, "hp");
        hpLabel.text = "+" + (hi.hp - baseHero.hp).ToString();

        UILabel atkLabel = PanelTools.findChild<UILabel>(upLevel, "atk");
        atkLabel.text = "+" + (hi.atk - baseHero.atk).ToString();

        UILabel defLabel = PanelTools.findChild<UILabel>(upLevel, "def");
        defLabel.text = "+" + (hi.def - baseHero.def).ToString();

        UILabel recoverLabel = PanelTools.findChild<UILabel>(upLevel, "re");
        recoverLabel.text = "+" + (hi.recover - baseHero.recover).ToString();

        UILabel levelLabel = PanelTools.findChild<UILabel>(upLevel, "level");
        levelLabel.text = "+" + curUpLevel.ToString();
    }

    protected void ShowUpSkill(int nLevel)
    {
        upSkill.SetActive(true);

        UILabel bblv = PanelTools.findChild<UILabel>(upSkill, "BBLv");
        bblv.text = "+" + (nLevel - baseHero.skillLevel).ToString();
    }


    int nGetResultExp = 0;
    bool isShowResult = false;

    protected void ShowResult()
    {
        isShowResult = true;
        HeroInfo hero = new HeroInfo();
        hero.id = baseHero.id;
        hero.type = baseHero.type;
        hero.btGrowup = baseHero.btGrowup;
        hero.level = baseHero.level;
        hero.skillLevel = baseHero.skillLevel;
        hero.exp = baseHero.exp;
        hero.equipId = baseHero.equipId;
        hero.btCollected = baseHero.btCollected;
        hero.InitDict(hero.type);

        int nCurExp = baseHero.exp;
        int nMaxExp = hero.GetLvlupExp();
        while (nGetResultExp > 0)
        {
            --nGetResultExp;
            ++hero.exp;
            if (hero.exp == nMaxExp)
            {
                hero.level += 1;
                hero.exp = 0;
                nMaxExp = hero.GetLvlupExp();
            }
        }


        if (baseHero.level != hero.level)
        {
            hero.InitDict(hero.type);
            upLevel.SetActive(true);

            UILabel hpLabel = PanelTools.findChild<UILabel>(upLevel, "hp");
            hpLabel.text = "+" + (hero.hp - baseHero.hp).ToString();

            UILabel atkLabel = PanelTools.findChild<UILabel>(upLevel, "atk");
            atkLabel.text = "+" + (hero.atk - baseHero.atk).ToString();

            UILabel defLabel = PanelTools.findChild<UILabel>(upLevel, "def");
            defLabel.text = "+" + (hero.def - baseHero.def).ToString();

            UILabel recoverLabel = PanelTools.findChild<UILabel>(upLevel, "re");
            recoverLabel.text = "+" + (hero.recover - baseHero.recover).ToString();

            UILabel levelLabel = PanelTools.findChild<UILabel>(upLevel, "level");
            levelLabel.text = "+" + (hero.level - baseHero.level).ToString();


            ShowUpLevelEffect();
        }

        float maxResultExp = hero.GetLvlupExp();
        float curResultExp = hero.exp;

        UIProgressBar pb = PanelTools.findChild<UIProgressBar>(gameObject, "Info/Progress Bar");
        pb.value = curResultExp / maxResultExp;

        UILabel leftExpLabel = PanelTools.findChild<UILabel>(gameObject, "Info/toUp");
        leftExpLabel.text = (maxResultExp - curResultExp).ToString();

        HeroInfo oldhero = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(baseHero.id);
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).heroInfoList.Remove(oldhero);
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).heroInfoList.Add(hero);
    }
}
