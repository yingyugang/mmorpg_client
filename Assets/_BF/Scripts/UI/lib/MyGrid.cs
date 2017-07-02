using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyGrid : MonoBehaviour 
{
    public UIScrollView _scrollView;
    UIPanel _thePanel;
    ObjList _theList = new ObjList();
    int _count;
    float _perWidth;
    Transform _curObj = null;

    void Start()
    {
        if (this._scrollView != null)
            _thePanel = this._scrollView.GetComponent<UIPanel>();

        _count = transform.childCount;
        for (int n = 0; n < _count; n++)
            _theList.addObj(transform.GetChild(n));
        _perWidth = Mathf.Abs(transform.GetChild(0).localPosition.x - transform.GetChild(1).localPosition.x);
        _theList.fWidth = Mathf.Abs(transform.GetChild(0).position.x - transform.GetChild(1).position.x);
    }

    void Update()
    {
        Transform curObj = getCurObj();
        if (curObj == _curObj)
            return;
        _curObj = curObj;
        if (_theList.setCurItem(_curObj))
        {
        }
    }

    Transform getCurObj()
    {
        if (this._thePanel == null)
            return null;

        float offset = this._thePanel.clipOffset.x;

        for(int i =0;i<this._count;i++)
        {
            Transform child = transform.GetChild(i);
            float temp = Mathf.Abs(child.localPosition.x - offset);
            if(temp*2 < this._perWidth)
                return child;
        }
        return null;
    }


    class ObjItem
    {
        public ObjItem _pre;
        public ObjItem _next;
        public Transform _object;

        public ObjItem()
        {
        }

        public bool isEquals(Transform item)
        {
            return this._object == item;
        }

        public bool isEquals(ObjItem item)
        {
            return this._object == item._object;
        }

        public void setPosition(float x)
        {
            this._object.position = new Vector3(x, this._object.position.y, this._object.position.z);
            foreach (UIWidget item in this._object.gameObject.GetComponentsInChildren<UIWidget>())
            {
                item.Invalidate(false);
            }
        }

        public void scale(float ratio)
        {
        }
    }

    class ObjList
    {
        public float fWidth;
        ObjItem _curItem;
        ObjItem _tailer;

        public void addObj(Transform obj)
        {
            ObjItem newItem = new ObjItem();
            newItem._object = obj;

            if (_curItem == null)
            {
                _curItem = newItem;
                _curItem._next = newItem;
                _curItem._pre = newItem;
            }
            else
            {
                _curItem._next._pre = newItem;
                newItem._next = _curItem._next;
                _curItem._next = newItem;
                newItem._pre = _curItem;
                _curItem = newItem;
            }
        }

        public bool setCurItem(Transform obj)
        {
            if (obj == null)
                return false;
            if (_curItem == null)
                return false;
            if (obj == _curItem._object)
                return false;

            ObjItem item = _curItem._next;

            while (true)
            {
                if (item.isEquals(obj))
                {
                    _curItem = item;
                    //把左，右各移动一格
                    if (!_curItem._pre.isEquals(item))
                    {
                        _curItem._pre.setPosition(_curItem._object.position.x - fWidth);
                    }
                    if (!_curItem._next.isEquals(item))
                    {
                        _curItem._next.setPosition(_curItem._object.position.x + fWidth);
                    }
                    return true;
                }
                else if(item.isEquals(_curItem))
                {
                    break;
                }
                item = item._next;
            }
            return false;
        }
    }
}
