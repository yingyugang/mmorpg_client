using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class Explanation : MonoBehaviour
{

    public UIButton btnReturn;
    public GameObject illustrations;

    // Use this for initialization
	void Start ()
    {
        if (btnReturn != null)
            UIEventListener.Get(btnReturn.gameObject).onClick = onReturn;
    }

	// Update is called once per frame
	void Update () 
    {	
	}

    void onReturn(GameObject go)
    {
        gameObject.SetActive(false);

        illustrations.SetActive(true);
    }

    public void ShowHeroDetail(int id)
    {
        UILabel returnLabel = PanelTools.findChild<UILabel>(gameObject, "Titlebg/ReturnButton/Label");
        //返回
        returnLabel.text = BaseLib.LanguageMgr.getString("66361007");

        UILabel titleLabel = PanelTools.findChild<UILabel>(gameObject, "Titlebg/titleLabel");
        //伙伴档案
        titleLabel.text = BaseLib.LanguageMgr.getString("66361009");

        string strTemp = "";
        ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_HERO);
        ConfigRow row = table.getRow(DICT_HERO.HERO_TYPEID, id);

        UILabel nameLabel = PanelTools.findChild<UILabel>(gameObject, "Info/nameLabel");
        strTemp = row.getStringValue(DICT_HERO.NAME_ID);
        nameLabel.text = BaseLib.LanguageMgr.getString(strTemp);

        UILabel atkLabel = PanelTools.findChild<UILabel>(gameObject, "Info/atkLabel");
        atkLabel.text = row.getStringValue(DICT_HERO.INIT_ATK);

        UILabel defLabel = PanelTools.findChild<UILabel>(gameObject, "Info/defLabel");
        defLabel.text = row.getStringValue(DICT_HERO.INIT_DEF);

        UILabel hpLabel = PanelTools.findChild<UILabel>(gameObject, "Info/hpLabel");
        hpLabel.text = row.getStringValue(DICT_HERO.INIT_HP);

        UILabel reLabel = PanelTools.findChild<UILabel>(gameObject, "Info/reLabel");
        reLabel.text = row.getStringValue(DICT_HERO.INIT_RECOVER);

        UILabel descLabel = PanelTools.findChild<UILabel>(gameObject, "Info/descLabel");
        strTemp = row.getStringValue(DICT_HERO.DESC_ID);
        descLabel.text = BaseLib.LanguageMgr.getString(strTemp);

        //模型
        Transform info = gameObject.transform.FindChild("Info");
        Transform modelOld = info.FindChild("model");
        if (modelOld != null)
        {
            Destroy(modelOld.gameObject);
        }

        string panelPrebPath = "PartnerModel/" + row.getStringValue(DICT_HERO.FBX_FILE);
        //GameObject modelPrefab = Resources.Load<GameObject>(panelPrebPath);
        GameObject modelPrefab = ResourceCenter.LoadAsset<GameObject>(panelPrebPath);
        GameObject model = NGUITools.AddChild(info.gameObject, modelPrefab);
        model.transform.localScale = new UnityEngine.Vector3(30, 30, 1);
        model.transform.localPosition = new UnityEngine.Vector3(0, 0, 0);
        model.name = "model";

        

    }

}
