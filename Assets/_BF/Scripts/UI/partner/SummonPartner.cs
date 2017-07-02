using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class SummonPartner : MonoBehaviour {

    public UIButton btnReturn;
    public GameObject btnClose;

    public GameObject summonMenu;
    public GameObject rareSummon;
    public GameObject friendSummon;
    public UIButton oneRareBtn;
    public UIButton tenRareBtn;
    public UIButton oneFriendBtn;
    public UIButton eightFriendBtn;


    GameObject expSummonPage;
    GameObject expSummonBtn;
    GameObject onceExpBtn;
    GameObject fiveExpBtn;

    GameObject titleBg;
    GameObject leftButton;
    GameObject rightButton;
    GameObject grid;


    GameObject tag1;
    GameObject tag2;
    GameObject tag3;

    int nState = 1;

    // Use this for initialization
	void Start () {

        if (btnReturn != null)
            UIEventListener.Get(btnReturn.gameObject).onClick = onReturn;

        if (btnClose != null)
        {
            UIEventListener.Get(btnClose).onClick = onReturn;
        }


        if (oneFriendBtn != null)
            UIEventListener.Get(oneFriendBtn.gameObject).onClick = onOnceFriend;
        if (oneRareBtn != null)
            UIEventListener.Get(oneRareBtn.gameObject).onClick = onOnceRare;
        if (tenRareBtn != null)
            UIEventListener.Get(tenRareBtn.gameObject).onClick = onTenRare;

        if (eightFriendBtn != null)
            UIEventListener.Get(eightFriendBtn.gameObject).onClick = onEightFriend;


        GameObject tags = PanelTools.findChild(gameObject, "tags");
        leftButton = PanelTools.findChild(tags, "leftButton");
        rightButton = PanelTools.findChild(tags, "rightButton");
        tag1 = PanelTools.findChild(tags, "tag1");
        tag2 = PanelTools.findChild(tags, "tag2");
        tag3 = PanelTools.findChild(tags, "tag3");

        grid = PanelTools.findChild(gameObject, "Scroll View/Grid");
        UIEventListener.Get(leftButton).onClick = onLeft;
        UIEventListener.Get(rightButton).onClick = onRight;

//         expSummonPage = PanelTools.findChild(gameObject, "Scroll View/Grid/cexpSummon");
//         onceExpBtn = PanelTools.findChild(expSummonPage, "onceButton");
//         UIEventListener.Get(onceExpBtn).onClick = onOnceExpSummon;
//         fiveExpBtn = PanelTools.findChild(expSummonPage, "fiveButton");
//         UIEventListener.Get(fiveExpBtn).onClick = onFiveExpSummon;

        InitUI();
	}


    public void InitUI()
    {
        nState = 1;
    }

    void onReturn(GameObject go)
    {
        transform.parent.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    void onFriend(GameObject go)
    {
        summonMenu.SetActive(false);
        friendSummon.SetActive(true);
        nState = 3;

        UILabel friendLabel = PanelTools.findChild<UILabel>(friendSummon, "friendLabel");
        //友情点：
        friendLabel.text = BaseLib.LanguageMgr.getString("66361202") +
            DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.friendPt.ToString();
    }

    void onExpPage(GameObject go)
    {
        summonMenu.SetActive(false);
        expSummonPage.SetActive(true);
        nState = 4;

        UILabel diamondLabel = PanelTools.findChild<UILabel>(expSummonPage, "diamondLabel");
        //钻石数：
        diamondLabel.text = BaseLib.LanguageMgr.getString("66361201") +
            DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.diamond.ToString();

    }

    void onRare(GameObject go)
    {
        summonMenu.SetActive(false);
        rareSummon.SetActive(true);
        nState = 2;

        UILabel diamondLabel = PanelTools.findChild<UILabel>(rareSummon, "diamondLabel");
        //钻石数：
        diamondLabel.text = BaseLib.LanguageMgr.getString("66361201") +
            DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.diamond.ToString();

    }

    void onOnceExpSummon(GameObject go)
    {

        UILabel oneCost = PanelTools.findChild<UILabel>(expSummonPage, "oneCost");

        int nCost = int.Parse(oneCost.text);

        if (nCost > DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.diamond)
        {
            return;
        }
        
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).getNewExpCard(0);
    }

    void onFiveExpSummon(GameObject go)
    {
        UILabel fiveCost = PanelTools.findChild<UILabel>(expSummonPage, "fiveCost");

        int nCost = int.Parse(fiveCost.text);

        if (nCost > DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.diamond)
        {
            return;
        }

        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).getNewExpCard(1);
    }
    
    void onOnceFriend(GameObject go)
    {
        UILabel oneCost = PanelTools.findChild<UILabel>(friendSummon, "oneCost");

        int nCost = int.Parse(oneCost.text);

        if (nCost > DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.friendPt)
        {
            return;
        }
        
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).getNewCard(0);
    }

    void onEightFriend(GameObject go)
    {
        UILabel eightCost = PanelTools.findChild<UILabel>(friendSummon, "eightCost");

        int nCost = int.Parse(eightCost.text);

        if (nCost > DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.friendPt)
        {
            return;
        }
        
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).getNewCard(1);
    }

    void onOnceRare(GameObject go)
    {
        UILabel oneCost = PanelTools.findChild<UILabel>(rareSummon, "oneCost");

        int nCost = int.Parse(oneCost.text);

        if (nCost > DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.diamond)
        {
            return;
        }
        
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).getNewCardDiamond();
    }

    void onTenRare(GameObject go)
    {
        UILabel tenCost = PanelTools.findChild<UILabel>(rareSummon, "tenCost");

        int nCost = int.Parse(tenCost.text);

        if (nCost > DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.diamond)
        {
            return;
        }
        
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).getNewCardDiamond(DiamondNewCard.TENS);
    }

    ulong movtTime = 0;
    
    void onLeft(GameObject go)
    {

        if (DataManager.getModule<DataServTime>(DATA_MODULE.Data_ServTime).getCurSvrtime() - movtTime < 1)
        {
            return;
        }

        movtTime = DataManager.getModule<DataServTime>(DATA_MODULE.Data_ServTime).getCurSvrtime();
        
        AlineOnChild aoc = grid.GetComponent<AlineOnChild>();
        aoc.CenterOn(rareSummon.transform);

        nState = 1;
        UpdateTag();
    }

    void onRight(GameObject go)
    {
        if (DataManager.getModule<DataServTime>(DATA_MODULE.Data_ServTime).getCurSvrtime() - movtTime < 1)
        {
            return;
        }

        movtTime = DataManager.getModule<DataServTime>(DATA_MODULE.Data_ServTime).getCurSvrtime();
        
        AlineOnChild aoc = grid.GetComponent<AlineOnChild>();
        aoc.CenterOn(friendSummon.transform);

        nState = 2;
        UpdateTag();
    }

    public void UpdateDiamond()
    {
        UILabel diamondLabel = PanelTools.findChild<UILabel>(rareSummon, "diamondLabel");
        //钻石数：
        diamondLabel.text = BaseLib.LanguageMgr.getString("66361201") +
            DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.diamond.ToString();
    }

    public void UpdateFriendship()
    {
        UILabel friendLabel = PanelTools.findChild<UILabel>(friendSummon, "friendLabel");
        //友情点：
        friendLabel.text = BaseLib.LanguageMgr.getString("66361202") +
            DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.friendPt.ToString();
    }

    public void UpdateExpDiamond()
    {
        UILabel diamondLabel = PanelTools.findChild<UILabel>(expSummonPage, "diamondLabel");
        //钻石数：
        diamondLabel.text = BaseLib.LanguageMgr.getString("66361201") +
            DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.diamond.ToString();
    }


    public void UpdateTag()
    {
//         CycleGrid cg = grid.GetComponent<CycleGrid>();
//         
//         switch(cg.curTransform.name)
//         {
//             case "arareSummon":
//                 nState = 1;
//                 tag1.SetActive(true);
//                 tag2.SetActive(false);
//                 tag3.SetActive(false);
//                 break;
//             case "bfriendSummon":
//                 nState = 2;
//                 tag1.SetActive(false);
//                 tag2.SetActive(true);
//                 tag3.SetActive(false);
//                 break;
//             case "cexpSummon":
//                 nState = 3;
//                 tag1.SetActive(false);
//                 tag2.SetActive(false);
//                 tag3.SetActive(true);
//                 break;
//         }

        switch (nState)
        {
            case 1:
                tag1.SetActive(true);
                tag2.SetActive(false);
                tag3.SetActive(false);
                break;
            case 2:
                tag1.SetActive(false);
                tag2.SetActive(true);
                tag3.SetActive(false);
                break;
        }
    }

    
}
