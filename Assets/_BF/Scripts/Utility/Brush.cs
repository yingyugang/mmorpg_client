using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;

public class Brush : MonoBehaviour {

	public GameObject brush;
	public List<GameObject> brushs;
	public Camera cam;

	public delegate void OnMirrorClean();
	public OnMirrorClean onMirrorClean;
	public List<GameObject> nodes;
	public List<GameObject> initialNodes;

	public float showButtonDelay;
	public GameObject button;
	public float showButtonEffectDelay;

	public Blur blur;
	public GameObject hero;
	public Texture2D heroEffectMap;
	public Texture2D heroEffectDistMap;

	public Camera buttonCam;
	public SpriteRenderer buttonSpriteRenderer;
	public GameObject buttonActiveEffect;
	public GameObject buttonActiveEffect1;
	public GameObject buttonEffect;
	public GameObject buttonClickEffect;

	public Vector3 RareOffset;
	public List<RateUIEffect> rates;

    public Shader mShader;
	public AnimationCurve curve = AnimationCurve.Linear(0,0,1,1);

	bool isShowedBtn;
    bool isShowHero = false;
	void Awake()
	{
		InitNodes();
		//mShader = Shader.Find("Custom/Dissolve_Unlit");
	}

	void Update () {
		if(Input.GetMouseButton(0))
		{
            if (brushs.Count < 1000)
            {
                Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
                pos = new Vector3(pos.x, pos.y, 0);
                GameObject go = Instantiate(brush, pos, Quaternion.identity) as GameObject;
                go.transform.parent = transform;
                brushs.Add(go);
                go.hideFlags = HideFlags.HideAndDontSave;
                //ShowButton();
                Clean();
                //isShowed = true;
            }
            
		}
		if(Input.GetMouseButtonDown(0))
		{
			if (isEnd)
			{
                DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).HideCallHeroEffect();
			}
            else
            {

                if (isShowHero)
                {
                    return;
                }

                Collider2D c2d = Physics2D.OverlapPoint(buttonCam.ScreenToWorldPoint(Input.mousePosition));
			    if(c2d!=null)
			    {
				    StartCoroutine(ShowHero());
			    }
            }
			
		}
	}

	List<Material> mMats;
	IEnumerator ShowHero()
	{
		Debug.Log("ShowHero!");
        isShowHero = true;
		buttonEffect.SetActive(false);//button disppear
		buttonClickEffect.SetActive(true);
		yield return new WaitForSeconds(0.2f);

		button.SetActive (false);
		button.GetComponent<Collider2D>().enabled = false;


		yield return new WaitForSeconds(1.0f);

        if (hero == null)
        {
            if (gameObject.transform.FindChild("model") != null)
            {
                hero = gameObject.transform.FindChild("model").gameObject;
            }
        }

        if (hero != null)
        {
            hero.SetActive(true);
            mMats = new List<Material>();
            SkinnedMeshRenderer[] smrs = hero.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer smr in smrs)
            {
                Texture tex = smr.material.mainTexture;
                smr.material = new Material(mShader);
                smr.material.mainTexture = tex;
				SetHeroEffectMaterial(smr.material);
                mMats.Add(smr.material);
                smr.sortingLayerName = "Default";
            }
            SpriteRenderer[] srs = hero.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in srs)
            {
                sr.material =new Material(mShader);
				SetHeroEffectMaterial(sr.material);
                mMats.Add(sr.material);
                sr.sortingLayerName = "Default";
            }
            StartCoroutine(_ShowHero());
        }
        else
        {
            StartCoroutine(_endEffect());
        }
        
	}

	void SetHeroEffectMaterial (Material mat){
			mat.SetFloat("_Amount",0);
			mat.SetFloat ("_StartAmount",0.21f);
			mat.SetFloat ("_TransRange",0.03f);
			mat.SetFloat ("_Illuminate",2.0f);
			mat.SetFloat ("_Distortion",0.5f);
			mat.SetFloat ("_DistSpeed",6.03f);
			mat.SetColor ("_DissColor",new Color(0.0f,0.3f,1.0f,1.0f));
			mat.SetTexture ("_DissolveSrc",heroEffectMap);
			mat.SetTexture ("_DissolveSrcBump",heroEffectDistMap);
		}

	IEnumerator _ShowHero()
	{

		float t = 0;
		while(t < 1)
		{
			t += Time.deltaTime;
			foreach(Material mat in mMats)
			{
				mat.SetFloat("_Amount",1-curve.Evaluate(t));
			}
			//buttonSpriteRenderer.color = new Color(buttonSpriteRenderer.color.r,buttonSpriteRenderer.color.g,buttonSpriteRenderer.color.b,1-t);
			yield return null;
		}
		GameObject go = rates[Random.Range(0,rates.Count)].startGo;
		go.SetActive(true);
		UITweener[] tws = go.GetComponents<UITweener>();
		foreach(UITweener tw in tws)
		{
			tw.PlayForward();
            tw.AddOnFinished(EndEffect);
		}
	}

	void ShowButton()
	{

        StartCoroutine(ShowHero());
        //StartCoroutine(_ShowButton(showButtonDelay));
	}

	IEnumerator _ShowButton(float delay)
	{
		buttonActiveEffect.SetActive(true);
		float t = 0;
		while(t < delay)
		{
			t += Time.deltaTime;
			blur.blurSize = Mathf.Lerp(0,10,t/delay);
			if(t/delay > 0.5f)
			{
				buttonSpriteRenderer.color = new Color(buttonSpriteRenderer.color.r,buttonSpriteRenderer.color.g,buttonSpriteRenderer.color.b,t/delay);
			}
			else
			{
				Material mat = buttonActiveEffect1.GetComponent<Renderer>().material;
				mat.SetColor("_TintColor",new Color(0.5f,0.5f,0.5f,t/delay * 2));
			}
				
			yield return null;
		}
		yield return new WaitForSeconds(Mathf.Max(0,showButtonEffectDelay - delay));
		buttonEffect.SetActive(true);

		buttonSpriteRenderer.GetComponent<Collider2D>().enabled = true;
	}

	static int nodeLayer = 31;
	void InitNodes()
	{
		nodes = new List<GameObject>();
		int x = 3,y = 5;
		for(int i = -2;i < x ; i ++)
		{
			for(int j = -4;j < y;j ++)
			{
				GameObject go = new GameObject();
				go.transform.parent = transform;
				go.transform.localPosition = new Vector3(i,j,0);
				go.AddComponent<BoxCollider2D>();
				go.layer = nodeLayer;
				nodes.Add(go);
			}
		}
		initialNodes.AddRange(nodes);

		for(int i=0;i<rates.Count;i++){
			rates[i].uiEffect.transform.position+=RareOffset;

		}

	}

	void Clean()
	{
		Collider2D c2d = Physics2D.OverlapPoint(cam.ScreenToWorldPoint(Input.mousePosition));
		if(c2d != null)
		{
			nodes.Remove(c2d.gameObject);
		}
		if(nodes.Count < 25)
		{
			if(!isShowedBtn)
			{
				if(onMirrorClean!=null)
					onMirrorClean();
				ShowButton();
				isShowedBtn = true;
			}
		}
	}

	void OnDrawGizmos()
	{
		foreach(GameObject go in nodes)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(go.transform.position,Vector3.one);
		}
	}

	public void Reset()
	{
		foreach(GameObject go in brushs)
		{
			go.SetActive(false);
		}
		List<GameObject> tmp = brushs;
		StartCoroutine(DestroyBrushs(tmp));
		brushs= new List<GameObject>();
		buttonSpriteRenderer.color = new Color(buttonSpriteRenderer.color.r,buttonSpriteRenderer.color.g,buttonSpriteRenderer.color.b,0);
		buttonSpriteRenderer.GetComponent<Collider2D>().enabled = false;
		buttonActiveEffect.SetActive(false);;
		buttonEffect.SetActive(false);
		buttonClickEffect.SetActive(false);
//		buttonActiveEffect1.color =  new Color(buttonActiveEffect1.color.r,buttonActiveEffect1.color.g,buttonActiveEffect1.color.b,0);
		Material mat = buttonActiveEffect1.GetComponent<Renderer>().material;
		mat.SetColor("_TintColor",new Color(0.5f,0.5f,0.5f,0.5f));
		nodes.Clear();
		nodes.AddRange(initialNodes);
		foreach(RateUIEffect effectGo in rates)
		{
			UITweener[] tws = effectGo.uiEffect.GetComponentsInChildren<UITweener>();
			if(tws!=null)
			{
				foreach(UITweener tw in tws)
				{
					tw.ResetToBeginning();
					tw.gameObject.SetActive(false);
				}
			}
		}

        isEnd = false;
        isShowedBtn = false;
        isShowHero = false;

        button.SetActive(true);
        button.GetComponent<Collider2D>().enabled = true;
	}

	IEnumerator DestroyBrushs(List<GameObject> tmp)
	{
		for(int i = 0;i < tmp.Count;i ++)
		{
			Destroy(tmp[i]);
			yield return null;
		}
	}

//	void OnGUI()
//	{
//		if(GUI.Button(new Rect(10,10,100,30),"Reset"))
//		{
//			Reset();
//		}
//	}

    public void EndEffect()
    {
        StartCoroutine(_endEffect());
    }


    bool isEnd = false;
    IEnumerator _endEffect()
    {
        yield return new WaitForSeconds(2.0f);
        isEnd = true;
    }



	[System.Serializable]
	public class RateUIEffect
	{
		public GameObject uiEffect;
		public GameObject startGo;
	}

}
