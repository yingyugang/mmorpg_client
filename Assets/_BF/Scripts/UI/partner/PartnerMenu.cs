using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class PartnerMenu : MonoBehaviour
{
    public UIButton btnBrowse;
    public UIButton btnTeam;
    public UIButton btnStrengthen;
    public UIButton btnEvolution;
    public UIButton btnEquip;
    public UIButton btnSell;
    public UIButton btnReturn;
    public UIButton btnClose;

    public GameObject browse;
    public GameObject strengthen;
    public GameObject evolution;
    public GameObject equip;
    public GameObject sell;
    public GameObject eidt;

    public GameObject Result;
    // Use this for initialization
	void Start ()
    {
        if (btnReturn != null)
            UIEventListener.Get(btnReturn.gameObject).onClick = onReturn;
        if (btnClose != null)
            UIEventListener.Get(btnClose.gameObject).onClick = onReturn;

        if (btnBrowse != null)
            UIEventListener.Get(btnBrowse.gameObject).onClick = onBrowse;
        if (btnStrengthen != null)
            UIEventListener.Get(btnStrengthen.gameObject).onClick = onStrengthen;
        if (btnEvolution != null)
            UIEventListener.Get(btnEvolution.gameObject).onClick = onEvolution;
        if (btnEquip != null)
            UIEventListener.Get(btnEquip.gameObject).onClick = onEquip;
        if (btnSell != null)
            UIEventListener.Get(btnSell.gameObject).onClick = onSell;
        if (btnTeam != null)
            UIEventListener.Get(btnTeam.gameObject).onClick = onTeam;

        InitUI();
    }


	// Update is called once per frame
	void Update () 
    {	
	}

    void onReturn(GameObject go)
    {
        transform.parent.gameObject.SetActive(false);
    }

    void onBrowse(GameObject go)
    {
        gameObject.SetActive(false);
        browse.SetActive(true);
        PartnerBrowse pb = browse.GetComponent<PartnerBrowse>();
        pb.ShowHeroBrowse();

//        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).ShowIllustrations();
    }

    void onStrengthen(GameObject go)
    {
        gameObject.SetActive(false);
        strengthen.SetActive(true);

        SelectBase sb = strengthen.GetComponent<SelectBase>();
        sb.ShowHeroBrowse();
    }

    void onEvolution(GameObject go)
    {
        gameObject.SetActive(false);
        evolution.SetActive(true);
        SelectEvolution se = evolution.GetComponent<SelectEvolution>();
        se.ShowHeroBrowse();
    }

    void onEquip(GameObject go)
    {
        gameObject.SetActive(false);
        equip.SetActive(true);

        PartnerEquip pe = equip.GetComponent<PartnerEquip>();
        pe.ShowEquip();
    }

    void onSell(GameObject go)
    {
        gameObject.SetActive(false);
        sell.SetActive(true);

        PartnerSell ps = sell.GetComponent<PartnerSell>();
        ps.ShowHeroBrowse();
    }

    void onTeam(GameObject go)
    {
        gameObject.SetActive(false);
        eidt.SetActive(true);
        
        PartnerEdit pe = eidt.GetComponent<PartnerEdit>();
        pe.InitShowTeam();
    }

    public void InitUI()
    {
        UILabel returnLabel = PanelTools.findChild<UILabel>(gameObject, "Titlebg/ReturnButton/Label");
        //返回
        returnLabel.text = BaseLib.LanguageMgr.getString("66361007");

        UILabel titleLabel = PanelTools.findChild<UILabel>(gameObject, "Titlebg/Label");
        //伙伴
        titleLabel.text = BaseLib.LanguageMgr.getString("66361000");

        UILabel browseLabel = PanelTools.findChild<UILabel>(gameObject, "Buttons/BrowseButton/Label");
        //伙伴一览
        browseLabel.text = BaseLib.LanguageMgr.getString("66361001");

        UILabel teamLabel = PanelTools.findChild<UILabel>(gameObject, "Buttons/TeamButton/Label");
        //伙伴一览
        teamLabel.text = BaseLib.LanguageMgr.getString("66361002");

        UILabel strengthenLabel = PanelTools.findChild<UILabel>(gameObject, "Buttons/StrengthenButton/Label");
        //强化合成
        strengthenLabel.text = BaseLib.LanguageMgr.getString("66361003");
        
        UILabel evolutionLabel = PanelTools.findChild<UILabel>(gameObject, "Buttons/EvolutionButton/Label");
        //进化合成
        evolutionLabel.text = BaseLib.LanguageMgr.getString("66361004");

        UILabel equipLabel = PanelTools.findChild<UILabel>(gameObject, "Buttons/EquipButton/Label");
        //伙伴一览
        equipLabel.text = BaseLib.LanguageMgr.getString("66361005");

        UILabel sellLabel = PanelTools.findChild<UILabel>(gameObject, "Buttons/SellButton/Label");
        //伙伴一览
        sellLabel.text = BaseLib.LanguageMgr.getString("66361006");
    }
}
