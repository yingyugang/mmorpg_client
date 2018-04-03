using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class SoundManager : SingleMonoBehaviour<SoundManager>
	{

		AudioSource mMusicSource;
		AudioSource mMusicSource1;
		AudioSource mVoiceSource;

		protected override void Awake ()
		{
			base.Awake ();
			mVoiceSource = gameObject.GetComponent<AudioSource> ();
//		mMusicSource = gameObject.GetOrAddComponent<AudioSource> ();
//		mMusicSource.loop = true;
		}

		public void PlayBGM (string bgm)
		{
//		AudioClip clip = ResourceManager..GetAudioClipBGM(bgm);
//		mMusicSource = gameObject.GetOrAddComponent<AudioSource> ();
//		mMusicSource.clip = clip;
//		mMusicSource.Play ();
		}

		//TODO
		public void PlayVoice(VoiceInfos voices){
			AudioClip clip = AudioClip.Create("aaa",10000,1,10000,false);
			if(voices.voices.Length > 0){
				clip.SetData (voices.voices[0].voice,0);
			}
			mVoiceSource.clip = clip;
			mVoiceSource.Play ();
		}

	}
}
