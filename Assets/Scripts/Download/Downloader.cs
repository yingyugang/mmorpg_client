using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MMO
{
	public class Downloader : MonoBehaviour
	{

		public void StartDownload (string assetBundleName, UnityAction onComplete)
		{
			StartCoroutine (_StartDownload (assetBundleName, onComplete));
		}

		private IEnumerator _StartDownload (string assetBundleName, UnityAction onComplete)
		{
			string URL = PathConstant.SERVER_ASSETBUNDLES_PATH + "/" + assetBundleName;
			Debug.Log (URL);
			var www = new WWW (URL);
			int size = 0;
			while (!www.isDone) {
				int deltaSize = www.bytesDownloaded - size;
				size = www.bytesDownloaded;
				DownloadManager.Instance.totalDownloadedSize += deltaSize;
				yield return null;
			}
			yield return null;
			if (www.isDone && string.IsNullOrEmpty (www.error)) {
				FileManager.WriteAllBytes (PathConstant.CLIENT_ASSETBUNDLES_PATH + "/" + assetBundleName, www.bytes);
				Debug.Log (PathConstant.CLIENT_ASSETBUNDLES_PATH + "/" + assetBundleName);
			} else {
				Debug.Log (assetBundleName);
				Debug.Log (www.error);
			}
			www.Dispose ();
			www = null;
			if (onComplete != null)
				onComplete ();
		}

	}
}
