using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;

public class EvolutionEffect : MonoBehaviour
{

    public GameObject CameraEffect;

    public GameObject heroBase;
    public GameObject curHeroModel;
    public GameObject evlHeroModel;

    public GameObject effectBase;

    public GameObject effect1;
    public Vector3 effect1Position;
	public Vector3 effect1Rotation;
    public GameObject effect2;
    public Vector3 effect2Position;
    public GameObject effect3;
    public Vector3 effect3Rotation;
    public Vector3 effect3Position;
    public GameObject effect4;
    public Vector3 effect4Position;
	public Vector3 effect4Rotation;
    public GameObject effectSoul;

    public float effect1Time;
    public float effect2Time;
    public float effect3Time;
    public float effect4Time=2;
	public float changeHeroTime=0.6f;
	public float changeColorTime=1;

    public Material heroMaterial;
    
    List<GameObject> creatEffects = new List<GameObject>();
    List<GameObject> creatHeros = new List<GameObject>();
    GameObject heroDest = null;
    GameObject heroResult = null;
    bool isEnd = false;
	bool isPlaying = false;
    // Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

//         if (Input.GetMouseButtonDown(0)&& !isPlaying)
//         {
//  			isPlaying=true;
//             CreatHeroModel();
//             ShowDisappearEffect();
// 
//         }
        
        if (isEnd)
        {
            Reset();
        }
	}

    public void StartPlay(string curModel, string evlModel)
    {
        curHeroModel = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroPrefabs[curModel];
        curHeroModel.layer = CameraEffect.layer;
        evlHeroModel = DataManager.getModule<DataHero>(DATA_MODULE.Data_Hero).dicHeroPrefabs[evlModel];
        evlHeroModel.layer = CameraEffect.layer;
        
        CreatHeroModel();
        ShowDisappearEffect();
    }

    void CreatHeroModel()
    {
        heroDest = NGUITools.AddChild(heroBase, curHeroModel);
        heroDest.layer = CameraEffect.layer;
    }

    void ClearHero()
    {

        NGUITools.Destroy(heroDest);
        NGUITools.Destroy(heroResult);
    }
    
    void ClearEffect()
    {
        foreach(GameObject go in creatEffects)
        {
            NGUITools.Destroy(go);
        }

        creatEffects.Clear();
    }

    void ShowDisappearEffect()
    {
        GameObject disappear1 = NGUITools.AddChild(effectBase, effect1);
        disappear1.transform.localPosition = effect1Position;
        disappear1.transform.localRotation = Quaternion.Euler(effect1Rotation);
        creatEffects.Add(disappear1);

        StartCoroutine(_ShowEffect2(effect1Time));
        StartCoroutine(_ShowEffect3(changeHeroTime + effect1Time));
    }


    IEnumerator _ShowEffect2(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject disappear4 = NGUITools.AddChild(effectBase, effect4);
        disappear4.transform.localPosition = effect4Position;
        disappear4.transform.localRotation = Quaternion.Euler(effect4Rotation);
        creatEffects.Add(disappear4);
    }

    IEnumerator _ShowEffect3(float delay)
    {
        yield return new WaitForSeconds(delay);

        heroDest.SetActive(false);
        heroResult = NGUITools.AddChild(heroBase, evlHeroModel);
        heroResult.layer = CameraEffect.layer;
        //StartCoroutine(ChangeHeroColor(heroResult, changeColorTime));
        StartCoroutine(_ShowEffect4(effect4Time));

    }



    IEnumerator _ShowEffect4(float delay)
    {
		yield return new WaitForSeconds(effect4Time-changeHeroTime);
        isEnd = true;
    }

    IEnumerator ChangeHeroColor(GameObject hero, float time)
    {
        SkinnedMeshRenderer[] smrs = hero.GetComponentsInChildren<SkinnedMeshRenderer>();
        SpriteRenderer[] srs = hero.GetComponentsInChildren<SpriteRenderer>();
        float t = 0;
        while (t < time)
        {
            for (int i = 0; i < smrs.Length; ++i)
            {
                smrs[i].sharedMaterial = heroMaterial;
                smrs[i].sharedMaterial.SetColor("_Color", new Color(1, 1, 1,1- t / time));
            }

            for (int i = 0; i < srs.Length; ++i)
            {
                srs[i].material = heroMaterial;
                srs[i].sharedMaterial.SetColor("_Color", new Color(1, 1, 1,1- t / time));
            }
            t += Time.deltaTime;

            yield return null;
        }
    }

    public void Reset()
    {
        ClearHero();
        ClearEffect();
        isEnd = false;
		isPlaying = false;
        gameObject.SetActive(false);
    }
}
