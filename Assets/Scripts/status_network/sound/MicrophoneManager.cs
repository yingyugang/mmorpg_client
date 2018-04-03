using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class MicrophoneManager : SingleMonoBehaviour<MicrophoneManager>
	{

		const int frequency = 10000;
		const string device = "Built-in Microphone";
		AudioClip mClip;

		protected override void Awake ()
		{
			base.Awake ();
		}

		public void StartMicrophone(){
			mClip = Microphone.Start(device, false, 5, frequency);
		}

		public float[] EndMicrophone(){
			Microphone.End (device);
			int length = Mathf.Min (10000,mClip.samples * mClip.channels);
			float[] data = new float[length];
			mClip.GetData (data, 0);
			return data;
		}

	}
}