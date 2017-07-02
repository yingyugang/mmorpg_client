using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class FriendPartner : MonoBehaviour {

    public UIButton btnReturn;
    public GameObject btnMainPage;

    public GameObject grid;
    public GameObject Item;
	public GameObject panelDungeon;
	public GameObject panelBattlePlan;

    // Use this for initialization
	void Start () {

        if (btnReturn != null)
            UIEventListener.Get(btnReturn.gameObject).onClick = onReturn;
        if (btnMainPage != null)
            UIEventListener.Get(btnMainPage.gameObject).onClick = onMainPage;
		
        InitUI();
	}
	
	// Update is called once per frame
	void Update () {

	}


    void InitUI()
    {
    }

    void onReturn(GameObject go)
    {
        //transform.parent.gameObject.SetActive(false);
		//transform.gameObject.SetActive(false);
		//panelDungeon.SetActive(true);		
		UI.PanelStack.me.goBack();
    }

    void onMainPage(GameObject go)
    {

        transform.parent.gameObject.SetActive(false);
    }

	//选中帮忙的好友
    void onHeroItem(GameObject go)
    {
        //DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).SetFriendHero();
        int ite = int.Parse(go.name);
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).SetFriendHero(heroList[ite].hero);
		DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).setFriendUserID(heroList[ite].userid);

		//UI.PanelStack.me.clear();
		UI.PanelStack.me.goNext(panelBattlePlan,onReturn1);

        transform.gameObject.SetActive(false);
		//panelBattlePlan.SetActive(true);			

    }

	void onReturn1(System.Object param)
	{
		transform.gameObject.SetActive(true);
	}

    public List<FriendHeroItem> itemList = new List<FriendHeroItem>();

    public class FriendHeroItem
    {
        public GameObject root;
        public GameObject equip;
        public GameObject hero;
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
        public UISprite frameSprite;
        public UISprite framebg;
        public UISprite starSprite;
        public UISprite seriesSprite;

        public void Release()
        {
            if (root != null)
            {
                GameObject.Destroy(root);
            }
        }
    }

    public void ClearUI()
    {
        foreach (FriendHeroItem fhi in itemList)
        {
            fhi.Release();
        }

        itemList.Clear();

		for (int i = grid.transform.childCount -1; i >= 0; i--)
		{
			GameObject go = grid.transform.GetChild(i).gameObject;
			Destroy(go);
		}
    }

    public void ShowHero()
    {
        TextData();
        ClearUI();

        int nCount = heroList.Count;
	
        for (int i = 0; i < nCount; ++i)
        {
            HelpHeroInfo hhi = heroList[i];
            
            FriendHeroItem _fItem = new FriendHeroItem();
            _fItem.root = NGUITools.AddChild(grid, Item);
            _fItem.root.name = i.ToString();
            _fItem.root.SetActive(true);

            _fItem.root.transform.localPosition = new UnityEngine.Vector3(0, -140 * i, 0);

            /*_fItem.id = PanelTools.findChild<UILabel>(_fItem.root, "idLabel");
            _fItem.id.text = hhi.userid.ToString();*/

            _fItem.name = PanelTools.findChild<UILabel>(_fItem.root, "nameLabel");
            _fItem.name.text = hhi.hero.name.ToString();

            _fItem.level = PanelTools.findChild<UILabel>(_fItem.root, "levelLabel");
            _fItem.level.text = "Lv. " + hhi.userLevel.ToString();

            //装备
            _fItem.equip = PanelTools.findChild(_fItem.root, "equip");
            if (hhi.hero.equipId == 0)
            {
                _fItem.equip.SetActive(false);
            }
            else
            {
                _fItem.equip.SetActive(true);
            }

			//ItemInfo iteminfo = DataManager.getModule<DataItem>(DATA_MODULE.Data_Item).getItem((uint)hhi.hero.equipId);

			ItemInfo iteminfo = new ItemInfo();
			iteminfo.init(hhi.hero.equipId);

			if (iteminfo != null)
			{
				_fItem.equipName = PanelTools.findChild<UILabel>(_fItem.equip, "equipName");
				_fItem.equipName.text = iteminfo.name;

// 				_fItem.equipIcon = PanelTools.findChild<UISprite>(_fItem.equip, "icon");
// 				_fItem.equipIcon.spriteName = iteminfo.icon;
			}
			else
				_fItem.equip.SetActive(false);


            //英雄
            _fItem.hero = PanelTools.findChild(_fItem.root, "Hero");

            _fItem.heroId = PanelTools.findChild<UILabel>(_fItem.hero, "id");
            _fItem.heroId.text = hhi.hero.id.ToString();

            _fItem.heroLevel = PanelTools.findChild<UILabel>(_fItem.hero, "levelLabel");
            _fItem.heroLevel.text = "Lv. " + hhi.hero.level.ToString();

            _fItem.heroIcon = PanelTools.findChild<UISprite>(_fItem.hero, "icon");

			if (hhi.hero.portarait != null)
            	_fItem.heroIcon.spriteName = hhi.hero.portarait.ToString();


            _fItem.seriesSprite = PanelTools.findChild<UISprite>(_fItem.hero, "seriesSprite");
            _fItem.seriesSprite.spriteName = "SERIES" + hhi.hero.series.ToString();
            
            _fItem.frameSprite = PanelTools.findChild<UISprite>(_fItem.hero, "frameSprite");
            _fItem.frameSprite.spriteName = "frame" + hhi.hero.series.ToString();

            _fItem.framebg = PanelTools.findChild<UISprite>(_fItem.hero, "framebg");
            _fItem.framebg.spriteName = "framebg" + hhi.hero.series.ToString();

            _fItem.starSprite = PanelTools.findChild<UISprite>(_fItem.hero, "starSprite");
            _fItem.starSprite.spriteName = "star" + hhi.hero.star.ToString();

            UIEventListener.Get(_fItem.root).onClick = onHeroItem;
            _fItem.root.SetActive(true);
        }
    }


    //测试数据
    public List<HelpHeroInfo> heroList = new List<HelpHeroInfo>();

    public class HelpHeroInfo
    {
        public int userid;
        public string userName;
        public int userLevel;
        public int isFriend;

        public HeroInfo hero = new HeroInfo();
    }

    private void TextData()
    {
        heroList.Clear();
        
		BattleFirend[] battleFriend = DataManager.getModule<DataBattle>(DATA_MODULE.Data_Battle).friendList;

		for (int i = 0; i < battleFriend.Length; i ++)
		{
			HelpHeroInfo hero = new HelpHeroInfo();
			hero.userid = (int)battleFriend[i].uid;
			hero.userName = battleFriend[i].uname;
			hero.userLevel = (int)battleFriend[i].ulevel;
			hero.hero.id = battleFriend[i].friendHero.id;
			hero.hero.type = battleFriend[i].friendHero.type;
			hero.hero.btGrowup = battleFriend[i].friendHero.btGrowup;
			hero.hero.level = battleFriend[i].friendHero.level;
			hero.hero.skillLevel = battleFriend[i].friendHero.skillLevel;
			hero.hero.equipId = battleFriend[i].friendHero.equipId;
			hero.hero.InitDict(hero.hero.type);
			heroList.Add(hero);
		}
	

		if (battleFriend.Length == 0)
		{
			for (int i = 0; i < 10; ++i)
			{
				HelpHeroInfo hero = new HelpHeroInfo();
				
				hero.userid = 1000 + i;
				hero.userName = "好友" + i.ToString();
				hero.userLevel = i;
				
				hero.hero.id = 100 + i;
				hero.hero.type = 10007 + i % 6;
				hero.hero.btGrowup = 1 + (i % 4);
				hero.hero.level = 10 + i;
				hero.hero.skillLevel = i;
				//hero.hero.equipId = battleFriend[i].friendHero.equipId;
				hero.hero.equipId = 0;
				hero.hero.InitDict(hero.hero.type);
				
				heroList.Add(hero);
			}
		}

		
		
		/*for (int i = 0; i < 10; ++i)
		{
			HelpHeroInfo hero = new HelpHeroInfo();
			
			hero.userid = 1000 + i;
            hero.userName = "好友" + i.ToString();
            hero.userLevel = i;

            hero.hero.id = 100 + i;
            hero.hero.type = 10007 + i % 6;
            hero.hero.btGrowup = 1 + (i % 4);
            hero.hero.level = 10 + i;
            hero.hero.skillLevel = i;
            hero.hero.InitDict(hero.hero.type);

            heroList.Add(hero);
        }*/
    }

	void OnEnable()
	{
	}
	
}
