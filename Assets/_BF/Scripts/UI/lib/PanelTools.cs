using System;
using UnityEngine;
using System.Collections.Generic;

namespace UI
{
	class PanelTools
	{
        /*动态图集部分*/
        static Dictionary<string, UIAtlas> UIAtlasList;
        static Dictionary<string, UIAtlas> GetUIAtlasList()
        {
            if (UIAtlasList != null)
                return UIAtlasList;

            UIAtlasList = new Dictionary<string, UIAtlas>();
            UIAtlas[] atlas = Resources.FindObjectsOfTypeAll<UIAtlas>();
            foreach (UIAtlas atla in atlas)
            {
                if (UIAtlasList.ContainsKey(atla.name) == false)
                {
                    UIAtlasList[atla.name] = atla;
                }
            }
            return UIAtlasList;
        }

        static public UIAtlas GetUIAtlas(string name)
        {
            UIAtlas atlas = null;
            if (GetUIAtlasList().TryGetValue(name, out atlas))
                return atlas;
            return null;
        }

        //设置对象图集图片
        public static void SetSpriteIcon(UISprite sprite, string strAtlas, string strIcon)
        {
            if (sprite == null || strAtlas==null || strIcon==null)
                return;
            sprite.atlas = GetUIAtlas(strAtlas);
            sprite.spriteName = strIcon;
        }

        //窗口查找
        //查找子窗口,路径用"/"分割
        public static Transform findChild(Transform tfParent, string strName)
        {
            if (tfParent == null || strName.Equals(string.Empty))
                return null;

            string strTag = "";
            int nIndex = strName.IndexOf("/");
            if (nIndex != -1)
            {
                strTag = strName.Substring(0, nIndex);
                strName = strName.Substring(nIndex + 1);
            }
            else
            {
                strTag = strName;
            }

            Transform tfObject = tfParent.FindChild(strTag);
            if (tfObject == null)
            {
                Transform dragPanelT = tfParent.FindChild("dragPanel");
                return findChild(dragPanelT, strName);
            }

            if (strTag == strName && nIndex == -1)
                return tfObject;
            else
                return findChild(tfObject, strName);
        }

        public static GameObject findChild(GameObject parent, string strName)
        {
            if (parent == null)
                return null;
            Transform obj = findChild(parent.transform, strName);
            if (obj == null)
                return null;
            return obj.gameObject;
        }

        public static T findChild<T>(Transform tfParent, string strName) where T : MonoBehaviour
        {
            Transform obj = findChild(tfParent, strName);
            if (obj == null)
                return null;
            T objCom = obj.gameObject.GetComponent<T>();
            return objCom;
        }

        public static T findChild<T>(GameObject tfParent, string strName) where T : MonoBehaviour
        {
            if (tfParent == null)
                return null;
            return findChild<T>(tfParent.transform, strName);
        }

        //直接设置Label文本
        public static UILabel setLabelText(Transform tfParent, string strName, string strText)
        {
            UILabel lable = findChild<UILabel>(tfParent,strName);
            if (lable == null)
                return null;
            lable.text = strText;
            return lable;
        }

        public static UIButton setBtnFunc(Transform tfParent, string strName, UIEventListener.VoidDelegate func)
        {
            UIButton btn = findChild<UIButton>(tfParent, strName);
            if (btn == null)
                return null;
            UIEventListener.Get(btn.gameObject).onClick = func;
            return btn;
        }

        /*
        通过分隔符'/'来确定父子窗口设置多语言字符串查找子窗口中的UILabel,
        然后设置标签
        */
        public static UILabel setLabelLanguage(Transform parent, string strName, int id)
        {
            return setLabelText(parent, strName, BaseLib.LanguageMgr.getString(id));
        }

        public static UILabel setLabelLanguage(Transform parent, string strName, string id)
        {
            return setLabelText(parent, strName, BaseLib.LanguageMgr.getString(id));
        }
    }
}
