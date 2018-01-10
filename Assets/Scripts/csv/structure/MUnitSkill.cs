using System.Collections;
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
