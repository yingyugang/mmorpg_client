using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class TestHeroController : MonoBehaviour {

	public List<GameObject> prefabs;
	public List<GameObject> objs;
	public List<GameObject> curObjs;
	public string path = "/_BF/Resources/Hero";
	public List<HeroRes> mSelectheros;
	public List<Vector2> mStartPositions;
	public List<string> animstates;
	public bool isDownload;
	public string[] assetbundles;
    public int rowPerPage = 10;
    public int currentPage = 0;
    public int maxPage;
    public bool Load = false;

    float cameraSize = 20;
    float scrollKeySpeed = 500;
    HeroRes mCurrentHero;
	Texture2D mTex;

    public float mDefaultScreenWidth = 640;
    public float mDefaultScreenHeight = 960;

    float mScreenScaleWidth;
    float mScreenScaleHeight;

    void Awake()
    {
        mScreenScaleWidth = Screen.width / mDefaultScreenWidth;
        mScreenScaleHeight = Screen.height / mDefaultScreenHeight;
       Debug.Log("mScreenScaleWidth:" + mScreenScaleWidth);
    }

	void Start()
	{
		animstates = new List<string>();
		animstates.Add("Attack");
		animstates.Add("Skill1");
		animstates.Add("StandBy");
		animstates.Add("Hit");
		animstates.Add("Death");
		animstates.Add("Run");
		animstates.Add("Cheer");
		animstates.Add("Sprint");
		animstates.Add("Power");
		mTex = CommonUtility.InitTexture2D(10,10,new Color(0,1,0,0.5f));
	}

	Vector2 mStartMousePos;
	Vector2 mCurrentMousePos;
	Vector2 mEndMousePos;
	bool mIsPressed;
	void Update()
	{
		if(Load)
		{
			Load=false;
			LoadPrefabs();
		}
		if(Input.GetMouseButtonDown(0))
		{
			mIsPressed = true;
			mStartMousePos = Input.mousePosition;

			if(mSelectheros.Count>0)
			{
				Collider2D col2D = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				if(col2D==null)
				{
					mSelectheros.Clear();
				}
				else if(col2D!=null)
				{
					HeroRes heroRes = col2D.GetComponent<HeroRes>();
					if(heroRes==null)
					{
						mSelectheros.Clear();
					}else{
						if(!mSelectheros.Contains(heroRes))
						{
							mSelectheros.Clear();
						}else{
							mStartPositions.Clear();
							for(int i= 0; i <mSelectheros.Count;i ++ )
							{
								mStartPositions.Add(mSelectheros[i].transform.position);
							}
						}
					}
				}
			}
		}

		if(Input.GetMouseButton(0))
		{
			mCurrentMousePos = Input.mousePosition;
			if(mSelectheros!=null)
			{
				for(int i=0;i<mSelectheros.Count;i++)
				{
					Vector2 pos0 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					Vector2 pos1 = Camera.main.ScreenToWorldPoint(mStartMousePos);
					mSelectheros[i].transform.position = pos0 - pos1 + mStartPositions[i];
				}
			}
		}

		if(Input.GetMouseButtonUp(0))
		{
			mIsPressed =false;
			mEndMousePos = Input.mousePosition;
			foreach(GameObject obj in objs)
			{
				if(obj.activeInHierarchy)
				{
					Vector2 pos = Camera.main.WorldToScreenPoint(obj.transform.position);
					if(pos.x < Mathf.Max(mStartMousePos.x,mEndMousePos.x) && pos.x > Mathf.Min(mStartMousePos.x,mEndMousePos.x))
					{
						if(pos.y < Mathf.Max(mStartMousePos.y,mEndMousePos.y) && pos.y > Mathf.Min(mStartMousePos.y,mEndMousePos.y))
						{
							if(!mSelectheros.Contains(obj.GetComponent<HeroRes>()))
							{
								mSelectheros.Add(obj.GetComponent<HeroRes>());
								mStartPositions.Add(obj.transform.position);
							}
						}
					}
				}
			}
			Collider2D col2D = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if(col2D!=null)
			{
				HeroRes heroRes = col2D.GetComponent<HeroRes>();
				if(col2D.GetComponent<HeroRes>()!=null)
				{
					mCurrentHero = heroRes;
					if(!mSelectheros.Contains(heroRes))mSelectheros.Add(heroRes);
				}
			}
		}

		if(Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			float theDistance = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollKeySpeed;
			if(Input.GetKey(KeyCode.LeftShift))
			{
				theDistance *= 4;
			}
			cameraSize += theDistance;
			cameraSize = Mathf.Clamp(cameraSize,1,100);
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

    Vector2 mDefaultCameraPos;
    Vector2 mStartMousePos1;
    Vector2 mCurrentMousePos1;
    void LateUpdate()
    {
        #region move the camera
        if(Input.GetMouseButtonDown(1))
        {
            mStartMousePos1 = Input.mousePosition;
            mDefaultCameraPos = Camera.main.transform.position;
        }
        if(Input.GetMouseButton(1))
        {
            mCurrentMousePos1 = Input.mousePosition;
            Vector2 pos = mDefaultCameraPos - (mCurrentMousePos1 - mStartMousePos1)/10;
            Camera.main.transform.position = new Vector3(pos.x,pos.y,-10);
        }
        #endregion

    }

	void OnGUI()
	{
        GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,new Vector3(mScreenScaleWidth,mScreenScaleHeight,1));
       
		int xOffset = 10;
		int yOffset = 10;
		int height = 30;
		int width = 100;

		if(mIsPressed && mSelectheros.Count==0)
		{
			GUI.DrawTexture(new Rect(mStartMousePos.x,Screen.height - mStartMousePos.y,mCurrentMousePos.x - mStartMousePos.x ,mStartMousePos.y - mCurrentMousePos.y ),mTex);
		}
		GUI.Label(new Rect(xOffset + width*6f,yOffset,120,height),"Camera Scale:" + Mathf.Round(cameraSize));
		cameraSize = GUI.HorizontalSlider(new Rect(xOffset + width*7f,yOffset,width * 3,height),cameraSize,1,100);
		Camera.main.orthographicSize = cameraSize;
		for(int i=0;i<curObjs.Count;i++)
		{
			if(GUI.Button(new Rect(xOffset,yOffset,width,height),curObjs[i].name))
			{
				mCurrentHero = curObjs[i].GetComponent<HeroRes>();
				curObjs[i].SetActive(true);
				curObjs[i].transform.position = Vector3.zero;
			}
			if(curObjs[i].activeInHierarchy)
			{
				if(mSelectheros.Contains(curObjs[i].GetComponent<HeroRes>()))
				{
					BoxCollider2D col2d = curObjs[i].GetComponent<BoxCollider2D>();
					if(col2d!=null)
					{
						Vector3 size = col2d.offset;
						Vector2 pos = Camera.main.WorldToScreenPoint(curObjs[i].transform.position + size);
						GUI.DrawTexture(new Rect(pos.x -5 ,Screen.height - pos.y-5,10,10),mTex);
					}
				}
				if(GUI.Button(new Rect(xOffset + width,yOffset,width-50,height),"Hide"))
				{
					curObjs[i].SetActive(false);
					if(mCurrentHero==curObjs[i].GetComponent<HeroRes>())
						mCurrentHero = null;
				}
				float curAnimT = 0;
				HeroRes hr = curObjs[i].GetComponent<HeroRes>();
				if(hr.CurrentAm!=null && hr.CurrentAm.clip!=null)
				{
					curAnimT = hr.CurrentAm.anim[hr.CurrentAm.clip.name].time % hr.CurrentAm.clip.length;
					GUI.HorizontalSlider(new Rect(xOffset + width * 2,yOffset,width*2,height),curAnimT,0,hr.CurrentAm.clip.length);
					GUI.Label(new Rect(xOffset + width * 4,yOffset,width*2,height),hr.CurrentAm.clip.length.ToString());
				}
			}
			yOffset += height;
		}

		if(GUI.Button(new Rect(xOffset,yOffset,width,height),"LoadAll"))
		{
			if(isDownload)
			{
				AssetBundleMgr.SingleTon().currentVersionSub = 0;
//				AssetBundleMgr.SingleTon().DownloadVersion(InitPages);
                InitPages(null,true);
			}
		}
		yOffset +=height;

		if(GUI.Button(new Rect(xOffset,yOffset,width/2,height),"<<"))
		{
			PreviousPage();
		}
		if(GUI.Button(new Rect(xOffset+width/2,yOffset,width/2,height),">>"))
		{
			NextPage();
		}
		yOffset += height;
		yOffset += height;

		if(GUI.Button(new Rect(xOffset,yOffset,width,height),"ShowAll"))
		{
			ShowAll();
		}
		if(curObjs!=null)
		{
			for(int i=0;i<animstates.Count;i++)
			{
                if(GUI.Button(new Rect(xOffset,yOffset+height,width,height),"All " + animstates[i]))
				{
					foreach(GameObject obj in animObjs)
					{
						if(obj.activeInHierarchy)
						{
 							HeroAnimation heroAnimation = obj.GetOrAddComponent<HeroAnimation>();
 							heroAnimation.PlayByFrags(animstates[i]);
 							HeroEffect heroEffect = obj.GetOrAddComponent<HeroEffect>();
 							_AnimType type = CommonUtility.AnimCilpNameStringToEnum(animstates[i]);
 							heroEffect.PlayEffects(type);
						}
					}
				}
				yOffset += height;
			}
		}

		xOffset = 10;
		yOffset = 10;

//		if(mCurrentHero!=null)
//		{
//			int index = 0;
//			foreach(string animName in mCurrentHero.bodyAnimMapping.Keys)
//			{
//				if(GUI.Button(new Rect(xOffset + width,Screen.height-yOffset-height,width,height),mCurrentHero.bodyAnimMapping[animName].clipName))
//				{
////					mCurrentHero.PlayByQueue(mCurrentHero.bodyAnimMapping[animName].clipName);
////					mCurrentHero.Play(mCurrentHero.BodyAnimArray[i].clipName);
//				}
//				yOffset += height;
//			}
//		}
	}

	public void InitPages(Dictionary<_AssetBundleType,Dictionary<string,GameObject>> allCachePrefabs,bool useLocal)
	{
        if (!useLocal)
        {
            prefabs = new List<GameObject>();
            objs = new List<GameObject>();
            Dictionary<string,GameObject> heroPrefabs = allCachePrefabs [_AssetBundleType.Hero];
            foreach (string fbx in heroPrefabs.Keys)
            {
                prefabs.Add(heroPrefabs [fbx]);
                GameObject go = Instantiate(heroPrefabs [fbx]) as GameObject;
                go.SetActive(false);
                objs.Add(go);
            }
        }
		maxPage = objs.Count / rowPerPage + (objs.Count % rowPerPage > 0 ? 1 : 0);
		curObjs.Clear();
		if(objs!=null && objs.Count>0)
			curObjs.AddRange(objs.GetRange(currentPage * rowPerPage,Mathf.Min(rowPerPage,objs.Count-currentPage * rowPerPage)));
	}

	List<GameObject> animObjs = new List<GameObject>();

	void NextPage()
	{
		Debug.Log("NextPage");
		currentPage ++;
		currentPage = Mathf.Clamp(currentPage,0,maxPage-1);
		HideAll();
		curObjs.Clear();
		int count = Mathf.Min(rowPerPage,objs.Count - currentPage * rowPerPage);
		curObjs.AddRange(objs.GetRange(currentPage * rowPerPage,count));
		ShowCurrentPage();
		animObjs = curObjs;
		Debug.Log("curObjs:" + curObjs.Count);
	}

	void PreviousPage()
	{
		Debug.Log("PreviousPage");
		currentPage --;
		currentPage = Mathf.Clamp(currentPage,0,maxPage-1);
		HideAll();
		curObjs.Clear();
		int count = Mathf.Min(rowPerPage,objs.Count - currentPage * rowPerPage);
		curObjs.AddRange(objs.GetRange(currentPage * rowPerPage,count));
		ShowCurrentPage();
		animObjs = curObjs;
		Debug.Log("curObjs:" + curObjs.Count);
	}

	void ShowCurrentPage()
	{
		
        int column = 4;
        float defaultHeroWidth = 2;
        float defaultHeroHeight = 3;
        float heroInterval = 1;
        float xOffset = (defaultHeroWidth * column + heroInterval * (column - 1)) / 2 - defaultHeroWidth / 2;

        int row = curObjs.Count / column + (curObjs.Count % column > 0 ? 1 : 0);
        float yOffset = (defaultHeroHeight * row + heroInterval * (row - 1)) / 2 - defaultHeroHeight / 2;

        Vector2 startPos = new Vector2(-xOffset,yOffset);;
        Vector2 pos;
		for(int i=0;i < curObjs.Count;i ++)
		{
			curObjs[i].SetActive(true);
            pos = startPos + new Vector2((i % column) * (defaultHeroWidth + heroInterval),-i / column * (defaultHeroHeight+heroInterval) );
            curObjs[i].transform.position = pos;
		}
	}

	void HideAll()
	{
		foreach(GameObject obj in objs)
		{
			obj.SetActive(false);
		}
	}

	Vector2 mStartPos = new Vector2(-35,18);
	void ShowAll()
	{
		for(int i=0;i < maxPage ;i++)
		{
			for(int j=0;j < rowPerPage;j++)
			{
				int index = i * rowPerPage + j;
				if(index<objs.Count)
				{
					objs[index].SetActive(true);
					objs[index].transform.position = mStartPos +  new Vector2(6,0) * j + new Vector2(0,-5 * i);
				}
			}
		}
		animObjs = objs;
	}

	void LoadPrefabs()
	{
		#if UNITY_EDITOR
		string[] paths = CommonUtility.GetFileByDirectory(path);
		prefabs.Clear();
		foreach(GameObject obj in objs)
		{
			DestroyImmediate(obj);
		}
		objs.Clear();
        GameObject goParent = new GameObject();
        goParent.name = "_Heros";
		foreach(string str in paths)
		{
			string assetPath = "Assets" + str.Replace(Application.dataPath, "").Replace('\\', '/');
			GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath,typeof(GameObject));
			if(prefab!=null)
			{
				prefabs.Add(prefab);
				GameObject obj = Instantiate(prefab) as GameObject;
                HeroRes heroRes = obj.GetComponent<HeroRes>();
                HeroResEffect heroResEffect = obj.GetComponent<HeroResEffect>();
               
                HeroAnimation heroAnimation = obj.AddComponent<HeroAnimation>();
                HeroEffect heroEffect = obj.AddComponent<HeroEffect>();

                heroAnimation.heroRes = heroRes;
                heroAnimation.heroResEffect = heroResEffect;
                heroEffect.heroResEffect = heroResEffect;

                obj.transform.parent = goParent.transform;
				obj.name = prefab.name;
				objs.Add(obj);
				obj.SetActive(false);
			}
		}
		#endif
	}


}
