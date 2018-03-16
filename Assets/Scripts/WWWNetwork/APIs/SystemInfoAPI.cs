using System;
using UnityEngine;

namespace WWWNetwork
{
	public class SystemInfoAPI:BaseAPI
	{
		public override void Send (UnityEngine.Events.UnityAction<UnityEngine.WWW> complete)
		{
			api = APIConstant.SYSTEM_INFO;
//			this.bytedata = System.Text.ASCIIEncoding.ASCII.GetBytes (JsonUtility.ToJson(data));
			base.Send (complete);
		}
	}
}

