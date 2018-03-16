using System;
using UnityEngine;

namespace WWWNetwork
{
	public class SigninAPI:BaseAPI
	{
		public override void Send (UnityEngine.Events.UnityAction<UnityEngine.WWW> complete)
		{
			api = APIConstant.SIGNIN;
			base.Send (complete);
		}
	}
}

