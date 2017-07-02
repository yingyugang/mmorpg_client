using UnityEngine;
using System.Collections;

public class ParticleUtility : MonoBehaviour {

	public float startTime;
	public float endTime;
	public float totalTime;
	public bool isLoop;
	float mCurrentTime;
	[HideInInspector]public bool isPlaying;
	ParticleEmitter mEmitter;
	public ParticleSystem mParticleSystem;

	void Awake()
	{
		mEmitter = GetComponent<ParticleEmitter>();
		mParticleSystem = GetComponent<ParticleSystem>();
	}

	public void Reset()
	{
		mCurrentTime = 0;
		isPlaying = true;
		if(mEmitter!=null)mEmitter.emit = true;
		if(mParticleSystem!=null)mParticleSystem.Play();
	}

	public void Stop()
	{
		isPlaying = false;
		if(mEmitter!=null)mEmitter.emit = false;
		if(mParticleSystem!=null)mParticleSystem.Stop();
	}

	void OnEnable()
	{
		Reset();
	}

	void OnDisable()
	{
		Stop();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	bool mIsStart;
	bool mIsEnd;
	void Update () {
		if(isPlaying)
		{
			mCurrentTime += Time.deltaTime;
			if(!mIsStart && mCurrentTime>=startTime)
			{
				mIsStart = true;
				if(mEmitter!=null)mEmitter.emit = true;
				if(mParticleSystem!=null)mParticleSystem.Play();
			}
			if(endTime>0 && !mIsEnd && mCurrentTime>=endTime)
			{
				if(mEmitter!=null)mEmitter.emit = false;
				if(mParticleSystem!=null)mParticleSystem.Stop();
			}
			if(isLoop)
			{
				if(mCurrentTime>=totalTime)
				{
					mIsStart = false;
					mIsEnd = false;
					mCurrentTime = 0;
				}
			}
		}
	}

}
