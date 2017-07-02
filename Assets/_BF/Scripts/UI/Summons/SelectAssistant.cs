using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class SelectAssistant : MonoBehaviour
{

    GameObject returnBtn;
    GameObject okBtn;

    GameObject friendList;
    GameObject item;
    GameObject grid;


    int nSelectCount = 0;

    // Use this for initialization
	void Awake () {

        returnBtn = PanelTools.findChild(gameObject, "Title/btnBack");
        if (returnBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onReturn;
        }

        okBtn = PanelTools.findChild(gameObject, "Title/btnOK");
        if (okBtn != null)
        {
            UIEventListener.Get(okBtn).onClick = onComfirm;
        } 

        friendList = PanelTools.findChild(gameObject, "FriendList");
        grid = PanelTools.findChild(friendList, "friendGrid");
        item = PanelTools.findChild(friendList, "Item");
	}

    void OnEnable()
    {
        ShowAssistant();
    }

    void onReturn(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowOrgTeam();
    }

    void onComfirm(GameObject go)
    {
        int nCount = selectUserIdList.Count;

        for (int i = 0; i < nCount; ++i)
        {
            DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).AddfriendHero(i + 1, selectUserIdList[i]);
        }

        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowOrgTeam();
    }

    void onItem(GameObject go)
    {
        UILabel selLabel = PanelTools.findChild<UILabel>(go, "selLabel");
        UILabel idLabel = PanelTools.findChild<UILabel>(go, "idLabel");
        int id = int.Parse(idLabel.text);

//         if (selLabel.gameObject.activeSelf)
//         {
//             if (nSelectCount < 0)
//             {
//                 return;
//             }
//             
//             --nSelectCount;
//             selLabel.gameObject.SetActive(false);
// 
//             selectHeroIdList.Remove(id);
//         }
//         else
//         {
            if (nSelectCount > 2)
            {
                return;
            }

            ++nSelectCount;

            selLabel.text = nSelectCount.ToString();
            selLabel.gameObject.SetActive(true);
            selectUserIdList.Add(id);
//        }
        
    }


    public void ClearUI()
    {
        foreach (AssistantItem r in itemList)
        {
            r.Release();
        }

        itemList.Clear();

        nSelectCount = 0;
        selectUserIdList.Clear();
    }


    public class AssistantItem
    {
        public GameObject root;
        public GameObject equip;
        public GameObject hero;
        public UILabel selected;
        public UILabel id;
        public UILabel name;
        public UILabel level;
        public UILabel friendship;
        public UILabel equipName;

        public UILabel heroId;
        public UILabel heroLevel;
        public UILabel isFriend;
        public UISprite heroIcon;
        public UISprite equipIcon;

        public void Release()
        {
            if (root != null)
            {
                GameObject.Destroy(root);
            }
        }
    }

    List<AssistantItem> itemList = new List<AssistantItem>();
    List<int> selectUserIdList = new List<int>();

    public void ShowAssistant()
    {
        ClearUI();

        int nCount = DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).heroList.Count;

        for (int i = 1; i < nCount; ++i)
        {
            DataSummons.HelpHeroInfo hhi = DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).heroList[i];

            AssistantItem assistant = new AssistantItem();
            assistant.root = NGUITools.AddChild(grid, item);
            assistant.root.SetActive(true);

            assistant.id = PanelTools.findChild<UILabel>(assistant.root, "idLabel");
            assistant.id.text = hhi.userid.ToString();

            assistant.selected = PanelTools.findChild<UILabel>(assistant.root, "selLabel");
            assistant.selected.gameObject.SetActive(false);


            //信息
            assistant.name = PanelTools.findChild<UILabel>(assistant.root, "nameLabel");
            assistant.name.text = hhi.userName;

            assistant.level = PanelTools.findChild<UILabel>(assistant.root, "levelLabel");
            assistant.level.text = "LV. " + hhi.userLevel.ToString();

            //装备
            assistant.equip = PanelTools.findChild(assistant.root, "equip");
            ItemInfo iteminfo = new ItemInfo();
            iteminfo.init(hhi.hero.equipId);

            if (iteminfo != null)
            {
                assistant.equipName = PanelTools.findChild<UILabel>(assistant.equip, "equipName");
                assistant.equipName.text = iteminfo.name;

                assistant.equipIcon = PanelTools.findChild<UISprite>(assistant.equip, "icon");
                assistant.equipIcon.spriteName = iteminfo.icon;
            }
            else
            {
                assistant.equip.SetActive(false);
            }

            //英雄
            assistant.hero = PanelTools.findChild(assistant.root, "Hero");

            assistant.heroId = PanelTools.findChild<UILabel>(assistant.hero, "idLabel");
            assistant.heroId.text = hhi.hero.id.ToString();

            assistant.heroLevel = PanelTools.findChild<UILabel>(assistant.hero, "levelLabel");
            assistant.heroLevel.text = hhi.hero.level.ToString();

            assistant.heroIcon = PanelTools.findChild<UISprite>(assistant.hero, "heroIcon");

            if (hhi.hero.portarait != null)
                assistant.heroIcon.spriteName = hhi.hero.portarait.ToString();


            UIEventListener.Get(assistant.root).onClick = onItem;

            itemList.Add(assistant);
        }

        UIGrid uiGrid = grid.GetComponent<UIGrid>();
        uiGrid.repositionNow = true;
    }

}
