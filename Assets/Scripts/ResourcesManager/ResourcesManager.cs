using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MMO
{
	public class ResourcesManager : SingleMonoBehaviour<ResourcesManager>
	{

		protected override void Awake ()
		{
			base.Awake ();
		}

		#region Units(hero and solider)

//		public GameObject GetHeroObject (string resPath)
//		{
//			GameObject prefab = GetHeroPrefab (resPath);
//			Common.SetShaderForEditor(prefab);
//			GameObject go = Instantiate (prefab) as GameObject;
//			return go;
//		}
//
//		public GameObject GetHeroPrefab (string resPath)
//		{
//			string subPath = resPath.Substring (0, resPath.LastIndexOf ('/'));
//			string prefabName = resPath.Substring (resPath.LastIndexOf ('/') + 1);
//			string abName = subPath.Substring (subPath.LastIndexOf ('/') + 1);
//			abName = PathConstant.HERO_AB_FRONT + abName;
//			return AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (abName, prefabName);
//		}
//
//		public GameObject GetSoliderObject (string resPath)
//		{
//			GameObject prefab = GetSoliderPrefab (resPath);
//			#if UNITY_EDITOR
//			Renderer[] rrs = prefab.GetComponentsInChildren<Renderer> (true);
//			for (int i = 0; i < rrs.Length; i++) {
//				Renderer rr = rrs [i];
//				rr.sharedMaterial.shader = Shader.Find (rr.sharedMaterial.shader.name);
//			}
//			#endif
//			GameObject go = Instantiate (prefab) as GameObject;
//			return go;
//		}
//
//		public GameObject GetSoliderPrefab (string resPath)
//		{
//			string subPath = resPath.Substring (0, resPath.LastIndexOf ('/'));
//			string prefabName = resPath.Substring (resPath.LastIndexOf ('/') + 1);
//			string abName = subPath.Substring (subPath.LastIndexOf ('/') + 1);
//			abName = PathConstant.SOLDIER_AB_FRONT + abName;
//			return AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (abName, prefabName);
//		}

		#endregion

		#region Buildings

//		public GameObject GetCityTerrain ()
//		{
//			GameObject prefab = AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (ABConstant.TERRAIN_CITY, ABConstant.TERRAIN_CITY);
//			GameObject go = Instantiate (prefab) as GameObject;
//			Common.SetShaderForEditor(go);
//			return go;
//		}
//
//		public GameObject GetBattleTerrain ()
//		{
//			GameObject prefab = AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (ABConstant.TERRAIN_GOBI, ABConstant.TERRAIN_GOBI);
//			GameObject go = Instantiate (prefab) as GameObject;
//			Common.SetShaderForEditor(go);
//			return go;
//		}
//
//		public GameObject GetBuildingObejct (string resPath)
//		{
//			string buildingName = resPath.Substring (resPath.LastIndexOf ('/') + 1);
//			string subBuildingName = buildingName;
//			if (buildingName.IndexOf ('_') != -1)
//				subBuildingName = buildingName.Substring (0, buildingName.IndexOf ('_'));
//			string abName = PathConstant.BUILDING_AB_FRONT + subBuildingName.ToLower ();
//			GameObject prefab = AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (abName, buildingName);
//			GameObject go = Instantiate (prefab) as GameObject;
//			Common.SetShaderForEditor(go);
//			return go;
//		}

		#endregion

		#region Audios

//		public AudioClip GetAudioClipBGM (string bgm)
//		{
//			AudioClip clip = AssetbundleManager.GetInstance.GetAssetFromLocal<AudioClip> (ABConstant.SOUND_BGM, bgm);
//			return clip;
//		}

//		public AudioClip GetAudioClipSE (string se)
//		{
//			AudioClip clip = AssetbundleManager.GetInstance.GetAssetFromLocal<AudioClip> (ABConstant.SOUND_SE, se);
//			return clip;
//		}

		#endregion

		#region UI

//		public GameObject GetUIInterface (string prefabName)
//		{
//			GameObject go = AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (ABConstant.PREFAB_INTERFACE, prefabName);
//			return go;
//		}

		#endregion

		#region Battles

//		public GameObject GetBattleRoot ()
//		{
//			GameObject prefab = AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (ABConstant.BATTLE, "BattleRoot");
//			GameObject go = Instantiate (prefab) as GameObject;
//			return go;
//		}
//
//		public GameObject GetBattleUIRoot ()
//		{
//			GameObject prefab = AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (ABConstant.BATTLE, "BattleUIRoot");
//			GameObject go = Instantiate (prefab) as GameObject;
//			return go;
//		}
//
//		public GameObject GetBattleGlobalSkillEffect ()
//		{
//			GameObject prefab = AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (ABConstant.BATTLE, "GlobalSkillEffect");
//			GameObject go = Instantiate (prefab) as GameObject;
//			return go;
//		}

		#endregion

		#region Citys

//		public GameObject GetCityRoot ()
//		{
//			GameObject prefab = AssetbundleManager.GetInstance.GetAssetFromLocal<GameObject> (ABConstant.CITY, "CityRoot");
//			GameObject go = Instantiate (prefab) as GameObject;
//			return go;
//		}

		#endregion

//		public Sprite GetSprite (string path)
//		{
//			return null;
//		}

		//get character head icon by character id
		public Sprite GetCharacterIconById (int charaId)
		{
			return null;
		}

		public AudioClip GetAudioClip(string audioName){
			return Resources.Load<AudioClip> (audioName);
		}

		public GameObject GetTerrainObjects(string terrainName){
			string abName = ABConstant.TERRAIN_OBJECTS + terrainName;
			GameObject go = LoadAsset<GameObject> (abName,terrainName);
			AssetbundleManager.Instance.UnloadAssetBundle (abName,false);
			return go;
		}

		public void GetTerrain(string terrainName,out GameObject terrain,out GameObject terrainT4M){
			string abName = ABConstant.TERRAIN + terrainName;
			terrain  = LoadAsset<GameObject> (abName,terrainName);
			AssetbundleManager.Instance.UnloadAssetBundle (abName,false);
			terrainT4M = LoadAsset<GameObject> (abName,string.Format("{0}{1}",terrainName,"T4M"));
			AssetbundleManager.Instance.UnloadAssetBundle (abName,false);
		}

		public Sprite GetSkillIcon(int skillId){
			Sprite sprite = null;
			if(CSVManager.Instance.skillDic.ContainsKey(skillId)){
				string iconName = CSVManager.Instance.skillDic[skillId].skill_icon;
//				string path = "Images/SkillIcons/" + iconName;
				sprite = LoadAsset<Sprite> (ABConstant.IMAGE_SKILLICONS,iconName);
			}
			return sprite;
		}

		public GameObject GetUnit(string abName,string unitName){
			AssetbundleManager.Instance.GetAssetbundleFromLocal (ABConstant.CHARACTERS + ABConstant.CHARACTERS_SHARED);
			GameObject go = LoadAsset<GameObject> (abName,unitName);
			return go;
		}

		public GameObject GetUnitFromLocal(string unitName){
			AssetbundleManager.Instance.GetAssetbundleFromLocal (ABConstant.CHARACTERS + ABConstant.CHARACTERS_SHARED);
			GameObject go = Resources.Load<GameObject> ("Units/" + unitName);//   LoadAsset<GameObject> (abName,unitName);
			return go;
		}
		//TODO need to be config.
		public GameObject GetBulletHit(string prefabName){
			string abName = ABConstant.EFFECTS + ABConstant.FPSPACK;
			GameObject go = LoadAsset<GameObject> (abName,prefabName);
			return go;
		}

		Dictionary<int,GameObject> mCachedEffect;
		public GameObject GetEffect(int effectId){
			if (mCachedEffect == null) {
				mCachedEffect = new Dictionary<int, GameObject> ();
			}
			if (mCachedEffect.ContainsKey (effectId)) {
				return mCachedEffect[effectId];
			}
			MEffect mEffect = null;
			if (CSVManager.Instance.effectDic.ContainsKey (effectId)) {
				mEffect = CSVManager.Instance.effectDic [effectId];
			} else {
				mEffect = CSVManager.Instance.effectDic [BattleConst.DEFAULT_EFFECT_ID];
			}
			GameObject go = LoadAsset<GameObject> ( mEffect.assetbundle,mEffect.effect_name);
			mCachedEffect.Add (effectId,go);
			return go;
		}

		T LoadAsset<T>(string abName,string assetName) where T : UnityEngine.Object{
			return AssetbundleManager.Instance.GetAssetFromLocal<T> (abName.ToLower(),assetName);
		}

	}
}