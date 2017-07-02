using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;

public class StrengthenEffect : MonoBehaviour {

    public GameObject CameraHero;
    public GameObject CameraEffect;

    public GameObject heroBase;
    public List<GameObject> heroSources;
    public GameObject effectBase;
    public List<GameObject> effectSources;

    public GameObject effect1;
    public Vector3 effect1Position=new Vector3(0,0,0);
	public Vector3 effect1Rotation=new Vector3(0,0,0);
	public Vector3 effect1Scale=new Vector3(1,1,1);
    public GameObject effect2;
	public Vector3 effect2Position=new Vector3(0,0,0);
	public Vector3 effect2Rotation=new Vector3(0,0,0);
	public Vector3 effect2Scale=new Vector3(1,1,1);
    public GameObject effect4;
	public Vector3 effect4Position=new Vector3(0,0,0);
	public Vector3 effect4Rotation=new Vector3(0,0,0);
	public Vector3 effect4Scale=new Vector3(1,1,1);

    public float effect1Time;
    public float effect2Time;

    public Shader heroShader;
    public Material heroMaterial;

    public int successType = 0;
    public GameObject uiroot;
    public GameObject success1;
    public GameObject success2;
    public GameObject success3;

    public GameObject heroModel;
    
    List<GameObject> creatEffects = new List<GameObject>();
    List<GameObject> creatHeros = new List<GameObject>();

    bool isEnd = false;
    // Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            if (isEnd)
            {
                DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).HideStrengthenEffect();
            }
//             else
//             {
//                 CreatHeroModel2();
//                 ShowDisappearEffect();
//             }
        }
	}

    void CreatHeroModel()
    {
        int nCount = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).srcHeroList.Count;
        for (int i = 0; i < nCount; ++i)
        {
            string srcfbx = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).srcHeroList[i];
            GameObject srcModel = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroPrefabs[srcfbx];

            GameObject hero = NGUITools.AddChild(heroSources[i], srcModel);
            creatHeros.Add(hero);
        }


        string fbxFile = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).destEnhanceHero.fbxFile;
        GameObject modelPrefab = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroPrefabs[fbxFile];
        GameObject heroDest = NGUITools.AddChild(heroBase, modelPrefab);
        creatHeros.Add(heroDest);


        //
        for (int i = 0; i < effectSources.Count; ++i)
        {
            
            if (i < nCount)
            {
                effectSources[i].SetActive(true);
            }
            else
            {
                effectSources[i].SetActive(false);
            }
        }
    }

    public void CreatHeroModel2()
    {
        for (int i = 0; i < 5; ++i)
        {
            GameObject hero = NGUITools.AddChild(heroSources[i], heroModel);
            creatHeros.Add(hero);
        }

        GameObject heroDest = NGUITools.AddChild(heroBase, heroModel);
        creatHeros.Add(heroDest);
    }

    void ClearHero()
    {
        foreach (GameObject go in creatHeros)
        {
            NGUITools.Destroy(go);
        }

        creatHeros.Clear();
    }
    
    void ClearEffect()
    {
        foreach(GameObject go in creatEffects)
        {
            NGUITools.Destroy(go);
        }

        creatEffects.Clear();
    }

    public void StartPlay()
    {
        CreatHeroModel();
        ShowDisappearEffect();
    }
    
    void ShowDisappearEffect()
    {
        foreach (GameObject go in effectSources)
        {
            GameObject disappear1 = NGUITools.AddChild(go, effect1);
            disappear1.transform.localPosition = effect1Position;
			disappear1.transform.localRotation =Quaternion.Euler(effect1Rotation);
			disappear1.transform.localScale=effect1Scale;
            creatEffects.Add(disappear1);
        }


		foreach (GameObject go in heroSources)
		{
			StartCoroutine(ChangeHeroColor(go,effect1Time));
		}

        StartCoroutine(_ShowEffect2(effect1Time));
    }


    IEnumerator _ShowEffect2(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (GameObject go in effectSources)
        {
            GameObject disappear2 = NGUITools.AddChild(go, effect2);
            disappear2.transform.localPosition = effect2Position;
			disappear2.transform.localRotation =Quaternion.Euler(effect2Rotation);
			disappear2.transform.localScale=effect2Scale;
            creatEffects.Add(disappear2);
        }

        StartCoroutine(_ShowEffect4(effect2Time));
    }

    IEnumerator _ShowEffect4(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject down = NGUITools.AddChild(effectBase, effect4);
        down.transform.localPosition = effect4Position;
		down.transform.localRotation =Quaternion.Euler(effect4Rotation);
		down.transform.localScale=effect4Scale;
        creatEffects.Add(down);

        StartCoroutine(_ShowSuccess());

		//StartCoroutine(ChangeHeroColor2(heroBase,0.5f,0.6f));
    }

    IEnumerator _ShowSuccess()
    {
        yield return new WaitForSeconds(0.2f);
        NGUITools.AddChild(uiroot, effect4);
        GameObject successEffect = null;
        
        switch (successType)
        {
            case 0:
                successEffect = NGUITools.AddChild(uiroot, success1);
                break;
            case 1:
                successEffect = NGUITools.AddChild(uiroot, success2);
                break;
            case 2:
                successEffect = NGUITools.AddChild(uiroot, success3);
                break;
        }

        successEffect.SetActive(true);
        creatEffects.Add(successEffect);

        yield return new WaitForSeconds(2.0f);
        isEnd = true;
        DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).HideStrengthenEffect();
    }

    IEnumerator ChangeHeroColor(GameObject hero,float time)//original to white
    {
        SkinnedMeshRenderer[] smrs = hero.GetComponentsInChildren<SkinnedMeshRenderer>();
		SpriteRenderer[] srs = hero.GetComponentsInChildren<SpriteRenderer>();
        float t=0;
		while(t<time){
			for (int i = 0; i < smrs.Length; ++i)
			{
                smrs[i].sharedMaterial = heroMaterial;
                smrs[i].sharedMaterial.SetColor("_Color",new Color(1,1,1,t/time));	
			}

			for (int i = 0; i < srs.Length; ++i)
			{
                srs[i].material = heroMaterial;
				srs[i].sharedMaterial.SetColor("_Color",new Color(1,1,1,t/time));	
    		}
    		t+=Time.deltaTime;
			
			yield return null;
		}

		hero.SetActive (false);

    }

	IEnumerator ChangeHeroColor2(GameObject hero,float delaytime,float time)//white to original
	{
		yield return new WaitForSeconds (delaytime);

		SkinnedMeshRenderer[] smrs = hero.GetComponentsInChildren<SkinnedMeshRenderer>();
		SpriteRenderer[] srs = hero.GetComponentsInChildren<SpriteRenderer>();
		float t=0;
		while(t<time){
			for (int i = 0; i < smrs.Length; ++i)
			{
				smrs[i].sharedMaterial = heroMaterial;
				smrs[i].sharedMaterial.SetColor("_Color",new Color(1,1,1,1-t/time));	
			}
			
			for (int i = 0; i < srs.Length; ++i)
			{
				srs[i].material = heroMaterial;
				srs[i].sharedMaterial.SetColor("_Color",new Color(1,1,1,1-t/time));	
			}
			t+=Time.deltaTime;
			
			yield return null;
		}
		
	}

    public void Reset()
    {
        ClearEffect();
        ClearHero();

        foreach (GameObject go in heroSources)
        {
            go.SetActive(true);
        }

        foreach (GameObject go in effectSources)
        {
            go.SetActive(true);
        }

        isEnd = false;
    }
}
