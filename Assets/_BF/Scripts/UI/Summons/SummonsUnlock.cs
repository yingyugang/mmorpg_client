using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class SummonsUnlock : MonoBehaviour {


    GameObject unlockBtn;
    GameObject returnBtn;
    GameObject closeBtn;

    bool isCanUnlock = false;

    // Use this for initialization
	void Start () {

        returnBtn = PanelTools.findChild(gameObject, "Title/btnBack");

        if (returnBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onReturn;
        }

        closeBtn = PanelTools.findChild(gameObject, "upLv/btnClose");

        if (closeBtn != null)
        {
            UIEventListener.Get(closeBtn).onClick = onClose;
        }

        unlockBtn = PanelTools.findChild(gameObject, "upLv/unlockBtn");

        if (unlockBtn != null)
        {
            UIEventListener.Get(unlockBtn).onClick = onUnlock;
        }

        
	}

    void OnDisable()
    {
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, true, (int)EVENT_GROUP.mainUI);
    }

    void OnEnable()
    {
        BaseLib.EventSystem.sendEvent((int)EVENT_MAINUI.showFooter, false, (int)EVENT_GROUP.mainUI);
        isCanUnlock = false;
        ShowSummon();
    }

    void onReturn(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowSummonsBrowse();
    }

    void onClose(GameObject go)
    {
        transform.parent.gameObject.SetActive(false);
    }

    void onUnlock(GameObject go)
    {
        
        if (isCanUnlock)
        {
            DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).SendUnlockMsg();
        }
    }

    public void ShowSummon()
    {
        int SummonId = (int)DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).SelectSummonId;
        ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_SUMMON);
        ConfigRow row = table.getRow(DICT_SUMMON.SUMMON_TYPEID, SummonId);
        
        
//         UILabel NOLabel = PanelTools.findChild<UILabel>(gameObject, "info/NOLabel");
//         NOLabel.text = "NO." + row.getStringValue(DICT_SUMMON.SUMMON_TYPEID);

//         UILabel soulLabel = PanelTools.findChild<UILabel>(gameObject, "upLv/soul/soulLabel");
//         soulLabel.text = DataManager.getModule<DataUser>(DATA_MODULE.Data_User).mainUser.soul.ToString()
//             + "/" + row.getStringValue(DICT_SUMMON.SOUL);

        int nSeries = row.getIntValue(DICT_SUMMON.SERIES);
        int chipType = SummonInfo.GetChipType(nSeries);
        int chipCount = DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).getItemCountByType((uint)chipType);

        int needCount = SummonInfo.GetNeedChipCount(SummonId);

        UILabel chipLabel = PanelTools.findChild<UILabel>(gameObject, "upLv/chip/chipLabel");
        chipLabel.text = chipCount.ToString();

        UILabel needChipLabel = PanelTools.findChild<UILabel>(gameObject, "upLv/chip/needChipLabel");
        needChipLabel.text = "/" + needCount.ToString();
//         UISprite chipIcon = PanelTools.findChild<UISprite>(gameObject, "upLv/chip/chipIcon");
//         ItemInfo item = new ItemInfo();
//         item.init(chipType);
//         chipIcon.spriteName = item.icon;


        int UniversalChip = DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).getItemCountByType(44007);
        UILabel allChipLabel = PanelTools.findChild<UILabel>(gameObject, "upLv/allChip/chipLabel");
        allChipLabel.text = UniversalChip.ToString();

        UILabel allNeedChipLabel = PanelTools.findChild<UILabel>(gameObject, "upLv/allChip/needChipLabel");
        allNeedChipLabel.text = "/" + needCount.ToString();
//         UISprite allChipIcon = PanelTools.findChild<UISprite>(gameObject, "upLv/allChip/chipIcon");
//         ItemInfo allChip = new ItemInfo();
//         allChip.init(44007);
//         allChipIcon.spriteName = allChip.icon;

//         UILabel unlockLabel = PanelTools.findChild<UILabel>(gameObject, "condition/unlockLabel");
//         unlockLabel.text = "解锁战役" + row.getStringValue(DICT_SUMMON.COPY_ID);


        if (chipCount + UniversalChip >= needCount)
        {
            isCanUnlock = true;
        }

        UILabel nameLabel = PanelTools.findChild<UILabel>(gameObject, "upLv/nameLabel");
        nameLabel.text = BaseLib.LanguageMgr.getString(row.getStringValue(DICT_SUMMON.NAME_ID));

        UISprite series = PanelTools.findChild<UISprite>(gameObject, "upLv/series");
        series.spriteName = "series" + row.getStringValue(DICT_SUMMON.SERIES);

        string strTemp = row.getStringValue(DICT_SUMMON.SUMMON_TYPEID);
        strTemp = strTemp.Substring(3, 1);
        int NO = int.Parse(strTemp) + 1;
        UILabel noLabel = PanelTools.findChild<UILabel>(gameObject, "upLv/noLabel");
        noLabel.text = "NO." + NO.ToString();
    }
}
