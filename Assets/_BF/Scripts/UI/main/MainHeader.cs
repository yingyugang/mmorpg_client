using UnityEngine;
using System.Collections;
using System;
using UI;
using DataCenter;
using BaseLib;

#pragma warning disable 0618
#pragma warning disable 0219
public class MainHeader : MonoBehaviour {

    UILabel _name;
    UILabel _honor;
    UILabel _level;
    UILabel _exp;
    UILabel _power;
    UILabel _nextPowerTime;
    UILabel _diamond;
    UILabel _coin;
  //  UILabel _soul;
    UIProgressBar _expBar;
    UIProgressBar _powerBar;

//    UISprite _pt1;
//    UISprite _pt2;
//    UISprite _pt3;
	float t;

	GameObject showDetail;
	UILabel _lv;
	UILabel _curExp;
	UILabel _toUpExp;
	UILabel _friendPoint;
	bool isPress;
	void Start () 
    {
        _name = UI.PanelTools.findChild<UILabel>(gameObject,"name");
        _honor = UI.PanelTools.findChild<UILabel>(gameObject, "honor");
        _level = UI.PanelTools.findChild<UILabel>(gameObject, "level");
        _exp = UI.PanelTools.findChild<UILabel>(gameObject, "exp/value");
        _power = UI.PanelTools.findChild<UILabel>(gameObject, "power/value");
        _nextPowerTime = UI.PanelTools.findChild<UILabel>(gameObject, "showDetail/powerTime");
        _diamond = UI.PanelTools.findChild<UILabel>(gameObject, "diamond");
        _coin = UI.PanelTools.findChild<UILabel>(gameObject, "coin");
    //    _soul = UI.PanelTools.findChild<UILabel>(gameObject, "soul");
        _expBar = UI.PanelTools.findChild<UIProgressBar>(gameObject,"exp");
        _powerBar = UI.PanelTools.findChild<UIProgressBar>(gameObject, "power");
//        _pt1 = UI.PanelTools.findChild<UISprite>(gameObject, "pt1");
//        _pt2 = UI.PanelTools.findChild<UISprite>(gameObject, "pt2");
//        _pt3 = UI.PanelTools.findChild<UISprite>(gameObject, "pt3");
		t = Time.time;
		showDetail = GameObject.Find ("showDetail");
		_lv = UI.PanelTools.findChild<UILabel>(gameObject, "showDetail/lv");
		_curExp = UI.PanelTools.findChild<UILabel>(gameObject, "showDetail/curExp");
		_toUpExp = UI.PanelTools.findChild<UILabel>(gameObject, "showDetail/toUp");
		_friendPoint = UI.PanelTools.findChild<UILabel>(gameObject, "showDetail/FriendPoint");
		if(showDetail)
			showDetail.SetActive(false);
		UIEventListener.Get(GameObject.Find("back")).onPress = onPression;
		isPress = false;

        EventSystem.register((int)EVENT_MAINUI.userUpdate, onUserUpdate,(int)EVENT_GROUP.mainUI);

		DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).getAllTaskInfo();
		EventSystem.register((int)EVENT_MAINUI.battleTask,onTaskInfo,(int)EVENT_GROUP.mainUI);

		DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).getFriendHelp();
		//EventSystem.register((int)EVENT_MAINUI.battleFriend, onHelperInfo, (int)EVENT_GROUP.mainUI);
        showUser();
    }

    void Update()
    {
        if(isPress)
		{
			showDetails();
		}
    }

	void showDetails()
	{
		UserInfo info = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser;
		_lv.text = "LV" + info.level.ToString ();
		_curExp.text = "当前经验：" + info.curexp.ToString ();
		_toUpExp.text = "距离升级：" + (info.maxexp - info.curexp).ToString ();
		_friendPoint.text = "友情点：" + info.friendPt.ToString ();
		showPower();
	}

	void onPression(GameObject go,bool state)
	{
		if(state)
		{
			isPress = true;
			showDetail.SetActive(true);
		}
		else
		{
			isPress = false;
			showDetail.SetActive(false);
		}
	}

    void showPower()
    {
        UserInfo info = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser;
        if (info.curpower >= info.maxpower)
        {
            if (_nextPowerTime != null)
                _nextPowerTime.text = "体力已全恢复";
            return;
        }
        //设置体力回复倒计时
        if (Time.time - t >= 1)
        {
            info.powerTime--;
            if (info.powerTime == 0)
            {
                info.powerTime = 600;
                    info.curpower++;
            }
            if (_nextPowerTime != null)
                _nextPowerTime.text = "体力回复：" + getTimeFormat(info.powerTime);
            t = Time.time;
        }
    }

    void onUserUpdate(int nEvent,System.Object param)
    {
        showUser();
    }

    void showUser()
    {
        UserInfo info = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser;
        if (_name != null)
            _name.text = info.name;
        if (_honor != null)
            _honor.text = info.arenaHonor;
        if (_level != null)
            _level.text = info.level.ToString();
        if (_diamond != null)
            _diamond.text = info.diamond.ToString();
        if (_coin != null)
            _coin.text = info.goldCoin.ToString();
      //  if (_soul != null)
     //       _soul.text = info.soul.ToString();
        if (_exp != null)
            _exp.text = info.curexp.ToString() + "/" + info.maxexp.ToString();
        if (_power != null)
            _power.text = info.curpower.ToString() + "/" + info.maxpower.ToString();
        if (_expBar != null)
			_expBar.value = (float)(info.maxexp -info.curexp) / (float)info.maxexp;
        if (_powerBar != null)
			_powerBar.value = (float)(info.maxpower - info.curpower) / (float)info.maxpower;

//        if (_pt1 != null && _pt2 != null && _pt3 != null)
//        {
//            _pt1.active = (info.arenaPoint >= 1);
//            _pt2.active = (info.arenaPoint >= 2);
//            _pt3.active = (info.arenaPoint >= 3);
//        }
    }

    static public string getTimeFormat(uint time)
    {
        int nDate = (int)(time / 86400);
        int temp = (int)(time % 86400);
        int nHour = (int)(temp / 3600);
        temp = (int)(time % 3600);
        int nMinute = (int)(temp / 60);
        int nHSec = (int)(temp % 60);
        
        return string.Format("{0:D2}分{1:D2}秒",nMinute, nHSec);
        //return string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", nDate, nHour, nMinute, nHSec);
    }

	void onTaskInfo(int nEvent, System.Object param)
	{
		DataManager.getModule<DataTask>(DATA_MODULE.Data_Task).getTaskInfo();
	}

	/*void onHelperInfo(int nEvent, System.Object param)
	{
		BattleFirend[] battleFriend = DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).friendList;
	}*/
}
