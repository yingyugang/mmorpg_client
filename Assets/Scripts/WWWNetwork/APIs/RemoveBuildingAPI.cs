using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WWWNetwork
{
	public class RemoveBuildingAPI : BaseAPI
	{
		public ChangeBuildingModel data;

		public override void Send (UnityEngine.Events.UnityAction<UnityEngine.WWW> complete)
		{
			Debug.Log (JsonUtility.ToJson (data));
			this.bytedata = System.Text.ASCIIEncoding.ASCII.GetBytes ("data=" + JsonUtility.ToJson (data));
			api = APIConstant.REMOVE_BUILDING;
			base.Send (complete);
		}
	}
}
