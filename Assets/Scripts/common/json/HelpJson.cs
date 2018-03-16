using CSV;
using System.Collections.Generic;

[System.Serializable]
public class HelpJson
{
	public List<HelpData> data;

	[System.Serializable]
	public class HelpData : BaseData{
		[CsvColumn (CanBeNull = true)]
		public string menu_name;
		[CsvColumn (CanBeNull = true)]
		public string description;
	}
}

