using UnityEngine;
using System.Collections;

public class ParticleEmitDelay : MonoBehaviour {

	public float activeDelay = 0.1f;
	public ParticleEmitter mEmitter;
	public ParticleSystem mParticleSystem;


	void Awake()
	{
		mEmitter = GetComponent<ParticleEmitter>();
		mParticleSystem = GetComponent<ParticleSystem>();
	}

	void OnEnable()
	{
		StopAllCoroutines();
		StartCoroutine("Active");
	}

	void OnDisable()
	{
		StopAllCoroutines();
	}

	IEnumerator Active()
	{
		yield return new WaitForSeconds(activeDelay);
		if(mEmitter!=null)
		{
			mEmitter.emit = true;
		}
		if(mParticleSystem!=null)
		{
			mParticleSystem.Play();;
		}
	}

}
