using UnityEngine;
using System.Collections;
using DataCenter;

public class mainHero : MonoBehaviour {

    UISprite _spriteClass;
    GameObject _spriteCaptain;
    UISprite _spriteHero;
	// Use this for initialization
    HeroInfo _curHero;
    UIButton _heroBtn;

	void Awake () 
    {
        _heroBtn = UI.PanelTools.setBtnFunc(transform, "icon", onClick);
        if (_heroBtn != null)
            _heroBtn.normalSprite = null;

        _spriteClass = UI.PanelTools.findChild<UISprite>(transform, "class");
        _spriteCaptain = UI.PanelTools.findChild(gameObject, "captain");
        _spriteHero = UI.PanelTools.findChild<UISprite>(transform, "icon/Background");
    }

    void clear()
    {
        this._curHero = null;
        this.gameObject.SetActive(false);
        if (this._spriteHero != null)
            _spriteHero.atlas = null;
    }

    void onClick(GameObject obj)
    {
        //详细信息
        if(this._curHero!=null)
        {
            DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).MainShowHeroDetailById(this._curHero.id);
        }
    }

    public void setCaptain(bool bFlag)
    {
        if (_spriteCaptain != null)
            _spriteCaptain.SetActive(bFlag);
    }

    public void init(HeroInfo hero)
    {
        if (hero == null)
        {
			this.gameObject.SetActive(false);
        }
        else
        {
            this._curHero = hero;
            if (_heroBtn != null)
                _heroBtn.normalSprite = null;
            UI.PanelTools.SetSpriteIcon(_spriteHero, hero.iconFile, hero.spriteName);
            showClassIcon(hero.series);
        }
    }

    void showClassIcon(int nType)
    {
        string strClass = null;
        switch (nType)
        {
            case (int)DataCenter.HERO_SERIES.HERO_SERIES_FIRE:
                strClass = "cls_red_38";
                break;
            case (int)DataCenter.HERO_SERIES.HERO_SERIES_WATER:
                strClass = "cls_blue_38";
                break;
            case (int)DataCenter.HERO_SERIES.HERO_SERIES_WIND:
                strClass = "cls_yellow_38";
                break;
            case (int)DataCenter.HERO_SERIES.HERO_SERIES_WOOD:
                strClass = "cls_green_38";
                break;
            case (int)DataCenter.HERO_SERIES.HERO_SERIES_BRIGHT:
                strClass = "cls_white_38";
                break;
            case (int)DataCenter.HERO_SERIES.HERO_SERIES_DARK:
                strClass = "cls_purple_38";
                break;
        }
        if(_spriteClass!=null)
            UI.PanelTools.SetSpriteIcon(_spriteClass,"mainUI",strClass);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
