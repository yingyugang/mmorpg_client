using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
[RequireComponent(typeof(AudioSource))]
public class HeroRes : MonoBehaviour {

	public Hero hero;
	public List<Transform> HitPoints;
	public List<Transform> ShootPoints;
	public List<AnimMapping> bodyAnims;
	public Dictionary<string,AnimMapping> bodyAnimMapping;
#if UNITY_EDITOR
	public List<GameObject> BodyPrefabs;
#endif
	public List<GameObject> bodyObjects;

	public List<AttackQueue> attackQueue;
	public List<AttackQueue> skillQueue;

	public Transform Shadow;
	public AnimMapping CurrentAm;
	public Vector3 CenterOffset;
	public Transform HeadTrans;

	void Awake()
	{
		if(Application.isPlaying)
		{
			bodyAnimMapping = new Dictionary<string, AnimMapping>();
			foreach(AnimMapping am in bodyAnims)
			{
				bodyAnimMapping.Add(am.clipName,am);
			}

			CenterOffset = new Vector3(0,3,0);
		}
	}

	public float gizmosRadius = 0.2f;
	void OnDrawGizmos()
	{
		foreach(Transform t in HitPoints)
		{
			if(t !=null)
			{
				Gizmos.DrawWireSphere(t.position,gizmosRadius);
			}
		}
		Gizmos.color = Color.red;
		foreach(Transform t in ShootPoints)
		{
			if(t != null)
			{
				Gizmos.DrawWireSphere(t.position,gizmosRadius);
			}
		}
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position + CenterOffset,gizmosRadius);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position,gizmosRadius);
	}
}

[System.Serializable]
public class AnimMapping
{
	public string clipName;
	public Animation anim;
	public AnimationClip clip;
	public GameObject body;
	public SkinnedMeshRenderer[] skinRs;
	public SpriteRenderer[] spriteRs;
	public List<AnimQueue> queue;
	public List<AnimationFrag> frags;
	public float globalSpeed = 1;
	public float realTime;
	public float offsetSpeed;
	#if UNITY_EDITOR
	public float sampleTime = 0;
	public float sampleTime1 = 0;
	public AnimationFrag sampleFrag;
	public int sampleType;//0 or 1
	#endif
}

[System.Serializable]
public class AnimQueue
{
	public float t;
	public float speed;
	public float realTimeSincePrevious;
	public bool backMesh;
	public bool showText;
}

[System.Serializable]
public class AnimationFrag
{
	public float startTime;
	public float stopTime;
	public float speed = 1;
	public int loopCount = 1;
	public bool rewind;
	public float rewindSpeed = 1;
	public AnimationCurve ac = AnimationCurve.Linear(0,0,1,1);
	public AnimationCurve acRewind = AnimationCurve.Linear(0,0,1,1);
}