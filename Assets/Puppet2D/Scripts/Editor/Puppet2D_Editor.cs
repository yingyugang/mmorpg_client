using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditorInternal;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;

public class Puppet2D_Editor : EditorWindow 
{

    static bool BoneCreation = false;
	static bool EditSkinWeights = false;
    static bool SplineCreation = false;

    GameObject currentBone;
    GameObject previousBone;

    public bool ReverseNormals ;

	static string _boneSortingLayer,_controlSortingLayer;
	static int _boneSortingIndex,_controlSortingIndex, _triangulationIndex, _numberBonesToSkinToIndex = 1;

	static Sprite boneNoJointSprite =new Sprite();
	static Sprite boneSprite  =new Sprite();
    static Sprite boneHiddenSprite  =new Sprite();
	static Sprite boneOriginal  =new Sprite();

	GameObject currentActiveBone = null;
     
    //static List<GameObject> SkinnedMeshesBeingEditted = new List<GameObject>(); 
	[SerializeField]
	static float BoneSize;
	static float ControlSize;
	static float VertexHandleSize;

	[MenuItem ("GameObject/Puppet2D/Window/Puppet2D")]
	[MenuItem ("Window/Puppet2D")]
    static void Init () 
    {
		Puppet2D_Editor window = (Puppet2D_Editor)EditorWindow.GetWindow (typeof (Puppet2D_Editor));
		window.Show();
    }
	void OnEnable() 
	{
		BoneSize = EditorPrefs.GetFloat("Puppet2D_EditorBoneSize", 0.85f);
		ControlSize = EditorPrefs.GetFloat("Puppet2D_EditorControlSize", 0.85f);
		VertexHandleSize = EditorPrefs.GetFloat("Puppet2D_EditorVertexHandleSize", 0.8f);
        BoneCreation = EditorPrefs.GetBool("Puppet2D_BoneCreation", false);
	}
    void OnGUI () 
    {
        string path = ("Assets/Puppet2D/Textures/GUI/BoneNoJoint.psd");
        string path2 = ("Assets/Puppet2D/Textures/GUI/BoneScaled.psd");
        string path3 = ("Assets/Puppet2D/Textures/GUI/BoneJoint.psd");
		string path4 = ("Assets/Puppet2D/Textures/GUI/Bone.psd");
        boneNoJointSprite =AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
        boneSprite =AssetDatabase.LoadAssetAtPath(path2, typeof(Sprite)) as Sprite;
        boneHiddenSprite =AssetDatabase.LoadAssetAtPath(path3, typeof(Sprite)) as Sprite;
		boneOriginal =AssetDatabase.LoadAssetAtPath(path4, typeof(Sprite)) as Sprite;

        Vector2 scrollPosition = Vector2.zero;
//        GUI.BeginScrollView(new Rect(0, 0, 400, 400), scrollPosition, new Rect(0, 0, 220, 200));  

        GUILayout.Label ("Bone Creation", EditorStyles.boldLabel);
		Texture aTexture = AssetDatabase.LoadAssetAtPath("Assets/Puppet2D/Textures/GUI/GUI_Bones.png",typeof(Texture))as Texture;
		Texture puppetManTexture = AssetDatabase.LoadAssetAtPath("Assets/Puppet2D/Textures/GUI/GUI_puppetman.png",typeof(Texture))as Texture;
		Texture rigTexture = AssetDatabase.LoadAssetAtPath("Assets/Puppet2D/Textures/GUI/GUI_Rig.png",typeof(Texture))as Texture;

		GUILayout.Space(15);
		GUI.DrawTexture(new Rect(0, 20, 64, 128), aTexture, ScaleMode.StretchToFill, true, 10.0F);
		GUILayout.Space(15);

		Color bgColor = GUI.backgroundColor;
		if(EditSkinWeights)
		{
			GUI.backgroundColor = Color.grey;
		}
		if(SplineCreation)
		{
			GUI.backgroundColor = Color.grey;
		}
		if (BoneCreation)
		{
			GUI.backgroundColor=Color.green;

		}

		if (GUI.Button(new Rect(80, 30, 150, 20),"Create Bone Tool" ))
        {  
            /*SpriteRenderer[] sprs = GameObject.FindObjectsOfType<SpriteRenderer>();
            foreach (SpriteRenderer spr in sprs)
            {
				if(spr.sprite)
                	if(spr.sprite.name.Contains("Bone"))
                    	sortOutBoneHierachy(spr.gameObject);
            }
            foreach (SpriteRenderer spr in sprs)
            {
                if(spr.sprite)
					if(spr.sprite.name.Contains("Bone"))
                    	sortOutBoneHierachy(spr.gameObject, true);
            }  */           
			BoneCreation = true;
            EditorPrefs.SetBool("Puppet2D_BoneCreation", BoneCreation);
			
        }
		if(!EditSkinWeights)
		{
			GUI.backgroundColor=bgColor;
		}
		if(SplineCreation)
		{
			GUI.backgroundColor = Color.grey;
		}
        
		if (GUI.Button(new Rect(80, 60, 150, 20),"Finish Bone" ))
        {

            BoneFinishCreation();
        }

		if (BoneCreation)
		{
			GUI.backgroundColor=Color.grey;

		}
		if(EditSkinWeights)
		{
			GUI.backgroundColor = Color.grey;
		}

		BoneSize = EditorGUI.Slider(new Rect(80, 100, 150, 20), BoneSize, 0F, 0.9999F);
		
		string[] sortingLayers = GetSortingLayerNames();

		_boneSortingIndex  = EditorGUI.Popup(new Rect(80, 130, 150, 20),_boneSortingIndex, sortingLayers);

		_boneSortingLayer = sortingLayers[_boneSortingIndex];


		GUILayout.Space(100);
          
        GUILayout.Label ("Rigging Setup", EditorStyles.boldLabel);
		GUI.DrawTexture(new Rect(0, 180, 64, 128), rigTexture, ScaleMode.StretchToFill, true, 10.0F);
		if (GUI.Button(new Rect(80, 180, 150, 20),"Create IK Control" ))
		{
			IKCreateTool();

        }
		if (GUI.Button(new Rect(80, 210, 150, 20),"Create Parent Control" ))
		{
            CreateParentControl();

        }
		if (GUI.Button(new Rect(80, 240, 150, 20),"Create Orient Control" ))
		{        
            CreateOrientControl();

        }

        ControlSize = EditorGUI.Slider(new Rect(80, 280, 150, 20), ControlSize, 0F, .9999F);

        _controlSortingIndex  = EditorGUI.Popup(new Rect(80, 310, 150, 20),_controlSortingIndex, sortingLayers);

        _controlSortingLayer = sortingLayers[_controlSortingIndex];


        GUILayout.Space(160);

        GUILayout.Label ("Skinning", EditorStyles.boldLabel);
        GUI.DrawTexture(new Rect(0, 360, 64, 128), puppetManTexture, ScaleMode.StretchToFill, true, 10.0F);

        //add by hsw
        if (GUI.Button(new Rect(80, 330, 150, 20), "Add  PolygonCollider2D"))
        {
            addPolygonCollider2d();
        }

		GUIStyle labelNew = EditorStyles.label;
		labelNew.alignment = TextAnchor.LowerLeft;
		labelNew.contentOffset = new Vector2(80,0);
		GUILayout.Label ("Type of Mesh: ", labelNew);
		labelNew.contentOffset = new Vector2(0,0);
		string[] TriangulationTypes = {"0", "1","2", "3"} ;
		
		_triangulationIndex  = EditorGUI.Popup(new Rect(180, 360, 50, 20),_triangulationIndex, TriangulationTypes);


        if (GUI.Button(new Rect(80, 380, 150, 20),"Convert Sprite To Mesh" ))
        {
			ConvertSpriteToMesh(_triangulationIndex);
        }
        if (GUI.Button(new Rect(80, 410, 150, 20),"Parent Object To Bones" ))
        {
            BindRigidSkin();

        }
		GUILayout.Space(73);
		labelNew.alignment = TextAnchor.LowerLeft;
		labelNew.contentOffset = new Vector2(80,0);
		GUILayout.Label ("Num Skin Bones: ", labelNew);
		labelNew.contentOffset = new Vector2(0,0);
		string[] NumberBonesToSkinTo = {"1", "2"} ;

		_numberBonesToSkinToIndex  = EditorGUI.Popup(new Rect(180, 450, 50, 20),_numberBonesToSkinToIndex, NumberBonesToSkinTo);

        if (GUI.Button(new Rect(80, 470, 150, 20),"Bind Smooth Skin" ))
        {
            BindSmoothSkin();

        }
        if(EditSkinWeights)
        {
            GUI.backgroundColor = Color.green;
        }
        if (GUI.Button(new Rect(80, 500, 150, 20),"Edit Skin Weights" ))
        {
            EditSkinWeights = EditWeights();

        }
        if(EditSkinWeights)
        {
            GUI.backgroundColor = bgColor;
        }
        if (GUI.Button(new Rect(80, 530, 150, 20),"Finish Edit Skin Weights" ))
        {   
            EditSkinWeights = false;
            FinishEditingWeights(); 

        }

        VertexHandleSize = EditorGUI.Slider(new Rect(80, 570, 150, 20), VertexHandleSize, 0F, .9999F);

		GUILayout.Space(120);
		GUILayout.Label ("Animation", EditorStyles.boldLabel);

		if (GUI.Button(new Rect(80, 620, 150, 20),"Bake Animation" ))
		{   
			Puppet2D_GlobalControl[] globalCtrlScripts = Transform.FindObjectsOfType<Puppet2D_GlobalControl> ();
			for (int i = 0; i < globalCtrlScripts.Length; i++) 
			{
				Puppet2D_BakeAnimation BakeAnim = globalCtrlScripts[i].gameObject.AddComponent<Puppet2D_BakeAnimation> ();
				BakeAnim.Run ();
				DestroyImmediate (BakeAnim);
				globalCtrlScripts[i].enabled = false;
			}
		}

        if (GUI.Button(new Rect(80, 650, 150, 20), "SpriteToUni2DMesh"))
        {
            addSpritetoMesh();
        }

        if (GUI.Button(new Rect(80, 680, 150, 20), "Export selections"))
        {
            EditorObjExporter.ExportWholeSelectionToSingle();
        }

        if (GUI.Button(new Rect(80, 710, 150, 20), "BoneMatching"))
        {
            CreatePrefab.addBoneMatching();
        }

        if (GUI.Button(new Rect(80, 740, 150, 20), "Create Prefab"))
        {
            CreatePrefab.create();
        }
		if(GUI.Button(new Rect(80, 770, 150, 20),"Create Scene"))
		{
			GameObject go = Selection.activeGameObject;
			SpriteRenderer[] srs = go.GetComponentsInChildren<SpriteRenderer>();
			for(int i=0; i < srs.Length; i ++)
			{
//				if(srs[i].gameObject.name.IndexOf("Background")!=-1)
//				{
				Attach(srs[i].gameObject,go);
//				}
			}
		}

        //GUI.EndScrollView();  

        if (GUI.changed)
        {
            ChangeControlSize();
            ChangeBoneSize();
            ChangeVertexHandleSize();

			EditorPrefs.SetFloat("Puppet2D_EditorBoneSize", BoneSize);
			EditorPrefs.SetFloat("Puppet2D_EditorControlSize", ControlSize);
			EditorPrefs.SetFloat("Puppet2D_EditorVertexHandleSize", VertexHandleSize);
        }

    }

	void Attach(GameObject spriteGo,GameObject parentGo)
	{
		GameObject goParent = new GameObject();
		goParent.name = spriteGo.name;
		ScrollingScript scroll = goParent.AddComponent<ScrollingScript>();
		if(spriteGo.name.IndexOf("Background")!=-1)
		{
			scroll.isLooping = true;
		}
		else
		{
			scroll.isLooping = false;
		}
		goParent.transform.parent = parentGo.transform;
		GameObject go = Instantiate(spriteGo,spriteGo.transform.position + new Vector3(-31,0,0),spriteGo.transform.rotation) as GameObject;
		go.name = spriteGo.name;
		spriteGo.transform.parent = goParent.transform;
		Vector3 pos = spriteGo.transform.localPosition;
		scroll.speed = new Vector2(Mathf.Abs(pos.z * 6),0);
		scroll.direction = new Vector2(1,0);
		go.transform.parent = goParent.transform;
	}

    void addSpritetoMesh()
    {
        GameObject[] selection = Selection.gameObjects;
        foreach (GameObject item in selection)
        {
            item.AddComponent<SpriteToUni2DMesh>();
        }
    }

	void OnFocus() {
		
		
		// Remove delegate listener if it has previously
		// been assigned.
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
		
		// Add (or re-add) the delegate.
		SceneView.onSceneGUIDelegate += this.OnSceneGUI;
	}

    //add by hsw
    void addPolygonCollider2d()
    {
        GameObject[] selection = Selection.gameObjects;
        foreach(GameObject item in selection)
        {
            item.AddComponent<PolygonCollider2D>();
        }
    }
	
	void OnDestroy() {
		// When the window is destroyed, remove the delegate
		// so that it will no longer do any drawing.
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;

		EditorPrefs.SetFloat("Puppet2D_EditorBoneSize", BoneSize);
		EditorPrefs.SetFloat("Puppet2D_EditorControlSize", ControlSize);
		EditorPrefs.SetFloat("Puppet2D_EditorVertexHandleSize", VertexHandleSize);
	}
	
	void OnSceneGUI(SceneView sceneView) 
	{
		Event e = Event.current;
        //int controlID = GUIUtility.GetControlID(FocusType.Passive);
        //HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
		
		switch (e.type)
		{
		case EventType.keyDown:
		{
			if (Event.current.keyCode == (KeyCode.Return))
			{
				BoneFinishCreation();
				//SplineFinishCreation();
				
			}
			if (BoneCreation)
			{
				if (Event.current.keyCode == (KeyCode.Backspace))
				{
					BoneDeleteMode();
				}
			}
			break;
		}
		case EventType.mouseMove:
		{
			if (Event.current.button == 0)
			{
				
				if (BoneCreation)
				{
					Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

					if(Event.current.control == true)
					{
						BoneMoveMode(worldRay.GetPoint(0));
					}
					if(Event.current.shift == true)
					{
						BoneMoveIndividualMode(worldRay.GetPoint(0));
					}
					
				}  
				
			}
			break;
		}
		case EventType.MouseDown:
		{
			
			if (Event.current.button == 0)
			{
				
				if (BoneCreation)
				{
					Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                       // HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                        int controlID = GUIUtility.GetControlID(FocusType.Passive);
                        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                    GameObject c = HandleUtility.PickGameObject(Event.current.mousePosition, true);
                    if (c)
                    {
                        Selection.activeGameObject = c;
                    }
                    else
                    {
    					if (Event.current.alt)                        		
    						BoneAddMode(worldRay.GetPoint(0));
    					else
    						BoneCreationMode(worldRay.GetPoint(0));
                    }
                        HandleUtility.AddDefaultControl(controlID);

						
					
				}  
				//                    else if(SplineCreation)
				//                    {
				//                        Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
				//
				//                        SplineCreationMode(worldRay.GetPoint(0));
				//                    }
			}
			/*else if (Event.current.button == 2)
    			{
    				if (BoneCreation)
    				{
    					Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
    					
    					BoneMoveMode(worldRay.GetPoint(0));
    					
    				} 
    			}*/
			else if (Event.current.button == 1)
			{
				if (BoneCreation)
				{                       
					BoneFinishCreation();
					Selection.activeObject = null;
					currentActiveBone = null;
					BoneCreation = true;
                   
				} 
			}
			break;
			
		}
			
		}
		
		// Do your drawing here using Handles.
		
		GameObject[] selection = Selection.gameObjects;
		
		Handles.BeginGUI();
		if(BoneCreation)
		{
			if(selection.Length>0)
			{
				Handles.color = Color.blue;
				Handles.Label(selection[0].transform.position + new Vector3(2,2,0),
				              "Left Click To Draw Bones\nPress Enter To Finish.\nBackspace To Delete A Bone\nHold Shift To Move Individual Bone\nHold Ctrl To Move Bone & Hierachy\nAlt Left Click To Add A Bone In Chain\nRight Click To Deselect");
			}
			else
			{
				Handles.color = Color.blue;
				Handles.Label(SceneView.lastActiveSceneView.camera.transform.position+Vector3.forward*2,
				              "Bone Create Mode.\nLeft Click to Draw Bones.\nOr click on a bone to be a parent");
			}
			
		}
		// Do your drawing here using GUI.
		Handles.EndGUI();   

        //HandleUtility.AddDefaultControl(controlID);
	}
	

    void BoneFinishCreation()
    {
		/*
        SpriteRenderer[] sprs = GameObject.FindObjectsOfType<SpriteRenderer>();
        foreach (SpriteRenderer spr in sprs)
        {
			if(spr.sprite)
				if(spr.sprite.name.Contains("Bone"))
                	sortOutBoneHierachy(spr.gameObject);
        }
        foreach (SpriteRenderer spr in sprs)
        {
            if(spr.sprite)
				if(spr.sprite.name.Contains("Bone"))
               		sortOutBoneHierachy(spr.gameObject, true);
        }  */
        Repaint();
        BoneCreation = false;
        EditorPrefs.SetBool("Puppet2D_BoneCreation", false);


    }
	[MenuItem ("GameObject/Puppet2D/Skeleton/Create Bone Tool")]
    static void CreateBoneTool()
    {    
        BoneCreation = true;
        EditorPrefs.SetBool("Puppet2D_BoneCreation", true);

    }


	static void sortOutBoneHierachy(GameObject changedBone, bool move = false)
	{


        SpriteRenderer spriteRenderer = changedBone.GetComponent<SpriteRenderer>();
		if(spriteRenderer)	
			if(spriteRenderer.sprite)
				if(!spriteRenderer.sprite.name.Contains("Bone"))
					return;

        // UNPARENT CHILDREN
		List<Transform> children = new List<Transform>();

		foreach (Transform child in changedBone.transform)
		{	
            if (child.GetComponent<Puppet2D_HiddenBone>() == null)    						
			    children.Add(child);
		}
        foreach (Transform child in children)
        {
            if (!move)   
                child.transform.parent = null;
        }
        Transform changedBonesParent = null;
        Transform changedBonesParentsParent = null;
        if (changedBone.transform.parent)
        {
			changedBonesParent = changedBone.transform.parent.transform;
            Undo.RecordObject(changedBonesParent, "bone parent");
		            
            if (changedBone.transform.parent.transform.parent)
            {
                changedBonesParentsParent = changedBone.transform.parent.transform.parent.transform;

                changedBone.transform.parent.transform.parent = null;
            }

        }
        if (!move)   
		    changedBone.transform.parent = null;

        List<Transform> parentsChildren = new List<Transform>();

        // ORIENT & SCALE PARENT

        if (changedBonesParent)
        {
            foreach (Transform child in changedBonesParent.transform)
            {
                if (child.GetComponent<Puppet2D_HiddenBone>() == null)
                {              
                    parentsChildren.Add(child);
                }

            }
            foreach (Transform child in parentsChildren)
            {
                Undo.RecordObject(child, "parents child");
                child.transform.parent = null;
            }
			SpriteRenderer sprParent = changedBonesParent.GetComponent<SpriteRenderer>();
			if(sprParent)			
				if(sprParent.sprite)
					if(sprParent.sprite.name.Contains("Bone"))
					{
                        float dist = Vector3.Distance(changedBonesParent.position, changedBone.transform.position);
                        if (dist > 0)
			                changedBonesParent.rotation = Quaternion.LookRotation(changedBone.transform.position - changedBonesParent.position, Vector3.forward) * Quaternion.AngleAxis(90, Vector3.right);
			            float length = (changedBonesParent.position - changedBone.transform.position).magnitude;
                        
			            changedBonesParent.localScale = new Vector3(length, length, length);   
					}


        }
        if (!move)  
            changedBone.transform.localScale = Vector3.one;

        // REPARENT CHILDREN

        if (children.Count > 0)
        {	
            foreach (Transform child in children)
            {
                SpriteRenderer spr = child.GetComponent<SpriteRenderer>();
                if (spr)
                    if (spr.sprite)
                        if (spr.sprite.name.Contains("Bone"))
                        {
                            Undo.RecordObject(child, "parents child");
                            child.transform.parent = changedBone.transform;
                        }
            }
        }
        else
        {
            Undo.RecordObject(spriteRenderer,"sprite change");
            spriteRenderer.sprite = boneNoJointSprite;   
        }         	
    
        if (changedBonesParent)
        {
            changedBone.transform.parent = changedBonesParent;
            if (changedBonesParentsParent)
                changedBone.transform.parent.transform.parent = changedBonesParentsParent;
        		
            foreach (Transform child in parentsChildren)
            {
                Undo.RecordObject(child, "parents child");
                child.transform.parent = changedBonesParent;
            }
			SpriteRenderer spr = changedBonesParent.GetComponent<SpriteRenderer>();
            if (spr)
            {		
                if (spr.sprite)
                {
                    if (spr.sprite.name.Contains("Bone"))
                    {
                        Undo.RecordObject(spr, "sprite change");
                        spr.sprite = boneSprite;
                    }
                }
            }
   
       

        }
        
        // SET CORRECT SPRITE
        if (!move)
        {
            if (children.Count > 0)
                changedBone.GetComponent<SpriteRenderer>().sprite = boneSprite;
            else
                changedBone.GetComponent<SpriteRenderer>().sprite = boneNoJointSprite;

        }

        children.Clear();
        parentsChildren.Clear();

	}
    GameObject BoneCreationMode(Vector3 mousePos)
	{
		if(Selection.activeGameObject == null)
			if(currentActiveBone)
				Selection.activeGameObject = currentActiveBone;
        if (Selection.activeGameObject)
		{
            if (Selection.activeGameObject.GetComponent<Puppet2D_HiddenBone>())
                Selection.activeGameObject = Selection.activeGameObject.transform.parent.gameObject;

			if(Selection.activeGameObject.GetComponent<SpriteRenderer>())
			{
				if(Selection.activeGameObject.GetComponent<SpriteRenderer>().sprite)
				{
					if(Selection.activeGameObject.GetComponent<SpriteRenderer>().sprite.name.Contains("Bone"))
					{
						// MAKE SURE SELECTION IS NOT AN IK OR PARENT
						
						Puppet2D_GlobalControl[] globalCtrlScripts = Transform.FindObjectsOfType<Puppet2D_GlobalControl> ();
						for (int i = 0; i < globalCtrlScripts.Length; i++) 
						{
							foreach(Puppet2D_IKHandle Ik in globalCtrlScripts[i]._Ikhandles)
							{
								if((Ik.topJointTransform == Selection.activeGameObject.transform)||(Ik.bottomJointTransform == Selection.activeGameObject.transform)||(Ik.middleJointTransform == Selection.activeGameObject.transform))
								{
									Debug.LogWarning("Cannot parent bone, as this one has an IK handle");
									Selection.activeGameObject = null;
								}
							}
						}
					}
					else
						return null;
				}
				else
					return null;
			}
			else
				return null;
		}




        GameObject newBone = new GameObject(GetUniqueBoneName("bone"));
        Undo.RegisterCreatedObjectUndo (newBone, "Created newBone");
        newBone.transform.position = mousePos;
        newBone.transform.position = new Vector3(newBone.transform.position.x, newBone.transform.position.y, 0);
       
        if( Selection.activeGameObject)
		{
            newBone.transform.parent = Selection.activeGameObject.transform;

            GameObject newInvisibleBone = new GameObject(GetUniqueBoneName("hiddenBone"));
            Undo.RegisterCreatedObjectUndo(newInvisibleBone, "Created new invisible Bone");

            SpriteRenderer spriteRendererInvisbile = newInvisibleBone.AddComponent<SpriteRenderer>();
            newInvisibleBone.transform.position = new Vector3(10000, 10000, 10000);
            spriteRendererInvisbile.sortingLayerName = _boneSortingLayer;
            spriteRendererInvisbile.sprite = boneHiddenSprite;
            newInvisibleBone.transform.parent = Selection.activeGameObject.transform;
            Puppet2D_HiddenBone hiddenBoneComp = newInvisibleBone.AddComponent<Puppet2D_HiddenBone>();
            hiddenBoneComp.boneToAimAt = newBone.transform;
            hiddenBoneComp.Refresh();
       
		}

        SpriteRenderer spriteRenderer = newBone.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = _boneSortingLayer;

        sortOutBoneHierachy(newBone);

		Selection.activeGameObject = newBone;

		currentActiveBone = newBone;

        return newBone;

    }
    void BoneMoveMode(Vector3 mousePos)
    {
        GameObject selectedGO = Selection.activeGameObject;

        if(selectedGO)
        {         
            if (selectedGO.GetComponent<Puppet2D_HiddenBone>())
            {
                selectedGO = Selection.activeGameObject.transform.parent.gameObject;
                Selection.activeGameObject = selectedGO;
            }

			if(selectedGO.GetComponent<SpriteRenderer>())
			{
				if(selectedGO.GetComponent<SpriteRenderer>().sprite)
				{
					if(selectedGO.GetComponent<SpriteRenderer>().sprite.name.Contains("Bone"))
					{
						// MAKE SURE SELECTION IS NOT AN IK OR PARENT
						
						Puppet2D_GlobalControl[] globalCtrlScripts = Transform.FindObjectsOfType<Puppet2D_GlobalControl> ();
						for (int i = 0; i < globalCtrlScripts.Length; i++) 
						{
							foreach(Puppet2D_IKHandle Ik in globalCtrlScripts[i]._Ikhandles)
							{
								if((Ik.topJointTransform == selectedGO.transform)||(Ik.bottomJointTransform == selectedGO.transform)||(Ik.middleJointTransform == selectedGO.transform))
								{
									Debug.LogWarning("Cannot move bone, as this one has an IK handle");
									return;
								}
							}
						}
					}
					else
						return;
				}
				else
					return;
			}
			else
				return;


            selectedGO.transform.position = mousePos;
            selectedGO.transform.position = new Vector3(Selection.activeGameObject.transform.position.x, Selection.activeGameObject.transform.position.y, 0);
            sortOutBoneHierachy(selectedGO, true);
                            
        }

    }
    void BoneMoveIndividualMode(Vector3 mousePos)
    {
        GameObject selectedGO = Selection.activeGameObject;
        if(selectedGO)
        {
            if (selectedGO.GetComponent<Puppet2D_HiddenBone>())
            {
                selectedGO = Selection.activeGameObject.transform.parent.gameObject;
                Selection.activeGameObject = selectedGO;
            }
			if(selectedGO.GetComponent<SpriteRenderer>())
			{
				if(selectedGO.GetComponent<SpriteRenderer>().sprite)
				{
					if(selectedGO.GetComponent<SpriteRenderer>().sprite.name.Contains("Bone"))
					{
						// MAKE SURE SELECTION IS NOT AN IK OR PARENT
						
						Puppet2D_GlobalControl[] globalCtrlScripts = Transform.FindObjectsOfType<Puppet2D_GlobalControl> ();
						for (int i = 0; i < globalCtrlScripts.Length; i++) 
						{
							foreach(Puppet2D_IKHandle Ik in globalCtrlScripts[i]._Ikhandles)
							{
								if((Ik.topJointTransform == selectedGO.transform)||(Ik.bottomJointTransform == selectedGO.transform)||(Ik.middleJointTransform == selectedGO.transform))
								{
									Debug.LogWarning("Cannot move bone, as this one has an IK handle");
									return;
								}
							}
						}
					}
					else
						return;
				}
				else
					return;
			}
			else
				return;


            List<Transform> children = new List<Transform>();
            foreach (Transform child in selectedGO.transform)
            {
                if (child.GetComponent<Puppet2D_HiddenBone>()==null)
                    children.Add(child);
            }
            foreach (Transform child in children)
                child.parent = null;

            selectedGO.transform.position = mousePos;
            selectedGO.transform.position = new Vector3(Selection.activeGameObject.transform.position.x, Selection.activeGameObject.transform.position.y, 0);
            sortOutBoneHierachy(selectedGO, true);

            foreach (Transform child in children)
            {
                child.parent = selectedGO.transform;
                sortOutBoneHierachy(child.gameObject, true);
            }
            children.Clear();

        }

    }
    void BoneDeleteMode()
    {
        GameObject selectedGO = Selection.activeGameObject;
        if(selectedGO)
        {
            if (selectedGO.GetComponent<Puppet2D_HiddenBone>())
            {
                GameObject hiddenBone = selectedGO;
                selectedGO = selectedGO.transform.parent.gameObject;
                DestroyImmediate(hiddenBone);

                Selection.activeGameObject = selectedGO;

            }
			if(selectedGO.GetComponent<SpriteRenderer>())
			{
				if(selectedGO.GetComponent<SpriteRenderer>().sprite)
				{
					if(selectedGO.GetComponent<SpriteRenderer>().sprite.name.Contains("Bone"))
					{
						// MAKE SURE SELECTION IS NOT AN IK OR PARENT
						
						Puppet2D_GlobalControl[] globalCtrlScripts = Transform.FindObjectsOfType<Puppet2D_GlobalControl> ();
						for (int i = 0; i < globalCtrlScripts.Length; i++) 
						{
							foreach(Puppet2D_IKHandle Ik in globalCtrlScripts[i]._Ikhandles)
							{
								if((Ik.topJointTransform == selectedGO.transform)||(Ik.bottomJointTransform == selectedGO.transform)||(Ik.middleJointTransform == selectedGO.transform))
								{
									Debug.LogWarning("Cannot move bone, as this one has an IK handle");
									return;
								}
							}
						}
					}
					else
						return;
				}
				else
					return;
			}
			else
				return;

            if (selectedGO.transform.parent)
            {
                GameObject parentGO = selectedGO.transform.parent.gameObject;
                DestroyImmediate(selectedGO);
                sortOutBoneHierachy(parentGO);
                Selection.activeGameObject = parentGO;
                foreach (Transform child in parentGO.transform)
                {
                    if(child.GetComponent<Puppet2D_HiddenBone>() == null)
                        sortOutBoneHierachy(child.gameObject, true);
                }
            }
            else
            {
                DestroyImmediate(selectedGO);
            }

        }

    }
    void BoneAddMode(Vector3 mousePos)
    {
        GameObject selectedGO = Selection.activeGameObject;

        if(selectedGO)
        {
            if (selectedGO.GetComponent<Puppet2D_HiddenBone>())
            {                 
                selectedGO = Selection.activeGameObject.transform.parent.gameObject;
                Selection.activeGameObject = selectedGO;
            }
			if(selectedGO.GetComponent<SpriteRenderer>())
			{
				if(selectedGO.GetComponent<SpriteRenderer>().sprite)
				{
					if(selectedGO.GetComponent<SpriteRenderer>().sprite.name.Contains("Bone"))
					{
						// MAKE SURE SELECTION IS NOT AN IK OR PARENT
						
						Puppet2D_GlobalControl[] globalCtrlScripts = Transform.FindObjectsOfType<Puppet2D_GlobalControl> ();
						for (int i = 0; i < globalCtrlScripts.Length; i++) 
						{
							foreach(Puppet2D_IKHandle Ik in globalCtrlScripts[i]._Ikhandles)
							{
								if((Ik.topJointTransform == selectedGO.transform)||(Ik.bottomJointTransform == selectedGO.transform)||(Ik.middleJointTransform == selectedGO.transform))
								{
									Debug.LogWarning("Cannot add bone, as this one has an IK handle");
									return;
								}
							}
						}
					}
					else
						return;
				}
				else
					return;
			}
			else
				return;


            List<Transform> children = new List<Transform>();
            foreach (Transform child in selectedGO.transform)
                children.Add(child);
            foreach (Transform child in children)
                child.parent = null;

            GameObject newBone = BoneCreationMode(mousePos);

            foreach (Transform child in children)
            {

                child.parent = newBone.transform;
                if (child.GetComponent<Puppet2D_HiddenBone>() == null)
                {
                    sortOutBoneHierachy(child.gameObject, true);
                }
                else
                    child.GetComponent<Puppet2D_HiddenBone>().Refresh(); 

            }
            Selection.activeGameObject = newBone;
            children.Clear();

        }

    }
    
	string GetUniqueBoneName (string name)
	{
		string nameToAdd = name;
        int nameToAddLength = nameToAdd.Length +1;
		int index =0;
		foreach(GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
		{
			if(go.name.StartsWith(nameToAdd))
			{
                string endOfName = go.name.Substring(nameToAddLength,go.name.Length-nameToAddLength);
								
				int indexTest = 0;
				if(int.TryParse(endOfName,out indexTest))
				{
					if(int.Parse(endOfName)>index)
					{
						index =int.Parse(endOfName);
					}
				}


			}
		}
		index++;
        return (name+"_"+index);

	}/*
	void BoneMoveMode(Vector3 mousePos)
	{

		if(Selection.activeGameObject)
		{
			Selection.activeGameObject.transform.position = mousePos;
			Selection.activeGameObject.transform.position = new Vector3(Selection.activeGameObject.transform.position.x, Selection.activeGameObject.transform.position.y, 0);
		}
		BoneCreationModeUpdate(new Bone(Selection.activeGameObject));
	}


    void BoneMoveMode(Vector3 mousePos)
    {
		if(currentBone)
		{ 
			currentBone.transform.position = mousePos;
			currentBone.transform.position = new Vector3(currentBone.transform.position.x, currentBone.transform.position.y, 0);
			//currentBone.transform.localScale = new Vector3(1, 1 , 1 );

			if (previousBone != null)
			{
				//previousBone.transform.localScale = new Vector3(1, 1 , 1 );
				previousBone.transform.eulerAngles = new Vector3(0, 0 , 0 );
				previousBone.transform.rotation = Quaternion.LookRotation(currentBone.transform.position - previousBone.transform.position, Vector3.forward) * Quaternion.AngleAxis(90, Vector3.right);
				float length = (previousBone.transform.position - currentBone.transform.position).magnitude;
				Transform parent = previousBone.transform.parent;

				previousBone.transform.parent = null;
				previousBone.transform.localScale = new Vector3(length, length , length );
				if(parent)
					previousBone.transform.parent = parent;
			}
		}
		else
		{
			GameObject selection = Selection.activeObject as GameObject;
			if(selection)
			{
				if(selection.GetComponent<SpriteRenderer>())
				{ 
					if(selection.GetComponent<SpriteRenderer>().sprite.name.Contains("Bone"))
					{ 

						Transform parent = null;
						Transform parentparent = null;
						List<Transform> fellowChildren= new List<Transform>();



						if(selection.transform.parent)
						{
							parent = selection.transform.parent;

                            for(int i = 0; i < parent.childCount; i++)
                            {
                               // Debug.Log(parent.GetChild(i));
                                if (parent.GetChild(i) != selection.transform)
                                {
                                    fellowChildren.Add(parent.GetChild(i));
                                   // Debug.Log("unparenting " + parent.GetChild(i).name);
                                    //parent.GetChild(i).parent = null;
                                   
                                }

                            }
                            foreach(Transform child in fellowChildren)
                                child.parent = null;

							if(parent.parent)
							{
								parentparent = parent.parent;

								parent.parent = null;

							}


						}
						selection.transform.parent = null;
						selection.transform.position = mousePos;
						selection.transform.position = new Vector3(selection.transform.position.x, selection.transform.position.y, 0);
						//currentBone.transform.localScale = new Vector3(1, 1 , 1 );

						if(parent)
						{
							if (parent.GetComponent<SpriteRenderer>())
							{
								if (parent.GetComponent<SpriteRenderer>().sprite.name.Contains("Bone"))
								{
									//previousBone.transform.localScale = new Vector3(1, 1 , 1 );
									parent.eulerAngles = new Vector3(0, 0 , 0 );
									parent.rotation = Quaternion.LookRotation(selection.transform.position - parent.position, Vector3.forward) * Quaternion.AngleAxis(90, Vector3.right);
									float length = (parent.position - selection.transform.position).magnitude;

									parent.localScale = new Vector3(length, length , length );

								}
							}
						}
						selection.transform.parent = parent;
                        if (parentparent)
                        {
                            parent.parent = parentparent;
                        }
                        if (parent)
                        {
                            foreach (Transform child in fellowChildren)
                            {
                                child.parent = parent;
                                
                            }
                        }

                        fellowChildren.Clear();
					}
				}
			}

		}

    }
    */
	[MenuItem ("GameObject/Puppet2D/Rig/Create IK Control")]
    static void IKCreateTool()
    {

        GameObject bone = Selection.activeObject as GameObject;
		if(bone)
		{
			if(bone.GetComponent<SpriteRenderer>())
			{
				if(!bone.GetComponent<SpriteRenderer>().sprite.name.Contains("Bone"))
				{
					Debug.LogWarning("This is not a Puppet2D Bone");
					return;
				}
			}
			else
			{
				Debug.LogWarning("This is not a Puppet2D Bone");
				return;
			}
		}
		else
		{
			Debug.LogWarning("This is not a Puppet2D Bone");
			return;
		}
		GameObject globalCtrl = CreateGlobalControl();
		foreach(Puppet2D_ParentControl parentControl in globalCtrl.GetComponent<Puppet2D_GlobalControl>()._ParentControls)
		{
			if((parentControl.bone.transform == bone.transform)||(parentControl.bone.transform == bone.transform.parent.transform))
			{
				Debug.LogWarning("Can't create a IK Control on Bone; it alreay has an Parent Control");
				return;
			}
		}
		foreach(Puppet2D_IKHandle ikhandle in globalCtrl.GetComponent<Puppet2D_GlobalControl>()._Ikhandles)
		{
			if((ikhandle.bottomJointTransform == bone.transform)||(ikhandle.middleJointTransform == bone.transform)||(ikhandle.topJointTransform == bone.transform))
			{
				Debug.LogWarning("Can't create a IK Control on Bone; it alreay has an IK handle");
				return;
			}
		}

		GameObject IKRoot = null;
		if(bone.transform.parent)
			if(bone.transform.parent.transform.parent)
        		IKRoot = bone.transform.parent.transform.parent.gameObject;
		if(IKRoot==null)
		{
			Debug.LogWarning("You need to select the end of a chain of three bones");
			return;
		}
		// CHECK IF TOP BONE HAS AN IK ATTACHED

		Puppet2D_GlobalControl[] globalCtrls = GameObject.FindObjectsOfType<Puppet2D_GlobalControl>();
		foreach(Puppet2D_GlobalControl glblCtrl in globalCtrls)
		{
			foreach(Puppet2D_IKHandle ik in glblCtrl._Ikhandles)
			{
				if(ik.topJointTransform == bone.transform.parent.transform.parent)
				{
					Debug.LogWarning( bone.transform.parent.transform.parent.name + " already has an IK control");
					return;
				}
			}
		}


		// CHECK TO SEE IF THE BOTTOM BONE IS POINTING AT THE MIDDLE BONE
		if (bone.transform.parent.transform.parent.rotation != Quaternion.LookRotation (bone.transform.parent.transform.position - bone.transform.parent.transform.parent.position, Vector3.forward) * Quaternion.AngleAxis (90, Vector3.right)) 
		{			//if(bone.transform.parent.transform.parent);

			sortOutBoneHierachy(bone.transform.parent.gameObject, true);
		}
        if (bone.transform.parent.rotation != Quaternion.LookRotation (bone.transform.position - bone.transform.parent.position, Vector3.forward) * Quaternion.AngleAxis (90, Vector3.right)) 
        {           //if(bone.transform.parent.transform.parent);

            sortOutBoneHierachy(bone, true);
        }
       /*
        GameObject selectedGO = null;

        if (bone.transform.parent.rotation != Quaternion.LookRotation(bone.transform.position - bone.transform.parent.transform.position, Vector3.forward) * Quaternion.AngleAxis(90, Vector3.right))
        {
            selectedGO = bone.transform.parent.gameObject;

            List<Transform> children = new List<Transform>();
            foreach (Transform child in selectedGO.transform)
            {
                if (child.GetComponent<Puppet2D_HiddenBone>()==null)
                    children.Add(child);
            }
            foreach (Transform child in children)
                child.parent = null;

            sortOutBoneHierachy(selectedGO, true);

            foreach (Transform child in children)
            {
                child.parent = selectedGO.transform;
                sortOutBoneHierachy(child.gameObject, true);
            }
            children.Clear();

        }

        if (bone.transform.parent.transform.parent.rotation != Quaternion.LookRotation(bone.transform.parent.transform.position - bone.transform.parent.transform.parent.position, Vector3.forward) * Quaternion.AngleAxis(90, Vector3.right))
        {
            selectedGO = bone.transform.parent.transform.parent.gameObject;

            List<Transform> children = new List<Transform>();
            foreach (Transform child in selectedGO.transform)
            {
                if (child.GetComponent<Puppet2D_HiddenBone>()==null)
                    children.Add(child);
            }
            foreach (Transform child in children)
                child.parent = null;

            sortOutBoneHierachy(selectedGO, true);

            foreach (Transform child in children)
            {
                child.parent = selectedGO.transform;
                sortOutBoneHierachy(child.gameObject, true);
            }
            children.Clear();

            //sortOutBoneHierachy(bone.transform.parent.gameObject, true);
        }*/

        GameObject control = new GameObject();
		Undo.RegisterCreatedObjectUndo (control, "Created control");
        control.name = (bone.name+"_CTRL");
        GameObject controlGroup = new GameObject();
        controlGroup.name = (bone.name+"_CTRL_GRP");
		Undo.RegisterCreatedObjectUndo (controlGroup, "Created controlgrp");

		control.transform.parent = controlGroup.transform;
        controlGroup.transform.position = bone.transform.position;
        controlGroup.transform.rotation = bone.transform.rotation;

        GameObject poleVector = new GameObject();
		Undo.RegisterCreatedObjectUndo (poleVector, "Created polevector");
        poleVector.name = (bone.name+"_POLE");

        SpriteRenderer spriteRenderer = control.AddComponent<SpriteRenderer>();
        string path = ("Assets/Puppet2D/Textures/GUI/IKControl.psd");
        Sprite sprite =AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingLayerName = _controlSortingLayer;

		// store middle bone position to check if it needs flipping

        Vector3 middleBonePos = bone.transform.parent.transform.position;

        Puppet2D_IKHandle ikHandle = control.AddComponent<Puppet2D_IKHandle>();
        ikHandle.topJointTransform = IKRoot.transform;
		ikHandle.middleJointTransform = bone.transform.parent.transform;
		ikHandle.bottomJointTransform = bone.transform;
        ikHandle.poleVector = poleVector.transform;
        ikHandle.scaleStart[0] = IKRoot.transform.localScale;
        ikHandle.scaleStart[1] = IKRoot.transform.GetChild(0).localScale;
		ikHandle.OffsetScale = bone.transform.localScale;

        if(bone.GetComponent<SpriteRenderer>().sprite.name.Contains("Bone"))
        {
            ikHandle.AimDirection = Vector3.forward;
            ikHandle.UpDirection = Vector3.right;
        }
        else
        {
			Debug.LogWarning("This is not a Puppet2D Bone");
            ikHandle.AimDirection = Vector3.right;
            ikHandle.UpDirection = Vector3.up;
        }


        //if (bone.transform.parent.transform.position.x < IKRoot.transform.position.x)

        Selection.activeObject = ikHandle;

        controlGroup.transform.parent = globalCtrl.transform;
        poleVector.transform.parent = globalCtrl.transform;
        if (globalCtrl.GetComponent<Puppet2D_GlobalControl>().AutoRefresh)
            globalCtrl.GetComponent<Puppet2D_GlobalControl>().Init();
        else
            globalCtrl.GetComponent<Puppet2D_GlobalControl>()._Ikhandles.Add(ikHandle);
        globalCtrl.GetComponent<Puppet2D_GlobalControl>().Run();
        if ((Vector3.Distance(bone.transform.parent.transform.position, middleBonePos) > 0.0001f))
        {
            ikHandle.Flip = true;
        }

    }
	[MenuItem ("GameObject/Puppet2D/Rig/Create Parent Control")]
    static void CreateParentControl()
    {
        GameObject bone = Selection.activeObject as GameObject;
		if(bone)
		{
			if(bone.GetComponent<SpriteRenderer>())
			{
				if(!bone.GetComponent<SpriteRenderer>().sprite.name.Contains("Bone"))
				{
					Debug.LogWarning("This is not a Puppet2D Bone");
					return;
				}
			}
			else
			{
				Debug.LogWarning("This is not a Puppet2D Bone");
				return;
			}
		}
		else
		{
			Debug.LogWarning("This is not a Puppet2D Bone");
			return;
		}
		GameObject globalCtrl = CreateGlobalControl();
		foreach(Puppet2D_IKHandle ikhandle in globalCtrl.GetComponent<Puppet2D_GlobalControl>()._Ikhandles)
		{
			if((ikhandle.bottomJointTransform == bone.transform)||(ikhandle.middleJointTransform == bone.transform))
			{
				Debug.LogWarning("Can't create a parent Control on Bone; it alreay has an IK handle");
				return;
			}
		}
		foreach(Puppet2D_ParentControl parentControl in globalCtrl.GetComponent<Puppet2D_GlobalControl>()._ParentControls)
		{
			if((parentControl.bone.transform == bone.transform))
			{
				Debug.LogWarning("Can't create a Parent Control on Bone; it alreay has an Parent Control");
				return;
			}
		}
        GameObject control = new GameObject();
		Undo.RegisterCreatedObjectUndo (control, "Created control");
        control.name = (bone.name+"_CTRL");
        GameObject controlGroup = new GameObject();
		Undo.RegisterCreatedObjectUndo (controlGroup, "Created controlgrp");
        controlGroup.name = (bone.name+"_CTRL_GRP");
       control.transform.parent = controlGroup.transform;
        controlGroup.transform.position = bone.transform.position;
        controlGroup.transform.rotation = bone.transform.rotation;

        SpriteRenderer spriteRenderer = control.AddComponent<SpriteRenderer>();
        string path = ("Assets/Puppet2D/Textures/GUI/ParentControl.psd");
        Sprite sprite =AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
        spriteRenderer.sprite = sprite;
		spriteRenderer.sortingLayerName = _controlSortingLayer;
        Puppet2D_ParentControl parentConstraint = control.AddComponent<Puppet2D_ParentControl>();
        parentConstraint.IsEnabled = true;
        parentConstraint.Orient = true;
        parentConstraint.Point = true;
        parentConstraint.bone = bone;
		parentConstraint.OffsetScale = bone.transform.localScale;
        Selection.activeObject = control;

        
        controlGroup.transform.parent = globalCtrl.transform;

        if (globalCtrl.GetComponent<Puppet2D_GlobalControl>().AutoRefresh)
            globalCtrl.GetComponent<Puppet2D_GlobalControl>().Init();
        else
            globalCtrl.GetComponent<Puppet2D_GlobalControl>()._ParentControls.Add(parentConstraint);


    }
    static GameObject CreateGlobalControl()
    {
        GameObject globalCtrl = GameObject.Find("Global_CTRL");
		//GameObject globalCtrl = GameObject.FindObjectOfType<Puppet2D_GlobalControl>().gameObject;

        if (globalCtrl)
        {
            return globalCtrl;
        }
        else
        {
            globalCtrl = new GameObject("Global_CTRL");
			Undo.RegisterCreatedObjectUndo (globalCtrl, "Created globalCTRL");

			globalCtrl.AddComponent<Puppet2D_GlobalControl>();
			//Puppet2D_GlobalControl globalCtrlScript = globalCtrl.AddComponent<Puppet2D_GlobalControl>();
			//globalCtrlScript.boneSize = BoneSize;

            return globalCtrl ;
        }

    }
	[MenuItem ("GameObject/Puppet2D/Rig/Create Orient Control")]
    static void CreateOrientControl()
    {
        GameObject bone = Selection.activeObject as GameObject;
		if(bone)
		{
			if(bone.GetComponent<SpriteRenderer>())
			{
				if(!bone.GetComponent<SpriteRenderer>().sprite.name.Contains("Bone"))
				{
					Debug.LogWarning("This is not a Puppet2D Bone");
					return;
				}
			}
			else
			{
				Debug.LogWarning("This is not a Puppet2D Bone");
				return;
			}
		}
		else
		{
			Debug.LogWarning("This is not a Puppet2D Bone");
			return;
		}
		GameObject globalCtrl = CreateGlobalControl();
		foreach(Puppet2D_IKHandle ikhandle in globalCtrl.GetComponent<Puppet2D_GlobalControl>()._Ikhandles)
		{
			if((ikhandle.bottomJointTransform == bone.transform)||(ikhandle.middleJointTransform == bone.transform))
			{
				Debug.LogWarning("Can't create a orient Control on Bone; it alreay has an IK handle");
				return;
			}
		}
		foreach(Puppet2D_ParentControl parentControl in globalCtrl.GetComponent<Puppet2D_GlobalControl>()._ParentControls)
		{
			if((parentControl.bone.transform == bone.transform))
			{
				Debug.LogWarning("Can't create a Parent Control on Bone; it alreay has an Parent Control");
				return;
			}
		}


        GameObject control = new GameObject();
		Undo.RegisterCreatedObjectUndo (control, "Created control");
        control.name = (bone.name+"_CTRL");
        GameObject controlGroup = new GameObject();
		Undo.RegisterCreatedObjectUndo (controlGroup, "Created controlGroup");
        controlGroup.name = (bone.name+"_CTRL_GRP");
        control.transform.parent = controlGroup.transform;
        controlGroup.transform.position = bone.transform.position;
        controlGroup.transform.rotation = bone.transform.rotation;
        SpriteRenderer spriteRenderer = control.AddComponent<SpriteRenderer>();
        string path = ("Assets/Puppet2D/Textures/GUI/OrientControl.psd");
        Sprite sprite =AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
        spriteRenderer.sprite = sprite;
		spriteRenderer.sortingLayerName = _controlSortingLayer;
        Puppet2D_ParentControl parentConstraint = control.AddComponent<Puppet2D_ParentControl>();
        parentConstraint.IsEnabled = true;
        parentConstraint.Orient = true;
        parentConstraint.Point = false;
        //parentConstraint.ConstrianedPosition = true;
        parentConstraint.bone = bone;
        Selection.activeObject = control;
		parentConstraint.OffsetScale = bone.transform.localScale;

        controlGroup.transform.parent = globalCtrl.transform;
    
        if (globalCtrl.GetComponent<Puppet2D_GlobalControl>().AutoRefresh)
            globalCtrl.GetComponent<Puppet2D_GlobalControl>().Init();
        else
            globalCtrl.GetComponent<Puppet2D_GlobalControl>()._ParentControls.Add(parentConstraint);
    }

//    static Dictionary<string, string> s_heroPath = new Dictionary<string, string>();

    //add by hsw
    static string getPrefabPath(GameObject obj)
    {
        if (obj == null)
            return null;
        PrefabType type = PrefabUtility.GetPrefabType(obj);
        if (type == PrefabType.PrefabInstance)
        {
            UnityEngine.Object parent = EditorUtility.GetPrefabParent(obj);
            string path= "";
            if(parent==null)
                path = AssetDatabase.GetAssetPath(obj);
            else
                path = AssetDatabase.GetAssetPath(parent);
            int index = path.LastIndexOf('/');
            path = path.Substring(0,index);
            return path;
        }
        else if(obj.transform.parent!=null)
        {
            return getPrefabPath(obj.transform.parent.gameObject);
        }
        return null;
    }

	[MenuItem ("GameObject/Puppet2D/Skin/ConvertSpriteToMesh")]
    public static void ConvertSpriteToMesh(int triIndex)
    {
        GameObject[] selection = Selection.gameObjects;
        foreach(GameObject spriteGO in selection)
        {
            PrefabType type = UnityEditor.PrefabUtility.GetPrefabType(spriteGO);
            if (type != UnityEditor.PrefabType.PrefabInstance)
            {
                string warn = string.Format("{0} prefabe path lose!",spriteGO.name);
                UnityEngine.Debug.LogWarning(warn);
                continue;
            }
            //string path = null;
            string path = getPrefabPath(spriteGO);
            Debug.Log(path);
			if(spriteGO.GetComponent<SpriteRenderer>())
			{
				string spriteName = spriteGO.GetComponent<SpriteRenderer>().sprite.name;
				if(spriteName.Contains("Bone"))
				{
					Debug.LogWarning("You can't convert Bones to Mesh");
					return;
				}
				if((spriteName=="orientControl")||(spriteName=="parentControl")||(spriteName=="VertexHandleControl")||(spriteName=="IKControl"))
				{
					Debug.LogWarning("You can't convert Controls to Mesh");
					return;
				}
	            PolygonCollider2D polyCol;
	            GameObject MeshedSprite;
	            Quaternion rot = spriteGO.transform.rotation;
	            spriteGO.transform.eulerAngles = Vector3.zero;
				int layer = spriteGO.layer;
				string sortingLayer = spriteGO.GetComponent<Renderer>().sortingLayerName;
				int sortingOrder = spriteGO.GetComponent<Renderer>().sortingOrder;

    	
				if(spriteGO.GetComponent<PolygonCollider2D>()==null)
	            {
	                polyCol = Undo.AddComponent<PolygonCollider2D> (spriteGO);
					//Puppet2D_CreatePolygonFromSprite polyFromSprite = Undo.AddComponent<Puppet2D_CreatePolygonFromSprite> (spriteGO);
					Puppet2D_CreatePolygonFromSprite polyFromSprite = ScriptableObject.CreateInstance("Puppet2D_CreatePolygonFromSprite") as Puppet2D_CreatePolygonFromSprite;
					if(path==null)
                        MeshedSprite = polyFromSprite.Run(spriteGO.transform, true,triIndex);
                    else
                        MeshedSprite = polyFromSprite.Run(spriteGO.transform, true, triIndex,path);

					//polyFromSprite.ReverseNormals = true;
	                //MeshedSprite =polyFromSprite.Run();
	                //MeshedSprite.name = (spriteGO.name+"_GEO");
                    MeshedSprite.name = (spriteGO.name + "_Mesh");
                    //DestroyImmediate(polyFromSprite);
	                //Undo.DestroyObjectImmediate(polyCol);


	                
	            }
	            else
	            {
	                polyCol = spriteGO.GetComponent<PolygonCollider2D>();

					//Puppet2D_CreatePolygonFromSprite polyFromSprite = Undo.AddComponent<Puppet2D_CreatePolygonFromSprite> (spriteGO);
					Puppet2D_CreatePolygonFromSprite polyFromSprite = ScriptableObject.CreateInstance("Puppet2D_CreatePolygonFromSprite") as Puppet2D_CreatePolygonFromSprite;
					if(path==null)
                        MeshedSprite = polyFromSprite.Run(spriteGO.transform, true,triIndex);
                    else
                        MeshedSprite = polyFromSprite.Run(spriteGO.transform, true, triIndex,path);
					//polyFromSprite.ReverseNormals = true;
	                //MeshedSprite = polyFromSprite.Run();
					//MeshedSprite.name = (spriteGO.name+"_GEO");
                    MeshedSprite.name = (spriteGO.name + "_Mesh");

	                //DestroyImmediate(polyFromSprite); 
					//Undo.DestroyObjectImmediate(polyCol);

				}
				MeshedSprite.layer = layer;
				MeshedSprite.GetComponent<Renderer>().sortingLayerName = sortingLayer;
				MeshedSprite.GetComponent<Renderer>().sortingOrder = sortingOrder;
				MeshedSprite.AddComponent<Puppet2D_SortingLayer>();
           
				
				MeshedSprite.transform.position = spriteGO.transform.position;
	            MeshedSprite.transform.rotation = rot;
	            
	            Sprite spriteInfo = spriteGO.GetComponent<SpriteRenderer>().sprite;
	            
	            TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(spriteInfo)) as TextureImporter;
	            
	            //textureImporter.textureType = TextureImporterType.Image;
	            
	            MeshedSprite.GetComponent<Renderer>().sharedMaterial.shader = Shader.Find("Unlit/Transparent");
	            
	            MeshedSprite.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", spriteInfo.texture);
	            
	            textureImporter.textureType = TextureImporterType.Sprite;
	            
	            //DestroyImmediate(spriteGO);
	            
	            Selection.activeGameObject = MeshedSprite;
	            
			}
			else
			{
				Debug.LogWarning("Object is not a sprite");
				return;
			}
        }
    }
	[MenuItem ("GameObject/Puppet2D/Skin/Parent Mesh To Bones")]
    static void BindRigidSkin()
    {
        GameObject[] selection = Selection.gameObjects;
        List<GameObject> selectedBones = new List<GameObject>();
        List<GameObject> selectedMeshes= new List<GameObject>();
        
        //GameObject bone;
        //GameObject control;
        foreach (GameObject Obj in selection)
        {
			if(Obj.GetComponent<SpriteRenderer>())
			{
				if (Obj.GetComponent<SpriteRenderer>().sprite.name.Contains("Bone"))
	            {
                    if (Obj.transform.childCount > 0)
                    {
                        foreach (Transform child in Obj.transform)
                        {
                            if (child.GetComponent<Puppet2D_HiddenBone>())
                            {
                                selectedBones.Add(child.gameObject);
                            }
                        }
                    }
                    else
                        selectedBones.Add(Obj);

				}
				else
				{
					selectedMeshes.Add(Obj);
				}
			}
            else
            {
                selectedMeshes.Add(Obj);
            }
        }

		if((selectedBones.Count == 0)||(selectedMeshes.Count==0))
		{
			Debug.LogWarning("You need to select at least one bone and one other object");
			return;
		}
        foreach (GameObject mesh in selectedMeshes)
        {
            float testdist = 1000000;
            GameObject closestBone =  null;
            foreach (GameObject bone in selectedBones)
            {
                float dist = Vector2.Distance(new Vector2(bone.GetComponent<Renderer>().bounds.center.x,bone.GetComponent<Renderer>().bounds.center.y), new Vector2(mesh.transform.position.x,mesh.transform.position.y));
                if (dist < testdist)
                {
                    testdist = dist;
                    //Debug.Log("closest bone to " + mesh.name + " is " + bone.name + " distance " + dist);
                    if(bone.GetComponent<Puppet2D_HiddenBone>())
                        closestBone = bone.transform.parent.gameObject;
                    else
                        closestBone = bone;

                }
                
            }
            //mesh.transform.parent = closestBone.transform;
			Undo.SetTransformParent (mesh.transform, closestBone.transform, "parent bone");

            /*
            ParentControl parentConstraint = closestBone.AddComponent<ParentControl>();
            parentConstraint.IsEnabled = true;
            parentConstraint.Orient = true;
            parentConstraint.Point = true;
            parentConstraint.MaintainOffset = true;
   
            parentConstraint.OffsetPos = closestBone.transform.InverseTransformPoint(mesh.transform.position);
            parentConstraint.OffsetOrient = closestBone.transform.rotation * mesh.transform.rotation ;
            parentConstraint.bone = mesh;
            */
        }
        
    }
	[MenuItem ("GameObject/Puppet2D/Skin/Bind Smooth Skin")]
    static void BindSmoothSkin()
    {
        GameObject[] selection = Selection.gameObjects;
        List<Transform> selectedBones = new List<Transform>();
        List<GameObject> selectedMeshes= new List<GameObject>();
        foreach (GameObject Obj in selection)
        {
            if (Obj.GetComponent<SpriteRenderer>()== null)
            {
				if ((Obj.GetComponent<MeshRenderer>())||(Obj.GetComponent<SkinnedMeshRenderer>()))
				{
					selectedMeshes.Add(Obj);
                }
                else
                {
                    Debug.LogWarning("Please select a mesh with a MeshRenderer, and some bones");
                    return;
                }
               
            }
			else if (Obj.GetComponent<SpriteRenderer>().sprite.name.Contains("Bone"))
            {
				selectedBones.Add(Obj.transform);
				if (Obj.GetComponent<SpriteRenderer> ().sprite.name.Contains ("BoneScaled"))
					Obj.GetComponent<SpriteRenderer> ().sprite = boneOriginal;

            }
			else
            {
                Debug.LogWarning("Please select a mesh with a MeshRenderer, not a sprite");
                return;
            }
        }
        if (selectedBones.Count == 0)
        {
            if (selectedMeshes.Count > 0)
            {
                if(EditorUtility.DisplayDialog("Detatch Skin?","Do you want to detatch the Skin From the bones?", "Detach", "Do Not Detach")) 
                {
                    //EditWeights();
                    foreach (GameObject mesh in selectedMeshes)
                    {
                        SkinnedMeshRenderer smr = mesh.GetComponent<SkinnedMeshRenderer>();
                        if (smr)
                        {
                            Material mat = smr.sharedMaterial;
                            //DestroyImmediate(mesh.GetComponent<Puppet2D_Bakedmesh>());
                            DestroyImmediate(smr);
                            MeshRenderer mr = mesh.AddComponent<MeshRenderer>();
                            mr.sharedMaterial = mat;
                        }
                    }
                    return;
                }



            }
            //Debug.LogWarning("Please select some bones, along with a mesh");
            return;
        }
        foreach (GameObject mesh in selectedMeshes)
        {
            Material mat = null;
			string sortingLayer = "";
			int sortingOrder = 0;
            if(mesh.GetComponent<MeshRenderer>()!=null)
            {
                mat = mesh.GetComponent<MeshRenderer>().sharedMaterial;

				sortingLayer = mesh.GetComponent<Renderer>().sortingLayerName;
				sortingOrder = mesh.GetComponent<Renderer>().sortingOrder;

                Undo.DestroyObjectImmediate(mesh.GetComponent<MeshRenderer>());
            }

            SkinnedMeshRenderer renderer = mesh.GetComponent<SkinnedMeshRenderer>();
            if(renderer == null)
                renderer = Undo.AddComponent<SkinnedMeshRenderer>(mesh);



			Puppet2D_SortingLayer puppet2D_SortingLayer = mesh.GetComponent<Puppet2D_SortingLayer>();
			if(puppet2D_SortingLayer != null)
				Undo.DestroyObjectImmediate(puppet2D_SortingLayer);


            Mesh sharedMesh = mesh.transform.GetComponent<MeshFilter>().sharedMesh;
            Vector3[] verts = sharedMesh.vertices;


            Matrix4x4[] bindPoses = new Matrix4x4[selectedBones.Count];


            List<Transform> closestBones =  new List<Transform>();
            closestBones.Clear();
            //Debug.Log(verts.Length);
            BoneWeight[] weights = new BoneWeight[verts.Length];
            int index = 0;
			int index2 = 0;

            for (int j = 0; j < weights.Length; j++)
            {
                float testdist = 1000000;
				float testdist2 = 1000000;
                //closestBones.Add(selectedBones[0].transform);
                for (int i = 0; i < selectedBones.Count; i++)
                {

                    Vector3 worldPt = mesh.transform.TransformPoint(verts[j]);

					float dist = Vector2.Distance(new Vector2(selectedBones[i].GetComponent<Renderer>().bounds.center.x,selectedBones[i].GetComponent<Renderer>().bounds.center.y), new Vector2(worldPt.x,worldPt.y));
                    
					if (dist < testdist)
                    {
                        testdist = dist;
                        index = selectedBones.IndexOf(selectedBones[i]);

                    }
                    
                    
                    Transform bone = selectedBones[i];
                    bindPoses[i] = bone.worldToLocalMatrix * mesh.transform.localToWorldMatrix;
                }
				for (int i = 0; i < selectedBones.Count; i++)
				{
					if(!(index==(selectedBones.IndexOf(selectedBones[i]))))
					{
					Vector3 worldPt = mesh.transform.TransformPoint(verts[j]);
					//float dist = Vector2.Distance(new Vector2(selectedBones[i].position.x,selectedBones[i].position.y), new Vector2(worldPt.x,worldPt.y));
					float dist = Vector2.Distance(new Vector2(selectedBones[i].GetComponent<Renderer>().bounds.center.x,selectedBones[i].GetComponent<Renderer>().bounds.center.y), new Vector2(worldPt.x,worldPt.y));

					if (dist < testdist2)
					{
						testdist2 = dist;
						index2 = selectedBones.IndexOf(selectedBones[i]);
						/*if(selectedBones[i].parent)
							if(selectedBones.IndexOf(selectedBones[i].parent.transform) != index)
								if(selectedBones[i].parent.GetComponent<SpriteRenderer>())
									if(selectedBones[i].parent.GetComponent<SpriteRenderer>().sprite.name.Contains("Bone"))
									{
										index2 = selectedBones.IndexOf(selectedBones[i].parent.transform);
										testdist2 = Vector2.Distance(new Vector2(selectedBones[i].parent.transform.renderer.bounds.center.x,selectedBones[i].parent.transform.renderer.bounds.center.y), new Vector2(worldPt.x,worldPt.y));

									}
						*/


					}
					}

				}

				float combinedDistance = testdist+testdist2;
				float weight1 = (testdist/combinedDistance);
				float weight2 =  (testdist2/combinedDistance);
				weight1 = Mathf.Lerp(1, 0, weight1);
				weight2 = Mathf.Lerp(1, 0, weight2);

				weight1= Mathf.Clamp01((weight1+0.5f)*(weight1+0.5f)*(weight1+0.5f) - 0.5f);
				weight2= Mathf.Clamp01((weight2+0.5f)*(weight2+0.5f)*(weight2+0.5f) - 0.5f);

				if (_numberBonesToSkinToIndex == 1)
				{
					weights [j].boneIndex0 = index;
					weights [j].weight0 = weight1;
					weights [j].boneIndex1 = index2;
					weights [j].weight1 = weight2;
				} 
				else 
				{
					weights [j].boneIndex0 = index;
					weights [j].weight0 = 1;
				}

                //Debug.Log("Skinning " + j + " closest bone is " + selectedBones[index].name + " index is " + index);
            }

            sharedMesh.boneWeights = weights;

            sharedMesh.bindposes = bindPoses;

            renderer.bones = selectedBones.ToArray();

            //sharedMesh = SmoothSkinWeights(sharedMesh);
           	renderer.sharedMesh = sharedMesh;
            if(mat)
            renderer.sharedMaterial = mat;

			renderer.sortingLayerName = sortingLayer;
			renderer.sortingOrder = sortingOrder;
			mesh.AddComponent<Puppet2D_SortingLayer>();


           
        }
		foreach (Transform bone in selectedBones) 
		{
			if (bone.GetComponent<SpriteRenderer> ().sprite.name=="Bone")
				bone.GetComponent<SpriteRenderer> ().sprite = boneSprite;
		}

    }
	[MenuItem ("GameObject/Puppet2D/Skin/Edit Skin Weights")]
    static bool EditWeights()
    {
        GameObject[] selection = Selection.gameObjects;

        foreach(GameObject sel in selection)
        {
            if ((sel.GetComponent<Puppet2D_Bakedmesh>() != null))
            {
                Debug.LogWarning("Already in edit mode");
                return false;
            }
            if ((sel.GetComponent<SkinnedMeshRenderer>()))
            {
                SkinnedMeshRenderer renderer = sel.GetComponent<SkinnedMeshRenderer>();
                Undo.RecordObject(sel, "add mesh to meshes being editted");
                Undo.AddComponent<Puppet2D_Bakedmesh>(sel);
                Mesh mesh = sel.GetComponent<MeshFilter>().sharedMesh;
	            

                Vector3[] verts = mesh.vertices;
                BoneWeight[] boneWeights = mesh.boneWeights;

                for (int i = 0; i < verts.Length; i++)
                {
                    Vector3 vert = verts[i];
                    Vector3 vertPos = sel.transform.TransformPoint(vert);
                    GameObject handle = new GameObject("vertex" + i);
                    Undo.RegisterCreatedObjectUndo (handle, "vertex created");
                    handle.transform.position = vertPos;
                    //handle.transform.parent = sel.transform;
                    Undo.SetTransformParent(handle.transform, sel.transform, "parent handle");
                    //handle.tag = "handle";

                    SpriteRenderer spriteRenderer = Undo.AddComponent<SpriteRenderer>(handle);
                    string path = ("Assets/Puppet2D/Textures/GUI/VertexHandle.psd");
                    Sprite sprite = AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
                    spriteRenderer.sprite = sprite;
                    spriteRenderer.sortingLayerName = _controlSortingLayer;
                    Puppet2D_EditSkinWeights editSkinWeights = Undo.AddComponent<Puppet2D_EditSkinWeights>(handle);

                    editSkinWeights.verts = mesh.vertices;

                    editSkinWeights.Weight0 = boneWeights[i].weight0;
                    editSkinWeights.Weight1 = boneWeights[i].weight1;
                    editSkinWeights.Weight2 = boneWeights[i].weight2;
                    editSkinWeights.Weight3 = boneWeights[i].weight3;

                    if (boneWeights[i].weight0 > 0)
                    {
                        editSkinWeights.Bone0 = renderer.bones[boneWeights[i].boneIndex0].gameObject;
                        editSkinWeights.boneIndex0 = boneWeights[i].boneIndex0;
                    }
                    else
                        editSkinWeights.Bone0 = null;

                    if (boneWeights[i].weight1 > 0)
                    {
                        editSkinWeights.Bone1 = renderer.bones[boneWeights[i].boneIndex1].gameObject;
                        editSkinWeights.boneIndex1 = boneWeights[i].boneIndex1;
                    }
                    else
                    {
                        editSkinWeights.Bone1 = null;
                        editSkinWeights.boneIndex1 = renderer.bones.Length;
                    }

                    if (boneWeights[i].weight2 > 0)
                    {
                        editSkinWeights.Bone2 = renderer.bones[boneWeights[i].boneIndex2].gameObject;
                        editSkinWeights.boneIndex2 = boneWeights[i].boneIndex2;
                    }
                    else
                    {
                        editSkinWeights.Bone2 = null;
                        editSkinWeights.boneIndex2 = renderer.bones.Length;
                    }

                    if (boneWeights[i].weight3 > 0)
                    {
                        editSkinWeights.Bone3 = renderer.bones[boneWeights[i].boneIndex3].gameObject;
                        editSkinWeights.boneIndex3 = boneWeights[i].boneIndex3;
                    }
                    else
                    {
                        editSkinWeights.Bone3 = null;
                        editSkinWeights.boneIndex3 = renderer.bones.Length;
                    }

                    editSkinWeights.mesh = mesh;
                    editSkinWeights.meshRenderer = renderer;
                    editSkinWeights.vertNumber = i;
                }

            }
            else
            {
                Debug.LogWarning("Selection does not have a meshRenderer");
                return false;
            }


        }
		return true;
    }

	[MenuItem ("GameObject/Puppet2D/Skin/Finish Editting Skin Weights")]
    static void FinishEditingWeights()
    {
		SpriteRenderer[] sprs = FindObjectsOfType<SpriteRenderer>();
		Puppet2D_Bakedmesh[] skinnedMeshesBeingEditted = FindObjectsOfType<Puppet2D_Bakedmesh>();
		foreach(SpriteRenderer spr in sprs)
		{
			if(spr.sprite)		
				if(spr.sprite.name.Contains("Bone"))			
					spr.gameObject.GetComponent<SpriteRenderer>().color = Color.white;					

		}
		foreach(Puppet2D_Bakedmesh bakedMesh in skinnedMeshesBeingEditted)
        {
			GameObject sel = bakedMesh.gameObject;

			DestroyImmediate(bakedMesh);

            int numberChildren = sel.transform.childCount;
            List<GameObject> vertsToDestroy = new List<GameObject>();
            for(int i = 0;i< numberChildren;i++)
            {
                vertsToDestroy.Add(sel.transform.GetChild(i).gameObject);


            }
            foreach(GameObject vert in vertsToDestroy)
                DestroyImmediate(vert);
        }
    }

    static Mesh SmoothSkinWeights(Mesh sharedMesh)
    {
        Debug.Log("smoothing weights");
        int[] triangles = sharedMesh.GetTriangles(0);
        //Vector3[] verts = sharedMesh.vertices;
        BoneWeight[] boneWeights = sharedMesh.boneWeights;

        for(int i =0;i<triangles.Length;i+=3)
        {
            BoneWeight v1 = boneWeights[triangles[i]];
            BoneWeight v2 = boneWeights[triangles[i+1]];
            BoneWeight v3 = boneWeights[triangles[i+2]];

            List<int> v1Bones = new List<int>(new int[] {v1.boneIndex0,v1.boneIndex1,v1.boneIndex2,v1.boneIndex3 });
            List<int> v2Bones = new List<int>(new int[]  {v2.boneIndex0,v2.boneIndex1,v2.boneIndex2,v2.boneIndex3 });
            List<int> v3Bones = new List<int>(new int[]  {v3.boneIndex0,v3.boneIndex1,v3.boneIndex2,v3.boneIndex3 });

            List<float> v1Weights = new List<float>(new float[] {v1.weight0,v1.weight1,v1.weight2,v1.weight3 });
            List<float> v2Weights = new List<float>(new float[]  {v2.weight0,v2.weight1,v2.weight2,v2.weight3 });
            List<float> v3Weights = new List<float>(new float[]  {v3.weight0,v3.weight1,v3.weight2,v3.weight3 });

            //List<int> v1v2Bones = v1Bones.Intersect(v2Bones).ToList();
            /*for (int j = 0; j < 4; j++)
            {
                if (!v2Bones.Contains(v1Bones[j]))
                {
                    for (int k = 0; k < 4; k++)
                    {
                        //if(v2Bones[k] == null)
                       // {
                            v2Bones[1] =v1Bones[j];
                            v2Weights[1] =0;
                        //    break;
                       // }
                    }

                }
                if (!v3Bones.Contains(v1Bones[j]))
                {
                    for (int k = 0; k < 4; k++)
                    {
                        //if(v3Bones[k] == null)
                        //{
                            v3Bones[1] =v1Bones[j];
                            v3Weights[1] =0;
                        //    break;
                       // }
                    }
                    
                }
                if (!v3Bones.Contains(v2Bones[j]))
                {
                    for (int k = 0; k < 4; k++)
                    {
                        //if(v3Bones[k] == null)
                        //{
                            v3Bones[1] =v2Bones[j];
                            v3Weights[1] =0;
                         //   break;
                       // }
                    }                    
                }
                                    
                if (!v1Bones.Contains(v2Bones[j]))
                {
                    for (int k = 0; k < 4; k++)
                    {
                        //if(v1Bones[k] == null)
                        //{
                            v1Bones[1] =v2Bones[j];
                            v1Weights[1] =0;
                        //    break;
                        //}
                    }
                    
                }
                if (!v1Bones.Contains(v3Bones[j]))
                {
                    for (int k = 0; k < 4; k++)
                    {
                        //if(v1Bones[k] == null)
                        //{
                            v1Bones[1] =v3Bones[j];
                            v1Weights[1] =0;
                        //    break;
                       // }
                    }
                    
                }
                if (!v2Bones.Contains(v3Bones[j]))
                {
                    for (int k = 0; k < 4; k++)
                    {
                        //if(v2Bones[k] == null)
                        //{
                            v2Bones[1] =v3Bones[j];
                            v2Weights[1] =0;
                         //   break;
                        //}
                    }                    
                }
                
            }*/
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    if (v1Bones[j] == v2Bones[k])
                    {
                        for (int l = 0; l < 2; l++)
                        {
                            if (v1Bones[j] == v3Bones[l])
                            {

                                v1Weights[j] =(v1Weights[j]+v2Weights[k]+v3Weights[l])/3;
                                v2Weights[k] = (v1Weights[j]+v2Weights[k]+v3Weights[l])/3;
                                v3Weights[l] = (v1Weights[j]+v2Weights[k]+v3Weights[l])/3;


                            }
                        }
                    }
                }/*
                for (int k = 0; k < 2; k++)
                {
                    if (v1Bones[j] == v3Bones[k])
                    {
                        v1Weights[j] =(v1Weights[j]+v3Weights[k])/2;
                        v3Weights[k] = (v1Weights[j]+v3Weights[k])/2;
                    }

                }
                for (int k = 0; k < 2; k++)
                {
                    if (v2Bones[j] == v3Bones[k])
                    {                       
                        v2Weights[j] =(v2Weights[j]+v3Weights[k])/2;
                        v3Weights[k] = (v2Weights[j]+v3Weights[k])/2;
                    }
                    
                }*/

            }
            boneWeights[triangles[i]].weight0 = v1Weights[0];
            boneWeights[triangles[i]].weight1 = v1Weights[1];
            //boneWeights[triangles[i]].weight2 = v1Weights[2];
            //boneWeights[triangles[i]].weight3 = v1Weights[3];

            boneWeights[triangles[i+1]].weight0 = v2Weights[0];
            boneWeights[triangles[i+1]].weight1 = v2Weights[1];
            //boneWeights[triangles[i+1]].weight2 = v2Weights[2];
            //boneWeights[triangles[i+1]].weight3 = v2Weights[3];

            boneWeights[triangles[i+2]].weight0 = v3Weights[0];
            boneWeights[triangles[i+2]].weight1 = v3Weights[1];
            //boneWeights[triangles[i+2]].weight2 = v3Weights[2];
            //boneWeights[triangles[i+2]].weight3 = v3Weights[3];

        }
        sharedMesh.boneWeights = boneWeights;
        return sharedMesh;
    }

	void ChangeBoneSize ()
	{
		string path = ("Assets/Puppet2D/Textures/GUI/BoneNoJoint.psd");
		Sprite sprite =AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
		TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(sprite)) as TextureImporter;
		textureImporter.spritePixelsToUnits = (1-BoneSize)*(1-BoneSize)*1000f;
		AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

		/*Puppet2D_GlobalControl[] globalCtrlScripts = GameObject.FindObjectsOfType<Puppet2D_GlobalControl>();
		foreach(Puppet2D_GlobalControl globalCtrlScript in globalCtrlScripts)
			globalCtrlScript.boneSize = BoneSize;*/
	}

	void ChangeControlSize ()
	{
		string path = ("Assets/Puppet2D/Textures/GUI/IKControl.psd");
		Sprite sprite =AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
		TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(sprite)) as TextureImporter;
		textureImporter.spritePixelsToUnits = (1-ControlSize)*(1-ControlSize)*1000f;
		AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

		path = ("Assets/Puppet2D/Textures/GUI/orientControl.psd");
		sprite =AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
		textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(sprite)) as TextureImporter;
		textureImporter.spritePixelsToUnits = (1-ControlSize)*(1-ControlSize)*1000f;
		AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

		path = ("Assets/Puppet2D/Textures/GUI/parentControl.psd");
		sprite =AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
		textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(sprite)) as TextureImporter;
		textureImporter.spritePixelsToUnits = (1-ControlSize)*(1-ControlSize)*1000f;
		AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

	}

	void ChangeVertexHandleSize ()
	{
		string path = ("Assets/Puppet2D/Textures/GUI/VertexHandle.psd");
		Sprite sprite =AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
		TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(sprite)) as TextureImporter;
		textureImporter.spritePixelsToUnits = (1-VertexHandleSize)*(1-VertexHandleSize)*1000f;
		AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
	}
   
        
    static public void AddNewSortingName() 
    {
        object newName= new object();

        var internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        //string[] stuff = (string[])sortingLayersProperty.GetValue(null, new object[0]);
        string[] stuff = (string[])sortingLayersProperty.GetValue(null, new object[0]);

        sortingLayersProperty.SetValue(null, newName,new object[stuff.Length]);
    }

    public string[] GetSortingLayerNames() 
    {
        var internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        //string[] stuff = (string[])sortingLayersProperty.GetValue(null, new object[0]);

        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }
}
