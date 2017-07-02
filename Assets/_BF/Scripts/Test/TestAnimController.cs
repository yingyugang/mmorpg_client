using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class TestAnimController : MonoBehaviour {

	public List<GameObject> prefabs;
	public List<GameObject> objs;
	public string path = "/ArtDate/Prefeb/Hero";
	public string url="D:/aniRecord.csv";
	public string fileName="aniRecord";

//	public int maxPage;
//	public int currentPage;
	public Transform controllObj;
	public GameObject currentObj;
	float height;
	float width;
	public float currentScale = 1;
	public float scaleStep = 0.1f;
	Vector3 defaulScale;
	public bool Load;
	AnimationState currentState;
	public List<AnimQueue> queue;
	public AudioClip clip;

	void Start () {
		height = Camera.main.orthographicSize;
		width = height * Screen.width/Screen.height;

	}
	void loadQuene(string obj,string ani)
	{
		//使用流的形式读取
		StreamReader sr =null;
		string strPath = "D:/"  + fileName + ".csv";
		try{
			sr = File.OpenText(strPath);
		}
		catch(Exception e)	
		{
			Debug.Log(e.Message);
		}
		string line;
		//ArrayList arrlist = new ArrayList();
		while ((line = sr.ReadLine()) != null)
		{
			string[] p;
			p=line.Split(',');
			if(p[0].Equals(obj) && p[1].Equals(ani))
			{
				AnimQueue aq = new AnimQueue();
				aq.t =float.Parse(p[2]);
				aq.speed = float.Parse(p[3]);
				aq.showText=bool.Parse(p[4]);
				queue.Add(aq);
			}			
		}
		//关闭流
		sr.Close();
		//销毁流
		sr.Dispose();
		//将数组链表容器返回
		//return arrlist;
	}
	ArrayList getDatas(string obj,string ani,string strPath)
	{
		StreamReader sr =null;
		sr = File.OpenText(strPath);
		string line;
		ArrayList arrlist = new ArrayList();
		while ((line = sr.ReadLine()) != null)
		{
			string[] p;
			p=line.Split(',');
			if(p[0].Equals(obj) && p[1].Equals(ani))
				continue;
			arrlist.Add(line);
		}
		sr.Close();
		sr.Dispose();
		File.Delete (strPath);
		return arrlist;
	}
	void Save(string obj,string ani)
	{
		StreamWriter sw;
		string strPath = "D:/"  + fileName + ".csv";
		FileInfo t = new FileInfo(strPath);
		if(!t.Exists)
		{
			//如果此文件不存在则创建
			sw = t.CreateText();
			sw.WriteLine ("Object,Animation,Time,Speed,ShowText,");
		}
		else
		{
			ArrayList arrlist=getDatas(obj,ani,strPath);
			sw = t.CreateText();
			foreach(string str in arrlist)
			{
				sw.WriteLine(str);
			}
			//如果此文件存在则打开
			//sw = t.AppendText();
			
		}
		
		for (int z = 0; z < queue.Count; z++)
		{
			string info=obj+","+ani+","+ queue[z].t+","+queue[z].speed+","+queue[z].showText+",";
			sw.WriteLine (info);
		}
		//关闭流
		sw.Close();
		//销毁流
		sw.Dispose();
	}

	public float moveSpeed = 1;
	void Update () {
		if(currentObj!=null)
		{
			float x = Input.GetAxis("Vertical") * moveSpeed;
			float y = Input.GetAxis("Horizontal") * moveSpeed;
			controllObj.transform.position += new Vector3(y,x,0);
			Vector3 pos = controllObj.transform.position;
			controllObj.transform.position = new Vector3(Mathf.Clamp(pos.x,-width,width),Mathf.Clamp(pos.y,-2 * height,height),0);
		}
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
#if UNITY_EDITOR
		if(Load)
		{
			Load = false;
			LoadPrefabs();
		}
#endif
	}

//	float animSpeed = 1.0f;
	float animPoint = 0.0f;
	float tmpSpeed = 1.0f;
//	string speedStr = "0";
	void OnGUI()
	{
#if UNITY_EDITOR
		if(GUI.Button(new Rect(Screen.width - 130,10,100,30),"加载所有模型"))
		{
			LoadPrefabs();
		}
#endif
		if(currentObj!=null)
		{
			GUI.Label(new Rect(Screen.width-190,50,55,30),"模型缩放");
			currentScale = GUI.HorizontalSlider(new Rect(Screen.width - 130,50,100,20),currentScale,0.1f,3.0f);
			controllObj.transform.localScale = Vector3.one * currentScale;
			currentScale = float.Parse(GUI.TextField(new Rect(Screen.width - 130,70,100,20),currentScale.ToString()));

			currentScale = Mathf.Clamp(currentScale,0.0001f,3.0f);
			GUI.Label(new Rect(Screen.width-190,100,55,30),"移动速度");

//			float speed = float.Parse(speedStr);
			moveSpeed = GUI.HorizontalSlider(new Rect(Screen.width - 130,100,100,20),moveSpeed,0.01f,3.0f);
//			string speedStr = moveSpeed - Mathf.FloorToInt(moveSpeed) > 0 ? moveSpeed.ToString() :  (moveSpeed.ToString() + ".");
			moveSpeed = float.Parse(GUI.TextField(new Rect(Screen.width - 130,130,100,20),moveSpeed.ToString()));
//			speedStr = GUI.TextField(new Rect(Screen.width - 130,130,100,20),speedStr);
			moveSpeed = Mathf.Clamp(moveSpeed,0,3.0f);
			if(currentObj.GetComponent<Animation>()!=null)
			{
				int num = 0;
				foreach(AnimationState state in currentObj.GetComponent<Animation>())
				{
					if(GUI.Button(new Rect(10,Screen.height-40*(1 +num),150,30),state.name))
					{
//						animSpeed = 1;
						currentState = state;
						queue = new List<AnimQueue>();
						loadQuene(currentObj.name,currentState.name);
					}
					num ++;
				}
				if(currentState!=null)
				{
					if(GUI.Button(new Rect(10,Screen.height-40*(1 +num),50,30),"播放"))
					{
						StartCoroutine(_Play());

					}
					if(GUI.Button(new Rect(100,Screen.height-40*(1 +num),50,30),"停止"))
					{
						currentObj.GetComponent<Animation>().Stop();
					}
					num ++;
					/*
					GUI.Label(new Rect(10,Screen.height-40*(1+num)-30,60,30),"动画速度");
					animSpeed = GUI.HorizontalSlider(new Rect(10,Screen.height-40*(1+num),150,30),animSpeed,0.01f,3.0f);
					*/
					num ++;
					int num1 = 0;
					GUI.Label(new Rect(Screen.width-320,170 + num1 * 30,60,30),"动画时间");

					float tmpAnimPoint = animPoint;
					animPoint = float.Parse(GUI.TextField(new Rect(Screen.width-250,170 + num1 * 30,100,30),animPoint.ToString()));
					if(tmpAnimPoint!=animPoint)
					{
						currentState.time = animPoint;
						currentObj.GetComponent<Animation>().Play(currentState.name);
						currentState.speed = 0;
						currentState.time = animPoint;
						currentObj.GetComponent<Animation>().Sample();
//						StartCoroutine(_Sample());
					}
//					GUI.Label(new Rect(Screen.width-250,170 + num1 * 30,100,30),animPoint.ToString());
					num1 ++;
					tmpAnimPoint = animPoint;
					animPoint = GUI.HorizontalSlider(new Rect(Screen.width-320,170 + num1 * 30,300,30),animPoint,0,currentState.length);
					if(tmpAnimPoint!=animPoint)
					{
						currentState.time = animPoint;
						currentObj.GetComponent<Animation>().Play(currentState.name);
						currentState.speed = 0;
						currentState.time = animPoint;
						currentObj.GetComponent<Animation>().Sample();
					}

					num1 ++;

					tmpSpeed = float.Parse(GUI.TextField(new Rect(Screen.width-250,170 + num1 * 30,50,30),tmpSpeed.ToString()));
					tmpSpeed = Mathf.Max(0.0001f,tmpSpeed);
					if(GUI.Button(new Rect(Screen.width-70,170 + num1 * 30,50,30),"添加"))
					{
						AnimQueue aq = new AnimQueue();
						aq.t = animPoint;
						aq.speed = tmpSpeed;
						if(queue.Count==0)
						{
							queue.Add(aq);
						}
						else
						{
							bool added = false;
							for(int z = 0;z < queue.Count; z++)
							{
								if(aq.t<queue[z].t)
								{
									queue.Insert(z,aq);
									added = true;
									break;
								}
							}
							if(!added)queue.Add(aq);
						}
					}
					if(GUI.Button(new Rect(Screen.width-140,170 + num1 * 30,50,30),"效果"))
					{
						currentState.time = animPoint;
						currentObj.GetComponent<Animation>().Play(currentState.name);
						currentState.speed = 0;
						currentState.time = animPoint;
						currentObj.GetComponent<Animation>().Sample();
					}

					num1 ++;
					for(int z = 0;z < queue.Count;z++)
					{
						GUI.Label(new Rect(Screen.width-320,170 + (z+num1) * 40,100,30),queue[z].t.ToString());
						GUI.Label(new Rect(Screen.width-200,170 + (z+num1) * 40,100,30),queue[z].speed.ToString());
						queue[z].showText = GUI.Toggle(new Rect(Screen.width-160,170 + (z+num1) * 40,100,30),queue[z].showText,"选择");
						if(GUI.Button(new Rect(Screen.width-70,170 + (z+num1) * 40,50,30),"删除"))
						{
							queue.Remove(queue[z]);
						}
					}

					GUI.HorizontalSlider(new Rect(300,200,700,30),currentState.time,0,currentState.length);
					GUI.Label(new Rect(300,180,700,30),currentState.time.ToString());
					GUI.Label(new Rect(240,190,55,35),"动画进度");

					if(GUI.Button(new Rect(Screen.width-70,165,50,30),"保存"))
					{
						Save(currentObj.name,currentState.name);
					}
				}
			}
		}
		int i = 0;
		int j = 0;
		int numPerRow = 7;
		int index = 0;
		foreach(GameObject pre in prefabs)
		{
			if(pre!=null)
			{
				if(GUI.Button(new Rect(10 + i*130,10 + j * 40,100,30),pre.name))
				{
					currentScale = 1;
					if(currentObj!=null)
					{
						currentState = null;
						queue = new List<AnimQueue>();
						currentObj.transform.parent = null;
						currentObj.transform.localScale = defaulScale;
						currentObj.SetActive(false);
					}
					currentObj = objs[index];
					defaulScale = currentObj.transform.localScale;
					controllObj.transform.position = new Vector3(0,-height/2-2,0);
					currentObj.transform.parent = controllObj.transform;
					currentObj.transform.localPosition =Vector3.zero;
					currentObj.SetActive(true);
				}
				i ++;
				if(i > numPerRow)
				{
					i = 0;
					j ++;
				}
				index ++;
			}
		}
	}
//
//	IEnumerator _Sample()
//	{
//		yield return null;
//		currentObj.animation.Stop();
//	}

	IEnumerator _Play()
	{
//		NcCurveAnimation[] ncCurves = currentObj.GetComponentsInChildren<NcCurveAnimation>(true);
//		AnimationState state = new AnimationState();
//
//		AnimationClip clip ;
//		clip.

		currentObj.GetComponent<Animation>().Play(currentState.name);
		currentState.speed = 1;
		AnimQueue nextAq = null;
		if(queue.Count>0)
		{
//			foreach(NcCurveAnimation ncCurve in ncCurves)
//			{
//				ncCurve.speed = queue[0].speed;
//			}
			yield return new WaitForSeconds(queue[0].t);
		}	
		for(int i=0;i<queue.Count;i++)
		{
			currentState.speed = queue[i].speed;
//			foreach(NcCurveAnimation ncCurve in ncCurves)
//			{
//				ncCurve.speed = queue[i].speed;
//			}
			float realDelay = 0;
			if(i<queue.Count-1)
			{
				nextAq = queue[i + 1];
				realDelay = (nextAq.t - queue[i].t)/queue[i].speed;
			}
			else
			{
				realDelay = (currentState.length - queue[i].t)/queue[i].speed;
			}
			if(queue[i].showText)
			{
				GetComponent<AudioSource>().clip = clip;
				GetComponent<AudioSource>().Play();
			}
			yield return new WaitForSeconds(realDelay);
		}
	}
	#if UNITY_EDITOR

	/// <summary>
	/// Gets the file by directory.
	/// only in Window.
	/// </summary>
	/// <returns>The file by directory.</returns>
	/// 

	string[] GetFileByDirectory()
	{
#if UNITY_WEBPLAYER
        string[] paths = null;
        Debug.LogError("Can't get files on WebPlayer");
#else
        string[] paths = Directory.GetFiles(Application.dataPath + path,"*.prefab",SearchOption.AllDirectories);
#endif
		return paths;
	}

	void LoadPrefabs()
	{
		string[] paths = GetFileByDirectory();
		prefabs.Clear();
		foreach(GameObject obj in objs)
		{
			DestroyImmediate(obj);
		}
		objs.Clear();
//		float height = Camera.main.orthographicSize;
//		float width = height * Screen.width/Screen.height;
//
//		int curRow = 0;
//		int curColumn = 0;
		foreach(string str in paths)
		{
			string assetPath = "Assets" + str.Replace(Application.dataPath, "").Replace('\\', '/');
			GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath,typeof(GameObject));
			if(prefab!=null)
			{
				prefabs.Add(prefab);
				GameObject obj = Instantiate(prefab) as GameObject;
				objs.Add(obj);
				obj.SetActive(false);
			}
		}
	}
	#endif


}
