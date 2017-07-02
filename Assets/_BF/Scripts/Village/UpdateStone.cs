using UnityEngine;
using System.Collections;
using DataCenter;
using BaseLib;

public class UpdateStone :NewUpdateVillage
{
	public override void init()
	{
		buildType = BUILD_TYPE.BUILD_STONE;
		itemSort = (int)(ITEM_SORT.stone);
		ex = "Stone";
	}
}
