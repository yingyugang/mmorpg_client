using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CSV;
using System.IO;
using System;
using System.Text;

namespace MMO
{
	/**
	 * 1つのキーで全部エッフェクトがcsvに入れされるスクリプトを作って。１０００個エッフェクトを追加したい場合は、UnityでプロフェブをAssetBundleResources/Effectsフォルダー中に入れて、
	 * アセットバンドル名前を設定して（自動で）、Tools/SetTheEffectsを選択して、プレフェブ名前とアセットバンドル名前がm_effect.csvにすぐ追加される。もしプレフェブ名前がもうcsvに保存
	 * された場合は、ロジックで判断して、追加されない。
	 * */
	public class GenerateEffectsCSV
	{

		const string EFFECT_DIRECT = "/Effects";
		const string keyPattern = "*****";

		[MenuItem ("Tools/Set the Effects")] 
		static void SetAssetbundleNames ()
		{
			SetEffects ();
			Debug.Log ("Effects Reset Done!");
		}

		static void SetEffects ()
		{
			CSVManager.LoadEffect ();
			HashSet<string> effectHash = new HashSet<string> ();
			int maxIndex = 0;
			for(int i=0;i<CSVManager.mEffectList.Count;i++){
				string key = GetEffectKey(CSVManager.mEffectList [i].effect_name,CSVManager.mEffectList [i].assetbundle);
				if (!effectHash.Contains (key)) {
					effectHash.Add (key);
				}
				maxIndex = Mathf.Max (maxIndex, CSVManager.mEffectList [i].id);
			}

			string effectPath = Application.dataPath + ABSetting.assetbundleRoot + EFFECT_DIRECT;
			string[] dirctors = FileManager.GetDirectories (effectPath, "*", SearchOption.TopDirectoryOnly);
			for (int i = 0; i < dirctors.Length; i++) {
				string abName = "effects/" + FileManager.GetDirectoryNameFromPath(dirctors[i]).ToLower();
				string[] files = FileManager.GetFiles (dirctors[i],"*", SearchOption.TopDirectoryOnly);
				for(int j=0;j<files.Length ;j++){
					string filePath = files [j];
					if(filePath.IndexOf(".DS_Store")!=-1 || filePath.IndexOf(".meta")!=-1 || filePath.IndexOf(".prefab")==-1){
						continue;
					}
					string fileName = FileManager.GetFileNameFromPath (filePath);
					string key = GetEffectKey(fileName,abName);
					if(!effectHash.Contains(key)){
						effectHash.Add (key);
						maxIndex++;
						MEffect mEffect = new MEffect ();
						mEffect.id = maxIndex;
						mEffect.effect_name = fileName;
						mEffect.assetbundle = abName;
						CSVManager.mEffectList.Add (mEffect);
					}
				}
			}
			StringBuilder sb = new StringBuilder ();
			sb.AppendLine (CSVManager.EFFECT_FILE_TITLE);
			for(int i=0;i<CSVManager.mEffectList.Count;i++){
				MEffect mEffect = CSVManager.mEffectList [i];
				string lineString = string.Format ("{0},{1},{2},{3}",mEffect.id,mEffect.effect_name,mEffect.assetbundle,mEffect.description);
				sb.AppendLine (lineString);
			}
			string effectCSVPath = CSVManager.GetCSVPath ("m_effect");
			File.WriteAllText (effectCSVPath,sb.ToString());
		}

		static string GetEffectKey(string effect_name,string abName){
			return effect_name + keyPattern + abName;
		}

	}
}
