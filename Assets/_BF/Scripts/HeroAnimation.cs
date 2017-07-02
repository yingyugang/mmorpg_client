using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroAnimation : MonoBehaviour {

	public HeroRes heroRes;
	public HeroResEffect heroResEffect;

	void Awake()
	{
		if(heroRes==null)heroRes = GetComponent<HeroRes>();
		if(heroResEffect==null)heroResEffect = GetComponent<HeroResEffect>();
	}

	void Reset()
	{
		StopCoroutine("_PlayByFrags");
		StopCoroutine("_PlayWithRealTime");
	}

	public void PlayWithRealTime(string clipName)
	{
		Reset();
		ShopAnim();
		StartCoroutine("_PlayWithRealTime",clipName);
	}

	IEnumerator _PlayWithRealTime(string clipName)
	{
		Debug.Log("_PlayWithRealTime");
		AnimMapping am = ActiveAnimMapping(clipName);
        if(am!=null && am.anim!=null)
        {
            AnimationState state = am.anim[am.clip.name];
            bool isPlaying = true;
            float _startTime = 0F;
            float _progressTime = 0F;
            float clipLenth = am.clip.length;
            am.anim.Play(am.clip.name);
            while (isPlaying) 
            {
                _progressTime = Mathf.Clamp(_progressTime + RealTime.deltaTime,0,clipLenth);
                state.normalizedTime = _progressTime / clipLenth; 
                //          Debug.Log(_progressTime);
                am.anim.Sample ();
                if(_progressTime >= clipLenth)
                {
                    if(state.wrapMode != WrapMode.Loop)
                    {
                        am.anim.Stop();
                        isPlaying = false;
                    }
                    else
                    {
                        _progressTime = 0;
                    }
                }
                
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            Debug.LogError("The " + heroResEffect.name + "'s clipName "+ clipName +" is null!");
            yield return null;
        }
	}

	public string currentAnimClip;
	public void Play(string clipName,float offsetSpeed = 1)
	{
		if(clipName == Hero.ACTION_STANDBY && currentAnimClip == clipName){
			return;
		}
		currentAnimClip = clipName;
		PlayByFrags(clipName,offsetSpeed);
	}
	
	public void PlayByFrags(string clipName,float offsetSpeed = 1)
	{
		AnimMapping am = ActiveAnimMapping(clipName);
		Reset();
		ShopAnim();
		am.offsetSpeed = offsetSpeed;
		if(am!=null )
		{
			if(am.frags!=null && am.frags.Count>0)
			{
				StartCoroutine("_PlayByFrags",am);
			}
			else
			{
                if(am.anim!=null && am.clip!=null)
				{
					am.anim[am.clip.name].speed = am.globalSpeed;
//					Debug.Log("am.globalSpeed:" + am.globalSpeed);
//					Debug.Log(offsetSpeed);
					am.anim.Play(am.clip.name);
				}
			}
		}
	}
	
	public void PlayByQueue(string clipName)
	{
		StopCoroutine("_PlayByQueue");
		ShopAnim();
		AnimMapping am = ActiveAnimMapping(clipName);

		if(am == null)
			return;
		if(am.clipName == null || am.clipName == "")
			return;
		am.anim.Play(am.clip.name);
		StartCoroutine("PlayByQueue",am);
	}
	
	IEnumerator _PlayByFrags(AnimMapping am)
	{
		Debug.Log("PlayByFrags");
		List<AnimationFrag> animFrags = am.frags;
		#if UNITY_EDITOR
		float totalDuration = 0;
		foreach(AnimationFrag frag in animFrags)
		{
			totalDuration += (frag.stopTime - frag.startTime) / frag.speed * frag.loopCount / am.globalSpeed;
		}
//		Debug.Log("totalDuration:" + totalDuration);
		#endif
		int index = 0;
		AnimationFrag currentFrag = null;
		while(index < animFrags.Count)
		{
			currentFrag = animFrags[index];
			currentFrag.loopCount = Mathf.Max(1,currentFrag.loopCount);
			for(int i=0;i<currentFrag.loopCount;i++)
			{
				float dur = (currentFrag.stopTime - currentFrag.startTime) / currentFrag.speed / am.globalSpeed;;
				if(!am.anim.IsPlaying(am.clip.name))
				{
					am.anim.Play(am.clip.name);
				}
				am.anim[am.clip.name].time = currentFrag.startTime;
				am.anim[am.clip.name].speed = currentFrag.speed * am.globalSpeed * am.offsetSpeed;
				yield return new WaitForSeconds(dur);
				if(currentFrag.rewind)
				{
					dur = (currentFrag.stopTime - currentFrag.startTime) / currentFrag.rewindSpeed / am.globalSpeed;;
					am.anim[am.clip.name].time = currentFrag.stopTime;
					am.anim[am.clip.name].speed = -currentFrag.rewindSpeed * am.globalSpeed  * am.offsetSpeed;
					yield return new WaitForSeconds(dur);
				}
			}
			index ++;
		}
		am.offsetSpeed = 1;
		ShopAnim();
	}
	
	IEnumerator PlayByQueue(AnimMapping animMap)
	{
		List<AnimQueue> queue = animMap.queue;
		if(queue.Count>0)
		{
			yield return new WaitForSeconds(queue[0].t);
		}
		AnimQueue nextAq;
		AnimationState currentState = animMap.anim[animMap.clip.name];
		for(int i=0;i<queue.Count;i++)
		{
			currentState.speed = queue[i].speed;
			float realDelay = 0;
			if(i < queue.Count-1)
			{
				nextAq = queue[i + 1];
				realDelay = (nextAq.t - queue[i].t)/queue[i].speed;
			}
			else
			{
				realDelay = (currentState.length - queue[i].t)/queue[i].speed;
			}
			yield return new WaitForSeconds(realDelay);
		}
	}
	
	public string SetOrderLayer(int location)
	{

		if(location == 2)
		{
			location = 0;
		}
		else if(location == 0)
		{
			location = 1;
		}
		else if(location == 1 )
		{
			location = 3;
		}
		else if(location == 3){
			location = 2;
		}
		else if(location == 4)
		{
			location = 4;
		}
		else if(location == 5)
		{
			location = 4;
		}
		string orderLayer = "UnitLayer" + location;
		CommonUtility.SetSortingLayerWithChildren(gameObject,orderLayer);
		if(heroRes.Shadow!=null && heroRes.Shadow.GetComponent<SpriteRenderer>()!=null)
		{
			heroRes.Shadow.GetComponent<SpriteRenderer>().sortingLayerName = "Character";
		}
		return orderLayer;
	}

	public bool HasAnimClip(string clipName)
	{
		return heroRes.bodyAnimMapping.ContainsKey(clipName);
	}

	public float GetAnimClipLength(string clipName)
	{
		AnimMapping am = GetAnimMapping(clipName);
		float animLength = 0;
		if (am != null)
		{
			if(am.frags==null || am.frags.Count == 0)
			{
				animLength = am.clip.length;
			}
			else
			{
				animLength = CommonUtility.GetFragsLength(am.frags);
			}
		}
		return animLength;
	}

	public bool IsPlaying(string clipName)
	{
		if(heroRes.CurrentAm==null || heroRes.CurrentAm.clipName != clipName || !heroRes.CurrentAm.anim.IsPlaying(heroRes.CurrentAm.clip.name))
		{
			return false;
		}
		return true;
	}

	void ShopAnim()
	{
		if(heroRes.CurrentAm!=null && heroRes.CurrentAm.anim!=null)
		{
			heroRes.CurrentAm.anim[heroRes.CurrentAm.clip.name].time = 0;
			heroRes.CurrentAm.anim[heroRes.CurrentAm.clip.name].speed = 1;
			heroRes.CurrentAm.anim.Stop();
		}
	}

	AnimMapping GetAnimMapping(string clipName)
	{
		Debug.Log(clipName);
		AnimMapping am = null;
		heroRes.bodyAnimMapping.TryGetValue(clipName,out am);
		if(am == null)
		{
			Debug.LogError(gameObject.name + " has not " + clipName);
		}
		else if(am.clipName == null || am.clipName == "" || am.body == null || am.clip == null)
		{
			Debug.LogError(gameObject.name + " has not " + clipName);
		}
		return am;
	}

	AnimMapping ActiveAnimMapping(string clipName)
	{
		AnimMapping am = null;
		heroRes.bodyAnimMapping.TryGetValue(clipName,out am);
		if(am == null)
		{
			Debug.LogError(gameObject.name + " has not " + clipName);
			return am;
		}
		if(am.clipName == null || am.clipName == "" || am.body == null || am.clip == null)
		{
			Debug.LogError(gameObject.name + " has not " + clipName);
			return am;
		}
		if(heroRes.CurrentAm!=am)
		{
			if(heroRes.CurrentAm!=null && heroRes.CurrentAm.body!=null)
				heroRes.CurrentAm.body.SetActive(false);
			if(am.body!=null && !am.body.activeInHierarchy)
				am.body.SetActive(true);
			heroRes.CurrentAm = am;
		}
		return am;
	}

}
