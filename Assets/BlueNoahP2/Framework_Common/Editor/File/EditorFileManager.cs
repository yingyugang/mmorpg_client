using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using BlueNoah.IO;

namespace BlueNoah.Editor.IO
{
    public class EditorFileManager
    {
        //be better to use the MonoScript.FromMonoBehaviour.
        public static string FindAsset(string fileName, string filePattern)
        {
            string[] guids = AssetDatabase.FindAssets(fileName);
            foreach (string guid in guids)
            {
                string filePath = AssetDatabase.GUIDToAssetPath(guid);
                if (filePath.IndexOf(string.Format("{0}.{1}", fileName, filePattern), StringComparison.CurrentCulture) != -1)
                {
                    return filePath;
                }
            }
            return null;
        }

        public static List<string> FindAssets(string fileName, string filePattern)
        {
            string[] guids = AssetDatabase.FindAssets(fileName);
            List<string> paths = new List<string>();
            foreach (string guid in guids)
            {
                string filePath = AssetDatabase.GUIDToAssetPath(guid);
                if (filePath.IndexOf(string.Format("{0}.{1}", fileName, filePattern), StringComparison.CurrentCulture) != -1)
                {
                    paths.Add(filePath);
                }
            }
            Debug.Log("paths.Count:" + paths.Count);
            return paths;
        }

        public static string ResourcesPathToFilePath(string resourcePath)
        {
            UnityEngine.Object obj = Resources.Load<UnityEngine.Object>(resourcePath);
            if (obj == null)
                return "";
            return AssetDatabasePathToFilePath(obj);
        }

        public static string AssetDatabasePathToFilePath(UnityEngine.Object obj)
        {
            string path = "";
            if (obj != null)
            {
                path = UnityEditor.AssetDatabase.GetAssetPath(obj);
            }
            return AssetDatabasePathToFilePath(path);
        }

        public static string AssetDatabasePathToFilePath(string path)
        {
            if (string.IsNullOrEmpty(path) || path.IndexOf("Assets", StringComparison.CurrentCulture) == -1)
            {
                return "";
            }
            return path = Application.dataPath + path.Remove(0, "Assets".Length);
        }

        public static List<T> LoadAllPrefabsOfType<T>(string path) where T : MonoBehaviour
        {
            if (path != "")
            {
                if (path.EndsWith("/", StringComparison.CurrentCulture))
                {
                    path = path.TrimEnd('/');
                }
            }
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] fileInf = dirInfo.GetFiles("*.prefab");
            //loop through directory loading the game object and checking if it has the component you want
            List<T> prefabComponents = new List<T>();
            foreach (FileInfo fileInfo in fileInf)
            {
                string fullPath = fileInfo.FullName.Replace(@"\", "/");
                string assetPath = "Assets" + fullPath.Replace(Application.dataPath, "");
                GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

                if (prefab != null)
                {
                    T hasT = prefab.GetComponent<T>();
                    if (hasT != null)
                    {
                        prefabComponents.Add(hasT);
                    }
                }
            }
            return prefabComponents;
        }

        public static List<GameObject> LoadAllPrefabs(string path)
        {
            if (path != "")
            {
                if (path.EndsWith("/", StringComparison.CurrentCulture))
                {
                    path = path.TrimEnd('/');
                }
            }
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] fileInf = dirInfo.GetFiles("*.prefab", SearchOption.AllDirectories);
            //loop through directory loading the game object and checking if it has the component you want
            List<GameObject> prefabs = new List<GameObject>();
            foreach (FileInfo fileInfo in fileInf)
            {
                string fullPath = fileInfo.FullName.Replace(@"\", "/");
                string assetPath = "Assets" + fullPath.Replace(Application.dataPath, "");
                GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
                prefabs.Add(prefab);
            }
            return prefabs;
        }

        public static MonoScript FindMono(string mono)
        {
            if (string.IsNullOrEmpty(mono))
                return null;
            Type t = GetType(mono);
            if (t == null)
                return null;
            if (!t.IsSubclassOf(typeof(MonoBehaviour)))
                return null;
            GameObject go = new GameObject();
            go.SetActive(false);
            MonoBehaviour monoBehaviour = (MonoBehaviour)go.AddComponent(t);
            MonoScript script = MonoScript.FromMonoBehaviour(monoBehaviour);
            GameObject.DestroyImmediate(go);
            return script;
        }

        public static string SearchSettingFile(string path)
        {
            string fileName = FileManager.GetFileNameFromPath(path);
            string fileMain = FileManager.GetFileMain(fileName);
            string filePattern = FileManager.GetFilePattern(fileName);
            string assetFilePath = FindAsset(fileMain, filePattern);
            return assetFilePath;
        }

        static Type GetType(string typeStr)
        {
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type type = a.GetType(typeStr);
                if (type != null)
                    return type;
            }
            return null;
        }
    }
}