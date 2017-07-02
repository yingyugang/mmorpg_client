using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class PartnerEquip : MonoBehaviour
{

    public GameObject btnReturn;
    public GameObject btnClose;

    public GameObject menu;
    public GameObject grid;
    public GameObject Item;
    public GameObject equipHero;
    public GameObject changeHero;

    // Use this for initialization
	void Start ()
    {
        if (btnReturn != null)
            UIEventListener.Get(btnReturn).onClick = onReturn;
        if (btnClose != null)
            UIEventListener.Get(btnClose).onClick = onClose;
    }
	
	// Update is called once per frame
	void Update () 
    {	
	}

    void onReturn(GameObject go)
    {
        gameObject.SetActive(false);
        menu.SetActive(true);
    }

    void onClose(GameObject go)
    {
        gameObject.SetActive(false);
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    void onEquip(GameObject go)
    {
        UILabel idLabel = PanelTools.findChild<UILabel>(go.transform.parent, "idLabel");
        int id = int.Parse(idLabel.text);
        
        gameObject.SetActive(false);
        equipHero.SetActive(true);

        SelectHero sh = equipHero.GetComponent<SelectHero>();
        sh.idEquip = id;
        sh.ShowHeroBrowse();

    }

    void onHero(GameObject go)
    {
        UILabel idLabel = PanelTools.findChild<UILabel>(go.transform.parent, "idLabel");
        int id = int.Parse(idLabel.text);

        UILabel idHeroLabel = PanelTools.findChild<UILabel>(go, "id");
        int idHero = int.Parse(idHeroLabel.text);

        gameObject.SetActive(false);
        changeHero.SetActive(true);

        ChangeHero ch = changeHero.GetComponent<ChangeHero>();
        ch.idEquip = id;
        ch.idHero = idHero;
        ch.ShowHeroBrowse();
    }

    public List<EquipItem> equipItemList = new List<EquipItem>();
    
    public class EquipItem
    {
        public GameObject root;
        public GameObject hero;
        public UILabel id;
        public UILabel name;
        public UILabel effect;
        public UISprite icon;
        public UIButton equipBtn;

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
        foreach (EquipItem ei in equipItemList)
        {
            ei.Release();
        }

        equipItemList.Clear();
    }

    public void ShowEquip()
    {
        ClearUI();

        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).equipDate();
        int nCount = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).equipList.Count;

        for (int i = 0; i < nCount; ++i)
        {
            EquipInfo ei = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).equipList[i];

            EquipItem _eItem = new EquipItem();
            _eItem.root = NGUITools.AddChild(grid, Item);
            _eItem.root.name = i.ToString();
            _eItem.root.SetActive(true);

            _eItem.root.transform.localPosition = new UnityEngine.Vector3(0, -140 * i, 0);

            _eItem.id = PanelTools.findChild<UILabel>(_eItem.root, "idLabel");
            _eItem.id.text = ei.id.ToString();
            
            _eItem.name = PanelTools.findChild<UILabel>(_eItem.root, "Equip/nameLabel");
            _eItem.name.text = ei.name;

            _eItem.effect = PanelTools.findChild<UILabel>(_eItem.root, "Equip/effectLabel");
            _eItem.effect.text = ei.effect;

            //装备图标
            _eItem.icon = PanelTools.findChild<UISprite>(_eItem.root, "Equip/icon");


            _eItem.equipBtn = PanelTools.findChild<UIButton>(_eItem.root, "Button");
            UIEventListener.Get(_eItem.equipBtn.gameObject).onClick = onEquip;
            
            _eItem.hero = PanelTools.findChild(_eItem.root, "Hero");
            UIEventListener.Get(_eItem.hero).onClick = onHero;

            if (ei.heroId == 0)
            {
                _eItem.hero.SetActive(false);
                _eItem.equipBtn.gameObject.SetActive(true);
            }
            else
            {
                _eItem.hero.SetActive(true);
                _eItem.equipBtn.gameObject.SetActive(false);
                ShowEquipHeroById(ei.heroId, _eItem.hero);
            }

            equipItemList.Add(_eItem);
        }
    }

    protected void ShowEquipHeroById(int id, GameObject go)
    {
        HeroInfo hi = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).GetHeroInfoById(id);

        UILabel idLabel = PanelTools.findChild<UILabel>(go, "id");
        idLabel.text = id.ToString();

        UISprite icon = PanelTools.findChild<UISprite>(go, "icon");
        icon.spriteName = hi.portarait;
        icon.MakePixelPerfect();

        UILabel level = PanelTools.findChild<UILabel>(go, "levelLabel");

        if (hi.isMaxLevel())
        {
            level.text = "LV.MAX";
        }
        else
        {
            level.text = "LV." + hi.level.ToString();
        }

        UISprite star = PanelTools.findChild<UISprite>(go, "starSprite");
        star.spriteName = "star" + hi.star.ToString();

        UISprite fight = PanelTools.findChild<UISprite>(go, "fightSprite");
        fight.gameObject.SetActive(hi.fight);

        UISprite seriesSprite = PanelTools.findChild<UISprite>(go, "seriesSprite");
        seriesSprite.spriteName = "SERIES" + hi.series.ToString();

        UISprite frameSprite = PanelTools.findChild<UISprite>(go, "frameSprite");
        frameSprite.spriteName = "frame" + hi.series.ToString();

        UISprite framebg = PanelTools.findChild<UISprite>(go, "framebg");
        framebg.spriteName = "framebg" + hi.series.ToString();
    }
}
