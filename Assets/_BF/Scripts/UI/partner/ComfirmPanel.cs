using UnityEngine;
using System.Collections;
using UI;

public class ComfirmPanel : MonoBehaviour {


    public UIEventListener.VoidDelegate onComfirm;
    public UIEventListener.VoidDelegate onCancel;

    void OnComfirm() { if (onComfirm != null) onComfirm(gameObject); }
    void OnCancel() { if (onCancel != null) onCancel(gameObject); }

    GameObject celButton;
    GameObject comButton;
    UILabel content;

    // Use this for initialization
	void Start () {

        celButton = PanelTools.findChild(gameObject, "celButton");
        comButton = PanelTools.findChild(gameObject, "comButton");
        content = PanelTools.findChild<UILabel>(gameObject, "contentLabel");

        if (celButton != null)
            UIEventListener.Get(celButton.gameObject).onClick = onReturn;

        if (comButton != null)
            UIEventListener.Get(comButton.gameObject).onClick = onDefine;
	}

    void onReturn(GameObject go)
    {
        gameObject.SetActive(false);

        OnCancel();
    }

    void onDefine(GameObject go)
    {
        gameObject.SetActive(false);

        OnComfirm();
    }

    public void SetComfirmFunc(UIEventListener.VoidDelegate func)
    {
        onComfirm = func;
    }

    public void SetComfirmText(string strText)
    {
        content = PanelTools.findChild<UILabel>(gameObject, "contentLabel");
        content.text = strText;
    }
}
