using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using ThinksquirrelSoftware.Utilities;
//using DataMgr;

public class RealTimeUtility : MonoBehaviour {

//	public GameObject SkillHero;
//	public Camera SkillCamera;
//	public Animation HeroAnim;
//	public Animation CamareAnim;
//	public UnitSkill unitkill;
//	public DataMgr.Skill currskill;
//	public Transform m_Trans;
//
//	public Transform HeroObjPoint;
//	public Dictionary<int,GameObject> HeroObjs;  
//
//
////	public Rect m_DefaultRect;
//	public float EffectDuration = 2;
//	public bool NeedShake = true;
//	public float ShakeDelayTime = 0.5f;
//	
//	Animator[] animators;
//	Animation[] animations;
//	NcCurveAnimation[] NcCurves;
////	void Awake()
////	{
////		animators = GetComponentsInChildren<Animator>();
////		animations = GetComponentsInChildren<Animation>();
////		m_DefaultRect = SkillCamera.pixelRect;
////	}
//
//	public GameObject CurrentHero;
//	public string CurrentAnimState;
//
//	public void Play()
//	{
//		EffectDuration = 1.7f;
////		SkillCamera.pixelRect = m_DefaultRect;
//		BattleController.SingleTon().ShowHeroOverlayBar(false);
//		AudioManager.PlaySound(BattleController.SingleTon().GlobalAudio);
//		SkillHero.SetActive(true);
//		CurrentHero.SetActive(true);
//		if(CurrentHero!=null)
//		{
//			CurrentHero.transform.parent = HeroObjPoint;
//			CurrentHero.transform.localPosition = Vector3.zero;
//			CurrentHero.transform.localScale = Vector3.one;
//			HeroAnim = CurrentHero.GetComponentInChildren<Animation>();
//			if(CurrentAnimState==null || HeroAnim[CurrentAnimState]==null)CurrentAnimState="Violent01";
//			HeroAnim.Play(CurrentAnimState);
////			HeroAnim.s = HeroAnim[CurrentAnimState];
//		}
//		if(animators==null)animators = GetComponentsInChildren<Animator>();
//		animations = GetComponentsInChildren<Animation>();
//		if(NcCurves==null)NcCurves = GetComponentsInChildren<NcCurveAnimation>();
//		Time.timeScale = 0.001f;
//		NomarlizeAnimSpeed();
////		StartCoroutine(PlayAnim(HeroAnim,"Sparta_Violent",false));
////		StartCoroutine(PlayAnim(CamareAnim,"NewSkillCamera",false));
//		StartCoroutine(ReleasePause(EffectDuration));
//	}
//
//	public void Play1(Animation anim,string name)
//	{
//		Debug.Log("Play1");
//		AudioManager.PlaySound(BattleController.SingleTon().GlobalAudio);
//		Time.timeScale = 0.001f;
//		anim[name].speed = 0.5f/Time.timeScale;
//		anim.Play(name);
//		Debug.Log("Time....................start..............." + Time.realtimeSinceStartup);
//		StartCoroutine(_Play1(anim,name));
//	}
//
//	IEnumerator _Play1(Animation anim,string name)
//	{
//		float _startTime = Time.realtimeSinceStartup;
//		bool isPlay = true;
//		bool ShootStandBy = false;
//		while(isPlay)
//		{
//			if(Time.realtimeSinceStartup - _startTime >= 1)
//			{
//				if(BattleController.SingleTon().GlobalEffect )BattleController.SingleTon().GlobalEffect.SetActive(true);
//			}
//			if(Time.realtimeSinceStartup - _startTime >= 1.8f && currskill.ShootStandByEffectPrefab != null && ShootStandBy == false)
//			{
//				ShootStandBy = true;
//				GameObject go = Instantiate(currskill.ShootStandByEffectPrefab,m_Trans.position,m_Trans.rotation) as GameObject;
//			}
//			if(Time.realtimeSinceStartup - _startTime >= 2f)
//			{
//
//				isPlay = false;
//			}
//			yield return new WaitForEndOfFrame();
//		}
//		//Debug.Log("Time.....................end.............." + Time.realtimeSinceStartup);
//		yield return null;
//		Time.timeScale = 1;
//		anim[name].speed = 1;
//
//
//		//GameObject go = Instantiate(currskill.ShootStandByEffectPrefab,m_Trans.position,m_Trans.rotation) as GameObject;
//		yield return new WaitForSeconds(0.5f);
//		//StartCoroutine(_Destory(go,100));
//
//		if (unitkill != null)
//		{
//			Debug.Log("skill name......................... " + unitkill.currSkill.SkillId);
//			BattleController.SingleTon().gSkilling = false;
//			unitkill.SetSkillDone();
//		}
//	}
//
//	public void PlayThunder(Vector3 pos,float delay,UnitAttribute attr,Unit unit,float damage)
//	{
//		StartCoroutine(_PlayThunder(pos,delay,attr,unit,damage));
//	}
//
//	IEnumerator _PlayThunder(Vector3 pos,float delay,UnitAttribute attr,Unit unit,float damage)
//	{
//		float _startTime = Time.realtimeSinceStartup;
//		bool isPlay = true;
//		while(isPlay)
//		{
//			if(Time.realtimeSinceStartup - _startTime >= delay)
//			{
//				if(attr!=null && unit !=null)
//				{
//					damage = Random.Range(damage/2,damage);
//					attr.LastAttacker = unit;
//					attr.Hit(damage,true);
//				}
//				Instantiate(BattleController.SingleTon().ThunderEffect,pos,Quaternion.identity);
//				Time.timeScale = 1;
//				AudioManager.PlaySound(BattleController.SingleTon().ThunderAudioClip,pos);
//				Time.timeScale = 0;
//				isPlay = false;
//			}
//			yield return new WaitForEndOfFrame();
//		}
//	}
//
//
////	void OnGUI()
////	{
////		if(GUI.Button(new Rect(10,10,100,30),"Pause!"))
////		{
////
//////			Instantiate(SkillEffect,transform.position,Quaternion.identity);
////			SkillCamera.pixelRect = m_DefaultRect;
////			SkillHero.SetActive(true);
////			Time.timeScale = 0;
////			StartCoroutine(PlayAnim(HeroAnim,"Sparta_Violent",false));
////			StartCoroutine(PlayAnim(CamareAnim,"NewSkillCamera",false));
////			StartCoroutine(ReleasePause(2));
////		}
////	}
//
//	public void NomarlizeAnimSpeed()
//	{
//		if(animators!=null)
//		{
//			foreach(Animator anim in animators)
//			{
//				anim.speed = 1 / Time.timeScale;
//			}
//		}
////		if(HeroAnim!=null)
////		{
////			if(HeroAnim[CurrentAnimState]!=null)HeroAnim[CurrentAnimState].speed = 1 / Time.timeScale;
////		}
//
//		if(animations!=null)
//		{
//			foreach(Animation anim in animations)
//			{
//				foreach(AnimationState state in anim)
//				{
//					state.speed = 1 / Time.timeScale;
//				}
//			}
//		}
//		if(NcCurves!=null)
//		{
//			foreach(NcCurveAnimation anim in NcCurves)
//			{
//				anim.IgnoreScaleTime = true;
//			}
//		}
//	}
//
//	IEnumerator ReleasePause(float delayTime)
//	{
//		float _startTime = Time.realtimeSinceStartup;
//		bool isPlay = true;
//		while(isPlay)
//		{
//			if(Time.realtimeSinceStartup - _startTime >= delayTime)
//			{
//				isPlay = false;
//			}
//			yield return new WaitForEndOfFrame();
//		}
//		yield return null;
//		Time.timeScale = 1;
//		BattleController.SingleTon().ShowHeroOverlayBar(true);
//		NomarlizeAnimSpeed();
//		if(CurrentHero!=null)
//		{
//			CurrentHero.SetActive(false);
//			CurrentHero.transform.parent = null;
//		}
//		SkillHero.SetActive(false);
//		if(NeedShake)StartCoroutine(DelayShake(ShakeDelayTime));
//		Debug.Log("ReleasePause");
//	}
//
//	IEnumerator DelayShake(float delay)
//	{
//		yield return new WaitForSeconds(delay);
//		CameraShake.Shake();
//	}
//
//	IEnumerator SkillHeroOut()
//	{
//		bool isPlay = true;
//		float width = SkillCamera.pixelRect.width;
//		float duration = 0.5f;
//		float widthPerSecond = width / duration;
//		while(isPlay)
//		{
//			if(width<=0)
//			{
//				isPlay=false;
////				SkillCamera.pixelRect = m_DefaultRect;
//				SkillHero.SetActive(false);
//				yield return null;
//			}
//			width -= widthPerSecond * Time.deltaTime;
//			SkillCamera.pixelRect = new Rect(SkillCamera.pixelRect.x,SkillCamera.pixelRect.y,width,SkillCamera.pixelRect.height);
//			yield return null;
//		}
//	}
//
//	public IEnumerator PlayAnim( this Animation animation, string clipName, bool useTimeScale)
//	{
//
////		Debug.Log(&quot;Overwritten Play animation, useTimeScale? &quot; + useTimeScale);
//		//We Don't want to use timeScale, so we have to animate by frame..
//		if(!useTimeScale)
//		{
////			Debug.Log(&quot;Started this animation! ( &quot; + clipName + &quot; ) &quot;);
//			AnimationState _currState = animation[clipName];
//			bool isPlaying = true;
////			float _startTime = 0F;
//			float _progressTime = 0F;
//			float _timeAtLastFrame = 0F;
//			float _timeAtCurrentFrame = 0F;
//			float deltaTime = 0F;
//			float stateLength = _currState.length;
//			animation.Play(clipName);
//			
//			_timeAtLastFrame = Time.realtimeSinceStartup;
//			while (isPlaying) 
//			{
//				_timeAtCurrentFrame = Time.realtimeSinceStartup;
//				deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
//				_timeAtLastFrame = _timeAtCurrentFrame; 
//				
//				_progressTime += deltaTime;
//				_currState.normalizedTime = _progressTime / stateLength; 
//				animation.Sample ();
//				if (_progressTime >= stateLength) 
//				{
//					//Debug.Log(&quot;Bam! Done animating&quot;);
//					if(_currState.wrapMode != WrapMode.Loop)
//					{
//						//Debug.Log(&quot;Animation is not a loop anim, kill it.&quot;);
//						//_currState.enabled = false;
//						isPlaying = false;
//					}
//					else
//					{
//						//Debug.Log(&quot;Loop anim, continue.&quot;);
//						_progressTime = 0.0f;
//					}
//				}
//				
//				yield return new WaitForEndOfFrame();
//			}
//			yield return null;
////			if(onComplete != null)
////			{
//////				Debug.Log(&quot;Start onComplete&quot;);
////				onComplete();
////			} 
//		}
//		else
//		{
//			animation.Play(clipName);
//		}
//	}

}
