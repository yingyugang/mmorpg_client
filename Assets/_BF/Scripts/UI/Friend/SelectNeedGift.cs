using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class SelectNeedGift : MonoBehaviour {

    GameObject returnBtn;

    GameObject Grid;
    GameObject Item;

    public int nIndex = 1;
    // Use this for initialization
	void Start () {

        returnBtn = PanelTools.findChild(gameObject, "Title/btnBack");
        if (returnBtn != null)
        {
            UIEventListener.Get(returnBtn).onClick = onReturn;
        }


        Grid = PanelTools.findChild(gameObject, "GiftList/ListGrid");
        Item = PanelTools.findChild(gameObject, "GiftList/row");

        ShowGiftType();
	}

    void onReturn(GameObject go)
    {
        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).ShowFriendGift();

    }

    void onItem(GameObject go)
    {
        UILabel idLabel = PanelTools.findChild<UILabel>(go, "id");
        uint id = uint.Parse(idLabel.text);


        DataManager.getModule<DataFriend>(DATA_MODULE.Data_Friend).setGift(nIndex, id);
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

    public class GiftItem
    {
        public GameObject root;
        public UILabel id;
        public UISprite giftIcon;
        public UILabel count;
        public UISprite frame;
        public UILabel name;
    }

    public void ShowGiftType()
    {
        ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.DICT_FRIEND_GIFT);
        int nCount = table.rows.Length;

        for (int i = 0; i < (nCount / 5 + 1); ++i)
        {
            row _row = new row();

            _row.root = NGUITools.AddChild(Grid, Item);
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

                ConfigRow row = table.rows[j];

                GiftItem giftItem = new GiftItem();
                giftItem.root = PanelTools.findChild(_row.root, _row.child.ToString());
                UIEventListener.Get(giftItem.root).onClick = onItem;
                giftItem.root.SetActive(true);
                ++_row.child;

                giftItem.id = PanelTools.findChild<UILabel>(giftItem.root, "id");
                giftItem.id.text = row.getStringValue(DICT_FRIEND_GIFT.GIFT_TYPEID);
                giftItem.count = PanelTools.findChild<UILabel>(giftItem.root, "countLabel");
                giftItem.count.text = row.getStringValue(DICT_FRIEND_GIFT.AMOUNT);

                DataCenter.FriendGift fg = new DataCenter.FriendGift();
                int type = row.getIntValue(DICT_FRIEND_GIFT.GIFT_TYPEID);
                fg.iniGift(type);


                giftItem.name = PanelTools.findChild<UILabel>(giftItem.root, "nameLabel");
                giftItem.name.text = fg.giftName;

                giftItem.giftIcon = PanelTools.findChild<UISprite>(giftItem.root, "giftIcon");
                giftItem.giftIcon.spriteName = fg.giftIcon;





            }

        }

    }
}
