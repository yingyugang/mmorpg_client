using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class AnimationDetailJson  {

	public AnimationDetailData[]  data;

	[System.Serializable]
	public class AnimationDetailData:BaseData,IComparable<AnimationDetailData>{
		public int order_id;
		public int sub_animation_id;
		public string body;
		public string eye;
		public string mouth;
		public string hand;
		public string other;
		public string foot;
		public string hair;
		public string loop;
		public string voice;
		public int loop_count;
		public float time;
		public int percentage;

		public int CompareTo(AnimationDetailData comparePart)
		{
			if (comparePart == null)
				return 1;
			else
				return this.order_id.CompareTo(comparePart.order_id);
		}
	
	}

}
