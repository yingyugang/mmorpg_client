using UnityEngine;
using System.Collections;
using DataCenter;
using BaseLib;

public class UpdateDrug :NewUpdateVillage 
{
	public override void init()
	{
		buildType = BUILD_TYPE.BUILD_SYNTHETIZE;
		itemSort = (int)(ITEM_SORT.drug);
		ex = "Drug";
	}
}
