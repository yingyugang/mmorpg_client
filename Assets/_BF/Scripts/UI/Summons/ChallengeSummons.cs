using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class ChallengeSummons : MonoBehaviour
{

    GameObject returnBtn;

    GameObject summonsView;
    GameObject item;
    GameObject grid;
    GameObject leftBtn;
    GameObject rightBtn;
    GameObject challengeBtn;

    // Use this for initialization
	void Start () {

        returnBtn = PanelTools.findChild(gameObject, "Title/btnBack");
        if (returnBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onReturn;
        }

        summonsView = PanelTools.findChild(gameObject, "Scroll View");
        grid = PanelTools.findChild(summonsView, "Grid");
        item = PanelTools.findChild(summonsView, "Item");

        leftBtn = PanelTools.findChild(gameObject, "Title/leftButton");
        UIEventListener.Get(leftBtn).onClick = onLeft;

        rightBtn = PanelTools.findChild(gameObject, "Title/rightButton");
        UIEventListener.Get(rightBtn).onClick = onRight;

        challengeBtn = PanelTools.findChild(gameObject, "challengeButton");
        UIEventListener.Get(challengeBtn).onClick = onChallenge;

        ShowSummons();
	}

    void onReturn(GameObject go)
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }

    void onChallenge(GameObject go)
    {
        DataManager.getModule<DataSummons>(DATA_MODULE.Data_Summons).ShowOrgTeam();
    }

    void onLeft(GameObject go)
    {
        AlineOnChild aoc = grid.GetComponent<AlineOnChild>();
        CycleGrid cg = grid.GetComponent<CycleGrid>();

        aoc.CenterOn(cg.preTransform);
        cg.LeftMove();
    }

    void onRight(GameObject go)
    {
        AlineOnChild aoc = grid.GetComponent<AlineOnChild>();
        CycleGrid cg = grid.GetComponent<CycleGrid>();

        aoc.CenterOn(cg.nextTransform);
        cg.RightMove();
    }

    class Item
    {
        public GameObject root;
        public UILabel id;
        public UISprite icon;
        public UILabel name;
        public UILabel number;

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

    public void ShowSummons()
    {
        ClearUI();
        TestDate();

        int nCount = dataList.Count;

        for (int i = 0; i < nCount; ++i)
        {
            SummonsData sd = dataList[i];

            if (sd == null)
            {
                return;
            }

            Item summonsItem = new Item();

            summonsItem.root = NGUITools.AddChild(grid, item);
            summonsItem.root.name = sd.id.ToString();
            summonsItem.root.SetActive(true);

            summonsItem.number = PanelTools.findChild<UILabel>(summonsItem.root, "NOLabel");
            summonsItem.number.text = "NO." + (i + 1).ToString();
//             summonsItem.icon = PanelTools.findChild<UISprite>(summonsItem.root, "heroIcon");
//             summonsItem.icon.spriteName = sd.icon.ToString();
// 
//             summonsItem.name = PanelTools.findChild<UILabel>(summonsItem.root, "heroName");
//             summonsItem.name.text = sd.name.ToString();


            itemList.Add(summonsItem);
        }

        CycleGrid cg = grid.GetComponent<CycleGrid>();
        cg.repositionNow = true;
    }


    class SummonsData
    {
        public int id;
        public string name;
        public int icon;
        public int isObtain;
    }

    List<SummonsData> dataList = new List<SummonsData>();

    void TestDate()
    {
        for (int i = 0; i < 10; ++i)
        {
            SummonsData sd = new SummonsData();

            sd.name = "幻兽" + i.ToString();
            sd.icon = 10007 + i;
            sd.isObtain = i % 2;

            dataList.Add(sd);
        }
    }
}
