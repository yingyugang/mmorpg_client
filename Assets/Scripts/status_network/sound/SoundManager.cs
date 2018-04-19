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
			mShootClip = ResourcesManager.Instance.GetAudioClip (BattleConst.BattleSounds.SHOOT);
			mReloadClip = ResourcesManager.Instance.GetAudioClip (BattleConst.BattleSounds.RELOAD);
			mEmptyClip = ResourcesManager.Instance.GetAudioClip (BattleConst.BattleSounds.EMPTY);
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

		AudioClip mShootClip;
		public void PlayShoot(AudioSource audioSource){
			if (mShootClip != null){
				if (audioSource != null) {
					audioSource.PlayOneShot (mShootClip);
				}
			}
		}

		AudioClip mReloadClip;
		public void PlayReload(AudioSource audioSource){
			if (mReloadClip != null && audioSource != null) {
				audioSource.clip = mReloadClip;
				audioSource.Play ();
			}
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
