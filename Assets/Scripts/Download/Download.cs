using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MMO
{
	public class Download : MonoBehaviour
	{
		
		public Slider slider;
		public Image img_background;

		void Awake(){
			slider.value = 0;
		}

		void Update(){
			if(DownloadManager.Instance.isDownloading){
				if (!slider.gameObject.activeInHierarchy)
					slider.gameObject.SetActive (true);
				slider.value = (float)DownloadManager.Instance.totalDownloadedSize / DownloadManager.Instance.totalDownloadSize;
			}
		}

	}
}
