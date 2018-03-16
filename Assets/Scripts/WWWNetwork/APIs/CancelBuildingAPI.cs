using System;
using UnityEngine;

namespace WWWNetwork
{
	public class CancelBuildingAPI:BaseAPI
	{
		public ChangeBuildingModel data;
		public override void Send (UnityEngine.Events.UnityAction<UnityEngine.WWW> complete)
		{
			this.bytedata = System.Text.ASCIIEncoding.ASCII.GetBytes ("data=" + JsonUtility.ToJson(data));
			api = APIConstant.REMOVE_BUILDING;
			base.Send (complete);
		}
	}
}

