using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class MultiSummon : MonoBehaviour
{

    public UIButton btnReturn;
    public GameObject btnClose;
    public GameObject detail;

    public GameObject grid;
    public GameObject Item;
    // Use this for initialization
	void Start () {

        if (btnReturn != null)
            UIEventListener.Get(btnReturn.gameObject).onClick = onReturn;

        if (btnClose != null)
            UIEventListener.Get(btnClose).onClick = onClose;

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
        transform.parent.gameObject.SetActive(false);
    }


    void onClose(GameObject go)
    {
        transform.parent.gameObject.SetActive(false);
    }

    void onHeroItem(GameObject go)
    {
        UILabel idLabel = PanelTools.findChild<UILabel>(go, "idLabel");
        int id = int.Parse(idLabel.text);

        gameObject.SetActive(false);

        PartnerDetail pd = detail.GetComponent<PartnerDetail>();
        pd.ShowHeroDetail(id, gameObject);
        detail.SetActive(true);
    }


    public List<HeroItem> itemList = new List<HeroItem>();

    public class HeroItem
    {
        public GameObject root;
        public GameObject hero;
        public UILabel id;
        public UILabel name;
        public UILabel cost;
        public UILabel type;

        public UILabel heroId;
        public UISprite star;
        public UISprite heroIcon;
        public UISprite seriesSprite;
        public UISprite frameSprite;
        public UISprite framebg;

        public UILabel hp;
        public UILabel hpValue;
        public UILabel atk;
        public UILabel atkValue;
        public UILabel def;
        public UILabel defValue;
        public UILabel recover;
        public UILabel recoverValue;

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
        foreach (HeroItem fhi in itemList)
        {
            fhi.Release();
        }

        itemList.Clear();
    }

    public void ShowHeros()
    {
        ClearUI();
        int nCount = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).changeHeroList.Count;

        for (int i = 0; i < nCount; ++i)
        {
            HeroInfo hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).changeHeroList[i];

            HeroItem item = new HeroItem();
            item.root = NGUITools.AddChild(grid, Item);
            item.root.name = i.ToString();
            item.root.SetActive(true);

            //item.root.transform.localPosition = new UnityEngine.Vector3(0, -120 * i, 0);

            item.id = PanelTools.findChild<UILabel>(item.root, "idLabel");
            item.id.text = hi.id.ToString();

            item.name = PanelTools.findChild<UILabel>(item.root, "nameLabel");
            item.name.text = hi.name;

            item.type = PanelTools.findChild<UILabel>(item.root, "typeLabel");
            item.type.text = "Unit No." + hi.library.ToString();

            item.cost = PanelTools.findChild<UILabel>(item.root, "costLabel");
            item.cost.text = hi.leader.ToString();


            item.hpValue = PanelTools.findChild<UILabel>(item.root, "hpLabel");
            item.hpValue.text = hi.hp.ToString();

            item.atkValue = PanelTools.findChild<UILabel>(item.root, "atkLabel");
            item.atkValue.text = hi.atk.ToString();

            item.defValue = PanelTools.findChild<UILabel>(item.root, "defLabel");
            item.defValue.text = hi.def.ToString();

            item.recoverValue = PanelTools.findChild<UILabel>(item.root, "reLabel");
            item.recoverValue.text = hi.recover.ToString();


            //英雄
            item.hero = PanelTools.findChild(item.root, "Hero");

            item.star = PanelTools.findChild<UISprite>(item.hero, "starSprite");
            item.star.spriteName = "star"+ hi.star.ToString();

            item.heroId = PanelTools.findChild<UILabel>(item.hero, "idLabel");
            item.heroId.text = hi.id.ToString();

            item.heroIcon = PanelTools.findChild<UISprite>(item.hero, "heroIcon");
            item.heroIcon.spriteName = hi.portarait.ToString();

            item.seriesSprite = PanelTools.findChild<UISprite>(item.hero, "seriesSprite");
            item.seriesSprite.spriteName = "SERIES" + hi.series.ToString();

            item.frameSprite = PanelTools.findChild<UISprite>(item.hero, "frameSprite");
            item.frameSprite.spriteName = "frame" + hi.series.ToString();

            item.framebg = PanelTools.findChild<UISprite>(item.hero, "framebg");
            item.framebg.spriteName = "framebg" + hi.series.ToString();


            UIEventListener.Get(item.root).onClick = onHeroItem;
            item.root.SetActive(true);


            itemList.Add(item);
        }

        UIGrid uiGrid = grid.GetComponent<UIGrid>();
        uiGrid.repositionNow = true;
    }
   
}
