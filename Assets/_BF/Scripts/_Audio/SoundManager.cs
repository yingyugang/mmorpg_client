using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager:SingleMonoBehaviour<SoundManager>
{

	List<AudioSource> audios;
	List<AudioSource> freeAudios;
	List<AudioSource> runAudios;
	AudioSource mBgmAudio;
	AudioSource mBgmAudio1;

	protected override void Awake ()
	{
		base.Awake ();
		Init ();
	}

	void Update(){
		for(int i=0;i<runAudios.Count;i++){
			if (!runAudios [i].isPlaying) {
				freeAudios.Add (runAudios [i]);
				runAudios.RemoveAt (i);
				i--;
			}
		}
	}

	void Init ()
	{
		audios = new List<AudioSource> ();
		freeAudios = new List<AudioSource> ();
		runAudios = new List<AudioSource> ();
		for (int i = 0; i < 10; i++) {
			AddNewAudio ();
		}
	}

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

	public void Stop ()
	{
		foreach (AudioSource audio in runAudios) {
			if (audio != null)
				audio.Stop ();
		}
	}
}

