using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class PartnerEvolution : MonoBehaviour
{

    public UIButton btnReturn;
    public GameObject btnClose;

    public GameObject SelectEvolution;

    int nCostCoin = 0;
    HeroInfo hero = null;
    bool isAdequate = true;
    bool isEvolution = false;

    public GameObject composeBtn;
    public Material heroMaterial;

    // Use this for initialization
	void Start ()
    {
        if (btnReturn != null)
            UIEventListener.Get(btnReturn.gameObject).onClick = onReturn;
        if (btnClose != null)
            UIEventListener.Get(btnClose).onClick = onClose;

        composeBtn = PanelTools.findChild(gameObject, "InfoBg/composeButton");
        UIEventListener.Get(composeBtn).onClick = onCompose;
        

    }
	
	// Update is called once per frame
	void Update () 
    {
	}

    void onClose(GameObject go)
    {
        transform.parent.gameObject.SetActive(false);
    }
    
    void onReturn(GameObject go)
    {
        gameObject.SetActive(false);
        SelectEvolution.SetActive(true);
    }

    void onCompose(GameObject go)
    {
        if (!isEvolution)
        {
            return;
        }

        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).evolveHero((uint)hero.id, heroIdList.ToArray());

    }

    void InitUI()
    {
        nCostCoin = 0;
        isAdequate = true;
        heroIdList.Clear();
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).evlSrcList.Clear();
    }

    List<uint> heroIdList = new List<uint>();

    public void ShowBeforeInfo(int id,int nTypeId)
    {
        InitUI();
        hero = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(id);

        UILabel levelLabel = PanelTools.findChild<UILabel>(gameObject, "Before/Info/level");
        levelLabel.text = hero.level.ToString();

        UILabel hpLabel = PanelTools.findChild<UILabel>(gameObject, "Before/Info/hp");
        hpLabel.text = hero.hp.ToString();

        UILabel atkLabel = PanelTools.findChild<UILabel>(gameObject, "Before/Info/atk");
        atkLabel.text = hero.atk.ToString();

        UILabel defLabel = PanelTools.findChild<UILabel>(gameObject, "Before/Info/def");
        defLabel.text = hero.def.ToString();

        UILabel leadLabel = PanelTools.findChild<UILabel>(gameObject, "Before/Info/cost");
        leadLabel.text = hero.leader.ToString();

        UILabel reLabel = PanelTools.findChild<UILabel>(gameObject, "Before/Info/re");
        reLabel.text = hero.recover.ToString();

        UISprite star = PanelTools.findChild<UISprite>(gameObject, "Before/Info/star");
        star.spriteName = "star" + hero.star.ToString();

        UISprite typeSprite = PanelTools.findChild<UISprite>(gameObject, "Before/Info/type");
        typeSprite.spriteName = "SERIES" + hero.series.ToString();

        //模型
        Transform item = gameObject.transform.FindChild("Before");
        Transform modelOld = item.transform.FindChild("model");
        if (modelOld != null)
        {
            Destroy(modelOld.gameObject);
        }

        if (DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroPrefabs.ContainsKey(hero.fbxFile))
        {
            DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).strCurModel = hero.fbxFile;
            GameObject modelPrefab = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroPrefabs[hero.fbxFile];
            GameObject model = NGUITools.AddChild(item.gameObject, modelPrefab);
            model.transform.localScale = new UnityEngine.Vector3(35, 35, 1);
            model.transform.localPosition = new UnityEngine.Vector3(-150, 20, 0);
            model.name = "model";

            HeroInfo.ChangeModel(model, 1/35);
        }


        //进化后信息
        //配置表
        ConfigTable evolveTable = ConfigMgr.getConfig(CONFIG_MODULE.DICT_HERO_EVOLVE);
        ConfigRow evolveRow = evolveTable.getRow(DICT_HERO_EVOLVE.HERO_TYPEID, nTypeId);
        int nEvolveTypeId = evolveRow.getIntValue(DICT_HERO_EVOLVE.EVOLVE_TYPEID);
        nCostCoin = evolveRow.getIntValue(DICT_HERO_EVOLVE.COIN);

        UILabel costLabel = PanelTools.findChild<UILabel>(gameObject, "InfoBg/cost");
        costLabel.text = nCostCoin.ToString();

        UILabel skill1 = PanelTools.findChild<UILabel>(gameObject, "InfoBg/skill1");
        UILabel skill2 = PanelTools.findChild<UILabel>(gameObject, "InfoBg/skill2");
        skill1.text = hero.skillLevel.ToString();
        skill2.text = (hero.skillLevel / 2 + 1).ToString();

        ConfigTable heroTable = ConfigMgr.getConfig(CONFIG_MODULE.DICT_HERO);
        ConfigRow heroRow = heroTable.getRow(DICT_HERO.HERO_TYPEID, nEvolveTypeId);

        UILabel aflevel = PanelTools.findChild<UILabel>(gameObject, "after/Info/level");
        aflevel.text = "1";

        UILabel afhp = PanelTools.findChild<UILabel>(gameObject, "after/Info/hp");
        afhp.text = heroRow.getStringValue(DICT_HERO.INIT_HP);

        UILabel afatk = PanelTools.findChild<UILabel>(gameObject, "after/Info/atk");
        afatk.text = heroRow.getStringValue(DICT_HERO.INIT_ATK);

        UILabel afdef = PanelTools.findChild<UILabel>(gameObject, "after/Info/def");
        afdef.text = heroRow.getStringValue(DICT_HERO.INIT_DEF);

        UILabel aflead = PanelTools.findChild<UILabel>(gameObject, "after/Info/cost");
        aflead.text = heroRow.getStringValue(DICT_HERO.LEADER);

        UILabel afre = PanelTools.findChild<UILabel>(gameObject, "after/Info/re");
        afre.text = heroRow.getStringValue(DICT_HERO.INIT_RECOVER);

        UISprite astar = PanelTools.findChild<UISprite>(gameObject, "after/Info/star");
        astar.spriteName = "star" + (hero.star + 1).ToString();

        UISprite atypeSprite = PanelTools.findChild<UISprite>(gameObject, "after/Info/type");
        atypeSprite.spriteName = "SERIES" + hero.series.ToString();

        //模型
        Transform aitem = gameObject.transform.FindChild("after");
        Transform amodelOld = aitem.FindChild("model");
        if (amodelOld != null)
        {
            Destroy(amodelOld.gameObject);
        }

        string apanelPrebPath = heroRow.getStringValue(DICT_HERO.PORTARAIT);

        if (DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroPrefabs.ContainsKey(apanelPrebPath))
        {
            DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).strEvlModel = apanelPrebPath;
            GameObject amodelPrefab = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroPrefabs[apanelPrebPath];
            GameObject amodel = NGUITools.AddChild(aitem.gameObject, amodelPrefab);
            amodel.transform.localScale = new UnityEngine.Vector3(35, 35, 1);
            amodel.transform.localPosition = new UnityEngine.Vector3(150, 20, 0);
            amodel.name = "model";

            HeroInfo.ChangeModel(amodel, 1.0f/35.0f);
            HeroInfo.ChangeWhiteModel(amodel, heroMaterial);
        }



        //素材
        HeroInfo hi = new HeroInfo();

        UISprite icon1 = PanelTools.findChild<UISprite>(gameObject, "Source/Item1/icon");
        UILabel label1 = PanelTools.findChild<UILabel>(gameObject, "Source/Item1/Label");
        UISprite seriesSprite1 = PanelTools.findChild<UISprite>(gameObject, "Source/Item1/seriesSprite");
        UISprite frame1 = PanelTools.findChild<UISprite>(gameObject, "Source/Item1/frameSprite");
        UISprite framebg1 = PanelTools.findChild<UISprite>(gameObject, "Source/Item1/framebg");

        int nSourceTypeId1 = evolveRow.getIntValue(DICT_HERO_EVOLVE.COST_HERO_1);

        if (nSourceTypeId1 != 0)
        {
            hi.InitDict(nSourceTypeId1);
            icon1.spriteName = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetPortaraitByTypeId(nSourceTypeId1);
            int nCount = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).CountHeroType(nSourceTypeId1);
            label1.text = nCount.ToString();

            frame1.spriteName = "frame" + hi.series.ToString();
            framebg1.spriteName = "framebg" + hi.series.ToString();
            seriesSprite1.spriteName = "SERIES" + hi.series.ToString();

            if (nCount == 0)
            {
                isAdequate = false;
            }

            HeroInfo hero1 = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoByTypeId(nSourceTypeId1);
            if (hero1 != null)
            {
                heroIdList.Add((uint)hero1.id);
                DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).evlSrcList.Add(hero1.id);
            }
        }
        else
        {
            icon1.transform.parent.gameObject.SetActive(false);
        }

        UISprite icon2 = PanelTools.findChild<UISprite>(gameObject, "Source/Item2/icon");
        UILabel label2 = PanelTools.findChild<UILabel>(gameObject, "Source/Item2/Label");
        UISprite seriesSprite2 = PanelTools.findChild<UISprite>(gameObject, "Source/Item2/seriesSprite");
        UISprite frame2 = PanelTools.findChild<UISprite>(gameObject, "Source/Item2/frameSprite");
        UISprite framebg2 = PanelTools.findChild<UISprite>(gameObject, "Source/Item2/framebg");

        int nSourceTypeId2 = evolveRow.getIntValue(DICT_HERO_EVOLVE.COST_HERO_2);
        if (nSourceTypeId2 != 0)
        {
            hi.InitDict(nSourceTypeId2);
            icon2.spriteName = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetPortaraitByTypeId(nSourceTypeId2);
            int nCount = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).CountHeroType(nSourceTypeId2);
            label2.text = nCount.ToString();
            frame2.spriteName = "frame" + hi.series.ToString();
            framebg2.spriteName = "framebg" + hi.series.ToString();
            seriesSprite2.spriteName = "SERIES" + hi.series.ToString();
            if (nCount == 0)
            {
                isAdequate = false;
            }

            HeroInfo hero2 = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoByTypeId(nSourceTypeId2);
            if (hero2 != null)
            {
                heroIdList.Add((uint)hero2.id);
                DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).evlSrcList.Add(hero2.id);
            }
            
        }
        else
        {
            icon2.transform.parent.gameObject.SetActive(false);
        }

        UISprite icon3 = PanelTools.findChild<UISprite>(gameObject, "Source/Item3/icon");
        UILabel label3 = PanelTools.findChild<UILabel>(gameObject, "Source/Item3/Label");
        UISprite seriesSprite3 = PanelTools.findChild<UISprite>(gameObject, "Source/Item3/seriesSprite");
        UISprite frame3 = PanelTools.findChild<UISprite>(gameObject, "Source/Item3/frameSprite");
        UISprite framebg3 = PanelTools.findChild<UISprite>(gameObject, "Source/Item3/framebg");
        int nSourceTypeId3 = evolveRow.getIntValue(DICT_HERO_EVOLVE.COST_HERO_3);
        if (nSourceTypeId3 != 0)
        {
            hi.InitDict(nSourceTypeId3);
            icon3.spriteName = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetPortaraitByTypeId(nSourceTypeId3);
            int nCount = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).CountHeroType(nSourceTypeId3);
            label3.text = nCount.ToString();
            frame3.spriteName = "frame" + hi.series.ToString();
            framebg3.spriteName = "framebg" + hi.series.ToString();
            seriesSprite3.spriteName = "SERIES" + hi.series.ToString();
            if (nCount == 0)
            {
                isAdequate = false;
            }

            HeroInfo hero3 = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoByTypeId(nSourceTypeId3);
            if (hero3 != null)
            {
                heroIdList.Add((uint)hero3.id);
                DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).evlSrcList.Add(hero3.id);
            }
        }
        else
        {
            icon3.transform.parent.gameObject.SetActive(false);
        }

        UISprite icon4 = PanelTools.findChild<UISprite>(gameObject, "Source/Item4/icon");
        UILabel label4 = PanelTools.findChild<UILabel>(gameObject, "Source/Item4/Label");
        UISprite seriesSprite4 = PanelTools.findChild<UISprite>(gameObject, "Source/Item4/seriesSprite");
        UISprite frame4 = PanelTools.findChild<UISprite>(gameObject, "Source/Item4/frameSprite");
        UISprite framebg4 = PanelTools.findChild<UISprite>(gameObject, "Source/Item4/framebg");
        int nSourceTypeId4 = evolveRow.getIntValue(DICT_HERO_EVOLVE.COST_HERO_4);
        if (nSourceTypeId4 != 0)
        {
            hi.InitDict(nSourceTypeId4);
            icon4.spriteName = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetPortaraitByTypeId(nSourceTypeId4);
            int nCount = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).CountHeroType(nSourceTypeId4);
            label4.text = nCount.ToString();
            frame4.spriteName = "frame" + hi.series.ToString();
            framebg4.spriteName = "framebg" + hi.series.ToString();
            seriesSprite4.spriteName = "SERIES" + hi.series.ToString();

            if (nCount == 0)
            {
                isAdequate = false;
            }

            HeroInfo hero4 = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoByTypeId(nSourceTypeId4);
            if (hero4 != null)
            {
                heroIdList.Add((uint)hero4.id);
                DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).evlSrcList.Add(hero4.id);
            }
        }
        else
        {
            icon4.transform.parent.gameObject.SetActive(false);
        }

        UISprite icon5 = PanelTools.findChild<UISprite>(gameObject, "Source/Item5/icon");
        UILabel label5 = PanelTools.findChild<UILabel>(gameObject, "Source/Item5/Label");
        UISprite seriesSprite5 = PanelTools.findChild<UISprite>(gameObject, "Source/Item5/seriesSprite");
        UISprite frame5 = PanelTools.findChild<UISprite>(gameObject, "Source/Item5/frameSprite");
        UISprite framebg5 = PanelTools.findChild<UISprite>(gameObject, "Source/Item5/framebg");
        int nSourceTypeId5 = evolveRow.getIntValue(DICT_HERO_EVOLVE.COST_HERO_5);
        if (nSourceTypeId5 != 0)
        {
            hi.InitDict(nSourceTypeId5);
            icon5.spriteName = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetPortaraitByTypeId(nSourceTypeId5);
            int nCount = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).CountHeroType(nSourceTypeId5);
            label5.text = nCount.ToString();
            frame5.spriteName = "frame" + hi.series.ToString();
            framebg5.spriteName = "framebg" + hi.series.ToString();
            seriesSprite5.spriteName = "SERIES" + hi.series.ToString();
            if (nCount == 0)
            {
                isAdequate = false;
            }

            HeroInfo hero5 = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoByTypeId(nSourceTypeId5);
            if (hero5 != null)
            {
                heroIdList.Add((uint)hero5.id);
                DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).evlSrcList.Add(hero5.id);
            }
        }
        else
        {
            icon5.transform.parent.gameObject.SetActive(false);
        }


        isEvolution = EvolutionaryConditions();
    }

    public bool EvolutionaryConditions()
    {
        UILabel conditionLabel = PanelTools.findChild<UILabel>(gameObject, "InfoBg/condition");

        if (!hero.isMaxLevel())
        {
            conditionLabel.text = "等级不足";
            return false;
        }
        
        if ((ulong)nCostCoin > DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.goldCoin)
        {
            conditionLabel.text = "金钱不足";
            return false;
        }

        if (!isAdequate)
        {
            conditionLabel.text = "素材不足";
            return false;
        }

        conditionLabel.text = "";
        
        return true;
    }
}
