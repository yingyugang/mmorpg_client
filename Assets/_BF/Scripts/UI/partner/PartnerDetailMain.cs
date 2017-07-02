using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class PartnerDetailMain : MonoBehaviour
{

    public UIButton btnReturn;
    public GameObject browse;

    public bool isMainUse = false;
    // Use this for initialization
	void Start ()
    {
        if (btnReturn != null)
            UIEventListener.Get(btnReturn.gameObject).onClick = onReturn;

        BaseLib.EventSystem.register((int)EVENT_MAINUI.showMainUI, onReturnMain, (int)EVENT_GROUP.mainUI);
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

	// Update is called once per frame
	void Update () 
    {	
	}

    void onReturn(GameObject go)
    {
        gameObject.SetActive(false);

        if (!isMainUse)
        {
            browse.SetActive(true);
        }
        else
        {
            transform.parent.gameObject.SetActive(false);
        }
    }

    public void ShowHeroDetail(int id)
    {
        HeroInfo hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(id);

        UILabel idLabel = PanelTools.findChild<UILabel>(gameObject, "Titlebg/idLabel");
        idLabel.text = "Unit No." + id.ToString();

        UILabel starLabel = PanelTools.findChild<UILabel>(gameObject, "Titlebg/starLabel");
        starLabel.text = hi.star.ToString() + "STAR";

        UILabel nameLabel = PanelTools.findChild<UILabel>(gameObject, "Titlebg/nameLabel");
        nameLabel.text = hi.name;

        UISprite seriesIcon = PanelTools.findChild<UISprite>(gameObject, "Titlebg/icon");

        switch (hi.series)
        {
            case 1:
                seriesIcon.spriteName = "cls_blue_38";
                break;
            case 2:
                seriesIcon.spriteName = "cls_red_38";
                break;
            case 3:
                seriesIcon.spriteName = "cls_green_38";
                break;
            case 4:
                seriesIcon.spriteName = "cls_yellow_38";
                break;
            case 5:
                seriesIcon.spriteName = "cls_white_38";
                break;
            case 6:
                seriesIcon.spriteName = "cls_purple_38";
                break;
        }

        //模型
        Transform info = gameObject.transform.FindChild("Info");
        Transform modelOld = info.FindChild("model");
        if (modelOld != null)
        {
            Destroy(modelOld.gameObject);
        }

        GameObject modelPrefab = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroPrefabs[hi.fbxFile];
        if (modelPrefab != null)
        {
            GameObject model = NGUITools.AddChild(info.gameObject, modelPrefab);
            HeroRes heroRes = model.GetComponent<HeroRes>();
//            heroRes.HideParticle();

            model.transform.localScale = new UnityEngine.Vector3(80, 80, 1);
            model.transform.localPosition = new UnityEngine.Vector3(100, -180, 0);
            model.name = "model";
        }

        UILabel growupLabel = PanelTools.findChild<UILabel>(gameObject, "Info/growup");
        growupLabel.text = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetGrowupStr(hi.btGrowup);

        UILabel levelLabel = PanelTools.findChild<UILabel>(gameObject, "Info/level");
        levelLabel.text = hi.level.ToString();

        UILabel expLabel = PanelTools.findChild<UILabel>(gameObject, "Info/exp");
        expLabel.text = hi.exp.ToString();

        UILabel hpLabel = PanelTools.findChild<UILabel>(gameObject, "Info/hp");
        hpLabel.text = hi.hp.ToString();

        UILabel atkLabel = PanelTools.findChild<UILabel>(gameObject, "Info/atk");
        atkLabel.text = hi.atk.ToString();

        UILabel defLabel = PanelTools.findChild<UILabel>(gameObject, "Info/def");
        defLabel.text = hi.def.ToString();

        UILabel reLabel = PanelTools.findChild<UILabel>(gameObject, "Info/re");
        reLabel.text = hi.recover.ToString();

        UILabel leadLabel = PanelTools.findChild<UILabel>(gameObject, "Info/lead");
        leadLabel.text = hi.leader.ToString();

        UILabel skillTeamLabel = PanelTools.findChild<UILabel>(gameObject, "Skill/skillNameTeam");
        skillTeamLabel.text = hi.skillCaptianName;

        UILabel skillEffectTeamLabel = PanelTools.findChild<UILabel>(gameObject, "Skill/skillEffectTeam");
        skillEffectTeamLabel.text = hi.skillCaptianDesc;

        UILabel skillNameLabel = PanelTools.findChild<UILabel>(gameObject, "Skill/skillName");
        skillNameLabel.text = hi.skillBaseName;

        UILabel skillEffectLabel = PanelTools.findChild<UILabel>(gameObject, "Skill/skillEffect");
        skillEffectLabel.text = hi.skillBaseDesc;
    }

}
