using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingleMonoBehaviour<SoundManager> {

	AudioSource mMusicSource;
	AudioSource mMusicSource1;

	protected override void Awake ()
	{
		base.Awake ();
//		mMusicSource = gameObject.GetOrAddComponent<AudioSource> ();
//		mMusicSource.loop = true;
	}

	public void PlayBGM(string bgm){
//		AudioClip clip = ResourceManager..GetAudioClipBGM(bgm);
//		mMusicSource = gameObject.GetOrAddComponent<AudioSource> ();
//		mMusicSource.clip = clip;
//		mMusicSource.Play ();
	}

}
