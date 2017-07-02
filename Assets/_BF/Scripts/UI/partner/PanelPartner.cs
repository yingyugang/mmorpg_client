using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class PanelPartner : MonoBehaviour {

    public GameObject partnerMenu;
    public GameObject partnerSummon;
    public GameObject friendPartner;
    public GameObject partnerDetail;
    public GameObject partnerEdit;
    public List<GameObject> partnerUI;

    public GameObject CallHeroBrushs;
    public GameObject StrengthenEffect;
    public GameObject Evolution;
    
    // Use this for initialization
	void Start () {

        PartnerMenu pm = partnerMenu.GetComponent<PartnerMenu>();
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).UIpartnerMenu = pm;

        SummonPartner summon = partnerSummon.GetComponent<SummonPartner>();
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).summon = summon;

        FriendPartner friendHero = friendPartner.GetComponent<FriendPartner>();
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).friendHero = friendHero;

        PartnerDetail pd = partnerDetail.GetComponent<PartnerDetail>();
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).UIpartnerDetail = pd;

        PartnerEdit pe = partnerEdit.GetComponent<PartnerEdit>();
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).UIpartnerEdit = pe;

        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).panelPartner = gameObject;
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).UIPanelPartner = this;
        BaseLib.EventSystem.register((int)EVENT_MAINUI.showMainUI, onReturnMain, (int)EVENT_GROUP.mainUI);

        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).equipDate();

        gameObject.SetActive(false);
    }

    void onReturnMain(int eventId, System.Object param)
    {
        InitUI();
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
	void Update () {
	
	}

    public void InitUI()
    {
        foreach (GameObject go in partnerUI)
        {
            if (go == null)
            {
                continue;
            }
            
            if (go.name == "PartnerMenu")
            {
                go.SetActive(true);
            }
            else
            {
                go.SetActive(false);
            }
        }
    }
}
