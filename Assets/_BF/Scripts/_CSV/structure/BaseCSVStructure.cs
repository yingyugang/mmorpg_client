using UnityEngine;
using System.Collections;
using CSV;

[System.Serializable]
public class BaseCSVStructure  {

	[CsvColumn (CanBeNull = true)]
	public int id{ get; set; }

}
