using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MMO
{
	public class AssetbundleManager : SingleMonoBehaviour<AssetbundleManager>
	{

		Dictionary<string,AssetBundle> mCachedAssetbundles;
		AssetBundleManifest mManifest;
		AssetBundle mManifestAB;
		public bool isStreaming;

		protected override void Awake ()
		{
			if (AssetbundleManager.Instance == null)
				DontDestroyOnLoad (gameObject);
			base.Awake ();
			mCachedAssetbundles = new Dictionary<string, AssetBundle> ();
		}

		void LoadManifestAssetbundle ()
		{
			string path = PathConstant.CLIENT_ASSETBUNDLES_PATH + "/" + SystemConstant.GetPlatformName ();
			if (FileManager.Exists (path)) {
				mManifestAB = AssetBundle.LoadFromFile (path);
				if (mManifestAB != null) {
					mManifest = mManifestAB.LoadAsset<AssetBundleManifest> ("assetbundlemanifest");
				}
			}
		}
		//get the assetbundle with depend assetbundles.
		public AssetBundle GetAssetbundleFromLocal (string abName)
		{
			abName = (abName + "." + PathConstant.AB_VARIANT).ToLower ();
			if (mCachedAssetbundles.ContainsKey (abName))
				return mCachedAssetbundles [abName];
			if (mManifestAB == null)
				LoadManifestAssetbundle ();
			if (mManifest != null) {
				string[] dependABs = mManifest.GetAllDependencies (abName);
				for (int i = 0; i < dependABs.Length; i++) {
					if (!mCachedAssetbundles.ContainsKey (dependABs [i])) {
						AssetBundle ab = LoadAssetBundle (dependABs [i]);// AssetBundle.LoadFromFile (PathConstant.CLIENT_ASSETBUNDLES_PATH + "/" + dependABs [i]);
						mCachedAssetbundles.Add (dependABs [i], ab);
					}
				}
			}
			Debug.Log (PathConstant.CLIENT_ASSETBUNDLES_PATH + "/" + abName);
			AssetBundle ab0 = LoadAssetBundle (abName);// AssetBundle.LoadFromFile (PathConstant.CLIENT_ASSETBUNDLES_PATH + "/" + abName);
			mCachedAssetbundles.Add (abName, ab0);
			return ab0;
		}

		AssetBundle LoadAssetBundle (string abName)
		{
			AssetBundle ab;
			if (!isStreaming) {
				ab = AssetBundle.LoadFromFile (PathConstant.CLIENT_ASSETBUNDLES_PATH + "/" + abName);
			} else {
				ab = AssetBundle.LoadFromFile (PathConstant.CLIENT_STREAMING_ASSETS_PATH + "/" + abName);
			}
			return ab;
		}

		public void UnloadAssetBundle (string abName, bool isForce)
		{
			abName = (abName + "." + PathConstant.AB_VARIANT).ToLower ();
			if (mCachedAssetbundles.ContainsKey (abName)) {
				Debug.Log ("abName:" + abName);
				AssetBundle ab = mCachedAssetbundles [abName];
				ab.Unload (isForce);
				mCachedAssetbundles.Remove (abName);
			}
		}

		public void ClearAssetBundles ()
		{
			foreach (string key in mCachedAssetbundles.Keys) {
				if (mCachedAssetbundles [key] != null) {
					mCachedAssetbundles [key].Unload (true);
				}
			}
			mCachedAssetbundles.Clear ();
		}
		//TODO need to make sure there is not another ab that use those depends.
		public void UnloadAssetBundleWithDepend (string abName, bool isForce)
		{
			Debug.Log ("this fantion is todo.");
		}

		public T GetAssetFromLocal<T> (string abName, string assetName) where T : Object
		{
			AssetBundle ab = GetAssetbundleFromLocal (abName);
			if (ab == null)
				return null;
			T t = ab.LoadAsset<T> (assetName);
			return t;
		}

		#region TODO Download Async.
		public Object GetAssetFromLoacalAsync (string abName, string assetName, UnityAction<Object> onComplete)
		{
			if (mLoadList == null) {
				mLoadList = new List<LoadQueueItem> ();
			}
			LoadQueueItem item = new LoadQueueItem (abName, assetName, onComplete);
			mLoadList.Add (item);
			return null;
		}

		List<LoadQueueItem> mLoadList;
		LoadQueueItem mCurrentLoadQueueItem;

		void Update ()
		{
			if (mCurrentLoadQueueItem == null) {
				if (mLoadList!=null && mLoadList.Count > 0) {
					mCurrentLoadQueueItem = mLoadList [0];
					mLoadList.RemoveAt (0);
				}
			}
		}

		public class LoadQueueItem
		{
			public string abName;
			public string assetName;
			public UnityAction<Object> onComplete;

			public LoadQueueItem (string abName, string assetName, UnityAction<Object> onComplete)
			{
				this.abName = abName;
				this.assetName = assetName;
				this.onComplete = onComplete;
			}
		}
		#endregion
	}
}