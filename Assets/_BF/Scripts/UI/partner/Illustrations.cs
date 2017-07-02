using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class Illustrations : MonoBehaviour
{

    public UIButton btnReturn;

    public GameObject menu;
    public GameObject detail;
    public GameObject Item;
    public GameObject grid;


    // Use this for initialization
	void Start ()
    {
        if (btnReturn != null)
            UIEventListener.Get(btnReturn.gameObject).onClick = onReturn;


        InitUI();
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

    void onDetail(GameObject go)
    {
        UILabel idLabel = PanelTools.findChild<UILabel>(go, "id");

        int id = int.Parse(idLabel.text);
        
        gameObject.SetActive(false);
        detail.SetActive(true);


        Explanation ex = detail.GetComponent<Explanation>();
        ex.ShowHeroDetail(id);

    }

    public void InitUI()
    {
        UILabel returnLabel = PanelTools.findChild<UILabel>(gameObject, "Titlebg/ReturnButton/Label");
        //返回
        returnLabel.text = BaseLib.LanguageMgr.getString("66361007");

        UILabel titleLabel = PanelTools.findChild<UILabel>(gameObject, "Titlebg/titleLabel");
        //伙伴图鉴
        titleLabel.text = BaseLib.LanguageMgr.getString("66361008");
    }

    public List<row> rowList = new List<row>();

    public class row
    {
        public GameObject root;
        public int child = 0;

        public void Release()
        {
            if (root != null)
            {
                GameObject.Destroy(root);
            }
        }
    }

    public class HeroItem
    {
        public GameObject root;
        public UILabel id;
        public UISprite icon;
        public UILabel no;
        public UILabel star;
    }

    public void ClearUI()
    {
        foreach (row r in rowList)
        {
            r.Release();
        }

        rowList.Clear();
    }

    public void ShowHero()
    {
        ClearUI();
        ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_HERO);
        int nCount = table.RowList.Count;

        for (int i = 0; i < (nCount / 5 + 1); ++i)
        {
            row _row = new row();

            _row.root = NGUITools.AddChild(grid, Item);
            _row.root.name = i.ToString();
            _row.root.SetActive(true);

            Vector3 position = _row.root.transform.position;

            _row.root.transform.localPosition = new UnityEngine.Vector3(position.x, position.y - i * 120, position.z);

            rowList.Add(_row);

            for (int j = i * 5; j < (i * 5 + 5); ++j)
            {
                if (j >= nCount)
                {
                    return;
                }

                HeroItem heroItem = new HeroItem();
                heroItem.root = PanelTools.findChild(_row.root, _row.child.ToString());
                heroItem.id = PanelTools.findChild<UILabel>(heroItem.root, "id");
                heroItem.id.text = table.RowList[j].getIntValue(DICT_HERO.HERO_TYPEID).ToString();

                heroItem.icon = PanelTools.findChild<UISprite>(heroItem.root, "icon");
                heroItem.icon.spriteName = table.RowList[j].getStringValue(DICT_HERO.PORTARAIT);

                heroItem.no = PanelTools.findChild<UILabel>(heroItem.root, "levelLabel");
                heroItem.no.text = "NO." + j.ToString();

                heroItem.star = PanelTools.findChild<UILabel>(heroItem.root, "starLabel");
                heroItem.star.text = table.RowList[j].getIntValue(DICT_HERO.STAR).ToString();

                heroItem.root.SetActive(true);
                UIEventListener.Get(heroItem.root).onClick = onDetail;

                ++_row.child;
            }

        }
    }


}
