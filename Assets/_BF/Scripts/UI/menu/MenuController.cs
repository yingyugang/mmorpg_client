using UnityEngine;
using System.Collections;
using DataCenter;

public class MenuController : MonoBehaviour {

	private GameObject bg;
	private GameObject panelLogin;
	private GameObject panelPlayerInfo;
	private GameObject panelInstruction;
	private GameObject panelGiftCode;
	private GameObject panelSetting;
	private bool init = false;

	void Start () {
		if(init)
			return ;
		UI.PanelTools.setBtnFunc(transform, "Bg/menuTitle/back", onBtnBackClick);
		UI.PanelTools.setBtnFunc(transform, "Bg/loginRewards", onLoginRewardsClick);
		UI.PanelTools.setBtnFunc(transform, "Bg/playerInfo", onPlayerInfoClick);
		UI.PanelTools.setBtnFunc(transform, "Bg/instruction", onInstructionClick);
		UI.PanelTools.setBtnFunc(transform, "Bg/giftCode", onGiftCodeClick);
		UI.PanelTools.setBtnFunc(transform, "Bg/setting", onSettingClick);

		bg = UI.PanelStack.me.FindPanel ("PanelMenu/Bg");
		panelLogin = UI.PanelStack.me.FindPanel ("PanelMenu/PanelLoginRewards");
		panelPlayerInfo = UI.PanelStack.me.FindPanel ("PanelMenu/PanelplayerInfo");
		panelInstruction = UI.PanelStack.me.FindPanel ("PanelMenu/PanelInstruction");
		panelGiftCode = UI.PanelStack.me.FindPanel ("PanelMenu/PanelGiftsCode");
		panelSetting = UI.PanelStack.me.FindPanel ("PanelMenu/PanelSetting");

        BaseLib.EventSystem.register((int)EVENT_MAINUI.showMainUI, onReturnMain, (int)EVENT_GROUP.mainUI);

		init = true;
	}

    void onReturnMain(int eventId, System.Object param)
    {
        gameObject.SetActive(false);
    }

	void OnEnable()
	{
		Start ();
		if(bg != null)
			bg.SetActive (true);
		if(panelLogin != null)
			panelLogin.SetActive(false);
		if(panelPlayerInfo != null)
			panelPlayerInfo.SetActive(false);
		if(panelInstruction != null)
			panelInstruction.SetActive(false);
		if(panelGiftCode != null)
			panelGiftCode.SetActive(false);
		if(panelSetting != null)
			panelSetting.SetActive(false);

		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, false, (int)EVENT_GROUP.mainUI);
	}

	void onBtnBackClick(GameObject go)
	{
		AudioManager.SingleTon().PlayMusic(AudioManager.SingleTon().MusicMainClip);
		this.transform.gameObject.SetActive (false);
		BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showBody, true, (int)EVENT_GROUP.mainUI);	
	}

	void onLoginRewardsClick(GameObject go)
	{
		AudioManager.SingleTon().PlayMusic(AudioManager.SingleTon().MusicMainClip);
		commonOperation(panelLogin);
	}

	void onPlayerInfoClick(GameObject go)
	{
		AudioManager.SingleTon().PlayMusic(AudioManager.SingleTon().MusicMainClip);
		commonOperation(panelPlayerInfo);
	}

	void onInstructionClick(GameObject go)
	{
		AudioManager.SingleTon().PlayMusic(AudioManager.SingleTon().MusicMainClip);
		commonOperation(panelInstruction);
	}

	void onGiftCodeClick(GameObject go)
	{
		AudioManager.SingleTon().PlayMusic(AudioManager.SingleTon().MusicMainClip);
		commonOperation(panelGiftCode);
	}

	void onSettingClick(GameObject go)
	{
		AudioManager.SingleTon().PlayMusic(AudioManager.SingleTon().MusicMainClip);
		commonOperation(panelSetting);
	}

	void commonOperation(GameObject obj)
	{
		bg.SetActive (false);
		obj.SetActive(true);
	}
}
