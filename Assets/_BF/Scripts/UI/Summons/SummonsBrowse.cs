using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class SummonsBrowse : MonoBehaviour {

    GameObject returnBtn;
    GameObject closeBtn;

    GameObject summonsList;
    GameObject item;
    GameObject grid;

    public int nState = 1;  //1.detail  2.strengthen 3.evolution 4.quality

    int maxCount = 0;
    int curCount = 0;
    // Use this for initialization
	void Start () {

        returnBtn = PanelTools.findChild(gameObject, "Title/btnBack");
        if (returnBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onReturn;
        }
        closeBtn = PanelTools.findChild(gameObject, "Title/btnClose");
        if (closeBtn != null)
        {
            UIEventListener.Get(closeBtn).onClick = onClose;
        }
	}

    void OnEnable()
    {
        ShowSummons();
    }

    void onClose(GameObject go)
    {
        transform.parent.gameObject.SetActive(false);
    }
    
    void onReturn(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowPanelSummons();
    }

    void onItem(GameObject go)
    {
        UILabel idLabel = PanelTools.findChild<UILabel>(go, "idLabel");
        uint id = uint.Parse(idLabel.text);
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).SelectSummonId = id;
        
        switch (nState)
        {
            case 1:
                DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowSummonsDetail(gameObject);
                break;
            case 2:
                DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowStrengthen();
                break;
            case 3:
                DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowBreak();
                break;
            case 4:
                DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowQualityStrengthen();
                break;
        }
    }

    void onLock(GameObject go)
    {
        UILabel idLabel = PanelTools.findChild<UILabel>(go, "idLabel");
        uint id = uint.Parse(idLabel.text);
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).SelectSummonId = id;

        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowUnlock();

    }

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

    public List<row> rowList = new List<row>();

    class Item
    {
        public GameObject root;
        public UILabel id;
        public UISprite icon;
        public UILabel name;
        public UILabel level;
        public UISprite star;
        public UISprite series;
        public UISprite frame;
        public UISprite framebg;
        public UISprite unlock;

        public void Release()
        {
            if (root != null)
            {
                GameObject.Destroy(root);
            }
        }
    }

    public void IntiUI()
    {
        summonsList = PanelTools.findChild(gameObject, "Scroll View");
        grid = PanelTools.findChild(summonsList, "Grid");
        item = PanelTools.findChild(summonsList, "row");

        ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_SUMMON);
        List<ConfigRow> showRows = new List<ConfigRow>();
        for (int i = 0; i < table.rows.Length; ++i)
        {
            if (table.rows[i].getIntValue(DICT_SUMMON.STAR) != 1)
            {
                continue;
            }

            showRows.Add(table.rows[i]);
        }

        maxCount = showRows.Count;

        for (int i = 0; i < (maxCount / 3 + 1); ++i)
        {

            row _row = new row();
            _row.root = NGUITools.AddChild(grid, item);
            _row.root.name = i.ToString();
            _row.root.SetActive(true);

            Vector3 position = _row.root.transform.position;
            _row.root.transform.localPosition = new UnityEngine.Vector3(position.x, position.y - i * 250, position.z);
            rowList.Add(_row);

            for (int j = i * 3; j < (i * 3 + 3); ++j)
            {
                if (j >= maxCount)
                {
                    return;
                }

                ConfigRow summonRow = showRows[j];
                Item summonItem = new Item();

                summonItem.root = PanelTools.findChild(_row.root, _row.child.ToString());
                summonItem.root.SetActive(true);

                string strTemp = summonRow.getStringValue(DICT_SUMMON.SUMMON_TYPEID);
                summonItem.root.name = strTemp.Substring(0, 4);
                summonItem.id = PanelTools.findChild<UILabel>(summonItem.root, "idLabel");
                summonItem.id.text = summonRow.getStringValue(DICT_SUMMON.SUMMON_TYPEID);

                summonItem.unlock = PanelTools.findChild<UISprite>(summonItem.root, "unlock");
                summonItem.unlock.gameObject.SetActive(true);

                UIEventListener.Get(summonItem.root).onClick = onLock;

                itemList.Add(summonItem);

                ++_row.child;
            }
        }

        UIGrid uiGrid = grid.GetComponent<UIGrid>();
        uiGrid.repositionNow = true;
    }
    
    public void ClearUI()
    {
        foreach (Item r in itemList)
        {
            r.Release();
        }

        itemList.Clear();
    }

    List<Item> itemList = new List<Item>();

    public void ShowSummons()
    {
        ClearUI();
        IntiUI();

        SummonInfo[] summonList = DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).getSummonList();
        curCount = summonList.Length;

        UILabel NOLabel = PanelTools.findChild<UILabel>(gameObject, "Title/NOLabel");
        NOLabel.text = curCount.ToString() + "/" + maxCount.ToString();

        for (int i = 0; i < curCount; ++i)
        {
            SummonInfo si = summonList[i];

            foreach (Item item in itemList)
            {
                if (si.type4 == uint.Parse(item.root.name))
                {
                    item.unlock = PanelTools.findChild<UISprite>(item.root, "unlock");
                    item.unlock.gameObject.SetActive(false);

                    item.id = PanelTools.findChild<UILabel>(item.root, "idLabel");
                    item.id.text = si.type.ToString();

                    item.name = PanelTools.findChild<UILabel>(item.root, "name");
                    item.name.text = si.name;

                    if (si.name == string.Empty)
                    {
                        item.name.text = si.type.ToString();
                    }

                    item.icon = PanelTools.findChild<UISprite>(item.root, "icon");
                    item.icon.spriteName = si.portarait;

                    item.level = PanelTools.findChild<UILabel>(item.root, "level");
                    item.level.text = "LV." + si.level.ToString();

                    item.star = PanelTools.findChild<UISprite>(item.root, "star");
                    item.star.spriteName = "star" + si.star.ToString();

                    item.frame = PanelTools.findChild<UISprite>(item.root, "frame");
                    item.frame.spriteName = "quality" + si.quality.ToString();

                    item.framebg = PanelTools.findChild<UISprite>(item.root, "framebg");
                    item.framebg.spriteName = "framebg" + si.series.ToString();

                    UIEventListener.Get(item.root).onClick = onItem;
                }
            }
        }
    }

}
