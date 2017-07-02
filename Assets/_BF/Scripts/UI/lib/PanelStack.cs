using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public delegate void panelBackFunc(System.Object param);

    class objList 
    {
        public GameObject obj;
        public objList next;
        public objList pre;
        public panelBackFunc _callBackfun = null;        
        public objList()
        {
            obj = null;
            next = null;
            pre = null;
            _callBackfun = null;
        }
    }


    public class PanelStack : Singleton<PanelStack>
	{
        objList _cur = null;
        public GameObject root { get; set; }

        private PanelStack()
        {

        }

        public void clear()
        {
            if (_cur == null)
                return;
            while (_cur != null)
            {
                if(_cur.obj != null)
                {
                    _cur.obj.SetActive(false);
                    _cur.obj = null;
                }
                _cur = _cur.pre;
            }
        }

        public GameObject goNext(string strPath,panelBackFunc func = null)
        {
            GameObject obj = this.FindPanel(strPath);
            if (obj == null)
                return null;
            return goNext(obj, func);
        }

        //
        public GameObject goNext(GameObject panel,panelBackFunc func = null)
        {
            if (panel == null)
                return null;

            objList newObj = new objList();
            newObj.obj = panel;
            newObj._callBackfun = func;
            if (_cur == null)
                _cur = newObj;    
            else
            {
                if (_cur.obj != null)
                {
                    _cur.obj.SetActive(false);
                }
                newObj.pre = _cur;
                _cur.next = newObj;
                _cur = _cur.next;
            }
            panel.SetActive(true);
            return panel;
        }

        public GameObject getPreObj()
        {
            if (_cur == null || _cur.pre == null || _cur.pre.obj==null)
                return null;
            return _cur.pre.obj;
        }

        public T getPreObjComponent<T>() where T : Component
        {
            return this.getComponent<T>(getPreObj());
        }

        public GameObject getNextObj()
        {
            if (_cur == null || _cur.next == null)
                return null;
            return _cur.next.obj;
        }

        public T getNextObjComponent<T>() where T : Component
        {
            return this.getComponent<T>(getNextObj());
        }

        //返回上一级
        public GameObject goBack()
        {
            if (_cur == null)
                return null;
            GameObject curObj = null;
            if (_cur.obj != null)
            {
                _cur.obj.SetActive(false);
                curObj = _cur.obj;
            }
            if (_cur._callBackfun != null)
                _cur._callBackfun(curObj);
            _cur = _cur.pre;
            if (_cur!=null && _cur.obj != null)
            {
                _cur.obj.SetActive(true);
                return _cur.obj;
            }
            return null;
        }

        // 查找子窗口,通过分隔符'/'来确定父子窗口
        public GameObject FindPanel(string name)
        {
            /*GameObject rootTemp = UICamera.currentCamera.gameObject;
            if (rootTemp == null)
                rootTemp = this.root;*/


			GameObject rootTemp = null;
			if (UICamera.currentCamera == null)
				rootTemp = this.root;
			else
				rootTemp = UICamera.currentCamera.gameObject;


            if (rootTemp == null || name == null)
                return null;
            string[] childs = name.Split('/');
            Transform p = rootTemp.transform;
            foreach (string child in childs)
            {
                p = p.transform.Find(child);
                if (p == null)
                    return null;
            }
            return p.gameObject;
        }

        public T getComponent<T>(GameObject obj) where T : Component
        {
            if (obj == null)
                return null;
            T[] list = obj.GetComponentsInChildren<T>(true);
            if (list.Length < 1)
                return null;
            return list[0];
        }
	}
}
