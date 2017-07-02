using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class PostList : MonoBehaviour
{
    GameObject returnBtn;
    GameObject allBtn;

    GameObject viewList;
    GameObject item;
    GameObject grid;

  
    // Use this for initialization
	void Start () {

        returnBtn = PanelTools.findChild(gameObject, "Title/btnBack");
        if (returnBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onReturn;
        }

        allBtn = PanelTools.findChild(gameObject, "Title/allBtn");
        if (allBtn != null)
        {
            UIEventListener.Get(allBtn).onClick = onAll;
        }

	}

    void onReturn(GameObject go)
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }

    void onAll(GameObject go)
    {
        DataManager.getModule<DataPresent>(DATA_MODULE.Data_Present).getPresent(0);
    }

    void onItem(GameObject go)
    {
        UILabel idLabel = PanelTools.findChild<UILabel>(go.transform.parent.gameObject, "idLabel");
        uint idPresent = uint.Parse(idLabel.text);
        DataManager.getModule<DataPresent>(DATA_MODULE.Data_Present).getPresent(idPresent);
        
    }

    class Item
    {
        public GameObject root;
        public UILabel id;
        public UISprite giftIcon;
        public UILabel name;
        public UILabel desc;
        public UILabel time;


        public void Release()
        {
            if (root != null)
            {
                GameObject.Destroy(root);
            }
        }
    }

    public void InitUI()
    {
        viewList = PanelTools.findChild(gameObject, "Scroll View");
        grid = PanelTools.findChild(viewList, "Grid");
        item = PanelTools.findChild(viewList, "Item");
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

    public void ShowPost()
    {
        ClearUI();
        InitUI();

        PresentInfo[] pis = DataManager.getModule<DataPresent>(DATA_MODULE.Data_Present).getPresentList();

        int nCount = pis.Length;

        for (int i = 0; i < nCount; ++i)
        {

            PresentInfo pi = pis[i];

            Item PostItem = new Item();

            PostItem.root = NGUITools.AddChild(grid, item);
            PostItem.root.name = pi.id.ToString();
            PostItem.root.SetActive(true);

            PostItem.id = PanelTools.findChild<UILabel>(PostItem.root, "idLabel");
            PostItem.id.text = pi.id.ToString();

            PostItem.name = PanelTools.findChild<UILabel>(PostItem.root, "nameLabel");
            PostItem.name.text = pi.name;

            PostItem.desc = PanelTools.findChild<UILabel>(PostItem.root, "diecLabel");
            PostItem.desc.text = pi.desc;

            PostItem.time = PanelTools.findChild<UILabel>(PostItem.root, "timeLabel");
            PostItem.time.text = DataManager.getModule<DataServTime>(DATA_MODULE.Data_ServTime).getServTime(pi.getTime, "yyyy-MM-dd HH:mm");
                

            GameObject recBtn = PanelTools.findChild(PostItem.root, "recButton");
            UIEventListener.Get(recBtn).onClick = onItem;

            itemList.Add(PostItem);
        }

        UIGrid uiGrid = grid.GetComponent<UIGrid>();
        uiGrid.repositionNow = true;
    }


}
