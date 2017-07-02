using UnityEngine;
using System.Collections;
using System;
using DataCenter;

public class LoginUI : MonoBehaviour 
{
    public string ServIP;
    public string Iggid;

    UIInput _servIP;
    UIInput _iggID;

    string _servKey = "servip";
    string _iggidKey = "iggid";

	void Start () 
    {
        BaseLib.EventSystem.register((int)DataCenter.EVENT_GLOBAL.sys_loginSucc,loginSucc);
        BaseLib.EventSystem.register((int)DataCenter.EVENT_GLOBAL.sys_loginFailed,loginFaild);

        _iggID = UI.PanelTools.findChild<UIInput>(transform,"userinfo/iggid/Input");
        _servIP = UI.PanelTools.findChild<UIInput>(transform, "userinfo/server/Input");
        UI.PanelTools.setBtnFunc(transform,"btnStart",onStart);
        readcfg();
        if (this._iggID != null && !Iggid.Equals(string.Empty))
            this._iggID.value = Iggid;
        if (this._servIP != null && !ServIP.Equals(string.Empty))
            this._servIP.value = ServIP;        

	}

    void onStart(GameObject btn)
    {
        WaitController.me.showWaitMsg("Logining...");
//      BaseLib.EventSystem.sendEvent((int)DataCenter.EVENT_GLOBAL.sys_chgScene, LoadSceneMgr.SCENE_MAIN);
        this.Iggid = this._iggID.value;
        this.ServIP = this._servIP.value;
        savecfg();
        login(this.ServIP, this.Iggid);
    }

    void readcfg()
    {
        this.ServIP = PlayerPrefs.GetString(_servKey);
        this.Iggid = PlayerPrefs.GetString(_iggidKey);
    }

    void savecfg()
    {
        PlayerPrefs.SetString(_servKey, ServIP);
        PlayerPrefs.SetString(_iggidKey, Iggid);
    }

	void Update () 
    {
	    
	}

    void login(string strIP,string iggid)
    {
        DataCenter.Login login = new DataCenter.Login();
        login.loginIp = strIP;
        login.loginPort = 5999;
        login.strIggid = iggid;
        DataCenter.DataOffline.me.iggid = Convert.ToUInt64(iggid);
        DataCenter.DataOffline.me.init();
        login.login();
    }

    void loginFaild(int nEvent, object param)
    {
        WaitController.me.hide();
        Debug.Log("loginFaild");        
    }

    void loginSucc(int nEvent, object param)
    {
        WaitController.me.hide();
        Debug.Log("login SUCC");
        DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.id = Convert.ToUInt64(Iggid);
        BaseLib.EventSystem.sendEvent((int)DataCenter.EVENT_GLOBAL.sys_chgScene, LoadSceneMgr.SCENE_MAIN);
    }
}
