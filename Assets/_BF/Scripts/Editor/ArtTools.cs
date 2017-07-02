using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Object = UnityEngine.Object;
using Ps2D;

class ArtTools: EditorWindow 
{
    [MenuItem("Tools/ArtTool")]
    static void AddWindow()
    {
        Rect wr = new Rect(0, 0, 400, 400);
        ArtTools window = (ArtTools)EditorWindow.GetWindow(typeof(ArtTools));
        window.Show();
    }
    static int _triangulationIndex = 1;

    void OnGUI()
    {
        int top = 0;
        int height = 30;
        int index = 0;
        int weight = 150;

        GUILayout.Label("Hero", EditorStyles.boldLabel);
        index++;
        if (GUI.Button(new Rect(5, top + height * index, weight, 20), "Create Sprite Prefab"))
        {
            createSprite();
        }
        index++;

        if (GUI.Button(new Rect(5, top + height * index, weight, 20), "Add  PolygonCollider2D"))
        {
            addPolygonCollider2d();
        }
        index ++;

        GUILayout.Space(62);
        GUIStyle labelNew = EditorStyles.label;
        labelNew.alignment = TextAnchor.LowerLeft;
        labelNew.contentOffset = new Vector2(10, 2);
        GUILayout.Label("Type of Mesh: ", labelNew);
        labelNew.contentOffset = new Vector2(0, 0);
        string[] TriangulationTypes = { "0", "1", "2", "3" };
        _triangulationIndex = EditorGUI.Popup(new Rect(105, top + height * index, 50, 20), _triangulationIndex, TriangulationTypes);
        index++;
        if (GUI.Button(new Rect(5, top + height * index, weight, 20), "Convert Sprite To Mesh"))
        {
            Puppet2D_Editor.ConvertSpriteToMesh(_triangulationIndex);
        }
        index++;

        if (GUI.Button(new Rect(5, top + height * index, weight, 20), "SpriteToUni2DMesh"))
        {
            addSpritetoMesh();
        }
        index++;

        if (GUI.Button(new Rect(5, top + height * index, weight, 20), "Export selections"))
        {
            EditorObjExporter.ExportWholeSelectionToSingle();
        }
        index++;
        if (GUI.Button(new Rect(5, top + height * index, weight, 20), "Create Prefab"))
        {
            CreatePrefab.create();
        }
        index++;
        if (GUI.Button(new Rect(5, top + height * index, weight, 20), "BoneMatching"))
        {
            CreatePrefab.addBoneMatching();
        }
        index++;

        GUILayout.Space(162);
        GUILayout.Label("Scene", EditorStyles.boldLabel);
        GUILayout.Space(20);

        top = 310;
        index = 0;
        height = 28;
        GUILayout.Label("FGround", labelNew);
        GUILayout.Space(10);
        s_FGround = EditorGUI.FloatField(new Rect(80, top + height * index, 70, 20), s_FGround);
        index++;
        GUILayout.Label("Ground", labelNew);
        GUILayout.Space(10);
        s_Ground = EditorGUI.FloatField(new Rect(80, top + height * index, 70, 20), s_Ground);
        index++;
        GUILayout.Label("CGround", labelNew);
        GUILayout.Space(10);
        s_CGround = EditorGUI.FloatField(new Rect(80, top + height * index, 70, 20), s_CGround);
        index++;
        GUILayout.Label("NGround", labelNew);
        GUILayout.Space(10);
        s_NGround = EditorGUI.FloatField(new Rect(80, top + height * index, 70, 20), s_NGround);
        index++;
        GUILayout.Label("BGround", labelNew);
        GUILayout.Space(10);
        s_BGround = EditorGUI.FloatField(new Rect(80, top + height * index, 70, 20), s_BGround);
        index++;
        GUILayout.Label("Sky", labelNew);
        GUILayout.Space(10);
        s_Sky = EditorGUI.FloatField(new Rect(80, top + height * index, 70, 20), s_Sky);
        index++;
        if (GUI.Button(new Rect(5, top + height * index, weight, 20), "Create ScenePrefab"))
        {
            createScene();
        }
        index++;
    }

    static float s_FGround = 1;
    static float s_Ground = 3f;
    static float s_CGround = 2f;
    static float s_NGround = 1;
    static float s_BGround = 0.5f;
    static float s_Sky = 0.25f;

    void addSpritetoMesh()
    {
        GameObject[] selection = Selection.gameObjects;
        foreach (GameObject item in selection)
        {
            item.AddComponent<SpriteToUni2DMesh>();
        }
    }

    void addPolygonCollider2d()
    {
        GameObject[] selection = Selection.gameObjects;
        foreach (GameObject item in selection)
        {
            item.AddComponent<PolygonCollider2D>();
        }
    }

    void createSprite()
    {
        Object[] imported = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        if (imported.Length == 0)
            return;
        foreach (Object obj in imported)
        {
            string strTemp = AssetDatabase.GetAssetPath(obj);

            Layout layout = loadLayout(getPs2DFile(strTemp));
            if (layout != null)
            {
                GameObject root = SpriteCreator.CreateSprites(layout, null);
                Selection.activeGameObject = PrefabUtility.CreatePrefab(strTemp + "/" + root.name + ".prefab", root, ReplacePrefabOptions.ConnectToPrefab);
                EditorGUIUtility.PingObject(Selection.activeGameObject);
            }
        }
    }

    string getPs2DFile(string strPath)
    {
        string[] files = Directory.GetFiles(strPath);
        foreach (string file in files)
        {
            if (file.Contains(".ps2dmap.json"))
                return file;
        }

        string[] dirs = Directory.GetDirectories(strPath);
        foreach (string dir in dirs)
        {
            string file = getPs2DFile(dir);
            if (file != null)
                return file;
        }
        return null;
    }

    Layout loadLayout(string strPath)
    {
        Layout layout = ScriptableObject.CreateInstance<Layout>();
        if (layout == null)
            return null;
        layout.selectedMapAssetPath = strPath;
        layout.layoutDocumentAsset = (TextAsset)AssetDatabase.LoadMainAssetAtPath(layout.selectedMapAssetPath);
        if (layout.layoutDocumentAsset == null)
            return null;
        layout.Load();
        if (layout.document == null)
            return null;
        if (layout.document.allLayers != null)
        {
            layout.document.allLayers[0].photoshopLayerName = layout.GetFriendlyDocumentName();
        }

        // try to auto-detect the texture source
        FormatGuesser guesser = new FormatGuesser();
        guesser.layout = layout;
        switch (guesser.Guess())
        {
            case TextureSource.AssetFolder:
                SpriteAssigner.AssignSpritesFromFolder(layout);
                break;

            case TextureSource.Spritesheet:
                SpriteAssigner.AssignSpritesFromSpritesheet(layout);
                break;
#if PS2D_TK2D
                case TextureSource.Tk2dSpriteCollection:
                    _spriteCollectionData = guesser.spriteCollectionData;
                    SpriteAssigner.AssignSpritesFromTk2dCollection(layout, _spriteCollectionData);
                    break;
#endif
            default:
                break;
        }
        return layout;
    }

    void createScene()
    {
        Object[] imported = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        if (imported.Length == 0)
            return;
        foreach (Object obj in imported)
        {
            string strTemp = AssetDatabase.GetAssetPath(obj);
            Layout layout = loadLayout(getPs2DFile(strTemp));
            if (layout != null)
            {
                layout.anchor = TextAnchor.MiddleCenter;
                layout.preservePhotoshopLayers = true;
                GameObject root = SpriteCreator.CreateSprites(layout, null);
                if(root==null)
                    continue;
                int count = root.transform.childCount;
                for (int index = 0; index < count; index++)
                    dealSceneNode(root.transform.GetChild(index).gameObject);
                SpriteRenderer[] RendererList = root.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer item in RendererList)
                    item.sortingOrder = item.sortingOrder * 5;
 
                Selection.activeGameObject = PrefabUtility.CreatePrefab(strTemp + "/" + root.name + ".prefab", root, ReplacePrefabOptions.ConnectToPrefab);
                EditorGUIUtility.PingObject(Selection.activeGameObject);
            }
        }
    }

    GameObject getChildNodeByName(GameObject root,string strName)
    {
        if (root == null || strName == null)
            return null;

        int count = root.transform.childCount;
        for (int index = 0; index < count; index++)
        {
            GameObject child = root.transform.GetChild(index).gameObject;
            if (child.name.Contains(strName))
                return child;
        }
        return null;
    }

    void dealSceneNode(GameObject root)
    {
        if (root == null)
            return;
        GameObject mainNode = getChildNodeByName(root,"Sky");
        if (mainNode == null)
            mainNode = getChildNodeByName(root, "_Ground");
        if (mainNode == null)
            mainNode = getChildNodeByName(root,"Ground");
        if (mainNode == null)
            mainNode = getChildNodeByName(root,"Effect");
        if (mainNode == null)
            return;

        while (root.transform.childCount > 1)
        {
            for (int index = 0; index < root.transform.childCount; index++)
            {
                GameObject child = root.transform.GetChild(index).gameObject;
                if (child == mainNode)
                    continue;
                else
                {
                    child.transform.parent = mainNode.transform;
                    continue;
                }
            }
        }

        GameObject newRoot = new GameObject();
        newRoot.transform.parent = root.transform;
        mainNode.transform.parent = newRoot.transform;
        newRoot.name = mainNode.name;

        GameObject clone = (GameObject)Object.Instantiate(mainNode);
        clone.name = mainNode.name;
        clone.transform.parent = newRoot.transform;
        clone.transform.position = new Vector3(mainNode.transform.position.x - 31, mainNode.transform.position.y, mainNode.transform.position.z);
        
        ScrollingScript scroll = newRoot.AddComponent<ScrollingScript>();
        scroll.isLooping = true;
        scroll.direction = new Vector2(1, 0);

        float fSpeed = 1;

        if (mainNode.name.Contains("_Sky"))
            fSpeed = s_Sky;
        else if (mainNode.name.Contains("_Ground"))
            fSpeed = s_Ground;
        else if (mainNode.name.Contains("_FGround"))
            fSpeed = s_FGround;
        else if (mainNode.name.Contains("_CGround"))
            fSpeed = s_CGround;
        else if (mainNode.name.Contains("_NGGround"))
            fSpeed = s_NGround;
        else if (mainNode.name.Contains("_BGround"))
            fSpeed = s_BGround;
        scroll.speed = new Vector2(fSpeed, 0);
    }
}