using CSV;

public class VersionCSVStructure
{
	[CsvColumn (CanBeNull = true)]
	public string ID { get; set; }

	[CsvColumn (CanBeNull = true)]
	public string FileName{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int FileSize{ get; set; }
	
	[CsvColumn (CanBeNull = true)]
	public int IsAssetBundle{ get; set; }
	
	[CsvColumn (CanBeNull = true)]
	public int IsCSV{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string HashCode{ get; set; }
}
