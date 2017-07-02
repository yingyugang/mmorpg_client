using UnityEngine;
using System.Collections;
using DataCenter;

public class MainFooter : MonoBehaviour 
{
    public GameObject partner;
	public GameObject village;
	private GameObject cur;

	// Use this for initialization
	void Awake()
	{
	}
	void Start () 
    {
        if (this.village != null)
            this.village.SetActive(false);
        UI.PanelTools.setBtnFunc(transform, "btnHome", onHome);
        UI.PanelTools.setBtnFunc(transform, "btnHero", onHero);
        UI.PanelTools.setBtnFunc(transform, "btnSummons", onSummons);
        UI.PanelTools.setBtnFunc(transform, "btnCallhero", onCallHero);
        UI.PanelTools.setBtnFunc(transform, "btnShop", onShop);
        UI.PanelTools.setBtnFunc(transform, "btnFriend", onFriend);
    }
	
    void onHome(GameObject go)
    {
        panelMain.showMain();
        setCurObj(null);
    }

    void onHero(GameObject go)
    {
        panelMain.showMain();
		setCurObj(partner);
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).MainShowHeroMenu();          
    }

	void setCurObj(GameObject obj)
	{
        AudioManager.me.PlayBtnActionClip();
        UI.PanelStack.me.clear();
		if(this.cur!=null)
			this.cur.SetActive(false);
		cur = obj;
        if (cur != null)
        {
            cur.SetActive(true);
            //UI.PanelStack.me.goNext(cur, panelBackFunc);
        }
	}

    //回到主界面
    void panelBackFunc(System.Object param)
    {
        panelMain.showMain();
    }

    void onSummons(GameObject go)
    {
        panelMain.showMain();
        GameObject summons = DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).panelSummons.gameObject;

        if (summons == null)
            return;
        setCurObj(summons);
        
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowPanelSummons();
    }
    
    void onCallHero(GameObject go)
    {
        panelMain.showMain();
        setCurObj(partner);
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).ShowSummonPartner();
    }

    void onShop(GameObject go)
    {
        panelMain.showMain();
        GameObject shop = DataManager.getModule<DataShop>(DATA_MODULE.Data_Shop).panelShop.gameObject;

        if (shop == null)
            return;
        setCurObj(shop);

        DataManager.getModule<DataShop>(DATA_MODULE.Data_Shop).ShowShopMenu();
    }

    void onFriend(GameObject go)
    {
        panelMain.showMain();
        GameObject friend = DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).panelFriends.gameObject;

        if (friend == null)
            return;
        setCurObj(friend);
        
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowPanelFriends();
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).initFriendList();
    }
}
