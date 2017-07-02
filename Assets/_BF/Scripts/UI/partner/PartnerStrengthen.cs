using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class PartnerStrengthen : MonoBehaviour
{

    public UIButton btnReturn;
    public UIButton btnSelectBase;
    public UIButton btnShowInfo;
    public UIButton btnComposite;

    public GameObject sourceGround1;
    public GameObject sourceGround2;
    public GameObject sourceGround3;
    public GameObject sourceGround4;
    public GameObject sourceGround5;

    public GameObject baseItem;
    public GameObject Item1;
    public GameObject Item2;
    public GameObject Item3;
    public GameObject Item4;
    public GameObject Item5;

    public List<GameObject> ItemList;

    public GameObject selectBase;
    public GameObject selectSource;
    public GameObject strengthenProcess;

    bool isShowInfo = false;
    public int idBaseHero = 0;
    public int levelBaseHero = 0;
    public int seriesBaseHero = 0;

    int nSourceExp = 0;
    int nCostGold = 0;
    int nBaseExp = 0;
    int nBaseMaxExp = 0;
    public List<uint> selList = new List<uint>();

    // Use this for initialization
	void Start ()
    {
        if (btnReturn != null)
            UIEventListener.Get(btnReturn.gameObject).onClick = onReturn;

        if (btnSelectBase != null)
            UIEventListener.Get(btnSelectBase.gameObject).onClick = onSelectBase;

        if (btnShowInfo != null)
            UIEventListener.Get(btnShowInfo.gameObject).onClick = onShowInfo;

        if (btnComposite != null)
        {
            UIEventListener.Get(btnComposite.gameObject).onClick = onComposite;
        }

        if (sourceGround1 != null)
            UIEventListener.Get(sourceGround1.gameObject).onClick = onSelectSource;
        if (sourceGround2 != null)
            UIEventListener.Get(sourceGround2.gameObject).onClick = onSelectSource;
        if (sourceGround3 != null)
            UIEventListener.Get(sourceGround3.gameObject).onClick = onSelectSource;
        if (sourceGround4 != null)
            UIEventListener.Get(sourceGround4.gameObject).onClick = onSelectSource;
        if (sourceGround5 != null)
            UIEventListener.Get(sourceGround5.gameObject).onClick = onSelectSource;


        ItemList.Add(Item1);
        ItemList.Add(Item2);
        ItemList.Add(Item3);
        ItemList.Add(Item4);
        ItemList.Add(Item5);

    }

    void LateUpdate()
    {

        foreach (KeyValuePair<int, GameObject> go in dicHeroModel)
        {
            HeroInfo.ChangeModel(go.Value, 0.1f);
        }

    }

    void onReturn(GameObject go)
    {
        gameObject.SetActive(false);
        selectBase.SetActive(true);

        SelectSource ss = selectSource.GetComponent<SelectSource>();
        ss.ClearUI();

    }

    void onSelectBase(GameObject go)
    {
        gameObject.SetActive(false);
        selectBase.SetActive(true);

        SelectSource ss = selectSource.GetComponent<SelectSource>();
        ss.ClearUI();
        ss.selectID.Clear();
    }

    void onSelectSource(GameObject go)
    {
        gameObject.SetActive(false);
        selectSource.SetActive(true);

        SelectSource ss = selectSource.GetComponent<SelectSource>();
        ss.ShowHeroBrowse();
    }

    void onShowInfo(GameObject go)
    {
        if (isShowInfo)
        {
            isShowInfo = false;
            HideInfo();
        }
        else
        {
            isShowInfo = true;
            ShowInfo();
        }
    }

    void onComposite(GameObject go)
    {
        if (selList.Count == 0)
        {
            return;
        }
        
        gameObject.SetActive(false);

        SelectSource ss = selectSource.GetComponent<SelectSource>();
        ss.ClearUI();
        
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).enhanceHero((uint)idBaseHero, selList.ToArray());
    }

    public void ClearUI()
    {
        nBaseExp = 0;
        nBaseMaxExp = 0;
        nSourceExp = 0;
        selList.Clear();

        SelectSource ss = selectSource.GetComponent<SelectSource>();
        ss.ClearUI();

        GameObject upSkill = PanelTools.findChild(gameObject, "InfoBg/upSkill");
        upSkill.SetActive(false);
    }

    Dictionary<int, GameObject> dicHeroModel = new Dictionary<int, GameObject>();
    public void ShowBase(int id)
    {
        dicHeroModel.Clear();
        idBaseHero = id;
        HeroInfo hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(id);


        //模型
        Transform modelOld = baseItem.transform.FindChild("model");
        if (modelOld != null)
        {
            Destroy(modelOld.gameObject);
        }

        if (DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroPrefabs.ContainsKey(hi.fbxFile))
        {
            GameObject modelPrefab = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroPrefabs[hi.fbxFile];
            GameObject model = NGUITools.AddChild(baseItem.gameObject, modelPrefab);
            model.transform.localScale = new UnityEngine.Vector3(35, 35, 1);
            model.transform.localPosition = new UnityEngine.Vector3(0, -180, 0);
            model.name = "model";

            dicHeroModel[hi.id] = model;
        }


        UILabel idLabel = PanelTools.findChild<UILabel>(baseItem, "idLabel");
        idLabel.text = id.ToString();

        UILabel levelLabel = PanelTools.findChild<UILabel>(baseItem, "level");
        levelBaseHero = hi.level;
        levelLabel.text = hi.level.ToString();

        UILabel hpLabel = PanelTools.findChild<UILabel>(baseItem, "hp");
        hpLabel.text = hi.hp.ToString();

        UILabel costLabel = PanelTools.findChild<UILabel>(baseItem, "cost");
        costLabel.text = hi.leader.ToString();

        UILabel atkLabel = PanelTools.findChild<UILabel>(baseItem, "atk");
        atkLabel.text = hi.atk.ToString();

        UILabel defLabel = PanelTools.findChild<UILabel>(baseItem, "def");
        defLabel.text = hi.def.ToString();

        UILabel reLabel = PanelTools.findChild<UILabel>(baseItem, "re");
        reLabel.text = hi.recover.ToString();

        UISprite iconSprite = PanelTools.findChild<UISprite>(baseItem, "icon");
        iconSprite.spriteName = hi.spriteName;

        UISprite typeSprite = PanelTools.findChild<UISprite>(baseItem, "type");
        typeSprite.spriteName = "SERIES" + hi.series.ToString();
        seriesBaseHero = hi.series;
        UISprite starSprite = PanelTools.findChild<UISprite>(baseItem, "star");
        starSprite.spriteName = "star" + hi.star.ToString();

        nBaseMaxExp = hi.GetLvlupExp();
        nBaseExp = hi.exp;
        int nLeftExp = nBaseMaxExp - hi.exp;

        UILabel leftExp = PanelTools.findChild<UILabel>(gameObject, "InfoBg/leftExp");
        leftExp.text = nLeftExp.ToString();

        UIProgressBar pb = PanelTools.findChild<UIProgressBar>(gameObject, "InfoBg/Progress Bar");
        pb.value = (float)nBaseExp / (float)nBaseMaxExp;

        ShowSource();
    }

    public void ClearSource()
    {
        foreach (GameObject go in ItemList)
        {
            go.SetActive(false);
        }

        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).srcHeroList.Clear();
    }
    
    public void ShowSource()
    {
        ClearSource();
        
        SelectSource ss = selectSource.GetComponent<SelectSource>();
        int nCount = ss.selectedItemList.Count;

        for (int i = 0; i < nCount; ++i)
        {
            int id = int.Parse(ss.selectedItemList[i].id.text);
            HeroInfo hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(id);
            selList.Add((uint)id);

            //模型
            Transform item = ItemList[i].transform;
            Transform modelOld = item.FindChild("model");

            if (modelOld != null)
            {
                Destroy(modelOld.gameObject);
            }

            if (DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroPrefabs.ContainsKey(hi.fbxFile))
            {
                GameObject modelPrefab = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroPrefabs[hi.fbxFile];
                GameObject model = NGUITools.AddChild(item.gameObject, modelPrefab);
                model.transform.localScale = new UnityEngine.Vector3(35, 35, 1);
                model.transform.localPosition = new UnityEngine.Vector3(0, 20, 0);
                model.name = "model";

                dicHeroModel[hi.id] = model;
            }

            UILabel idLabel = PanelTools.findChild<UILabel>(ItemList[i], "idLabel");
            idLabel.text = hi.id.ToString();

            UILabel levelLabel = PanelTools.findChild<UILabel>(ItemList[i], "level");
            levelLabel.text = hi.level.ToString();

            UILabel hpLabel = PanelTools.findChild<UILabel>(ItemList[i], "hp");
            hpLabel.text = hi.hp.ToString();

            UILabel costLabel = PanelTools.findChild<UILabel>(ItemList[i], "cost");
            costLabel.text = hi.leader.ToString();

            UILabel atkLabel = PanelTools.findChild<UILabel>(ItemList[i], "atk");
            atkLabel.text = hi.atk.ToString();

            UILabel defLabel = PanelTools.findChild<UILabel>(ItemList[i], "def");
            defLabel.text = hi.def.ToString();

            UILabel reLabel = PanelTools.findChild<UILabel>(ItemList[i], "re");
            reLabel.text = hi.recover.ToString();

            UISprite iconSprite = PanelTools.findChild<UISprite>(ItemList[i], "icon");
            iconSprite.spriteName = hi.spriteName;

            UISprite typeSprite = PanelTools.findChild<UISprite>(ItemList[i], "type");
            typeSprite.spriteName = "SERIES" + hi.series.ToString();

            ItemList[i].SetActive(true);

            if (DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(idBaseHero).series == hi.series)
            {
                GameObject upSkill = PanelTools.findChild(gameObject, "InfoBg/upSkill");
                upSkill.SetActive(true);
            }

            DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).srcHeroList.Add(hi.fbxFile);
        }

        //费用和经验
        UILabel gold = PanelTools.findChild<UILabel>(gameObject, "InfoBg/cost");
        UILabel getExp = PanelTools.findChild<UILabel>(gameObject, "InfoBg/getExp");

        nCostGold = ss.nCostGold;
        nSourceExp = ss.nGetExp;

        gold.text = nCostGold.ToString();
        getExp.text = ss.nGetExp.ToString();

        UISprite midSprite = PanelTools.findChild<UISprite>(gameObject, "InfoBg/Progress Bar/Midground");
        midSprite.fillAmount = ((float)nBaseExp + (float)nSourceExp) / (float)nBaseMaxExp;



    }

    public void ShowInfo()
    {
        foreach (GameObject go in ItemList)
        {
            foreach (Transform child in go.transform)
            {
                if (child.name != "icon")
                {
                    child.gameObject.SetActive(true);
                }
                
            }
        }
    }

    public void HideInfo()
    {
        foreach (GameObject go in ItemList)
        {
            foreach (Transform child in go.transform)
            {
                if (child.name != "model")
                {
                    child.gameObject.SetActive(false);
                }

            }
        }
    }
}
