using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

[ExecuteInEditMode]
public class BattleResult : MonoBehaviour {

	public UILabel LabelStageName;
	public UILabel LabelSubStageName;
	public UILabel LabelCoin;
	public UILabel LabelSoul;
	public UILabel LabelExp;
	public UIProgressBar ProgressExp;
	public UILabel LabelLevelExp;
	public UILabel LabelHint0;
	public UILabel LabelHint1;
	public UILabel LabelHint2;
	public UILabel LabelHint3;
    public GameObject Congratulation;

	// Use this for initialization
	void Start () {
        BaseLib.EventSystem.register((int)EVENT_MAINUI.showMainUI, onReturnMain, (int)EVENT_GROUP.mainUI);

        UIEventListener.Get(gameObject).onClick = onReturn;

        InitUI();
	}

    void onReturnMain(int eventId, System.Object param)
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, false, (int)EVENT_GROUP.mainUI);
    }

    void OnDisable()
    {
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, true, (int)EVENT_GROUP.mainUI);
    }

    float time = 0; 
    
    void onReturn(GameObject go)
    {

        if (time + 1 < Time.time)
        {
            ++nState;
            time = Time.time;
        }
    }

    void onNext(GameObject go)
    {
        ClearUI();
    }

    void onAgain(GameObject go)
    {
        ClearUI();
    }

    void onExit(GameObject go)
    {
        ClearUI();

        GameObject dungeon = UI.PanelStack.me.FindPanel("Scale/PanelStageMgr/PanelDungeon");
        DungeonMgr dungeonmgr = dungeon.GetComponent<DungeonMgr>();

        int battleID = DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).currBattleID;
        dungeonmgr.init(battleID);
        
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showHeader, true, (int)EVENT_GROUP.mainUI);
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, false, (int)EVENT_GROUP.mainUI);
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, true, (int)EVENT_GROUP.mainUI);
        UI.PanelStack.me.goNext(dungeon,onReturn);

    }

    void onReturn(System.Object param)
    {
        GameObject PanelStageMap = UI.PanelStack.me.FindPanel("Scale/PanelStageMgr/PanelStageMap");
        PanelStageMap.GetComponent<StageMgr>().initStageMgr(DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).currAreaID);
        //PanelStageMap.SetActive(true);
    }

	public bool IsLoad;
	// Update is called once per frame
	void Update () {

        if (gameObject.activeSelf && isStartUpdate)
        {
            Congratulation.SetActive(true);
            
            if (nState == 1)
            {
                UpdateProgress();
            }
            else if (nState == 2)
            {
                UpdateOnce();
            }
            else if (nState == 3)
            {
                return;
            }
            else if (nState == 4)
            {
                BattleInfo.SetActive(false);
                MaterialInfo.SetActive(true);
                ShowMaterial();
                nState = 5;

                if (bri.partnerList.Count == 0)
                {
                    nState = 7;
                }
            }
            else if (nState == 5)
            {
                return;
            }
            else if (nState == 6)
            {
                MaterialInfo.SetActive(false);
                PartnerInfo.SetActive(true);
                ShowPartner();
                nState = 7;
            }
            else if (nState == 7)
            {
                return;
            }
            else if (nState == 8)
            {
                Rewarded.SetActive(false);
                MaterialInfo.SetActive(false);
                PartnerInfo.SetActive(false);
                NextLevel.SetActive(true);
            }
            
        }


#if UNITY_EDITOR
		if(IsLoad)
		{
			LabelStageName = transform.FindChild("LabelStage").GetComponent<UILabel>();
			LabelSubStageName = transform.FindChild("LabelSubStage").GetComponent<UILabel>();
			LabelCoin = transform.FindChild("LabelCoinNum").GetComponent<UILabel>();
			LabelSoul = transform.FindChild("LabelSoulNum").GetComponent<UILabel>();
			LabelExp = transform.FindChild("LabelExpNum").GetComponent<UILabel>();
			ProgressExp = transform.FindChild("ProgressExp").GetComponent<UIProgressBar>();
			LabelLevelExp = transform.FindChild("LabelLeveExp").GetComponent<UILabel>();
			LabelHint0 = transform.FindChild("LabelHint0").GetComponent<UILabel>();
			LabelHint1 = transform.FindChild("LabelHint1").GetComponent<UILabel>();
			LabelHint2 = transform.FindChild("LabelHint2").GetComponent<UILabel>();
			LabelHint3 = transform.FindChild("LabelHint3").GetComponent<UILabel>();
			IsLoad = false;
		}
#endif
// 		if(Input.GetMouseButtonUp(0))
// 		{
// 			mInProgress = false;
// 		}
// 		if(Input.GetKeyDown(KeyCode.H))
// 		{
//             CurrentStage = "伊古尼亚瀑布";
//             CurrentSubStage = "落水的冲击";
// 			BattleCoin = 40000;
// 			BattleSoul = 10000;
// 			Exp = 4000;
// 			CurrentLevel = 1;
// 			BaseExpPerLevel = 100;
// 			AdjustEXPPerLevel = 0.2f;
// 			StartCoroutine(_ProgressExp());
// 			StartCoroutine(_ProgressCoinAndSoul());
// 		}
	}


	public int Level = 1;
	void ShowLevelUpExpRemain(int num)
	{
		LabelLevelExp.gameObject.SetActive(true);
		LabelLevelExp.text = num.ToString();
	}

    BattleResultInfo bri = new BattleResultInfo();

    public void UpdateUI()
    {
        bri = DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).GetBattleResult();

        LabelStageName.text = bri.BattleName;
        LabelSubStageName.text = bri.FieldName;

        coinRate = throughTime / bri.gold;
        soulRate = throughTime / bri.soul;
        expRate = throughTime / bri.exp;

        curExp = (int)DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.curexp;
        curLevel = (int)DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.level;

        levelExp = GetMaxExpByLevel(curLevel);

        ProgressExp.value = (float)curExp / (float)levelExp;
        leftExp = levelExp - curExp;
        LabelLevelExp.text = leftExp.ToString();

        isStartUpdate = true;
    }


    public GameObject BattleInfo;
    public GameObject MaterialInfo;
    public GameObject PartnerInfo;
    public GameObject NextLevel;
    public GameObject Rewarded;

    public void InitUI()
    {
        BattleInfo = PanelTools.findChild(gameObject, "BattleInfo");
        BattleInfo.SetActive(true);
        MaterialInfo = PanelTools.findChild(gameObject, "MaterialInfo");
        MaterialInfo.SetActive(false);
        PartnerInfo = PanelTools.findChild(gameObject, "PartnerInfo");
        PartnerInfo.SetActive(false);

        NextLevel = PanelTools.findChild(gameObject, "NextLevel");
        NextLevel.SetActive(false);

        Rewarded = PanelTools.findChild(gameObject, "Rewarded");
        Rewarded.SetActive(true);

        GameObject nextBtn = PanelTools.findChild(NextLevel, "nextBtn");
        UIEventListener.Get(nextBtn).onClick = onNext;
        GameObject againBtn = PanelTools.findChild(NextLevel, "againBtn");
        UIEventListener.Get(againBtn).onClick = onAgain;
        GameObject exitBtn = PanelTools.findChild(NextLevel, "exitBtn");
        UIEventListener.Get(exitBtn).onClick = onExit;


    }

    public void ClearUI()
    {
        BattleInfo.SetActive(true);
        Rewarded.SetActive(true);
        MaterialInfo.SetActive(false);
        PartnerInfo.SetActive(false);
        NextLevel.SetActive(false);
        Congratulation.SetActive(false);
        gameObject.SetActive(false);
        nState = 1;
        isStartUpdate = false;
    }


    public int GetMaxExpByLevel(int nLevel)
    {
        int nExp = 0;

        ConfigTable userLevelTable = ConfigMgr.getConfig(CONFIG_MODULE.DICT_USER_LEVEL);
        ConfigRow configRow = userLevelTable.getRow(DICT_USER_LEVEL.LEVEL  , nLevel);
        nExp = configRow.getIntValue(DICT_USER_LEVEL.EXP);

        return nExp;
    }



    int curExp = 0;       //当前经验
    int levelExp = 0;    //升级经验
    int curLevel = 0;    //当前等级
    int leftExp = 0;

    float throughTime = 3.0f;
    float cumulativeTime = 0;
    float coinRate = 0;
    float soulRate = 0;
    float expRate = 0;
    int coinTimes = 1;
    int soulTimes = 1;
    int expTimes = 1;
    int cumulativeCoin = 0;
    int cumulativeSoul = 0;
    int cumulativeExp = 0;
    int cumulativePro = 0;
    int nState = 1;
    bool isStartUpdate = false;

    public void UpdateProgress()
    {
        if (bri == null || cumulativeTime > throughTime)
        {
            nState = 2;
            return;
        }

        cumulativeTime += Time.deltaTime;

        while (cumulativeTime > coinRate * coinTimes)
        {
            if (cumulativeCoin < bri.gold)
            {
                ++cumulativeCoin;
                LabelCoin.text = cumulativeCoin.ToString();
            }
            ++coinTimes;
        }

        while (cumulativeTime > soulRate * soulTimes)
        {
            if (cumulativeSoul < bri.soul)
            {
                ++cumulativeSoul;
                LabelSoul.text = cumulativeSoul.ToString();
            }
            ++soulTimes;
        }

        while (cumulativeTime > expRate * expTimes)
        {
            if (cumulativeExp < bri.exp)
            {
                ++cumulativeExp;
                LabelExp.text = cumulativeExp.ToString();

                ++cumulativePro;

                if (curExp + cumulativePro > levelExp)
                {
                    curExp = 0;
                    cumulativePro = 0;
                }

                ProgressExp.value = ((float)curExp + (float)cumulativePro) / (float)levelExp;
                --leftExp;

                if (leftExp <= 0)
                {
                    LabelHint0.text = "等级上升到了[2894FF]" + curLevel + "[FFFFFF]级！";
                    LabelHint0.gameObject.SetActive(true);

                    ++curLevel;

                    levelExp = GetMaxExpByLevel(curLevel);
                    leftExp = levelExp;
                }

                LabelLevelExp.text = leftExp.ToString();

            }
            ++expTimes;
        }

        //Debug.Log("cumulativePro:" + cumulativePro.ToString());
    }

    void UpdateOnce()
    {
        LabelCoin.text = bri.gold.ToString();
        LabelSoul.text = bri.soul.ToString();
        LabelExp.text = bri.exp.ToString();

        cumulativePro = bri.exp;
        curExp = (int)DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.curexp;
        curLevel = (int)DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.level;

        levelExp = GetMaxExpByLevel(curLevel);

        if (curExp + cumulativePro > levelExp)
        {

            while (curExp + cumulativePro > levelExp)
            {
                cumulativePro -= levelExp;
                ++curLevel;
                levelExp = GetMaxExpByLevel(curLevel);
            }
            
            ProgressExp.value = (float)(curExp + cumulativePro) / (float)levelExp;

            leftExp = levelExp - curExp - cumulativePro;
            LabelHint0.text = "等级上升到了[2894FF]" + Level + "[FFFFFF]级！";
            LabelHint0.gameObject.SetActive(true);
        }
        else
        {
            leftExp = levelExp - curExp - cumulativePro;
            ProgressExp.value = ((float)curExp + (float)cumulativePro) / (float)levelExp;
        }

        LabelLevelExp.text = leftExp.ToString();

        Debug.Log("cumulativePro:" + cumulativePro.ToString());

        nState = 3;
    }

    public class row
    {
        public GameObject root;
        public int child = 0;

        public void Release()
        {
            if (root != null)
            {
                GameObject.Destroy(root);
            }
        }
    }

    public class MaterialItem
    {
        public GameObject root;
        public UILabel id;
        public UISprite icon;
        public UILabel name;
        public UILabel count;
    }

    public class PartnerItem
    {
        public GameObject root;
        public UILabel id;
        public UISprite icon;
        public UISprite frame;
        public UISprite framebg;
        public UISprite star;
        public UISprite series;
        public UILabel level;
    }

    void ShowMaterial()
    {
        GameObject grid = PanelTools.findChild(MaterialInfo, "ItemList/ItemListGrid");
        GameObject Item = PanelTools.findChild(MaterialInfo, "ItemList/row");

        int nCount = bri.materialList.Count;

        for (int i = 0; i < (nCount / 5 + 1); ++i)
        {
            row _row = new row();

            _row.root = NGUITools.AddChild(grid, Item);
            _row.root.name = i.ToString();

            Vector3 position = _row.root.transform.position;
            _row.root.transform.localPosition = new UnityEngine.Vector3(position.x, position.y - i * 120, position.z);

            for (int j = i * 5; j < (i * 5 + 5); ++j)
            {
                if (j >= nCount)
                {
                    return;
                }

                _row.root.SetActive(true);

                BattleMaterial bm = bri.materialList[j];

                //取配置表
                ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_ITEM);
                ConfigRow configRow = table.getRow(DICT_ITEM.ITEM_TYPEID, bm.id);

                //更新物体信息
                MaterialItem mi = new MaterialItem();
                mi.root = PanelTools.findChild(_row.root, _row.child.ToString());
                mi.id = PanelTools.findChild<UILabel>(mi.root, "id");
                mi.id.text = bm.id.ToString();

                mi.count = PanelTools.findChild<UILabel>(mi.root, "count");
                mi.count.text = "×" + bm.count.ToString();

                mi.name = PanelTools.findChild<UILabel>(mi.root, "nameLabel");
                mi.name.text = BaseLib.LanguageMgr.getString(configRow.getStringValue(DICT_ITEM.NAME_ID));

                mi.icon = PanelTools.findChild<UISprite>(mi.root, "icon");
                mi.icon.spriteName = configRow.getStringValue(DICT_ITEM.ITEM_TYPEID);
                mi.icon.MakePixelPerfect();

                mi.root.SetActive(true);
                ++_row.child;
            }
        }
    }

    void ShowPartner()
    {
        GameObject grid = PanelTools.findChild(PartnerInfo, "ItemList/ItemListGrid");
        GameObject Item = PanelTools.findChild(PartnerInfo, "ItemList/row");

        int nCount = bri.partnerList.Count;

        for (int i = 0; i < (nCount / 5 + 1); ++i)
        {
            row _row = new row();

            _row.root = NGUITools.AddChild(grid, Item);
            _row.root.name = i.ToString();

            Vector3 position = _row.root.transform.position;
            _row.root.transform.localPosition = new UnityEngine.Vector3(position.x, position.y - i * 120, position.z);


            for (int j = i * 5; j < (i * 5 + 5); ++j)
            {
                if (j >= nCount)
                {
                    return;
                }

                _row.root.SetActive(true);

                //取配置表
                ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_HERO);
                ConfigRow configRow = table.getRow(DICT_HERO.HERO_TYPEID, bri.partnerList[j]);

                //更新物体信息
                PartnerItem pi = new PartnerItem();
                pi.root = PanelTools.findChild(_row.root, _row.child.ToString());
                pi.id = PanelTools.findChild<UILabel>(pi.root, "id");
                pi.id.text = bri.partnerList[j].ToString();

                pi.icon = PanelTools.findChild<UISprite>(pi.root, "icon");
                pi.icon.spriteName = configRow.getStringValue(DICT_HERO.PORTARAIT);

                pi.star = PanelTools.findChild<UISprite>(pi.root, "star");
                pi.star.spriteName = configRow.getStringValue(DICT_HERO.STAR);

                pi.level = PanelTools.findChild<UILabel>(pi.root, "level");
                pi.level.text = "Lv.1";

                pi.frame = PanelTools.findChild<UISprite>(pi.root, "frame");
                pi.frame.spriteName = "frame" + configRow.getStringValue(DICT_HERO.SERIES);

                pi.framebg = PanelTools.findChild<UISprite>(pi.root, "framebg");
                pi.framebg.spriteName = "framebg" + configRow.getStringValue(DICT_HERO.SERIES);

                pi.series = PanelTools.findChild<UISprite>(pi.root, "series");
                pi.series.spriteName = "SERIES" + configRow.getStringValue(DICT_HERO.SERIES);
                
                pi.root.SetActive(true);
                ++_row.child;
            }



        }

    }
}
