using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSV;

namespace MMO
{
	public class BaseCSVStructure  {

		[CsvColumn (CanBeNull = true)]
		public int id{ get; set; }

	}
}
