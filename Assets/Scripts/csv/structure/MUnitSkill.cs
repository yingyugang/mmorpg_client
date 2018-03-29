﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSV;

namespace MMO
{
	[System.Serializable]
	public class MUnitSkill : BaseCSVStructure
	{
		[CsvColumn (CanBeNull = true)]
		public int unit_id{ get; set; }
		[CsvColumn (CanBeNull = true)]
		public int unlock_level{ get; set; }
		[CsvColumn (CanBeNull = true)]
		//この三つアニメ属性が前端に役に立つ
		public string anim_name{ get; set; }
		[CsvColumn (CanBeNull = true)]
		public float anim_length{ get; set; }
		[CsvColumn (CanBeNull = true)]
		public float anim_action_point{ get; set; }
		[CsvColumn (CanBeNull = true)]
		public int skill_id{ 
			get{
				if(skill!=null){
					return skill.id;
				}
				return -1;
			}
			set{
				if(CSVManager.Instance.skillDic.ContainsKey(value)){
					skill = CSVManager.Instance.skillDic[value];
				}
			}
		}
		public MSkill skill;
	}
}
