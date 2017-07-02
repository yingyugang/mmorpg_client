using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.IO;
using Object = UnityEngine.Object;
#pragma warning disable 0618
public class CreatePrefab 
{
    public static void addBoneMatching()
    {
        Object[] imported = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        if (imported.Length == 0)
            return;
        foreach (Object obj in imported)
        {
            if (UnityEditor.PrefabUtility.GetPrefabType(obj) != UnityEditor.PrefabType.Prefab)
                continue;
            GameObject preb = (GameObject)obj;
            preb.AddComponent<BoneMatching>();            
        }
    }

    public static void create()
    {
        string destPath = "Assets/ArtDate/Prefeb/Hero/";
        List<string> fileList = new List<string>();

        Object[] imported = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        if (imported.Length == 0)
            return;
        foreach (Object obj in imported)
        {
            string strTemp = AssetDatabase.GetAssetPath(obj);
            if (strTemp.Contains(".")) continue;
            getFbxList(strTemp, ref fileList);
            setMat(strTemp);
        }

        foreach (string file in fileList)
        {
            int begin = file.LastIndexOf("\\")+1;
            int end = file.IndexOf(".");
            if (end <= begin)
                continue;
            string strName = file.Substring(begin,end-begin);

            //已经存在的不覆盖直接返回。
            string strDestPath = destPath + strName + ".prefab";
            if (UnityEditor.AssetDatabase.LoadAssetAtPath(strDestPath, typeof(GameObject)) != null)
                continue;
            List<AnimationClip> animList = new List<AnimationClip>();
            //生成动作列表
            createAnimations(strName, ref animList);

            GameObject obj =(GameObject)UnityEditor.AssetDatabase.LoadAssetAtPath(file, typeof(GameObject));
            if (obj == null)
                continue;
            GameObject newObj = Object.Instantiate(obj) as GameObject;
            if (newObj == null)
                continue;
            SkinnedMeshRenderer[] meshlist = newObj.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach(SkinnedMeshRenderer item in meshlist)
            {
                Puppet2D_SortingLayer layerobj = item.gameObject.AddComponent<Puppet2D_SortingLayer>();
                layerobj.gameObject.GetComponent<Renderer>().sortingOrder = getLayerOrder(strName,layerobj.gameObject.name);
            }

            if (newObj.GetComponent<Animation>() != null)
            {
                if (animList.Count > 0)
                    newObj.GetComponent<Animation>().clip = animList[0];
                List<string> nameList = new List<string>();
                foreach (AnimationState state in newObj.GetComponent<Animation>())
                    nameList.Add(state.name);
                foreach (string name in nameList)
                    newObj.GetComponent<Animation>().RemoveClip(name);
            }

            foreach (AnimationClip item in animList)
            {
                newObj.GetComponent<Animation>().AddClip(item, item.name);
            }

            Animation[] AnimationList = newObj.GetComponentsInChildren<Animation>();
            foreach (Animation item in AnimationList)
            {
                item.cullingType = AnimationCullingType.AlwaysAnimate;
            }

            Selection.activeGameObject = PrefabUtility.CreatePrefab(strDestPath, newObj, ReplacePrefabOptions.Default);
            EditorGUIUtility.PingObject(Selection.activeGameObject);
            GameObject.DestroyImmediate(newObj as Object);
        }
    }

    static void setMat(string strPath)
    {
        List<string> theList = new List<string>();
        getMatList(strPath,ref theList);

        foreach (string file in theList)
        {
            Material obj = (Material)UnityEditor.AssetDatabase.LoadAssetAtPath(file, typeof(Material));
            Shader newShade = Shader.Find("Unlit/Transparent Colored");
            if (obj != null && newShade != null)
                obj.shader = newShade;
        }
    }

    static void getMatList(string strPath, ref List<string> theList)
    {
        string[] files = Directory.GetFiles(strPath);
        foreach (string file in files)
        {
            if (file.Contains(".meta")) continue;
            if (!file.Contains(".mat")) continue;
            theList.Add(file);
        }

        string[] dirs = Directory.GetDirectories(strPath);
        foreach (string dir in dirs)
        {
            getMatList(dir, ref theList);
        }
    }

    static int getLayerOrder(string heroName,string objName)
    {
        int value = 0;
        string srcSpritePath = "Assets/ArtDate/Hero/";
        string path = srcSpritePath + heroName + "/"+ heroName + "_StandBy.prefab";
        GameObject obj = (GameObject)UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
        if (obj == null)
            return 0;
        GameObject newObj = Object.Instantiate(obj) as GameObject;
        for (int index = 0; index < newObj.transform.childCount;index++)
        {
            Transform child = newObj.transform.GetChild(index);
            Debug.Log(child.gameObject.name);
            if (objName.Contains(child.gameObject.name))
            {
                SpriteRenderer render = child.gameObject.GetComponentInChildren<SpriteRenderer>();
                if(render!=null)
                {
                    value = render.sortingOrder;
                    break;
                }
            }
        }

        GameObject.DestroyImmediate(newObj as Object);
        return value;
    }

    static void getFbxList(string strPath,ref List<string> theList)
    {
        string[] files = Directory.GetFiles(strPath);
        foreach(string file in files)
        {
            if (file.Contains(".meta")) continue;
            if (file.Contains("@")) continue;
            if (!file.Contains(".FBX")) continue;
            theList.Add(file);
        }

        string[] dirs = Directory.GetDirectories(strPath);
        foreach (string dir in dirs)
        {
            if (dir.Contains(".")) continue;            
            getFbxList(dir,ref theList);
        }
    }

    static void getAnimList(string strPath,ref List<string> theList)
    {
        string[] files = Directory.GetFiles(strPath);
        foreach (string file in files)
        {
            if (file.Contains(".meta")) continue;
            if (!file.Contains("@")) continue;
            if (!file.Contains(".FBX")) continue;
            if (!checkAnimaionName(file))
            {
                UnityEngine.Debug.LogError(string.Format("{0} name error!",file));
                continue;
            }
            theList.Add(file);
        }

        string[] dirs = Directory.GetDirectories(strPath);
        foreach (string dir in dirs)
        {
            if (dir.Contains(".")) continue;            
            getAnimList(dir, ref theList);
        }
    }

    static bool checkAnimaionName(string name)
    {
        string[] fileName = name.Split('@');
        if (fileName.Length != 2)
            return false;
        string[] items = fileName[1].Split('.');
        if (items.Length != 2)
            return false;
        string item = items[0];
        if(item=="Attack" ||
           item == "Cheer"||
           item == "Death"||
           item == "Hit" || 
           item == "Power" ||
           item == "Run" ||
           item == "Skill1" ||
           item == "Sprint"||
           item == "StandBy" ||
           item == "Jump"
            )
            return true;

        return false;
    }

    static void createAnimations(string strName, ref List<AnimationClip> thsList)
    {
        string strModelPath = "Assets/ArtDate/Hero/" + strName;

        List<string> theList = new List<string>();
        getAnimList(strModelPath,ref theList);

        foreach(string file in theList)
        {
            string duplicatePostfix = "";

            int begin = file.LastIndexOf("\\")+1;
            int end = file.IndexOf(".");
            if (end <= begin)
                continue;
            string fbxName = file.Substring(begin,end-begin);
            fbxName = fbxName.Substring(0, fbxName.IndexOf("@"));
            Object[] objs0 = AssetDatabase.LoadAllAssetsAtPath(file);
            foreach (Object obj0 in objs0)
            {
                if (obj0.GetType() == typeof(AnimationClip))
                {
                    if (obj0.name.IndexOf("001") == -1)
                    {
                        string copyPath = "Assets/ArtDate/Anima/Hero/" + fbxName + "_" + obj0.name + duplicatePostfix + ".anim";
                        //string copyPath = "Assets/Animations/" + fbxName + "_" +obj0.name + duplicatePostfix + ".anim";
                        AnimationClip newClip = new AnimationClip();
                        newClip.name = fbxName + "_" + obj0.name + duplicatePostfix + ".anim";
                        AssetDatabase.CreateAsset(newClip, copyPath);
                        AssetDatabase.Refresh();

                        AnimationClip copy = AssetDatabase.LoadAssetAtPath(copyPath, typeof(AnimationClip)) as AnimationClip;
                        if (copy == null)
                        {
                            Debug.Log("No copy found at " + copyPath);
                            return;
                        }
                        if (copyPath.Contains("Run") || copyPath.Contains("StandBy"))
                            copy.wrapMode = WrapMode.Loop;
                        else
                            copy.wrapMode = WrapMode.Once;

                        

                        //						 Copy curves from imported to copy
                        AnimationClipCurveData[] curveDatas = AnimationUtility.GetAllCurves((AnimationClip)obj0, true);
                        for (int i = 0; i < curveDatas.Length; i++)
                        {
                            AnimationUtility.SetEditorCurve(
                                copy,
                                curveDatas[i].path,
                                curveDatas[i].type,
                                curveDatas[i].propertyName,
                                curveDatas[i].curve
                                );
                        }

                        thsList.Add(copy);
                        Debug.Log(obj0.name);
                    }
                }
            }
        }
    }
}
