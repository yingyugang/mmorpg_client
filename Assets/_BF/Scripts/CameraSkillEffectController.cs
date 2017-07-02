using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraSkillEffectController : MonoBehaviour {	     
	// Use this for initialization

	private Camera cam;
	private float origSize;
	private float targetSize;
	private float from;
	private float to;
	//private int speed;
	private float holdtime;
	private float time;
	//private bool zoomin;
	private bool zoomout;
	private Vector3 origPos;
	private Vector3 currCamPos;
	private float posTime;
	public float add;
	public bool isPlay;
	public bool isRePosition;
	private GameObject effectPrefab;
	private GameObject effect;
	private BoxCollider2D coll2D;
	private Transform currHeroTrans;
	private Hero currHero;
	private string heroSortingLayerName;
	private Animator ani;

	
	public Vector3 targetPos;


	void Start () {
		cam = Camera.main;
		origSize = cam.orthographicSize;
		targetSize = 3;
		from = origSize;
		to = targetSize;
		//speed = 2;
		time = 0;
		origPos = Camera.main.transform.position;
		add = 1f;
		targetPos = new Vector3(0,0,-10);
		zoomout = true;
		//zoomin = false;
		isRePosition = false;
		posTime = 0;
		isPlay = false;
		effect = null;
		heroSortingLayerName = "";
	}
	
	// Update is called once per frame
	void Update () {
	
		if (isRePosition && posTime >= 1)
		{
			setPowerEffect();
			Time.timeScale = 0.001f;
			isRePosition = false;
			from = cam.orthographicSize;
			Debug.Log("holdtime........................" + isRePosition);
		}
		else
		if (isRePosition && posTime < 1)
		{
			posTime += Time.deltaTime * 4;
			cam.transform.position = Vector3.Lerp(currCamPos,targetPos,posTime);
			cam.orthographicSize = Mathf.Lerp(from,to,1); 
			Debug.Log("");
			//setPowerEffect();
		}

		else


		if (isPlay)
		{

			time += Time.deltaTime * 2;
			cam.orthographicSize = Mathf.Lerp(from,to,time); 
			//Debug.Log("time................................." + time);

			if ((from - to)< 0.001f)
			{
				holdtime += Time.deltaTime;
			}

			if (zoomout && holdtime == 0)
			{
				cam.transform.position = Vector3.Lerp(origPos,targetPos,time);
				//Debug.Log("origPos...................." + origPos);
				//Debug.Log("targetPos...................." + targetPos);
				add -= Time.deltaTime ;
				//Debug.Log("add...................." + add);
			}
			else if (zoomout == false && cam.orthographicSize > targetSize)
			{
				cam.transform.position = Vector3.Lerp(targetPos,origPos,time);
				add += Time.deltaTime ;
			}

			//Debug.Log("..............OK................." + cam.orthographicSize);
		
			if (cam.orthographicSize <= targetSize && holdtime == 0)
			{
				holdtime = 0;
				time = 0;
				from = targetSize;
				to = targetSize;	
				zoomout = true;
				Time.timeScale = 0.003f;
				currHero.heroAnimation.Play(Hero.ACTION_POWER);
				setPowerEffect();
			}
			else if ((zoomout || cam.orthographicSize <= targetSize) && holdtime > 0.001)
			{
				Time.timeScale = 1;
				holdtime = 0;
				time = 0;
				from = targetSize;
				to = origSize;
				zoomout = false;
				add = 1;
			}
			else if (cam.orthographicSize >= origSize)
			//else if (Vector3.Distance(cam.transform.position,origPos) < 0.01f)
			{
				
//				MotionBlur blur = BattleController.SingleTon().BattleCamera.GetComponent<MotionBlur>();
//				if(blur!=null)
//					blur.enabled = false;
				isPlay = false;			
				holdtime = 0;
				time = 0;
				from = origSize;
				to = targetSize;
				zoomout = true;
				add = 1;
			}
		}
	}
	
	public void play(Hero hero)
	{
		//if (hero)
			//targetPos = hero.localPosition;
//		MotionBlur blur = BattleController.SingleTon().BattleCamera.GetComponent<MotionBlur>();
//		if(blur!=null)
//			blur.enabled = true;
		currHero = hero;
		currHeroTrans = hero.transform;
		coll2D = currHeroTrans.GetComponentInChildren<BoxCollider2D>();
		targetPos = new Vector3(currHeroTrans.localPosition.x,currHeroTrans.localPosition.y + 3f,Camera.main.transform.position.z);


		Renderer[] renderers = hero.GetComponentsInChildren<Renderer>();
		if (renderers.Length > 0)
			heroSortingLayerName = renderers[0].sortingLayerName;


		targetPos.x = targetPos.x > 8f ? 8f:targetPos.x;	
		zoomout = true;
		//zoomin = false;
		//if (isPlay && cam.orthographicSize < 7)\
		//Debug.Log("isPlay ........................................." + isPlay);
		if (isPlay)
		{
			Time.timeScale = 1;
			posTime = 0;
			holdtime = 0;
			isRePosition = true;
			currCamPos = cam.transform.position;
			//to  = cam.orthographicSize;
			from  = cam.orthographicSize;
			to  = targetSize;

			/*Debug.Log("currCamPos...................................." + currCamPos);
			while (posTime < 1)
			{
				posTime += Time.deltaTime ;
				cam.transform.position = Vector3.Lerp(currCamPos,targetPos,posTime);
			}*/

		}
		else
		{
			isRePosition = false;
			isPlay = true;
		}
	}

	void setPowerEffect()
	{
		effectPrefab = Resources.Load("Effect/Effect_Water_Corr") as GameObject;
		if (effect != null)
			Destroy(effect);
		
		effect = GameObject.Instantiate(effectPrefab) as GameObject;


		if (heroSortingLayerName != "")
		{
			Renderer[] renderers = effect.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < renderers.Length; i ++)
			{
				renderers[i].sortingLayerName = heroSortingLayerName;
				renderers[i].sortingOrder = -1;
			}
		}

		//ani = effect.GetComponentInChildren<Animator>();

		ani = effect.GetComponentInChildren<Animator>();
		ani.speed = ani.speed * 10000;
	

		/*NcCurveAnimation[] NcAni = effect.GetComponentsInChildren<NcCurveAnimation>();
		for (int i = 0; i < NcAni.Length; i ++)
		{
			NcAni[i].IgnoreScaleTime = true;
		}*/
		
		effect.transform.localPosition = new Vector3(currHeroTrans.localPosition.x,currHeroTrans.localPosition.y + coll2D.size.y/2,2);
		effect.transform.localScale = new Vector3(1f,1f,1);
	}
}