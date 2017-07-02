using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BaseLib;

namespace DataCenter
{
    public class DataShop : DataModule
    {
        public DataShop()
        {
        }

        public override void release()
        {
        }

        public override bool init()
        {

            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_BUY_HERO_SIZE, onBuyHeroSize, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLEINT_BUY_ITEM_SIZE, onBuyItemSize, (int)DataCenter.EVENT_GROUP.packet);

            return true;
        }

        void onBuyHeroSize(int nEvent, System.Object param)
        {
            MSG_BUY_HERO_SIZE_RESPONSE msg = (MSG_BUY_HERO_SIZE_RESPONSE)param;

            if (msg.err == 100000)
            {
                Debug.Log("BuyHeroSizeSuc");

                ShowShopNotice();
            }
        }

        void onBuyItemSize(int nEvent, System.Object param)
        {
            MSG_BUY_ITEM_SIZE_RESPONSE msg = (MSG_BUY_ITEM_SIZE_RESPONSE)param;

            if (msg.wErrCode == 100000)
            {
                Debug.Log("BuyItemSizeSuc");

                ShowShopNotice();
            }
        }

        //UI界面
        public PanelShop panelShop;

        public void ShowShopMenu()
        {
            panelShop.ShowPageByIndex(0);
        }

        public void ShowShopDiamond()
        {
            panelShop.ShowPageByIndex(1);
        }

        public void ShowShopComfirm(string strTitle, string strContent, UIEventListener.VoidDelegate func)
        {
			panelShop.gameObject.SetActive(true);
			GameObject comfirm = panelShop.ShopPages[2];
            comfirm.SetActive(true);
            ShopComfirm sc = comfirm.GetComponent<ShopComfirm>();
            sc.ShowInfo(strTitle, strContent, func);
        }

        public void ShowShopNotice()
        {
            string title = "购买成功";
            string content = "购买成功";

            GameObject notice = panelShop.ShopPages[3];
            notice.SetActive(true);

            ShopComfirm sc = notice.GetComponent<ShopComfirm>();
            sc.ShowInfo(title, content, null);
        }
    }
}