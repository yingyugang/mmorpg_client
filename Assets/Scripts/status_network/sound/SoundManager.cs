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

		Dictionary<string,AudioClip> mVoiceDic;

		protected override void Awake ()
		{
			base.Awake ();
			mVoiceSource = gameObject.GetComponent<AudioSource> ();
			mVoiceDic = new Dictionary<string, AudioClip> ();
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

		public void PlayVoice(string voiceName,AudioSource audioSource){
			AudioClip audioClip;
			if (mVoiceDic.ContainsKey (voiceName)) {
				audioClip = mVoiceDic [voiceName];
			} else {
				audioClip = ResourcesManager.Instance.GetAudioClip (voiceName);
			}
			audioSource.clip = audioClip;
			audioSource.Play ();
		}

		AudioClip mEmptyClip;
		public void PlayEmpty(AudioSource audioSource){
			if (mEmptyClip != null && audioSource != null) {
				audioSource.clip = mEmptyClip;
				audioSource.Play ();
			}
		}

	}
}
