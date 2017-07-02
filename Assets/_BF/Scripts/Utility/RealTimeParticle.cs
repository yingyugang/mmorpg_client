using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RealTimeParticle : MonoBehaviour {

	ParticleEmitter emitter;
	ParticleSystem particle;

	public bool Load = false;
	private double lastTime;

	private void Awake()
	{
		particle = GetComponent<ParticleSystem>();
		emitter = GetComponent<ParticleEmitter>();
	}
	
	// Use this for initialization
//	void Start ()
//	{
//		Time.timeScale = 0;
//		lastTime = Time.realtimeSinceStartup;
//	}
	
	// Update is called once per frame
	void Update () 
	{
//		float deltaTime = Time.realtimeSinceStartup - (float)lastTime;
		if(Time.timeScale == 0)
		{
			if(emitter!=null)emitter.Simulate(RealTime.deltaTime);
			if(particle!=null)particle.Simulate(RealTime.deltaTime, false, false); //last must be false!!
		}
#if UNITY_EDITOR
		if(Load)
		{
			ParticleSystem[] pss = GetComponentsInChildren<ParticleSystem>(true);
			ParticleEmitter[] pes = GetComponentsInChildren<ParticleEmitter>(true);
			foreach(ParticleSystem ps in pss)
			{
				ps.gameObject.GetOrAddComponent<RealTimeParticle>();
			}
			foreach(ParticleEmitter pe in pes)
			{
				pe.gameObject.GetOrAddComponent<RealTimeParticle>();
			}
			Load = false;
		}
#endif

//		lastTime = Time.realtimeSinceStartup;
	}

//	private ParticleSystem particle;
}
