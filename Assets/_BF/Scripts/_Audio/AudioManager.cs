using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AudioManager : SingletonMonoBehaviour<AudioManager> {

	static private AudioObject[] audioObject;
	static public AudioManager audioManager;
//	static private float musicVolume=.75f;
	static private float sfxVolume=.75f;
	static private Transform cam;

	public AudioSource MusicSource;
	public AudioSource BattleCoinSource;
	public AudioSource BattleSoulSource;
	public AudioSource BattleHCSource;
	public AudioSource BattleBCSource;
	public AudioSource CommonSource;

	public string BaseClipPath = "";
	public AudioClip MusicMainClip ; 
	public AudioClip MusicTaskClip ;
	public AudioClip MusicBattleClip ;
	public AudioClip MusicVillageClip;

	public AudioClip BattleHCClip;
	public AudioClip BattleBCClip;
	public AudioClip BattleCoinClip;
	public AudioClip BattleSoulClip;

	public AudioClip BtnActionClip;
	public AudioClip BtnCancelClip;

	public AudioClip BattleSkillClip;
	public AudioClip BattleSkillActionClip;
	public AudioClip BattleWinClip;
	public AudioClip BattleFailClip;

	public AudioClip BattleDefenseClip;

	public AudioClip actionFailedSound;
	public AudioClip WhoopAudio;
	public AudioClip[] musicList;

	public AudioClip hitElement1;
	public AudioClip hitElement2;
	public AudioClip hitElement3;
	public AudioClip hitElement4;
	public AudioClip hitElement5;
	public AudioClip hitElement6;
	public AudioClip hitElement7;
	public AudioClip hitElement8;


	public bool playMusic=true;
	public bool shuffle=false;
//	private int currentTrackID=0;
//	private GameObject thisObj;



	public static AudioManager instance;
	public static AudioManager SingleTon()
	{
		return me;
	}

	void CheckAudios(List<AudioSource> audioList){
		for (int i = 0; i < audioList.Count; i++) {
			if (audioList[i] == null) {
				audioList.RemoveAt (i);
				i--;
			}
		}
	}

	void Update(){
		CheckAudios(audios);
		CheckAudios(freeAudios);
		CheckAudios(runAudios);
		for(int i=0;i<runAudios.Count;i++){
			if (!runAudios [i].isPlaying) {
				freeAudios.Add (runAudios [i]);
				runAudios.RemoveAt (i);
				i--;
			}
		}
	}

	protected override void Init()
	{
//		if(instance==null)
//		{
//			instance=this;
//			DontDestroyOnLoad(gameObject);
//		}else{
//			Destroy(gameObject);
//		}
//		thisObj=gameObject;
//		transform.position = Camera.main.transform.position;
		Debug.Log(gameObject);
		gameObject.AddComponent<AudioListener>();
		if(MusicSource==null)
		{
			GameObject go = new GameObject();
			go.name = "_MusicSource";
			go.transform.parent = transform;
			MusicSource = go.AddComponent<AudioSource>();
		}
		if(BattleCoinSource==null)
		{
			GameObject go = new GameObject();
			go.name = "_BattleCoinSource";
			go.transform.parent = transform;
			BattleCoinSource = go.AddComponent<AudioSource>();
		}
		if(BattleSoulSource==null)
		{
			GameObject go = new GameObject();
			go.name = "_BattleSoulSource";
			go.transform.parent = transform;
			BattleSoulSource = go.AddComponent<AudioSource>();
		}
		if(BattleBCSource==null)
		{
			GameObject go = new GameObject();
			go.name = "_BattleBCSource";
			go.transform.parent = transform;
			BattleBCSource = go.AddComponent<AudioSource>();
		}
		if(BattleHCSource==null)
		{
			GameObject go = new GameObject();
			go.name = "_BattleHCSource";
			go.transform.parent = transform;
			BattleHCSource = go.AddComponent<AudioSource>();
		}
		if(CommonSource==null)
		{
			GameObject go = new GameObject();
			go.name = "_CommonSource";
			go.transform.parent = transform;
			CommonSource = go.AddComponent<AudioSource>();
		}
		if(MusicMainClip==null)MusicMainClip = Resources.Load<AudioClip>(BaseClipPath + "bf002_mypage") as AudioClip;
		if(MusicTaskClip==null)MusicTaskClip = Resources.Load<AudioClip>(BaseClipPath + "bf005_worldmap") as AudioClip;
		if(MusicBattleClip==null)MusicBattleClip = Resources.Load<AudioClip>(BaseClipPath + "bf011_map_firecave") as AudioClip;
		if(MusicVillageClip==null)MusicVillageClip = Resources.Load<AudioClip>(BaseClipPath + "bf009_village") as AudioClip;
		
		if(BattleHCClip==null)BattleHCClip = Resources.Load<AudioClip>(BaseClipPath + "bf228_se_heart_drop") as AudioClip;
		if(BattleBCClip==null)BattleBCClip = Resources.Load<AudioClip>(BaseClipPath + "bf231_se_karma_drop") as AudioClip;
		if(BattleCoinClip==null)BattleCoinClip = Resources.Load<AudioClip>(BaseClipPath + "bf229_se_zell_drop") as AudioClip;
		if(BattleSoulClip==null)BattleSoulClip = Resources.Load<AudioClip>(BaseClipPath + "bf231_se_karma_drop") as AudioClip;

		if(BtnActionClip==null)BtnActionClip = Resources.Load<AudioClip>(BaseClipPath + "bf300_se_action") as AudioClip;
		if(BtnCancelClip==null)BtnCancelClip = Resources.Load<AudioClip>(BaseClipPath + "bf301_se_cancel") as AudioClip;

		if(BattleSkillClip==null)BattleSkillClip = Resources.Load<AudioClip>(BaseClipPath + "bf233_se_skill_action") as AudioClip;
		if(BattleSkillActionClip==null)BattleSkillActionClip = Resources.Load<AudioClip>(BaseClipPath + "bf311_se_battle_attack") as AudioClip;
		if(BattleWinClip==null)BattleWinClip = Resources.Load<AudioClip>(BaseClipPath + "bf120_battle_win") as AudioClip;

		if(BattleDefenseClip==null)BattleDefenseClip = Resources.Load<AudioClip>(BaseClipPath + "Audios/" + "EtherealHeavyHit3") as AudioClip;

		hitElement1 = Resources.Load<AudioClip>(BaseClipPath + "bf209_se_water1") as AudioClip;
		hitElement2 = Resources.Load<AudioClip>(BaseClipPath + "bf210_se_water2") as AudioClip;
		hitElement3 = Resources.Load<AudioClip>(BaseClipPath + "bf218_se_saint1") as AudioClip;
		hitElement4 = Resources.Load<AudioClip>(BaseClipPath + "bf218_se_saint2") as AudioClip;
		hitElement5 = Resources.Load<AudioClip>(BaseClipPath + "bf225_se_buff") as AudioClip;
		hitElement6 = Resources.Load<AudioClip>(BaseClipPath + "bf300_se_action") as AudioClip;
		hitElement7 = Resources.Load<AudioClip>(BaseClipPath + "bf311_se_battle_attack") as AudioClip;
		hitElement8 = Resources.Load<AudioClip>(BaseClipPath + "bf228_se_heart_drop") as AudioClip;


		//		MusicSource.loop = 
		//		cam=Camera.main.transform;
		//		if(playMusic && musicList!=null && musicList.Length>0){
		//			GameObject musicObj=new GameObject();
		//			musicObj.name="MusicSource";
		//			musicObj.transform.position=cam.position;
		//			musicObj.transform.parent=cam;
		//			musicSource=musicObj.AddComponent<AudioSource>();
		//			musicSource.loop=false;
		//			musicSource.playOnAwake=false;
		//			musicSource.volume=musicVolume;
		//			musicSource.ignoreListenerVolume=true;
		//			StartCoroutine(MusicRoutine());
		//		}
		//		audioObject=new AudioObject[20];
		//		for(int i=0; i<audioObject.Length; i++){
		//			GameObject obj=new GameObject();
		//			obj.name="AudioSource";
		//			AudioSource src=obj.AddComponent<AudioSource>();
		//			src.playOnAwake=false;
		//			src.loop=false;
		//			src.minDistance=minFallOffRange;
		//			Transform t=obj.transform;
		//			t.parent=thisObj.transform;
		//			audioObject[i]=new AudioObject(src, t);
		//		}
		AudioListener.volume=sfxVolume;
		if(audioManager==null) audioManager=this;

		audios = new List<AudioSource> ();
		freeAudios = new List<AudioSource> ();
		runAudios = new List<AudioSource> ();
		for (int i = 0; i < 10; i++) {
			AddNewAudio ();
		}
	}

//
//
//	void Awake(){
//
//	}

//	void OnEnable()
//	{
//		PlayMusic(MusicMainClip);
//	}

	public void PlayDefenseClip()
	{
		CommonSource.clip = BattleDefenseClip;
		CommonSource.loop = false;
		CommonSource.Play();
	}

	public void PlayMusic(AudioClip clip)
	{
		MusicSource.volume = 0.2f;
		MusicSource.clip = clip;
		MusicSource.loop = true;
		MusicSource.Play();
	}

	public void PlayCoinClip()
	{
		if (!IsPlayable ("Coin"))
			return;
		this.Play (BattleCoinClip);
	}

	public void PlayHCClip()
	{
		if (!IsPlayable ("HC"))
			return;
		this.Play (BattleHCClip);
	}

	public void PlayBCClip()
	{
		if (!IsPlayable ("BC"))
			return;
		this.Play (BattleBCClip);
	}

	public void PlaySoulClip()
	{
		if (!IsPlayable ("Soul"))
			return;
		this.Play (BattleSoulClip);
	}

	public void PlayHitClip(AudioClip clip){
		if (!IsPlayable ("Hit"))
			return;
		this.Play (clip);
	}

	public void PlayBtnActionClip()
	{
		CommonSource.clip = BtnActionClip;
		CommonSource.loop = false;
		CommonSource.Play();
	}

	public void PlayBtnCancelClip()
	{
		CommonSource.clip = BtnCancelClip;
		CommonSource.loop = false;
		CommonSource.Play();
	}

	public void PlayBattleSkillClip()
	{
		CommonSource.clip = BattleSkillClip;
		CommonSource.loop = false;
		CommonSource.Play();
	}

	public void PlayBattleSkillActionClip()
	{
		CommonSource.clip = BattleSkillActionClip;
		CommonSource.loop = false;
		CommonSource.Play();
	}

	public void PlayBattleWinClip()
	{
		CommonSource.clip = BattleWinClip;
		CommonSource.loop = false;
		CommonSource.Play();
	}

	List<AudioSource> audios;
	List<AudioSource> freeAudios;
	List<AudioSource> runAudios;

	void AddNewAudio ()
	{
		GameObject go = new GameObject ("audio");
		AudioSource audio = go.AddComponent<AudioSource> ();
		audios.Add (audio);
		freeAudios.Add (audio);
	}

	public AudioSource Play (AudioClip clip)
	{
		if (freeAudios.Count == 0) {
			AddNewAudio ();
		}
		AudioSource audio = freeAudios [0];
		runAudios.Add (audio);
		audio.clip = clip;
		audio.Play ();
		freeAudios.RemoveAt (0);
		return audio;
	}


	public float commonSpatialBlend = 1;
	public int commonMaxDistance = 50;
	Dictionary<string,AudioClipInterval> mSoundsDic;
	//防止声音叠加
	Dictionary<string,float> mPlayingSounds;
	float mSoundInterval = 0.1f;
	public bool IsPlayable(string clipName)
	{
		if (mSoundsDic == null)
			mSoundsDic = new Dictionary<string, AudioClipInterval> ();
		if (mPlayingSounds == null)
			mPlayingSounds = new Dictionary<string, float> ();
		float soundInterval = this.mSoundInterval;
		if (mSoundsDic.ContainsKey(clipName)) {
			soundInterval = mSoundsDic [clipName].minInterval;		
		}
		if (mPlayingSounds.ContainsKey (clipName)) {
			if (mPlayingSounds [clipName] < Time.time) {
				mPlayingSounds [clipName] = Time.time + soundInterval;
				return true;
			} else {
				return false;
			}
		} else {
			mPlayingSounds.Add(clipName,Time.time + soundInterval);
			return true;
		}
	}

	public AudioClipInterval GetSound(string name){
		if(!mSoundsDic.ContainsKey(name)){
			return null;
		}
		return this.mSoundsDic [name];
	}


//	public void PlayWhoopAudio()
//	{
//		musicList[0] = WhoopAudio;
//		StartCoroutine(ToggleMusicRoutine());
//	}
//	
//	IEnumerator ToggleMusicRoutine()
//	{
//		float curTime = 0;
//		musicSource.volume = 0.9f;
//		while(curTime<2)
//		{
//			//			Debug.Log(musicSource.volume);
//			musicSource.volume -= Time.deltaTime / 2;
//			curTime += Time.deltaTime / 2;
//			yield return null;
//		}
//		StopCoroutine("MusicRoutine");
//		StartCoroutine(MusicRoutine());
//		curTime = 0;
//		musicSource.volume = 0;
//		while(curTime<2)
//		{
//			musicSource.volume += Time.deltaTime / 2;
//			musicSource.volume = Mathf.Min(0.1f,musicSource.volume);
//			curTime += Time.deltaTime / 2;
//			yield return null;
//		}
//	}
//
//	static public void Init(){
//		if(audioManager==null){
//			GameObject objParent=new GameObject();
//			objParent.name="AudioManager";
//			audioManager=objParent.AddComponent<AudioManager>();
//		}		
//	}
//
//	public IEnumerator MusicRoutine(){
//		while(true){
//			if(shuffle) musicSource.clip=musicList[Random.Range(0, musicList.Length)];
//			else{
//				musicSource.clip=musicList[currentTrackID];
//				currentTrackID+=1;
//				if(currentTrackID==musicList.Length) currentTrackID=0;
//			}
//
//			musicSource.Play();
//			
//			yield return new WaitForSeconds(musicSource.clip.length-0.05f);
//		}
//	}
//	
//	//check for the next free, unused audioObject
//	static private int GetUnusedAudioObject(){
//		for(int i=0; i<audioObject.Length; i++){
//			if(!audioObject[i].inUse){
//				return i;
//			}
//		}
//		
//		//if everything is used up, use item number zero
//		return 0;
//	}
//	
//	//this is a 3D sound that has to be played at a particular position following a particular event
//	static public void PlaySound(AudioClip clip, Vector3 pos){
//		if(audioManager==null) Init();
//		
//		int ID=GetUnusedAudioObject();
//		
//		audioObject[ID].inUse=true;
//		
//		audioObject[ID].thisT.position=pos;
//		audioObject[ID].source.clip=clip;
//		audioObject[ID].source.Play();
//
//		float duration=audioObject[ID].source.clip.length;
//		audioManager.StartCoroutine(audioManager.ClearAudioObject(ID, duration));
//	}
//	
//	//this no position has been given, assume this is a 2D sound
//	static public void PlaySound(AudioClip clip){
//		if(audioManager==null) Init();
//		
//		int ID=GetUnusedAudioObject();
//		
//		audioObject[ID].inUse=true;
//		
//		audioObject[ID].source.clip=clip;
//		audioObject[ID].source.Play();
//		
//		float duration=audioObject[ID].source.clip.length;
//		
//		audioManager.StartCoroutine(audioManager.ClearAudioObject(ID, duration));
//	}
//	
//	//a sound routine for 2D sound, make sure they follow the listener, which is assumed to be the main camera
//	static IEnumerator SoundRoutine2D(int ID, float duration){
//		while(duration>0){
//			audioObject[ID].thisT.position=cam.position;
//			yield return null;
//		}
//		
//		//finish playing, clear the audioObject
//		audioManager.StartCoroutine(audioManager.ClearAudioObject(ID, 0));
//	}
//	
//	//function call to clear flag of an audioObject, indicate it's is free to be used again
//	private IEnumerator ClearAudioObject(int ID, float duration){
//		yield return new WaitForSeconds(duration);
//		
//		audioObject[ID].inUse=false;
//	}
	
//	static public void SetSFXVolume(float val){
//		sfxVolume=val;
//		AudioListener.volume=val;
//	}
//	
//	static public void SetMusicVolume(float val){
//		musicVolume=val;
//		if(audioManager && audioManager.musicSource){
//			audioManager.musicSource.volume=val;
//		}
//	}
//	
//	public static float GetMusicVolume(){ return musicVolume; }
//	public static float GetSFXVolume(){ return sfxVolume; }
}


[System.Serializable]
public class AudioObject{
	public AudioSource source;
	public bool inUse=false;
	public Transform thisT;
	
	public AudioObject(AudioSource src, Transform t){
		source=src;
		thisT=t;
	}
}

public class AudioClipInterval
{
	public AudioClip clip;
	public float minInterval;
}