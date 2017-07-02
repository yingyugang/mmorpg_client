using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class ShopDiamond : MonoBehaviour {

    GameObject ReturnBtn;

    GameObject item1;
    GameObject item2;
    GameObject item3;
    
    // Use this for initialization
	void Start () {

        ReturnBtn = PanelTools.findChild(gameObject, "Title/btnBack");
        if (ReturnBtn != null)
        {
            UIEventListener.Get(ReturnBtn).onClick = onReturn;
        }

        item1 = PanelTools.findChild(gameObject, "Scroll View/Grid/Item1");
        if (item1 != null)
        {
            UIEventListener.Get(item1).onClick = onItem1;
        }

        item2 = PanelTools.findChild(gameObject, "Scroll View/Grid/Item2");
        if (item2 != null)
        {
            UIEventListener.Get(item2).onClick = onItem2;
        }

        item3 = PanelTools.findChild(gameObject, "Scroll View/Grid/Item3");
        if (item3 != null)
        {
            UIEventListener.Get(item3).onClick = onItem3;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void onReturn(GameObject go)
    {
        DataManager.getModule<DataShop>(DATA_MODULE.Data_Shop).ShowShopMenu();
    }

    void onItem1(GameObject go)
    {
        
    }

    void onItem2(GameObject go)
    {

    }

    void onItem3(GameObject go)
    {

    }
}
