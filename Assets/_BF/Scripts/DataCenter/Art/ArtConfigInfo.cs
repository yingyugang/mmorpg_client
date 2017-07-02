using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BaseLib;
namespace DataCenter
{
	public class ArtConfig
	{
		public string heroId;
		public string heroName;
	}

	public class ArtConfigInfo : DataModule
	{
		public override void release()
		{
		}
		
		public override bool init()
		{
			return true;
		}
	
		static public List<ArtConfig> GetHeroList()
		{
			List<ArtConfig> acs = new List<ArtConfig>();
			ConfigTable table = ConfigMgr.getConfig(CONFIG_MODULE.ART_RESOURCE);
			if (table == null )
				return acs;
			for(int i = 0;i < table.rows.Length;i ++)
			{
				ArtConfig ac = new ArtConfig();
				ac.heroId = table.rows[i].getStringValue(ART_RESOURCE.ID);
				ac.heroName = table.rows[i].getStringValue(ART_RESOURCE.RESOURCE);
				acs.Add(ac);
			}
			return acs;
		}

	}
}
