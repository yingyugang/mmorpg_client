using System.Collections.Generic;
using UnityEngine;
using CSV;

namespace MMO
{
	[System.Serializable]
	public class MUnit:BaseCSVStructure
	{

		public string resource_name;

//		private List<int> elementList;

		public List<MUnitSkill> unitSkillList;

//		public int[] skillIds;

//		public List<int> skillIdList;
	}
}

