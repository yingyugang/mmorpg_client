using System;
using UnityEngine.Events;
using UnityEngine;

namespace WWWNetwork
{
	public class BaseAPI:MonoBehaviour
	{
		public string api;
		public byte[] bytedata;

		public virtual void Send (UnityAction<WWW> complete)
		{
			WWWNetworkManager.Instance.Send (api, bytedata, complete);
		}

		public virtual void Send (UnityAction complete)
		{
			WWWNetworkManager.Instance.Send (api, bytedata, complete);
		}
	}
}

