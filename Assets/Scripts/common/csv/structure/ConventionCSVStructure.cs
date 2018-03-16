using CSV;
[System.Serializable]
public class ConventionCSVStructure
{
	[CsvColumn (CanBeNull = true)]
	public int id{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string title{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string description{ get; set; }
}
