using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class AssetbundleManager : SingleMonoBehaviour<AssetbundleManager>
	{
		public bool isStreaming;
		Dictionary<string,AssetBundle> mCachedAssetbundles;
		AssetBundleManifest mManifest;
		AssetBundle mManifestAB;

		protected override void Awake ()
		{
			if (AssetbundleManager.Instance == null)
				DontDestroyOnLoad (gameObject);
			base.Awake ();
			mCachedAssetbundles = new Dictionary<string, AssetBundle> ();
		}

		void LoadManifestAssetbundle(){
			string path = PathConstant.CLIENT_ASSETBUNDLES_PATH + "/" + SystemConstant.GetPlatformName();
			if (FileManager.Exists (path)) {
				mManifestAB = AssetBundle.LoadFromFile (path);
				if (mManifestAB != null) {
					mManifest = mManifestAB.LoadAsset<AssetBundleManifest> ("assetbundlemanifest");
				}
			}
		}

		AssetBundle LoadAssetbundle(string abName){
			AssetBundle ab;
			if(isStreaming)
				ab = AssetBundle.LoadFromFile (PathConstant.CLIENT_STREAMING_ASSETS_PATH + "/" + abName);
			else
				ab = AssetBundle.LoadFromFile (PathConstant.CLIENT_ASSETBUNDLES_PATH + "/" + abName);
			return ab;
		}

		//get the assetbundle with depend assetbundles.
		public AssetBundle GetAssetbundleFromLocal (string abName)
		{
			abName = (abName + "." + PathConstant.AB_VARIANT).ToLower ();
			if(mCachedAssetbundles.ContainsKey(abName))
				return mCachedAssetbundles[abName];
			if (mManifestAB == null)
				LoadManifestAssetbundle ();
			if (mManifest != null) {
				string[] dependABs = mManifest.GetAllDependencies (abName);
				for (int i = 0; i < dependABs.Length; i++) {
					if (!mCachedAssetbundles.ContainsKey (dependABs [i])) {
						AssetBundle ab = LoadAssetbundle(dependABs [i]);
						mCachedAssetbundles.Add (dependABs [i], ab);
					}
				}
			}
			Debug.Log (PathConstant.CLIENT_ASSETBUNDLES_PATH + "/" + abName);
			AssetBundle ab0 = LoadAssetbundle(abName);
			mCachedAssetbundles.Add (abName,ab0);
			return ab0;
		}

		public void UnloadAssetBundle(string abName,bool isForce){
			abName = (abName + "." + PathConstant.AB_VARIANT).ToLower ();
			if(mCachedAssetbundles.ContainsKey(abName)){
				Debug.Log ("abName:" + abName);
				AssetBundle ab = mCachedAssetbundles [abName];
				ab.Unload (isForce);
				mCachedAssetbundles.Remove (abName);
			}
		}

		public void ClearAssetBundles(){
			foreach(string key in mCachedAssetbundles.Keys){
				if(mCachedAssetbundles[key] != null){
					mCachedAssetbundles [key].Unload (true);
				}
			}
			mCachedAssetbundles.Clear ();
		}

		//TODO need to make sure there is not another ab that use those depends.
		public void UnloadAssetBundleWithDepend(string abName,bool isForce){
			Debug.Log ("this fantion is todo.");
		}

		public T GetAssetFromLocal<T>(string abName,string assetName) where T : Object{
			AssetBundle ab = GetAssetbundleFromLocal (abName);
			if (ab == null)
				return null;
			T t = ab.LoadAsset<T> (assetName);
			return t;
		}

	}
}