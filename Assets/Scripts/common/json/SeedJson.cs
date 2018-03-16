using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SeedJson {
	public List<SeedData>  data;

	[System.Serializable]
	public class SeedData:BaseSeedSeedingFlowerData{

	}
}

[System.Serializable]
public class BaseSeedSeedingFlowerData:BaseData{
	public string name;
	public int size;
	public int weight;
	public string character;
	public int best_temperature;
	public int best_water;
	public int sale_value;
	public string likes;
	public string dislikes;
	public string cv;
//	public int color_type;
	public int num;
	public int get_at;
	public string assetbundle;
	public string voice_assetbundle;
	public string static_assetbundle;
	public string cutin_assetbundle;
	//voice图鉴是否完成
	public bool voice_complete {
		get {
			if (voiceDic == null || voiceDic.Count==0) {
				return true;
			} else if (ownVoiceDic == null) {
				return false;
			}
			return voiceDic.Count <= ownVoiceDic.Count;
		}
	}
	//ill图鉴是否完成
	public bool ill_complete {
		get {
			if (illustrationDic == null || illustrationDic.Count==0) {
				return true;
			} else if (ownIllustrationDic == null) {
				return false;
			}
			return illustrationDic.Count <= ownIllustrationDic.Count;
		}
	}
	//animation图鉴是否完成
	public bool animation_complete {
		get {
			if (animationDic == null || animationDic.Count == 0) {
				return true;
			} else if (ownAnimationDic == null) {
				return false;
			}
			return animationDic.Count <= ownAnimationDic.Count;
		}
	}
	//all图鉴是否完成
	public bool complete {
		get {
			return voice_complete && ill_complete && animation_complete;
		}
	}

	public Dictionary<int,VoiceJson.VoiceData> voiceDic;
	public Dictionary<int,VoiceJson.VoiceData> ownVoiceDic;

	public Dictionary<int,IllustrationJson.IllustrationData> illustrationDic;
	public Dictionary<int,IllustrationJson.IllustrationData> ownIllustrationDic;

	public Dictionary<int,AnimationJson.AnimationData> animationDic;
	public Dictionary<int,AnimationJson.AnimationData> ownAnimationDic;


	public void AddOwnVoiceData(VoiceJson.VoiceData data){
		if(ownVoiceDic==null){
			ownVoiceDic = new Dictionary<int, VoiceJson.VoiceData> ();
		}
		if(!ownVoiceDic.ContainsKey(data.id)){
			ownVoiceDic.Add (data.id,data);
		}
	}

	public void AddVoiceData(VoiceJson.VoiceData data){
		if(voiceDic==null){
			voiceDic = new Dictionary<int, VoiceJson.VoiceData> ();
		}
		if(!voiceDic.ContainsKey(data.id)){
			voiceDic.Add (data.id,data);
		}
	}

	public void AddOwnIllustrationData(IllustrationJson.IllustrationData data){
		if(ownIllustrationDic==null){
			ownIllustrationDic = new Dictionary<int, IllustrationJson.IllustrationData> ();
		}
		if(!ownIllustrationDic.ContainsKey(data.id)){
			ownIllustrationDic.Add (data.id,data);
		}
	}

	public void AddIllustrationData(IllustrationJson.IllustrationData data){
		if(illustrationDic==null){
			illustrationDic = new Dictionary<int, IllustrationJson.IllustrationData> ();
		}
		if(!illustrationDic.ContainsKey(data.id)){
			illustrationDic.Add (data.id,data);
		}
	}


	public void AddOwnAnimationData(AnimationJson.AnimationData data){
		if(ownAnimationDic==null){
			ownAnimationDic = new Dictionary<int, AnimationJson.AnimationData> ();
		}
		if(!ownAnimationDic.ContainsKey(data.id)){
			ownAnimationDic.Add (data.id,data);
		}
	}

	public void AddAnimationData(AnimationJson.AnimationData data){
		if(animationDic==null){
			animationDic = new Dictionary<int, AnimationJson.AnimationData> ();
		}
		if(!animationDic.ContainsKey(data.id)){
			animationDic.Add (data.id,data);
		}
	}

	public void ClearOwn(){
		ownVoiceDic = null;
		ownIllustrationDic = null;
		ownAnimationDic = null;
	}

	public Dictionary<string,AnimationJson.AnimationData> namedAnimationDic;

	public AnimationJson.AnimationData GetAnimation(string name){
		if(namedAnimationDic==null){
			namedAnimationDic = new Dictionary<string, AnimationJson.AnimationData> ();
			if (animationDic != null) {
				foreach (AnimationJson.AnimationData data in animationDic.Values) {
					if (!namedAnimationDic.ContainsKey (data.name)) {
						namedAnimationDic.Add (data.name, data);
					}
				}
			}
		}
		AnimationJson.AnimationData anim;
		namedAnimationDic.TryGetValue (name,out anim);
		return anim;
	}

}






