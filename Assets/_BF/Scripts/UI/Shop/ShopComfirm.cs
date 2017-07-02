using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class ShopComfirm : MonoBehaviour {

    UILabel titleLabel;
    UILabel contentLabel;

    GameObject celBtn;
    GameObject okBtn;

    UIEventListener.VoidDelegate ShopFun;

    // Use this for initialization
	void Start () {

        celBtn = PanelTools.findChild(gameObject, "celButton");

        if (celBtn != null)
        {
            UIEventListener.Get(celBtn).onClick = onReturn;
        }

        okBtn = PanelTools.findChild(gameObject, "okButton");

        if (okBtn != null)
        {
            UIEventListener.Get(okBtn).onClick = onOK;
        }
	}

    void onReturn(GameObject go)
    {
        gameObject.SetActive(false);
    }

    void onOK(GameObject go)
    {
        gameObject.SetActive(false);
        if (ShopFun != null) ShopFun(go);
    }

    public void ShowInfo(string strTitle, string strContent, UIEventListener.VoidDelegate func)
    {
        titleLabel = PanelTools.findChild<UILabel>(gameObject, "titleLabel");
        titleLabel.text = strTitle;

        contentLabel = PanelTools.findChild<UILabel>(gameObject, "contentLabel");
        contentLabel.text = strContent;

        ShopFun = func;
    }



}
