using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public BattlePerspective battlePerspective = BattlePerspective.StayBehind;
	public CameraFollow cameraFollow;
	public MobileSimpleRpgCamera mobileSimpleRpgCamera;
	public FixedCamera fixedCamera;
	public Animation cameraAnimation;

	static CameraController instance;
	public static CameraController SingleTon()
	{
		return instance;
	}

	void Awake()
	{
		if (instance == null) {
			instance = this;		
		}
		if(cameraFollow==null)
		{
			cameraFollow = Camera.main.GetComponent<CameraFollow> ();
		}
		if(mobileSimpleRpgCamera==null)
		{
			mobileSimpleRpgCamera = Camera.main.GetComponent<MobileSimpleRpgCamera> ();
		}
		if (fixedCamera == null) 
		{
			fixedCamera = Camera.main.GetComponent<FixedCamera> ();
		}
		if(cameraAnimation == null)
		{
			cameraAnimation = Camera.main.GetComponent<Animation> ();
			if(cameraAnimation == null)
			{
				cameraAnimation = Camera.main.gameObject.AddComponent<Animation> ();
			}
		}
		#if UNITY_STANDALONE_WIN
		Texture2D cursorTex = TextureUtility.CreateTexture2D (6,6,Color.green);
		Cursor.SetCursor (cursorTex,new Vector2(3,3),CursorMode.Auto);
		#endif
	}
	
#region toggle battlePerspective type

	public void PlayBossComingAnimation(AnimationClip clip,Transform t,string stateName)
	{
		if(clip!=null)
		{
			if(cameraFollow)cameraFollow.enabled = false;
			if(mobileSimpleRpgCamera)mobileSimpleRpgCamera.enabled = false;
			if(fixedCamera)fixedCamera.enabled = false;
			GameObject go = new GameObject();
			Camera.main.transform.parent = go.transform;
			go.transform.position = t.position;
			go.transform.rotation = t.rotation;
			StartCoroutine(_PlayBossComingAnimation(clip,t,stateName));
		}
	}

	IEnumerator _PlayBossComingAnimation(AnimationClip clip,Transform t,string stateName)
	{
		cameraAnimation.AddClip(clip,clip.name);
		cameraAnimation.clip = clip;
		cameraAnimation.Play();
		if(stateName!=null)t.GetComponent<Animator>().Play(stateName,0);
		yield return new WaitForSeconds (clip.length);
		Camera.main.transform.parent = null;
		if(cameraFollow)cameraFollow.enabled = true;
	}

	public void PlayDeathAnimation(AnimationClip[] clips,float[] startNormalizeTimes,Transform t,string stateName){
		if(clips!=null)
		{
			if(cameraFollow)cameraFollow.enabled = false;
			if(mobileSimpleRpgCamera)mobileSimpleRpgCamera.enabled = false;
			if(fixedCamera)fixedCamera.enabled = false;
			Camera.main.transform.position = t.position;
			Camera.main.transform.parent = t;
			StartCoroutine(_PlayDeathAnimation(clips,startNormalizeTimes,t.GetComponent<Animator>(),stateName));
		}
	}

	IEnumerator _PlayDeathAnimation(AnimationClip[] clips,float[] startNormalizeTimes, Animator anim,string stateName)
	{
		for(int i=0;i < clips.Length;i++)
		{
			Debug.Log(clips[i].name);
			cameraAnimation.AddClip(clips[i],clips[i].name);
			cameraAnimation.clip = clips[i];
			cameraAnimation.Play();
			anim.Play(stateName,0,startNormalizeTimes[i]);
			yield return new WaitForSeconds(clips[i].length);
		}
		yield return null;
	}

	public void TogglePerspective()
	{
		if(battlePerspective == BattlePerspective.StayBehind)
		{
			TogglePerspective(BattlePerspective.AroundBoss);
		}
		else if(battlePerspective == BattlePerspective.AroundBoss)
		{
			TogglePerspective(BattlePerspective.StayBehind);
		}
	}
	
	public void TogglePerspective(BattlePerspective bp)
	{
		if(bp == BattlePerspective.StayBehind)
		{
			cameraFollow.enabled = false;
			mobileSimpleRpgCamera.enabled = true;

		}
		else if(bp == BattlePerspective.AroundBoss)
		{
			cameraFollow.enabled = true;
			mobileSimpleRpgCamera.enabled = false;
		}
		battlePerspective =  bp;
	}
#endregion

}
