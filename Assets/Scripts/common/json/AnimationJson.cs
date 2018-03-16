using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AnimationJson {
	public AnimationData[]  data;

	[System.Serializable]
	public class AnimationData:BaseData{
		public int target_type;
		public int target_id;
		public int animation_id;
		public string sub_animation_id;
		public int loop_mode;
		public int loop_count;
		public string name;
		public string tips;
		public int rarity;
		public string description;
		public int action_type;
		public int get_at;
		public string assetbundle;

		public List<AnimationDetailJson.AnimationDetailData> details;

		public List<List<AnimationDetailJson.AnimationDetailData>> orderedDetails{
			get{ 
				if (_orderedDetails == null) {
					details.Sort ();
					_orderedDetails = new List<List<AnimationDetailJson.AnimationDetailData>> ();
					Dictionary<int,List<AnimationDetailJson.AnimationDetailData>> detailsDic = new Dictionary<int, List<AnimationDetailJson.AnimationDetailData>> ();
					foreach(AnimationDetailJson.AnimationDetailData data in details){
						if(!detailsDic.ContainsKey(data.order_id)){
							detailsDic.Add (data.order_id,new List<AnimationDetailJson.AnimationDetailData>());
						}
						detailsDic [data.order_id].Add (data);
					}
					_orderedDetails.AddRange (detailsDic.Values);
				}
				return _orderedDetails;
			}
			set{ 
				_orderedDetails = value;
			}
		}

		List<List<AnimationDetailJson.AnimationDetailData>> _orderedDetails;
	}
}

