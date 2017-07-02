using UnityEngine;
using System.Collections;
using DataCenter;

public class SelConfig : MonoBehaviour {

    public UIToggle checkBox;
    public UIInput pathInput;
    public GameObject setObj;
    public UIButton confirm;
	// Use this for initialization
    public UIToggle offLineBox;
    void Start() 
    {
        if(confirm!=null)
            UIEventListener.Get(confirm.gameObject).onClick = onConfirm;
        if (checkBox!=null)
            checkBox.value = false;
        if (offLineBox != null)
        {
            UIEventListener.Get(offLineBox.gameObject).onClick = onOffline;
            offLineBox.value = false;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (setObj != null)
            setObj.SetActive(checkBox.value);	    
	}

    void onConfirm(GameObject obj)
    {
        if (checkBox.value)
        {
            ConfigMgr.initPath(true, pathInput.value);
        }
        else
        {
            ConfigMgr.initPath(true,null);
        }
    }

    void onOffline(GameObject obj)
    {
        NetworkMgr.me.offLine = offLineBox.value;
    }
}
