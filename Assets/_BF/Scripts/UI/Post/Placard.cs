using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class Placard : MonoBehaviour
{
    GameObject returnBtn;

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

        viewList = PanelTools.findChild(gameObject, "Scroll View");
        grid = PanelTools.findChild(viewList, "Grid");
        item = PanelTools.findChild(viewList, "Item");

        ShowPlacard();
	}

    void onReturn(GameObject go)
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }

    void onItem(GameObject go)
    {
        
    }

    class Item
    {
        public GameObject root;
        public UILabel id;
        public UISprite icon;
        public UILabel name;

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
        foreach (Item r in itemList)
        {
            r.Release();
        }

        itemList.Clear();
    }

    List<Item> itemList = new List<Item>();

    public void ShowPlacard()
    {
        ClearUI();

        for (int i = 0; i < 10; ++i)
        {

            Item placardItem = new Item();

            placardItem.root = NGUITools.AddChild(grid, item);
            placardItem.root.name = i.ToString();
            placardItem.root.SetActive(true);

            UIEventListener.Get(placardItem.root).onClick = onItem;

            itemList.Add(placardItem);
        }

        UIGrid uiGrid = grid.GetComponent<UIGrid>();
        uiGrid.repositionNow = true;
    }


}
