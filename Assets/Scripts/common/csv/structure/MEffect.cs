using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSV;

namespace MMO
{
	[System.Serializable]
	public class MEffect : BaseCSVStructure
	{
		//技能名称
		[CsvColumn (CanBeNull = true)]
		public string effect_name{ get; set; }
		//技能描述
		[CsvColumn (CanBeNull = true)]
		public string assetbundle{ get; set; }
		//技能描述
		[CsvColumn (CanBeNull = true)]
		public string description{ get; set; }

	}
}
