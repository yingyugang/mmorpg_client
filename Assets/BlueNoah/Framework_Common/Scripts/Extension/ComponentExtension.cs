using System;
using UnityEngine;

namespace BlueNoah.Utility
{
    public static class ComponentExtension
    {
        public static RectTransform RectTransform(this GameObject obj)
        {
            return obj.transform as RectTransform;
        }

        public static RectTransform RectTransform(this MonoBehaviour obj)
        {
            return obj.transform as RectTransform;
        }

        public static void ResetLocal(this Transform trans)
        {
            trans.localScale = Vector3.one;
            trans.localEulerAngles = Vector3.zero;
            trans.localPosition = Vector3.zero;
        }

        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T t = go.GetComponent<T>();
            if (t == null)
            {
                t = go.AddComponent<T>();
            }
            return t;
        }

        public static Component GetOrAddComponent(this GameObject go, System.Type type)
        {
            Component comp = go.GetComponent(type);
            if (comp == null)
            {
                comp = go.AddComponent(type);
            }
            return comp;
        }

        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            T t = component.gameObject.GetComponent<T>();
            if (t == null)
            {
                t = component.gameObject.AddComponent<T>();
            }
            return t;
        }

        public static T Find<T>(this Transform transform, string path) where T : Component
        {
            Transform childTrans = transform.Find(path);
            if (childTrans == null)
                return null;
            return childTrans.GetComponent<T>();
        }

        public static void GetPath(this Transform transform, Component comp, ref string resultPath)
        {
            if (comp == null)
                return;
            string path = "/" + comp.gameObject.name;
            Transform trans = transform;
            GameObject obj = comp.gameObject;
            while (obj.transform.parent != null)
            {
                if (trans == obj.transform.parent)
                {
                    break;
                }
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            path = path.Remove(0, 1);
            resultPath = path;
        }
    }
}

