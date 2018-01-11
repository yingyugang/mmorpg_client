using System.Collections.Generic;
using UnityEngine;
using CSV;

namespace MMO
{
	[System.Serializable]
	public class MUnit:BaseCSVStructure
	{
		[CsvColumn ()]
		public string name;
		[CsvColumn ()]
		public int m_world_id;
		[CsvColumn ()]
		public string elements;
		[CsvColumn ()]
		public int m_unit_rarity_id;
		[CsvColumn ()]
		public string resource_name;
		[CsvColumn ()]
		public string ui_resource_name;
		[CsvColumn ()]
		public int m_master_id;
		[CsvColumn ()]
		public int m_skill_default;
		[CsvColumn ()]
		public int m_skill_id_1;
		[CsvColumn ()]
		public int m_skill_id_2;
		[CsvColumn ()]
		public int m_skill_id_3;
		[CsvColumn ()]
		public int m_skill_id_4;
		[CsvColumn ()]
		public int min_hp;
		[CsvColumn ()]
		public int max_hp;
		[CsvColumn ()]
		public int min_atk;
		[CsvColumn ()]
		public int max_atk;
		[CsvColumn ()]
		public int min_def;
		[CsvColumn ()]
		public int max_def;
		[CsvColumn ()]
		public int min_mat;
		[CsvColumn ()]
		public int max_mat;
		[CsvColumn ()]
		public int min_mdf;
		[CsvColumn ()]
		public int max_mdf;
		[CsvColumn ()]
		public int min_hit;
		[CsvColumn ()]
		public int max_hit;
		[CsvColumn ()]
		public int min_spd;
		[CsvColumn ()]
		public int max_spd;
		[CsvColumn ()]
		public int min_tec;
		[CsvColumn ()]
		public int max_tec;
		[CsvColumn ()]
		public int min_luk;
		[CsvColumn ()]
		public int max_luk;
		[CsvColumn ()]
		public int m_weapon_id;
		[CsvColumn ()]
		public string adv;
		[CsvColumn ()]//1.好戦型 2.厌戦型
		public int disposition;
		[CsvColumn ()]
		public int is_active;
		[CsvColumn ()]
		public float width;

		public List<int> Elements {
			get {
				if (elementList == null) {
					string[] eles = elements.Split ('|'); 
					elementList = new List<int> ();
					int elementId = 0;
					for (int i = 0; i < eles.Length; i++) {
						elementId = int.Parse (eles [i]);
						if (elementId > 0 && elementId < 9)
							elementList.Add (elementId);
						else
							Debug.LogError ("id:" + id + "=> elementId:" + elementId + " is not exiting!");
					}
				}
				return elementList;
			}
		}

		private List<int> elementList;

		public List<MUnitSkill> unitSkillList;

		public int[] skillIds;

		public List<int> skillIdList;

	}
}

