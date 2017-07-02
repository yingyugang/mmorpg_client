using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class ShopMenu : MonoBehaviour
{

    GameObject ReturnBtn;
    GameObject CallBtn;
    GameObject DiamondBtn;
    GameObject PowerBtn;
    GameObject MaxHeroBtn;
    GameObject MaxItemBtn;
    GameObject ArenaBtn;
    GameObject TimeGift;

    // Use this for initialization
    void Start()
    {
        ReturnBtn = PanelTools.findChild(gameObject, "Title/btnBack");
        if (ReturnBtn != null)
        {
            UIEventListener.Get(ReturnBtn).onClick = onReturn;
        }

        CallBtn = PanelTools.findChild(gameObject, "Title/callButton");
        if (CallBtn != null)
        {
            UIEventListener.Get(CallBtn).onClick = onCall;
        }


        DiamondBtn = PanelTools.findChild(gameObject, "Scroll View/Grid/Diamond");
        if (DiamondBtn != null)
        {
            UIEventListener.Get(DiamondBtn).onClick = onDiamond;
        }

        PowerBtn = PanelTools.findChild(gameObject, "Scroll View/Grid/Power");
        if (PowerBtn != null)
        {
            UIEventListener.Get(PowerBtn).onClick = onPower;
        }

        MaxHeroBtn = PanelTools.findChild(gameObject, "Scroll View/Grid/MaxHero");
        if (MaxHeroBtn != null)
        {
            UIEventListener.Get(MaxHeroBtn).onClick = onMaxHero;
        }

        MaxItemBtn = PanelTools.findChild(gameObject, "Scroll View/Grid/MaxItem");
        if (MaxItemBtn != null)
        {
            UIEventListener.Get(MaxItemBtn).onClick = onMaxItem;
        }

        ArenaBtn = PanelTools.findChild(gameObject, "Scroll View/Grid/Arena");
        if (ArenaBtn != null)
        {
            UIEventListener.Get(ArenaBtn).onClick = onArena;
        }

        TimeGift = PanelTools.findChild(gameObject, "Scroll View/Grid/TimeGift");
        if (TimeGift != null)
        {
            UIEventListener.Get(TimeGift).onClick = onTimeGift;
        }
    }

    void onReturn(GameObject go)
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }

    void onCall(GameObject go)
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);

        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).ShowSummonPartner();
    }
    
    void onDiamond(GameObject go)
    {
        DataManager.getModule<DataShop>(DATA_MODULE.Data_Shop).ShowShopDiamond();
    }

    void onPower(GameObject go)
    {
        string title = "购买确认";
        string conteng = "是否花费100钻石回复所有体力？";

        DataManager.getModule<DataShop>(DATA_MODULE.Data_Shop).ShowShopComfirm(title, conteng, BuyPower);
    }

    void onMaxHero(GameObject go)
    {
        string title = "购买确认";
        string conteng = "是否花费100钻石扩充5个英雄空间？";

        DataManager.getModule<DataShop>(DATA_MODULE.Data_Shop).ShowShopComfirm(title, conteng, BuyMaxHero);

    }

    void onMaxItem(GameObject go)
    {
        string title = "购买确认";
        string conteng = "是否花费100钻石扩充5个道具空间？";

        DataManager.getModule<DataShop>(DATA_MODULE.Data_Shop).ShowShopComfirm(title, conteng, BuyMaxItem);

    }

    void onArena(GameObject go)
    {
        
    }

    void onTimeGift(GameObject go)
    {
        
    }


    void BuyMaxHero(GameObject go)
    {
        MSG_BUY_HERO_SIZE_REQUEST msg = new MSG_BUY_HERO_SIZE_REQUEST();
        NetworkMgr.sendData(msg);
    }

    void BuyMaxItem(GameObject go)
    {
        MSG_BUY_ITEM_SIZE_REQUEST msg = new MSG_BUY_ITEM_SIZE_REQUEST();
        NetworkMgr.sendData(msg);
    }

    void BuyPower(GameObject go)
    {
        DataManager.getModule<DataUser>(DATA_MODULE.Data_User).BuyPower();
    }


}
