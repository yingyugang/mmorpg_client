using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSV;

namespace MMO
{
	[System.Serializable]
	public class MSkillShoot : BaseCSVStructure
	{

		[CsvColumn (CanBeNull = true)]
		public int shoot_type{ get; set; }
		[CsvColumn (CanBeNull = true)]
		public int shoot_object_id{ get; set; }
		[CsvColumn (CanBeNull = true)]
		public int shoot_move_speed{ get; set; }
		[CsvColumn (CanBeNull = true)]
		public int shoot_start_max_angle{ get; set; }
		[CsvColumn (CanBeNull = true)]
		public int is_const_angle{ get; set; }
		[CsvColumn (CanBeNull = true)]
		public int is_plus_player_angle{ get; set; }
		[CsvColumn (CanBeNull = true)]
		public string description{ get; set; }

	}
}
