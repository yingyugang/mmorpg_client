using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BaseLib;

namespace DataCenter
{
	public class TestCSV : DataModule {


		public override void release()
		{
		}
		
		public override bool init()
		{
			ConfigTable ct = ConfigMgr.getConfig (CONFIG_MODULE.TESTCSV);
			foreach(ConfigRow row in ct.rows)
			{
				Debug.Log(row.getStringValue(TESTCSV.NAME0));
			}
			return true;
		}
	}
}
