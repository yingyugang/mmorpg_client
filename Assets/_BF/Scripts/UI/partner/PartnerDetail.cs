using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class PartnerDetail : MonoBehaviour
{

    public GameObject HeroModelView;
    public UIButton btnReturn;
    public UIButton btnCollected;
    public GameObject browse;
    public GameObject btnClose;

    public bool isSummonUse = false;

    public GameObject returnPage;
    HeroInfo hero = null;
    ThumbnailView tv = null;
    // Use this for initialization
	void Start ()
    {
        if (btnReturn != null)
            UIEventListener.Get(btnReturn.gameObject).onClick = onReturn;

        if (btnClose != null)
            UIEventListener.Get(btnClose).onClick = onReturn;

        if (btnCollected != null)
            UIEventListener.Get(btnCollected.gameObject).onClick = onCollected;
    }

    void OnEnable()
    {
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, false, (int)EVENT_GROUP.mainUI);
    }

    void OnDisable()
    {
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, true, (int)EVENT_GROUP.mainUI);

        if (tv != null)
        {
            NGUITools.Destroy(tv.gameObject);
        }
    }

	// Update is called once per frame
	void Update () 
    {	
	}



    void onReturn(GameObject go)
    {
        gameObject.SetActive(false);

        if (returnPage != null)
        {
            returnPage.SetActive(true);
        }
    }

    void onCollected(GameObject go)
    {
        UISprite star = PanelTools.findChild<UISprite>(btnCollected.gameObject, "star");
        UILabel label = PanelTools.findChild<UILabel>(btnCollected.gameObject, "Label");
        
        if (hero.btCollected == 0)
        {
            star.gameObject.SetActive(true);
            //取消
            label.text = BaseLib.LanguageMgr.getString("66361102") + "\n" + BaseLib.LanguageMgr.getString("66361101");

            DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).CollectedHero(hero.id, true);
        }
        else
        {
            star.gameObject.SetActive(false);
            //收藏
            label.text = BaseLib.LanguageMgr.getString("66361101");

            DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).CollectedHero(hero.id, false);
        }
    }

    public void ShowHeroDetail(int id, GameObject rePage)
    {

        returnPage = rePage;
        hero = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(id);

        UILabel idLabel = PanelTools.findChild<UILabel>(gameObject, "leftInfo/idLabel");
        idLabel.text = "Unit No." + hero.library.ToString();

//         UILabel starLabel = PanelTools.findChild<UILabel>(gameObject, "Titlebg/starLabel");
//         starLabel.text = hero.star.ToString() + "STAR";

        UISprite starSprite = PanelTools.findChild<UISprite>(gameObject, "leftInfo/star");
        starSprite.spriteName = "star" + hero.star.ToString();
        

        UILabel nameLabel = PanelTools.findChild<UILabel>(gameObject, "Titlebg/nameLabel");
        nameLabel.text = hero.name;

        UISprite seriesIcon = PanelTools.findChild<UISprite>(gameObject, "leftInfo/seriseIcon");
        seriesIcon.spriteName = "SERIESbg" + hero.series.ToString();

        //模型
        //InitFBX();
        GameObject modelPrefab = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroModel(hero.fbxFile);

        if (modelPrefab != null)
        {

            if (tv != null)
            {
                NGUITools.Destroy(tv.gameObject);
            }

            GameObject HeroModelViews = GameObject.Find("HeroModelViews");
            GameObject DeatilView = NGUITools.AddChild(HeroModelViews, HeroModelView);

            tv = DeatilView.GetComponent<ThumbnailView>();
            GameObject heroView = PanelTools.findChild(gameObject, "HeroModel/heroView");

            tv.effect = modelPrefab;
            tv.showObject = heroView;
            tv.Init();
            DeatilView.SetActive(true);
        }

        UILabel growupLabel = PanelTools.findChild<UILabel>(gameObject, "leftInfo/typeLabel");
        growupLabel.text = "Type:" + DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetGrowupStr(hero.btGrowup);

        UILabel levelLabel = PanelTools.findChild<UILabel>(gameObject, "leftInfo/levelLabel");
        levelLabel.text = hero.level.ToString() + "/" + hero.getMaxLevel().ToString();

        //剩余经验
        UILabel expLabel = PanelTools.findChild<UILabel>(gameObject, "leftInfo/expLabel");
        int nBaseMaxExp = hero.GetLvlupExp();
        int nLeftExp = nBaseMaxExp - hero.exp;
        expLabel.text = nLeftExp.ToString();

        UISlider expSlider = PanelTools.findChild<UISlider>(gameObject, "leftInfo/expBar");
        expSlider.value = (float)hero.exp / (float)nBaseMaxExp;


        UILabel hpLabel = PanelTools.findChild<UILabel>(gameObject, "rightInfo/hpLabel");
        hpLabel.text = hero.hp.ToString();

        UILabel atkLabel = PanelTools.findChild<UILabel>(gameObject, "rightInfo/atkLabel");
        atkLabel.text = hero.atk.ToString();

        UILabel defLabel = PanelTools.findChild<UILabel>(gameObject, "rightInfo/defLabel");
        defLabel.text = hero.def.ToString();

        UILabel reLabel = PanelTools.findChild<UILabel>(gameObject, "rightInfo/reLabel");
        reLabel.text = hero.recover.ToString();

        UILabel costLabel = PanelTools.findChild<UILabel>(gameObject, "rightInfo/costLabel");
        costLabel.text = "Cost:" + hero.leader.ToString();

        UILabel skillTeamLabel = PanelTools.findChild<UILabel>(gameObject, "Skill/skillNameTeam");
        skillTeamLabel.text = hero.skillCaptianName;

        UILabel skillEffectTeamLabel = PanelTools.findChild<UILabel>(gameObject, "Skill/skillEffectTeam");
        skillEffectTeamLabel.text = hero.skillCaptianDesc;

        UILabel skillNameLabel = PanelTools.findChild<UILabel>(gameObject, "Skill/skillName");
        skillNameLabel.text = hero.skillBaseName;

        UILabel skillEffectLabel = PanelTools.findChild<UILabel>(gameObject, "Skill/skillEffect");
        skillEffectLabel.text = hero.skillBaseDesc;

        UILabel skillLevel = PanelTools.findChild<UILabel>(gameObject, "Skill/skillLevel");
        skillLevel.text = "Lv." + hero.skillLevel.ToString();

        //是否收藏
//         UISprite star = PanelTools.findChild<UISprite>(btnCollected.gameObject, "star");
//         UILabel label = PanelTools.findChild<UILabel>(btnCollected.gameObject, "Label");
// 
//         if (hero.btCollected == 0)
//         {
//             star.gameObject.SetActive(false);
//             //收藏
//             label.text = BaseLib.LanguageMgr.getString("66361101");
//         }
//         else
//         {
//             star.gameObject.SetActive(true);
//             //取消
//             label.text = BaseLib.LanguageMgr.getString("66361102") + "\n" + BaseLib.LanguageMgr.getString("66361101");
//         }

    }

}
